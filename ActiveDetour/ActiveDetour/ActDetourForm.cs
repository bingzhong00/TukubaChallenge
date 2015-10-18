using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using testTCP;

namespace ActiveDetour
{
    public partial class ActiveDetourMonitor : Form
    {
        // LRFから荒めのピクセル(250mmx250mm?)に変換
        // ピクセルに強度を持たせ、初期値0.5から時間経過で減衰していく（ノイズ除去）
        // なるべく前進し、あいてる２ピクセルをルートとして検索
        // 一度ルートに選定されたピクセルは、強度がある程度以上にならない限りは、推奨ルート値を持つ（ジレンマ対策）
        // 車体に近いところほど、推奨ルート値が減少しない。

        ActiveDetour ad;

        TCPClient objTCPSC = new TCPClient();

        public ActiveDetourMonitor()
        {
            InitializeComponent();

            ad = new ActiveDetour();
            pictureBox1.Image = ad.GetBmp();
            tmr_upDateLRF.Enabled = true;
            tmr_upDate.Enabled = true;

            if (objTCPSC.Start())
            {
                lbl_BoxPCConnect.Text = "BoxPc Connected. OK";
            }
            if (ad.LRF_IPConnectFlg)
            {
                lbl_LrfConnect.Text = "LRF Connected. OK";
            }

            tb_HandleRange_Scroll(null, null);
        }

        private void tmr_upDate_Tick(object sender, EventArgs e)
        {

            if (cb_EmgStop.Checked)
            {
                // 緊急停止ボタン ON
                ad.EmgBrakeON();

                tb_Handle.Text = "0.0";
                tb_Accel.Text = "0.0";

                tb_Accel.BackColor = Color.Red;
                tb_Handle.BackColor = Color.Red;

                if (TCP_IsConnected())
                {
                    // 停止情報送信
                    TCP_SendCommand("AC,0.0,0.0\n");


                    tb_ResiveData.Text = TCP_ReciveCommand();

                    // 個々の取得コマンド
                    // REncoder
                    if (false)
                    {
                        TCP_SendCommand("A1" + "\n");
                        SetHeadMarkLED(1);
                    }
                }

            }
            else
            {
                // 通常処理

                ad.Update();
                pictureBox1.Invalidate();

                tb_Accel.BackColor = SystemColors.Window;
                tb_Handle.BackColor = SystemColors.Window;

                if (ad.GetEmgBrake())
                {
                    // ActiveDetourからの緊急停止
                    tb_Handle.Text = "0.0";
                    tb_Accel.Text = "0.0";

                    tb_Accel.BackColor = Color.Red;
                }
                else
                {
                    tb_Handle.Text = ad.GetHandle().ToString();
                    tb_Accel.Text = ad.GetAccel().ToString();
                }

                // アクセルのみ0.0
                if (cb_AccelOff.Checked)
                {
                    tb_Accel.Text = "0.0";
                }

                {
                    if (TCP_IsConnected())
                    {
                        tb_ResiveData.Text = TCP_ReciveCommand();

                        // 個々の取得コマンド
                        // REncoder
                        if (false)
                        {
                            TCP_SendCommand("A1" + "\n");
                        }

                        // ハンドル、アクセル 制御コマンド
                        if (!string.IsNullOrEmpty(tb_Handle.Text) && !string.IsNullOrEmpty(tb_Accel.Text))
                        {
                            TCP_SendCommand("AC," + tb_Handle.Text + "," + tb_Accel.Text + "\n");
                        }

                        // ヘッドマークLED
                        if (ad.GetEmgBrake())
                        {
                            /* patternは、0～9    (,dmyのところは、,だけでも可、dmyはなんでもok
                                pattern
                                0 通常表示   赤->北の方向、青->向いている方向、緑->北を向いている時
                                1 全赤
                                2 全緑
                                3 全青
                                4 白黒点滅回転
                                5 ?点滅回転
                                6 ?点滅回転
                                7 スマイル
                                8 ハザード
                                9 緑ＬＥＤぐるぐる
                             */
                            SetHeadMarkLED(1);
                        }
                        else
                        {
                            SetHeadMarkLED(4);
                        }
                    }
                }
            }

            // 速度表示
            if (SpeedMH < 0)
            {
                lbl_Speed.Text = "Spped:UnKnown";
            }
            else
            {
                lbl_Speed.Text = "Spped:" + SpeedMH.ToString() + "mm/s";
            }
        }


        private int ptnHeadLED = -1;
        /// <summary>
        /// LED表示
        /// </summary>
        /// <param name="setPattern"></param>
        private void SetHeadMarkLED(int setPattern )
        {
            if (ptnHeadLED != setPattern)
            {
                TCP_SendCommand("AL," + setPattern.ToString() + ",\n");
            }
            ptnHeadLED = setPattern;
        }

        /// <summary>
        /// コマンド送信
        /// </summary>
        /// <param name="comStr"></param>
        private void TCP_SendCommand(string comStr)
        {
            System.Net.Sockets.NetworkStream objStm = objTCPSC.MyProperty;

            if (objStm != null)
            {
                Byte[] dat = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetBytes(comStr);
                objStm.Write(dat, 0, dat.GetLength(0));
            }
        }


        private int SpeedMH = -1;   // 速度　mm/Sec
        double oldResiveMS;         // 速度計測用 受信時間差分
        int oldWheelR;              // 速度計測用　前回ロータリーエンコーダ値

        private string TCP_ReciveCommand()
        {
            System.Net.Sockets.TcpClient objSck = objTCPSC.SckProperty;
            System.Net.Sockets.NetworkStream objStm = objTCPSC.MyProperty;
            string readStr = "";

            if (objStm != null && objSck != null)
            {
                // ソケット受信
                if (objSck.Available > 0)
                {
                    Byte[] dat = new Byte[objSck.Available];

                    objStm.Read(dat, 0, dat.GetLength(0));

                    readStr = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetString(dat);

                    {
                        string[] rsvCmd = readStr.Split('$');

                        SpeedMH = -1;   // 未計測
                        for (int i = 0; i < rsvCmd.Length; i++)
                        {
                            // ロータリーエンコーダから　速度を計算
                            if (rsvCmd[i].Length > 3 && rsvCmd[i].Substring(0, 3) == "A1,")
                            {
                                double ResiveMS;
                                int wheelR;
                                string[] splStr = (rsvCmd[i].Replace('[', ',').Replace(']', ',').Replace(' ', ',')).Split(',');

                                // 0 A1
                                double.TryParse(splStr[1], out ResiveMS); // ms? 万ミリ秒に思える
                                int.TryParse(splStr[2], out wheelR);        // Right Wheel

                                // 絶対値用計算
                                //SpeedMH = (int)(((double)(wheelR - oldWheelR)/260.0 * Math.PI*140.0)*10000.0/(ResiveMS - oldResiveMS));
                                // mm/sec
                                SpeedMH = (int)(((double)wheelR / 260.0 * Math.PI * 140.0) * 10000.0 / 200.0);

                                oldResiveMS = ResiveMS;
                                oldWheelR = wheelR;
                            }
                        }
                    }
                }
            }

            return readStr;
        }

        private bool TCP_IsConnected()
        {
            System.Net.Sockets.TcpClient objSck = objTCPSC.SckProperty;
            System.Net.Sockets.NetworkStream objStm = objTCPSC.MyProperty;

            if (objStm != null && objSck != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 緊急停止ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_EmgStop_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_EmgStop.Checked)
            {
                ad.EmgBrakeON();
                cb_EmgStop.ForeColor = Color.Red;
            }
            else
            {
                cb_EmgStop.ForeColor = SystemColors.ControlText;
            }
        }

        private void btn_LRFConnect_Click(object sender, EventArgs e)
        {
            if (ad.ConnectLRF())
            {
                lbl_LrfConnect.Text = "LRF Connected. OK";
            }
            else
            {
                lbl_LrfConnect.Text = "LRF Connect Fail";
            }
        }

        private void tb_HandleRange_Scroll(object sender, EventArgs e)
        {
            double val = tb_HandleRange.Value;
            val = val * 0.1;
            lbl_HandleRange.Text = "HandleRange " + val.ToString();

            if (null != ad)
            {
                ad.SetHandleRange(val);
            }
        }

        /// <summary>
        /// BoxPC接続ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_BoxPcConnect_Click(object sender, EventArgs e)
        {
            if (objTCPSC.Start())
            {
                lbl_BoxPCConnect.Text = "BoxPc Connected. OK";
            }
        }

        /// <summary>
        /// LRF更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmr_upDateLRF_Tick(object sender, EventArgs e)
        {
            if (null != ad)
            {
                ad.UpdateLRF();
            }
        }

    }
}
