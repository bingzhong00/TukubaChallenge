// Copyright (c) 2011 TAJIMA Yoshiyuki 
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
// 
//   1. Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//   2. Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE FREEBSD PROJECT ``AS IS'' AND ANY EXPRESS OR 
// IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
// MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
// SHALL THE FREEBSD PROJECT OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
// OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE 
// OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
// OF THE POSSIBILITY OF SUCH DAMAGE.
// 
// The views and conclusions contained in the software and documentation are those 
// of the authors and sho:]uld not be interpreted as representing official policies, 
// either expressed or implied, of the FreeBSD Project.


#define LOGWRITE_MODE   // ログファイル出力
#define LOGIMAGE_MODE   // イメージログ出力

#define GPSLOG_OUTPUT   //  GPSログ出力
//#define EMULATOR_MODE   // エミュレートモード

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Drawing.Drawing2D;

using LocationPresumption;
using CersioIO;
using SCIP_library;
using Axiom.Math;

namespace VehicleRunner
{
    public partial class VehicleRunnerForm : Form
    {
        public static string saveLogFname;
        public static string saveGPSLogFname;

        LocPreSumpSystem LocSys;
        CersioCtrl CersioCt;

        Bitmap worldMapBmp;

        UsbIOport usbGPS;

        // エミュレーションモード
#if EMULATOR_MODE
        public static bool IsEmurateMode = true;
#else
        public static bool IsEmurateMode = false;
#endif

        Random Rand = new Random();
        bool LocalizeFlag;

        double LRFViewScale = 1.0;

        private int selPicboxLRFmode = 1;

        /// <summary>
        /// 位置補正指示トリガー
        /// </summary>
        private bool bLocRivisionTRG = false;

        /// <summary>
        /// 
        /// </summary>
        public VehicleRunnerForm()
        {
            InitializeComponent();


            saveLogFname = GetTimeStampFileName("./Log/LocSampLog", ".log");
            saveGPSLogFname = GetTimeStampFileName("./Log/GPSLog", ".log");

            // ログファイルディレクトリ確認
            {
                string logDir = Path.GetDirectoryName(saveLogFname);
                if (!File.Exists(logDir))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(logDir);
                    }
                    catch
                    {
                    }
                }
            }

            // セルシオコントローラ初期化
            CersioCt = new CersioCtrl();
            CersioCt.Start();

            if (!IsEmurateMode) CersioCt.ConnectBoxPC();

            // 自己位置推定初期化
            LocSys = new LocPreSumpSystem();

            // マップ情報設定
            //  マップファイル名、実サイズの横[mm], 実サイズ縦[mm] (北向き基準)
            LocSys.InitWorld( RootingData.MapFileName, RootingData.RealWidth, RootingData.RealHeight);
            LocSys.SetStartPostion( (int)RootingData.startPosition.x,
                                    (int)RootingData.startPosition.y,
                                    RootingData.startDir );

            // REをリセット
            CersioCt.RE_Reset(RootingData.startPosition.x, RootingData.startPosition.y, RootingData.startDir);

            worldMapBmp = MakeScaledWorldMap(LocSys.worldMap.mapBmp);

            // LRF 入力スケール調整反映
            //btm_LRFScale_Click(null, null);
            tb_LRFScale_TextChanged(null, null);

            // 自己位置推定計算
            cb_LocationPresumption.Checked = LocalizeFlag = false;

            // エミュレーションモード表示
            lbl_EmurateMode.Visible = IsEmurateMode;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.SetDesktopLocation(0, 0);

            // USB
            {
                // すべてのシリアル・ポート名を取得する
                string[] ports = System.IO.Ports.SerialPort.GetPortNames();

                // シリアルポートを毎回取得して表示するために表示の度にリストをクリアする
                cb_UsbSirial.Items.Clear();

                foreach (string port in ports)
                {
                    // 取得したシリアル・ポート名を出力する
                    cb_UsbSirial.Items.Add(port);
                }

                if (cb_UsbSirial.Items.Count > 0)
                {
                    cb_UsbSirial.SelectedIndex = 0;
                }
            }

            LocPreSumpSystem.bRivisonGPS = cb_RivisonGPS.Checked;
            LocPreSumpSystem.bRivisonPF = cb_RivisionPF.Checked;
            LocPreSumpSystem.bTimeRivision = cb_TimeRivision.Checked;

            // 画面更新
            PictureUpdate();

            // ハードウェア更新タイマ起動
            tm_UpdateHw.Enabled = true;
            tm_SendCom.Enabled = true;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            LocSys.Close();
            CersioCt.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DeleteTimer();
            tm_SendCom.Enabled = false;
            tm_UpdateHw.Enabled = false;
            tm_LocUpdate.Enabled = false;

#if LOGIMAGE_MODE
            // 軌跡ログ出力
            if (!string.IsNullOrEmpty(saveLogFname))
            {
                MarkPoint tgtMaker = null;

                // 次の目的地取得
                if (null != CersioCt)
                {
                    int tgtPosX = 0;
                    int tgtPosY = 0;
                    double dir = 0;

                    CersioCt.BrainCtrl.RTS.getNowTarget(ref tgtPosX, ref tgtPosY);
                    CersioCt.BrainCtrl.RTS.getNowTargetDir(ref dir);

                    tgtMaker = new MarkPoint(tgtPosX, tgtPosY, dir);
                }

                Bitmap bmp = LocSys.MakeMakerLogBmp(false, tgtMaker);
                if (null != bmp)
                {
                    // 画像ファイル保存
                    string saveImageLogFname = Path.ChangeExtension(saveLogFname, "png");
                    bmp.Save(saveImageLogFname, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
#endif
        }

        // 

        /// <summary>
        /// タイムスタンプファイル作成
        /// </summary>
        /// <returns></returns>
        public string GetTimeStampFileName(string strPrev, string strExt )
        {
            return strPrev + DateTime.Now.ToString("yyyyMMdd_HHmmss") + strExt;
        }



        // Draw -------------------------------------------------------------------------------------------
        Font drawFont = new Font("MS UI Gothic", 16, FontStyle.Bold);

        int selAreaMapMode = 0;
        int areaMapDrawCnt = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picbox_AreaMap_Paint(object sender, PaintEventArgs e)
        {
            // 書き換えＢＭＰ（追加障害物）描画
            Graphics g = e.Graphics;

            if (selAreaMapMode == 0)
            {
                // エリアマップ描画
                //g.FillRectangle(Brushes.Black, 0, 0, picbox_AreaMap.Width, picbox_AreaMap.Height);

                g.DrawImage(LocSys.AreaOverlayBmp, 0, 0);


                // ターゲット描画
                if (null != CersioCt)
                {
                    int tgtPosX, tgtPosY;
                    double dir = 0;
                    tgtPosX = tgtPosY = 0;
                    float olScale = (float)LocSys.AreaOverlayBmp.Width / (float)LocSys.AreaBmp.Width;

                    CersioCt.BrainCtrl.RTS.getNowTarget(ref tgtPosX, ref tgtPosY);
                    CersioCt.BrainCtrl.RTS.getNowTargetDir(ref dir);

                    DrawMaker(g, olScale, new MarkPoint(LocSys.worldMap.GetAreaX(tgtPosX), LocSys.worldMap.GetAreaY(tgtPosY), dir), Brushes.GreenYellow, 8);
                }
            }
            else if (selAreaMapMode == 1)
            {
                // 全体マップ描画
                float viewScale;

                g.FillRectangle(Brushes.Black, 0, 0, picbox_AreaMap.Width, picbox_AreaMap.Height);

                if (((float)LocSys.worldMap.WorldSize.w / (float)picbox_AreaMap.Width) < ((float)LocSys.worldMap.WorldSize.h / (float)picbox_AreaMap.Height))
                {
                    viewScale = (float)(1.0 / ((float)LocSys.worldMap.WorldSize.h / (float)picbox_AreaMap.Height));
                }
                else
                {
                    viewScale = (float)(1.0 / ((float)LocSys.worldMap.WorldSize.w / (float)picbox_AreaMap.Width));
                }

                //g.ResetTransform();
                //g.TranslateTransform(-ctrX, -ctrY, MatrixOrder.Append);
                //g.RotateTransform((float)layer.wAng, MatrixOrder.Append);
                //g.ScaleTransform(viewScale, viewScale, MatrixOrder.Append);

                if (null != worldMapBmp)
                {
                    g.DrawImage(worldMapBmp, 0, 0);
                }

                g.ResetTransform();
                int mkSize = 8;
                // 描画順を常にかえて、重なっても見えるようにする
                for (int i = 0; i < 4; i++)
                {
                    switch ((i + areaMapDrawCnt) % 4)
                    {
                        case 0:
                            // RE想定ロボット位置描画
                            DrawMaker(g, viewScale, new MarkPoint(LocSys.worldMap.GetWorldX(LocSys.E1.X), LocSys.worldMap.GetWorldY(LocSys.E1.Y), LocSys.E1.Theta), Brushes.Purple, mkSize);
                            break;
                        case 1:
                            // PF想定ロボット位置描画
                            DrawMaker(g, viewScale, new MarkPoint(LocSys.worldMap.GetWorldX(LocSys.V1.X), LocSys.worldMap.GetWorldY(LocSys.V1.Y), LocSys.V1.Theta), Brushes.Cyan, mkSize);
                            break;
                        case 2:
                            // 実ロボット想定位置描画
                            DrawMaker(g, viewScale, new MarkPoint(LocSys.worldMap.GetWorldX(LocSys.R1.X), LocSys.worldMap.GetWorldY(LocSys.R1.Y), LocSys.R1.Theta), Brushes.Red, mkSize);
                            break;
                        case 3:
                            // GPS位置描画
                            DrawMaker(g, viewScale, new MarkPoint(LocSys.worldMap.GetWorldX(LocSys.G1.X), LocSys.worldMap.GetWorldY(LocSys.G1.Y), LocSys.G1.Theta), Brushes.Green, mkSize);
                            break;
                    }
                }

                // エリア描画
                g.DrawRectangle(Pens.Red,
                                 (LocSys.worldMap.WldOffset.x * viewScale),
                                 (LocSys.worldMap.WldOffset.y * viewScale),
                                 (LocSys.worldMap.GridSize.w * viewScale),
                                 (LocSys.worldMap.GridSize.h * viewScale));
            }

            g.ResetTransform();

            // Info
            DrawString(g, 0, drawFont.Height * 0,
                       "R1 X:" + ((int)LocSys.worldMap.GetWorldX(LocSys.R1.X)).ToString("D4") +
                       ",Y:" + ((int)LocSys.worldMap.GetWorldY(LocSys.R1.Y)).ToString("D4") +
                       ",角度:" + ((int)LocSys.R1.Theta).ToString("D3"),
                       Brushes.Red, Brushes.Black);
            /*
            DrawString(g, 0, drawFont.Height * 1,
                       "Compass:" + CersioCt.hwCompass.ToString("D3") + "/ ReDir:" + ((int)(CersioCt.hwREDir)).ToString("D3") +
                       ",ReX:" + ((int)(CersioCt.hwREX)).ToString("D4") + ",Y:" + ((int)(CersioCt.hwREY)).ToString("D4"),
                       Brushes.Blue, Brushes.White);
            */
            DrawString(g, 0, drawFont.Height * 1,
                       "RunCnt:" + updateHwCnt.ToString("D8") + "/ Goal:" + (CersioCt.goalFlg ? "TRUE" : "FALSE" + "/ Cp:" + CersioCt.BrainCtrl.RTS.GetNowCheckPointIdx().ToString()),
                       Brushes.Blue, Brushes.White);

            DrawString(g, 0, drawFont.Height * 2,
                       "LocProc:" + LocSys.swCNT_Update.ElapsedMilliseconds + "ms /Draw:" + LocSys.swCNT_Draw.ElapsedMilliseconds + "ms /MRF:" + LocPreSumpSystem.swCNT_MRF.ElapsedMilliseconds + "ms",
                       Brushes.Blue, Brushes.White);


            areaMapDrawCnt++;

        }

        /// <summary>
        /// picbox_AreaMapサイズのワールドマップ
        /// </summary>
        /// <param name="worldBMP"></param>
        /// <returns></returns>
        private Bitmap MakeScaledWorldMap(Bitmap worldBMP )
        {
            float viewScale = 1.0f;

            if (((float)worldBMP.Width / (float)picbox_AreaMap.Width) < ((float)worldBMP.Height / (float)picbox_AreaMap.Height))
            {
                viewScale = (float)(1.0 / ((float)worldBMP.Height / (float)picbox_AreaMap.Height));
            }
            else
            {
                viewScale = (float)(1.0 / ((float)worldBMP.Width / (float)picbox_AreaMap.Width));
            }

            Bitmap scaledBmp = new Bitmap( (int)(viewScale * worldBMP.Width + 0.5), (int)(viewScale * worldBMP.Height+0.5));
            Graphics g = Graphics.FromImage(scaledBmp);

            g.ResetTransform();
            //g.TranslateTransform(-ctrX, -ctrY, MatrixOrder.Append);
            //g.RotateTransform((float)layer.wAng, MatrixOrder.Append);
            g.ScaleTransform(viewScale, viewScale, MatrixOrder.Append);

            g.DrawImage(worldBMP, 0, 0);

            g.Dispose();

            return scaledBmp;
        }


        /// <summary>
        /// マーカー描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="fScale"></param>
        /// <param name="robot"></param>
        /// <param name="brush"></param>
        /// <param name="size"></param>
        private void DrawMaker(Graphics g, float fScale, MarkPoint robot, Brush brush, int size)
        {
            double mkX = robot.X * fScale;
            double mkY = robot.Y * fScale;
            double mkDir = robot.Theta - 90.0;

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

        private void DrawString(Graphics g, int x, int y, string str, Brush brush, Brush bkBrush )
        {
            g.DrawString(str, drawFont, bkBrush, x + 1, y);
            g.DrawString(str, drawFont, bkBrush, x - 1, y);
            g.DrawString(str, drawFont, bkBrush, x, y - 1);
            g.DrawString(str, drawFont, bkBrush, x, y + 1);
            g.DrawString(str, drawFont, brush, x, y);
        }

        /// <summary>
        /// 表示切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picbox_AreaMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                // エリア、ワールドマップ切り替え
                selAreaMapMode = (++selAreaMapMode) % 2;
            }

            if (IsEmurateMode)
            {
                // マーカー位置移動
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    // マウスクリックでR1,E1,V1の位置を移動
                    if (selAreaMapMode == 0)
                    {
                        float viewScale = 1.0f;

                        viewScale = (float)LocSys.worldMap.GridSize.w / (float)picbox_AreaMap.Width;

                        if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                        {
                            LocSys.E1.X = e.X * viewScale;
                            LocSys.E1.Y = e.Y * viewScale;
                        }
                        else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                        {
                            LocSys.V1.X = e.X * viewScale;
                            LocSys.V1.Y = e.Y * viewScale;
                        }
                        else if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt)
                        {
                            LocSys.G1.X = e.X * viewScale;
                            LocSys.G1.Y = e.Y * viewScale;

                            // GPSの値があれば、GPSの位置情報もリセット
                            if (LocPreSumpSystem.bEnableGPS)
                            {
                                LocPreSumpSystem.SetStartGPS(CersioCt.hwGPS_LandX,
                                                              CersioCt.hwGPS_LandY,
                                                              (int)(LocSys.worldMap.GetWorldX(LocSys.G1.X) + 0.5),
                                                              (int)(LocSys.worldMap.GetWorldY(LocSys.G1.Y) + 0.5), false);
                            }
                        }
                        else
                        {
                            LocSys.R1.X = e.X * viewScale;
                            LocSys.R1.Y = e.Y * viewScale;
                        }
                    }
                    else if (selAreaMapMode == 1)
                    {
                        float viewScale = 1.0f;

                        if (((float)LocSys.worldMap.WorldSize.w / (float)picbox_AreaMap.Width) < ((float)LocSys.worldMap.WorldSize.h / (float)picbox_AreaMap.Height))
                        {
                            viewScale = (float)LocSys.worldMap.WorldSize.h / (float)picbox_AreaMap.Height;
                        }
                        else
                        {
                            viewScale = (float)LocSys.worldMap.WorldSize.w / (float)picbox_AreaMap.Width;
                        }

                        if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                        {
                            LocSys.E1.X = LocSys.worldMap.GetAreaX((int)(e.X * viewScale));
                            LocSys.E1.Y = LocSys.worldMap.GetAreaY((int)(e.Y * viewScale));
                        }
                        else if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                        {
                            LocSys.V1.X = LocSys.worldMap.GetAreaX((int)(e.X * viewScale));
                            LocSys.V1.Y = LocSys.worldMap.GetAreaY((int)(e.Y * viewScale));
                        }
                        else if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt)
                        {
                            LocSys.G1.X = LocSys.worldMap.GetAreaX((int)(e.X * viewScale));
                            LocSys.G1.Y = LocSys.worldMap.GetAreaY((int)(e.Y * viewScale));

                            // GPSの値があれば、GPSの位置情報もリセット
                            if (LocPreSumpSystem.bEnableGPS)
                            {
                                LocPreSumpSystem.SetStartGPS(CersioCt.hwGPS_LandX,
                                                              CersioCt.hwGPS_LandY,
                                                              (int)(LocSys.worldMap.GetWorldX(LocSys.G1.X) + 0.5),
                                                              (int)(LocSys.worldMap.GetWorldY(LocSys.G1.Y) + 0.5), false);
                            }
                        }
                        else
                        {
                            LocSys.R1.X = LocSys.worldMap.GetAreaX((int)(e.X * viewScale));
                            LocSys.R1.Y = LocSys.worldMap.GetAreaY((int)(e.Y * viewScale));
                        }
                    }
                }
            }

        }


        // ---------------------------------------------------------------------------------------------------
        Font fntMini = new Font("MS UI Gothic", 9);

        /// <summary>
        /// LRFウィンドウデータ描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picbox_LRF_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            //g.ResetTransform();
            //g.TranslateTransform(-ctrX, -ctrY, MatrixOrder.Append);
            //g.RotateTransform((float)layer.wAng, MatrixOrder.Append);
            //g.ScaleTransform(scale, scale, MatrixOrder.Prepend);

            // LRF取得データを描画
            {
                int ctrX = picbox_LRF.Width / 2;
                int ctrY = picbox_LRF.Height * 6 / 8;

                float scale = 1.0f;

                // 背景色
                switch (selPicboxLRFmode)
                {
                    case 0:
                        ctrX = picbox_LRF.Width / 2;
                        ctrY = picbox_LRF.Height * 6 / 10;

                        picbox_LRF.BackColor = Color.Gray;//Color.White;
                        scale = 1.0f;
                        break;
                    case 1:
                        ctrX = picbox_LRF.Width / 2;
                        ctrY = picbox_LRF.Height * 6 / 8;

                        picbox_LRF.BackColor = Color.Black;

                        scale = 5.0f;

                        // EBSに反応があればズーム
                        //scale += ((float)CersioCt.BrainCtrl.EBS_CautionLv * 3.0f / (float)Brain.EBS_CautionLvMax);

                        // EHS
                        //if (CersioCt.BrainCtrl.EHS_Result != Brain.EHS_MODE.None) scale = 10.0f;
                        break;
                    case 2:
                        picbox_LRF.BackColor = Color.Black;
                        break;
                }

                if (selPicboxLRFmode == 0 || selPicboxLRFmode == 1)
                {
                    // ガイド描画
                    // 30mを2m区切りで描画
                    for (int i = 1; i <= 30 / 2; i++)
                    {
                        int cirSize = (int)((((i * 2000) * 2) / LocSys.RealToMapSclae) * scale);

                        g.DrawPie(Pens.LightGray,
                                            (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                            cirSize, cirSize,
                                            -135 - 90, 270);
                    }
                }

                switch (selPicboxLRFmode)
                {
                    case 0:
                        // LRF描画
                        if (LocSys.LRF_Data != null)
                        {
                            //double rScale = (1.0 / LocSys.RealToMapSclae);
                            double rPI = Math.PI / 180.0;
                            int pixelSize = 3;
                            double picScale = (LRFViewScale / 1000.0f) * scale;

                            // LRFの値を描画
                            for (int i = 0; i < LocSys.LRF_Data.Length; i++)
                            {
                                double val = LocSys.LRF_Data[i] * picScale;// *rScale;
                                double rad = (i - MapRangeFinder.AngleRangeHalf - 90) * rPI;

                                float x = (float)(ctrX + val * Math.Cos(rad));
                                float y = (float)(ctrY + val * Math.Sin(rad));
                                g.FillRectangle(Brushes.Yellow, x, y, pixelSize, pixelSize);
                            }
                        }

                        {
                            int iH = 80;

                            //g.FillRectangle(Brushes.Black, 0, picbox_LRF.Height - iH, picbox_LRF.Width, iH);
                            DrawIngicator(g, picbox_LRF.Height - iH);
                        }
                        break;

                    case 1:
                        // EBS範囲描画
                        {
                            int stAng = Brain.EBS_stAng + (int)CersioCt.BrainCtrl.EBS_HandleDiffAngle;
                            int edAng = Brain.EBS_edAng + (int)CersioCt.BrainCtrl.EBS_HandleDiffAngle;

                            int cirSize;

                            if (CersioCt.BrainCtrl.EBS_CautionLv >= Brain.EBS_StopLv)
                            {
                                // ブレーキレンジ内
                                Brush colBrs = Brushes.Red;
                                cirSize = (int)((Brain.EBS_BrakeRange * 2.0 / LocSys.RealToMapSclae) * scale);
                                g.FillPie(colBrs, (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                                  cirSize, cirSize,
                                                  stAng - 90, (edAng - stAng));
                            }
                            else if (CersioCt.BrainCtrl.EBS_CautionLv >= Brain.EBS_SlowDownLv)
                            {
                                // スローダウンレンジ内
                                Brush colBrs = Brushes.Orange;
                                cirSize = (int)((Brain.EBS_SlowRange * 2.0 / LocSys.RealToMapSclae) * scale);
                                g.FillPie(colBrs, (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                                  cirSize, cirSize,
                                                  stAng - 90, (edAng - stAng));
                            }


                            {
                                Pen colPen = Pens.Yellow;

                                // スローダウン　レンジ枠
                                cirSize = (int)((Brain.EBS_SlowRange * 2.0 / LocSys.RealToMapSclae) * scale);
                                g.DrawPie(colPen, (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                                  cirSize, cirSize,
                                                  stAng - 90, (edAng - stAng));

                                // ブレーキ　レンジ枠
                                cirSize = (int)((Brain.EBS_BrakeRange * 2.0 / LocSys.RealToMapSclae) * scale);
                                g.DrawPie(colPen, (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                                  cirSize, cirSize,
                                                  stAng - 90, (edAng - stAng));
                            }
                        }

                        // EHS範囲描画
                        {
                            int stAng;
                            int edAng;

                            int cirSize = (int)((Brain.EHS_MaxRange * 2.0 / LocSys.RealToMapSclae) * scale);

                            Pen colPen = Pens.LightGreen;

                            // 左側
                            stAng = Brain.EHS_stLAng;
                            edAng = Brain.EHS_edLAng;
                            if (CersioCt.BrainCtrl.EHS_Result == Brain.EHS_MODE.LeftWallHit || CersioCt.BrainCtrl.EHS_Result == Brain.EHS_MODE.CenterPass)
                            {
                                g.FillPie(Brushes.Red, (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                                  cirSize, cirSize,
                                                  stAng - 90, (edAng - stAng));
                            }
                            g.DrawPie(colPen, (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                              cirSize, cirSize,
                                              stAng - 90, (edAng - stAng));


                            // 右側
                            stAng = Brain.EHS_stRAng;
                            edAng = Brain.EHS_edRAng;
                            if (CersioCt.BrainCtrl.EHS_Result == Brain.EHS_MODE.RightWallHit || CersioCt.BrainCtrl.EHS_Result == Brain.EHS_MODE.CenterPass)
                            {
                                g.FillPie(Brushes.Red, (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                                  cirSize, cirSize,
                                                  stAng - 90, (edAng - stAng));
                            }
                            g.DrawPie(colPen, (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                              cirSize, cirSize,
                                              stAng - 90, (edAng - stAng));
                        }

                        // ノイズリダクションLRF描画
                        if (LocSys.LRF_UntiNoiseData != null)
                        {
                            double rPI = Math.PI / 180.0;
                            int pixelSize = 3;
                            double picScale = (LRFViewScale / 1000.0f) * scale;

                            double[] lrfdata = LocSys.LRF_UntiNoiseData;

                            // LRFの値を描画
                            for (int i = 0; i < lrfdata.Length; i++)
                            {
                                double val = lrfdata[i] * picScale;
                                double rad = (i - MapRangeFinder.AngleRangeHalf - 90) * rPI;

                                float x = (float)(ctrX + val * Math.Cos(rad));
                                float y = (float)(ctrY + val * Math.Sin(rad));
                                g.FillRectangle(Brushes.Cyan, x, y, pixelSize, pixelSize);
                            }
                        }
                        break;
                    case 2:
                        DrawIngicator(g, picbox_LRF.Height / 2 - 50 );
                        break;
                }
            }
        }

        /// <summary>
        /// インジケータ描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="baseY"></param>
        private void DrawIngicator(Graphics g, int baseY)
        {
            {
                // Handle
                int stX = 20;
                int stY = baseY + 8;
                int Wd = 200;
                int Ht = 15;

                float handleVal = (float)((Wd / 2) * (-CersioCtrl.nowSendHandleValue));
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
                g.DrawString("Handle", fntMini, Brushes.White, stX + Wd / 2 - 38 / 2, stY + Ht - fntMini.GetHeight());

                // ACC
                stX = 40;
                stY = baseY + 30;
                Wd = 10;
                Ht = 40;

                float accVal = (float)(Ht * CersioCtrl.nowSendAccValue);
                g.FillRectangle(Brushes.Red, stX, stY + Ht - accVal, Wd, accVal);
                g.DrawRectangle(Pens.White, stX, stY, Wd, Ht);
                g.DrawString("Acc", fntMini, Brushes.White, stX - 35, stY + Ht - fntMini.GetHeight());

            }

            // Compus
            if (CersioCt.bhwCompass || IsEmurateMode)
            {
                DrawMaker(g, Brushes.Red, 90, baseY + 50, -CersioCt.hwCompass, 12);
                g.DrawString("Compus", fntMini, Brushes.White, 90 - 25, baseY + 50 + 14);
            }

            // RE Dir
            if (CersioCt.bhwREPlot || IsEmurateMode)
            {
                DrawMaker(g, Brushes.Purple, 150, baseY + 50, -CersioCt.hwREDir * 180.0 / Math.PI, 12);
                g.DrawString("R.E.Dir", fntMini, Brushes.White, 150 - 25, baseY + 50 + 14);
            }

            // GPS Dir
            if (CersioCt.bhwGPS || IsEmurateMode)
            {
                DrawMaker(g, Brushes.LimeGreen, 210, baseY + 50, CersioCt.hwGPS_MoveDir, 12);
                g.DrawString("GPSDir", fntMini, Brushes.White, 210 - 25, baseY + 50 + 14);
            }

        }

        /// <summary>
        /// 表示切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picbox_LRF_Click(object sender, EventArgs e)
        {
            // モード切り替え
            selPicboxLRFmode = (++selPicboxLRFmode) % 3;
            picbox_LRF.Invalidate();
        }

        /// <summary>
        /// キー入力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
#if EMULATOR_MODE
            double mvScale = 300.0 / LocSys.RealToMapSclae;      // 300mm動く

            MarkPoint tgtObj = LocSys.E1;               // 操作対象

            switch (e.KeyCode) {
                case Keys.Up:
                    // 前進
                    CalcMoveRobot(tgtObj, mvScale,0.0);
                    break;
                case Keys.Down:
                    CalcMoveRobot(tgtObj, - mvScale, 0.0);
                    break;
                case Keys.Right:
                    CalcMoveRobot(tgtObj, 0.0, -5.0);
                    break;
                case Keys.Left:
                    CalcMoveRobot(tgtObj, 0.0, 5.0);
                    break;
             }
#endif
        }

        /// <summary>
        /// センサマーカーを移動させる
        /// </summary>
        /// <param name="movObj"></param>
        /// <param name="movVal"></param>
        /// <param name="dirVal"></param>
        private void CalcMoveRobot(MarkPoint movObj, double movVal, double dirVal, bool bOnNoise = false )
        {
            double mvRad = (movObj.Theta - 90.0) * Math.PI / 180.0;

            double nzX = 0.0;
            double nzY = 0.0;
            double nzDir = 0.0;

            // リアル　座標更新(ブレを入れる)
            if (bOnNoise)
            {
                nzX = (Rand.NextDouble() - 0.5) * 0.3;
                nzY = (Rand.NextDouble() - 0.5) * 0.3;
                nzDir = (Rand.NextDouble() - 0.5) * 2;
            }

            movObj.X += (-Math.Cos(mvRad) + nzX) * movVal;
            movObj.Y += (Math.Sin(mvRad) + nzY) * movVal;

            movObj.Theta += (dirVal + nzDir + 360) % 360;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e) {

        }


        private void cb_LRFConnect_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_LRFConnect.Checked)
            {
                // 元のカーソルを保持
                Cursor preCursor = Cursor.Current;
                // カーソルを待機カーソルに変更
                Cursor.Current = Cursors.WaitCursor;

                {
                    int intLrfPot;
                    if (int.TryParse(tb_LRFPort.Text, out intLrfPot))
                    {
                        LocSys.InitURG(tb_LRFIpAddr.Text, intLrfPot);

                        if (LocSys.IsLRFConnect())
                        {
                            tb_LRFIpAddr.BackColor = Color.Lime;
                            tb_LRFPort.BackColor = Color.Lime;
                        }
                    }
                }

                // カーソルを元に戻す
                Cursor.Current = preCursor;
            }
            else
            {
                LocSys.CloseURG();

                tb_LRFIpAddr.BackColor = SystemColors.Window;
                tb_LRFPort.BackColor = SystemColors.Window;
            }
        }

        private void cb_LocationPresumpring_CheckedChanged(object sender, EventArgs e)
        {
            LocalizeFlag = cb_LocationPresumption.Checked;
        }

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            this.Focus();
            e.IsInputKey = true;
        }

        private void cb_TimerUpdate_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            this.Focus();
            e.IsInputKey = true;
        }

        // 自動更新ボタン
        private void cb_TimerUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_TimerUpdate.Checked)
            {
                CersioCt.SendCommandList.Clear();
                // スタート時のGPS情報があれば設定
                if (CersioCt.bhwGPS)
                {
                    LocPreSumpSystem.SetStartGPS( CersioCt.hwGPS_LandX,
                                                  CersioCt.hwGPS_LandY,
                                                  (int)(LocSys.worldMap.GetWorldX(LocSys.R1.X)+0.5),
                                                  (int)(LocSys.worldMap.GetWorldY(LocSys.R1.Y)+0.5), true);
                }

                // RE1リセット
                CersioCt.RE_Reset(LocSys.R1.X, LocSys.R1.Y, LocSys.R1.Theta);

                tm_LocUpdate.Enabled = true;
            }
            else tm_LocUpdate.Enabled = false;
        }


        //--------------------------------------------------------------------------------------------------------------------
        // タイマイベント処理

        // Form内のピクチャー更新
        private void PictureUpdate()
        {
            LocSys.UpdateLocalizeBitmap(LocalizeFlag, true );

            this.picbox_AreaMap.Refresh();
            this.picbox_LRF.Refresh();
        }

        // Form内のクラスを更新
        static int updateMainCnt = 0;

        /// <summary>
        /// メイン処理　更新
        /// </summary>
        private void TickFormUpdate()
        {
            // セルシオ コントロール
            CersioCt.Update(LocSys, cb_EmgBrake.Checked, cb_EHS.Checked, bLocRivisionTRG, cb_LocationPresumption.Checked );

            // ハンドル、アクセル値　表示
            tb_AccelVal.Text = CersioCtrl.nowSendAccValue.ToString("f2");
            tb_HandleVal.Text = CersioCtrl.nowSendHandleValue.ToString("f2");

            // REからのスピード取得
            tb_RESpeed.Text = CersioCtrl.SpeedMH.ToString("f1");

            // LRF　マッチングスコア
            lbl_MattingScore.Text = "MatchingScore:" + CersioCt.BrainCtrl.MatchingScore.ToString();

            if (IsEmurateMode)
            {
                // エミュレート動作時

                // カーソルを操作
                {
                    if (CersioCtrl.LeftBtn)
                    {
                        this.Form1_KeyDown(this, new KeyEventArgs(Keys.Left));
                    }
                    else if (CersioCtrl.RightBtn)
                    {
                        this.Form1_KeyDown(this, new KeyEventArgs(Keys.Right));
                    }
                }

                if (updateMainCnt % 2 == 0)
                {
                    if (CersioCtrl.FwdBtn)
                    {
                        this.Form1_KeyDown(this, new KeyEventArgs(Keys.Up));
                    }
                }
            }
            else
            {
                // 実走行時
                if (updateMainCnt % 50 == 0)
                {
                    // 状態を見て、自動接続
                    if (!CersioCt.TCP_IsConnected())
                    {
                        CersioCt.ConnectBoxPC();
                    }
                }
            }

            // エマージェンシーブレーキ 動作カラー表示
            if (Brain.EmgBrk && cb_EmgBrake.Checked) cb_EmgBrake.BackColor = Color.Red;
            else cb_EmgBrake.BackColor = SystemColors.Control;

            if (CersioCt.BrainCtrl.EHS_Result != Brain.EHS_MODE.None && cb_EHS.Checked)
            {
                cb_EHS.BackColor = Color.Orange;
            }
            else cb_EHS.BackColor = SystemColors.Control;

            // UntiEBS Cnt
            lbl_BackCnt.Text = "EBS cnt:" + CersioCt.BrainCtrl.EmgBrakeContinueCnt.ToString();

            bLocRivisionTRG = false;
            updateMainCnt++;

        }

        /// <summary>
        /// LRFスケール変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btm_LRFScale_Click(object sender, EventArgs e)
        {
            double tval;
            double.TryParse(tb_LRFScale.Text, out tval);

            if (tval != 0.0)
            {
                LRFViewScale = tval;
            }
        }

        /// <summary>
        /// 自己推定位置リセット
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PositionReset_Click(object sender, EventArgs e)
        {
            LocSys.V1.X = LocSys.R1.X;
            LocSys.V1.Y = LocSys.R1.Y;
            LocSys.V1.Theta = LocSys.R1.Theta;

            PictureUpdate();
        }

        // 障害物停止チェックボックス
        private void cb_EmgBrake_CheckedChanged(object sender, EventArgs e)
        {

        }

        // ハードウェア用 周期の短いカウンタ
        private int updateHwCnt = 0;

        /// <summary>
        /// ハードウェア系の更新
        /// (間隔短め 50MS)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tm_UpdateHw_Tick(object sender, EventArgs e)
        {
            // LRF値取得
            bool resultLRF = LocSys.LRF_Update();

            try
            {
                if (LocSys.IsLRFConnect())
                {
                    // LRFから取得
                    if (resultLRF) lb_LRFResult.Text = "OK";
                    else lb_LRFResult.Text = "OK(noData)";
                }
                else
                {
                    // 仮想マップから取得
                    lb_LRFResult.Text = "Disconnect";
                }
            }
            catch
            {
            }

            // セルシオ ハードウェア情報取得
            CersioCt.GetHWStatus( ((usbGPS!=null)?true:false) );

            // ロータリーエンコーダ値,コンパス値 更新
            if (CersioCt.bhwREPlot)
            {
                LocSys.SetRotaryEncoderData(CersioCt.hwREX, CersioCt.hwREY, CersioCt.hwREDir);

                lbl_REX.Text = CersioCt.hwREX.ToString("f1");
                lbl_REY.Text = CersioCt.hwREY.ToString("f1");
                lbl_REDir.Text = CersioCt.hwREDir.ToString("f1");
            }
            if (CersioCt.bhwRE)
            {
                lbl_RErotR.Text = CersioCt.hwRErotR.ToString("f1");
                lbl_RErotL.Text = CersioCt.hwRErotL.ToString("f1");
            }
            if (CersioCt.bhwCompass)
            {
                LocSys.SetCompassData(CersioCt.hwCompass);

                lbl_Compass.Text = CersioCt.hwCompass.ToString();
            }
            if (CersioCt.bhwGPS)
            {
                // 途中からでもGPSのデータを拾う
                if (!LocPreSumpSystem.bEnableGPS)
                {
                    LocPreSumpSystem.SetStartGPS(CersioCt.hwGPS_LandX,
                                                  CersioCt.hwGPS_LandY,
                                                  (int)(LocSys.worldMap.GetWorldX(LocSys.R1.X)+0.5),
                                                  (int)(LocSys.worldMap.GetWorldY(LocSys.R1.Y)+0.5), false);
                }
                LocSys.SetGPSData(CersioCt.hwGPS_LandX, CersioCt.hwGPS_LandY, CersioCt.hwGPS_MoveDir);
                lbl_GPS_Y.Text = CersioCt.hwGPS_LandY.ToString("f5");
                lbl_GPS_X.Text = CersioCt.hwGPS_LandX.ToString("f5");
            }

            if (CersioCt.ptnHeadLED == -1)
            {
                lbl_LED.Text = "ND";
            }
            else
            {
                string ledStr = CersioCt.ptnHeadLED.ToString();

                if (CersioCt.ptnHeadLED >= 0 && CersioCt.ptnHeadLED < CersioCtrl.LEDMessage.Count())
                {
                    ledStr += "," + CersioCtrl.LEDMessage[CersioCt.ptnHeadLED];
                }

                if (!ledStr.Equals(lbl_LED.Text))
                {
                    lbl_LED.Text = ledStr;
                }
            }

            // BoxPC接続状態確認
            if (CersioCt.TCP_IsConnected())
            {
                tb_SendData.BackColor = Color.Lime;
                tb_ResiveData.BackColor = Color.Lime;
            }
            else
            {
                tb_SendData.BackColor = SystemColors.Window;
                tb_ResiveData.BackColor = SystemColors.Window;
            }


            // 送受信文字更新
            if (null != CersioCt.hwResiveStr)
            {
                tb_ResiveData.Text = CersioCt.hwResiveStr.Replace('\n', ' ');
            }
            if (null != CersioCt.hwSendStr)
            {
                tb_SendData.Text = CersioCt.hwSendStr.Replace('\n', ' ');
            }

            if (null != usbGPS)
            {
                tb_SirialResive.Text = usbGPS.resiveStr;
                
#if GPSLOG_OUTPUT
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(saveGPSLogFname, true, System.Text.Encoding.GetEncoding("shift_jis"));
                    sw.Write(usbGPS.resiveStr);
                    sw.Close();
                }
#endif
                if (!string.IsNullOrEmpty(usbGPS.resiveStr))
                {
                    CersioCt.usbGPSResive.Add(usbGPS.resiveStr);
                }
                usbGPS.resiveStr = "";

                if (CersioCt.usbGPSResive.Count > 30)
                {
                    CersioCt.usbGPSResive.RemoveRange(0, CersioCt.usbGPSResive.Count - 30);
                }
            }


            updateHwCnt++;
            if (0 == updateHwCnt % 10)
            {
                PictureUpdate();
            }
        }

        /// <summary>
        /// 自己位置推定計算用　更新
        /// (間隔長め)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tm_LocUpdate_Tick(object sender, EventArgs e)
        {
            // 重い計算処理 更新
            TickFormUpdate();

#if LOGWRITE_MODE
            // ログファイル出力
            if( !string.IsNullOrEmpty(saveLogFname))
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(saveLogFname, true, System.Text.Encoding.GetEncoding("shift_jis"));

                // 固有識別子 + 時間
                sw.Write("LocPresumpLog:"+ DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + System.Environment.NewLine);

                // ハードウェア情報
                if (CersioCt.TCP_IsConnected())
                {
                    sw.Write("hwSendStr:" + CersioCt.hwSendStr.Replace('\n', ' ') + System.Environment.NewLine);
                    sw.Write("hwResiveStr:" + CersioCt.hwResiveStr + System.Environment.NewLine);
                    sw.Write("handle:" + CersioCtrl.nowSendHandleValue + " / acc:" + CersioCtrl.nowSendAccValue + System.Environment.NewLine);
                }
                else
                {
                    sw.Write("Comment:No Connect BoxPC" + System.Environment.NewLine);
                }
                CersioCt.hwSendStr = "";

                // 位置情報
                {
                    sw.Write("R1:X " + LocSys.worldMap.GetWorldX(LocSys.R1.X).ToString("f3") +
                             "/Y " + LocSys.worldMap.GetWorldY(LocSys.R1.Y).ToString("f3") +
                             "/ Dir " + LocSys.R1.Theta.ToString("f2") +
                             System.Environment.NewLine);

                    sw.Write("V1:X " + LocSys.worldMap.GetWorldX(LocSys.V1.X).ToString("f3") +
                             "/Y " + LocSys.worldMap.GetWorldY(LocSys.V1.Y).ToString("f3") +
                             "/ Dir " + LocSys.V1.Theta.ToString("f2") +
                             System.Environment.NewLine);

                    sw.Write("E1:X " + LocSys.worldMap.GetWorldX(LocSys.E1.X).ToString("f3") +
                             "/Y " + LocSys.worldMap.GetWorldY(LocSys.E1.Y).ToString("f3") +
                             "/ Dir " + LocSys.E1.Theta.ToString("f2") +
                             System.Environment.NewLine);

                    sw.Write("C1:X " + LocSys.worldMap.GetWorldX(LocSys.C1.X).ToString("f3") +
                             "/Y " + LocSys.worldMap.GetWorldY(LocSys.C1.Y).ToString("f3") +
                             "/ Dir " + LocSys.C1.Theta.ToString("f2") +
                             System.Environment.NewLine);

                    sw.Write("G1:X " + LocSys.worldMap.GetWorldX(LocSys.G1.X).ToString("f3") +
                             "/Y " + LocSys.worldMap.GetWorldY(LocSys.G1.Y).ToString("f3") +
                             "/ Dir " + LocSys.G1.Theta.ToString("f2") +
                             System.Environment.NewLine);
                }

                // 
                {
                    sw.Write("MatchingScore:" + CersioCt.BrainCtrl.MatchingScore.ToString() + System.Environment.NewLine);

                    // Rooting情報
                    {
                        sw.Write("RTS_TargetIndex:" + CersioCt.BrainCtrl.RTS.GetNowCheckPointIdx() + System.Environment.NewLine);

                        int tgtPosX = 0;
                        int tgtPosY = 0;
                        double tgtDir = 0;
                        CersioCt.BrainCtrl.RTS.getNowTarget(ref tgtPosX, ref tgtPosY);
                        CersioCt.BrainCtrl.RTS.getNowTargetDir(ref tgtDir);

                        sw.Write("RTS_TargetPos:X " + tgtPosX.ToString("f3") +
                                 "/Y " + tgtPosY.ToString("f3") +
                                 "/Dir " + tgtDir.ToString("f2") +
                                 System.Environment.NewLine);

                        sw.Write("RTS_Handle:" + CersioCt.BrainCtrl.RTS.getHandleValue().ToString("f2") + System.Environment.NewLine);
                    }

                    // Brain情報
                    {
                        sw.Write("EBS_CautionLv:" + CersioCt.BrainCtrl.EBS_CautionLv.ToString() + System.Environment.NewLine);
                        sw.Write("EHS_Handle:" + CersioCt.BrainCtrl.EhsHandleVal.ToString("f2") + "/Result " + Brain.EHS_ResultStr[(int)CersioCt.BrainCtrl.EHS_Result] + System.Environment.NewLine);
                    }

                    // CersioCtrl
                    {
                        sw.Write("GoalFlg:" + (CersioCt.goalFlg ? "TRUE": "FALSE") + System.Environment.NewLine);

                        if( CersioCt.bhwCompass )
                        {
                            sw.Write("hwCompass:" + CersioCt.hwCompass.ToString() + System.Environment.NewLine );
                        }

                        if( CersioCt.bhwREPlot )
                        {
                            sw.Write("hwREPlot:X " + CersioCt.hwREX.ToString("f3") +
                                     "/Y " + CersioCt.hwREY.ToString("f3") +
                                     "/Dir " + CersioCt.hwREDir.ToString("f2") +
                                     System.Environment.NewLine);
                        }

                        if( CersioCt.bhwGPS )
                        {
                            sw.Write("hwGPS:X " + CersioCt.hwGPS_LandX.ToString("f5") +
                                     "/Y " + CersioCt.hwGPS_LandY.ToString("f5") +
                                     "/Dir " + CersioCt.hwGPS_MoveDir.ToString("f2") +
                                     System.Environment.NewLine);
                        }
                    }

                    // 特記事項メッセージ出力
                    if (Brain.addLogMsg != null)
                    {
                        sw.Write("AddLog:" + Brain.addLogMsg + System.Environment.NewLine);
                    }
                    Brain.addLogMsg = null;
                }
                // 改行
                sw.Write(System.Environment.NewLine);

                //閉じる
                sw.Close();
            }
#endif
        }

        /// <summary>
        /// LRF Scale変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_LRFScale_TextChanged(object sender, EventArgs e)
        {
            double tval;
            double.TryParse(tb_LRFScale.Text, out tval);

            if (tval != 0.0)
            {
                LRFViewScale = tval;
            }
        }

        /// <summary>
        /// 位置補正 開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LocRevision_Click(object sender, EventArgs e)
        {
            // 強制位置補正
            bLocRivisionTRG = true;
        }

        /// <summary>
        /// 直進ルート生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_StraightMode_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_StraightMode.Checked)
            {
                CersioCt.BrainCtrl.RTS.ResetStraightMode();
            }

        }

        /// <summary>
        /// 送信コマンド処理 タイマー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tm_SendCom_Tick(object sender, EventArgs e)
        {
            if (null != CersioCt)
            {
                CersioCt.SendCommandTick();
            }
        }

        /// <summary>
        /// usbGPS取得ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_SirialConnect_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_SirialConnect.Checked)
            {
                if (null != usbGPS)
                {
                    usbGPS.Close();
                    usbGPS = null;
                }

                {
                    usbGPS = new UsbIOport();
                    if (usbGPS.Open(cb_UsbSirial.Text, 4800))
                    {
                        // 接続成功
                        tb_SirialResive.BackColor = Color.Lime;
                        cb_RivisonGPS.Checked = true;
                    }
                    else
                    {
                        // 接続失敗
                        tb_SirialResive.BackColor = SystemColors.Control;
                        tb_SirialResive.Text = "ConnectFail";
                        usbGPS = null;
                    }
                }
            }
            else
            {
                if (null != usbGPS)
                {
                    usbGPS.Close();
                    usbGPS = null;

                    tb_SirialResive.BackColor = SystemColors.Control;
                }
            }
        }

        private void cb_RivisonGPS_CheckedChanged(object sender, EventArgs e)
        {
            LocPreSumpSystem.bRivisonGPS = cb_RivisonGPS.Checked;
        }

        private void cb_RivisionPF_CheckedChanged(object sender, EventArgs e)
        {
            LocPreSumpSystem.bRivisonPF = cb_RivisionPF.Checked;
        }

        private void cb_TimeRivision_CheckedChanged(object sender, EventArgs e)
        {
            LocPreSumpSystem.bTimeRivision = cb_TimeRivision.Checked;
        }

    }
}
