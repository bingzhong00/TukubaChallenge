﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CersioIO
{
    public class TCPClient : IDisposable
    {

        private string ipString;//ListenするIPアドレス

        public string ipStringProperty
        {
            get { return ipString; }
            set { ipString = value; }
        }

        private int port;//Listenするポート番号

        public int portProperty
        {
            get { return port; }
            set { port = value; }
        }

        private System.Net.Sockets.NetworkStream ns;

        public System.Net.Sockets.NetworkStream MyProperty
        {
            get { return ns; }
        }

        private System.Net.Sockets.TcpClient objSck;

        public System.Net.Sockets.TcpClient SckProperty
        {
            get { return objSck; }
        }


        // 接続フラグ
        public bool ConnectResult = false;

        public TCPClient(string ipAddr, int ipPort )
        {
            ipString = ipAddr;
            port = ipPort;
            ConnectResult = false;
        }

        //~TCPClient()
        public void Dispose()
        {
            DisConnect();
        }

        /// <summary>
        /// 切断
        /// </summary>
        public void DisConnect()
        {
            if (ns != null)
            {
                try
                {
                    ns.Close();
                }
                catch { }
                ns = null;
            }
            if (objSck != null)
            {
                try
                {
                    objSck.Close();
                }
                catch { }
                objSck = null;
            }
				
            ConnectResult = false;
        }

        public bool Start()
        {
            System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(this.ipString);

            if (objSck != null)
            {
                objSck.Close();
                objSck = null;
            }
            
            objSck = new System.Net.Sockets.TcpClient();
            // 小さいバッファを貯めない(遅延させない)
            objSck.NoDelay = true;

            try
            {
                objSck.Connect(ipAdd, port);
            }
            catch(Exception)
            {
                return false;
            }
            //catch (Exception)
            //{
                
            //    throw;
            //}
            

            //NetworkStreamを取得
            ns = objSck.GetStream();

            ConnectResult = true;
            return ConnectResult;

        }


        public async void StartAsync()
        {
            System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(this.ipString);

            if (objSck != null)
            {
                objSck.Close();
                objSck = null;
            }

            objSck = new System.Net.Sockets.TcpClient();
            // 小さいバッファを貯めない(遅延させない)
            objSck.NoDelay = true;
            try
            {
                await objSck.ConnectAsync(ipAdd, port);
            }
            catch (Exception)
            {
                //return false;
                return;
            }

            //NetworkStreamを取得
            ns = objSck.GetStream();

            ConnectResult = true;
            //return ConnectResult;
        }

    }
}
