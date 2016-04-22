using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CersioSim
{
    /// <summary>
    /// URG sim
    /// </summary>
    class SCIPsim
    {
        // センサー情報
        // ロータリーエンコーダ
        // パルス数
        public long senReR, senReL;
        // PlotX,Y
        public double senRePlotX, senRePlotY;
        // Angle
        public double senReAng;


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

        public async Task<bool>Open(string ipAddr = "127.0.0.10", int ipPort = 10940)
        {
            sw.Start();

            //ListenするIPアドレス
            System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(ipAddr);

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
                ns = null;
            }
            if (client != null)
            {
                client.Close();
                Console.WriteLine("クライアントとの接続を閉じました。");
                client = null;
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

            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("shift_jis");
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
            Console.WriteLine("receive:" + resMsg);

            if (!disconnected)
            {
                //クライアントにデータを送信する
                //クライアントに送信する文字列を作成


                // エコーバック
                {
                    //文字列をByte型配列に変換
                    byte[] sendBytes = enc.GetBytes(resMsg + '\n' + '\n');

                    //データを送信する
                    ns.Write(sendBytes, 0, sendBytes.Length);

                    //Console.WriteLine("send:" + resMsg);
                }

                {
                    string sendMsg = AnalizeMessage(resMsg);

                    if (!string.IsNullOrEmpty(sendMsg))
                    {
                        byte[] sendBytes = enc.GetBytes(sendMsg + "\n\n");

                        ns.Write(sendBytes, 0, sendBytes.Length);
                        //Console.WriteLine("send:" + sendMsg);
                    }
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
            int nowMs = (int)(sw.ElapsedMilliseconds);
            hwResiveStr = readStr;

            string[] rsvCmd = readStr.Split('\n');

            for (int i = 0; i < rsvCmd.Length; i++)
            {
                string commandStr = rsvCmd[i].Substring(0, 2);

                // ロータリーエンコーダから　速度を計算
                if (commandStr == "MD")
                {
                    // 開始インデックス
                    string startIdx = rsvCmd[i].Substring(2, 4);
                    // 終了インデックス
                    string endIdx = rsvCmd[i].Substring(6, 4);
                    // スキップインデックス数
                    string skipData = rsvCmd[i].Substring(10, 2);
                    // スキャン回数
                    string scanNum = rsvCmd[i].Substring(12, 1);
                    // データ数
                    string getNum = rsvCmd[i].Substring(13, 2);

                    // エコーバック?
                    sendStr += rsvCmd[i] + "\n";

                    sendStr += "99b" + "\n";
                    sendStr += encode(nowMs, 4) + "0" + "\n";

                    int num = (int.Parse(endIdx) - int.Parse(startIdx)) / int.Parse(skipData);
                    int n;
                    string sendData = "";

                    for( n=0;n<num; n++)
                    {
                        sendData += encode(1024, 3);
                    }

                    while (sendData.Length > 0)
                    {
                        if (sendData.Length >= 64)
                        {
                            sendStr += sendData.Substring(0, 64) + "0" + "\n";
                            sendData = sendData.Substring(64);
                        }
                        else
                        {
                            sendStr += sendData.Substring(0, sendData.Length-1 ) + "0" + "\n";
                            sendData = "";
                        }
                    }
                }
                else if (commandStr == "QT")
                {
                    //sendStr += rsvCmd[i] + "\n\n";
                    sendStr += "00P" + "\n";
                }
                else
                {
                    // わからないのは、エコーバックして00をかえす
                    //sendStr += rsvCmd[i] + "\n\n";
                    //sendStr += "00P" + "\n";
                }
            }


            return sendStr;
        }


        string encode(int val, int numByte)
        {
            System.Text.Encoding enc = System.Text.Encoding.UTF8;
            byte[] btCode = new byte[numByte];

            int i;

            string resMsg = "";
            for (i = 0; i < numByte; ++i)
            {
                btCode[i] = (byte)(((val >> (6 * (numByte - (i+1)))) & 0x3f) + 0x30);
                resMsg += Convert.ToChar(btCode[i]);
            }

            return resMsg;
        }
    }
}
