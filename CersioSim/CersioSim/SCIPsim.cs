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
        // ※切断検出,再接続

        // センサー情報
        public short[] lrfData = new short[1080];
        public int numLrfData;


        // NetworkStreamを取得
        private System.Net.Sockets.NetworkStream ns = null;
        private System.Net.Sockets.TcpClient client = null;
        private System.Net.Sockets.TcpListener listener = null;

        public System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        public string listenIP = "";
        public int listenPort = 0;

        public SCIPsim(string ipAddr = "127.0.0.10", int ipPort = 10940)
        {
            listenIP = ipAddr;
            listenPort = ipPort;
        }

        /// <summary>
        /// Open 通信要求受付(非同期)
        /// </summary>
        /// <param name="ipAddr">IPアドレス</param>
        /// <param name="ipPort">ポート</param>
        /// <returns></returns>
        public async Task<bool>Open()
        {
            sw.Start();

            //ListenするIPアドレス
            System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(listenIP);

            //TcpListenerオブジェクトを作成する
            listener =
                new System.Net.Sockets.TcpListener(ipAdd, listenPort);

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

        /// <summary>
        /// クローズ
        /// </summary>
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
            if (null != listener)
            {
                listener.Stop();
                Console.WriteLine("Listenerを閉じました。");
            }
        }

        /// <summary>
        /// メッセージ受信
        /// </summary>
        /// <returns></returns>
        public bool readMessage()
        {
            // ストリームがOpenされていない
            if (ns == null) return false;

            //読み取り、書き込みのタイムアウトを10秒にする
            //デフォルトはInfiniteで、タイムアウトしない
            //(.NET Framework 2.0以上が必要)
            ns.ReadTimeout = 10000;
            ns.WriteTimeout = 10000;

            if (!listener.Server.Connected)
            {
                int i = 0;
                i++;
            }

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
#if DEBUG
            Console.WriteLine("receive:" + resMsg);
#endif

            if (!disconnected)
            {
                //クライアントにデータを送信する
                //クライアントに送信する文字列を作成

                string sendMsg = AnalizeMessage(resMsg);

                if (!string.IsNullOrEmpty(sendMsg))
                {
                    byte[] sendBytes = enc.GetBytes(sendMsg);

                    ns.Write(sendBytes, 0, sendBytes.Length);
#if DEBUG
                    Console.WriteLine("send:" + sendMsg);
#endif
                }
            }

            return !disconnected;
        }


        /*
        SCIP コマンドは「動作モードの設定」「ステータス情報の取得」「距離データの取得」の３種類に分類できます。
        以降では、各種類について、使い方を紹介します。

            動作モードの設定
            "SCIP2.0" ... SCIP2.0 モードへの切替え
            "TM0", "TM1", "TM2" ... タイムスタンプモード
            "SS" ... 通信ボーレートの変更
            "BM" ... レーザの発光
            "QT" ... レーザの消灯、計測停止
            "RS" ... パラメータリセット、計測停止
            "CR" ... モータ回転速度の変更
            ステータス情報の取得
            "VV" ... バージョン情報の取得
            "PP" ... パラメータ情報の取得
            "II" ... ステータス情報の取得
            距離データの取得
            "GD", "GS" ... 距離データの逐次取得
            "MD", "MS" ... 距離データの連続取得
        */
        /*
        名称 図番 SCIP 2.0準拠  “URG” シリーズ通信仕様書 Ｃ－４２－０３３２０Ｂ
        7／18（SENSOR→HOST）
        (1)ステータスが“99”でない時
        ‘M’‘D’or‘S’
        開始ステップ(4byte) 
        終了ステップ(4byte) 
        まとめるステップ数(2byte) 
        間引きスキャン数(1byte) 
        送信回数(2byte)
        文字列LF
        ステータス(2byte)
        SUM(1byte)
        NL2
        
       ＊ステータス：
       “00”...本コマンドの正常受信通知
       “01”...『開始ステップ』に数字以外の文字が混在
       “02”...『終了ステップ』に数字以外の文字が混在
       “03”...『まとめるステップ数』に数字以外の文字が混在
       “04”...『終了ステップ』の指定数が最大設定範囲外
       “05”...『終了ステップ』の指定数が『開始ステップ』の指定数以下
       “06”...『間引き数』に数字以外の文字が混在
       “07”...『送信回数』に数字以外の文字が混在
       “21”～“49”...ステータスが示す異常可能性を検知のため通信を中断
       “50”～“97”...レーザ異常・モータ異常などハードウェア故障
       “98”...センサの正常状態が確認できたため、通信中断状態から復帰
        */

        /// <summary>
        /// 受信メッセージ解析
        /// 返信データ生成
        /// </summary>
        /// <param name="readStr"></param>
        /// <returns></returns>
        public string AnalizeMessage( string readStr )
        {
            // 送り返す文字列
            string sendStr = "";
            int nowMs = (int)(sw.ElapsedMilliseconds);

            string[] rsvCmd = readStr.Split('\n');

            for (int iCmd = 0; iCmd < rsvCmd.Length; iCmd++)
            {
                string commandStr = rsvCmd[iCmd].Substring(0, 2);

                // 距離データの連続取得
                if (commandStr == "MD")
                {
                    // 開始インデックス
                    string startIdx = rsvCmd[iCmd].Substring(2, 4);
                    // 終了インデックス
                    string endIdx = rsvCmd[iCmd].Substring(6, 4);
                    // スキップインデックス数
                    string skipData = rsvCmd[iCmd].Substring(10, 2);
                    // スキャン回数
                    string scanNum = rsvCmd[iCmd].Substring(12, 1);
                    // データ数
                    string getNum = rsvCmd[iCmd].Substring(13, 2);

                    // エコーバック
                    sendStr += rsvCmd[iCmd] + "\n";
                    // ステータス 00 or 99 以外はNG
                    sendStr += "99b" + "\n";

                    // 返信時 MS
                    sendStr += encode(nowMs, 4) + "0" + "\n";

                    // 送信データ作成
                    {
                        // 要求データ量算出
                        int num = (int.Parse(endIdx) - int.Parse(startIdx)) / int.Parse(skipData);
                        int n;
                        string sendData = "";

                        // 実データ量から、要求される(インデックス、スキップ数)量に変換する
                        short[] retData = new short[num];

                        if (numLrfData < num)
                        {
                            // データ数 < 要求データ数場合
                            int numSkip = (num / numLrfData);

                            for (n = 0; n < numLrfData; n++)
                            {
                                for (int i = 0; i < numSkip; i++)
                                {
                                    sendData += encode(lrfData[n], 3);
                                }
                            }
                        }
                        else if (numLrfData < num)
                        {
                            // データ数 > 要求データ数場合
                            int numSkip = (numLrfData / num);

                            for (n = 0; n < num; n++)
                            {
                                int idx = n * numSkip;
                                if (idx > numLrfData) idx = numLrfData - 1;

                                sendData += encode(lrfData[idx], 3);
                            }
                        }
                        else
                        {
                            // データ数 == 要求データ数場合
                            for (n = 0; n < num; n++)
                            {
                                sendData += encode(lrfData[n], 3);
                            }
                        }


                        // 1行64バイトごとに分割
                        while (sendData.Length > 0)
                        {
                            string dataStr = "";

                            if (sendData.Length >= 64)
                            {
                                dataStr = sendData.Substring(0, 64);
                                sendData = sendData.Substring(64);
                            }
                            else
                            {
                                dataStr = sendData.Substring(0, sendData.Length - 1);
                                sendData = "";
                            }

                            // チェックサム付与、改行
                            sendStr += dataStr + ChackSum(dataStr) + "\n";
                        }
                        // 終端
                        sendStr += "\n";
                    }
                }
                // レーザの消灯、計測停止
                else if (commandStr == "QT")
                {
                    // エコーバック
                    sendStr += rsvCmd[iCmd] + "\n";
                    // ステータス 00 or 99 以外はNG
                    sendStr += "00P" + "\n";
                    // 終端
                    sendStr += "\n";
                }
                // ステータス
                else if (commandStr == "PP")
                {
                    /*
                    PP
                    00P
                    MODL:UTM-30LX-EW;I
                    DMIN:23;7
                    DMAX:60000;J
                    ARES:1440;^
                    AMIN:0;?
                    AMAX:1080;Z
                    AFRT:540;0
                    SCAN:2400;U
                    */
                    string dataStr = "";

                    // エコーバック
                    sendStr += rsvCmd[iCmd] + "\n";
                    // ステータス 00 or 99 以外はNG
                    sendStr += "00P" + "\n";

                    // センサ型式情報
                    dataStr = "MODL:UTM-30LX-EW";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    // 最小計測可能距離[mm]
                    dataStr = "DMIN:23";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    // 最大計測可能距離[mm]
                    dataStr = "DMAX:60000";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    // 角度分解能 (360°の分割数) 
                    dataStr = "ARES:1440";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    // 有効計測エリア開始ステップ番号
                    dataStr = "AMIN:0";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    // 有効計測エリア終了ステップ番号
                    dataStr = "AMAX:1080";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    // センサ正面ステップ番号
                    dataStr = "AFRT:540";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    // 標準操作角速度[rpm]
                    dataStr = "SCAN:2400";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    // 終端
                    sendStr += "\n";
                }
                // RS-232C要求
                else if (commandStr == "SS")
                {
                    string dataStr = "04";  // RS-232C未対応

                    // エコーバック
                    sendStr += rsvCmd[iCmd] + "\n";

                    // ステータス 00 or 99 以外はNG
                    sendStr += dataStr + ChackSum(dataStr) + "\n";
                    // 終端
                    sendStr += "\n";
                }
                // SCIPコマンド
                else if (rsvCmd[iCmd] == "SCIP2.0")
                {
                    // 
                    sendStr += rsvCmd[iCmd] + "\n";
                    // 終端
                    sendStr += "\n";
                }
                else if (rsvCmd[iCmd] == "VV")
                {
                    /*
                        VEND:Hokuyo Automatic Co., Ltd.;[ [LF] 
                        PROD:SOKUIKI Sensor URG-04LX;[ [LF] 
                        FIRM: 3.2.00(28/Aug./2007);f[LF] 
                        PROT:SCIP 2.0;N[LF] 
                        SERI: H0508486;T[LF][LF] 
                     */
                    string dataStr = "";

                    // エコーバック
                    sendStr += rsvCmd[iCmd] + "\n";
                    // ステータス 00 or 99 以外はNG
                    sendStr += "00P" + "\n";

                    //
                    dataStr = "VEND:Hokuyo Automatic Co., Ltd.";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    // 
                    dataStr = "PROD:SOKUIKI Sensor URG-04LX";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    //
                    dataStr = "FIRM: 3.2.00(28/Aug./2007)";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    // 
                    dataStr = "PROT:SCIP 2.0";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    // 
                    dataStr = "SERI: H0508486";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    // 終端
                    sendStr += "\n";
                }
                else if (rsvCmd[iCmd] == "II")
                {
                    /*
                    II [L F ] 0 0 P[L F ] 
                    MODL:URG-04LX(Hokuyo Automatic Co.,Ltd.);N  [L F ] 
                    LASR:OFF;7  [L F ] 
                    SCSP:Initial(600[rpm]) <-Default setting by user;A              [L F ] 
                    MESM:Measuring by Sensitive Mode;A             [L F ] 
                    SBPS:19200[bps] <-Default setting by user;A           [L F ] 
                    TIME:002AA9;f             [L F ] 
                    STAT: Sensor works well.;8   [N L2 ] 
                     */
                    string dataStr = "";

                    // エコーバック
                    sendStr += rsvCmd[iCmd] + "\n";
                    // ステータス 00 or 99 以外はNG
                    sendStr += "00P" + "\n";

                    //
                    dataStr = "MODL:URG-04LX(Hokuyo Automatic Co.,Ltd.)";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    //
                    dataStr = "LASR:OFF";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    //
                    dataStr = "SCSP:Initial(600[rpm]) <-Default setting by user";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    //
                    dataStr = "MESM:Measuring by Sensitive Mode";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    //
                    dataStr = "SBPS:19200[bps] <-Default setting by user";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    //
                    dataStr = "TIME:" + encode(nowMs, 4) + "0";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    //
                    dataStr = "STAT: Sensor works well.";
                    sendStr += dataStr + ";" + ChackSum(dataStr) + "\n";
                    // 終端
                    sendStr += "\n";
                }
                else
                {
                    string dataStr = "";

                    // わからないコマンドは　未定義コマンドとして返す
                    sendStr += rsvCmd[iCmd] + "\n";
                    dataStr = "0D";
                    sendStr += dataStr + ChackSum(dataStr) + "\n";
                    // 終端
                    sendStr += "\n";

                    /*
                    ◆  不良電文応答  ◆ 
                    未定義コマンド、欠落コマンド、規定したバイト数と一致しないコマンドを受けた場合などは、そのコ
                    マンド文字列のエコーバックと共にエラーステータスを返します 
 
                      ＊エラーステータス  ：  “0A”  …  内部送信電文作成エラー
                      “0B”  …  処理済コマンドの再受信、または、送信バッファ不足
                      “0C”  …  欠落コマンド１
                      “0D”  …  未定義コマンド１
                      “0E”  …  未定義コマンド２
                      “0F”  …  欠落コマンド２
                      “0G”  …  文字列が 17 文字以上存在
                      “0H”  …  文字列中に指定外文字が存在
                      “0 I”～  …  センサがF / Wアップデートモードにあるため処理不能
                      */
                    Console.WriteLine("UnKnown Command:" + rsvCmd[iCmd] );
                }

                /*
                 * hector slam urg_nodeからの通信
                receive:ME0000108001000

                receive:ND0000108001000
                */
            }


            return sendStr;
        }

        /// <summary>
        /// SCIPプロトコル エンコード
        /// </summary>
        /// <param name="val"></param>
        /// <param name="numByte"></param>
        /// <returns></returns>
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

        /// <summary>
        /// チェックサム
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        char ChackSum(string str)
        {
            byte[] data = System.Text.Encoding.GetEncoding("shift_jis").GetBytes(str);
            byte sum = 0;

            foreach (byte val in data)
            {
                sum += val;
            }

            return Convert.ToChar((sum&0x3F)+0x30);
        }
    }
}
