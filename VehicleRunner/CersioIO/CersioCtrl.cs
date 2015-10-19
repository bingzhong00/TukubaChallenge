//#define EMULATOR_MODE  // エミュレーション
//#define SEND_A1_COMMAND   // A1コマンドを投げる

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SCIP_library;


namespace CersioIO
{
    public class CersioCtrl
    {
        public Rooting RTS = new Rooting();
        public DriveIOport DriveIO = new DriveIOport();

        TCPClient objTCPSC = new TCPClient();

        // エミュレーション用　パッド操作
        static public bool LeftBtn = false;
        static public bool RightBtn = false;
        static public bool FwdBtn = false;

        // エマージェンジーブレーキ
        static public bool EmgBrk = false;

        // ハンドル、アクセル操作
        static public double handleValue;
        static public double accValue;
        static private int AccelSlowDownCnt;

        // ハンドル、アクセル上限値
        private const double HandleRate = 1.0;  //<========== 調整
        private const double AccRate = 1.0;

        // スローダウン時のアクセル上限
        private const double AccSlowdownRate = 0.4;

        // ゴール到達フラグ
        public bool goalFlg = false;

        // HW
        public int hwCompass = 0;
        public double hwREX = 0.0;
        public double hwREY = 0.0;
        public double hwRERad = 0.0;

        // 受信文字
        public string hwResiveStr;
        public string hwSendStr;

        // 緊急ブレーキ距離
        public const double EmgBrakgeRange = 1100.0;

        public int checkpntIdx;

        public void Start()
        {
            RTS.ResetSeq();
            
            handleValue = 0.0;
            accValue = 0.0;
            checkpntIdx = 0;

            // 回線オープン
            objTCPSC.Start();
        }

        public void Close()
        {
            DriveIO.Close();
            objTCPSC.Dispose();
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="ang"></param>
        /// <param name="lrfData"></param>
        /// <param name="useEBS"></param>
        /// <returns></returns>
        public bool Update(int x, int y, double ang, double[] lrfData, bool useEBS )
        {
            hwSendStr = "";

            LeftBtn = RightBtn = FwdBtn = false;

            // 現在座標更新
            RTS.setNowPostion(x, y, ang);
            // ルート計算
            RTS.calcRooting();

            // エマージェンシー ブレーキチェック
            EmgBrk = false;
            if (null != lrfData && lrfData.Length > 0)
            {
                EmgBrk = CheckEBS(lrfData);
            }

            if ((EmgBrk && useEBS) || goalFlg)
            {
                // エマージェンシー ブレーキ
                handleValue = 0.0;
                accValue = 0.0;

                if (TCP_IsConnected())
                {
                    // 停止情報送信
                    TCP_SendCommand("AC,0.0,0.0\n");

                    //TCP_ReciveCommand();

                    // 個々の取得コマンド
                    // REncoder
#if SEND_A1_COMMAND
                    if (false)
                    {
                        TCP_SendCommand("A1" + "\n");
                    }
#endif
                    if (goalFlg)
                    {
                        // ゴールならスマイル
                        SetHeadMarkLED(7);
                    }
                    else
                    {
                        // 赤
                        SetHeadMarkLED(1);
                    }
                }
            }
            else
            {
                // ルート計算から、目標ハンドル、目標アクセル値をもらう。
                double handleTgt = RTS.getHandleValue() * HandleRate;
                double accTgt = RTS.getAccelValue() * AccRate;


                // スローダウン
                if (AccelSlowDownCnt > 0)
                {
                    accValue = accTgt = RTS.getAccelValue() * AccRate * AccSlowdownRate;
                    AccelSlowDownCnt--;
                }

                // ハンドル、アクセル操作を徐々に目的値に変更する
                handleValue += (handleTgt - handleValue) * 0.15;
                accValue += (accTgt - accValue) * 0.15;


#if EMULATOR_MODE
                // デジタルボタン操作に変換
                if (handleValue > 0.05) LeftBtn = true;
                if (handleValue < -0.05) RightBtn = true;

                if (accValue > 0.5) FwdBtn = true;
#endif

                // 送信
                if (TCP_IsConnected())
                {
                    double sHandleValue = handleValue;
                    // ハンドルアクセル送信
                    TCP_SendCommand("AC," + sHandleValue.ToString("f") + "," + accValue.ToString("f") + "\n");

                    /*
                    // コンパス取得
                    TCP_SendCommand("A2" + "\n");

                    // ロータリーエンコーダ　絶対値取得
                    TCP_SendCommand("A4" + "\n");

                    double rad = (double)(-hwCompass) * Math.PI / 180.0;
                    TCP_SendCommand("AR," + rad.ToString("f") + "\n");
                    */

                    // 結果受信
                    //TCP_ReciveCommand();

#if SEND_A1_COMMAND
                    if (false)
                    {
                        TCP_SendCommand("A1" + "\n");
                    }
#endif
                    // ＬＥＤパターン平常
                    SetHeadMarkLED(0);
                }
            }

            // チェックポイント通過をLEDで伝える
            {
                int nowIdx = RTS.GetNowCheckPointIdx();
                if (checkpntIdx != nowIdx)
                {
                    SetHeadMarkLED(4, true);
                    checkpntIdx = nowIdx;
                }
            }

            return RTS.getGoalFlg();
        }

        /// <summary>
        /// ハードウェアステータス取得
        /// </summary>
        public void GetHWStatus()
        {
            if (TCP_IsConnected())
            {
                TCP_ReciveCommand();

                // コンパス取得
                TCP_SendCommand("A2" + "\n");

                // ロータリーエンコーダ　絶対値取得
                TCP_SendCommand("A4" + "\n");

                //double rad = (double)(-hwCompass) * Math.PI / 180.0;
                //TCP_SendCommand("AR," + rad.ToString("f") + "\n");
            }

            // カウンタ更新
            if (cntHeadLED > 0) cntHeadLED--;
        }

        // セルシオにパッド情報をおくる


        // ※EBS 自動ブレーキ
        // LRFの値を取得して、前方に何かあれば範囲とともに記録、次のときも存在しつつければ注意値を加算していく。
        // 一定上の注意値になったら、ブレーキを指示
        public bool CheckEBS( double[] lrfData)
        {
            int rangeAngHalf = 270/2;
            int stAng = -15;
            int edAng = 15;

            for (int i = (stAng+rangeAngHalf); i < (edAng+rangeAngHalf); i++)
            {
                // 以下ならとまる
                if (lrfData[i] < (EmgBrakgeRange*URG_LRF.getScale())) return true;
            }
            return false;
        }

        // =====================================================================================
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
                TCP_SendCommand("AL," + setPattern.ToString() + ",\n");
                cntHeadLED = 20*1;          // しばらく変更しない
                ptnHeadLED = setPattern;
                return true;
            }

            if (ptnHeadLED == setPattern) return true;
            return false;
        }

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


        private int SpeedMH = -1;   // 速度　mm/Sec
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

                        SpeedMH = -1;   // 未計測
                        for (int i = 0; i < rsvCmd.Length; i++)
                        {
                            if (rsvCmd[i].Length <= 3) continue;

                            // ロータリーエンコーダから　速度を計算
                            if (rsvCmd[i].Substring(0, 3) == "A1,")
                            {
                                double ResiveMS;
                                //int wheelR;
                                string[] splStr = (rsvCmd[i].Replace('[', ',').Replace(']', ',').Replace(' ', ',')).Split(',');

                                // 0 A1
                                double.TryParse(splStr[1], out ResiveMS); // ms? 万ミリ秒に思える
                                //int.TryParse(splStr[2], out wheelR);        // Right Wheel

                                // 絶対値用計算
                                //SpeedMH = (int)(((double)(wheelR - oldWheelR)/260.0 * Math.PI*140.0)*10000.0/(ResiveMS - oldResiveMS));
                                // mm/sec
                                //SpeedMH = (int)(((double)wheelR / 260.0 * Math.PI * 140.0) * 10000.0 / 200.0);

                                //oldResiveMS = ResiveMS;
                                //oldWheelR = wheelR;
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
                                hwREX = ResiveX;
                                hwREY = -ResiveY;
                                hwRERad = -ResiveRad * 180.0 / Math.PI;
                            }

                        }
                    }
                }
            }

            return readStr;
        }

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
