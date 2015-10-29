using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using CersioIO;

/* Todo List
 * ・チェックポイント編集時　マップ上のポイント選択(EditMode MouseDown時)
 * ・対象レイヤー  ビュー移動
 * 
 * ◇今後のバージョンアップ
 * ・RE連携
 * 　開始時間　接続調整
 * 　ステップ数（時間差）調整
 * 　生成し終わったレイヤーに、一括で時間ごとに位置、回転をはめ込んで調整。
 * 　（生成しながらでは、合わせにくいので）
 * */


namespace LRFMapEditer
{
    public partial class MapEditForm : Form
    {
        public const int LRF_PixelSize = 2;     // 点の描画サイズ

        public static double LRF_Range = 30000.0;            // LRF最大距離 30m [mm]
        public static double LRF_ScaleOfPixel = 1.0 / 100.0; // 10cm = 1Pixel [mm]からピクセルサイズへの変換

        public Color colEditLayerPixel = Color.Cyan;   // エディット時ピクセルカラー
        public Color colLayerPixel = Color.Green;      // 通常時ピクセルカラー
        public Color colLayerBase = Color.Black;       // 抜き色

        // View
        float ViewTransX = 0.0f;
        float ViewTransY = 0.0f;
        float ViewScale = 1.0f;

        // ロータリーエンコーダ
        PointD[] reWheelR;
        PointD[] reWheelL;

        // LRF ログファイル名
        public string LRF_LogFileName;
        public string nowMapFilename;

        // 
        public List<LayerData> MapLyaer;
        private LayerData EditLayer;        // 編集中のレイヤー

        Bitmap WorldMap;        // ワールドマップ
        Bitmap EditMap;        // エディットマップ
        Bitmap LayerMap;        // 編集中以外のレイヤーBMPをまとめたもの

        Bitmap CheckPointMap;        // チェックポイントマップ
        Bitmap RenderedWorldMap;     // チェックポイント用ワールドマップ


        private int FileVersion = 0x00000100;   // ファイルバージョン


        enum TAB_PAGE : int
        {
            MAP_EDIT = 0,
            CHECKPOINT,
        };

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MapEditForm()
        {
            InitializeComponent();

            // ホイールイベント追加
            //this.pb_LRFLog.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pb_LRFLog_MouseWheel);
            this.pb_VMap.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pb_VMap_MouseWheel);
            this.pb_CPMap.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pb_CPMap_MouseWheel);


            // LRF init
            //LoadLRFLogFile("urgLog20151003_3.txt");

            ViewTransX = (pb_VMap.Width / 2);
            ViewTransY = (pb_VMap.Height / 2);

            // MapLayer init
            //MapLyaer = new List<LayerData>();
            WorldMap = new Bitmap(pb_VMap.Width, pb_VMap.Height);
            LayerMap = new Bitmap(pb_VMap.Width, pb_VMap.Height);
            pb_VMap.Image = WorldMap;

            // 
            EditMap = new Bitmap(pb_EditLayer.Width, pb_EditLayer.Height);
            pb_EditLayer.Image = EditMap;

            //
            CheckPointMap = new Bitmap(pb_CPMap.Width, pb_CPMap.Height);
            pb_CPMap.Image = CheckPointMap;
        }

        private void MapEditForm_Load(object sender, EventArgs e)
        {
            UpdateWorldMap();
            tmr_MapUpdate.Enabled = true;
        }

        private void MapEditForm_Shown(object sender, EventArgs e)
        {
            /*
            NewMapLoadForm newDlg = new NewMapLoadForm(this);
            newDlg.ShowDialog();

            EditLayer = null;
            SelectEditLayer(0);
            UpdateWorldMap();
             * */
        }



        /// <summary>
        /// ガイドライン
        /// </summary>
        /// <param name="g"></param>
        private void DrawGuideLine(Graphics g)
        {
            int GideLength = 5000;  // ガイドラインの長さ(ドット)
            g.ResetTransform();
            // View
            g.ScaleTransform(ViewScale, ViewScale, MatrixOrder.Append);
            g.TranslateTransform(ViewTransX, ViewTransY, MatrixOrder.Append);

            g.DrawLine(Pens.DarkGray, -GideLength, 0, GideLength, 0);
            g.DrawLine(Pens.DarkGray, 0, -GideLength, 0, GideLength);

            int gridPix = (int)(10000.0*LRF_ScaleOfPixel);
            int nonScl = (int)(5.0/ViewScale);
            for( int i=0; i<=(GideLength*2 /gridPix); i++ )
            {
                g.DrawLine(Pens.DarkGray, -GideLength + (gridPix * i), -nonScl, -GideLength + (gridPix * i), nonScl);
                g.DrawLine(Pens.DarkGray, -nonScl, -GideLength + (gridPix * i), nonScl, -GideLength + (gridPix * i));
            }
        }

        /// <summary>
        /// ロータリーエンコーダの軌跡を描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="xyWR"></param>
        /// <param name="xyWL"></param>
        private void DrawREconderData(Graphics g, PointD[] xyWR, PointD[] xyWL )
        {
            int drawNum = 100;  // 色変えの単位
            //int ctrX = WorldMap.Width / 2;
            //int ctrY = WorldMap.Height / 2;

            float reRot = (float)num_RERotate.Value;

            //多角形を描画する
            g.ResetTransform();
            g.ScaleTransform((float)LRF_ScaleOfPixel, (float)LRF_ScaleOfPixel);
            g.RotateTransform(reRot, MatrixOrder.Append);
            //g.TranslateTransform(ctrX, ctrY, MatrixOrder.Append);
            // View
            g.ScaleTransform(ViewScale, ViewScale, MatrixOrder.Append);
            g.TranslateTransform(ViewTransX, ViewTransY, MatrixOrder.Append);
            //g.ScaleTransform(0.01f, 0.01f);

            //Point[] wRinePos = new Point[1000];
            //Point[] wLinePos = new Point[1000];
            for (int i = 0; i < xyWR.Length / drawNum; i++)
            {
                int n = i * drawNum;
                int en = xyWR.Length - n;
                if (en > drawNum) en = drawNum;

                Point[] wRinePos = new Point[en];
                Point[] wLinePos = new Point[en];

                // 右車輪
                for (int iR = 0; iR < en; iR++)
                {
                    wRinePos[iR] = new Point((int)xyWR[iR + n].X, (int)xyWR[iR + n].Y);
                }
                // 左車輪
                for (int iL = 0; iL < en; iL++)
                {
                    wLinePos[iL] = new Point((int)xyWL[iL + n].X, (int)xyWL[iL + n].Y);
                }

                // 色変え
                if (i % 2 == 0)
                {
                    g.DrawLines(Pens.Purple, wRinePos);
                    g.DrawLines(Pens.DarkMagenta, wLinePos);
                }
                else
                {
                    g.DrawLines(Pens.Violet, wRinePos);
                    g.DrawLines(Pens.BlueViolet, wLinePos);
                }
            }
        }

        // ===================================================================================

        /// <summary>
        /// スケールバー変更
        /// </summary>
        /// <param name="vwScale"></param>
        private void SetViewScale(float vwScale)
        {
            if ((int)(vwScale * 100.0f) > tber_ViewScale.Maximum)
            {
                vwScale = (float)tber_ViewScale.Maximum / 100.0f;
            }
            if ((int)(ViewScale * 100.0f) < tber_ViewScale.Minimum)
            {
                vwScale = (float)tber_ViewScale.Minimum / 100.0f;
            }

            ViewScale = vwScale;
            tber_ViewScale.Value = (int)(vwScale * 100.0f);
        }



        // --------------------------------------------------------------------------------------
        // メニュー
        private void ts_LoadLogData_Click(object sender, EventArgs e)
        {
            // LRF入力ダイアログ
            NewMapLoadForm newDlg = new NewMapLoadForm(this);

            if (newDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            // 取り込んだレイヤー情報
            lb_NumLayer.Text = MapLyaer.Count.ToString();
            sb_VMapLayer.Maximum = MapLyaer.Count;
            num_Layer.Maximum = MapLyaer.Count;

            SelectEditLayer(0);
        }

        /// <summary>
        /// ロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ts_LoadMapData_Click(object sender, EventArgs e)
        {
            OpenFileDialog fDlg = new OpenFileDialog();

            fDlg.Filter = "LayerMapEditFile (*.lme)|*.lme";

            var Result = fDlg.ShowDialog();
            if (Result != System.Windows.Forms.DialogResult.OK) return;

            // ダイアログ消去待ち
            Application.DoEvents();

            // init MapData
            {
                MapLyaer = new List<LayerData>();
            }

            try
            {
                FileStream fstrm = new FileStream(fDlg.FileName, FileMode.Open);
                BinaryReader strm = new BinaryReader(fstrm);

                this.Read(strm);

                strm.Close();
                fstrm.Close();

                //MessageBox.Show(fDlg.FileName + " Load Finish", "Load OK");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Load Fail");
            }

            nowMapFilename = fDlg.FileName;
            lb_NumLayer.Text = MapLyaer.Count.ToString();
            sb_VMapLayer.Maximum = MapLyaer.Count;
            num_Layer.Maximum = MapLyaer.Count;

            SelectEditLayer(MapLyaer.Count-1);
            UpdateWorldMap();
        }

        /// <summary>
        /// セーブ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ts_SaveMapData_Click(object sender, EventArgs e)
        {
            SaveFileDialog fDlg = new SaveFileDialog();

            fDlg.Filter = "LayerMapEditFile (*.lme)|*.lme";

            var Result = fDlg.ShowDialog();
            if (Result != System.Windows.Forms.DialogResult.OK) return;

            // ダイアログ消去待ち
            Application.DoEvents();

            try
            {
                FileStream fstrm = new FileStream(fDlg.FileName, FileMode.Create );
                BinaryWriter strm = new BinaryWriter(fstrm);

                this.Write(strm);

                strm.Close();
                fstrm.Close();

                MessageBox.Show(fDlg.FileName+" Save Finish", "Save OK");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Save Fail");
            }

            nowMapFilename = fDlg.FileName;

        }

        /// <summary>
        /// マップをBMP保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ts_SaveMapImage_Click(object sender, EventArgs e)
        {
            SaveFileDialog fDlg = new SaveFileDialog();

            fDlg.Filter = "Image Files (*.png)|*.png|Image Files (*.bmp)|*.bmp|All Files(*.*)|*.*";

            var Result = fDlg.ShowDialog();
            if (Result != System.Windows.Forms.DialogResult.OK) return;

            try
            {
                SaveMapBmp(fDlg.FileName);
                //WorldMap.Save(fDlg.FileName);
            }
            catch (Exception exp )
            {
                MessageBox.Show(exp.Message, "Save Fail");
            }
        }

        // File IO
        /// <summary>
        /// バイナリデータ セーブ
        /// </summary>
        /// <param name="strm"></param>
        private void Write( BinaryWriter strm )
        {
            strm.Write(FileVersion);
            strm.Write(2);  // セクション数

            // LRF LogFilename
            strm.Write(LRF_LogFileName);

            // マップレイヤー セクション
            {
                if (null == MapLyaer)
                {
                    strm.Write((int)0);
                }
                else
                {
                    strm.Write(MapLyaer.Count);

                    WaitProressBar.Value = 0;
                    WaitProressBar.Maximum = MapLyaer.Count;
                    WaitProressBar.Step = 1;

                    foreach (var layer in MapLyaer)
                    {
                        layer.Write(strm);

                        WaitProressBar.PerformStep();
                    }
                }
            }

            // チェックポイント セクション
            {
                if (null == CheckPoints)
                {
                    strm.Write((int)0);
                }
                else
                {
                    strm.Write(CheckPoints.Count);
                    foreach (var cp in CheckPoints)
                    {
                        cp.Write(strm);
                    }
                }
            }

            WaitProressBar.Value = 0;
        }

        /// <summary>
        /// バイナリデータ ロード
        /// </summary>
        /// <param name="strm"></param>
        private void Read(BinaryReader strm)
        {
            strm.ReadInt32();   // FileVersion
            strm.ReadInt32();   // セクション数

            LRF_LogFileName = strm.ReadString();

            // マップレイヤー セクション
            {
                int numLayer = strm.ReadInt32();

                WaitProressBar.Value = 0;
                WaitProressBar.Maximum = numLayer;
                WaitProressBar.Step = 1;

                MapLyaer = new List<LayerData>();
                for (int i = 0; i < numLayer; i++)
                {
                    LayerData layer = new LayerData();
                    layer.Read(strm);

                    layer.MakeMapBmp(LRF_Range, LRF_ScaleOfPixel, LRF_PixelSize, colLayerPixel, colLayerBase);
                    MapLyaer.Add(layer);

                    WaitProressBar.PerformStep();
                }
            }

            // チェックポイント セクション
            {
                int numCp = strm.ReadInt32();

                CheckPoints = new List<CheckPointData>();
                for (int i = 0; i < numCp; i++)
                {
                    CheckPointData cp = new CheckPointData();
                    cp.Read(strm);
                }
                UpdateCheckPointList();
            }

            WaitProressBar.Value = 0;
        }

        /// <summary>
        /// レイヤー選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sb_VMapLayer_ValueChanged(object sender, EventArgs e)
        {
            num_Layer.Value = sb_VMapLayer.Value;
        }

        private void num_Layer_ValueChanged(object sender, EventArgs e)
        {
            int selIdx = (int)num_Layer.Value;

            if (null != MapLyaer && selIdx >= MapLyaer.Count)
            {
                selIdx = MapLyaer.Count - 1;
            }
            num_Layer.Value = selIdx;
            SelectEditLayer(selIdx);
        }

        /// <summary>
        /// エディットレイヤー選択
        /// </summary>
        /// <param name="selIdx"></param>
        private void SelectEditLayer(int selIdx)
        {
            // 前のエディットレイヤーを元のカラーに戻す
            if (null != EditLayer)
            {
                EditLayer.UpdateMapBmp(LRF_PixelSize, colLayerPixel, colLayerBase);
            }

            if (null != MapLyaer)
            {
                EditLayer = MapLyaer[selIdx];
                EditLayer.UpdateMapBmp(LRF_PixelSize, colEditLayerPixel, colLayerBase);

                num_PositionX.Value = (int)EditLayer.lcX;
                num_PositionY.Value = (int)EditLayer.lcY;
                num_Angle.Value = (int)EditLayer.lcAng;
                cb_UseLayer.Checked = EditLayer.useFlg;

                UpdateEditMap();

                UpdateAllLayerFLG = true;
                UpdateTRG = true;
            }
        }

        private void num_Angle_ValueChanged(object sender, EventArgs e)
        {
            if (null != EditLayer)
            {
                EditLayer.lcAng = (double)num_Angle.Value;
                UpdateTRG = true;
            }
        }

        // マップ時差更新
        private void tmr_MapUpdate_Tick(object sender, EventArgs e)
        {
            if (tabCtrlEditPage.SelectedIndex == (int)TAB_PAGE.MAP_EDIT)
            {
                // マップエディットモード時
                if (UpdateTRG)
                {
                    if (viewMoveFlg)
                    {
                        // 高速描画
                        UpdateWorldMap(true, true);
                        UpdateAllLayerFLG = true;
                    }
                    else
                    {
                        UpdateWorldMap(UpdateAllLayerFLG, false);   // エディットレイヤのみ更新
                        UpdateAllLayerFLG = false;
                    }
                    UpdateTRG = false;
                }
            }
            else if (tabCtrlEditPage.SelectedIndex == (int)TAB_PAGE.CHECKPOINT)
            {
                // チェックポイントモード時
                UpdateCheckPointMap();
                UpdateTRG = false;
            }
        }

        // Mapスケール　スクロールバー変更
        private void tber_MapScale_MouseUp(object sender, MouseEventArgs e)
        {
            SetViewScale((float)tber_ViewScale.Value/100.0f);
            //UpdateWorldMap(true, false);
            UpdateTRG = true;
        }

        private void tber_MapScale_ValueChanged(object sender, EventArgs e)
        {
            lbl_ViewScale.Text = "MapScale:"+ tber_ViewScale.Value +"%";
        }

        /// <summary>
        /// レイヤー使用ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_UseLayer_CheckedChanged(object sender, EventArgs e)
        {
            if (EditLayer != null)
            {
                EditLayer.useFlg = cb_UseLayer.Checked;
            }
        }

        private void cb_UseLayer_Click(object sender, EventArgs e)
        {
            UpdateTRG = true;
        }


        // ==================================================================================

        /// <summary>
        /// 全レイヤーの描画に必要なマップサイズを求める
        /// </summary>
        /// <param name="mapOffsetX">オフセット</param>
        /// <param name="mapOffsetY"></param>
        /// <param name="mapWidth">幅</param>
        /// <param name="mapHeight">高さ</param>
        private void CalcOutputMapSize(ref int mapOffsetX, ref int mapOffsetY, ref int mapWidth, ref int mapHeight)
        {
            double minX, minY;
            double maxX, maxY;

            minX = maxX = 0.0;
            minY = maxY = 0.0;

            foreach (var layer in MapLyaer)
            {
                if (layer.useFlg)
                {
                    if (layer.wX < minX)
                    {
                        minX = layer.wX;
                    }
                    if (layer.wY < minY)
                    {
                        minY = layer.wY;
                    }

                    if (layer.wX + layer.MapBmp.Width > maxX)
                    {
                        maxX = layer.wX + layer.MapBmp.Width;
                    }
                    if (layer.wY + layer.MapBmp.Height > maxY)
                    {
                        maxY = layer.wY + layer.MapBmp.Height;
                    }
                }
            }

            mapOffsetX = (int)-minX;
            mapOffsetY = (int)-minY;
            mapWidth = (int)(maxX - minX);
            mapHeight = (int)(maxY - minY);
        }

        /// <summary>
        /// マップをBMP保存
        /// </summary>
        /// <param name="fname"></param>
        public void SaveMapBmp( string fname )
        {
            int mapOfstX = 0;
            int mapOfstY = 0;
            int mapWidth = 0;
            int mapHeight = 0;

            // 出力マップサイズを求める
            {
                int mapReso = 100;  // 最小ピクセル単位 (100ピクセル単位)

                CalcOutputMapSize(ref mapOfstX, ref mapOfstY, ref mapWidth, ref mapHeight);

                // 最小単位区切りに収まるサイズに変更
                mapWidth = ((mapWidth + (mapReso - 1)) / mapReso) * mapReso;
                mapHeight = ((mapHeight + (mapReso - 1)) / mapReso) * mapReso;
            }

            if (mapWidth == 0 || mapHeight == 0)
            {
                MessageBox.Show("出力対象のレイヤーがありません",
                                 "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                return;
            }

            {
                Bitmap SaveMapBmp = new Bitmap(mapWidth, mapHeight);

                ViewTransX = mapOfstX;
                ViewTransY = mapOfstY;
                ViewScale = 1.0f;

                Graphics g = Graphics.FromImage(SaveMapBmp);
                g.FillRectangle(new SolidBrush(Color.FromArgb(0xFF, 0xFF, 0xFF)), 0, 0, mapWidth, mapHeight);
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                foreach (var layer in MapLyaer)
                {
                    if (layer.useFlg)
                    {
                        layer.UpdateMapBmp(LRF_PixelSize, Color.FromArgb(0x0, 0x0, 0x0), Color.FromArgb(0xFF, 0xFF, 0xFF), false);
                        DrawLayerToWorld(g, layer, false, false);
                    }
                }
                g.Dispose();

                // 画像ファイル書き出し
                if (Path.GetExtension(fname).ToLower() == ".bmp")
                {
                    SaveMapBmp.Save(fname, System.Drawing.Imaging.ImageFormat.Bmp);
                }
                else if (Path.GetExtension(fname).ToLower() == ".png")
                {
                    SaveMapBmp.Save(fname, System.Drawing.Imaging.ImageFormat.Png);
                }


                // レイヤーカラーを戻す
                foreach (var layer in MapLyaer)
                {
                    layer.UpdateMapBmp(LRF_PixelSize, colLayerPixel, colLayerBase);
                }
                if (null != EditLayer)
                {
                    EditLayer.UpdateMapBmp(LRF_PixelSize, colEditLayerPixel, colLayerBase);
                }
            }

            lbl_ViewPosition.Text = "MapSize:" + mapWidth.ToString() + "," + mapHeight.ToString();

            MessageBox.Show(mapWidth.ToString() + "," + mapHeight.ToString() + "サイズのマップを生成しました",
                             "BmpSave",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Information);

        }



        private void MapEditForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }

        private void tb_SkipFrame_KeyDown(object sender, KeyEventArgs e)
        {
            MapEditForm_KeyDown(sender, e);
            tb_SkipFrame.Text = "";
        }

        /// <summary>
        /// RE Log読み込み
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_RELoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog fDlg = new OpenFileDialog();

            fDlg.Filter = "RotaryEncoder TelnetLogFile (*.out)|*.out";

            var Result = fDlg.ShowDialog();
            if (Result != System.Windows.Forms.DialogResult.OK) return;

            // REncoder
            try
            {
                
                {
                    REncoderInput reInput = new REncoderInput();
                    reInput.OpenFile(fDlg.FileName);
                    double[] reData = reInput.getScanData();
                    reInput.CloseFile();

                    // 変換&演算
                    {
                        reDataR = new double[reData.Length / 4];
                        reDataL = new double[reData.Length / 4];

                        for (int i = 0; i < reData.Length/4; i++)
                        {
                            reDataL[i] = reData[i * 4 + 3];
                            reDataR[i] = reData[i * 4 + 2];
                        }

                        REncoderToMap.CalcWheelPlotXY(out reWheelR, out reWheelL, reDataR, reDataL, (int)num_LSPlv.Value, ((double)trackBar_LSP.Value * 0.01));
                        UpdateTRG = true;
                    }
                }
                tb_RELogFile.Text = fDlg.FileName;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Load Fail");
            }

            cb_REDisp.Checked = true;
        }

        // ロータリーエンコーダ情報更新
        private void cb_REDisp_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTRG = true;
        }

        private void num_RERotate_ValueChanged(object sender, EventArgs e)
        {
            UpdateTRG = true;
        }

        // ================================================================
        /// <summary>
        /// タブチェンジ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCtrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCtrlEditPage.SelectedIndex == (int)TAB_PAGE.MAP_EDIT)
            {
                // MapEdit
                if (null != MapLyaer)
                {
                    // レイヤーカラーを戻す
                    foreach (var layer in MapLyaer)
                    {
                        layer.UpdateMapBmp(LRF_PixelSize, colLayerPixel, colLayerBase);
                    }
                    if (null != EditLayer)
                    {
                        EditLayer.UpdateMapBmp(LRF_PixelSize, colEditLayerPixel, colLayerBase);
                    }
                }
            }
            else if (tabCtrlEditPage.SelectedIndex == (int)TAB_PAGE.CHECKPOINT)
            {
                // CheckPoint
                if (null != MapLyaer)
                {
                    // 全体ＭＡＰ作成
                    RenderedWorldMap = MakeCheckPointBMP_FromMaplayer();
                    UpdateCheckPointMap();
                }

                // 右クリックでビュー移動
                // 左クリックで、ＣＰ追加or選択、ドラッグ移動

            }
        }


        /// <summary>
        /// 北向きを基準とした方向　(360度)
        /// </summary>
        /// <param name="vecX"></param>
        /// <param name="vecY"></param>
        /// <returns></returns>
        private double CalcVecToDir(double vecX, double vecY)
        {
            double vecLen = Math.Sqrt(vecX * vecX + vecY * vecY);

            if (vecLen == 0.0) return 0.0;

            vecX /= vecLen;
            vecY /= vecLen;

            // DotProduct NorthVec
            double asVal = 0.0 * vecX + -1.0 * vecY;
            return ((Math.Asin(asVal) * 180.0 / Math.PI) - 90.0) * ((vecX<0.0)?-1.0:1.0);
        }

        // -----------------------------------------------------------------------
        // RE
        double[] reDataR;
        double[] reDataL;
        private void trackBar_LSP_ValueChanged(object sender, EventArgs e)
        {
            Lbl_LSP.Text = ((double)trackBar_LSP.Value * 0.01).ToString("0.00");
            num_LSPlv_ValueChanged(sender, e);
        }

        private void num_LSPlv_ValueChanged(object sender, EventArgs e)
        {
            if (null != reDataR && null != reDataL)
            {
                REncoderToMap.CalcWheelPlotXY(out reWheelR, out reWheelL, reDataR, reDataL, (int)num_LSPlv.Value, ((double)trackBar_LSP.Value * 0.01));
            }
            UpdateTRG = true;
        }

        /// <summary>
        /// マップローカル座標修正
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (null != MapLyaer)
            {
                LayerData prntLayer = null;

                foreach (var layer in MapLyaer)
                {
                    if (null == prntLayer) layer.CalcFixWorldPos(0.0, 0.0, 0.0);
                    else layer.CalcFixWorldPos(prntLayer.wX, prntLayer.wY, prntLayer.wAng);

                    prntLayer = layer;
                }

                UpdateWorldMap();
            }
        }


        /// <summary>
        /// ベースカラーを返す
        /// </summary>
        /// <param name="rgb"></param>
        /// <returns></returns>
        public byte GetMapColorBase(int rgb)
        {
            if( rgb == 0 ) return colLayerBase.R;
            else if (rgb == 1) return colLayerBase.G;
            else if (rgb == 2) return colLayerBase.B;

            return 0;
        }

    }
}
