using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;

namespace CersioIO
{
    // 通信オブジェクト
    class IpcRemoteObj : MarshalByRefObject
    {
        public IpcRemoteObj()
        {
            urgData = new double[1080];
            urgDataSend = new double[1080];
        }

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
        public double[] urgDataSend { get; set; }

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
        /// hector-slam
        /// </summary>
        public double hslamPlotX { get; set; }
        public double hslamPlotY { get; set; }
        public double hslamAng { get; set; }

        /// <summary>
        /// urg node
        /// </summary>
        public double[] urgData { get; set; }
       
    }

    // サーバー
    class IpcServer
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
    class IpcClient
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
