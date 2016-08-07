using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CersioIO;

namespace RosIF
{
    class Program
    {
        static void Main(string[] args)
        {
            // 接続情報
            string myAddr = "192.168.1.4";
            string roscoreAddr = "http://192.168.1.32:11311";

            // 更新タイミング MS
            const int sleepMS = 100;


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


            Console.WriteLine("VehircleRunner ROS-Interface Ver0.10");
            Console.WriteLine("");

            Console.WriteLine("Connect VehircleRunner IPC..");

            IpcClient ipc = new IpcClient();
            ROS_if_forVehicleRunner rosifVR = new ROS_if_forVehicleRunner();


            // 終了イベント
            Console.CancelKeyPress += (sender, e) => {
                Console.WriteLine("終了");

                // trueにすると、プログラムを終了させない
                //e.Cancel = true;

                rosifVR.disconnect();
            };


            Console.WriteLine("Connect ROS... myIP:" + myAddr +" / roscore IP:" + roscoreAddr);
            rosifVR.connect(myAddr, roscoreAddr);

            Console.WriteLine("");
            Console.WriteLine("***** Start *****");
            Console.WriteLine("CTRL+Cで、アプリを終了します。");
            Console.WriteLine("");

            // 現在のカーソル行取得
            int curStartRow = Console.CursorTop;

            while ( true )
            {
                try
                {
                    if (null != ipc.RemoteObject)
                    {
                        rosifVR.rePlotX = ipc.RemoteObject.rePlotX;
                        rosifVR.rePlotY = ipc.RemoteObject.rePlotY;
                        rosifVR.reAng = ipc.RemoteObject.reAng;

                        // Compus
                        rosifVR.compusDir = ipc.RemoteObject.compusDir;

                        // RE パルス値
                        rosifVR.reRpulse = ipc.RemoteObject.reRpulse;
                        rosifVR.reLpulse = ipc.RemoteObject.reLpulse;

                        // GPS
                        rosifVR.gpsGrandX = ipc.RemoteObject.gpsGrandX;
                        rosifVR.gpsGrandY = ipc.RemoteObject.gpsGrandY;

                        /*
                        /// v-slam
                        rosifVR.vslamPlotX;
                        rosifVR.vslamPlotY;
                        rosifVR.vslamAng;

                        /// amcl-slam
                        rosifVR.hslamPlotX;
                        rosifVR.hslamPlotY;
                        rosifVR.hslamAng;
                        */

                        // URG ROS->VR SubScribe(購読)
                        for (int i = 0; i < rosifVR.urg_scan.Length; i++) {
                            ipc.RemoteObject.urgData[i] = rosifVR.urg_scan[i];
                        }

                        // URG VR->ROS Publish(配信)
                        for (int i = 0; i < rosifVR.urg_scan.Length; i++)
                        {
                            rosifVR.urg_scan_send[i] = ipc.RemoteObject.urgDataSend[i];
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("IPC Connect Error!");
                    Console.WriteLine(ex.Message);

                    /*
                    {
                        rosifVR.rePlotX += 1.0;
                        rosifVR.rePlotY += 1.0;
                        rosifVR.reAng += 1.0;

                        // Compus
                        rosifVR.compusDir += 1.0;

                        // RE パルス値
                        rosifVR.reRpulse += 1.0;
                        rosifVR.reLpulse += 1.0;

                        // GPS
                        rosifVR.gpsGrandX += 1.0;
                        rosifVR.gpsGrandY += 1.0;
                    }*/

                    break;
                }

                // コンソール表示
                {
                    Console.WriteLine("Subscribe ----------- ");
                    Console.WriteLine("vslamPlotX:" + rosifVR.vslamPlotX.ToString("f2") + "/ vslamPlotY:" + rosifVR.vslamPlotY.ToString("f2") + "/ vslamAng:" + rosifVR.vslamAng.ToString("f2"));
                    //Console.WriteLine("hslamPlotX:" + rosifVR.hslamPlotX.ToString("f2") + "/ hslamPlotY:" + rosifVR.hslamPlotY.ToString("f2") + "/ hslamAng:" + rosifVR.hslamAng.ToString("f2"));
                    Console.WriteLine("");

                    Console.WriteLine("Publish ------------- ");
                    Console.WriteLine("reRpulse:" + rosifVR.reRpulse.ToString("f2") + "/ reLpulse:" + rosifVR.reLpulse.ToString("f2"));
                    Console.WriteLine("rePlotX:" + rosifVR.rePlotX.ToString("f2") + "/ rePlotY:" + rosifVR.rePlotY.ToString("f2") + "/ reAng:" + rosifVR.reAng.ToString("f2"));
                    Console.WriteLine("compusDir:" + rosifVR.compusDir.ToString("f2"));
                    Console.WriteLine("gpsGrandX:" + rosifVR.gpsGrandX.ToString("f2") + "/ gpsGrandY:" + rosifVR.gpsGrandY.ToString("f2"));
                }

                // 送信(Publish)
                rosifVR.Publish();

                // カーソル位置を初期化
                Console.SetCursorPosition(0, curStartRow);

                // 処理を休止
                System.Threading.Thread.Sleep(sleepMS);
            }

            Console.WriteLine("エラー発生 アプリを終了します。");

            // 
            rosifVR.disconnect();

            // ３秒後終了
            System.Threading.Thread.Sleep(3000);
        }
    }
}
