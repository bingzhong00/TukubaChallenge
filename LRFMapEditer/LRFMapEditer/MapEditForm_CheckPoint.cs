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
            if (EditListIndex != -1 && EditListIndex < listbox_CheckPoint.Items.Count - 1)
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
            //int wheelOneDelta = 120;

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
            if (null != RenderedWorldMap)
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
                !string.IsNullOrEmpty(lbl_StartDir.Text))
            {
                double stX = 0.0;
                double stY = 0.0;
                double stDir = 0.0;

                double.TryParse(lbl_StartX.Text, out stX);
                double.TryParse(lbl_StartY.Text, out stY);
                double.TryParse(lbl_StartDir.Text, out stDir);

                DrawMaker(g, Brushes.Red, stX, stY, stDir, 10.0);
            }

            // チェックポイント描画
            {
                CheckPointData oldCp = null;
                foreach (var cp in CheckPoints)
                {
                    DrawCheckPoint(g, cp, oldCp, ((cp == EditCheckPoint) ? true : false));
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
        private void DrawCheckPoint(Graphics g, CheckPointData cp, CheckPointData pastCp, bool bSelected)
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

            mkDir = mkDir - 90.0;

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
        public string GetTimeStampFileName(string strHead, string strExt)
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
                    sw.Write("static public startPosition = new Vector3(" + lbl_StartX.Text + "," + lbl_StartY.Text + ", 0);" + System.Environment.NewLine);
                    sw.Write("static public double startDir = " + lbl_StartDir.Text + ";" + System.Environment.NewLine);
                }

                // チェックポイント書き込み
                foreach (var cp in CheckPoints)
                {
                    //string listItm = cp.wdPosX.ToString() + "," + cp.wdPosY.ToString();
                    string listItm = "new Vector3(" + cp.wdPosX.ToString() + "," + cp.wdPosY.ToString() + ",0),";
                    sw.Write(listItm + System.Environment.NewLine);
                }

                //閉じる
                sw.Close();

                MessageBox.Show("Save Finish");
            }

        }
    }
}