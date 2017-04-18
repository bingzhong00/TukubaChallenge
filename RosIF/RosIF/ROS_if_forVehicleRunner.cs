using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RosSharp;
using RosSharp.geometry_msgs;
using RosSharp.rosgraph_msgs;

namespace RosIF
{
    class ROS_if_forVehicleRunner
    {
        private Node rosNode = null;

        public const string NodeName = "VehicleRunnerROSIF";

        public string ledCommand = "";

        /// amcl
        public double amclPlotX;
        public double amclPlotY;
        public double amclAng;

        public double ReDirectR;
        public double ReDirectL;

        // ROS LRF
        public double[] urg_scan;

        // 
        public double sendHandle;
        public double sendAccel;


        public DateTime rosClock = DateTime.Now;

        // SubScriber ---------------------------------
        // hokuyo-node
        RosSharp.Topic.Subscriber<RosSharp.sensor_msgs.LaserScan> subUrg;
        // [debug] clock
        RosSharp.Topic.Subscriber<RosSharp.rosgraph_msgs.Clock> subClock;
        // tf base_link
        RosSharp.Topic.Subscriber<RosSharp.geometry_msgs.Twist> subRosif_pub;
        // RE Pulse Direct
        RosSharp.Topic.Subscriber<RosSharp.geometry_msgs.Vector3> subREDirect;


        // Publisher ---------------------------------
        //RosSharp.Topic.Publisher<RosSharp.rosgraph_msgs.Clock> pubClock;
        // LED
        RosSharp.Topic.Publisher<RosSharp.std_msgs.String> pubBenzLED;
        // cmd_vel
        RosSharp.Topic.Publisher<RosSharp.geometry_msgs.Twist> pubCmdVel;


        // ================================================================================================
        public const int numUrgData = 1080;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ROS_if_forVehicleRunner()
        {
            urg_scan = new double[numUrgData];
        }


        // ================================================================================================
        /// <summary>
        /// hokuyo-node /scan
        /// </summary>
        /// <param name="dt"></param>
        private void cbSubScriber_URG(RosSharp.sensor_msgs.LaserScan dt)
        {
            if (dt.ranges.Count-1 <= urg_scan.Length)
            {
                for (int i = 0; i < dt.ranges.Count-1; i++)
                {
                    urg_scan[i] = dt.ranges[i];
                }
            }
            else
            {
                // データ数が異なる場合
                Console.WriteLine("urg data OverFlow: dt.ranges.Count " + dt.ranges.Count.ToString() + "/ urg_scan.Length " + urg_scan.Length.ToString());
            }
        }

        /// <summary>
        /// SubScriber CallBack
        /// </summary>
        /// <param name="dt"></param>
        private void cbSubScriber_rofif_pub(RosSharp.geometry_msgs.Twist dt)
        {
            amclPlotX = dt.linear.x;
            amclPlotY = dt.linear.y;
            amclAng = dt.angular.z;
        }

        /// <summary>
        /// SubScriber CallBack
        /// </summary>
        /// <param name="dt"></param>
        private void cbSubScriber_reDirect(RosSharp.geometry_msgs.Vector3 dt)
        {
            ReDirectR = dt.x;
            ReDirectL = dt.y;
            //amclAng = dt.angular.z;
        }

        /// <summary>
        /// [debug] clock
        /// </summary>
        /// <param name="dt"></param>
        private void cbSubScriber_Clock(RosSharp.rosgraph_msgs.Clock dt)
        {
            //Console.WriteLine("time:" + dt.clock.ToString() );

            rosClock = dt.clock;
        }


        // ================================================================================================
        /// <summary>
        /// 送信
        /// </summary>
        public void Publish()
        {
            try
            {
                // LED
                if (null!= pubBenzLED)
                {
                    var data = new RosSharp.std_msgs.String() { data = ledCommand };
                    pubBenzLED.OnNext(data);
                }

                // Publish cmd_vel
                if (null != pubCmdVel)
                {
                    var data = new RosSharp.geometry_msgs.Twist()
                    {
                        // linear X + 前進
                        // angular Z 左右
                        linear = new Vector3() { x = sendAccel, y = 0.0, z = 0.0 },
                        angular = new Vector3() { x = 0.0, y = 0.0, z = sendHandle }
                    };

                    pubCmdVel.OnNext(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        // ================================================================================================

        /// <summary>
        /// ROS接続
        /// </summary>
        /// <param name="myAddr"></param>
        /// <param name="tgtAddr"></param>
        public bool connect( string myAddr, string tgtAddr )
        {
            // ローカルネットワークのホスト名またはIPアドレス
            Ros.HostName = myAddr;
            // Masterへの接続URI
            Ros.MasterUri = new Uri(tgtAddr);
            // ROS TOPICのタイムアウト時間[msec]
            Ros.TopicTimeout = 3000;
            // XML-RPCのメソッド呼び出しのタイムアウト時間[msec]
            Ros.XmlRpcTimeout = 3000;

            // Subscriver
            try
            {
                // Nodeの生成。Nodeが生成されるまで待つ。
                // Node名
                rosNode = Ros.InitNodeAsync(NodeName).Result;

                // Subscriberの生成。Subscriberが生成されるまで待つ。
                subRosif_pub = rosNode.SubscriberAsync<RosSharp.geometry_msgs.Twist>("/rosif/base_link").Result;

                //subUrg = rosNode.SubscriberAsync<RosSharp.sensor_msgs.LaserScan>("/scan").Result;
                //subUrg = rosNode.SubscriberAsync<RosSharp.sensor_msgs.LaserScan>("/last").Result;
                subUrg = rosNode.SubscriberAsync<RosSharp.sensor_msgs.LaserScan>("/torosif/scan").Result;
                //subClock = rosNode.SubscriberAsync<RosSharp.rosgraph_msgs.Clock>("/clock").Result;
                subREDirect = rosNode.SubscriberAsync<RosSharp.geometry_msgs.Vector3>("/sh2_encLR_Direct").Result;

                // Publisher生成
                //pubClock = rosNode.PublisherAsync<RosSharp.rosgraph_msgs.Clock>("/clock").Result;
                pubBenzLED = rosNode.PublisherAsync<RosSharp.std_msgs.String>("/benz/led").Result;
                pubCmdVel = rosNode.PublisherAsync<RosSharp.geometry_msgs.Twist>("/cmd_vel").Result;

                // Subscribe CallBack指定
                if( null!= subUrg)          subUrg.Subscribe(cbSubScriber_URG);
                if (null != subRosif_pub)   subRosif_pub.Subscribe(cbSubScriber_rofif_pub);
                if (null != subREDirect)    subREDirect.Subscribe(cbSubScriber_reDirect);
                
                if (null != subClock)       subClock.Subscribe(cbSubScriber_Clock);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ROS Connect Error!");
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// ROS切断
        /// </summary>
        public void disconnect()
        {
            Console.Clear();
            Console.WriteLine("ROS Disconnect..");

            if (null != rosNode)
            {
                
                try
                {
                    System.Threading.Thread.Sleep(0);


                    try
                    {
                        if (null != pubBenzLED)
                        {
                            pubBenzLED.WaitForDisconnection();
                            //pubBenzLED.Dispose();
                            pubBenzLED = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    try
                    {
                        if (null != pubCmdVel)
                        {
                            pubCmdVel.WaitForDisconnection();
                            //pubCmdVel.Dispose();
                            pubCmdVel = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    try
                    {
                        if (null != subRosif_pub)
                        {
                            subRosif_pub.WaitForDisconnection();
                            //subRosif_pub.Dispose();
                            subRosif_pub = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    try
                    {
                        if (null != subUrg)
                        {
                            subUrg.WaitForDisconnection();
                            //subUrg.Dispose();
                            subUrg = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    try
                    {
                        if (null != subREDirect)
                        {
                            subREDirect.WaitForDisconnection();
                            //subREDirect.Dispose();
                            subREDirect = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    try
                    {
                        if (null != subClock)
                        {
                            subClock.WaitForDisconnection();
                            //subClock.Dispose();
                            subClock = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    System.Threading.Thread.Sleep(0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                /*
                try
                {
                    rosNode.Dispose();
                    //rosNode.DisposeAsync();
                    Console.WriteLine("ROS Node Dispose");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    rosNode = null;
                }
                */

                rosNode = null;

                System.Threading.Thread.Sleep(1000);

                try
                {
                    Ros.Dispose();
                    Console.WriteLine("ROS Dispose");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                System.Threading.Thread.Sleep(3000);
            }
        }

        // ================================================================================================
    }
}
