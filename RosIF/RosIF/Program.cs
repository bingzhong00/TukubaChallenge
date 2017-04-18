
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using VRIpcLib;

namespace RosIF
{
    class Program
    {
        static ROS_if_forVehicleRunner rosifVR = new ROS_if_forVehicleRunner();
        static bool loopFlg = true;

        static void Main(string[] args)
        {
            // 接続情報
            string myAddr = "192.168.1.2";
            //string roscoreAddr = "http://192.168.1.80:11311"; // Gazebo Emu
            string roscoreAddr = "http://192.168.1.101:11311";   // Raspberry Pi

            // 更新タイミング MS
            const int sleepMS = 100;

            Thread thread = null;

            // パラメータ取得
            // RosIF.exe 自IP roscore側IP
            if (args.Length >= 1)
            {
                myAddr = args[0];
            }
            if (args.Length >= 2)
            {
                roscoreAddr = args[1];
            }


            {
                Console.WriteLine("自身のIPアドレスをチェック...");
                // ホスト名を取得する
                string hostname = Dns.GetHostName();

                // ホスト名からIPアドレスを取得する
                IPAddress[] adrList = Dns.GetHostAddresses(hostname);

                if (adrList.Length == 1)
                {
                    myAddr = adrList[0].ToString();
                    Console.WriteLine("自身のIPアドレスを設定:" + myAddr);
                }
                else
                {
                    bool bCheck = false;
                    foreach (IPAddress address in adrList)
                    {
                        Console.WriteLine(address.ToString());
                        if (myAddr == address.ToString())
                        {
                            bCheck = true;
                        }
                    }

                    if (!bCheck)
                    {
                        Console.WriteLine("合致するIPアドレスがありません。:" + myAddr);
                    }
                    else
                    {
                        Console.WriteLine("IPアドレスが合致しました。:" + myAddr);
                    }
                }
                Console.WriteLine("");
            }


            Console.WriteLine("VehircleRunner ROS-Interface Ver1.00");
            Console.WriteLine("");

            Console.WriteLine("Connect VehircleRunner IPC..");
            IpcServer ipc = new IpcServer();


            // 終了イベント
            Console.CancelKeyPress += (sender, e) =>
            {
                Console.Clear();
                Console.WriteLine("終了....");

                // ループを抜けることで終了させる
                loopFlg = false;
                // trueにすると、プログラムを終了させない
                e.Cancel = true;

                //rosifVR.disconnect();
            };


            // ROS 接続
            Console.WriteLine("Connect ROS... myIP:" + myAddr + " / roscore IP:" + roscoreAddr);

            if (!rosifVR.connect(myAddr, roscoreAddr))
            {
                // ROS接続失敗
                Console.WriteLine("");
                Console.WriteLine("***** Exit *****");

                System.Threading.Thread.Sleep(3000);
                return;
            }

            thread = new Thread(new ThreadStart(ThreadFunction));
            thread.Start();

            Console.WriteLine("");
            Console.WriteLine("***** Start *****");
            Console.WriteLine("CTRL+Cで、アプリを終了します。");
            Console.WriteLine("");

            // 現在のカーソル行取得
            int curStartRow = Console.CursorTop;

            while (loopFlg)
            {
#if true
                try
                {
                    // IPC通信
                    if (null != ipc && null != ipc.RemoteObject)
                    {
                        // ------------------------------------------------
                        // ROS -> VR 受信データ
                        {
                            // amcl
                            ipc.RemoteObject.amclPlotX = rosifVR.amclPlotX;
                            ipc.RemoteObject.amclPlotY = rosifVR.amclPlotY;
                            ipc.RemoteObject.amclAng = rosifVR.amclAng;

                            // URG ROS->VR SubScribe(購読)                      
                            for (int i = 0; i < rosifVR.urg_scan.Length; i++)
                            {
                                ipc.RemoteObject.urgData[i] = rosifVR.urg_scan[i];
                            }

                            ipc.RemoteObject.reRpulse = rosifVR.ReDirectR;
                            ipc.RemoteObject.reLpulse = rosifVR.ReDirectL;
                        }

                        // -----------------------------------------------
                        // VR -> ROS 送信データ
                        {
                            rosifVR.sendHandle = ipc.RemoteObject.sendHandle;
                            rosifVR.sendAccel = ipc.RemoteObject.sendAccel;

                            rosifVR.ledCommand = ipc.RemoteObject.ledCommand;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("IPC Connect Error!");
                    Console.WriteLine(ex.Message);

                    break;
                }

                // コンソール表示
                {
                    Console.WriteLine("Subscribe ----------- ");
                    //Console.WriteLine("clock:" + rosifVR.rosClock.ToLongTimeString());
                    //Console.WriteLine("vslamPlotX:" + rosifVR.vslamPlotX.ToString("f2") + "/ vslamPlotY:" + rosifVR.vslamPlotY.ToString("f2") + "/ vslamPlotZ:" + rosifVR.vslamPlotZ.ToString("f2")+"    ");
                    //Console.WriteLine("vslamAng(Degree):" + (rosifVR.vslamAng*180.0/Math.PI).ToString("f2") + "      ");
                    //Console.WriteLine("vslamPlotX:" + ipc.RemoteObject.vslamPlotX.ToString("f2") + "/ vslamPlotY:" + ipc.RemoteObject.vslamPlotY.ToString("f2") + "/ vslamAng:" + rosifVR.vslamAng.ToString("f2"));
                    Console.WriteLine("ReDirectL:" + rosifVR.ReDirectL.ToString("f2") + " / ReDirectR:" + rosifVR.ReDirectR.ToString("f2"));
                    Console.WriteLine("amclX:" + rosifVR.amclPlotX.ToString("f2") + "/ amclY:" + rosifVR.amclPlotY.ToString("f2") + "/ amclAng:" + rosifVR.amclAng.ToString("f2"));

                    {
                        string urgStr = "";
                        if (null != ipc.RemoteObject.urgData)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                urgStr += rosifVR.urg_scan[(ROS_if_forVehicleRunner.numUrgData * i / 8)].ToString("f2") + ",";
                                //urgStr += ipc.RemoteObject.urgData[(ROS_if_forVehicleRunner.numUrgData * i / 8)].ToString("f2") + ",";
                            }
                        }
                        Console.WriteLine("urg:" + urgStr);
                    }
                    Console.WriteLine("");

                    Console.WriteLine("Publish ------------- ");
                    Console.WriteLine("Handle:" + rosifVR.sendHandle.ToString("f2") + "/ Accel:" + rosifVR.sendAccel.ToString("f2"));
                    //Console.WriteLine("reRpulse:" + rosifVR.reRpulse.ToString("f2") + "/ reLpulse:" + rosifVR.reLpulse.ToString("f2"));
                    //Console.WriteLine("rePlotX:" + rosifVR.rePlotX.ToString("f2") + "/ rePlotY:" + rosifVR.rePlotY.ToString("f2") + "/ reAng:" + rosifVR.reAng.ToString("f2"));
                    //Console.WriteLine("compusDir:" + rosifVR.compusDir.ToString("f2"));
                    //Console.WriteLine("gpsGrandX:" + rosifVR.gpsGrandX.ToString("f2") + "/ gpsGrandY:" + rosifVR.gpsGrandY.ToString("f2"));
                }

                // 送信(Publish)
                //rosifVR.Publish();

                // カーソル位置を初期化
                Console.SetCursorPosition(0, curStartRow);
#endif
                // 処理を休止
                System.Threading.Thread.Sleep(sleepMS);
                //System.Threading.Thread.Sleep(0);
            }

            System.Threading.Thread.Sleep(100);

            // trueのまま抜けていたらエラー
            if (loopFlg)
            {
                Console.WriteLine("エラー発生 「loopFlg が Trueのままループを抜けている。」アプリを終了します。");
            }

            // 
            rosifVR.disconnect();
        }

        /// <summary>
        /// ROS 送信ループ
        /// </summary>
        static void ThreadFunction()
        {
            while (loopFlg)
            {
                rosifVR.Publish();
                System.Threading.Thread.Sleep(0);
            }
        }
    }
}
