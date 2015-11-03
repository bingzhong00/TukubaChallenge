using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using CersioIO;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LRFMapEditer
{
    public partial class MapEditForm : Form
    {
        /// <summary>
        /// レイヤーの親子関係を更新
        /// </summary>
        /// <param name="bAllLayerUpdate">True..すべてのレイヤー / False..EditLayer以降</param>
        public void UpdateLayerData(bool bAllLayerUpdate = true)
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
                Rectangle rect = new Rectangle(0, 0, layer.MapBmp.Width - 1, layer.MapBmp.Height - 1);
                Pen colPen = Pens.Silver;

                if (layer.drawColor == colEditLayerPixel)
                {
                    colPen = new Pen(colEditLayerPixel);
                }
                g.DrawRectangle(colPen, rect);
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
        private void UpdateWorldMap(bool allLayerUpdate = true, bool fastDraw = false)
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

                    //DrawBGMap(g);
                }
                else
                {
                    //g.FillRectangle(Brushes.Black, 0, 0, pb_VMap.Width, pb_VMap.Height);
                    //DrawBGMap(g);

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


        // VMap PictureBox  操作イベント ====================================================================================
        private bool wldMoveFlg = false;
        private bool viewMoveFlg = false;
        private bool wldRotFlg = false;
        private bool bgMoveFlg = false;

        int msX, msY;
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
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                // BGMap移動
                bgMoveFlg = true;
            }

            // 移動前の座標を記憶
            msX = e.X;
            msY = e.Y;

            if (null != EditLayer && wldMoveFlg)
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

            if (bgMoveFlg)
            {
                stX = (int)BGMapViewTransX;
                stY = (int)BGMapViewTransY;
                stAng = BGMapViewScale;
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
                        ViewScale = (float)stAng + (float)difX * 0.1f;
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
            else if (bgMoveFlg)
            {
                // BGMap移動
                int difX = e.X - msX;
                int difY = e.Y - msY;

                if (difX != 0 && difY != 0)
                {
                    if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    {
                        // Shift+でスケール
                        BGMapViewScale = (float)stAng + (float)difX * 0.1f;
                    }
                    else
                    {
                        BGMapViewTransX = stX + difX;
                        BGMapViewTransY = stY + difY;
                    }
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
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                bgMoveFlg = false;
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
                else if (e.Alt)
                {
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

        /// <summary>
        /// エディット中レイヤー表示
        /// </summary>
        private void UpdateEditMap()
        {
            Graphics g = Graphics.FromImage(pb_EditLayer.Image);

            g.FillRectangle(Brushes.Black, 0, 0, pb_EditLayer.Width, pb_EditLayer.Height);
            g.DrawImage(EditLayer.MapBmp,
                        (pb_EditLayer.Width - EditLayer.MapBmp.Width) / 2,
                        (pb_EditLayer.Height - EditLayer.MapBmp.Height) / 2);

            g.Dispose();
            pb_EditLayer.Invalidate();
        }
    }
}
