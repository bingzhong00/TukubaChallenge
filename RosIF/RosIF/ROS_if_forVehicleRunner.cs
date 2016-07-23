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

        /// hector-slam
        public double hslamPlotX;
        public double hslamPlotY;
        public double hslamAng;


        //SubScriber
        // V-Slam
        RosSharp.Topic.Subscriber<RosSharp.visualization_msgs.Marker> subVSlam;

        // Publisher
        // ロータリーエンコーダ　パルス値
        RosSharp.Topic.Publisher<RosSharp.geometry_msgs.Twist> pubRE;
        // ロータリーエンコーダ　プロット値
        RosSharp.Topic.Publisher<RosSharp.geometry_msgs.Twist> pubPlot;
        // 地磁気
        RosSharp.Topic.Publisher<RosSharp.geometry_msgs.Twist> pubCompus;
        // ＧＰＳ
        RosSharp.Topic.Publisher<RosSharp.geometry_msgs.Twist> pubGPS;


        /// <summary>
        /// SubScriber CallBack
        /// </summary>
        /// <param name="dt"></param>
        private void callBack(RosSharp.visualization_msgs.Marker dt)
        {
            Console.WriteLine("time:" + dt.header.stamp.Second.ToString() + "," + dt.header.stamp.Millisecond.ToString());
            Console.WriteLine("pose:" + dt.pose.position.x.ToString() + "," + dt.pose.position.y.ToString() + "," + dt.pose.position.z.ToString());
            Console.WriteLine("color:" + dt.color.r.ToString() + "," + dt.color.g.ToString() + "," + dt.color.b.ToString());

            vslamPlotX = dt.pose.position.x;
            vslamPlotY = dt.pose.position.y;
        }

        private void callBackTurtle(RosSharp.geometry_msgs.Twist dt)
        {
            Console.WriteLine("vec:" + dt.linear.x.ToString() + "," + dt.linear.y.ToString() + "," + dt.linear.z.ToString());
        }


        // 型を判明させる。
        // ※ subscrier  hector slam

        // ※ subscrier  urg_node


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
                var subscriber = rosNode.SubscriberAsync<RosSharp.visualization_msgs.Marker>("/svo/points").Result;
                //var subscriber = rosNode.SubscriberAsync<RosSharp.geometry_msgs.Twist>("/turtle1/cmd_vel").Result;
                //var subscriber = rosNode.SubscriberAsync<RosSharp.std_msgs.String>("/chatter").Result;

                // Publisher生成
                pubRE = rosNode.PublisherAsync<RosSharp.geometry_msgs.Twist>("/vehiclerunner/re").Result;
                pubPlot = rosNode.PublisherAsync<RosSharp.geometry_msgs.Twist>("/vehiclerunner/replot").Result;
                pubCompus = rosNode.PublisherAsync<RosSharp.geometry_msgs.Twist>("/vehiclerunner/compus").Result;
                pubGPS = rosNode.PublisherAsync<RosSharp.geometry_msgs.Twist>("/vehiclerunner/gpsplot").Result;


                // メッセージを購読
                subscriber.Subscribe(callBack);
                //subscriber.Subscribe(callBackTurtle);
                //subscriber.Subscribe(x => Console.WriteLine(x.ToString()));
                //subscriber.Subscribe(x => Console.WriteLine(x.data));
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


        /// <summary>
        /// 送信
        /// </summary>
        public void Publish()
        {
            try
            {
                // ロータリーエンコーダ　パルス値
                if( null != pubRE )
                {
                    var data = new RosSharp.geometry_msgs.Twist() { linear = new Vector3() { x = reRpulse, y = reLpulse, z = 0.0 } };
                    //Console.WriteLine("data = {0}", data.data);
                    pubRE.OnNext(data);
                }

                // ロータリーエンコーダ　プロット値
                if (null != pubPlot)
                {
                    var data = new RosSharp.geometry_msgs.Twist() { linear = new Vector3() { x = rePlotX, y = rePlotY, z = 0.0 }, angular = new Vector3() { x= reAng, y = 0, z = 0 } };
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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

    }
}
