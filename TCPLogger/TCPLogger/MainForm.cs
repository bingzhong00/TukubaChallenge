using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using SCIP_library;

namespace TCPLogger
{
    public partial class MainForm : Form
    {
//        TCPServerCheck objTCPSC;
        TCPClient objTCPSC;// default 192.168.1.1:50001
        TCPServerCheck objTCPSC_INFO;// default 192.168.1.1:9020
        BServerReader bsReder;

        public URG_LRF urgLRF;
        public bool LRF_IPConnectFlg = false;

        Bitmap autoMapBmp;
        public const int mapWidthPix = 3000;
        public const int mapHeightPix = 3000;

        public double[] LRF_Data;
        private string saveLogFname = "";

        public MainForm()
        {
            InitializeComponent();

            // 初期値をセット
            objTCPSC = new TCPClient();
            textBox_ChangeIPAdress.Text = objTCPSC.ipStringProperty;
            textBox_port.Text = objTCPSC.portProperty.ToString();

            objTCPSC_INFO = new TCPServerCheck();
            textBox_ChangeIPAdressINFO.Text = objTCPSC_INFO.ipStringProperty;
            textBox_portINFO.Text = objTCPSC_INFO.portProperty.ToString();

            urgLRF = new URG_LRF();
            bsReder = new BServerReader();

            // SavePath
            tb_LogFile.Text = Path.GetDirectoryName(Directory.GetCurrentDirectory()) + "\\" + GetNowTimeStampFileName("tcpLog", ".log");
            tb_MapLogFile.Text = Path.GetDirectoryName(Directory.GetCurrentDirectory()) + "\\" + GetNowTimeStampFileName("AutoMap", "png");

            // 300m四方
            autoMapBmp = new Bitmap(mapWidthPix, mapHeightPix);
            {
                Graphics g = Graphics.FromImage(autoMapBmp);
                g.FillRectangle(Brushes.Black, 0, 0, mapWidthPix, mapHeightPix);
                g.Dispose();
            }
            picbox_LRFMap.Image = autoMapBmp;
        }

        /// <summary>
        /// LRF接続
        /// </summary>
        /// <returns></returns>
        public bool ConnectLRF(string logFname)
        {
            urgLRF.Close();

            if (!string.IsNullOrEmpty(logFname))
            {
                urgLRF.setSaveLogFile(logFname);
            }

            LRF_IPConnectFlg = urgLRF.IpOpen();
            return LRF_IPConnectFlg;
        }

        /// <summary>
        /// LRF切断
        /// </summary>
        public void DisconnectLRF()
        {
            urgLRF.Close();
            LRF_IPConnectFlg = false;
        }


        /// <summary>
        /// BServer 接続ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Connect_Click(object sender, EventArgs e)
        {
            bool check;

            try
            {
                objTCPSC.ipStringProperty = textBox_ChangeIPAdress.Text;
                objTCPSC.portProperty = Int32.Parse(textBox_port.Text);
            }
            catch
            {
            }

            //toolStripStatusLabel1.Text = textBox_ChangeIPAdress.Text + " Connecting...";
            textBox_ChangeIPAdress.BackColor = Color.Yellow;
            textBox_port.BackColor = Color.Yellow;

            check = objTCPSC.Start();
            if (check || true )
            {
                // 接続ＯＫ
                button_sendCMD.Enabled = true;
                saveLogFname = tb_LogFile.Text;

                //toolStripStatusLabel1.Text = textBox_ChangeIPAdress.Text + " Connected";

                textBox_port.BackColor = Color.Lime;
                textBox_ChangeIPAdress.BackColor = Color.Lime;

                textBox_sendCMD.Focus();
            }
            else
            {
                // 接続ＮＧ
                //toolStripStatusLabel1.Text = textBox_ChangeIPAdress.Text + " Connect Fail!";

                textBox_port.BackColor = Color.Red;
                textBox_ChangeIPAdress.BackColor = Color.Red;
            }
        }


        /// <summary>
        /// Telnet 接続ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ConnectINFO_Click(object sender, EventArgs e)
        {
            bool check;

            try
            {
                objTCPSC_INFO.ipStringProperty = textBox_ChangeIPAdressINFO.Text;
                objTCPSC_INFO.portProperty = Int32.Parse(textBox_portINFO.Text);
            }
            catch
            {
            }

            //toolStripStatusLabel1.Text = textBox_ChangeIPAdressINFO.Text + " Connecting...";

            check = objTCPSC_INFO.Start();
            if (check)
            {
                button_sendCMD.Enabled = true;

                //toolStripStatusLabel1.Text = textBox_ChangeIPAdressINFO.Text + " Connected";
            }
            else
            {
                //toolStripStatusLabel1.Text = textBox_ChangeIPAdressINFO.Text + " Connect Fail!";
            }
        }

        private void textBox_ChangeIPAdress_Leave(object sender, EventArgs e)
        {
            objTCPSC.ipStringProperty = textBox_ChangeIPAdress.Text;
        }

        private void textBox_port_Leave(object sender, EventArgs e)
        {
            objTCPSC.portProperty = Convert.ToInt32(textBox_port.Text);
        }





        private void button_sendCMD_Click(object sender, EventArgs e)
        {
            TCP_SendCommand(textBox_sendCMD.Text + "\n");
            textBox_sendCMD.Focus();
        }

        private void TCP_SendCommand(string comStr)
        {
            System.Net.Sockets.NetworkStream objStm = objTCPSC.MyProperty;

            if (objStm != null)
            {
                Byte[] dat = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetBytes(comStr);
                objStm.Write(dat, 0, dat.GetLength(0));
            }
        }


        /// <summary>
        /// 更新タイマ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Bserver データ取得
            System.Net.Sockets.TcpClient objSck = objTCPSC.SckProperty;
            System.Net.Sockets.NetworkStream objStm = objTCPSC.MyProperty;

            if (objStm != null && objSck != null)
            {
                // ソケット受信
                if (objSck.Available > 0)
                {
                    Byte[] dat = new Byte[objSck.Available];

                    objStm.Read(dat, 0, dat.GetLength(0));

                    string readStr = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetString(dat);

                    // リストボックス更新
                    listBox_ReceiveData.Items.Add(readStr);
                    if( listBox_ReceiveData.Items.Count > 500 )
                    {
                        listBox_ReceiveData.Items.RemoveAt(0);
                    }
                    listBox_ReceiveData.SelectedIndex = listBox_ReceiveData.Items.Count - 1;

                    //MessageBox.Show(
                    //    System.Text.Encoding.GetEncoding("SHIFT-JIS").GetString(dat));

                    // ログファイル出力
                    if( cb_LogFile.Checked && !string.IsNullOrEmpty(saveLogFname) )
                    {
                        System.IO.StreamWriter sw = new System.IO.StreamWriter( saveLogFname, true, System.Text.Encoding.GetEncoding("shift_jis"));

                        //TextBox1.Textの内容を書き込む
                        sw.Write(readStr + System.Environment.NewLine );

                        //閉じる
                        sw.Close();
                    }

                    // 受信データ解析
                    bsReder.TCP_ReciveCommand(readStr);
                }


                // 自動コマンド送信
                if (cb_AutoAll.Checked)
                {
                    // 全取得コマンド
                    TCP_SendCommand("A0" + "\n");
                }
                else
                {
                    // 個々の取得コマンド
                    if (cb_AutoREncoder.Checked)
                    {
                        TCP_SendCommand("A1" + "\n");
                    }
                    if (cb_AutoCompass.Checked)
                    {
                        TCP_SendCommand("A2" + "\n");
                    }
                    if (cb_AutoGPS.Checked)
                    {
                        TCP_SendCommand("A3" + "\n");
                    }
                    if (cb_AutoREPlot.Checked)
                    {
                        TCP_SendCommand("A4" + "\n");
                    }
                }

            }

            // LRFデータ取得
            // ログファイルは、内部でコマンドごと取得
            if (urgLRF != null && LRF_IPConnectFlg )
            {
                LRF_Data = urgLRF.getScanData();
            }
        }

        /// <summary>
        /// 終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            objTCPSC.Dispose();// = null;// default 192.168.1.1:50001
            objTCPSC_INFO.Dispose();// = null;// default 192.168.1.1:9020

            DisconnectLRF();

            // Mapファイル保存
            if( cb_MapLogFile.Checked )
            {
                autoMapBmp.Save(tb_MapLogFile.Text, System .Drawing.Imaging.ImageFormat.Png);
            }
        }

        public string GetNowTimeStampFileName(string fnameHead, string fnameExt)
        {
            return fnameHead + DateTime.Now.ToString("yyyyMMdd_HHmmss") + fnameExt;
        }

        /// <summary>
        /// セーブファイルダイアログ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LogFileDir_Click(object sender, EventArgs e)
        {
            SaveFileDialog SvDlg = new SaveFileDialog();

            SvDlg.InitialDirectory = Path.GetDirectoryName(tb_LogFile.Text);
            SvDlg.FileName = GetNowTimeStampFileName("tcpLog", ".log");
            SvDlg.Filter = "LogFile(*.log)|*.log|All Files(*.*)|*.*";

            if (DialogResult.OK == SvDlg.ShowDialog())
            {
                tb_LogFile.Text = SvDlg.FileName;
                cb_LogFile.Checked = true;
            }

        }

        private void cb_LogFile_CheckedChanged(object sender, EventArgs e)
        {
            tb_LogFile.Enabled = cb_LogFile.Checked;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerUpdate.Enabled = false;
            timerMapUpdate.Enabled = false;
        }

        private void cb_AutoAll_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_AutoAll.Checked)
            {
                cb_AutoREncoder.Checked = true;
                cb_AutoCompass.Checked = true;
                cb_AutoGPS.Checked = true;
            }
            else
            {
                cb_AutoREncoder.Checked = false;
                cb_AutoCompass.Checked = false;
                cb_AutoGPS.Checked = false;
            }
        }

        private void textBox_sendCMD_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_MapLogFileDir_Click(object sender, EventArgs e)
        {
            SaveFileDialog SvDlg = new SaveFileDialog();

            SvDlg.InitialDirectory = Path.GetDirectoryName(tb_MapLogFile.Text);
            SvDlg.FileName = GetNowTimeStampFileName("AutoMap", ".png");
            SvDlg.Filter = "ImageFile(*.png)|*.png|All Files(*.*)|*.*";

            if (DialogResult.OK == SvDlg.ShowDialog())
            {
                tb_MapLogFile.Text = SvDlg.FileName;
                cb_MapLogFile.Checked = true;
            }
        }

        private void cb_Lrf_CheckedChanged(object sender, EventArgs e)
        {
            // Connecting...
            tb_LrfIP.BackColor = Color.Yellow;
            tb_LrfPort.BackColor = Color.Yellow;

            string saveLrfLogFname = null;

            if (cb_MapLogFile.Checked)
            {
                saveLrfLogFname = tb_MapLogFile.Text;
            }

            if (ConnectLRF(saveLrfLogFname))
            {
                // 接続ＯＫ
                saveLogFname = tb_LogFile.Text;

                tb_LrfIP.BackColor = Color.Lime;
                tb_LrfPort.BackColor = Color.Lime;
            }
            else
            {
                tb_LrfIP.BackColor = Color.Red;
                tb_LrfPort.BackColor = Color.Red;
            }

        }

        private void cb_LogStart_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_LogStart.Checked)
            {
                timerUpdate.Enabled = true;

                if (cb_MapLogFile.Checked)
                {
                    timerMapUpdate.Enabled = true;
                }
            }
            else
            {
                timerUpdate.Enabled = false;
                timerMapUpdate.Enabled = false;
            }
        }




        public double RealToMapSclae = 100;                // マップサイズから メートル変換  1ピクセル 100mm
        public const int AngleRange = 270;     // 認識角 270度
        public const int AngleRangeHalf = (AngleRange / 2);

        /// <summary>
        /// マップ描画タスク
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerMapUpdate_Tick(object sender, EventArgs e)
        {
            // LRF描画
            if (LRF_Data != null)
            {
                Graphics g = Graphics.FromImage(autoMapBmp);

                //double rScale = (1.0 / LocSys.RealToMapSclae);
                double rPI = Math.PI / 180.0;
                int pixelSize = 1;
                double picScale = 1.0 / RealToMapSclae;

                float ctrX = (mapWidthPix / 2.0f) + (float)bsReder.hwREX;
                float ctrY = (mapHeightPix / 2.0f) + (float)bsReder.hwREY;

                // LRFの値を描画
                for (int i = 0; i < LRF_Data.Length; i++)
                {
                    double val = LRF_Data[i] * picScale;// *rScale;
                    double rad = ((i - AngleRangeHalf - 90) * rPI) + (float)bsReder.hwREDir;

                    // LRFは左下から右回り
                    float x = (float)(ctrX + val * Math.Cos(rad));
                    float y = (float)(ctrY + val * Math.Sin(rad));
                    //g.FillRectangle(Brushes.Yellow, x, y, pixelSize, pixelSize);
                    g.DrawLine(Pens.White, ctrX, ctrY, x,y);
                }

                g.Dispose();

                picbox_LRFMap.Invalidate();
            }
            tb_PosX_REPlot.Text = bsReder.hwREX.ToString("f2");
            tb_PosY_REPlot.Text = bsReder.hwREY.ToString("f2");
        }

        /// <summary>
        /// マップ クリア
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            // REリセット
            RE_Reset(0.0,0.0, 0.0);

            Graphics g = Graphics.FromImage(autoMapBmp);
            g.FillRectangle(Brushes.Black, 0, 0, mapWidthPix, mapHeightPix);
            g.Dispose();

            picbox_LRFMap.Invalidate();
        }

        /// <summary>
        /// ロータリーエンコーダにスタート情報をセット
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dir"></param>
        public void RE_Reset(double x, double y, double dir)
        {
            TCP_SendCommand("AD," + ((float)x).ToString("f") + "," + ((float)y).ToString("f") + "\n");

            // 角度をパイ
            double rad = dir * Math.PI / 180.0;
            TCP_SendCommand("AR," + rad.ToString("f") + "\n");
        }

    }
}
