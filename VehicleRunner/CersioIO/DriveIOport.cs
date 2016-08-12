using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Management;

namespace CersioIO
{
    /// <summary>
    /// USBシリアルデータ、送受信
    /// </summary>
    public class UsbIOport
    {
        protected SerialPort serialPort = null;

        // SH2との通信
        public bool Open( string portName, int bouRate )
        {
            serialPort = new SerialPort(portName, bouRate, Parity.None, 8, StopBits.One);
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            serialPort.Open();

            if (serialPort.IsOpen) return true;
            return false;
        }

        public void Close()
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                // 停止
                serialPort.Close();
            }
        }

        public bool IsConnect()
        {
            if (null == serialPort) return false;

            return serialPort.IsOpen;
        }

        // シリアル通信部 -------------------------------------------------------------------------------
        public bool SendSirialData(string data )
        {
            // シリアルポートにデータ送信
            if (!serialPort.IsOpen)
            {
                return false;
            }

            byte[] dat = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetBytes(data);
            serialPort.Write(dat, 0, dat.GetLength(0));

            return true;
        }

        protected Byte[] sirialResvPool = new Byte[256];
        protected int resvIdx = 0;
        public string resiveStr = "";


        private void serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            // シリアルポートからデータ受信
            {
                Byte[] data = new Byte[serialPort.BytesToRead];
                serialPort.Read(data, 0, data.GetLength(0));

                //ASCII エンコード
                //string text = System.Text.Encoding.ASCII.GetString(data);

                //データがShift-JISの場合
                //string text = System.Text.Encoding.GetEncoding("shift_jis").GetString(data);

                //データがEUCの場合
                //string text = System.Text.Encoding.GetEncoding("euc-jp").GetString(data);

                //データがunicodeの場合
                //string text = System.Text.Encoding.Unicode.GetString(data);

                //データがutf-8の場合
                string text = System.Text.Encoding.UTF8.GetString(data);
                resiveStr += text;
            }
        }

        /// <summary>
        /// デバイス名一覧
        /// </summary>
        /// <returns></returns>
        public static string[] GetDeviceNames()
        {
            var deviceNameList = new System.Collections.ArrayList();
            var check = new System.Text.RegularExpressions.Regex("(COM[1-9][0-9]?[0-9]?)");

            ManagementClass mcPnPEntity = new ManagementClass("Win32_PnPEntity");
            ManagementObjectCollection manageObjCol = mcPnPEntity.GetInstances();

            //全てのPnPデバイスを探索しシリアル通信が行われるデバイスを随時追加する
            foreach (ManagementObject manageObj in manageObjCol)
            {
                //Nameプロパティを取得
                var namePropertyValue = manageObj.GetPropertyValue("Name");
                if (namePropertyValue == null)
                {
                    continue;
                }

                //Nameプロパティ文字列の一部が"(COM1)～(COM999)"と一致するときリストに追加"
                string name = namePropertyValue.ToString();
                if (check.IsMatch(name))
                {
                    deviceNameList.Add(name);
                }
            }

            //戻り値作成
            if (deviceNameList.Count > 0)
            {
                string[] deviceNames = new string[deviceNameList.Count];
                int index = 0;
                foreach (var name in deviceNameList)
                {
                    deviceNames[index++] = name.ToString();
                }
                return deviceNames;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// デバイス名リストを返す
        /// </summary>
        /// <returns></returns>
        public static string[] GetDeviceList()
        {
            // すべてのシリアル・ポート名を取得する
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            string[] portsName = GetDeviceNames();


            if (ports.Length > 0)
            {
                string[] portListStr = new string[ports.Length];

                int i = 0;
                foreach (string port in ports)
                {
                    if (null != portsName)
                    {
                        portListStr[i] = port + ":" + portsName[i];
                    }
                    else
                    {
                        portListStr[i] = port;
                    }
                    i++;
                }

                return portListStr;
            }
            else
            {
                return null;
            }
        }
    }


    /// <summary>
    /// SH2用　通信ライブラリ
    /// </summary>
    public class DriveIOport : UsbIOport
    {
        public new void Close()
        {
            Send_AC_Command(0, 0);
            base.Close();
        }

        // 送信
        public bool Send_AC_Command(double handleVal, double accVal)
        {
            // シリアルポートにデータ送信
            if (!serialPort.IsOpen)
            {
                return false;
            }

            /*
            typedef	struct {				// struct 入力データ	
	            union {						// 						
		            _BYTE		B_DATA[4];	//  Byte Access			
		            struct {				//  Bit  Access			
			            _WORD	S_DATA[2];	//    エンコードデータ	(アクセル値、ハンドル値)
		            } ;						//              
	            } DATA;
              
	            _BYTE		IO_DATA;    	//  OUTPUT
	            _BYTE		CR;				//  チェックサム
            } IN_DATA;
             * */

            Byte[] dat = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            Byte sum = 0;

            short accSH = (short)(1280 + ((accVal + 1.0) / 2.0) * 2048);
            short hdlSH = (short)(1280 + ((handleVal + 1.0) / 2.0) * 2048);

            dat[0] = (Byte)((hdlSH >> 8) & 0xFF);
            dat[1] = (Byte)((hdlSH) & 0xFF);

            dat[2] = (Byte)((accSH >> 8) & 0xFF);
            dat[3] = (Byte)((accSH) & 0xFF);

            dat[4] = (Byte)0x00;    // IO_DATA

            // チェックサム計算
            for (int i = 0; i < 5; i++)
            {
                sum ^= dat[i];
            }
            dat[5] = sum;

            //System.Text.Encoding.GetEncoding("SHIFT-JIS").GetBytes("abcあいう");
            serialPort.Write(dat, 0, dat.GetLength(0));

            // 受信データを削除しておく
            resiveStr = "";

            return true;
        }

        private void serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            // シリアルポートからデータ受信
            {
                Byte[] buff = new Byte[serialPort.BytesToRead];
                serialPort.Read(buff, 0, buff.GetLength(0));

                // 受信サイズの10バイトたまるまで、受け取る
                for (int i = 0; i < buff.Length; i++)
                {
                    sirialResvPool[resvIdx + i] = buff[i];
                }
                resvIdx += buff.Length;
            }

            // 10バイト受け取るまで待つ
            if (resvIdx < 10) return;
            resvIdx = 0;



            try
            {
                for (int i = 0; i < 10; i++)
                {
                    resiveStr += ((int)sirialResvPool[i]).ToString() + " ";
                }
            }
            catch
            {
            }
        }
    }
}
