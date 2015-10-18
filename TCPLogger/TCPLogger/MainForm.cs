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

namespace TCPLogger
{
    public partial class MainForm : Form
    {
//        TCPServerCheck objTCPSC;
        TCPClient objTCPSC;// default 192.168.1.1:50001
        TCPServerCheck objTCPSC_INFO;// default 192.168.1.1:9020

 
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

            // SavePath
            tb_LogFile.Text = Path.GetDirectoryName(Directory.GetCurrentDirectory()) + "\\" + GetTimeStampFileName();


        
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

            toolStripStatusLabel1.Text = textBox_ChangeIPAdress.Text + " Connecting...";

            check = objTCPSC.Start();
            if (check || true )
            {
                // 接続ＯＫ
                button_sendCMD.Enabled = true;
                saveLogFname = tb_LogFile.Text;

                toolStripStatusLabel1.Text = textBox_ChangeIPAdress.Text + " Connected";

                textBox_port.BackColor = Color.Lime;
                textBox_ChangeIPAdress.BackColor = Color.Lime;

                textBox_sendCMD.Focus();
                timer1.Enabled = true;
            }
            else
            {
                // 接続ＮＧ
                toolStripStatusLabel1.Text = textBox_ChangeIPAdress.Text + " Connect Fail!";

                textBox_port.BackColor = SystemColors.Window;
                textBox_ChangeIPAdress.BackColor = SystemColors.Window;
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

            toolStripStatusLabel1.Text = textBox_ChangeIPAdressINFO.Text + " Connecting...";

            check = objTCPSC_INFO.Start();
            if (check)
            {
                button_sendCMD.Enabled = true;
                timer1.Enabled = true;

                toolStripStatusLabel1.Text = textBox_ChangeIPAdressINFO.Text + " Connected";
            }
            else
            {
                toolStripStatusLabel1.Text = textBox_ChangeIPAdressINFO.Text + " Connect Fail!";
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

        private void timer1_Tick(object sender, EventArgs e)
        {
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
                }

            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            objTCPSC.Dispose();// = null;// default 192.168.1.1:50001
            objTCPSC_INFO.Dispose();// = null;// default 192.168.1.1:9020




        }

        public string GetTimeStampFileName()
        {
            return "tcpLog" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".log";
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
            SvDlg.FileName = GetTimeStampFileName();
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
            timer1.Enabled = false;
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






        //listBox_ReceiveData
    }
}
