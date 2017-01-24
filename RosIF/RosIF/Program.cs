
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRIpcLib;

namespace RosIF
{
    class Program
    {
        static void Main(string[] args)
        {
            // 接続情報
            string myAddr = "192.168.1.4";
            string roscoreAddr = "http://192.168.1.45:11311";

            // 更新タイミング MS
            const int sleepMS = 100;

            bool loopFlg = true;

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


            Console.WriteLine("VehircleRunner ROS-Interface Ver0.20");
            Console.WriteLine("");

            Console.WriteLine("Connect VehircleRunner IPC..");
            IpcServer ipc = new IpcServer();

            ROS_if_forVehicleRunner rosifVR = new ROS_if_forVehicleRunner();


            // 終了イベント
            Console.CancelKeyPress += (sender, e) => {
                Console.Clear();
                Console.WriteLine("終了....");

                loopFlg = false;
                // trueにすると、プログラムを終了させない
                e.Cancel = true;

                //rosifVR.disconnect();
            };


            Console.WriteLine("Connect ROS... myIP:" + myAddr +" / roscore IP:" + roscoreAddr);
            rosifVR.connect(myAddr, roscoreAddr);

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
                    if (null != ipc && null != ipc.RemoteObject)
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


                        /// v-slam
                        ipc.RemoteObject.vslamPlotX = rosifVR.vslamPlotX;
                        ipc.RemoteObject.vslamPlotY = rosifVR.vslamPlotY;
                        ipc.RemoteObject.vslamAng = rosifVR.vslamAng;
                        

                        /// amcl-slam
                        ipc.RemoteObject.amclPlotX = rosifVR.amclPlotX;
                        ipc.RemoteObject.amclPlotY = rosifVR.amclPlotY;
                        ipc.RemoteObject.amclAng  = rosifVR.amclAng;
                        

                        // URG ROS->VR SubScribe(購読)                      
                        for (int i = 0; i < rosifVR.urg_scan.Length; i++) {
                            ipc.RemoteObject.urgData[i] = rosifVR.urg_scan[i];
                        }
                        

                        // URG VR->ROS Publish(配信)
                        /*
                        for (int i = 0; i < rosifVR.urg_scan.Length; i++)
                        {
                            rosifVR.urg_scan_send[i] = ipc.RemoteObject.urgDataSend[i];
                            rosifVR.urg_intensities_send[i] = ipc.RemoteObject.urgIntensitiesSend[i];
                        }
                        */
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
                    Console.WriteLine("clock:" + rosifVR.rosClock.ToLongTimeString());
                    Console.WriteLine("vslamPlotX:" + rosifVR.vslamPlotX.ToString("f2") + "/ vslamPlotY:" + rosifVR.vslamPlotY.ToString("f2") + "/ vslamPlotZ:" + rosifVR.vslamPlotZ.ToString("f2")+"    ");
                    Console.WriteLine("vslamAng(Degree):" + (rosifVR.vslamAng*180.0/Math.PI).ToString("f2") + "      ");
                    //Console.WriteLine("vslamPlotX:" + ipc.RemoteObject.vslamPlotX.ToString("f2") + "/ vslamPlotY:" + ipc.RemoteObject.vslamPlotY.ToString("f2") + "/ vslamAng:" + rosifVR.vslamAng.ToString("f2"));
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
                        Console.WriteLine("urg:" + urgStr );
                    }
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
#endif
                // 処理を休止
                System.Threading.Thread.Sleep(sleepMS);
            }

            // trueのまま抜けていたらエラー
            if (loopFlg)
            {
                Console.WriteLine("エラー発生 アプリを終了します。");
            }

            // 
            rosifVR.disconnect();
        }
    }
}
