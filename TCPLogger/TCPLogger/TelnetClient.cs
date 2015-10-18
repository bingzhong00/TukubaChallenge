using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iPentecSocket;

namespace TCPLogger
{

    // ※　サンプル添付のみ　未実装

    class TelnetClient
    {
        static iPentecSyncClientSocket sock;

        //Telnet Commands
        const byte cmdSE = 0xF0;
        const byte cmdNOP = 0xF1;
        const byte cmdDM = 0xF2;
        const byte cmdBRK = 0xF3;
        const byte cmdIP = 0xF4;
        const byte cmdAO = 0xF5;
        const byte cmdAYT = 0xF6;
        const byte cmdEC = 0xF7;
        const byte cmdEL = 0xF8;
        const byte cmdGA = 0xF9;
        const byte cmdSB = 0xFA;

        const byte cmdWILL = 0xFB;
        const byte cmdWONT = 0xFC;
        const byte cmdDO = 0xFD;
        const byte cmdDONT = 0xFE;
        const byte cmdIAC = 0xFF;

        //Telnet Options
        const byte op_suppress_go_ahead = 0x03;
        const byte op_status = 0x05;
        const byte op_echo = 0x01;
        const byte op_timing_mark = 0x06;
        const byte op_terminal_type = 0x18;
        const byte op_window_size = 0x1F;
        const byte op_terminal_speed = 0x20;
        const byte op_remote_flow_control = 0x21;
        const byte op_linemode = 0x22;
        const byte op_environment_variables = 0x24;

        //static void Main(string[] args)
        static void TelnetMain(string[] args)
        {
            sock = new iPentecSyncClientSocket();
            sock.Connect += new iPentecSyncClientSocket.ConnectEventHandler(sock_Connect);

            sock.Port = 23;
            sock.Host = "yamaha-router.ipentec.com";
            sock.Timeout = 5000;
            sock.Open();

            byte[] data = new byte[15];
            data[0] = cmdIAC;
            data[1] = cmdWILL;
            data[2] = op_terminal_type;
            data[3] = cmdIAC;
            data[4] = cmdDO;
            data[5] = op_suppress_go_ahead;
            data[6] = cmdIAC;
            data[7] = cmdWILL;
            data[8] = op_suppress_go_ahead;
            data[9] = cmdIAC;
            data[10] = cmdDO;
            data[11] = op_echo;
            data[12] = cmdIAC;
            data[13] = cmdWILL;
            data[14] = op_window_size;
            sock.Write(data);

            int rbytes = sock.Read(out data);
            string recv = DataToString(data, rbytes);
            System.Console.WriteLine(recv);

            data = new byte[3];
            data[0] = cmdIAC;
            data[1] = cmdWONT;
            data[2] = op_echo;
            sock.Write(data);

            data = new byte[11];
            data[0] = cmdIAC;
            data[1] = cmdSB;
            data[2] = op_window_size;
            data[3] = 0x00;
            data[4] = 0x50; //80
            data[5] = 0x00;
            data[6] = 0x18; //24
            data[7] = cmdIAC;
            data[8] = cmdSE;
            data[9] = cmdDO;
            data[10] = op_echo;
            sock.Write(data);

            data = new byte[3];
            data[0] = cmdIAC;
            data[1] = cmdDONT;
            data[2] = op_status;
            sock.Write(data);

            data = new byte[3];
            data[0] = cmdIAC;
            data[1] = cmdWONT;
            data[2] = op_remote_flow_control;
            sock.Write(data);

            data = new byte[1];
            data[0] = 0x0D; //ここでCRを送っておくと1回目でログインできる。
            sock.Write(data);

            rbytes = sock.Read(out data);
            recv = DataToString(data, rbytes);
            System.Console.WriteLine(recv);


            //0D 0A Password
            recv = Encoding.ASCII.GetString(data);
            System.Console.WriteLine(recv);
            while (recv.IndexOf("Password") < 0)
            {
                sock.Read(out data);
                recv = Encoding.ASCII.GetString(data);
            }

            //(CRを送信していない場合は1度目は失敗する)
            string cmd = "ROUTER-PASSWORD\r";
            byte[] cmddata = Encoding.ASCII.GetBytes(cmd);
            sock.Write(cmddata);

            rbytes = sock.Read(out data);
            recv = Encoding.GetEncoding("SHIFT_JIS").GetString(data);
            System.Console.WriteLine(recv);

            //(CRを送信していない場合は2度目でログインできる。)
            /*
            sock.Write(cmddata);
            sock.Read(out data);
            recv = Encoding.GetEncoding("SHIFT_JIS").GetString(data);
            System.Console.WriteLine(recv);
            */

            //">"を待つ
            while (recv.IndexOf(">") < 0)
            {
                sock.Read(out data);
                recv = Encoding.GetEncoding("SHIFT_JIS").GetString(data);
            }

            cmd = "administrator\r\n";
            sock.Write(Encoding.ASCII.GetBytes(cmd));

            //"Password"を待つ
            rbytes = sock.Read(out data);

            recv = Encoding.GetEncoding("SHIFT_JIS").GetString(data);
            System.Console.WriteLine(recv);
            while (recv.IndexOf("Password") < 0)
            {
                rbytes = sock.Read(out data);

                recv = Encoding.GetEncoding("SHIFT_JIS").GetString(data);
                System.Console.WriteLine(recv);
            }

            cmd = "ROUTER-ADMINISTRATOR-PASSWORD\r";
            sock.Write(Encoding.ASCII.GetBytes(cmd));

            //"#"を待つ
            rbytes = sock.Read(out data);

            recv = Encoding.GetEncoding("SHIFT_JIS").GetString(data);
            System.Console.WriteLine(recv);
            while (recv.IndexOf("#") < 0)
            {
                rbytes = sock.Read(out data);

                recv = Encoding.ASCII.GetString(data);
                System.Console.WriteLine(recv);
            }

            //何らかの処理
        }

        static void sock_Send(byte[] SendData)
        {
            //throw new NotImplementedException();
        }

        static void sock_Receive(byte[] ReceiveData)
        {
            //throw new NotImplementedException();
        }

        static string DataToString(byte[] data, int dataBytes)
        {
            string result = "";
            for (int i = 0; i < Math.Min(data.Length, dataBytes); i++)
            {
                result += CmdToString(data[i]) + " ";
            }

            return result;
        }

        static string CmdToString(byte data)
        {
            switch (data)
            {
                case 0xF0:
                    return "SE";
                    break;
                case 0xF1:
                    return "NOP";
                    break;
                case 0xF2:
                    return "DM";
                    break;
                case 0xF3:
                    return "BRK";
                    break;
                case 0xF4:
                    return "IP";
                    break;
                case 0xF5:
                    return "AO";
                    break;
                case 0xF6:
                    return "AYT";
                    break;
                case 0xF7:
                    return "EC";
                    break;
                case 0xF8:
                    return "EC";
                    break;
                case 0xF9:
                    return "GA";
                    break;
                case 0xFA:
                    return "SB";
                    break;

                case 0xFB:
                    return "WILL";
                    break;
                case 0xFC:
                    return "WONT";
                    break;
                case 0xFD:
                    return "DO";
                    break;
                case 0xFE:
                    return "DONT";
                    break;
                case 0xFF:
                    return "IAC";
                    break;

                case 0x03:
                    return "suppress_go_ahead";
                    break;
                case 0x05:
                    return "status";
                    break;
                case 0x01:
                    return "echo";
                    break;
                case 0x06:
                    return "timing_mark";
                    break;
                case 0x18:
                    return "terminal_type";
                    break;
                case 0x1F:
                    return "window_size";
                    break;
                case 0x20:
                    return "terminal_speed";
                    break;
                case 0x21:
                    return "remote_flow_control";
                    break;
                case 0x22:
                    return "linemode";
                    break;
                case 0x24:
                    return "environment_variables";
                    break;

                default:
                    return "unknown";
                    break;
            }

        }

        static void sock_Connect(System.Net.Sockets.Socket client)
        {
            System.Console.WriteLine("Connect");
        }
    }
}
