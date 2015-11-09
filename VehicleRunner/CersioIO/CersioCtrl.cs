//#define EMULATOR_MODE  // エミュレーション


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using LocationPresumption;
using SCIP_library;


namespace CersioIO
{
    public class CersioCtrl
    {
        // ブレイン
        public Brain BrainCtrl;

        // SH2直結時のアクセル、ハンドルコントロール
        public DriveIOport UsbDriveIO;

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
        public const double HandleRate = 1.0;//0.8;  //<========== 調整 0.0 ～ 1.0(100%)
        public const double AccRate =0.5;// 1.0;

        // ハンドル、アクセルの変化係数
        public const double HandleControlPow = 0.125; // 0.15;
        public const double AccControlPowUP = 0.100; // 0.15;   // 加速時は緩やかに
        public const double AccControlPowDOWN = 0.150;

        // ゴール到達フラグ
        public bool goalFlg = false;

        // HW
        public int hwCompass = 0;
        public bool bhwCompass = false;

        public double hwREX = 0.0;
        public double hwREY = 0.0;
        public double hwREDir = 0.0;
        public bool bhwREPlot = false;

        public double hwRErotR = 0.0;
        public double hwRErotL = 0.0;
        public bool bhwRE = false;

        public double hwGPS_LandX = 0.0;
        public double hwGPS_LandY = 0.0;
        public double hwGPS_MoveDir = 0.0;  // 0 ～ 359度
        public bool bhwGPS = false;

        // 受信文字
        public string hwResiveStr;
        public string hwSendStr;


        public List<string> usbGPSResive = new List<string>();

        // --------------------------------------------------------------------------------------------------
        public CersioCtrl()
        {
            BrainCtrl = new Brain(this);
            goalFlg = false;

            //UsbDriveIO = new DriveIOport();      // USB SH2通信 ※現在使わない

            // GPS初期位置設定
            //LocPreSumpSystem.SetStartGPS(RootingData.GPS_LandX, RootingData.GPS_LandY);
        }

        /// <summary>
        /// 起動
        /// </summary>
        public void Start()
        {
            BrainCtrl.Reset();
            goalFlg = false;

        }

        /// <summary>
        /// 終了
        /// </summary>
        public void Close()
        {
            if (TCP_IsConnected())
            {
                // 動力停止
                TCP_SendCommand("AC,0.0,0.0\n");
                System.Threading.Thread.Sleep(50);
                // LEDを戻す
                TCP_SendCommand("AL,0,\n");
                System.Threading.Thread.Sleep(50);
            }

            if (null != UsbDriveIO)
            {
                UsbDriveIO.Close();
            }

            objTCPSC.Dispose();
        }

        /// <summary>
        /// BoxPcと接続
        /// </summary>
        /// <returns></returns>
        public bool ConnectBoxPC()
        {
            if (TCP_IsConnected())
            {
                objTCPSC.Dispose();
                System.Threading.Thread.Sleep(100);
            }

            // 回線オープン
            return objTCPSC.Start();
        }

        /// <summary>
        /// ロータリーエンコーダにスタート情報をセット
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dir"></param>
        public void RE_Reset(double x, double y, double dir)
        {
            SendCommand("AD," + ((float)x).ToString("f") + "," + ((float)y).ToString("f") + "\n");
            //SendCommand("AD,0.0,0.0\n");

            // 角度をパイに、回転の+-を調整
            double rad = -dir * Math.PI / 180.0;
            SendCommand("AR," + rad.ToString("f") + "\n");
        }

        /// <summary>
        /// RE回転のみリセット
        /// </summary>
        /// <param name="dir"></param>
        public void RE_Reset(double dir)
        {
            // 角度をパイに、回転の+-を調整
            double rad = -dir * Math.PI / 180.0;
            SendCommand("AR," + rad.ToString("f") + "\n");
        }


        // =====================================================================================

        /// <summary>
        /// 自己更新
        /// </summary>
        /// <param name="LocSys"></param>
        /// <param name="useEBS"></param>
        /// <returns></returns>
        public bool Update(LocPreSumpSystem LocSys, bool useEBS, bool useEHS, bool bLocRivisionTRG, bool useAlwaysPF )
        {
            bool untiEBS = false;

 
            //bhwREPlot = false;
            //bhwCompass = false;

            LeftBtn = RightBtn = FwdBtn = false;

            if (!goalFlg)
            {
                // 自走処理
                untiEBS = BrainCtrl.Update(LocSys, useEBS, useEHS, bLocRivisionTRG, useAlwaysPF);
            }
            
            // 直進用　常にスタート時の方位を向いてることにする。
            RE_Reset(RootingData.startDir);

            if ((Brain.EmgBrk && useEBS && !untiEBS) || goalFlg)
            {
                // 強制停止状態

                // エマージェンシー ブレーキ
                //handleValue = 0.0;
                nowSendAccValue = 0.0;

                if (TCP_IsConnected())
                {
                    // 停止情報送信
                    SetCommandAC(0.0, 0.0);

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
                        SetHeadMarkLED((int)CersioCtrl.LED_PATTERN.RED, true);
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
                double diffAcc = (accTgt - nowSendAccValue);

                // ハンドル、アクセル操作を徐々に目的値に変更する
                nowSendHandleValue += (handleTgt - nowSendHandleValue) * HandleControlPow;
                nowSendAccValue += ((diffAcc > 0.0) ? (diffAcc * AccControlPowUP) : (diffAcc * AccControlPowDOWN));   


#if EMULATOR_MODE
                // デジタルボタン操作に変換
                if (handleValue > 0.05) LeftBtn = true;
                if (handleValue < -0.05) RightBtn = true;

                if (accValue > 0.5) FwdBtn = true;
#endif

                // 送信
                SetCommandAC(nowSendHandleValue, nowSendAccValue);
            }

            goalFlg = BrainCtrl.RTS.getGoalFlg();
            return goalFlg;
        }

        // =====================================================================================
        /// <summary>
        /// ハードウェアステータス取得
        /// </summary>
        /// <param name="useUsbGPS">USBのＧＰＳを使っている</param>
        public void GetHWStatus( bool useUsbGPS )
        {
            /*
            hwGPS_LandX = emuGPSX;
            hwGPS_LandY = emuGPSY;
            bhwGPS = true;

            emuGPSX += 0.00001;//2cm
            */

            if (TCP_IsConnected())
            {
                TCP_ReciveCommand();

                // ロータリーエンコーダ値(回転累計)
                SendCommand("A1" + "\n");

                // コンパス取得
                SendCommand("A2" + "\n");

                // GPS取得
                if (!useUsbGPS)
                {
                    SendCommand("A3" + "\n");
                }

                // ロータリーエンコーダ　絶対値取得
                SendCommand("A4" + "\n");

                // ロータリーエンコーダ　ハンドル値修正
                //double rad = (double)(-hwCompass) * Math.PI / 180.0;
                //TCP_SendCommand("AR," + rad.ToString("f") + "\n");
            }

            //usbGPS取得
            if( useUsbGPS ) AnalizeUsbGPS();

            // カウンタ更新
            if (cntHeadLED > 0) cntHeadLED--;
        }


        /// <summary>
        /// ACコマンド発行
        /// </summary>
        /// <param name="sendHandle"></param>
        /// <param name="sendAcc"></param>
        public void SetCommandAC( double sendHandle, double sendAcc )
        {
            if (TCP_IsConnected())
            {
                // LAN接続
                SendCommand("AC," + sendHandle.ToString("f2") + "," + sendAcc.ToString("f2") + "\n");
            }
            else
            {
                // USB接続時
                if (null != UsbDriveIO)
                {
                    if (UsbDriveIO.IsConnect())
                    {
                        UsbDriveIO.SendSirialData(sendHandle, sendAcc);
                    }
                }
            }
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
        public int ptnHeadLED = -1;
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
                SendCommand("AL," + setPattern.ToString() + ",\n");

                cntHeadLED = 20 * 1;          // しばらく変更しない
                ptnHeadLED = setPattern;
                return true;
            }


            // 通常以外は、現在設定中でさらに送られてきたら延長
            if (setPattern != (int)LED_PATTERN.Normal && ptnHeadLED == setPattern)
            {
                cntHeadLED = 10 * 1;          // 占有時間延長
                return true;
            }
            return false;
        }

        public static string[] LEDMessage = { "Normal", "RED:緊急停止", "GREEN", "BLUE:壁回避", "WHITE_FLASH:チェックポイント通過", "UnKnown1", "UnKnown2", "SMILE:ルート完了", "HAZERD:徐行", "ROT_GREEN" };


        //--------------------------------------------------------------------------------------------------------------
        // BoxPC通信
        

        public List<string> SendCommandList = new List<string>();
        /// <summary>
        /// コマンド分割送信
        /// </summary>
        public void SendCommandTick()
        {
            if (TCP_IsConnected())
            {
                // 先頭から順に送信
                if (SendCommandList.Count > 0)
                {
                    TCP_SendCommand(SendCommandList[0]);
                    SendCommandList.RemoveAt(0);
                }

                // もし、たまりすぎたら捨てる
                if (SendCommandList.Count > 100)
                {
                    SendCommandList.Clear();
                    Brain.addLogMsg += "SendCommandTick: OverFlow!\n";
                }
            }
            else
            {
                // 接続されていないなら、リストをクリア
                SendCommandList.Clear();
            }
        }

        public void SendCommand( string comStr )
        {
            SendCommandList.Add(comStr);
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

                try
                {
                    objStm.Write(dat, 0, dat.GetLength(0));
                }
                catch (Exception e)
                {
                    // 接続エラー
                    Brain.addLogMsg += "TCP_SendCommand:Error "+e.Message+"\n";

                    objTCPSC.DisConnect();
                }

                hwSendStr += "/" + comStr;
            }
        }


        public static int SpeedMH = 0;   // 速度　mm/Sec
        double oldResiveMS;         // 速度計測用 受信時間差分
        double oldWheelR;              // 速度計測用　前回ロータリーエンコーダ値

        public double emuGPSX = 134.0000;
        public double emuGPSY = 35.0000;

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
                                const double tiyeSize = 140.0;  // タイヤ直径 [mm]
                                const double OnePuls = 260.0;   // 一周のパルス数
                                double ResiveMS;
                                //string[] splStr = (rsvCmd[i].Replace('[', ',').Replace(']', ',').Replace(' ', ',')).Split(',');
                                string[] splStr = rsvCmd[i].Split(',');

                                // 0 A1
                                double.TryParse(splStr[1], out ResiveMS); // ms? 万ミリ秒に思える
                                double.TryParse(splStr[2], out hwRErotR);        // Right Wheel
                                double.TryParse(splStr[3], out hwRErotL);        // Left Wheel

                                // 絶対値用計算 10000  万ミリ秒
                                SpeedMH = (int)(((double)(hwRErotR - oldWheelR) / OnePuls * Math.PI * tiyeSize) * 10000.0 / (ResiveMS - oldResiveMS));
                                // mm/sec
                                //SpeedMH = (int)(((double)wheelR / 260.0 * Math.PI * 140.0) * 10000.0 / 200.0);

                                oldResiveMS = ResiveMS;
                                oldWheelR = hwRErotR;

                                bhwRE = true;
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
                            else if (rsvCmd[i].Substring(0, 3) == "A3,")
                            {
                                // GPS情報
                                // $A3,38.266,36.8002,140.11559$
                                double ResiveMS;
                                double ResiveLandX; // 緯度
                                double ResiveLandY; // 経度
                                string[] splStr = rsvCmd[i].Split(',');

                                // splStr[0] "A3"
                                // ミリ秒取得
                                double.TryParse(splStr[1], out ResiveMS); // ms? 万ミリ秒に思える

                                double.TryParse(splStr[2], out ResiveLandX);   // GPS値
                                double.TryParse(splStr[3], out ResiveLandY);
                                hwGPS_LandX = ResiveLandX;
                                hwGPS_LandY = ResiveLandY;
                                bhwGPS = true;
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


        // -----------------------------------------------------------------------------------------------
        //const double GPSScale = 1.852 * 1000.0 * 1000.0;

        //const double GPSScaleX = 1.51985 * 1000.0 * 1000.0;    // 経度係数  35度時
        //const double GPSScaleY = 1.85225 * 1000.0 * 1000.0;    // 緯度係数

        private bool AnalizeUsbGPS()
        {
            if (usbGPSResive.Count <= 10) return false;

            string strBuf = "";

            foreach (var lineStr in usbGPSResive)
            {
                strBuf += lineStr;
            }

            {
                byte[] dat = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetBytes(strBuf);
                MemoryStream mst = new MemoryStream(dat, false);
                StreamReader fsr = new StreamReader(mst, Encoding.GetEncoding("Shift_JIS"));

                string str;

                do
                {
                    str = fsr.ReadLine();

                    if (str == null) break;
                    if (str.Length == 0) continue;

                    string[] dataWord = str.Split(',');



                    switch (dataWord[0])
                    {
                        case "$GPRMC":
                            try
                            {
                                // $GPRMC,020850.000,A,3604.8100,N,14006.9366,E,0.00,15.03,171015,,,A*54
                                if (dataWord.Length < 13) break;

                                double ido = 0.0;

                                // 定世界時(UTC）での時刻。日本標準時は協定世界時より9時間進んでいる。hhmmss.ss
                                //lsData.ms = ParseGPS_MS(dataWord[1]);

                                // dataWord[2] A,V  ステータス。V = 警告、A = 有効
                                if (dataWord[2].ToUpper() != "A") break;    // 受信不良時は受け取らない

                                {
                                    // dataWord[3] 緯度。dddmm.mmmm
                                    string[] dataGPS = dataWord[3].Split('.');
                                    //ido = double.Parse(dataGPS[0]);
                                    //lsData.GPSLandY = -(double.Parse(dataGPS[0]) + (double.Parse(dataGPS[1])/60.0)) * GPSScale;
                                    string strDo = dataWord[3].Substring(0, dataGPS[0].Length - 2);
                                    string strHun = dataWord[3].Substring(strDo.Length, dataWord[3].Length - strDo.Length);

                                    hwGPS_LandY = double.Parse(strDo) + (double.Parse(strHun) / 60.0);
                                    //hwGPS_LandY = -(double.Parse(strDo) * 60.0 + (double.Parse(strHun))) * GPSScale;  // MAP座標系
                                    ido = double.Parse(strDo);
                                }
                                // dataWord[4] N,S N = 北緯、South = 南緯

                                {
                                    // dataWord[5] 経度。dddmm.mmmm
                                    string[] dataGPS = dataWord[5].Split('.');

                                    //lsData.GPSLandX = (double.Parse(dataGPS[0]) + (double.Parse(dataGPS[1])) / 60.0) * (GPSScale * Math.Cos((ido / 100.0) * Math.PI / 180.0));
                                    string strDo = dataWord[5].Substring(0, dataGPS[0].Length - 2);
                                    string strHun = dataWord[5].Substring(strDo.Length, dataWord[5].Length - strDo.Length);

                                    hwGPS_LandX = double.Parse(strDo) + (double.Parse(strHun) / 60.0);
                                    //hwGPS_LandX = (double.Parse(strDo) * 60.0 + (double.Parse(strHun))) * (GPSScale * Math.Cos(ido * Math.PI / 180.0));  // MAP座標系
                                }
                                // dataWord[6] E = 東経、West = 西経

                                // dataWord[7] 地表における移動の速度。000.0～999.9[knot]
                                // dataWord[8] 地表における移動の真方位。000.0～359.9度
                                hwGPS_MoveDir = -double.Parse(dataWord[8]);

                                // dataWord[9] 協定世界時(UTC）での日付。ddmmyy
                                // dataWord[10] 磁北と真北の間の角度の差。000.0～359.9度 	
                                // dataWord[11] 磁北と真北の間の角度の差の方向。E = 東、W = 西 	
                                // dataWord[12] モード, N = データなし, A = Autonomous（自律方式）, D = Differential（干渉測位方式）, E = Estimated（推定）* チェックサム

                                bhwGPS = true;
                            }
                            catch
                            {
                            }
                            break;
                        case "$GPGGA":
                            // ※未対応
                            break;
                        case "$GPGSA":
                            // ※未対応
                            break;
                    }
                }
                while (true);



                // Close
                {
                    if (null != fsr)
                    {
                        fsr.Close();
                        fsr = null;
                    }
                    if (null != mst)
                    {
                        mst.Close();
                        mst = null;
                    }
                }
            }

            usbGPSResive.Clear();

            return true;
        }

        private long ParseGPS_MS(string str)
        {
            return (long)(double.Parse(str) * 1000.0);
        }


    }
}
