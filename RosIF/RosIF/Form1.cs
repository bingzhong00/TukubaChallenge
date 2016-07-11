using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RosSharp;
//using CersioIO;

namespace RosIF
{
    public partial class Form1 : Form
    {
        public const string NodeName = "VehicleRunnerROSIF";

        //IpcClient ipc = new IpcClient();

        Node rosNode = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void callBack(RosSharp.visualization_msgs.Marker dt)
        {
            Console.WriteLine( "time:" + dt.header.stamp.Second.ToString() + "," + dt.header.stamp.Millisecond.ToString());
            Console.WriteLine( "pose:" + dt.pose.position.x.ToString() + "," + dt.pose.position.y.ToString() + "," + dt.pose.position.z.ToString());
            Console.WriteLine( "color:" + dt.color.r.ToString() + "," + dt.color.g.ToString() + "," + dt.color.b.ToString());

            //ipc.RemoteObject.vslamPlotX = dt.pose.position.x;
            //ipc.RemoteObject.vslamPlotY = dt.pose.position.y;
        }

        private void callBackTurtle(RosSharp.geometry_msgs.Twist dt)
        {
            Console.WriteLine("vec:" + dt.linear.x.ToString() + "," + dt.linear.y.ToString() + "," + dt.linear.z.ToString());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // ローカルネットワークのホスト名またはIPアドレス
            Ros.HostName = "192.168.1.4";
            // Masterへの接続URI
            Ros.MasterUri = new Uri("http://192.168.1.32:11311");
            // ROS TOPICのタイムアウト時間[msec]
            Ros.TopicTimeout = 3000;
            // XML-RPCのメソッド呼び出しのタイムアウト時間[msec]
            Ros.XmlRpcTimeout = 3000;

#if true
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

                // メッセージを購読
                subscriber.Subscribe( callBack );
                //subscriber.Subscribe(callBackTurtle);
                //subscriber.Subscribe(x => Console.WriteLine(x.ToString()));
                //subscriber.Subscribe(x => Console.WriteLine(x.data));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Subscribe Error!");
                Console.WriteLine(ex.Message);
            }
#else
            // Subscriver
            try
            {
                // Nodeの生成。Nodeが生成されるまで待つ。
                // Node名
                rosNode = Ros.InitNodeAsync(NodeName).Result;

                // Subscriberの生成。Subscriberが生成されるまで待つ。
                //var subscriber = rosNode.SubscriberAsync<RosSharp.geometry_msgs.Twist>("/turtle1/cmd_vel").Result;
                var subscriber = rosNode.SubscriberAsync<RosSharp.std_msgs.String>("/chatter").Result;

                // メッセージを購読
                //subscriber.Subscribe(x => Console.WriteLine(x.linear.x.ToString() + x.linear.y.ToString() + x.linear.z.ToString()));
                //subscriber.Subscribe(x => Console.WriteLine(x.ToString()));
                subscriber.Subscribe(x => Console.WriteLine(x.data));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Subscribe Error!");
                Console.WriteLine(ex.Message);
            }
#endif

            Console.WriteLine("start");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Ros.Dispose();
        }
    }
}
