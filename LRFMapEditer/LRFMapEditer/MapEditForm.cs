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
        /// レイヤーの親子関係を更新
        /// </summary>
        /// <param name="bAllLayerUpdate">True..すべてのレイヤー / False..EditLayer以降</param>
        public void UpdateLayerData(bool bAllLayerUpdate=true)
        {
            if (null != MapLyaer)
            {
                bool updateFlg = false;
                LayerData prntLayer = null;
                if (bAllLayerUpdate) updateFlg = true;

                foreach (var layer in MapLyaer)
                {
                    // allLayerUpdateじゃなければ、エディット中のレイヤー以降を更新
                    if (EditLayer == layer) updateFlg = true;

                    if (updateFlg)
                    {
                        if (null == prntLayer) layer.CalcWorldPos(0.0, 0.0, 0.0);
                        else layer.CalcWorldPos(prntLayer.wX, prntLayer.wY, prntLayer.wAng);
                    }

                    prntLayer = layer;
                }
            }
        }

        /// <summary>
        /// 指定のインデックス以外のレイヤーを１枚のワールドマップに描画
        /// </summary>
        /// <param name="noDrawLayer">描画しないレイヤー nullならすべて描画</param>
        /// <returns></returns>
        private void UpdateLayerMap(LayerData noDrawLayer)
        {
            //Bitmap newWorldMap = new Bitmap(WorldMapWidth, WorldMapHeight);
            if (null == MapLyaer) return;

            Graphics g = Graphics.FromImage(LayerMap);
            g.FillRectangle(Brushes.Black, 0, 0, LayerMap.Width, LayerMap.Height);

            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            foreach (var layer in MapLyaer)
            {
                if (layer == noDrawLayer) break;

                if (layer.useFlg)
                {
                    DrawLayerToWorld(g, layer, false, false);
                }
            }
            g.Dispose();

            LayerMap.MakeTransparent(Color.White);

            //return LayerMap;
        }

        // 前回のエディットレイヤー描画位置
        /*
        private double OldEdtWX;
        private double OldEdtWY;
        private double OldEdtAng;
        */
        /// <summary>
        /// ワールドマップ更新
        /// </summary>
        /// <param name="allLayerUpdate">全レイヤー書き換え</param>
        /// <param name="fastDraw">高速描画</param>
        private void UpdateWorldMap( bool allLayerUpdate = true, bool fastDraw = false )
        {
            // 各レイヤーの座標更新
            UpdateLayerData();

            // 画面更新
            Graphics g = Graphics.FromImage(pb_VMap.Image);
            if (fastDraw)
            {
                // 高速描画（枠だけ）
                g.FillRectangle(Brushes.Black, 0, 0, pb_VMap.Width, pb_VMap.Height);

                if (null != MapLyaer)
                {
                    foreach (var layer in MapLyaer)
                    {
                        DrawLayerToWorld(g, layer, false, true);
                    }
                }

                // ガイドライン描画
                DrawGuideLine(g);

                // ＲＥ描画
                if (cb_REDisp.Checked && null != reWheelR && reWheelR.Length > 0)
                {
                    DrawREconderData(g, reWheelR, reWheelL);
                }

                // エディットレイヤー描画
                if (null != EditLayer)
                {
                    DrawLayerToWorld(g, EditLayer, true, false);
                }
            }
            else
            {
                // 全レイヤー表示
                if (allLayerUpdate)
                {
                    // 全面書き換え
                    UpdateLayerMap(EditLayer);
                }

                //g.FillRectangle(Brushes.Black, 0, 0, WorldMap.Width, WorldMap.Height);
                g.InterpolationMode = InterpolationMode.NearestNeighbor;

                if (null == MapLyaer)
                {
                    // LRFがなければ、背景を消しておく
                    g.FillRectangle(Brushes.Black, 0, 0, pb_VMap.Width, pb_VMap.Height);
                }
                else
                {
                    //g.FillRectangle(Brushes.Black, 0, 0, pb_VMap.Width, pb_VMap.Height);

                    // バックレイヤー描画
                    g.DrawImage(LayerMap, 0, 0, LayerMap.Width, LayerMap.Height);

                    bool bDraw = false;
                    // エディットレイヤーから先は、常に変化するので高速描画
                    foreach (var layer in MapLyaer)
                    {
                        if (EditLayer == layer) bDraw = true;
                        if (!bDraw) continue;

                        DrawLayerToWorld(g, layer, false, true);
                    }
                }

                // ガイドライン描画
                DrawGuideLine(g);

                // ＲＥ描画
                if (cb_REDisp.Checked && null != reWheelR && reWheelR.Length > 0)
                {
                    DrawREconderData(g, reWheelR, reWheelL);
                }

                // エディットレイヤー描画
                if (null != EditLayer)
                {
                    DrawLayerToWorld(g, EditLayer, true, false);

                    /*
                    OldEdtWX = EditLayer.GetLocalX();
                    OldEdtWY = EditLayer.GetLocalY();
                    OldEdtAng = EditLayer.GetLocalAng();
                     */
                }
            }
            g.Dispose();

            pb_VMap.Invalidate();
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
        /// ワールド座標、回転を使ってレイヤーマップを生成
        /// </summary>
        /// <param name="g"></param>
        /// <param name="layer"></param>
        /// <param name="drawGuideLine"></param>
        /// <param name="fastDraw"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="bUseView">View座標で描画(BMP出力時などはfalse)</param>
        private void DrawLayerToWorld(Graphics g, LayerData layer, bool drawGuideLine, bool fastDraw)
        {
            int ctrX = layer.MapBmp.Width / 2;
            int ctrY = layer.MapBmp.Height / 2;

            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            g.ResetTransform();
            //g.ScaleTransform((float)DrawMapScale, (float)DrawMapScale);
            g.TranslateTransform(-ctrX, -ctrY, MatrixOrder.Append);
            g.RotateTransform((float)layer.wAng, MatrixOrder.Append);
            g.TranslateTransform(ctrX + (int)layer.wX, ctrY + (int)layer.wY, MatrixOrder.Append);
            // View
            g.ScaleTransform(ViewScale, ViewScale, MatrixOrder.Append);
            g.TranslateTransform(ViewTransX, ViewTransY, MatrixOrder.Append);

            //g.DrawImageUnscaled(layer.MapBmp, 0, 0);
            if (fastDraw)
            {
                // 高速描画用トライアングル
                g.DrawPolygon(Pens.Red, layer.psLayerTriangle);
            }
            else
            {
                // 通常描画
                g.DrawImage(layer.MapBmp, 0, 0);
            }

            if (drawGuideLine)
            {
                // 枠線描画
                Rectangle rect = new Rectangle( 0, 0, layer.MapBmp.Width - 1, layer.MapBmp.Height-1 );
                Pen colPen = Pens.Silver;

                if (layer.drawColor == colEditLayerPixel)
                {
                    colPen = new Pen(colEditLayerPixel);
                }
                g.DrawRectangle(colPen, rect);
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

        // VMap PictureBox  操作イベント ====================================================================================
        private bool wldMoveFlg = false;
        private bool viewMoveFlg = false;
        private bool wldRotFlg = false;
        int msX,msY;
        int stX, stY;
        double stAng;

        // 時差更新　必要フラグ
        private bool UpdateTRG = false;
        private bool UpdateAllLayerFLG = false;

        private void pb_VMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                // オブジェクト移動
                wldMoveFlg = true;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // View移動
                viewMoveFlg = true;
            }

            // 移動前の座標を記憶
            msX = e.X;
            msY = e.Y;

            if (null != EditLayer)
            {
                stX = (int)EditLayer.GetLocalX();
                stY = (int)EditLayer.GetLocalY();
                stAng = EditLayer.GetLocalAng();
            }

            if (viewMoveFlg)
            {
                stX = (int)ViewTransX;
                stY = (int)ViewTransY;
                stAng = ViewScale;
            }
        }

        private void pb_VMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (wldMoveFlg)
            {
                // オブジェクト移動
                int difX = e.X - msX;
                int difY = e.Y - msY;
                if (null != EditLayer && difX != 0 && difY != 0)
                {
                    EditLayer.lcX = stX + difX;
                    EditLayer.lcY = stY + difY;

                    num_PositionX.Value = (int)EditLayer.lcX;
                    num_PositionY.Value = (int)EditLayer.lcY;
                    UpdateTRG = true;
                }
            }
            else if (viewMoveFlg)
            {
                /*
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    Console.WriteLine("Shiftキーが押されています。");
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                    Console.WriteLine("Ctrlキーが押されています。");
                if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt)
                    Console.WriteLine("Altキーが押されています。");
                */
                
                // View移動
                int difX = e.X - msX;
                int difY = e.Y - msY;

                if (difX != 0 && difY != 0)
                {
                    if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    {
                        // Shift+でスケール
                        ViewScale = (float)stAng + (float)difX*0.1f;
                        SetViewScale(ViewScale);
                    }
                    else
                    {
                        ViewTransX = stX + difX;
                        ViewTransY = stY + difY;
                    }
                    UpdateTRG = true;
                }
            }
            else if (wldRotFlg)
            {
                // 回転
                int difX = e.X - msX;
                int difY = e.Y - msY;
                if (null != EditLayer && difX != 0)
                {
                    EditLayer.lcAng = stAng + difX;
                    if (EditLayer.lcAng > 360.0) EditLayer.lcAng -= 360.0;
                    if (EditLayer.lcAng < -360.0) EditLayer.lcAng += 360.0;

                    num_Angle.Value = (int)EditLayer.lcAng;
                    UpdateTRG = true;
                }
            }
        }

        private void pb_VMap_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                wldMoveFlg = false;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                viewMoveFlg = false;
            }

            // Mapの時差更新 OFF
            UpdateTRG = true;
        }

        /// <summary>
        /// マウスホイール情報取得
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pb_VMap_MouseWheel(object sender, MouseEventArgs e)
        {
            int wheelOneDelta = 120;

            if (null != EditLayer)
            {
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                {
                    // レイヤー回転
                    EditLayer.lcAng += e.Delta * SystemInformation.MouseWheelScrollLines / wheelOneDelta;
                    if (EditLayer.lcAng > 360.0) EditLayer.lcAng -= 360.0;
                    if (EditLayer.lcAng < -360.0) EditLayer.lcAng += 360.0;
                    num_Angle.Value = (int)EditLayer.lcAng;
                }
                else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    // レイヤー間の距離変更
                    bool bPassLater = false;
                    double addLength = e.Delta * SystemInformation.MouseWheelScrollLines / wheelOneDelta;

                    // エディットレイヤー以降のレイヤーを調整
                    foreach (var layer in MapLyaer)
                    {
                        if (bPassLater || layer == EditLayer)
                        {
                            layer.lcPivotLength += addLength;
                            bPassLater = true;
                        }
                    }
                }
                else
                {
                    // レイヤー選択
                    int nowIdx = sb_VMapLayer.Value + e.Delta / wheelOneDelta;

                    nowIdx = Math.Max(nowIdx, sb_VMapLayer.Minimum);
                    nowIdx = Math.Min(nowIdx, sb_VMapLayer.Maximum);
                    sb_VMapLayer.Value = nowIdx;
                }

                UpdateTRG = true;
            }
        }

        private void pb_VMap_Click(object sender, EventArgs e)
        {
            // ホイールイベント通知用
            pb_VMap.Focus();
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

        /// <summary>
        /// エディット中レイヤー表示
        /// </summary>
        private void UpdateEditMap()
        {
            Graphics g = Graphics.FromImage(pb_EditLayer.Image);

            g.FillRectangle(Brushes.Black, 0, 0, pb_EditLayer.Width, pb_EditLayer.Height);
            g.DrawImage( EditLayer.MapBmp,
                        (pb_EditLayer.Width - EditLayer.MapBmp.Width)/2,
                        (pb_EditLayer.Height - EditLayer.MapBmp.Height) / 2);

            g.Dispose();
            pb_EditLayer.Invalidate();
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
                    foreach (var layer in MapLyaer)
                    {
                        layer.Write(strm);
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

                MapLyaer = new List<LayerData>();
                for (int i = 0; i < numLayer; i++)
                {
                    LayerData layer = new LayerData();
                    layer.Read(strm);

                    layer.MakeMapBmp(LRF_Range, LRF_ScaleOfPixel, LRF_PixelSize, colLayerPixel, colLayerBase);
                    MapLyaer.Add(layer);
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
            EditLayer.lcAng = (double)num_Angle.Value;
            UpdateTRG = true;
        }

        // マップ時差更新
        private void tmr_MapUpdate_Tick(object sender, EventArgs e)
        {
            if (tabCtrl.SelectedIndex == (int)TAB_PAGE.MAP_EDIT)
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
            else if (tabCtrl.SelectedIndex == (int)TAB_PAGE.CHECKPOINT)
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


        // キー操作
        private void MapEditForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (null != EditLayer)
            {
                bool bCngVal = false;
                if (e.Control)
                {
                    int selIdx = (int)num_Layer.Value;

                    if (e.KeyCode == Keys.Right) selIdx++;
                    if (e.KeyCode == Keys.Left) selIdx--;

                    if (selIdx < 0) selIdx = 0;
                    if (selIdx > num_Layer.Maximum) selIdx = (int)num_Layer.Maximum - 1;

                    if (selIdx != num_Layer.Value)
                    {
                        num_Layer.Value = selIdx;
                    }
                }
                else if (e.Shift)
                {
                    if (e.KeyCode == Keys.Right) { EditLayer.lcAng += 1; bCngVal = true; }
                    if (e.KeyCode == Keys.Left) { EditLayer.lcAng -= 1; bCngVal = true; }
                }
                else if (e.Alt) {
                    if (e.KeyCode == Keys.Right) { EditLayer.lcAng += 5; bCngVal = true; }
                    if (e.KeyCode == Keys.Left) { EditLayer.lcAng -= 5; bCngVal = true; }
                }
                else
                {
                    if (e.KeyCode == Keys.Right) { EditLayer.lcX += 1; bCngVal = true; }
                    if (e.KeyCode == Keys.Left) { EditLayer.lcX -= 1; bCngVal = true; }
                }
                if (e.KeyCode == Keys.Up) { EditLayer.lcY -= 1; bCngVal = true; }
                if (e.KeyCode == Keys.Down) { EditLayer.lcY += 1; bCngVal = true; }
                if (e.KeyCode == Keys.Space) { EditLayer.useFlg = EditLayer.useFlg ? false : true; bCngVal = true; }

                if (e.KeyCode == Keys.PageUp) { EditLayer.lcAng += 2; bCngVal = true; }
                if (e.KeyCode == Keys.PageDown) { EditLayer.lcAng -= 2; bCngVal = true; }

                if (bCngVal)
                {
                    num_PositionX.Value = (int)EditLayer.lcX;
                    num_PositionY.Value = (int)EditLayer.lcY;
                    num_Angle.Value = (int)EditLayer.lcAng;
                    cb_UseLayer.Checked = EditLayer.useFlg;

                    UpdateTRG = true;
                }
            }
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
            if (tabCtrl.SelectedIndex == (int)TAB_PAGE.MAP_EDIT)
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
            else if (tabCtrl.SelectedIndex == (int)TAB_PAGE.CHECKPOINT)
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

        // ===============================================================================================================
        List<CheckPointData> CheckPoints = new List<CheckPointData>();
        CheckPointData EditCheckPoint = null;
        int EditListIndex = -1;

        /// <summary>
        /// チェックポイントをリストに追加
        /// </summary>
        /// <param name="newCP"></param>
        private void AddCheckPoint(CheckPointData newCP)
        {
            EditListIndex = listbox_CheckPoint.SelectedIndex;

            if (EditListIndex == -1)
            {
                // 新規追加(末尾)
                CheckPoints.Add(newCP);
            }
            else
            {
                // 挿入
                CheckPoints.Insert(EditListIndex, newCP);
            }

            string listItm = newCP.wdPosX.ToString() + "," + newCP.wdPosY.ToString();
            listbox_CheckPoint.Items.Add(listItm);
        }

        /// <summary>
        /// リスト更新
        /// </summary>
        private void UpdateCheckPointList()
        {
            listbox_CheckPoint.Items.Clear();
            lbl_StartX.Text = "";
            lbl_StartY.Text = "";
            lbl_StartDir.Text = "";

            foreach (var cp in CheckPoints)
            {
                string listItm = cp.wdPosX.ToString() + "," + cp.wdPosY.ToString();
                listbox_CheckPoint.Items.Add(listItm);
            }

            // スタート位置設定
            if (CheckPoints.Count > 0)
            {
                var cp = CheckPoints[0];
                lbl_StartX.Text = cp.wdPosX.ToString();
                lbl_StartY.Text = cp.wdPosY.ToString();
            }

            // 2個以上あれば、スタート向きを計算
            if (CheckPoints.Count > 1)
            {
                var cp1 = CheckPoints[0];
                var cp2 = CheckPoints[1];
                int dir = (int)CalcVecToDir(cp2.wdPosX - cp1.wdPosX, cp2.wdPosY - cp1.wdPosY);
                lbl_StartDir.Text = dir.ToString();
            }

            // 再選択
            if (EditListIndex != -1 && EditListIndex < listbox_CheckPoint.Items.Count-1)
            {
                listbox_CheckPoint.SelectedIndex = EditListIndex;
            }
        }

        /// <summary>
        /// リスト選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listbox_CheckPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditListIndex = listbox_CheckPoint.SelectedIndex;

            if (EditListIndex >= 0 && CheckPoints.Count > 0)
            {
                EditCheckPoint = CheckPoints[EditListIndex];
                //rbtn_AddPoint.Checked = false;
                //rbtn_Edit.Checked = true;
            }
            else
            {
                EditCheckPoint = null;
            }
        }

        /// <summary>
        /// Addモード変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtn_AddPoint_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_AddPoint.Checked)
            {
            }
        }

        /// <summary>
        /// チェックポイント削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (null != EditCheckPoint)
            {
                CheckPoints.Remove(EditCheckPoint);
                EditCheckPoint = null;
                listbox_CheckPoint.SelectedIndex = -1;

                UpdateCheckPointList();
            }
        }


        /// <summary>
        /// リスト選択解除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DisSelect_Click(object sender, EventArgs e)
        {
            // 選択解除
            EditCheckPoint = null;
            listbox_CheckPoint.SelectedIndex = -1;
        }

        /// <summary>
        /// MapLayerからチェックポイント用のＢＭＰを作成
        /// </summary>
        /// <returns></returns>
        private Bitmap MakeCheckPointBMP_FromMaplayer()
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

            Bitmap FullMapBmp = new Bitmap(mapWidth, mapHeight);

            ViewTransX = mapOfstX;
            ViewTransY = mapOfstY;
            ViewScale = 1.0f;

            Graphics g = Graphics.FromImage(FullMapBmp);
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

            // レイヤーカラーを戻す
            foreach (var layer in MapLyaer)
            {
                layer.UpdateMapBmp(LRF_PixelSize, colLayerPixel, colLayerBase);
            }
            if (null != EditLayer)
            {
                EditLayer.UpdateMapBmp(LRF_PixelSize, colEditLayerPixel, colLayerBase);
            }

            return FullMapBmp;
        }

        // チェックポイント　マウス操作 ===============================================================================================================
        /// <summary>
        /// MouseButton Down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pb_CPMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (rbtn_AddPoint.Checked)
                {
                    // チェックポイント新規 追加
                    CheckPointData newCP = new CheckPointData(e.X - ViewTransX, e.Y - ViewTransY);
                    AddCheckPoint(newCP);
                }
                else if (rbtn_Edit.Checked)
                {
                    // オブジェクト移動
                    wldMoveFlg = true;
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // View移動
                viewMoveFlg = true;
            }

            // 移動前の座標を記憶
            msX = e.X;
            msY = e.Y;

            if (null != EditCheckPoint)
            {
                stX = (int)EditCheckPoint.wdPosX;
                stY = (int)EditCheckPoint.wdPosY;
                //stAng = EditLayer.GetLocalAng();
            }

            if (viewMoveFlg)
            {
                stX = (int)ViewTransX;
                stY = (int)ViewTransY;
                stAng = ViewScale;
            }
        }

        /// <summary>
        /// MouseButton Move
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pb_CPMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (wldMoveFlg)
            {
                // オブジェクト移動
                int difX = e.X - msX;
                int difY = e.Y - msY;
                if (null != EditCheckPoint && difX != 0 && difY != 0)
                {
                    EditCheckPoint.wdPosX = stX + difX;
                    EditCheckPoint.wdPosY = stY + difY;

                    //num_PositionX.Value = (int)EditLayer.lcX;
                    //num_PositionY.Value = (int)EditLayer.lcY;
                    UpdateTRG = true;
                }
            }
            else if (viewMoveFlg)
            {
                /*
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    Console.WriteLine("Shiftキーが押されています。");
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                    Console.WriteLine("Ctrlキーが押されています。");
                if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt)
                    Console.WriteLine("Altキーが押されています。");
                */

                // View移動
                int difX = e.X - msX;
                int difY = e.Y - msY;

                if (difX != 0 && difY != 0)
                {
                    
                    if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    {
                        /*
                        // Shift+でスケール
                        ViewScale = (float)stAng + (float)difX * 0.1f;
                        SetViewScale(ViewScale);
                         */ 
                    }
                    else
                    {
                        ViewTransX = stX + difX;
                        ViewTransY = stY + difY;
                    }
                    UpdateTRG = true;
                }
            }

        }

        /// <summary>
        /// MouseButton UP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pb_CPMap_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                wldMoveFlg = false;
                UpdateCheckPointList();
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                viewMoveFlg = false;
            }

            // Mapの時差更新 OFF
            UpdateTRG = true;
        }


        /// <summary>
        /// マウスホイール情報取得
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pb_CPMap_MouseWheel(object sender, MouseEventArgs e)
        {
            int wheelOneDelta = 120;

            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                /*
                // レイヤー回転
                EditLayer.lcAng += e.Delta * SystemInformation.MouseWheelScrollLines / wheelOneDelta;
                if (EditLayer.lcAng > 360.0) EditLayer.lcAng -= 360.0;
                if (EditLayer.lcAng < -360.0) EditLayer.lcAng += 360.0;
                num_Angle.Value = (int)EditLayer.lcAng;
                 * */
            }
            else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                /*
                // レイヤー間の距離変更
                bool bPassLater = false;
                double addLength = e.Delta * SystemInformation.MouseWheelScrollLines / wheelOneDelta;

                // エディットレイヤー以降のレイヤーを調整
                foreach (var layer in MapLyaer)
                {
                    if (bPassLater || layer == EditLayer)
                    {
                        layer.lcPivotLength += addLength;
                        bPassLater = true;
                    }
                }
                 * */
            }
            else
            {
                // チェックポイント選択
                /*
                int nowIdx = sb_VMapLayer.Value + e.Delta / wheelOneDelta;

                nowIdx = Math.Max(nowIdx, sb_VMapLayer.Minimum);
                nowIdx = Math.Min(nowIdx, sb_VMapLayer.Maximum);
                sb_VMapLayer.Value = nowIdx;
                 * */
            }

            UpdateTRG = true;
        }


        /// <summary>
        /// チェックポイントマップ描画
        /// </summary>
        private void UpdateCheckPointMap()
        {
            // 画面更新
            Graphics g = Graphics.FromImage(pb_CPMap.Image);

            g.FillRectangle(Brushes.Black, 0, 0, pb_CPMap.Width, pb_CPMap.Height);

            // MAP描画
            if( null != RenderedWorldMap )
            {
                g.ResetTransform();
                // View
                g.ScaleTransform(ViewScale, ViewScale, MatrixOrder.Append);
                g.TranslateTransform(ViewTransX, ViewTransY, MatrixOrder.Append);

                g.DrawImage(RenderedWorldMap, 0, 0);
            }

            // ガイドライン
            DrawGuideLine(g);

            // ＲＥ描画
            if (cb_REDisp.Checked && null != reWheelR && reWheelR.Length > 0)
            {
                DrawREconderData(g, reWheelR, reWheelL);
            }

            // 開始位置
            if (!string.IsNullOrEmpty(lbl_StartX.Text) && !string.IsNullOrEmpty(lbl_StartY.Text) &&
                !string.IsNullOrEmpty(lbl_StartDir.Text) )
            {
                double stX = 0.0;
                double stY = 0.0;
                double stDir = 0.0;

                double.TryParse( lbl_StartX.Text, out stX );
                double.TryParse( lbl_StartY.Text, out stY );
                double.TryParse( lbl_StartDir.Text, out stDir );

                DrawMaker(g, Brushes.Red, stX, stY, stDir, 10.0);
            }

            // チェックポイント描画
            {
                CheckPointData oldCp = null;
                foreach (var cp in CheckPoints)
                {
                    DrawCheckPoint(g, cp, oldCp, ((cp == EditCheckPoint)?true:false) );
                    oldCp = cp;
                }
            }

            g.Dispose();

            pb_CPMap.Invalidate();
        }

        /// <summary>
        /// チェックポイント マーカ表示
        /// </summary>
        /// <param name="g"></param>
        /// <param name="cp"></param>
        /// <param name="pastCp"></param>
        private void DrawCheckPoint(Graphics g, CheckPointData cp, CheckPointData pastCp, bool bSelected )
        {
            int nonScl = (int)(10.0 / ViewScale);
            int nonSclHf = nonScl / 2;
            int nonScl3Q = nonSclHf * 3 / 4;

            if (nonSclHf <= 0) nonSclHf = 1;
            if (nonScl3Q <= 0) nonScl3Q = 1;

            g.ResetTransform();
            // View
            g.ScaleTransform(ViewScale, ViewScale, MatrixOrder.Append);
            g.TranslateTransform(ViewTransX, ViewTransY, MatrixOrder.Append);

            // チェックポイント間のライン
            if (null != pastCp)
            {
                g.DrawLine(Pens.Blue,
                           (float)pastCp.wdPosX, (float)pastCp.wdPosY,
                           (float)cp.wdPosX, (float)cp.wdPosY);
            }

            // WorldPos
            g.TranslateTransform((float)cp.wdPosX, (float)cp.wdPosY, MatrixOrder.Prepend);

            // 菱型
            /*
            Point[] ps = {
                 new Point( 0, -nonScl3Q),
                 new Point(nonScl3Q, 0),
                 new Point(0, nonScl3Q),
                 new Point(-nonScl3Q, 0)
             };
            g.FillPolygon(Brushes.Yellow, ps);
            */
            Pen dPen = Pens.Cyan;
            if (bSelected) dPen = Pens.Yellow;

            g.DrawEllipse(dPen, -nonSclHf, -nonSclHf, nonScl, nonScl);
        }

        /// <summary>
        /// マーカー描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="brush"></param>
        /// <param name="mkX"></param>
        /// <param name="mkY"></param>
        /// <param name="mkDir"></param>
        /// <param name="size"></param>
        private void DrawMaker(Graphics g, Brush brush, double mkX, double mkY, double mkDir, double size)
        {
            double nonScl = size / ViewScale;

            if (nonScl <= 1.0) nonScl = 1.0;

            g.ResetTransform();
            // View
            g.ScaleTransform(ViewScale, ViewScale, MatrixOrder.Append);
            g.TranslateTransform(ViewTransX, ViewTransY, MatrixOrder.Append);

            mkDir = mkDir-90.0;

            var P1 = new PointF(
                (float)(mkX + nonScl * -Math.Cos(mkDir * Math.PI / 180.0)),
                (float)(mkY + nonScl * Math.Sin(mkDir * Math.PI / 180.0)));
            var P2 = new PointF(
                (float)(mkX + nonScl * -Math.Cos((mkDir - 150) * Math.PI / 180.0)),
                (float)(mkY + nonScl * Math.Sin((mkDir - 150) * Math.PI / 180.0)));
            var P3 = new PointF(
                (float)(mkX + nonScl * -Math.Cos((mkDir + 150) * Math.PI / 180.0)),
                (float)(mkY + nonScl * Math.Sin((mkDir + 150) * Math.PI / 180.0)));

            g.FillPolygon(brush, new PointF[] { P1, P2, P3 });
        }

        /// <summary>
        /// タイムスタンプ付きファイル名生成
        /// </summary>
        /// <param name="strHead"></param>
        /// <param name="strExt"></param>
        /// <returns></returns>
        public string GetTimeStampFileName(string strHead, string strExt )
        {
            return strHead + DateTime.Now.ToString("yyyyMMdd_HHmm") + strExt;
        }

        /// <summary>
        /// チェックポイント出力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportCheckPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog SvDlg = new SaveFileDialog();

            // SavePath
            string saveFname = Path.GetDirectoryName(Directory.GetCurrentDirectory()) + "\\" + GetTimeStampFileName("CheckPoint", ".txt");

            SvDlg.InitialDirectory = Path.GetDirectoryName(saveFname);
            SvDlg.FileName = saveFname;
            SvDlg.Filter = "TxtFile(*.txt)|*.txt|All Files(*.*)|*.*";

            if (DialogResult.OK == SvDlg.ShowDialog())
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(SvDlg.FileName, true, System.Text.Encoding.GetEncoding("shift_jis"));

                // マップファイル名
                if (null != nowMapFilename)
                {
                    sw.Write("//" + nowMapFilename + System.Environment.NewLine);
                }

                // 改行
                sw.Write("//" + System.Environment.NewLine);

                // スタート情報
                if (!string.IsNullOrEmpty(lbl_StartX.Text) && !string.IsNullOrEmpty(lbl_StartY.Text) &&
                    !string.IsNullOrEmpty(lbl_StartDir.Text))
                {
                    sw.Write("// StartPostion " + lbl_StartX.Text + "," + lbl_StartY.Text + System.Environment.NewLine);
                    sw.Write("// StartDir     " + lbl_StartDir.Text + System.Environment.NewLine);
                }


                // チェックポイント書き込み
                foreach (var cp in CheckPoints)
                {
                    //string listItm = cp.wdPosX.ToString() + "," + cp.wdPosY.ToString();
                    string listItm = "new Vector3("+cp.wdPosX.ToString() + "," + cp.wdPosY.ToString()+",0),";
                    sw.Write(listItm + System.Environment.NewLine);
                }

                //閉じる
                sw.Close();

                MessageBox.Show("Save Finish");
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

    }
}
