/*!
 * \file
 * \brief Get distance data from Ethernet type URG
 * \author Jun Fujimoto
 * $Id: get_distance_ethernet.cs 403 2013-07-11 05:24:12Z fujimoto $
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace SCIP_library
{

    public class URG_LRF
    {
        public const string LRF_IP_ADDR = "192.168.1.10";
        public const int LRF_PORT = 10940;

        // データを何個飛ばしにするか。
        public static int LRFskipNum = 4;

        public static double LRF_Scale = 1.0;

        TcpClient urg = null;
        NetworkStream stream = null;

        URGLogRead urgLog = null;

        /// <summary>
        /// IP モードでオープン
        /// </summary>
        /// <param name="LRFip">ip addr</param>
        /// <param name="LRFport">ip port</param>
        /// <returns></returns>
        public bool IpOpen(string LRFip = LRF_IP_ADDR, int LRFport = LRF_PORT)
        {
            try
            {
                urg = new TcpClient();
                urg.NoDelay = true;
                urg.Connect(LRFip, LRFport);
                stream = urg.GetStream();

                if (null != stream)
                {
                    // SCIP Ver2 Command
                    write(stream, SCIP_Writer.SCIP2());
                    read_line(stream); // ignore echo back
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// ログファイルモードで　オープン
        /// </summary>
        /// <param name="fname"></param>
        /// <returns></returns>
        public bool LogFileOpen(string fname)
        {
            urgLog = new URGLogRead();
            return urgLog.OpenFile(fname);
        }

        /// <summary>
        /// 各モードに応じて、クローズ
        /// </summary>
        public void Close()
        {
            if (null != urgLog)
            {
                urgLog.CloseFile();
                urgLog = null;
            }

            if (null != stream)
            {
                try
                {
                    write(stream, SCIP_Writer.QT());    // stop measurement mode
                    read_line(stream); // ignore echo back

                    stream.Close();
                    urg.Close();

                    stream = null;
                }
                catch
                {
                }
            }
        }


        /// <summary>
        /// スケール値セット
        /// </summary>
        /// <param name="val"></param>
        public static void setScale(double val)
        {
            LRF_Scale = val;
            SCIP_Reader.SetScale(val);
            URGLogRead.SetScale(val);
        }

        /// <summary>
        /// スケール値取得
        /// </summary>
        /// <returns></returns>
        public static double getScale()
        {
            return LRF_Scale;
        }

        /// <summary>
        /// URGデータ取得
        /// </summary>
        /// <returns></returns>
        public double[] getScanData()
        {
            if (null != stream)
            {
                // LAN接続中のURG から取得
                return getScanDataIP();
            }
            else if (null != urgLog)
            {
                // ログファイルから取得
                double[] data = urgLog.getScanData();

                if (data == null || data.Length == 0)
                {
                    return data;
                }

                // 1080のデータから 270へ変換
                double[] modData = new double[data.Length / LRFskipNum];
                int modIdx = 0;
                for (int i = 0; i < data.Length; i += LRFskipNum)
                {
                    double minVal = data[i];
                    for (int n = 1; n < LRFskipNum; n++)
                    {
                        if (minVal < data[i + n]) minVal = data[i + n];
                    }
                    modData[modIdx] = minVal;
                    modIdx++;
                }

                return modData;
            }

            return new double[0];
        }

        /// <summary>
        /// LANからデータ取得
        /// </summary>
        /// <returns></returns>
        public double[] getScanDataIP()
        {
            const int start_step = 0;
            const int end_step = 1080;

            try
            {
                write(stream, SCIP_Writer.MD(start_step, end_step,4,1));  //   270=度のParticleFilterの仕様にあわせる
                //read_line(stream);  // ignore echo back

                List<double> distances = new List<double>();
                long time_stamp = 0;
                bool retryFlg = true;

                while (retryFlg)
                {
                    string receive_data = read_line(stream);
                    if (!SCIP_Reader.MD(receive_data, ref time_stamp, ref distances))
                    {
                        //Console.WriteLine(receive_data);
                        break;
                    }

                    if (distances.Count == 0)
                    {
                        Console.WriteLine("not DistanceData :" + receive_data);
                        continue;
                    }
                    else
                    {
                        retryFlg = false;
                    }
                    // show distance data
                    //Console.WriteLine("time stamp: " + time_stamp.ToString() + " distance[100] : " + distances[100].ToString());
                }

                return distances.ToArray();
            }
            catch
            {
                return new double[0];
            }
        }




        public static void TestMode()
        {
            const int GET_NUM = 10;
            const int start_step = 0;
            const int end_step = 1080;

            try
            {
                string ip_address = LRF_IP_ADDR;
                int port_number = LRF_PORT;
                //get_connect_information(out ip_address, out port_number);

                TcpClient urg = new TcpClient();
                urg.Connect(ip_address, port_number);
                NetworkStream stream = urg.GetStream();

                write(stream, SCIP_Writer.SCIP2());
                read_line(stream); // ignore echo back
                write(stream, SCIP_Writer.MD(start_step, end_step));
                read_line(stream);  // ignore echo back

                List<double> distances = new List<double>();
                long time_stamp = 0;
                for (int i = 0; i < GET_NUM; ++i)
                {
                    string receive_data = read_line(stream);
                    if (!SCIP_Reader.MD(receive_data, ref time_stamp, ref distances))
                    {
                        Console.WriteLine(receive_data);
                        break;
                    }
                    if (distances.Count == 0)
                    {
                        Console.WriteLine(receive_data);
                        continue;
                    }
                    // show distance data
                    Console.WriteLine("time stamp: " + time_stamp.ToString() + " distance[100] : " + distances[100].ToString());
                }
                write(stream, SCIP_Writer.QT());    // stop measurement mode
                read_line(stream); // ignore echo back
                stream.Close();
                urg.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Press any key.");
                //Console.ReadKey();
            }
        }

        /// <summary>
        /// アドレス、ポートを実行時に手動変更
        /// </summary>
        private static void get_connect_information(out string ip_address, out int port_number)
        {
            ip_address = LRF_IP_ADDR;
            port_number = LRF_PORT;
            Console.WriteLine("Please enter IP Address. [default: " + ip_address + "]");
            string str = Console.ReadLine();
            if (str != "")
            {
                ip_address = str;
            }
            Console.WriteLine("Please enter Port number. [default: " + port_number.ToString() + "]");
            str = Console.ReadLine();
            if (str != "")
            {
                port_number = int.Parse(str);
            }

            Console.WriteLine("Connect setting = IP Address : " + ip_address + " Port number : " + port_number.ToString());
        }

        /// <summary>
        /// Read to "\n\n" from NetworkStream
        /// </summary>
        /// <returns>receive data</returns>
        static string read_line(NetworkStream stream)
        {
            if (stream.CanRead)
            {
                StringBuilder sb = new StringBuilder();
                bool is_NL2 = false;
                bool is_NL = false;
                do
                {
                    char buf = (char)stream.ReadByte();
                    if (buf == '\n')
                    {
                        if (is_NL)
                        {
                            is_NL2 = true;
                        }
                        else
                        {
                            is_NL = true;
                        }
                    }
                    else
                    {
                        is_NL = false;
                    }
                    sb.Append(buf);
                } while (!is_NL2);

                return sb.ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// write data
        /// </summary>
        static bool write(NetworkStream stream, string data)
        {
            if (stream.CanWrite)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                stream.Write(buffer, 0, buffer.Length);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}