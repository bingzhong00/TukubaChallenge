//#define EMULATOR_MODE  // bServer エミュレーション起動


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using LocationPresumption;
using SCIP_library;
using System.Diagnostics;
using VRIpcLib;


namespace CersioIO
{
    public class CersioCtrl
    {
        // BOXPC(bServer)通信クラス
        // エミュレータ用プロセス
        Process processEmuSim = null;

        /// <summary>
        /// bServerEmuratorフラグ
        /// </summary>
        public bool bServerEmu = false;

        /// <summary>
        /// bServer通信ソケット
        /// </summary>
        TCPClient objTCPSC = null;


        /// <summary>
        /// ROSIFプロセス
        /// </summary>
        Process processRosIF = null;


        // モータードライバー直結時
        // アクセル、ハンドルコントローラ
        public DriveIOport UsbMotorDriveIO;


        // 最近　送信したハンドル、アクセル
        static public double nowSendHandleValue;
        static public double nowSendAccValue;


        // ハンドル、アクセル上限値
        static public double HandleRate = 1.0;
        static public double AccRate = 0.5;

        // ハンドル、アクセルの変化係数
        public const double HandleControlPow = 0.125; // 0.15;
        public const double AccControlPowUP = 0.100; // 0.15;   // 加速時は緩やかに
        public const double AccControlPowDOWN = 0.150;

        // HW
        public int hwCompass = 0;
        public bool bhwCompass = false;

        public double hwREX = 0.0;
        public double hwREY = 0.0;
        public double hwREDir = 0.0;

        public double hwREStartX = 0.0;
        public double hwREStartY = 0.0;
        public double hwREStartDir = 0.0;   // 向きをリセットした値
        public bool bhwREPlot = false;

        public double hwRErotR = 0.0;
        public double hwRErotL = 0.0;
        public bool bhwRE = false;

        public double hwGPS_LandX = 0.0;
        public double hwGPS_LandY = 0.0;

        /// <summary>
        /// GPS移動情報からの向き
        /// 比較的遅れる
        /// </summary>
        public double hwGPS_MoveDir = 0.0;  // 0 ～ 359度
        public bool bhwGPS = false;
        public bool bhwUsbGPS = false;

        // 受信文字
        public string hwResiveStr;
        public string hwSendStr;

        /// <summary>
        /// USB GPS取得データ
        /// </summary>
        public List<string> usbGPSResive = new List<string>();

        /// <summary>
        /// ROS中継　通信オブジェクト
        /// </summary>
        private IpcClient ipc = new IpcClient();

        /// <summary>
        /// bServer IpAddr
        /// </summary>
        private string bServerAddr = "192.168.1.101";

        /// <summary>
        /// bServer エミュレータ
        /// </summary>
        private string bServerEmuAddr = "127.0.0.1";

        /// <summary>
        /// bServer ポートNo
        /// </summary>
        private int bServerPort = 50001;

        // --------------------------------------------------------------------------------------------------
        public CersioCtrl()
        {
        }

        /// <summary>
        /// 終了
        /// </summary>
        public void Disconnect()
        {
            // 停止コマンド送信
            SendCommand_Stop();

            // USB SH制御解除
            if (null != UsbMotorDriveIO)
            {
                UsbMotorDriveIO.Close();
            }

            // bServer切断
            if (null != objTCPSC)
            {
                objTCPSC.Dispose();
                objTCPSC = null;
            }

            // エミュレータ終了
            if (null != processEmuSim && !processEmuSim.HasExited)
            {
                processEmuSim.Kill();
            }
        }

        /// <summary>
        /// BoxPcと接続
        /// </summary>
        /// <returns></returns>
        public void ConnectBoxPC_Async()
        {
            if (TCP_IsConnected())
            {
                objTCPSC.Dispose();
                // 少し待つ
                System.Threading.Thread.Sleep(100);
            }

            // 通信接続
            objTCPSC = null;

            objTCPSC = new TCPClient(bServerAddr, bServerPort);
            // 回線オープン
            objTCPSC.StartAsync();

            bServerEmu = false;

            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void ConnectBoxPC_Emulator()
        {
            // セルシオ　エミュレータ起動
            /*
            if (null == processEmuSim || processEmuSim.HasExited)
            {
                processEmuSim = Process.Start(@"..\..\..\CersioSim\bin\CersioSim.exe");

                //アイドル状態になるまで待機
                //processEmuSim.WaitForInputIdle();
            }
            */

            // 通信接続
            if (null != objTCPSC)
            {
                if (TCP_IsConnected())
                {
                    SendCommand_Stop();

                    objTCPSC.Dispose();
                    // 少し待つ
                    System.Threading.Thread.Sleep(100);
                }
            }

            bServerEmu = true;
            objTCPSC = null;

            objTCPSC = new TCPClient(bServerEmuAddr, bServerPort);
            // 回線オープン
            objTCPSC.Start();

            return;
        }

        /// <summary>
        /// ROFIF起動
        /// </summary>
        /// <returns></returns>
        public bool Run_RosIF()
        {
            if (null == processRosIF || processRosIF.HasExited)
            {
                processRosIF = Process.Start(@"..\..\..\RosIF\bin\RosIF.exe");
            }

            return true;
        }

        /// <summary>
        /// 静止指示
        /// </summary>
        public void SendCommand_Stop()
        {
            if (TCP_IsConnected())
            {
                // 他のACコマンドを発行するスレッドの終了を待つ
                System.Threading.Thread.Sleep(250);

                // 動力停止
                TCP_SendCommand("AC,0.0,0.0\n");
                System.Threading.Thread.Sleep(50);

                // LEDを戻す
                TCP_SendCommand("AL,0,\n");
                System.Threading.Thread.Sleep(50);
            }
        }

        /// <summary>
        /// ロータリーエンコーダにスタート情報をセット
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dir"></param>
        public void SendCommand_RE_Reset(double x, double y, double dir)
        {
            // 現在座標をセット
            SendCommand("AD," + ((float)x).ToString("f") + "," + ((float)y).ToString("f") + "\n");

            // 角度をリセット
            SendCommand_RE_Reset(dir);
        }

        public void SendCommand_RE_Reset()
        {
            SendCommand_RE_Reset(0.0, 0.0, 0.0);
        }

        /// <summary>
        /// RE回転のみリセット
        /// </summary>
        /// <param name="dir"></param>
        public void SendCommand_RE_Reset(double dir)
        {
            // 角度をパイに、回転の+-を調整
            double rad = -dir * Math.PI / 180.0;
            SendCommand("AR," + rad.ToString("f") + "\n");
        }

        /// <summary>
        /// ロータリーエンコーダのスタート位置セット
        /// 回転差分計算用
        /// </summary>
        /// <param name="mmX"></param>
        /// <param name="mmY"></param>
        /// <param name="dir"></param>
        public void setREPlot_Start(double mmX, double mmY, double dir)
        {
            hwREStartX = mmX;
            hwREStartY = mmY;
            hwREStartDir = dir;
        }

        /// <summary>
        /// １回転のパルス値セット
        /// </summary>
        /// <param name="dir"></param>
        public void SendCommand_RE_OneRotatePulse_Reset(double wheelL, double wheelR )
        {
            SendCommand("EP," + wheelL.ToString("f") + "," + wheelR.ToString("f") + "\n");
        }


        /// <summary>
        /// ハードウェアステータス取得
        /// </summary>
        /// <param name="useUsbGPS">USB接続のGPSを使う</param>
        public void GetHWStatus(bool useUsbGPS)
        {
            if (TCP_IsConnected())
            {
                // 受信コマンド解析
                TCP_ReciveCommand();

                // センサーデータ要求コマンド送信
                // ロータリーエンコーダ値(回転累計)
                SendCommand("A1" + "\n");

                // コンパス取得
                SendCommand("A2" + "\n");

                // GPS取得
                // usbGPSを使う場合は、bServerのGPS情報を取得しない。
                if (!useUsbGPS)
                {
                    SendCommand("A3" + "\n");
                }

                // ロータリーエンコーダ　絶対値取得
                SendCommand("A4" + "\n");

                // ロータリーエンコーダ　ハンドル値修正
                //double rad = (double)(-hwCompass) * Math.PI / 180.0;
                //TCP_SendCommand("AR," + rad.ToString("f") + "\n");


                // ROS-IFへデータ書き込み
                try
                {
                    // REPlotX,Y
                    ipc.RemoteObject.rePlotX = hwREX;
                    ipc.RemoteObject.rePlotY = hwREY;
                    ipc.RemoteObject.reAng = hwREDir;

                    // Compus
                    if (bhwCompass)
                    {
                        ipc.RemoteObject.compusDir = hwCompass;
                    }

                    // RE パルス値
                    ipc.RemoteObject.reRpulse = hwRErotR;
                    ipc.RemoteObject.reLpulse = hwRErotL;

                    if (bhwGPS && !bhwUsbGPS)
                    {
                        ipc.RemoteObject.gpsGrandX = hwGPS_LandX;
                        ipc.RemoteObject.gpsGrandY = hwGPS_LandY;
                    }
                }
                catch { }

                // コマンド送信
                SendCommandQue();
            }

            // USB GPS情報取得
            if (useUsbGPS)
            {
                AnalizeUsbGPS();

                // Ros-IF
                // GPS
                ipc.RemoteObject.gpsGrandX = hwGPS_LandX;
                ipc.RemoteObject.gpsGrandY = hwGPS_LandY;
            }

            // カウンタ更新
            if (cntHeadLED > 0) cntHeadLED--;
        }

        /// <summary>
        /// ROSのLRFデータ取得
        /// </summary>
        /// <returns></returns>
        public double[] GetROS_LRFdata()
        {
            try
            {
                return ipc.RemoteObject.urgData;
            }
            catch
            {
                return null;
            }
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
            else if (null != UsbMotorDriveIO)
            {
                // USB接続時
                if (UsbMotorDriveIO.IsConnect())
                {
                    UsbMotorDriveIO.Send_AC_Command(sendHandle, sendAcc);
                }
            }
        }

        /// <summary>
        /// 現在値でコマンド発行
        /// </summary>
        public void SetCommandAC()
        {
            SetCommandAC(nowSendHandleValue, nowSendAccValue);
        }

        /// <summary>
        /// 滑らかに動くハンドル、アクセルワークを計算する
        /// </summary>
        /// <param name="targetHandleVal"></param>
        /// <param name="targetAccelVal"></param>
        public void CalcHandleAccelControl(double targetHandleVal, double targetAccelVal )
        {
            double handleTgt = targetHandleVal * HandleRate;
            double accTgt = targetAccelVal * AccRate;
            double diffAcc = (accTgt - nowSendAccValue);

            // ハンドル、アクセル操作を徐々に目的値に変更する
            nowSendHandleValue += (handleTgt - nowSendHandleValue) * HandleControlPow;
            nowSendAccValue += ((diffAcc > 0.0) ? (diffAcc * AccControlPowUP) : (diffAcc * AccControlPowDOWN));
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
            if (!TCP_IsConnected()) return false;


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
        private List<string> SendCommandList = new List<string>();

        /// <summary>
        /// コマンド分割送信
        /// </summary>
        private void SendCommandQue()
        {
            if (TCP_IsConnected())
            {
                // 先頭から順に送信
                while (SendCommandList.Count > 0)
                {
                    string sendMsg = SendCommandList[0];
                    TCP_SendCommand(sendMsg);

                    if (SendCommandList.Count > 0)
                    {
                        SendCommandList.RemoveAt(0);
                    }
                }
            }

            // 接続されていないなら、リストをクリア
            SendCommandList.Clear();
        }

        /// <summary>
        /// 送信コマンド受付
        /// リストに積んでいく
        /// </summary>
        /// <param name="comStr"></param>
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
            if (null == objTCPSC) return;


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
                    // ※ログ出力先　再検討
                    //Brain.addLogMsg += "TCP_SendCommand:Error "+e.Message+"\n";

                    objTCPSC.DisConnect();
                }

                hwSendStr += "/" + comStr;
            }
        }

        // -----------------------------------------------------------------------------------------
        //
        //
        public static int SpeedMH = 0;   // 速度　mm/Sec
        double oldResiveMS;         // 速度計測用 受信時間差分
        double oldWheelR;              // 速度計測用　前回ロータリーエンコーダ値

        // RE初期値
        double ReInitR = 0;
        double ReInitL = 0;
        bool bInitRePulse = true;   // 初期化要求フラグ

        public double emuGPSX = 134.0000;
        public double emuGPSY = 35.0000;

        /// <summary>
        /// 受信コマンド解析
        /// </summary>
        /// <returns></returns>
        public string TCP_ReciveCommand()
        {
            if (null == objTCPSC) return "";

            System.Net.Sockets.TcpClient objSck = objTCPSC.SckProperty;
            System.Net.Sockets.NetworkStream objStm = objTCPSC.MyProperty;
            string readStr = "";



            if (objStm != null && objSck != null)
            {
                // ソケット受信
                if (objSck.Available > 0 && objStm.DataAvailable)
                {
                    Byte[] dat = new Byte[objSck.Available];

                    if (0 == objStm.Read(dat, 0, dat.GetLength(0)))
                    {
                        // 切断を検知
                        objTCPSC.Dispose();
                        return "";
                    }

                    readStr = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetString(dat);
                    hwResiveStr = readStr;

                    {
                        string[] rsvCmd = readStr.Split('$');

                        for (int i = 0; i < rsvCmd.Length; i++)
                        {
                            if (rsvCmd[i].Length <= 3) continue;

                            // ロータリーエンコーダから　速度を計算
                            if (rsvCmd[i].Substring(0, 3) == "A1,")
                            {
                                const double tiyeSize = 140.0;  // タイヤ直径 [mm]
                                const double OnePuls = 250.0;   // 一周のパルス数
                                double ResiveMS;
                                double ResReR, ResReL;
                                string[] splStr = rsvCmd[i].Split(',');

                                // 0 A1
                                double.TryParse(splStr[1], out ResiveMS);        // 小数点付き 秒
                                double.TryParse(splStr[2], out ResReR);        // Right Wheel
                                double.TryParse(splStr[3], out ResReL);        // Left Wheel

                                // 0.5秒以上の経過時間があれば計算 (あまりに瞬間的な値では把握しにくいため)
                                if ((ResiveMS - oldResiveMS) > 0.5)
                                {
                                    // 速度計算(非動輪を基準)
                                    SpeedMH = (int)(((double)(hwRErotR - oldWheelR) / OnePuls * (Math.PI * tiyeSize)) / (ResiveMS - oldResiveMS));

                                    oldResiveMS = ResiveMS;
                                    oldWheelR = hwRErotR;
                                }

                                // 初回のパルス値リセット
                                if(bInitRePulse)
                                {
                                    ReInitR = ResReR;
                                    ReInitL = ResReL;
                                    bInitRePulse = false;
                                }

                                // 初回からの変化値
                                hwRErotR = ResReR - ReInitR;
                                hwRErotL = ResReL - ReInitL;



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
                                hwCompass = ResiveCmp;
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

                                // データが足らないことがある
                                if (splStr.Length >= 4)
                                {
                                    // splStr[0] "A3"
                                    // ミリ秒取得
                                    double.TryParse(splStr[1], out ResiveMS); // ms? 万ミリ秒に思える

                                    double.TryParse(splStr[2], out ResiveLandX);   // GPS値
                                    double.TryParse(splStr[3], out ResiveLandY);
                                    hwGPS_LandX = ResiveLandX;
                                    hwGPS_LandY = ResiveLandY;
                                    bhwGPS = true;
                                    bhwUsbGPS = false;
                                }
                            }
                            else if (rsvCmd[i].Substring(0, 3) == "A4,")
                            {
                                // ロータリーエンコーダ  プロット座標
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

                                // splStr[0] "A4"
                                // ミリ秒取得
                                double.TryParse(splStr[1], out ResiveMS); // ms? 万ミリ秒に思える
                                double.TryParse(splStr[2], out ResiveX);  // 絶対座標X mm
                                double.TryParse(splStr[3], out ResiveY);  // 絶対座標Y mm
                                double.TryParse(splStr[4], out ResiveRad);  // 向き -2PI 2PI

                                // 座標系変換
                                // 右上から右下へ
#if true
                                // 座標軸を変換
                                ResiveY = -ResiveY;
                                ResiveRad = -ResiveRad;
                                // リセットした時点での電子コンパスの向きを元にマップ座標へ変換する
                                // x*cos - y*sin
                                // x*sin + y*cos
                                {
                                    hwREDir = ((-ResiveRad * 180.0) / Math.PI) + hwREStartDir;

                                    double theta = hwREStartDir / 180.0 * Math.PI;
                                    hwREX = (ResiveX * Math.Cos(theta) - ResiveY * Math.Sin(theta)) + hwREStartX;
                                    hwREY = (ResiveX * Math.Sin(theta) + ResiveY * Math.Cos(theta)) + hwREStartY;
                                }
#else
                                hwREDir = ((-ResiveRad * 180.0) / Math.PI);

                                hwREX = ResiveX;
                                hwREY = ResiveY;
#endif
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
            if (null == objTCPSC) return false;

            System.Net.Sockets.TcpClient objSck = objTCPSC.SckProperty;
            System.Net.Sockets.NetworkStream objStm = objTCPSC.MyProperty;

            if (objStm != null && objSck != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 接続先 アドレス取得
        /// </summary>
        /// <returns></returns>
        public string TCP_GetConnectedAddr()
        {
            if (!TCP_IsConnected()) return "";

            return objTCPSC.ipStringProperty;
        }


        // -----------------------------------------------------------------------------------------------
        //const double GPSScale = 1.852 * 1000.0 * 1000.0;

        //const double GPSScaleX = 1.51985 * 1000.0 * 1000.0;    // 経度係数  35度時
        //const double GPSScaleY = 1.85225 * 1000.0 * 1000.0;    // 緯度係数

        /// <summary>
        /// USB GPSデータ解析
        /// </summary>
        /// <returns></returns>
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
                                    string strDo = dataWord[3].Substring(0, dataGPS[0].Length - 2);
                                    string strHun = dataWord[3].Substring(strDo.Length, dataWord[3].Length - strDo.Length);

                                    hwGPS_LandY = double.Parse(strDo) + (double.Parse(strHun) / 60.0);
                                    ido = double.Parse(strDo);
                                }
                                // dataWord[4] N,S N = 北緯、South = 南緯

                                {
                                    // dataWord[5] 経度。dddmm.mmmm
                                    string[] dataGPS = dataWord[5].Split('.');

                                    string strDo = dataWord[5].Substring(0, dataGPS[0].Length - 2);
                                    string strHun = dataWord[5].Substring(strDo.Length, dataWord[5].Length - strDo.Length);

                                    hwGPS_LandX = double.Parse(strDo) + (double.Parse(strHun) / 60.0);
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
                                bhwUsbGPS = true;
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
