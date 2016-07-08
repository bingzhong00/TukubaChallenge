﻿#define LogQuiet  

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CersioSim
{
    class bServer
    {
        // センサー情報
        // ロータリーエンコーダ
        // パルス数
        public long senReR, senReL;

        // Plot計算用
        public long senReR_, senReL_;
        public PointD plotWheelR = new PointD();
        public PointD plotWheelL = new PointD();

        public bool bResetRE = false;

        // PlotX,Y
        public double senRePlotX, senRePlotY;
        // Angle
        public double senReAng;

        // リセット、補正値
        public double senRePlotX_Rst = 0.0;
        public double senRePlotY_Rst = 0.0;
        public double senReAng_Rst = 0.0;

        public double senRePlotX_RstAdd = 0.0;
        public double senRePlotY_RstAdd = 0.0;
        public double senReAng_RstAdd = 0.0;

        // 出力値
        public double senRePlotX_Out, senRePlotY_Out;
        // Angle
        public double senReAng_Out;

        // 地磁気
        // 0～360度
        public double senCompusDir;

        // GPS 緯度,経度
        public double senGpsLandX, senGpsLandY;

        // コントロール情報
        public double ctrHandle;        // ハンドル
        public double ctrAccel;         // アクセル

        public int ctrLedPattern;       // LED点灯パターン

        public string hwResiveStr;

        //NetworkStreamを取得
        private System.Net.Sockets.NetworkStream ns = null;
        private System.Net.Sockets.TcpClient client = null;
        private System.Net.Sockets.TcpListener listener = null;

        public System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        public async Task<bool>Open(string ipAddr = "127.0.0.1", int ipPort = 50001)
        {
            sw.Start();

            //ListenするIPアドレス
            System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(ipAddr);

            //ホスト名からIPアドレスを取得する時は、次のようにする
            //string host = "localhost";
            //System.Net.IPAddress ipAdd =
            //    System.Net.Dns.GetHostEntry(host).AddressList[0];
            //.NET Framework 1.1以前では、以下のようにする
            //System.Net.IPAddress ipAdd =
            //    System.Net.Dns.Resolve(host).AddressList[0];

            //TcpListenerオブジェクトを作成する
            listener =
                new System.Net.Sockets.TcpListener(ipAdd, ipPort);

            //Listenを開始する
            listener.Start();
            Console.WriteLine("Listenを開始しました({0}:{1})。",
                ((System.Net.IPEndPoint)listener.LocalEndpoint).Address,
                ((System.Net.IPEndPoint)listener.LocalEndpoint).Port);

            //接続要求があったら受け入れる
            //client = listener.AcceptTcpClient();
            client = await listener.AcceptTcpClientAsync();
            Console.WriteLine("クライアント({0}:{1})と接続しました。",
                ((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Address,
                ((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Port);

            //NetworkStreamを取得
            ns = client.GetStream();

            return true;
        }

        public void Close()
        {
            //閉じる
            if (ns != null)
            {
                ns.Close();
            }
            if (client != null)
            {
                client.Close();
                Console.WriteLine("クライアントとの接続を閉じました。");
            }

            //リスナを閉じる
            listener.Stop();
            Console.WriteLine("Listenerを閉じました。");
        }


        public bool readMessage()
        {
            if (ns == null) return false;

            //読み取り、書き込みのタイムアウトを10秒にする
            //デフォルトはInfiniteで、タイムアウトしない
            //(.NET Framework 2.0以上が必要)
            ns.ReadTimeout = 10000;
            ns.WriteTimeout = 10000;

            // データがない
            if (!ns.DataAvailable) return false;

            //クライアントから送られたデータを受信する

            System.Text.Encoding enc = System.Text.Encoding.UTF8;
            bool disconnected = false;

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            byte[] resBytes = new byte[256];
            int resSize = 0;
            do
            {
                //データの一部を受信する
                resSize = ns.Read(resBytes, 0, resBytes.Length);
                //Readが0を返した時はクライアントが切断したと判断
                if (resSize == 0)
                {
                    disconnected = true;
                    Console.WriteLine("クライアントが切断しました。");
                    break;
                }

                //受信したデータを蓄積する
                ms.Write(resBytes, 0, resSize);
                //まだ読み取れるデータがあるか、データの最後が\nでない時は、
                // 受信を続ける
            } while (ns.DataAvailable || resBytes[resSize - 1] != '\n');

            //受信したデータを文字列に変換
            string resMsg = enc.GetString(ms.GetBuffer(), 0, (int)ms.Length);
            ms.Close();

            //末尾の\nを削除
            resMsg = resMsg.TrimEnd('\n');
#if !LogQuiet
            Console.WriteLine("receive:" + resMsg);
#endif

            if (!disconnected)
            {
                //クライアントにデータを送信する
                //クライアントに送信する文字列を作成
                /*
                string sendMsg = resMsg.Length.ToString();

                //文字列をByte型配列に変換
                byte[] sendBytes = enc.GetBytes(sendMsg + '\n');

                //データを送信する
                ns.Write(sendBytes, 0, sendBytes.Length);
                Console.WriteLine(sendMsg);
                 * */

                string sendMsg = AnalizeMessage(resMsg);

                if (!string.IsNullOrEmpty( sendMsg))
                {
                    byte[] sendBytes = enc.GetBytes(sendMsg + '\n');

                    ns.Write(sendBytes, 0, sendBytes.Length);
#if !LogQuiet
                    Console.WriteLine("send:" + sendMsg);
#endif
                }

            }

            return !disconnected;
        }

        /// <summary>
        /// 受信メッセージ解析
        /// </summary>
        /// <param name="readStr"></param>
        /// <returns></returns>
        public string AnalizeMessage( string readStr )
        {
            // 送り返す文字列
            string sendStr = "";
            double nowMs = (double)(sw.ElapsedMilliseconds) / 1000.0;
            hwResiveStr = readStr;

            string[] rsvCmd = readStr.Split('\n');

            for (int i = 0; i < rsvCmd.Length; i++)
            {
                string commandStr = rsvCmd[i].Substring(0, (rsvCmd[i] + ",").IndexOf(","));

                // ロータリーエンコーダから　速度を計算
                if (commandStr == "A1")
                {
                    // REパルス数
                    // A1,ms,R,L
                    //sendStr += string.Format("A1,{0},{1},{2}$", nowMs, senReR, senReL);
                }
                else if (commandStr == "A2")
                {
                    // コンパス情報
                    // A2,22.5068,210$
                    sendStr += string.Format("A2,{0},{1}$", nowMs, (int)senCompusDir%360 );
                }
                else if (commandStr == "A3")
                {
                    // GPS情報
                    //              経度、緯度
                    // $A3,38.266,36.8002,140.11559$
                    sendStr += string.Format("A3,{0},{1},{2}$", nowMs, senGpsLandX, senGpsLandY);
                }
                else if (commandStr == "A4")
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
                    /*
                    senRePlotX_Out = (senRePlotX - senRePlotX_Rst + senRePlotX_RstAdd);
                    senRePlotY_Out = (senRePlotY - senRePlotY_Rst + senRePlotY_RstAdd);
                    senReAng_Out = (senReAng - senReAng_Rst + senReAng_RstAdd);
                    */
                    /*
                    {
                        double resAng = 0.0;


                        // ※ここではなくMainFormで計算すべきか
                        // 結果をFromに表示も必要
                        REncoderToMap.CalcWheelPlotXY(ref plotWheelR, ref plotWheelL, ref resAng,
                                                      senReR, senReL,
                                                      senReR_, senReL_);

                        senRePlotX_Out = ((plotWheelR.X + plotWheelL.X) * 0.5) + senRePlotX_RstAdd;
                        senRePlotY_Out = ((plotWheelR.Y + plotWheelL.Y) * 0.5) + senRePlotY_RstAdd;
                        senReAng_Out = resAng + senReAng_RstAdd;
                    }
                    */

                    sendStr += string.Format("A4,{0},{1},{2},{3}$", nowMs,
                                              senRePlotX_Out + senRePlotX_RstAdd,
                                              senRePlotY_Out + senRePlotY_RstAdd,
                                              senReAng_Out + senReAng_RstAdd);
                }
                else if (commandStr == "AC")
                {
                    // ハンドル、アクセル値 取得
                    string[] splStr = rsvCmd[i].Split(',');

                    double.TryParse(splStr[1], out ctrHandle);  // ハンドル -1.0～1.0 
                    double.TryParse(splStr[2], out ctrAccel);   // アクセル -1.0～1.0 
                }
                else if (rsvCmd[i].Substring(0, 3) == "AL,")
                {
                    // LEDパターン
                    string[] splStr = rsvCmd[i].Split(',');

                    int.TryParse(splStr[1], out ctrLedPattern);  // LEDパターン
                }
                else if (rsvCmd[i].Substring(0, 3) == "AD,")
                {
                    // RePlot 向きリセット
                    string[] splStr = rsvCmd[i].Split(',');

                    senRePlotX_Rst = senRePlotX;
                    senRePlotY_Rst = senRePlotY;

                    double.TryParse(splStr[1], out senRePlotX_RstAdd);
                    double.TryParse(splStr[2], out senRePlotY_RstAdd);

                    //
                    plotWheelR.X = 250.0;
                    plotWheelR.Y = 0.0;
                    plotWheelL.X = -250.0;
                    plotWheelL.Y = 0.0;

                    bResetRE = true;
                }
                else if (rsvCmd[i].Substring(0, 3) == "AR,")
                {
                    // RePlot 向きリセット
                    string[] splStr = rsvCmd[i].Split(',');

                    senReAng_Rst = senReAng;
                    double.TryParse(splStr[1], out senReAng_RstAdd);
                }

            }


            return sendStr;
        }
    }
}