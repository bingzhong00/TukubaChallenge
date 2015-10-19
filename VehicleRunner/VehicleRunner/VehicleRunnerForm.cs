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
// of the authors and should not be interpreted as representing official policies, 
// either expressed or implied, of the FreeBSD Project.


#define LOGWRITE_MODE   // ログファイル出力
#define LOGIMAGE_MODE   // イメージログ出力

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Threading;
using System.IO;

using LocationPresumption;
using CersioIO;

using Axiom.Math;

namespace VehicleRunner
{
    public partial class LocPresumpForm : Form
    {
        public static string saveLogFname;

        LocPreSumpSystem LocSys;
        CersioCtrl CersioCt;

        // エミュレーションモード
        bool bEmurateMode = false;

        Random Rand = new Random();
        bool LocalizeFlag;

        double LRFViewScale = 1.0;


        /// <summary>
        /// 
        /// </summary>
        public LocPresumpForm()
        {
            InitializeComponent();

            saveLogFname = GetTimeStampFileName();

            // セルシオコントローラ初期化
            CersioCt = new CersioCtrl();
            CersioCt.Start();

            // 自己位置推定初期化
            LocSys = new LocPreSumpSystem();
            LocSys.InitWorld("./OoSimizuKouen1100x1800.bmp", 100.0 * 1100.0, 100.0 * 1800.0);
            LocSys.SetStartPostion(788, 1428);

            // LRF 入力スケール調整反映
            //btm_LRFScale_Click(null, null);
            tb_LRFScale_TextChanged(null, null);

            // 自己位置推定計算
            cb_LocationPresumption.Checked = LocalizeFlag = false;

            // エミュレーションモード表示
            lbl_EmurateMode.Visible = bEmurateMode;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
            }

            // ハードウェア更新タイマ起動
            tm_UpdateHw.Enabled = true;

            // 画面更新
            PictureUpdate();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            LocSys.Close();
            CersioCt.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DeleteTimer();
            tm_UpdateHw.Enabled = false;
            tm_LocUpdate.Enabled = false;

#if LOGIMAGE_MODE
            // 軌跡ログ出力
            if (!string.IsNullOrEmpty(saveLogFname))
            {
                string saveImageLogFname = Path.ChangeExtension(saveLogFname, "png");

                Bitmap bmp = LocSys.MakeMakerLogBmp(false);
                bmp.Save(saveImageLogFname, System.Drawing.Imaging.ImageFormat.Png);
            }
#endif
        }

        // 

        /// <summary>
        /// タイムスタンプファイル作成
        /// </summary>
        /// <returns></returns>
        public string GetTimeStampFileName()
        {
            return "LocSampLog" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".log";
        }



        // Draw -------------------------------------------------------------------------------------------
        Font drawFont = new Font("MS UI Gothic", 16);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // 書き換えＢＭＰ（追加障害物）描画
            Graphics g = e.Graphics;

            g.FillRectangle(Brushes.Black, 0, 0, picbox_AreaMap.Width, picbox_AreaMap.Height);

            g.DrawImage(LocSys.AreaOverlayBmp, 0,0);
            g.DrawString("Pos X:" + (int)LocSys.R1.X +"(" + (int)LocSys.worldMap.GetWorldX(LocSys.R1.X)+")" +
                         ",Y:" + (int)LocSys.R1.Y +"(" + (int)LocSys.worldMap.GetWorldY(LocSys.R1.Y) + ")" +
                         ",角度:" + (int)LocSys.R1.Theta +
                         " Run:" + updateCnt.ToString(),
                         drawFont, Brushes.Black, 0, 0);


            // ターゲット描画
            if (null != CersioCt)
            {
                int tgtPosX, tgtPosY;
                double dir = 0;
                tgtPosX = tgtPosY = 0;
                float olScale = (float)LocSys.AreaOverlayBmp.Width / (float)LocSys.AreaBmp.Width;

                CersioCt.RTS.getNowTarget(ref tgtPosX, ref tgtPosY);
                CersioCt.RTS.getNowTargetDir(ref dir);

                DrawMaker(g, olScale, new MarkPoint( LocSys.worldMap.GetAreaX(tgtPosX), LocSys.worldMap.GetAreaY(tgtPosY), dir), Brushes.GreenYellow, 8);
            }


            //this.pictureBox1.Invalidate();
        }

        /// <summary>
        /// マーカー描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="fScale"></param>
        /// <param name="robot"></param>
        /// <param name="brush"></param>
        /// <param name="size"></param>
        /*
        private void DrawMaker(Graphics g, MarkPoint robot, Brush brush, int size)
        {
            float olScale = (float)LocSys.AreaOverlayBmp.Width / (float)LocSys.AreaBmp.Width;
            //int BitMapH = AreaBmp.Height;

            var P1 = new PointF(
                (float)(robot.X + size * Math.Cos(robot.Theta * Math.PI / 180.0)),
                (float)(robot.Y + size * Math.Sin(robot.Theta * Math.PI / 180.0)));
            var P2 = new PointF(
                (float)(robot.X + size * Math.Cos((robot.Theta - 150) * Math.PI / 180.0)),
                (float)(robot.Y + size * Math.Sin((robot.Theta - 150) * Math.PI / 180.0)));
            var P3 = new PointF(
                (float)(robot.X + size * Math.Cos((robot.Theta + 150) * Math.PI / 180.0)),
                (float)(robot.Y + size * Math.Sin((robot.Theta + 150) * Math.PI / 180.0)));

            g.FillPolygon(brush, new PointF[] { P1, P2, P3 });
        }*/
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

        /// <summary>
        /// LRFウィンドウデータ描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            // LRF取得データを描画
            {
                double rScale = (1.0 / LocSys.RealToMapSclae);
                double rPI = Math.PI / 180.0;

                int ctrX = picbox_LRF.Width / 2;
                int ctrY = picbox_LRF.Height*5/8;
                int pixelSize = 3;
                double picScale = picbox_LRF.Width * (LRFViewScale/1000.0f);

                // ガイド描画
                for (int i = 1; i <= 30/5; i++)
                {
                    int cirSize = (int)((i * 5000) / LocSys.RealToMapSclae);

                    e.Graphics.DrawPie( Pens.LightGray,
                                        (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                        cirSize, cirSize,
                                        -135-90, 270);
                }

                if (LocSys.LRF_Data != null)
                {
                    // LRFの値を描画
                    // +4間引く
                    for (int i = 0; i < LocSys.LRF_Data.Length; i++)
                    {
                        //double val = LocSys.LRF_Data[LocSys.LRF_Data.Length - 1 - i] * rScale;
                        double val = LocSys.LRF_Data[i] * picScale * rScale;
                        double rad = (i - MapRangeFinder.AngleRangeHalf - 90) * rPI;

                        float x = (float)(ctrX + val * Math.Cos(rad));
                        float y = (float)(ctrY + val * Math.Sin(rad));
                        e.Graphics.FillRectangle(Brushes.Black, x, y, pixelSize, pixelSize);
                    }
                }
            }

            // エマージェンシーブレーキ 動作カラー表示
            if (CersioCtrl.EmgBrk)  cb_EmgBrake.BackColor = Color.Red;
            else                    cb_EmgBrake.BackColor = SystemColors.Control;
        }


        /// <summary>
        /// マウス入力、　障害物設置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e) {
            //PaintFlag = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) {
            /*
            if (PaintFlag) {
                lock (MapAccess) {
                    MapAccess.BeginAccess();
                    MapAccess[e.X, e.Y] = Color.Black;
                    MapAccess.EndAccess();
                }
            }
             * */
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e) {
            //PaintFlag = false;
        }

        /// <summary>
        /// キー入力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            double mvScale = 300.0 / LocSys.RealToMapSclae;      // 300mm動く

            MarkPoint R1 = LocSys.R1;                   // リアルロボット
            MarkPoint V1 = LocSys.V1;                   // 仮想ロボット

            switch (e.KeyCode) {
                case Keys.Up:
                    // 前進
                    CalcMoveRobot(mvScale);
                    //LRFupdate();
                    //OneShotIssue();
                    break;
                case Keys.Down:
                    CalcMoveRobot(-mvScale);
                    //LRFupdate();
                    //OneShotIssue();
                    break;
                case Keys.Right:
                    R1.Theta = (R1.Theta - 5 + 360) % 360;

                    if (cb_EleConpus.Checked)
                    {
                        V1.Theta = (V1.Theta - 5 + 360) % 360;
                    }

                    //LRFupdate();
                    //OneShotIssue();
                    break;
                case Keys.Left:
                    R1.Theta = (R1.Theta + 5 + 360) % 360;

                    if (cb_EleConpus.Checked)
                    {
                        V1.Theta = (V1.Theta + 5 + 360) % 360;
                    }

                    //LRFupdate();
                    //OneShotIssue();
                    break;

                case Keys.U:
                    //LRFupdate();
                    //OneShotIssue();
                    break;
            }
            //this.pictureBox1.Refresh();
            //this.pictureBox2.Refresh();
        }

        private void CalcMoveRobot(double movVal)
        {
            MarkPoint R1 = LocSys.R1;                   // リアルロボット
            MarkPoint V1 = LocSys.V1;                   // 仮想ロボット

            double R1Rad = (R1.Theta - 90.0) * Math.PI / 180.0;
            double V1Rad = (V1.Theta - 90.0) * Math.PI / 180.0;

            // リアル　座標更新(ブレを入れる)
            R1.X += (-Math.Cos(R1Rad) + (Rand.NextDouble() - 0.5) * 0.3) * movVal;
            R1.Y += (Math.Sin(R1Rad) + (Rand.NextDouble() - 0.5) * 0.3) * movVal;
            R1.Theta = (R1.Theta + (Rand.NextDouble() - 0.5) * 4 + 360) % 360;

            if (cb_RotEnc.Checked)
            {
                // 仮想ロボット　座標更新
                V1.X += -Math.Cos(V1Rad) * movVal;
                V1.Y += Math.Sin(V1Rad) * movVal;
            }
        }

        // LRF 実データ取得
        /* 現時点不要
        private void LocPresumpUpdate()
        {
            // 自己位置推定　計算実行
            LocSys.FilterLocalizeUpdate(LocalizeFlag);
        }
        */

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
                    }
                }

                // カーソルを元に戻す
                Cursor.Current = preCursor;
            }
            else
            {
                LocSys.CloseURG();
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
                // Run
                tm_LocUpdate.Enabled = true;
                //OneShotIssue();
                //OneShotUpdateFlg = false;   // １回更新のフラグOFF で　更新しつづける
            }
            else
            {
                // Stop
                tm_LocUpdate.Enabled = false;

                //OneShotUpdateFlg = true;    // １回更新のフラグON で　次の更新で完了させる
            }
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
        static int updateCallCnt = 0;
        private void TickFormUpdate()
        {
            MarkPoint R1 = LocSys.R1;                   // リアルロボット

            {
                // 通常時
                // OneShotでなければ、データ取得、自己位置推定計算も実行
                //LRFupdate();

                // 自己位置推定　計算実行
                LocSys.FilterLocalizeUpdate(LocalizeFlag, bEmurateMode);

                bool goalFlg = false;

                goalFlg = CersioCt.Update((int)LocSys.worldMap.GetWorldX(R1.X),
                                          (int)LocSys.worldMap.GetWorldY(R1.Y),
                                          (int)(R1.Theta), LocSys.LRF_Data, cb_EmgBrake.Checked);

                // ハンドル、アクセル値　表示
                tb_AccelVal.Text = CersioCtrl.accValue.ToString();
                tb_HandleVal.Text = CersioCtrl.handleValue.ToString();

                // セルシオ自動運転　Form内エミュレート
                /*
                if (serialPort1.IsOpen)
                {
                    SendSirialData(CersioCtrl.handleValue, CersioCtrl.accValue);
                }
                else
                 * */
                {
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

                    if (updateCallCnt % 2 == 0)
                    {
                        if (CersioCtrl.FwdBtn)
                        {
                            this.Form1_KeyDown(this, new KeyEventArgs(Keys.Up));
                        }
                    }
                }

                // ゴールしたら終わる
                if (goalFlg)
                {
                    //OneShotUpdateFlg = true;
                    CersioCt.goalFlg = true;
                }
                updateCallCnt++;
            }


            //PictureUpdate();
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


        private int updateCnt = 0;
        /// <summary>
        /// ハードウェア系の更新
        /// (間隔短め)
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
                    else lb_LRFResult.Text = "NG";
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
            CersioCt.GetHWStatus();

            // ロータリーエンコーダ値,コンパス値 更新
            LocSys.RotaryEncoder_Update(CersioCt.hwREX, CersioCt.hwREY, CersioCt.hwRERad);
            LocSys.Compus_Update(CersioCt.hwCompass);

            // 送受信文字更新
            tb_ResiveData.Text = CersioCt.hwResiveStr;
            tb_SendData.Text = CersioCt.hwSendStr;

            updateCnt++;
            if (0 == updateCnt % 10)
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
            //if (cb_LogFile.Checked && !string.IsNullOrEmpty(saveLogFname))
            if( !string.IsNullOrEmpty(saveLogFname))
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(saveLogFname, true, System.Text.Encoding.GetEncoding("shift_jis"));

                // 固有識別子 + 時間
                sw.Write("LocPresumpLog:"+ DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + System.Environment.NewLine);

                // ハードウェア情報
                if (CersioCt.TCP_IsConnected())
                {
                    sw.Write("hwSendStr:" + CersioCt.hwSendStr + System.Environment.NewLine);
                    sw.Write("hwResiveStr:" + CersioCt.hwResiveStr + System.Environment.NewLine);
                    sw.Write("handle:" + CersioCtrl.handleValue + " / acc:" + CersioCtrl.accValue + System.Environment.NewLine);
                    //sw.Write("checkPoint:" + CersioCt.RTS.GetNowCheckPointIdx() + "/" + CersioCt.RTS.GetNumCheckPoint() + System.Environment.NewLine);
                }
                else
                {
                    sw.Write("Comment:No Connect BoxPC" + System.Environment.NewLine);
                }

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




    }
}
