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
        public const string ipcAddr = "ipcConnect";
        public const string ipcAddrCom = "rosif";

        public int Counter { get; set; }

        // RE plot
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

        /// <summary>
        /// v-slam
        /// </summary>
        public double vslamPlotX;
        public double vslamPlotY;
        public double vslamAng;

        /// <summary>
        /// hector-slam
        /// </summary>
        public double hslamPlotX;
        public double hslamPlotY;
        public double hslamAng;
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
