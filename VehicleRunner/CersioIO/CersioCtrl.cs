//#define EMULATOR_MODE  // エミュレーション


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LocationPresumption;
using SCIP_library;


namespace CersioIO
{
    public class CersioCtrl
    {
        // ブレイン
        public Brain BrainCtrl;

        // SH2直結時のアクセル、ハンドルコントロール
        // ※現在使わない
        public DriveIOport DriveIO = new DriveIOport();

        // BOXPC通信用
        TCPClient objTCPSC = new TCPClient();

        // エミュレーション用　パッド操作
        static public bool LeftBtn = false;
        static public bool RightBtn = false;
        static public bool FwdBtn = false;

        // 最近　送信したハンドル、アクセル
        static public double nowSendHandleValue;
        static public double nowSendAccValue;


        // ハンドル、アクセル上限値
        public const double HandleRate = 0.8;  //<========== 調整 0.0 ～ 1.0(100%)
        public const double AccRate = 1.0;

        // ゴール到達フラグ
        public bool goalFlg = false;

        // HW
        public int hwCompass = 0;
        public bool bhwCompass = false;

        public double hwREX = 0.0;
        public double hwREY = 0.0;
        public double hwREDir = 0.0;
        public bool bhwREPlot = false;

        // 受信文字
        public string hwResiveStr;
        public string hwSendStr;


        // --------------------------------------------------------------------------------------------------
        public CersioCtrl()
        {
            BrainCtrl = new Brain(this);
            goalFlg = false;
        }

        /// <summary>
        /// 起動
        /// </summary>
        public void Start(bool bEmuMode )
        {
            BrainCtrl.Reset();
            goalFlg = false;

            // 回線オープン
            if (!bEmuMode)
            {
                objTCPSC.Start();
            }
        }

        /// <summary>
        /// 終了
        /// </summary>
        public void Close()
        {
            DriveIO.Close();
            objTCPSC.Dispose();
        }

        /// <summary>
        /// ロータリーエンコーダにスタート情報をセット
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dir"></param>
        public void RE_Reset(double x, double y, double dir)
        {
            //TCP_SendCommand("AD," + ((float)x).ToString("f") + "," + ((float)y).ToString("f") + "\n");
            TCP_SendCommand("AD,0.0,0.0\n");

            // 角度をパイに、回転の+-を調整
            double rad = -dir * Math.PI / 180.0;
            TCP_SendCommand("AR," + rad.ToString("f") + "\n");
        }


        // =====================================================================================

        /// <summary>
        /// 自己更新
        /// </summary>
        /// <param name="LocSys"></param>
        /// <param name="useEBS"></param>
        /// <returns></returns>
        public bool Update(LocPreSumpSystem LocSys, bool useEBS, bool useEHS)
        {
            hwSendStr = "";
            bhwREPlot = false;
            bhwCompass = false;

            LeftBtn = RightBtn = FwdBtn = false;

            if (!goalFlg)
            {
                // 自走処理
                BrainCtrl.Update(LocSys, useEHS);
            }


            if ((Brain.EmgBrk && useEBS) || goalFlg)
            {
                // 強制停止状態

                // エマージェンシー ブレーキ
                //handleValue = 0.0;
                nowSendAccValue = 0.0;

                if (TCP_IsConnected())
                {
                    // 停止情報送信
                    TCP_SendCommand("AC,0.0,0.0\n");

                    // 個々の取得コマンド
                    // REncoder
                    if (goalFlg)
                    {
                        // ゴールならスマイル
                        SetHeadMarkLED((int)CersioCtrl.LED_PATTERN.SMILE);
                    }
                    else
                    {
                        // 赤
                        SetHeadMarkLED((int)CersioCtrl.LED_PATTERN.RED);
                    }
                }
            }
            else
            {
                // 走行可能状態

                // ルート計算から、目標ハンドル、目標アクセル値をもらう。
                // 上限値をかける
                double handleTgt = BrainCtrl.getHandleValue() * HandleRate;
                double accTgt = BrainCtrl.getAccelValue() * AccRate;

                // ハンドル、アクセル操作を徐々に目的値に変更する
                nowSendHandleValue += (handleTgt - nowSendHandleValue) * 0.15;
                nowSendAccValue += (accTgt - nowSendAccValue) * 0.15;


#if EMULATOR_MODE
                // デジタルボタン操作に変換
                if (handleValue > 0.05) LeftBtn = true;
                if (handleValue < -0.05) RightBtn = true;

                if (accValue > 0.5) FwdBtn = true;
#endif

                // 送信
                if (TCP_IsConnected())
                {
                    double sHandleValue = nowSendHandleValue;
                    // ハンドルアクセル送信
                    TCP_SendCommand("AC," + sHandleValue.ToString("f") + "," + nowSendAccValue.ToString("f") + "\n");

                    if (sendALcom != null)
                    {
                        TCP_SendCommand(sendALcom);
                    }
                    sendALcom = null;
                }
            }

            goalFlg = BrainCtrl.RTS.getGoalFlg();
            return goalFlg;
        }

        // =====================================================================================
        /// <summary>
        /// ハードウェアステータス取得
        /// </summary>
        public void GetHWStatus()
        {
            if (TCP_IsConnected())
            {
                TCP_ReciveCommand();

                // ロータリーエンコーダ値(回転累計)
                TCP_SendCommand("A1" + "\n");

                // コンパス取得
                TCP_SendCommand("A2" + "\n");

                // ロータリーエンコーダ　絶対値取得
                TCP_SendCommand("A4" + "\n");

                // ロータリーエンコーダ　ハンドル値修正
                //double rad = (double)(-hwCompass) * Math.PI / 180.0;
                //TCP_SendCommand("AR," + rad.ToString("f") + "\n");
            }

            // カウンタ更新
            if (cntHeadLED > 0) cntHeadLED--;
        }


        // =====================================================================================
        /* patternは、0～9    (,dmyのところは、,だけでも可、dmyはなんでもok
        pattern
        0 通常表示   赤->北の方向、青->向いている方向、緑->北を向いている時
        1 全赤　　　　　　　　　緊急停止時
        2 全緑　　　　　　　　　
        3 全青　　　　　　　　　
        4 白黒点滅回転　　　　　チェックポイント通過時
        5 ?点滅回転　　　　　　　
        6 ?点滅回転　　　　　　　
        7 スマイル               ゴール時
        8 ハザード               回避、徐行時
        9 緑ＬＥＤぐるぐる       BoxPC起動時
        */
        private string sendALcom;

        public enum LED_PATTERN {
            Normal = 0,     // 0 通常表示
            RED,            // 1 全赤
            GREEN,          // 2 全緑
            BLUE,           // 3 全青
            WHITE_FLASH,    // 4 白黒点滅回転
            UnKnown1,       // 5 ?点滅回転
            UnKnown2,       // 6 ?点滅回転
            SMILE,          // 7 スマイル 
            HAZERD,         // 8 ハザード
            ROT_GREEN,      // 9 緑ＬＥＤぐるぐる
        };
        private int ptnHeadLED = -1;
        private int cntHeadLED = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setPattern"></param>
        /// <param name="bForce">強制変更</param>
        /// <returns>変更したor済み..True</returns>
        public bool SetHeadMarkLED(int setPattern, bool bForce=false )
        {
            if (bForce || (ptnHeadLED != setPattern && cntHeadLED == 0))
            {
                //TCP_SendCommand("AL," + setPattern.ToString() + ",\n");
                sendALcom = "AL," + setPattern.ToString() + ",\n";

                cntHeadLED = 10 * 1;          // しばらく変更しない
                ptnHeadLED = setPattern;
                return true;
            }


            if (ptnHeadLED == setPattern)
            {
                cntHeadLED = 10 * 1;          // 占有時間延長
                return true;
            }
            return false;
        }

        //--------------------------------------------------------------------------------------------------------------
        // BoxPC通信

        /// <summary>
        /// コマンド送信
        /// </summary>
        /// <param name="comStr"></param>
        public void TCP_SendCommand(string comStr)
        {
            System.Net.Sockets.NetworkStream objStm = objTCPSC.MyProperty;

            if (objStm != null)
            {
                Byte[] dat = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetBytes(comStr);
                objStm.Write(dat, 0, dat.GetLength(0));

                hwSendStr += "/" + comStr;
            }
        }


        public static int SpeedMH = 0;   // 速度　mm/Sec
        double oldResiveMS;         // 速度計測用 受信時間差分
        int oldWheelR;              // 速度計測用　前回ロータリーエンコーダ値

        public string TCP_ReciveCommand()
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
                    hwResiveStr = readStr;

                    {
                        string[] rsvCmd = readStr.Split('$');

                        SpeedMH = 0;   // 未計測
                        for (int i = 0; i < rsvCmd.Length; i++)
                        {
                            if (rsvCmd[i].Length <= 3) continue;

                            // ロータリーエンコーダから　速度を計算
                            if (rsvCmd[i].Substring(0, 3) == "A1,")
                            {
                                double ResiveMS;
                                int wheelR;
                                //string[] splStr = (rsvCmd[i].Replace('[', ',').Replace(']', ',').Replace(' ', ',')).Split(',');
                                string[] splStr = rsvCmd[i].Split(',');

                                // 0 A1
                                double.TryParse(splStr[1], out ResiveMS); // ms? 万ミリ秒に思える
                                int.TryParse(splStr[2], out wheelR);        // Right Wheel

                                // 絶対値用計算
                                SpeedMH = (int)(((double)(wheelR - oldWheelR)/260.0 * Math.PI*140.0)*10000.0/(ResiveMS - oldResiveMS));
                                // mm/sec
                                //SpeedMH = (int)(((double)wheelR / 260.0 * Math.PI * 140.0) * 10000.0 / 200.0);

                                oldResiveMS = ResiveMS;
                                oldWheelR = wheelR;
                            }
                            else if (rsvCmd[i].Substring(0, 3) == "A2,")
                            {
                                // コンパス情報
                                // A2,22.5068,210$
                                double ResiveMS;
                                int ResiveCmp;
                                string[] splStr = rsvCmd[i].Split(',');

                                // splStr[0] "A2"
                                // ミリ秒取得
                                double.TryParse(splStr[1], out ResiveMS); // ms? 万ミリ秒に思える
                                int.TryParse(splStr[2], out ResiveCmp);   // デジタルコンパス値
                                hwCompass = -ResiveCmp;     // 回転方向が+-逆なので合わせる
                                bhwCompass = true;
                            }
                            else if (rsvCmd[i].Substring(0, 3) == "A4,")
                            {
                                // ロータリーエンコーダ　絶対値
                                // 開始時　真北基準
                                /*
                                 * コマンド
                                    A4
                                    ↓
                                    戻り値
                                    A4,絶対座標X,絶対座標Y,絶対座標上での向きR$

                                    絶対座標X[mm]
                                    絶対座標Y[mm]
                                    絶対座標上での向き[rad]　-2π～2π
                                    浮動小数点です。
                                    */
                                double ResiveMS;
                                double ResiveX;
                                double ResiveY;
                                double ResiveRad;

                                string[] splStr = rsvCmd[i].Split(',');

                                // splStr[0] "A2"
                                // ミリ秒取得
                                double.TryParse(splStr[1], out ResiveMS); // ms? 万ミリ秒に思える
                                double.TryParse(splStr[2], out ResiveX);  // 雑体座標X mm
                                double.TryParse(splStr[3], out ResiveY);  // 雑体座標Y mm
                                double.TryParse(splStr[4], out ResiveRad);  // 向き -2PI 2PI

                                // 座標系変換
                                // 右上から右下へ
                                hwREX = ResiveX;
                                hwREY = -ResiveY;
                                hwREDir = -ResiveRad * 180.0 / Math.PI;

                                // ※座標回転
                                /*
                                {
                                    double wRad = ResiveRad;
                                    double cs = Math.Cos(wRad);
                                    double sn = Math.Sin(wRad);

                                    hwREX = (ResiveX * cs - ResiveY * sn);
                                    hwREY = (ResiveX * sn + ResiveY * cs);
                                    hwREY = -hwREY;
                                }
                                */

                                bhwREPlot = true;
                            }

                        }
                    }
                }
            }

            return readStr;
        }

        /// <summary>
        /// BoxPCとの通信状態をかえす
        /// </summary>
        /// <returns></returns>
        public bool TCP_IsConnected()
        {
            System.Net.Sockets.TcpClient objSck = objTCPSC.SckProperty;
            System.Net.Sockets.NetworkStream objStm = objTCPSC.MyProperty;

            if (objStm != null && objSck != null)
            {
                return true;
            }
            return false;
        }



    }
}
