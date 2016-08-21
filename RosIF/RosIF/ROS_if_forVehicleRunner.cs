using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RosSharp;
using RosSharp.geometry_msgs;

namespace RosIF
{
    class ROS_if_forVehicleRunner
    {
        Node rosNode = null;

        public const string NodeName = "VehicleRunnerROSIF";


        public double rePlotX;
        public double rePlotY;
        public double reAng;

        // Compus
        public double compusDir;

        // RE パルス値
        public double reRpulse;
        public double reLpulse;

        // GPS
        public double gpsGrandX;
        public double gpsGrandY;

        /// v-slam
        public double vslamPlotX;
        public double vslamPlotY;
        public double vslamAng;

        /// amcl
        public double amclPlotX;
        public double amclPlotY;
        public double amclAng;

        public double[] urg_scan;

        public double[] urg_scan_send;


        //SubScriber
        // V-Slam
        RosSharp.Topic.Subscriber<RosSharp.visualization_msgs.Marker> subVSlam;
        // hokuyo-node
        RosSharp.Topic.Subscriber<RosSharp.sensor_msgs.LaserScan> subUrg;

        // Publisher
        // ロータリーエンコーダ　パルス値
        RosSharp.Topic.Publisher<RosSharp.geometry_msgs.Twist> pubRE;
        // ロータリーエンコーダ　プロット値
        RosSharp.Topic.Publisher<RosSharp.geometry_msgs.Twist> pubPlot;
        // 地磁気
        RosSharp.Topic.Publisher<RosSharp.geometry_msgs.Twist> pubCompus;
        // ＧＰＳ
        RosSharp.Topic.Publisher<RosSharp.geometry_msgs.Twist> pubGPS;
        // hokuyo-node
        RosSharp.Topic.Publisher<RosSharp.sensor_msgs.LaserScan> pubUrg;

        // ================================================================================================

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ROS_if_forVehicleRunner()
        {
            urg_scan = new double[1080];
            urg_scan_send = new double[1080];
        }


        // ================================================================================================

        /// <summary>
        /// SubScriber CallBack
        /// </summary>
        /// <param name="dt"></param>
        private void cbSubScriber_VSlam(RosSharp.visualization_msgs.Marker dt)
        {
            Console.WriteLine("time:" + dt.header.stamp.Second.ToString() + "," + dt.header.stamp.Millisecond.ToString());
            Console.WriteLine("pose:" + dt.pose.position.x.ToString() + "," + dt.pose.position.y.ToString() + "," + dt.pose.position.z.ToString());
            Console.WriteLine("color:" + dt.color.r.ToString() + "," + dt.color.g.ToString() + "," + dt.color.b.ToString());

            vslamPlotX = dt.pose.position.x;
            vslamPlotY = dt.pose.position.y;
        }

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
                Console.WriteLine("urg data OverFlow: dt.ranges.Count " + dt.ranges.Count.ToString() + "/ urg_scan.Length " + urg_scan.Length.ToString());
            }
        }

        /// <summary>
        /// test
        /// </summary>
        /// <param name="dt"></param>
        private void callBackTurtle(RosSharp.geometry_msgs.Twist dt)
        {
            Console.WriteLine("vec:" + dt.linear.x.ToString() + "," + dt.linear.y.ToString() + "," + dt.linear.z.ToString());
        }

        // ================================================================================================

        /// <summary>
        /// 送信
        /// </summary>
        public void Publish()
        {
            try
            {
                // ロータリーエンコーダ　パルス値
                if (null != pubRE)
                {
                    var data = new RosSharp.geometry_msgs.Twist() { linear = new Vector3() { x = reRpulse, y = reLpulse, z = 0.0 } };
                    //Console.WriteLine("data = {0}", data.data);
                    pubRE.OnNext(data);
                }

                // ロータリーエンコーダ　プロット値
                if (null != pubPlot)
                {
                    var data = new RosSharp.geometry_msgs.Twist() { linear = new Vector3() { x = rePlotX, y = rePlotY, z = 0.0 }, angular = new Vector3() { x = reAng, y = 0, z = 0 } };
                    //Console.WriteLine("data = {0}", data.data);
                    pubPlot.OnNext(data);
                }

                // 地磁気
                if (null != pubCompus)
                {
                    var data = new RosSharp.geometry_msgs.Twist() { linear = new Vector3() { x = 0.0, y = 0.0, z = compusDir } };
                    //Console.WriteLine("data = {0}", data.data);
                    pubCompus.OnNext(data);
                }

                // ＧＰＳ
                if (null != pubGPS)
                {
                    var data = new RosSharp.geometry_msgs.Twist() { linear = new Vector3() { x = gpsGrandX, y = gpsGrandY, z = 0.0 } };
                    //Console.WriteLine("data = {0}", data.data);
                    pubGPS.OnNext(data);
                }

                /*
                // URG
                if (null != pubUrg)
                {
                    var data = new RosSharp.sensor_msgs.LaserScan() { linear = new Vector3() { x = gpsGrandX, y = gpsGrandY, z = 0.0 } };
                    //Console.WriteLine("data = {0}", data.data);
                    pubUrg.OnNext(data);
                }*/

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
        public void connect( string myAddr, string tgtAddr )
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
                subVSlam = rosNode.SubscriberAsync<RosSharp.visualization_msgs.Marker>("/svo/points").Result;
                //var subscriber = rosNode.SubscriberAsync<RosSharp.geometry_msgs.Twist>("/turtle1/cmd_vel").Result;
                //var subscriber = rosNode.SubscriberAsync<RosSharp.std_msgs.String>("/chatter").Result;
                subUrg = rosNode.SubscriberAsync<RosSharp.sensor_msgs.LaserScan>("/last").Result;

                // Publisher生成
                pubRE = rosNode.PublisherAsync<RosSharp.geometry_msgs.Twist>("/vehiclerunner/re").Result;
                pubPlot = rosNode.PublisherAsync<RosSharp.geometry_msgs.Twist>("/vehiclerunner/replot").Result;
                pubCompus = rosNode.PublisherAsync<RosSharp.geometry_msgs.Twist>("/vehiclerunner/compus").Result;
                pubGPS = rosNode.PublisherAsync<RosSharp.geometry_msgs.Twist>("/vehiclerunner/gpsplot").Result;
                //pubUrg = rosNode.PublisherAsync<RosSharp.sensor_msgs.LaserScan>("/scan").Result;


                // メッセージを購読
                subVSlam.Subscribe(cbSubScriber_VSlam);
                subUrg.Subscribe(cbSubScriber_URG);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ROS Connect Error!");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// ROS切断
        /// </summary>
        public void disconnect()
        {
            if (null != rosNode)
            {
                rosNode.Dispose();
                rosNode = null;
                Ros.Dispose();
            }
        }


    }
}
