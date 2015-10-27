using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;

namespace LocSampLogViewer
{
    public partial class MainForm : Form
    {
        public LocSumpLogReader LogReader = new LocSumpLogReader();
        public LocSumpLogData[] LogData;
        public LocSumpLogData[] TcpLogData;

        LocSumpLogData nowLsData = new LocSumpLogData();
        LocSumpLogData drawLsData = new LocSumpLogData();

        Bitmap WorldMapBmp;       // マップ
        Bitmap SubWindowBmp;        // サブ

        public static double LRF_ScaleOfPixel = 1.0 / 100.0; // 10cm = 1Pixel [mm]からピクセルサイズへの変換

        public MainForm()
        {
            InitializeComponent();

            WorldMapBmp = new Bitmap(PicBox_Map.Width, PicBox_Map.Height);
            PicBox_Map.Image = WorldMapBmp;

            SubWindowBmp = new Bitmap(PicBox_Sub.Width, PicBox_Sub.Height);
            PicBox_Sub.Image = SubWindowBmp;

            ViewTransX = PicBox_Map.Width / 2.0f;
            ViewTransY = PicBox_Map.Height / 2.0f;

            tmr_Update.Enabled = true;
            UpdateSubWindow();
        }

        /// <summary>
        /// VehicleRunnder ログファイル選択ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_LoadLogDlg_Click(object sender, EventArgs e)
        {
            OpenFileDialog fDlg = new OpenFileDialog();

            fDlg.Filter = "LocSamp AutoRunningLog (*.log)|*.log";

            var Result = fDlg.ShowDialog();
            if (Result != System.Windows.Forms.DialogResult.OK) return;

            Tb_LogFileName.Text = fDlg.FileName;

            LogReader.OpenFile(fDlg.FileName);
            LogData = LogReader.getScanData();

            if( null == LogData )
            {
                MessageBox.Show("対応しない形式のログデータです");
                return;
            }
            if( LogData.Length == 0 )
            {
                MessageBox.Show("読み込み失敗");
                return;
            }

            ScrlBar_Time.Maximum = LogData.Length;
            ScrlBar_Time.Value = 0;
            ShowLogData(0);
        }

        /// <summary>
        /// ログデータ表示
        /// </summary>
        /// <param name="index"></param>
        private void ShowLogData(int index)
        {
            if( null == LogData ) return;

            LocSumpLogData lsData = LogData[index];
            nowLsData = lsData;

            // 時間
            {
                long hour = lsData.ms/(60*60*1000);
                long min = (lsData.ms-(hour*(60*60*1000)))/(60*1000);
                double ms = (lsData.ms - (((hour * (60 * 60)) + min*60) * 1000))/1000.0;
                Lbl_Time.Text = hour.ToString("D2") + ":" + min.ToString("D2") + ":" + ms.ToString("00.0000");
            }

            // Send Handle,ACC
            if (lsData.bSend)
            {
                Lbl_Handle.Enabled = true;
                Lbl_ACC.Enabled = true;

                Lbl_Handle.Text = lsData.sendHandle.ToString("f3");
                Lbl_ACC.Text = lsData.sendACC.ToString("f3");

                drawLsData.sendHandle = lsData.sendHandle;
                drawLsData.sendACC = lsData.sendACC;
            }
            else
            {
                Lbl_Handle.Enabled = false;
                Lbl_ACC.Enabled = false;
                //Lbl_Handle.Text = "ND";
                //Lbl_ACC.Text = "ND";
            }

            Lbl_RE.Text = "ND";

            // Compus
            if (lsData.bCompus)
            {
                Lbl_Compus.Enabled = true;
                Lbl_Compus.Text = lsData.CompusDir.ToString();

                drawLsData.CompusDir = lsData.CompusDir;
            }
            else
            {
                Lbl_Compus.Enabled = false;
                //Lbl_Compus.Text = "ND";
            }

            // GPS
            if (lsData.bGPS)
            {
                Lbl_GPS_X.Enabled = true;
                Lbl_GPS_Y.Enabled = true;
                Lbl_GPS_X.Text = lsData.GPSLandX.ToString();
                Lbl_GPS_Y.Text = lsData.GPSLandY.ToString();

                drawLsData.GPSLandX = lsData.GPSLandX;
                drawLsData.GPSLandY = lsData.GPSLandY;
            }
            else
            {
                Lbl_GPS_X.Enabled = false;
                Lbl_GPS_Y.Enabled = false;
                //Lbl_GPS_X.Text = "ND";
            }

            // REPlot
            if (lsData.bREPlot)
            {
                Lbl_REPlotX.Enabled = true;
                Lbl_REPlotY.Enabled = true;
                Lbl_REPlotDir.Enabled = true;
                Lbl_REPlotX.Text = lsData.REPlotX.ToString("f3");
                Lbl_REPlotY.Text = lsData.REPlotY.ToString("f3");
                Lbl_REPlotDir.Text = lsData.REPlotDir.ToString("f3");

                drawLsData.REPlotX = lsData.REPlotX;
                drawLsData.REPlotY = lsData.REPlotY;
                drawLsData.REPlotDir = lsData.REPlotDir;
            }
            else
            {
                Lbl_REPlotX.Enabled = false;
                Lbl_REPlotY.Enabled = false;
                Lbl_REPlotDir.Enabled = false;
                //Lbl_REPlotX.Text = "ND";
            }


            // R1
            if (lsData.bR1)
            {
                Lbl_R1_X.Enabled = true;
                Lbl_R1_Y.Enabled = true;
                Lbl_R1_Dir.Enabled = true;
                Lbl_R1_X.Text = lsData.R1_X.ToString("f3");
                Lbl_R1_Y.Text = lsData.R1_Y.ToString("f3");
                Lbl_R1_Dir.Text = lsData.R1_Dir.ToString("f3");

                drawLsData.R1_X = lsData.R1_X;
                drawLsData.R1_Y = lsData.R1_Y;
                drawLsData.R1_Dir = lsData.R1_Dir;
            }
            else
            {
                Lbl_R1_X.Enabled = false;
                Lbl_R1_Y.Enabled = false;
                Lbl_R1_Dir.Enabled = false;
                //Lbl_R1_X.Text = "ND";
            }

            // E1
            if (lsData.bE1)
            {
                Lbl_E1_X.Enabled = true;
                Lbl_E1_Y.Enabled = true;
                Lbl_E1_Dir.Enabled = true;
                Lbl_E1_X.Text = lsData.E1_X.ToString("f3");
                Lbl_E1_Y.Text = lsData.E1_Y.ToString("f3");
                Lbl_E1_Dir.Text = lsData.E1_Dir.ToString("f3");

                drawLsData.E1_X = lsData.E1_X;
                drawLsData.E1_Y = lsData.E1_Y;
                drawLsData.E1_Dir = lsData.E1_Dir;
            }
            else
            {
                Lbl_E1_X.Enabled = false;
                Lbl_E1_Y.Enabled = false;
                Lbl_E1_Dir.Enabled = false;
                //Lbl_R1_X.Text = "ND";
            }

            UpdateSubWindow();
        }


        /// <summary>
        /// サブウィンドウ更新
        /// </summary>
        private void UpdateSubWindow()
        {
            Graphics g = Graphics.FromImage(SubWindowBmp);
            g.FillRectangle(Brushes.Black, 0, 0, SubWindowBmp.Width, SubWindowBmp.Height);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            Font fnt = new Font("MS UI Gothic", 9);

            if (null != drawLsData)
            {
                
                {
                    // Handle
                    int stX = 40;
                    int stY = 5;
                    int Wd = 200;
                    int Ht = 15;

                    float handleVal = (float)((Wd / 2) * (drawLsData.sendHandle));
                    if (handleVal > 0)
                    {
                        g.FillRectangle(Brushes.Red, stX + Wd / 2, stY, handleVal, Ht);
                    }
                    else
                    {
                        g.FillRectangle(Brushes.Red, stX + Wd / 2 + handleVal, stY, -handleVal, Ht);
                    }
                    g.DrawRectangle(Pens.White, stX, stY, Wd, Ht);
                    g.DrawLine(Pens.White, stX + Wd / 2, stY, stX + Wd / 2, stY + Ht);
                    g.DrawString("Handle", fnt, Brushes.White, stX-38, stY + Ht - fnt.GetHeight());

                    // ACC
                    stX = 40;
                    stY = 30;
                    Wd = 10;
                    Ht = 40;

                    float accVal = (float)(Ht * drawLsData.sendACC);
                    g.FillRectangle(Brushes.Red, stX, stY + Ht - accVal, Wd, accVal);
                    g.DrawRectangle(Pens.White, stX, stY, Wd, Ht);
                    g.DrawString("Acc", fnt, Brushes.White, stX - 35, stY + Ht-fnt.GetHeight());

                }

                // Compus
                {
                    DrawMaker(g, Brushes.Red, 90, 50, -drawLsData.CompusDir, 12);
                    g.DrawString("Compus", fnt, Brushes.White, 90-25, 50+14);
                }

                // RE Dir
                {
                    DrawMaker(g, Brushes.Purple, 150, 50, -drawLsData.REPlotDir * 180.0 / Math.PI, 12);
                    g.DrawString("R.E.Dir", fnt, Brushes.White, 150 - 25, 50 + 14);
                }
            }
            fnt.Dispose();
            g.Dispose();

            PicBox_Sub.Invalidate();
        }


        private float ViewScale = 1.0f;
        private float ViewTransX = 0.0f;
        private float ViewTransY = 0.0f;
        /// <summary>
        /// 
        /// </summary>
        private void UpdateMapWindow()
        {
            Graphics g = Graphics.FromImage(WorldMapBmp);
            g.FillRectangle(Brushes.Black, 0, 0, WorldMapBmp.Width, WorldMapBmp.Height);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            
            g.ResetTransform();
            // View
            g.ScaleTransform(ViewScale, ViewScale, MatrixOrder.Append);
            g.TranslateTransform(ViewTransX, ViewTransY, MatrixOrder.Append);

            DrawGuideLine(g);

            if (null != LogData)
            {
                // REPlot
                DrawREconderData(g,2);

                {
                    DrawMaker(g, Brushes.Purple,
                              drawLsData.REPlotX * LRF_ScaleOfPixel,
                              -drawLsData.REPlotY * LRF_ScaleOfPixel,       // - NorthUP
                              -drawLsData.REPlotDir*180.0/Math.PI,
                              12);
                }
            }

            if( null != TcpLogData )
            {
                DrawGPSData(g, TcpLogData);
            }


            g.Dispose();

            PicBox_Map.Invalidate();
        }

        private void DrawMaker(Graphics g, Brush brush, double mkX, double mkY, double mkDir, double size)
        {
            //double nonScl = size / ViewScale;
            //if (nonScl <= 1.0) nonScl = 1.0;

            mkDir = mkDir - 90.0;

            var P1 = new PointF(
                (float)(mkX + size * -Math.Cos(mkDir * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin(mkDir * Math.PI / 180.0)));
            var P2 = new PointF(
                (float)(mkX + size * -Math.Cos((mkDir - 150) * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin((mkDir - 150) * Math.PI / 180.0)));
            var P3 = new PointF(
                (float)(mkX + size * -Math.Cos((mkDir + 150) * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin((mkDir + 150) * Math.PI / 180.0)));

            g.FillPolygon(brush, new PointF[] { P1, P2, P3 });
        }

        /// <summary>
        /// ロータリーエンコーダの軌跡を描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="xyWR"></param>
        /// <param name="xyWL"></param>
        private void DrawREconderData(Graphics g, int mode )
        {
            int drawNum = 100;  // 色変えの単位

            int px = 0;
            int py = 0;

            for (int i = 0; i < LogData.Length / drawNum; i++)
            {
                int n = i * drawNum;
                int en = LogData.Length - n;
                if (en > drawNum) en = drawNum;

                Point[] drawLinePos = new Point[en+1];

                drawLinePos[0] = new Point(px, py);

                for (int iR = 0; iR < en; iR++)
                {
                    if (mode == 0)
                    {
                        if (LogData[iR + n].bR1)
                        {
                            px = (int)LogData[iR + n].R1_X;
                            py = (int)LogData[iR + n].R1_Y;
                        }
                    }
                    else if (mode == 1)
                    {
                        //V1
                    }
                    else if (mode == 2)
                    {
                        if (LogData[iR + n].bREPlot)
                        {
                            px = (int)(LogData[iR + n].REPlotX * LRF_ScaleOfPixel);
                            py = (int)(-LogData[iR + n].REPlotY * LRF_ScaleOfPixel);     // - NorthUP
                        }
                    }

                    drawLinePos[iR+1] = new Point(px, py);
                }

                // 色変え
                if (i % 2 == 0)
                {
                    g.DrawLines(Pens.DarkMagenta, drawLinePos);
                }
                else
                {
                    g.DrawLines(Pens.BlueViolet, drawLinePos);
                }
            }
        }


        /// <summary>
        /// GPS軌跡描画
        /// </summary>
        /// <param name="g"></param>
        private void DrawGPSData(Graphics g, LocSumpLogData[] lcData )
        {
            int drawNum = 10;  // 色変えの単位

            int px = 0;
            int py = 0;

            double fstX, fstY;
            bool bFst = true;
            fstX = fstY = 0.0;

            for (int i = 0; i < lcData.Length / drawNum; i++)
            {
                int n = i * drawNum;
                int en = lcData.Length - n;
                if (en > drawNum) en = drawNum;

                Point[] drawLinePos = new Point[en+1];

                // 前回のポイントから途切れないように。
                drawLinePos[0] = new Point(px, py);

                for (int iR = 0; iR < en; iR++)
                {
                    if (lcData[iR + n].bGPS)
                    {
                        if (bFst)
                        {
                            bFst = false;
                            fstX = lcData[iR + n].GPSLandX;
                            fstY = lcData[iR + n].GPSLandY;
                        }

                        px = (int)((lcData[iR + n].GPSLandX - fstX) * LRF_ScaleOfPixel);
                        py = (int)((lcData[iR + n].GPSLandY - fstY) * LRF_ScaleOfPixel);
                    }

                    drawLinePos[iR+1] = new Point(px, py);
                }

                // 色変え
                if (i % 2 == 0)
                {
                    g.DrawLines(Pens.Green, drawLinePos);
                }
                else
                {
                    g.DrawLines(Pens.Lime, drawLinePos);
                }
            }
        }

        /// <summary>
        /// ガイドライン
        /// </summary>
        /// <param name="g"></param>
        private void DrawGuideLine(Graphics g)
        {
            int GideLength = 5000;  // ガイドラインの長さ(ドット)

            g.DrawLine(Pens.DarkGray, -GideLength, 0, GideLength, 0);
            g.DrawLine(Pens.DarkGray, 0, -GideLength, 0, GideLength);

            int gridPix = (int)(10000.0 * LRF_ScaleOfPixel);    // 10mごと
            int nonScl = (int)(5.0 / ViewScale);
            for (int i = 0; i <= (GideLength * 2 / gridPix); i++)
            {
                g.DrawLine(Pens.DarkGray, -GideLength + (gridPix * i), -nonScl, -GideLength + (gridPix * i), nonScl);
                g.DrawLine(Pens.DarkGray, -nonScl, -GideLength + (gridPix * i), nonScl, -GideLength + (gridPix * i));
            }
        }


        long playMS = 0;
        int playIdx = 0;
        /// <summary>
        /// タイマー更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmr_Update_Tick(object sender, EventArgs e)
        {
            if (Btn_Play.Checked)
            {
                // 再生時間分ログデータのインデックスを進める
                playMS += tmr_Update.Interval;
                while (playIdx < LogData.Length-1 && LogData[playIdx].ms < playMS)
                {
                    playIdx++;
                }

                if (playIdx >= LogData.Length - 1)
                {
                    Btn_Play.Checked = false;
                    playIdx = LogData.Length - 1;
                }
                ScrlBar_Time.Value = playIdx;
            }

            Lbl_ViewPosScale.Text = ViewTransX.ToString("0000")+","+ViewTransY.ToString("0000")+"("+(ViewScale*100.0f).ToString("000")+"%)";
            UpdateMapWindow();
        }

        private void btn_Rewind_Click(object sender, EventArgs e)
        {
            Btn_Play.Checked = false;
            playIdx = 0;
            playMS = 0;
            ScrlBar_Time.Value = playIdx;
        }

        private void ScrlBar_Time_Scroll(object sender, ScrollEventArgs e)
        {
            if (null != LogData)
            {
                playIdx = ScrlBar_Time.Value;
                playMS = LogData[playIdx].ms;
            }

            Btn_Play.Checked = false;
        }

        private void ScrlBar_Time_ValueChanged(object sender, EventArgs e)
        {
            ShowLogData(ScrlBar_Time.Value);
        }

        private void ScrlBar_ViewScale_ValueChanged(object sender, EventArgs e)
        {
            ViewScale = (float)ScrlBar_ViewScale.Value / 100.0f;
        }


        // Mouse ---------------------------------------------------------------
        int msX, msY;
        int stX, stY;
        bool viewMoveFlg = false;
        private void PicBox_Map_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                // オブジェクト移動
                viewMoveFlg = true;
            }

            // 移動前の座標を記憶
            msX = e.X;
            msY = e.Y;

            if (viewMoveFlg)
            {
                stX = (int)ViewTransX;
                stY = (int)ViewTransY;
            }
        }

        private void PicBox_Map_MouseMove(object sender, MouseEventArgs e)
        {
            if (viewMoveFlg)
            {
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
                        ViewTransX = stX + difX;
                        ViewTransY = stY + difY;
                    }
                    //UpdateTRG = true;
                }
            }

        }

        private void PicBox_Map_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                viewMoveFlg = false;
            }

        }

        private void Btn_LoadTCPLogDlg_Click(object sender, EventArgs e)
        {
            OpenFileDialog fDlg = new OpenFileDialog();

            fDlg.Filter = "TCPLogger (*.log)|*.log";

            var Result = fDlg.ShowDialog();
            if (Result != System.Windows.Forms.DialogResult.OK) return;

            Tb_TCPLogFileName.Text = fDlg.FileName;

            TCP_LogReader tcpLogReader = new TCP_LogReader();
            tcpLogReader.OpenFile(fDlg.FileName);
            TcpLogData = tcpLogReader.getScanData();
            tcpLogReader.CloseFile();

            if (null == TcpLogData)
            {
                MessageBox.Show("対応しない形式のログデータです");
                return;
            }
            if (TcpLogData.Length == 0)
            {
                MessageBox.Show("読み込み失敗");
                return;
            }

            ScrlBar_Time.Maximum = TcpLogData.Length;
            ScrlBar_Time.Value = 0;
            ShowLogData(0);
        }

        private void Btn_GPSDlg_Click(object sender, EventArgs e)
        {
            OpenFileDialog fDlg = new OpenFileDialog();

            fDlg.Filter = "GPS Log (*.log)|*.log";

            var Result = fDlg.ShowDialog();
            if (Result != System.Windows.Forms.DialogResult.OK) return;

            Tb_GPSFileName.Text = fDlg.FileName;

            GPS_LogReader gpsReader = new GPS_LogReader();
            gpsReader.OpenFile(fDlg.FileName);
            TcpLogData = gpsReader.getScanData();
            gpsReader.CloseFile();

            if (null == TcpLogData)
            {
                MessageBox.Show("対応しない形式のログデータです");
                return;
            }
            if (TcpLogData.Length == 0)
            {
                MessageBox.Show("読み込み失敗");
                return;
            }

            ScrlBar_Time.Maximum = TcpLogData.Length;
            ScrlBar_Time.Value = 0;
            ShowLogData(0);
        }

    }
}
