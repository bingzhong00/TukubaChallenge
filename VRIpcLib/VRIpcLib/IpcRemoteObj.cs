using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;

namespace VRIpcLib
{
    // 通信オブジェクト
    public class IpcRemoteObj : MarshalByRefObject
    {
        public const int numUrgData = 1080;

        /// <summary>
        /// アドレス
        /// </summary>
        public const string ipcAddr = "ipcConnect";
        /// <summary>
        /// チャンネル
        /// </summary>
        public const string ipcAddrCom = "rosif";

        /// <summary>
        /// 自動的に切断されるのを回避する
        /// </summary>
        public override object InitializeLifetimeService()
        {
            return null;
        }

        public int Counter { get; set; }

        // -------------------------------------------------
        // VR -> ROS 送信情報
        // -------------------------------------------------
        // RE plot
        public double rePlotX { get; set; }
        public double rePlotY { get; set; }
        public double reAng { get; set; }

        // Compus
        public double compusDir { get; set; }

        // RE パルス値
        public double reRpulse { get; set; }
        public double reLpulse { get; set; }

        // GPS
        public double gpsGrandX { get; set; }
        public double gpsGrandY { get; set; }

        /// <summary>
        /// 送信用
        /// </summary>
        //public List<double> urgDataSend { get; set; }
        //public List<double> urgIntensitiesSend { get; set; }

        // -------------------------------------------------
        // ROS -> VR 受信情報
        // -------------------------------------------------
        /// <summary>
        /// v-slam
        /// </summary>
        public double vslamPlotX { get; set; }
        public double vslamPlotY { get; set; }
        public double vslamAng { get; set; }

        /// <summary>
        /// amcl
        /// </summary>
        public double amclPlotX { get; set; }
        public double amclPlotY { get; set; }
        public double amclAng { get; set; }

        /// <summary>
        /// urg node
        /// </summary>
        public double[] _urgData = new double[numUrgData];
        
        public double[] urgData
        {
            set { _urgData = value; }
            get { return _urgData; }
        }

        // ※WatchDogを追加して、通信接続を監視する？
    }

    // サーバー
    public class IpcServer
    {
        public IpcRemoteObj RemoteObject { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IpcServer()
        {
            // サーバーチャンネルの生成
            IpcServerChannel channel = new IpcServerChannel(IpcRemoteObj.ipcAddr);

            // チャンネルを登録
            ChannelServices.RegisterChannel(channel, true);

            // リモートオブジェクトを生成して公開
            RemoteObject = new IpcRemoteObj();
            RemotingServices.Marshal(RemoteObject, IpcRemoteObj.ipcAddrCom, typeof(IpcRemoteObj));
        }
    }

    // クライアント
    public class IpcClient
    {
        public IpcRemoteObj RemoteObject { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IpcClient()
        {
            // クライアントチャンネルの生成
            IpcClientChannel channel = new IpcClientChannel();

            // チャンネルを登録
            ChannelServices.RegisterChannel(channel, true);

            // リモートオブジェクトを取得
            RemoteObject = Activator.GetObject(typeof(IpcRemoteObj), "ipc://" + IpcRemoteObj.ipcAddr + "/" + IpcRemoteObj.ipcAddrCom) as IpcRemoteObj;
        }
    }
}
