using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace CersioIO
{
    public class UsbIOport
    {
        protected SerialPort serialPort = null;

        // SH2との通信
        public bool Open( string portName, int bouRate )
        {
            SerialPort serialPort1 = new SerialPort( portName, bouRate, Parity.None, 8, StopBits.One);
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);

            if (serialPort1.IsOpen) return true;
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

        private Byte[] sirialResvPool = new Byte[256];
        private int resvIdx = 0;
        public string resiveStr = "";


        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
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


    /// <summary>
    /// SH2用　通信ライブラリ
    /// </summary>
    public class DriveIOport : UsbIOport
    {
        public new void Close()
        {
            SendSirialData(0, 0);
            base.Close();
        }

        // いずれ分離か セルシオ通信系ライブラリ化
        public bool SendSirialData(double handleVal, double accVal)
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

            return true;
        }
    }
}
