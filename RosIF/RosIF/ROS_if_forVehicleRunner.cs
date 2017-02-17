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

        public string ledCommand;

        /// v-slam
        public double vslamPlotX;
        public double vslamPlotY;
        public double vslamPlotZ;
        public double vslamAng;

        /// amcl
        public double amclPlotX;
        public double amclPlotY;
        public double amclAng;

        public double[] urg_scan;

        public double[] urg_scan_send;
        public double[] urg_intensities_send;

        public DateTime rosClock = DateTime.Now;

        // SubScriber ---------------------------------
        // V-Slam
 //       RosSharp.Topic.Subscriber<RosSharp.visualization_msgs.Marker> subVSlam;
        RosSharp.Topic.Subscriber<RosSharp.geometry_msgs.Twist> subVSlam;
        // hokuyo-node
        RosSharp.Topic.Subscriber<RosSharp.sensor_msgs.LaserScan> subUrg;
        // [debug] clock
        RosSharp.Topic.Subscriber<RosSharp.rosgraph_msgs.Clock> subClock;
        // tf base_link
        RosSharp.Topic.Subscriber<RosSharp.geometry_msgs.Twist> subRosif_pub;


        // Publisher ---------------------------------
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

        RosSharp.Topic.Publisher<RosSharp.rosgraph_msgs.Clock> pubClock;

        // LED
        RosSharp.Topic.Publisher<RosSharp.std_msgs.String> pubBenzLED;

        // cmd_vel
        RosSharp.Topic.Publisher<RosSharp.geometry_msgs.Twist> pubCmdVel;


        // ================================================================================================
        public const int numUrgData = 1080;

        private Queue<Vector3> vSlamMoveVecQue = new Queue<Vector3>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ROS_if_forVehicleRunner()
        {
            urg_scan = new double[numUrgData];
            urg_scan_send = new double[numUrgData];
            urg_intensities_send = new double[numUrgData];
        }


        // ================================================================================================

        /// <summary>
        /// SubScriber CallBack
        /// </summary>
        /// <param name="dt"></param>
        //private void cbSubScriber_VSlam(RosSharp.visualization_msgs.Marker dt)
        private void cbSubScriber_VSlam(RosSharp.geometry_msgs.Twist dt)
        {
#if false
            if (dt.color.r == 0.0f && dt.color.g == 0.0f && dt.color.b == 0.5f && dt.color.a == 1.0f)
            {
                Console.WriteLine("seqid:" + dt.header.seq.ToString());
                Console.WriteLine("time:" + dt.header.stamp.Ticks.ToString() + " ,MS " + dt.header.stamp.Millisecond.ToString());
                Console.WriteLine("pose: ");// + dt.pose.position.x.ToString() + "," + dt.pose.position.y.ToString() + "," + dt.pose.position.z.ToString());
                Console.WriteLine("   position: ");// + dt.pose.position.x.ToString() + "," + dt.pose.position.y.ToString() + "," + dt.pose.position.z.ToString());
                Console.WriteLine("             x:" + dt.pose.position.x.ToString());
                Console.WriteLine("             y:" + dt.pose.position.y.ToString());
                Console.WriteLine("             z:" + dt.pose.position.z.ToString());
                //Console.WriteLine("color:" + dt.color.r.ToString() + "," + dt.color.g.ToString() + "," + dt.color.b.ToString());
            }
#endif

            // カメラに対して
            // 左右      X:   左 -1.0 <----> +1.0 右
            // 上下　    Y:   上 +1.0 <----> -1.0 下
            // 奥・手前　Z: 手前 +1.0 <----> -1.0 奥

            // 特定の色のマーカー情報を読む
            //if ( dt.color.r == 0.0f && dt.color.g == 0.0f && dt.color.b == 0.5f && dt.color.a == 1.0f )
            {
                // 単位変換 m -> mm
#if true
                vslamPlotX = dt.linear.x * 1000.0;
                vslamPlotY = dt.linear.y * 1000.0;
                vslamPlotZ = dt.linear.z * 1000.0;    // とりあえず Z軸も保持
#else
                vslamPlotX = dt.pose.position.x * 1000.0;
                vslamPlotY = dt.pose.position.y * 1000.0;
                vslamPlotZ = dt.pose.position.z * 1000.0;    // とりあえず Z軸も保持
#endif

#if false
                // Quatanionから角度(向き)へ変換
                vslamAng = MatrixMath.QuaternionToAngle((float)dt.pose.orientation.x,
                                                         (float)dt.pose.orientation.y,
                                                         (float)dt.pose.orientation.z,
                                                         (float)dt.pose.orientation.w);
#endif

#if true
                // キューに入っているベクトルを足して方向に変換
                {
                    vSlamMoveVecQue.Enqueue(dt.linear);

                    Vector3 dirVec = new Vector3();
                    Vector3 prevVec = vSlamMoveVecQue.First();
                    foreach (var vec in vSlamMoveVecQue)
                    {
                        dirVec.x += (vec.x - prevVec.x);
                        dirVec.y += (vec.y - prevVec.y);
                        dirVec.z += (vec.z - prevVec.z);

                        prevVec = vec;
                    }

                    double vecLeng = Math.Sqrt(dirVec.x * dirVec.x) + Math.Sqrt(dirVec.y * dirVec.y);
                    if (vecLeng*1000.0 > 1.0)
                    {
                        // 正規化
                        dirVec.x /= vecLeng;
                        dirVec.y /= vecLeng;

                        // 方向へ変換
                        vslamAng = Math.Atan2(dirVec.y, dirVec.x);
                    }


                    if (vSlamMoveVecQue.Count > 10)
                    {
                        vSlamMoveVecQue.Dequeue();
                    }
                }
#endif

            }
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
        /// [debug] clock
        /// </summary>
        /// <param name="dt"></param>
        private void cbSubScriber_Clock(RosSharp.rosgraph_msgs.Clock dt)
        {
            //Console.WriteLine("time:" + dt.clock.ToString() );

            rosClock = dt.clock;
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

                
                // URG
                if (null != pubUrg)
                {

                    //var data = new RosSharp.sensor_msgs.LaserScan(); // { linear = new Vector3() { x = gpsGrandX, y = gpsGrandY, z = 0.0 } };
                    //Console.WriteLine("data = {0}", data.data);
                    var data = MakeLaserScanData();

                    pubUrg.OnNext(data);
                }

                
                if (null != pubClock)
                {
                    var data = new RosSharp.rosgraph_msgs.Clock()  { clock = DateTime.Now };
                    pubClock.OnNext(data);
                }

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
                        linear = new Vector3() { x = 1.0, y = 0.0, z = 0.0 },
                        angular = new Vector3() { x = 0.0, y = 0.0, z = 0.0 }
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
                // subVSlam = rosNode.SubscriberAsync<RosSharp.visualization_msgs.Marker>("/svo/points").Result;
                subVSlam = rosNode.SubscriberAsync<RosSharp.geometry_msgs.Twist>("/torosif/vslam").Result;
                //var subscriber = rosNode.SubscriberAsync<RosSharp.geometry_msgs.Twist>("/turtle1/cmd_vel").Result;
                //var subscriber = rosNode.SubscriberAsync<RosSharp.std_msgs.String>("/chatter").Result;

                //subRosif_pub = rosNode.SubscriberAsync <RosSharp.geometry_msgs.Twist> ("/rosif/amcl_result").Result;
                subRosif_pub = rosNode.SubscriberAsync<RosSharp.geometry_msgs.Twist>("/rosif/base_link").Result;

                //subUrg = rosNode.SubscriberAsync<RosSharp.sensor_msgs.LaserScan>("/scan").Result;
                //subUrg = rosNode.SubscriberAsync<RosSharp.sensor_msgs.LaserScan>("/last").Result;
                subUrg = rosNode.SubscriberAsync<RosSharp.sensor_msgs.LaserScan>("/torosif/scan").Result;
                subClock = rosNode.SubscriberAsync<RosSharp.rosgraph_msgs.Clock>("/clock").Result;
                

                // Publisher生成
                pubRE = rosNode.PublisherAsync<RosSharp.geometry_msgs.Twist>("/vehiclerunner/re").Result;
                pubPlot = rosNode.PublisherAsync<RosSharp.geometry_msgs.Twist>("/vehiclerunner/replot").Result;
                pubCompus = rosNode.PublisherAsync<RosSharp.geometry_msgs.Twist>("/vehiclerunner/compus").Result;
                pubGPS = rosNode.PublisherAsync<RosSharp.geometry_msgs.Twist>("/vehiclerunner/gpsplot").Result;
                //pubUrg = rosNode.PublisherAsync<RosSharp.sensor_msgs.LaserScan>("/scanVR").Result;

                //pubClock = rosNode.PublisherAsync<RosSharp.rosgraph_msgs.Clock>("/clock").Result;
                pubBenzLED = rosNode.PublisherAsync<RosSharp.std_msgs.String>("/benz/led").Result;
                pubCmdVel = rosNode.PublisherAsync<RosSharp.geometry_msgs.Twist>("/cmd_vel").Result;

                // Subscribe CallBack指定
                if (null != subVSlam) subVSlam.Subscribe(cbSubScriber_VSlam);
                if( null!= subUrg)    subUrg.Subscribe(cbSubScriber_URG);
                if (null != subRosif_pub)    subRosif_pub.Subscribe(cbSubScriber_rofif_pub);
                if (null != subClock) subClock.Subscribe(cbSubScriber_Clock);
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
                try
                {
                    if (null != pubRE) pubRE.Dispose();
                    if (null != pubPlot) pubPlot.Dispose();
                    if (null != pubCompus) pubCompus.Dispose();
                    if (null != pubGPS) pubGPS.Dispose();
                    if (null != pubUrg) pubUrg.Dispose();
                    if (null != pubBenzLED) pubBenzLED.Dispose();
                    if (null != pubCmdVel) pubCmdVel.Dispose();

                    if (null != subVSlam) subVSlam.Dispose();
                    if (null != subRosif_pub) subRosif_pub.Dispose();
                    if (null != subUrg) subUrg.Dispose();
                    if (null != subClock) subClock.Dispose();

                } catch { }

                try
                {
                    rosNode.Dispose();
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

        static uint seqCnt = 0;
        /// <summary>
        /// LaserScan データ作成
        /// </summary>
        /// <returns></returns>
        public RosSharp.sensor_msgs.LaserScan MakeLaserScanData()
        {
            var dat = new RosSharp.sensor_msgs.LaserScan();

            {
                var header = new RosSharp.std_msgs.Header();
                header.seq = seqCnt;
                header.stamp = rosClock; 
                header.frame_id = "laser";

                dat.header = header;
                seqCnt++;
            }

            int num_readings = urg_scan_send.Length;
            float laser_frequency = 40;

            // public float angle_min { get; set; }
            dat.angle_min = (-270.0f/2.0f) * (float)Math.PI / 360.0f;

            // public float angle_max { get; set; }
            dat.angle_max = (270.0f / 2.0f) * (float)Math.PI / 360.0f;

            //public float angle_increment { get; set; }
            dat.angle_increment = (270.0f * (float)Math.PI / 360.0f) / num_readings;

            //public float time_increment { get; set; }
            dat.time_increment = (1 / laser_frequency) / (num_readings);

            //public float scan_time { get; set; }
            dat.scan_time = 0.025f;

            //public float range_min { get; set; }
            //public float range_max { get; set; }
            dat.range_min = 0.025f;
            dat.range_max = 30.0f;

            //public List<float> ranges { get; set; }
            //public List<float> intensities { get; set; }
            for (int i = 0; i < urg_scan_send.Length; i++)
            {
                dat.ranges.Add( (float)(urg_scan_send[i]+i*0.1f) );
                dat.intensities.Add( (float)(urg_intensities_send[i]+i) );
            }

            return dat;
        }

    }
}
