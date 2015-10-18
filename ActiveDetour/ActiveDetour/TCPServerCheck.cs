using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testTCP
{
    public class TCPServerCheck : IDisposable
    {
        //string ipString;// = "192.168.1.1"; 
        //int port;

        private System.Threading.Thread ListeningCallbackThread;   // 接続待ちスレッド
        private volatile bool SLTAlive;  // 接続待ちスレッド終了指示フラグ(volatile が指定されていることに注意)


        private string ipString;//ListenするIPアドレス

        public string ipStringProperty
        {
            get { return ipString; }
            set { ipString = value; }
        }

        private int port;//Listenするポート番号

        public int portProperty
        {
            get { return port; }
            set { port = value; }
        }

        private System.Net.Sockets.NetworkStream ns;

        public System.Net.Sockets.NetworkStream MyProperty
        {
            get { return ns; }
        }

        private System.Net.Sockets.TcpListener listener;

        public System.Net.Sockets.TcpListener listenerProperty
        {
            get { return listener; }
            //set { listener = value; }
        }

        //System.Net.Sockets.TcpClient client
        private System.Net.Sockets.TcpClient client;

        public System.Net.Sockets.TcpClient clientProperty
        {
            get { return client; }
            //set { client = value; }
        }
        



        public TCPServerCheck()
        {
            ipString = "192.168.1.1";
            port = 9020;

            // スレッド終了指示フラグを未終了に初期化
            SLTAlive = false;

        }

        public void Dispose()
        {
            // 閉じる
            if (ns != null)
                ns.Close();
            if (client != null)
                client.Close();
            //        Console.WriteLine("クライアントとの接続を閉じました。");

            //リスナを閉じる
            if (listener != null)
                listener.Stop();
            //        Console.WriteLine("Listenerを閉じました。");

            // スレッド終了指示フラグを未終了に初期化
            SLTAlive = false;


        }

        /*
        ~TCPServerCheck()
        {
        }
        */

        public bool Start()
        {
            if (!SLTAlive)
            {
                ListeningCallbackThread = new System.Threading.Thread(ListeningCallback);
                // 接続待ち用スレッドを開始
                ListeningCallbackThread.Start();
                // スレッド終了指示フラグを未終了に設定
                SLTAlive = true;

                return true;
            }
            else
                return false;
        }

        // 接続待ちスレッド用メソッド
        private void ListeningCallback()
        {
            System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(this.ipString);

            //ホスト名からIPアドレスを取得する時は、次のようにする
            //string host = "localhost";
            //System.Net.IPAddress ipAdd =
            //    System.Net.Dns.GetHostEntry(host).AddressList[0];
            //.NET Framework 1.1以前では、以下のようにする
            //System.Net.IPAddress ipAdd =
            //    System.Net.Dns.Resolve(host).AddressList[0];

            //TcpListenerオブジェクトを作成する
            //            System.Net.Sockets.TcpListener listener =
            listener =
                new System.Net.Sockets.TcpListener(ipAdd, port);

            Console.WriteLine("Listenを開始しました({0}:{1})。",
                ((System.Net.IPEndPoint)listener.LocalEndpoint).Address,
                ((System.Net.IPEndPoint)listener.LocalEndpoint).Port);



            //try
            //{
                //Listenを開始する
                listener.Start();
            //}
            //catch
            //{
            //    listener.Stop();
            //    listener = null;
            //    return ;
            //}

            do
            {
                while (SLTAlive)
                {
                    if (listener.Pending() == true)
                    {
                        //接続要求があったら受け入れる
                        //                System.Net.Sockets.TcpClient client = listener.AcceptTcpClient();
                        client = listener.AcceptTcpClient();
                        if (client.Connected == true)
                        {
                            Console.WriteLine("クライアント({0}:{1})と接続しました。",
                                ((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Address,
                                ((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Port);
                            //NetworkStreamを取得
                            ns = client.GetStream();

                            //読み取り、書き込みのタイムアウトを10秒にする
                            //デフォルトはInfiniteで、タイムアウトしない
                            //(.NET Framework 2.0以上が必要)
                            ns.ReadTimeout = 10000;
                            ns.WriteTimeout = 10000;

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
                            Console.WriteLine(resMsg);

                            if (!disconnected)
                            {
                                //クライアントにデータを送信する
                                //クライアントに送信する文字列を作成
                                string sendMsg = resMsg.Length.ToString();
                                //文字列をByte型配列に変換
                                byte[] sendBytes = enc.GetBytes(sendMsg + '\n');
                                //データを送信する
                                ns.Write(sendBytes, 0, sendBytes.Length);
                                Console.WriteLine(sendMsg);
                            }
                        }
                    }
                }
                // 短時間だけ待機
                System.Threading.Thread.Sleep(100);
            } while (true);






            //閉じる
            //        ns.Close();
            //        client.Close();
            //        Console.WriteLine("クライアントとの接続を閉じました。");

            //リスナを閉じる
            //        listener.Stop();
            //        Console.WriteLine("Listenerを閉じました。");

            //        Console.ReadLine();

        }

    }
}
