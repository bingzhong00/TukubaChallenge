using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace iPentecSocket
{
    public partial class iPentecSyncClientSocket : Component
    {
        public iPentecSyncClientSocket()
        {
            //InitializeComponent();
        }

        public iPentecSyncClientSocket(IContainer container)
        {
            container.Add(this);

            //InitializeComponent();
        }

        private int port = 0;
        private string host = "";
        private int timeout = 0;

        Socket clientSocket = null;

        private static String response = String.Empty;

        private int MAX_BUFFER_SIZE = 2048;

        public enum SocketAction { SA_READ, SA_WRITE, SA_CLOSE, SA_NONE }

        // イベント処理用のデリゲート
        public delegate void ConnectEventHandler(Socket client);
        public delegate void DisconnectEventHandler();
        public delegate void ReceiveEventHandler(byte[] ReceiveData);
        public delegate void SendEventHandler(byte[] SendData);
        //
        public event ConnectEventHandler Connect = null;
        public event DisconnectEventHandler Disconnect = null;
        public event ReceiveEventHandler Receive = null;
        public event SendEventHandler Send = null;

        public int Port
        {
            set { port = value; }
            get { return port; }
        }

        public string Host
        {
            set { host = value; }
            get { return host; }
        }

        public int Timeout
        {
            set { timeout = value; }
            get { return timeout; }
        }

        private void DisposedEvent(object sender, EventArgs e)
        {
            Close(clientSocket);
        }

        public void Open()
        {
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(host);

                IPAddress ipAddress = ipHostInfo.AddressList[0];
                //IPv6でないアドレスを取得
                for (int i = 0; i < ipHostInfo.AddressList.Length; i++)
                {
                    if (ipHostInfo.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddress = ipHostInfo.AddressList[i];
                    }
                }
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP  socket.
                clientSocket = new Socket(
                  AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.ReceiveTimeout = timeout;

                try
                {
                    clientSocket.Connect(remoteEP);
                    if (Connect != null)
                    {
                        Connect(clientSocket);
                    }
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public void Close(Socket client)
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();

            if (Disconnect != null)
            {
                Close(clientSocket);
            }
        }

        public int Read(out byte[] bytes)
        {
            // Receive the response from the remote device.
            bytes = new byte[MAX_BUFFER_SIZE];

            int bytesReceive = clientSocket.Receive(bytes, SocketFlags.None);
            if (Receive != null)
            {
                Receive(bytes);
            }
            return bytesReceive;
        }

        public void Read(out byte[] bytes, out int bytesReceive)
        {
            bytesReceive = Read(out bytes);
        }

        public int Write(byte[] data, int bytesWrite)
        {
            int bytesRec = clientSocket.Send(data, bytesWrite, SocketFlags.None);

            if (Send != null)
            {
                Send(data);
            }
            return bytesRec;
        }

        public int Write(byte[] data)
        {
            int bytesRec = clientSocket.Send(data, SocketFlags.None);
            if (Send != null)
            {
                Send(data);
            }
            return bytesRec;
        }
    }
}
