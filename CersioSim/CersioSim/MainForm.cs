using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing;
using System.Drawing.Drawing2D;

using System.Diagnostics;
using LocationPresumption;

using Axiom.Math;

namespace CersioSim
{
    public partial class CersioSimForm : Form
    {
        public Bitmap SimAreaBmp;
        public Bitmap MapBmp;

        // mm からピクセルへの変換
        const double ScalePixelToReal = 100.0;    // 10mmを１ピクセルとする
        const double ScaleRealToPixel = 1.0 / ScalePixelToReal;

        CarSim carSim = new CarSim();
        MarkPoint carInitPos = new MarkPoint(820 * ScalePixelToReal, 850 * ScalePixelToReal, 0);
        MarkPoint carPos;
        LRFMapForm mapForm;

        const string MapFileName = "../mapdata/utubo01_1200x1300_fix.png";

        private double viewX;
        private double viewY;

        private double viewScale = 1.0;

        /// <summary>
        /// 
        /// </summary>
        public CersioSimForm()
        {
            InitializeComponent();

            SimAreaBmp = new Bitmap(picbox_SimArea.Width, picbox_SimArea.Height);
            picbox_SimArea.Image = SimAreaBmp;

            SetView_CarCenter();

            MapBmp = new Bitmap(MapFileName);

            carPos = new MarkPoint(carInitPos.X, carInitPos.Y, carInitPos.Theta );
            carSim.CarInit(carInitPos);
            carSim.MapInit(MapFileName);
            {
                mapForm = new LRFMapForm(carSim, 1200, 1300, ScaleRealToPixel);
                mapForm.Show();
            }

            tmr_Update.Enabled = true;
        }

        /// <summary>
        /// クルマの中心にビューを移動
        /// </summary>
        public void SetView_CarCenter()
        {
            //画面中心に移動
            viewX = -(picbox_SimArea.Width / viewScale) / 2;
            viewY = -(picbox_SimArea.Height / viewScale) / 2;
            // クルマの位置
            //viewX += carPos.X * ScaleRealToPixel;
            //viewY += carPos.Y * ScaleRealToPixel;
            viewX += ((double)carSim.wdCarF.x) * ScaleRealToPixel;
            viewY += ((double)carSim.wdCarF.y) * ScaleRealToPixel;
        }

        /// <summary>
        /// タイマー更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmr_Update_Tick(object sender, EventArgs e)
        {
            //if (keyUp) carAccVal += 0.05;
            //else carAccVal *= 0.80;

            //if (carAccVal > 1.0) carAccVal = 1.0;

            /*
            if (keyLeft) carSim.carHandleAng += -0.5;
            else if (keyRight) carSim.carHandleAng += 0.5;
            else carSim.carHandleAng *= 0.80;
            */

            if (carSim.carHandleAng > carSim.carHandleAngMax) carSim.carHandleAng = carSim.carHandleAngMax;
            if (carSim.carHandleAng < -carSim.carHandleAngMax) carSim.carHandleAng = -carSim.carHandleAngMax;

            if (Math.Abs(carSim.carAccVal) < 0.01) carSim.carAccVal = 0.0;
            if (Math.Abs(carSim.carHandleAng) < 0.01) carSim.carHandleAng = 0.0;

            lbl_HandleVal.Text = "ハンドル:" + carSim.carHandleAng.ToString("F2");
            lbl_AccVal.Text = "アクセル:" + carSim.carAccVal.ToString("F2");

            //lbl_CarX.Text = "carX:" + ((double)carSim.wdCarF.x).ToString("F2");
            //lbl_CarY.Text = "carY:" + ((double)carSim.wdCarF.y).ToString("F2");
            lbl_CarX.Text = "carX:" + ((double)carSim.mkp.X * ScaleRealToPixel).ToString("F2");
            lbl_CarY.Text = "carY:" + ((double)carSim.mkp.Y * ScaleRealToPixel).ToString("F2");
            lbl_Speed.Text = "Speed(Km):" + ((double)(4.0 * carSim.carAccVal)).ToString("F2");

            // CarSim更新
            // 位置情報更新
            carSim.calcTirePos(tmr_Update.Interval);

            // センサー情報更新
            carSim.SenserUpdate();

            //carPos.X = (double)carSim.wdCarF.x;
            //carPos.Y = (double)carSim.wdCarF.y;

            // 画面移動
            if (cb_TraceView.Checked == true)
            {
                SetView_CarCenter();
            }

            // 描画更新
            DrawUpdateSimArea();
            picbox_SimArea.Invalidate();

            // マップフォーム処理
            mapForm.tmr_Update_Tick(sender, e);
        }

        /// <summary>
        /// 描画更新
        /// </summary>
        public void DrawUpdateSimArea()
        {
            Graphics g = Graphics.FromImage(SimAreaBmp);

            g.FillRectangle(Brushes.Black, 0, 0, SimAreaBmp.Width, SimAreaBmp.Height);

            g.TranslateTransform((float)(-viewX * viewScale), (float)(-viewY * viewScale), MatrixOrder.Append);
            g.ScaleTransform((float)(viewScale), (float)(viewScale));

            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(MapBmp, 0, 0);

            g.ResetTransform();
            g.ScaleTransform((float)(ScaleRealToPixel * viewScale), (float)(ScaleRealToPixel * viewScale));

            // グリッド線
            for (int x = 0; x <= (int)((SimAreaBmp.Width / ScaleRealToPixel) / 1000.0); x++)
            {
                int dx = (x * 1000) + (((int)((-viewX * ScalePixelToReal) + 0.5)) % 1000);
                g.DrawLine(Pens.DarkGray, dx, 0, dx, (int)(SimAreaBmp.Height / ScaleRealToPixel));
            }
            for (int y = 0; y <= (int)((SimAreaBmp.Height / ScaleRealToPixel) / 1000.0); y++)
            {
                int dy = (y * 1000) + (((int)((-viewY * ScalePixelToReal) + 0.5)) % 1000);
                g.DrawLine(Pens.DarkGray, 0, dy, (int)(SimAreaBmp.Width / ScaleRealToPixel), dy);
            }


            g.TranslateTransform((float)-viewX, (float)-viewY, MatrixOrder.Append);

            carSim.DrawCar(g, ScaleRealToPixel, viewScale, viewX, viewY);

            g.Dispose();
        }


        // キー入力----------------------------------------------------------------------
        private void tbar_Scroll(object sender, EventArgs e)
        {
            //carSim.carAccVal = (double)tbar_Speed.Value * 0.1;
        }

        int stX, stY;
        int msX, msY;

        private bool wldMoveFlg = false;
        private bool viewMoveFlg = false;
        //private bool wldRotFlg = false;

        private void picbox_SimArea_MouseDown(object sender, MouseEventArgs e)
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
                //bgMoveFlg = true;
            }

            // 移動前の座標を記憶
            msX = e.X;
            msY = e.Y;

            if (viewMoveFlg)
            {
                stX = (int)viewX;
                stY = (int)viewY;
                //stAng = ViewScale;
            }

        }

        private void picbox_SimArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (wldMoveFlg)
            {
                /*
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
                }*/
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
                        //ViewScale = (float)stAng + (float)difX * 0.1f;
                        //SetViewScale(ViewScale);
                    }
                    else
                    {
                        viewX = stX + -difX;
                        viewY = stY + -difY;
                    }
                    //UpdateTRG = true;
                }
            }

        }

        private void picbox_SimArea_MouseUp(object sender, MouseEventArgs e)
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
                //bgMoveFlg = false;
            }

            // Mapの時差更新 OFF
            //UpdateTRG = true;

        }

        // マウスコントローラ
        bool bMsControlON = false;
        private void picbox_MsController_MouseDown(object sender, MouseEventArgs e)
        {
            System.Drawing.Point mp = picbox_MsController.PointToScreen(new System.Drawing.Point(picbox_MsController.Width / 2, picbox_MsController.Height / 2));

            //マウスポインタの位置を設定する
            System.Windows.Forms.Cursor.Position = mp;
            //System.Drawing.Point cp = picbox_MsController.PointToClient(mp);

            msX = picbox_MsController.Width / 2;
            msY = picbox_MsController.Height / 2;
            bMsControlON = true;
        }

        private void picbox_MsController_MouseMove(object sender, MouseEventArgs e)
        {
            if (bMsControlON)
            {
                int difX = e.X - msX;
                int difY = e.Y - msY;

                //マウスポインタの位置を枠からでないようにする
                {
                    int newMsX = e.X;
                    int newMsY = e.Y;
                    bool bAjastMs = false;
                    if (newMsX < 0) { newMsX = 0; bAjastMs = true; }
                    if (newMsX > picbox_MsController.Width) { newMsX = picbox_MsController.Width; bAjastMs = true; }
                    if (newMsY < 0) { newMsY = 0; bAjastMs = true; }
                    if (newMsY > picbox_MsController.Height) { newMsY = picbox_MsController.Height; bAjastMs = true; }

                    if (bAjastMs)
                    {
                        System.Drawing.Point mp = picbox_MsController.PointToScreen(new System.Drawing.Point(newMsX, newMsY));
                        System.Windows.Forms.Cursor.Position = mp;
                    }
                }

                carSim.carHandleAng = ((double)difX / (picbox_MsController.Width / 2.0)) * 30.0;
                carSim.carAccVal = (double)-difY / (picbox_MsController.Height / 2.0);
            }
        }

        private void picbox_MsController_MouseUp(object sender, MouseEventArgs e)
        {
            carSim.carAccVal = 0.0;
            carSim.carHandleAng = 0.0;
            bMsControlON = false;
        }

        /// <summary>
        /// ビューサイズ変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tBarScale_Scroll(object sender, EventArgs e)
        {
            viewScale = 1.0 + tBarScale.Value*0.5;
            lbl_ScaleVal.Text = viewScale.ToString("F1");
        }

    }
}
