using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CersioIO;

namespace bServerCom
{
    class Program
    {
        static void Main(string[] args)
        {
            /// <summary>
            /// bServer通信ソケット
            /// </summary>
            TCPClient objTCPSC = null;

            // 接続情報
            string bServerAddr = "192.168.1.101";
            //string bServerAddr = "127.0.0.1"; // for Simurator

            /// <summary>
            /// bServer ポートNo
            /// </summary>
            int bServerPort = 50001;

            // 更新タイミング MS
            const int sleepMS = 20;

            bool loopFlg = true;

            // パラメータ取得
            if (args.Length >= 1)
            {
                bServerAddr = args[0];
            }
            if (args.Length >= 2)
            {
                bServerPort = int.Parse(args[1]);
            }

            Console.WriteLine("bServer Communication -Interface Ver0.10");
            Console.WriteLine("");

            // 終了イベント
            Console.CancelKeyPress += (sender, e) =>
            {
                Console.Clear();
                Console.WriteLine("終了....");

                loopFlg = false;
                // trueにすると、プログラムを終了させない
                e.Cancel = true;

                // bServer切断
                if (null != objTCPSC)
                {
                    objTCPSC.Dispose();
                    objTCPSC = null;
                }
            };

            Console.WriteLine("Connect ... IP:" + bServerAddr + " port:" + bServerPort.ToString());

            objTCPSC = null;
            objTCPSC = new TCPClient(bServerAddr, bServerPort);
            objTCPSC.Start();
            if (TCP_IsConnected(objTCPSC))
            {
                Console.WriteLine("接続OK!");
            }

            Console.WriteLine("");
            Console.WriteLine("***** Start *****");
            Console.WriteLine("CTRL+Cで、アプリを終了します。");
            Console.WriteLine("Command List");
            Console.WriteLine("A1 ... RE パルス取得");
            Console.WriteLine("A2 ... 地磁気取得");
            Console.WriteLine("A3 ... GPS 取得");
            Console.WriteLine("A4 ... RE プロット座標取得");
            Console.WriteLine("");
            Console.WriteLine("AC,ハンドル,アクセル");
            Console.WriteLine("AL,LED番号");
            Console.WriteLine("AD,プロット座標X 変更,プロット座標Y 変更");
            Console.WriteLine("AR,プロット向き 変更");
            Console.WriteLine("ES,左１回転パルス値,右１回転パルス値");
            Console.WriteLine("");

            // 現在のカーソル行取得
            int curStartRow = Console.CursorTop;

            while (loopFlg)
            {
                try
                {
                    // 通信できるか？
                    if (!TCP_IsConnected(objTCPSC))
                    {
                        Console.WriteLine("接続開始...");

                        // 再接続
                        objTCPSC.Dispose();
                        // 少し待つ
                        System.Threading.Thread.Sleep(100);

                        // 通信接続
                        objTCPSC = null;
                        objTCPSC = new TCPClient(bServerAddr, bServerPort);

                        objTCPSC.Start();

                        if (TCP_IsConnected(objTCPSC))
                        {
                            Console.WriteLine("接続OK!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("接続 Error!");
                    Console.WriteLine(ex.Message);
                    break;
                }

                // コンソール表示
                {
                    string resiveStr = TCP_ReciveCommand(objTCPSC);

                    if (!string.IsNullOrEmpty(resiveStr))
                    {
                        Console.WriteLine("Resive:" + resiveStr);
                    }
                }

                if (Console.KeyAvailable)
                {
                    // キー入力があれば、受付
                    string sendStr = Console.ReadLine();

                    if (!string.IsNullOrEmpty(sendStr))
                    {
                        TCP_SendCommand(objTCPSC, sendStr+"\n");

                        Console.WriteLine("Send:" + sendStr);
                    }
                }


                // カーソル位置を初期化
                //Console.SetCursorPosition(0, curStartRow);

                // 処理を休止
                System.Threading.Thread.Sleep(sleepMS);
            }


            if (null != objTCPSC)
            {
                objTCPSC.Dispose();
                objTCPSC = null;
            }
        }

        /// <summary>
        /// BoxPCとの通信状態をかえす
        /// </summary>
        /// <returns></returns>
        static bool TCP_IsConnected(TCPClient objTCPSC)
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
        /// コマンド送信
        /// </summary>
        static void TCP_SendCommand(TCPClient objTCPSC, string comStr)
        {
            if (null == objTCPSC) return;

            System.Net.Sockets.NetworkStream objStm = objTCPSC.MyProperty;

            if (objStm != null)
            {
                try
                {
                    Byte[] dat = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetBytes(comStr);

                    objStm.Write(dat, 0, dat.GetLength(0));
                }
                catch (Exception e)
                {
                    // 接続エラー
                    Console.WriteLine("TCP_SendCommand() 送信失敗 / 送信文字:" + comStr);
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// コマンド受信
        /// </summary>
        /// <returns></returns>
        static string TCP_ReciveCommand(TCPClient objTCPSC)
        {
            if (null == objTCPSC) return "";

            string resiveStr = "";

            try
            {
                System.Net.Sockets.TcpClient objSck = objTCPSC.SckProperty;
                System.Net.Sockets.NetworkStream objStm = objTCPSC.MyProperty;

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
                        string readStr = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetString(dat);
                        resiveStr += readStr;
                    }

                }
            }
            catch (Exception e)
            {
                // 接続エラー
                Console.WriteLine("TCP_ReciveCommand() 受信失敗");
                Console.WriteLine(e.Message);
            }

            return resiveStr;
        }
    }
}
