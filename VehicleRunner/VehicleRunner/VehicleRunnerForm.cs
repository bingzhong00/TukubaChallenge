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

using Axiom.Math;

namespace VehicleRunner
{
    public partial class VehicleRunnerForm : Form
    {
        public static string saveLogFname;

        LocPreSumpSystem LocSys;
        CersioCtrl CersioCt;

        // エミュレーションモード
#if EMULATOR_MODE
        public static bool IsEmurateMode = true;
#else
        public static bool IsEmurateMode = false;
#endif

        Random Rand = new Random();
        bool LocalizeFlag;

        double LRFViewScale = 1.0;

        private int selPicboxLRFmode = 0;

        private int R1LocRevisionCnt = 0;

        /// <summary>
        /// 
        /// </summary>
        public VehicleRunnerForm()
        {
            InitializeComponent();


            saveLogFname = GetTimeStampFileName("./Log/LocSampLog", ".log");

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
            CersioCt.Start( IsEmurateMode );

            // 自己位置推定初期化
            LocSys = new LocPreSumpSystem();

            // マップ情報設定
            //  マップファイル名、実サイズの横[mm], 実サイズ縦[mm] (北向き基準)
            LocSys.InitWorld( RootingData.MapFileName, RootingData.RealWidth, RootingData.RealHeight);
            LocSys.SetStartPostion( (int)RootingData.startPosition.x,
                                    (int)RootingData.startPosition.y,
                                    RootingData.startDir );

            // REをリセット
            CersioCt.RE_Reset(0.0, 0.0, RootingData.startDir);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picbox_AreaMap_Paint(object sender, PaintEventArgs e)
        {
            // 書き換えＢＭＰ（追加障害物）描画
            Graphics g = e.Graphics;

            //g.FillRectangle(Brushes.Black, 0, 0, picbox_AreaMap.Width, picbox_AreaMap.Height);

            g.DrawImage(LocSys.AreaOverlayBmp, 0,0);

            DrawString(g,  0, drawFont.Height*0,
                       "R1 X:" + ((int)LocSys.worldMap.GetWorldX(LocSys.R1.X)).ToString("D4") +
                       ",Y:" + ((int)LocSys.worldMap.GetWorldY(LocSys.R1.Y)).ToString("D4") +
                       ",角度:" + ((int)LocSys.R1.Theta).ToString("D3"),
                       Brushes.Red, Brushes.Black );

            DrawString(g, 0, drawFont.Height * 1,
                       "Compass:" + CersioCt.hwCompass.ToString("D3") + "/ ReDir:" + ((int)(CersioCt.hwRERad * 180.0 / Math.PI)).ToString("D3") +
                       ",ReX:" + ((int)(CersioCt.hwREX)).ToString("D4") + ",Y:" + ((int)(CersioCt.hwREY)).ToString("D4"),
                       Brushes.Blue, Brushes.White);

            DrawString(g, 0, drawFont.Height * 2,
                       "Goal:" + (CersioCt.goalFlg ? "TRUE" : "FALSE") + "/ Run:" + updateHwCnt.ToString("D8"),
                       Brushes.Blue, Brushes.White);

            DrawString(g, 0, drawFont.Height * 3,
                       "LocProc:" + LocSys.swCNT_Update.ElapsedMilliseconds + "ms /Draw:" + LocSys.swCNT_Draw.ElapsedMilliseconds + "ms /MRF:" + LocPreSumpSystem.swCNT_MRF.ElapsedMilliseconds + "ms",
                       Brushes.Blue, Brushes.White);
                           
            

            // ターゲット描画
            if (null != CersioCt)
            {
                int tgtPosX, tgtPosY;
                double dir = 0;
                tgtPosX = tgtPosY = 0;
                float olScale = (float)LocSys.AreaOverlayBmp.Width / (float)LocSys.AreaBmp.Width;

                CersioCt.BrainCtrl.RTS.getNowTarget(ref tgtPosX, ref tgtPosY);
                CersioCt.BrainCtrl.RTS.getNowTargetDir(ref dir);

                DrawMaker(g, olScale, new MarkPoint( LocSys.worldMap.GetAreaX(tgtPosX), LocSys.worldMap.GetAreaY(tgtPosY), dir), Brushes.GreenYellow, 8);
            }
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
        private void picbox_AreaMap_Click(object sender, EventArgs e)
        {
            // ※エリア、ワールドマップ切り替え
        }


        // ---------------------------------------------------------------------------------------------------

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
                int ctrY = picbox_LRF.Height * 5 / 8;

                float scale = 1.0f;

                // 背景色
                switch (selPicboxLRFmode)
                {
                    case 0:
                        picbox_LRF.BackColor = Color.White;
                        scale = 1.0f;
                        break;
                    case 1:
                        picbox_LRF.BackColor = Color.Black;

                        scale = 5.0f;

                        // EBSに反応があればズーム
                        scale += ((float)CersioCt.BrainCtrl.EBS_CautionLv * 3.0f / (float)Brain.EBS_CautionLvMax);

                        // EHS
                        if (CersioCt.BrainCtrl.EHS_Result != Brain.EHS_MODE.None) scale = 10.0f;
                        break;
                }

                // ガイド描画
                // 30mを2m区切りで描画
                for (int i = 1; i <= 30 / 2; i++)
                {
                    int cirSize = (int)((((i * 2000)*2) / LocSys.RealToMapSclae) * scale);

                    g.DrawPie(Pens.LightGray,
                                        (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                        cirSize, cirSize,
                                        -135 - 90, 270);
                }

                switch (selPicboxLRFmode)
                {
                    case 0:
                        // LRF描画
                        if (LocSys.LRF_Data != null)
                        {
                            double rScale = (1.0 / LocSys.RealToMapSclae);
                            double rPI = Math.PI / 180.0;
                            int pixelSize = 3;
                            double picScale = picbox_LRF.Width * (LRFViewScale / 1000.0f);

                            // LRFの値を描画
                            for (int i = 0; i < LocSys.LRF_Data.Length; i++)
                            {
                                double val = LocSys.LRF_Data[i] * picScale * rScale;
                                double rad = (i - MapRangeFinder.AngleRangeHalf - 90) * rPI;

                                float x = (float)(ctrX + val * scale * Math.Cos(rad));
                                float y = (float)(ctrY + val * scale * Math.Sin(rad));
                                g.FillRectangle(Brushes.Black, x, y, pixelSize, pixelSize);
                            }
                        }
                        break;

                    case 1:
                        // EBS範囲描画
                        {
                            int stAng = Brain.EBS_stAng;
                            int edAng = Brain.EBS_edAng;

                            int cirSize;

                            if (CersioCt.BrainCtrl.EBS_CautionLv >= Brain.EBS_StopLv)
                            {
                                // ブレーキレンジ内
                                Brush colBrs = Brushes.Red;
                                cirSize = (int)((Brain.EBS_BrakeRange*2.0 / LocSys.RealToMapSclae) * scale);
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
                                g.FillPie( Brushes.Red, (ctrX - cirSize / 2), (ctrY - cirSize / 2),
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
                        if (CersioCt.BrainCtrl.lrfUntiNoise != null)
                        {
                            double rScale = (1.0 / LocSys.RealToMapSclae);
                            double rPI = Math.PI / 180.0;
                            int pixelSize = 3;
                            double picScale = picbox_LRF.Width * (LRFViewScale / 1000.0f);

                            double[] lrfdata = CersioCt.BrainCtrl.lrfUntiNoise;

                            // LRFの値を描画
                            for (int i = 0; i < lrfdata.Length; i++)
                            {
                                double val = lrfdata[i] * picScale * rScale;
                                double rad = (i - MapRangeFinder.AngleRangeHalf - 90) * rPI;

                                float x = (float)(ctrX + val * scale * Math.Cos(rad));
                                float y = (float)(ctrY + val * scale * Math.Sin(rad));
                                g.FillRectangle(Brushes.Cyan, x, y, pixelSize, pixelSize);
                            }
                        }
                        break;
                }
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
            selPicboxLRFmode = (++selPicboxLRFmode) % 2;
            picbox_LRF.Invalidate();
        }

        /// <summary>
        /// キー入力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
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
            if (cb_TimerUpdate.Checked) tm_LocUpdate.Enabled = true;
            else                        tm_LocUpdate.Enabled = false;
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
            // 通常時

            // 自己位置推定　計算実行
            LocSys.FilterLocalizeUpdate(LocalizeFlag, IsEmurateMode);

            // 強制 位置補正
            if (R1LocRevisionCnt > 0)
            {
                // 指定回数分位置補正して、LRFの値を使う
                if (!LocalizeFlag)
                {
                    LocSys.FilterLocalizeUpdate(true, IsEmurateMode);
                }

                RevisionProgBer.PerformStep();
                R1LocRevisionCnt--;
                if (R1LocRevisionCnt == 0)
                {
                    // パーティクルフィルタの結果で、位置をリセット
                    LocSys.R1.Set(LocSys.V1);

                    // REの向きをリセット
                    // 位置座標は、ログが残らなくなるのでしない
                    // （移動差分を見てるのでリセットしなくても問題ない）
                    CersioCt.RE_Reset(0, 0, LocSys.V1.Theta);
                    RevisionProgBer.Value = 0;
                }
            }

            // セルシオ コントロール
            CersioCt.Update( LocSys, cb_EmgBrake.Checked);

            // ハンドル、アクセル値　表示
            tb_AccelVal.Text = CersioCtrl.nowSendAccValue.ToString("f2");
            tb_HandleVal.Text = CersioCtrl.nowSendHandleValue.ToString("f2");

            // REからのスピード取得
            tb_RESpeed.Text = CersioCtrl.SpeedMH.ToString("f1");

            // LRF　マッチングスコア
            lbl_MattingScore.Text = "MatchingScore:" + CersioCt.BrainCtrl.MatchingScore.ToString();

            // セルシオ自動運転　Form内エミュレート
            /*
            if (serialPort1.IsOpen)
            {
                SendSirialData(CersioCtrl.handleValue, CersioCtrl.accValue);
            }
                * */

            if( IsEmurateMode )
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

                if (updateMainCnt % 2 == 0)
                {
                    if (CersioCtrl.FwdBtn)
                    {
                        this.Form1_KeyDown(this, new KeyEventArgs(Keys.Up));
                    }
                }
            }

            // エマージェンシーブレーキ 動作カラー表示
            if (Brain.EmgBrk) cb_EmgBrake.BackColor = Color.Red;
            else cb_EmgBrake.BackColor = SystemColors.Control;

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
            if (CersioCt.bhwREPlot)
            {
                LocSys.SetRotaryEncoderData(CersioCt.hwREX, CersioCt.hwREY, CersioCt.hwRERad);
            }
            if (CersioCt.bhwCompass)
            {
                LocSys.SetCompassData(CersioCt.hwCompass);
            }

            // 送受信文字更新
            tb_ResiveData.Text = CersioCt.hwResiveStr;
            tb_SendData.Text = CersioCt.hwSendStr;

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
                    sw.Write("handle:" + CersioCtrl.nowSendHandleValue + " / acc:" + CersioCtrl.nowSendAccValue + System.Environment.NewLine);
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
                                 "/Y " + tgtPosY.ToString("f3") + "/Dir " +
                                 tgtDir.ToString("f2") +
                                 System.Environment.NewLine);
                    }

                    // 特記事項メッセージ出力
                    if (Brain.addLogMsg != null)
                    {
                        sw.Write(Brain.addLogMsg + System.Environment.NewLine);
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
            // ※のちにGPS
            LocSys.ResetLPF_V1(LocSys.R1);

            RevisionProgBer.Value = 0;
            R1LocRevisionCnt = 20;
        }


    }
}
