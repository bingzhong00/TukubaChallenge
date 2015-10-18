namespace TCPLogger
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button_Connect = new System.Windows.Forms.Button();
            this.textBox_ChangeIPAdress = new System.Windows.Forms.TextBox();
            this.listBox_ReceiveData = new System.Windows.Forms.ListBox();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.textBox_sendCMD = new System.Windows.Forms.TextBox();
            this.button_sendCMD = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.listBox_ReceiveDataINFO = new System.Windows.Forms.ListBox();
            this.textBox_ChangeIPAdressINFO = new System.Windows.Forms.TextBox();
            this.textBox_portINFO = new System.Windows.Forms.TextBox();
            this.button_ConnectINFO = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tb_LogFile = new System.Windows.Forms.TextBox();
            this.btn_LogFileDir = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.cb_LogFile = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_AutoGPS = new System.Windows.Forms.CheckBox();
            this.cb_AutoCompass = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_AutoAll = new System.Windows.Forms.CheckBox();
            this.cb_AutoREncoder = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_LoginID = new System.Windows.Forms.TextBox();
            this.tb_LoginPW = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Connect
            // 
            this.button_Connect.Location = new System.Drawing.Point(266, 3);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(57, 28);
            this.button_Connect.TabIndex = 0;
            this.button_Connect.Text = "接続";
            this.button_Connect.UseVisualStyleBackColor = true;
            this.button_Connect.Click += new System.EventHandler(this.button_Connect_Click);
            // 
            // textBox_ChangeIPAdress
            // 
            this.textBox_ChangeIPAdress.Location = new System.Drawing.Point(77, 7);
            this.textBox_ChangeIPAdress.Name = "textBox_ChangeIPAdress";
            this.textBox_ChangeIPAdress.Size = new System.Drawing.Size(92, 19);
            this.textBox_ChangeIPAdress.TabIndex = 1;
            this.textBox_ChangeIPAdress.Text = "192.168.1.1";
            this.textBox_ChangeIPAdress.Leave += new System.EventHandler(this.textBox_ChangeIPAdress_Leave);
            // 
            // listBox_ReceiveData
            // 
            this.listBox_ReceiveData.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBox_ReceiveData.FormattingEnabled = true;
            this.listBox_ReceiveData.ItemHeight = 12;
            this.listBox_ReceiveData.Location = new System.Drawing.Point(0, 220);
            this.listBox_ReceiveData.Name = "listBox_ReceiveData";
            this.listBox_ReceiveData.ScrollAlwaysVisible = true;
            this.listBox_ReceiveData.Size = new System.Drawing.Size(544, 136);
            this.listBox_ReceiveData.TabIndex = 2;
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(175, 8);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(68, 19);
            this.textBox_port.TabIndex = 3;
            this.textBox_port.Text = "50001";
            this.textBox_port.Leave += new System.EventHandler(this.textBox_port_Leave);
            // 
            // textBox_sendCMD
            // 
            this.textBox_sendCMD.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox_sendCMD.Location = new System.Drawing.Point(76, 41);
            this.textBox_sendCMD.Name = "textBox_sendCMD";
            this.textBox_sendCMD.Size = new System.Drawing.Size(257, 26);
            this.textBox_sendCMD.TabIndex = 4;
            this.textBox_sendCMD.Text = "AC,0.5,0.5";
            this.textBox_sendCMD.TextChanged += new System.EventHandler(this.textBox_sendCMD_TextChanged);
            // 
            // button_sendCMD
            // 
            this.button_sendCMD.Enabled = false;
            this.button_sendCMD.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button_sendCMD.Location = new System.Drawing.Point(339, 41);
            this.button_sendCMD.Name = "button_sendCMD";
            this.button_sendCMD.Size = new System.Drawing.Size(75, 29);
            this.button_sendCMD.TabIndex = 5;
            this.button_sendCMD.Text = "送信";
            this.button_sendCMD.UseVisualStyleBackColor = true;
            this.button_sendCMD.Click += new System.EventHandler(this.button_sendCMD_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // listBox_ReceiveDataINFO
            // 
            this.listBox_ReceiveDataINFO.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBox_ReceiveDataINFO.Enabled = false;
            this.listBox_ReceiveDataINFO.FormattingEnabled = true;
            this.listBox_ReceiveDataINFO.ItemHeight = 12;
            this.listBox_ReceiveDataINFO.Location = new System.Drawing.Point(0, 88);
            this.listBox_ReceiveDataINFO.Name = "listBox_ReceiveDataINFO";
            this.listBox_ReceiveDataINFO.ScrollAlwaysVisible = true;
            this.listBox_ReceiveDataINFO.Size = new System.Drawing.Size(312, 268);
            this.listBox_ReceiveDataINFO.TabIndex = 6;
            // 
            // textBox_ChangeIPAdressINFO
            // 
            this.textBox_ChangeIPAdressINFO.Enabled = false;
            this.textBox_ChangeIPAdressINFO.Location = new System.Drawing.Point(5, 22);
            this.textBox_ChangeIPAdressINFO.Name = "textBox_ChangeIPAdressINFO";
            this.textBox_ChangeIPAdressINFO.Size = new System.Drawing.Size(107, 19);
            this.textBox_ChangeIPAdressINFO.TabIndex = 7;
            this.textBox_ChangeIPAdressINFO.Text = "192.168.1.1";
            // 
            // textBox_portINFO
            // 
            this.textBox_portINFO.Enabled = false;
            this.textBox_portINFO.Location = new System.Drawing.Point(118, 22);
            this.textBox_portINFO.Name = "textBox_portINFO";
            this.textBox_portINFO.Size = new System.Drawing.Size(68, 19);
            this.textBox_portINFO.TabIndex = 8;
            this.textBox_portINFO.Text = "23";
            // 
            // button_ConnectINFO
            // 
            this.button_ConnectINFO.Enabled = false;
            this.button_ConnectINFO.Location = new System.Drawing.Point(245, 49);
            this.button_ConnectINFO.Name = "button_ConnectINFO";
            this.button_ConnectINFO.Size = new System.Drawing.Size(66, 23);
            this.button_ConnectINFO.TabIndex = 9;
            this.button_ConnectINFO.Text = "接続";
            this.button_ConnectINFO.UseVisualStyleBackColor = true;
            this.button_ConnectINFO.Click += new System.EventHandler(this.button_ConnectINFO_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "Telnet Log";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(12, 81);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.listBox_ReceiveData);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_sendCMD);
            this.splitContainer1.Panel1.Controls.Add(this.button_sendCMD);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_ChangeIPAdress);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_port);
            this.splitContainer1.Panel1.Controls.Add(this.button_Connect);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tb_LoginPW);
            this.splitContainer1.Panel2.Controls.Add(this.tb_LoginID);
            this.splitContainer1.Panel2.Controls.Add(this.listBox_ReceiveDataINFO);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.textBox_ChangeIPAdressINFO);
            this.splitContainer1.Panel2.Controls.Add(this.textBox_portINFO);
            this.splitContainer1.Panel2.Controls.Add(this.button_ConnectINFO);
            this.splitContainer1.Size = new System.Drawing.Size(864, 358);
            this.splitContainer1.SplitterDistance = 546;
            this.splitContainer1.TabIndex = 11;
            // 
            // tb_LogFile
            // 
            this.tb_LogFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_LogFile.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_LogFile.Location = new System.Drawing.Point(98, 19);
            this.tb_LogFile.Name = "tb_LogFile";
            this.tb_LogFile.Size = new System.Drawing.Size(701, 20);
            this.tb_LogFile.TabIndex = 12;
            // 
            // btn_LogFileDir
            // 
            this.btn_LogFileDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_LogFileDir.Location = new System.Drawing.Point(812, 16);
            this.btn_LogFileDir.Name = "btn_LogFileDir";
            this.btn_LogFileDir.Size = new System.Drawing.Size(35, 23);
            this.btn_LogFileDir.TabIndex = 13;
            this.btn_LogFileDir.Text = "...";
            this.btn_LogFileDir.UseVisualStyleBackColor = true;
            this.btn_LogFileDir.Click += new System.EventHandler(this.btn_LogFileDir_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip.Location = new System.Drawing.Point(0, 442);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(888, 23);
            this.statusStrip.TabIndex = 14;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(46, 18);
            this.toolStripStatusLabel1.Text = "Status";
            // 
            // cb_LogFile
            // 
            this.cb_LogFile.AutoSize = true;
            this.cb_LogFile.Checked = true;
            this.cb_LogFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_LogFile.Location = new System.Drawing.Point(14, 21);
            this.cb_LogFile.Name = "cb_LogFile";
            this.cb_LogFile.Size = new System.Drawing.Size(67, 16);
            this.cb_LogFile.TabIndex = 15;
            this.cb_LogFile.Text = "SaveLog";
            this.cb_LogFile.UseVisualStyleBackColor = true;
            this.cb_LogFile.CheckedChanged += new System.EventHandler(this.cb_LogFile_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cb_LogFile);
            this.groupBox1.Controls.Add(this.tb_LogFile);
            this.groupBox1.Controls.Add(this.btn_LogFileDir);
            this.groupBox1.Location = new System.Drawing.Point(12, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(864, 47);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log出力";
            // 
            // cb_AutoGPS
            // 
            this.cb_AutoGPS.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_AutoGPS.Location = new System.Drawing.Point(372, 18);
            this.cb_AutoGPS.Name = "cb_AutoGPS";
            this.cb_AutoGPS.Size = new System.Drawing.Size(107, 36);
            this.cb_AutoGPS.TabIndex = 17;
            this.cb_AutoGPS.Text = "GPS [A3]";
            this.cb_AutoGPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_AutoGPS.UseVisualStyleBackColor = true;
            // 
            // cb_AutoCompass
            // 
            this.cb_AutoCompass.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_AutoCompass.Location = new System.Drawing.Point(262, 18);
            this.cb_AutoCompass.Name = "cb_AutoCompass";
            this.cb_AutoCompass.Size = new System.Drawing.Size(104, 36);
            this.cb_AutoCompass.TabIndex = 18;
            this.cb_AutoCompass.Text = "Compass [A2]";
            this.cb_AutoCompass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_AutoCompass.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.cb_AutoAll);
            this.groupBox2.Controls.Add(this.cb_AutoREncoder);
            this.groupBox2.Controls.Add(this.cb_AutoCompass);
            this.groupBox2.Controls.Add(this.cb_AutoGPS);
            this.groupBox2.Location = new System.Drawing.Point(4, 88);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(526, 122);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "連続送信（自動取得）";
            // 
            // cb_AutoAll
            // 
            this.cb_AutoAll.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_AutoAll.Location = new System.Drawing.Point(10, 18);
            this.cb_AutoAll.Name = "cb_AutoAll";
            this.cb_AutoAll.Size = new System.Drawing.Size(107, 36);
            this.cb_AutoAll.TabIndex = 20;
            this.cb_AutoAll.Text = "ALL [A0]";
            this.cb_AutoAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_AutoAll.UseVisualStyleBackColor = true;
            this.cb_AutoAll.CheckedChanged += new System.EventHandler(this.cb_AutoAll_CheckedChanged);
            // 
            // cb_AutoREncoder
            // 
            this.cb_AutoREncoder.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_AutoREncoder.Location = new System.Drawing.Point(149, 18);
            this.cb_AutoREncoder.Name = "cb_AutoREncoder";
            this.cb_AutoREncoder.Size = new System.Drawing.Size(107, 36);
            this.cb_AutoREncoder.TabIndex = 19;
            this.cb_AutoREncoder.Text = "REncoder [A1]";
            this.cb_AutoREncoder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_AutoREncoder.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "BServer Log";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "単発送信";
            // 
            // tb_LoginID
            // 
            this.tb_LoginID.Enabled = false;
            this.tb_LoginID.Location = new System.Drawing.Point(5, 50);
            this.tb_LoginID.Name = "tb_LoginID";
            this.tb_LoginID.Size = new System.Drawing.Size(107, 19);
            this.tb_LoginID.TabIndex = 11;
            this.tb_LoginID.Text = "noboru";
            // 
            // tb_LoginPW
            // 
            this.tb_LoginPW.Enabled = false;
            this.tb_LoginPW.Location = new System.Drawing.Point(118, 51);
            this.tb_LoginPW.Name = "tb_LoginPW";
            this.tb_LoginPW.PasswordChar = '*';
            this.tb_LoginPW.Size = new System.Drawing.Size(100, 19);
            this.tb_LoginPW.TabIndex = 12;
            this.tb_LoginPW.Text = "piyokou";
            // 
            // checkBox1
            // 
            this.checkBox1.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(149, 60);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(107, 36);
            this.checkBox1.TabIndex = 21;
            this.checkBox1.Text = "RE PlotX,Y [A4]";
            this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox2.Enabled = false;
            this.checkBox2.Location = new System.Drawing.Point(262, 60);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(107, 36);
            this.checkBox2.TabIndex = 22;
            this.checkBox2.Text = "Unknown [A5]";
            this.checkBox2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 465);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Text = "Cersio CommandLogger Ver0.50";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Connect;
        private System.Windows.Forms.TextBox textBox_ChangeIPAdress;
        private System.Windows.Forms.ListBox listBox_ReceiveData;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.TextBox textBox_sendCMD;
        private System.Windows.Forms.Button button_sendCMD;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ListBox listBox_ReceiveDataINFO;
        private System.Windows.Forms.TextBox textBox_ChangeIPAdressINFO;
        private System.Windows.Forms.TextBox textBox_portINFO;
        private System.Windows.Forms.Button button_ConnectINFO;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox tb_LogFile;
        private System.Windows.Forms.Button btn_LogFileDir;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.CheckBox cb_LogFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cb_AutoGPS;
        private System.Windows.Forms.CheckBox cb_AutoCompass;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cb_AutoREncoder;
        private System.Windows.Forms.CheckBox cb_AutoAll;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_LoginPW;
        private System.Windows.Forms.TextBox tb_LoginID;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

