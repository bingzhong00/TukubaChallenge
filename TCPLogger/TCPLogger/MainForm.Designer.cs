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
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.listBox_ReceiveDataINFO = new System.Windows.Forms.ListBox();
            this.textBox_ChangeIPAdressINFO = new System.Windows.Forms.TextBox();
            this.textBox_portINFO = new System.Windows.Forms.TextBox();
            this.button_ConnectINFO = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.cb_AutoREPlot = new System.Windows.Forms.CheckBox();
            this.cb_AutoAll = new System.Windows.Forms.CheckBox();
            this.cb_AutoREncoder = new System.Windows.Forms.CheckBox();
            this.cb_AutoCompass = new System.Windows.Forms.CheckBox();
            this.cb_AutoGPS = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tb_LoginPW = new System.Windows.Forms.TextBox();
            this.tb_LoginID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_LogFile = new System.Windows.Forms.TextBox();
            this.btn_LogFileDir = new System.Windows.Forms.Button();
            this.cb_LogFile = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_MapLogFile = new System.Windows.Forms.CheckBox();
            this.tb_MapLogFile = new System.Windows.Forms.TextBox();
            this.btn_MapLogFileDir = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage_TextLog = new System.Windows.Forms.TabPage();
            this.tabPage_MapLog = new System.Windows.Forms.TabPage();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_PosY_REPlot = new System.Windows.Forms.TextBox();
            this.tb_PosX_REPlot = new System.Windows.Forms.TextBox();
            this.tb_PosY_Gps = new System.Windows.Forms.TextBox();
            this.tb_PosX_Gps = new System.Windows.Forms.TextBox();
            this.rb_REPlot = new System.Windows.Forms.RadioButton();
            this.rb_Gps = new System.Windows.Forms.RadioButton();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.cb_UsbGps = new System.Windows.Forms.CheckBox();
            this.picbox_LRFMap = new System.Windows.Forms.PictureBox();
            this.tb_LrfPort = new System.Windows.Forms.TextBox();
            this.tb_LrfIP = new System.Windows.Forms.TextBox();
            this.cb_Lrf = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cb_LogStart = new System.Windows.Forms.CheckBox();
            this.timerMapUpdate = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage_TextLog.SuspendLayout();
            this.tabPage_MapLog.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_LRFMap)).BeginInit();
            this.SuspendLayout();
            // 
            // button_Connect
            // 
            this.button_Connect.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button_Connect.Location = new System.Drawing.Point(249, 75);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(82, 28);
            this.button_Connect.TabIndex = 0;
            this.button_Connect.Text = "接続";
            this.button_Connect.UseVisualStyleBackColor = true;
            this.button_Connect.Click += new System.EventHandler(this.button_Connect_Click);
            // 
            // textBox_ChangeIPAdress
            // 
            this.textBox_ChangeIPAdress.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox_ChangeIPAdress.Location = new System.Drawing.Point(64, 79);
            this.textBox_ChangeIPAdress.Name = "textBox_ChangeIPAdress";
            this.textBox_ChangeIPAdress.Size = new System.Drawing.Size(104, 23);
            this.textBox_ChangeIPAdress.TabIndex = 1;
            this.textBox_ChangeIPAdress.Text = "192.168.1.1";
            this.textBox_ChangeIPAdress.Leave += new System.EventHandler(this.textBox_ChangeIPAdress_Leave);
            // 
            // listBox_ReceiveData
            // 
            this.listBox_ReceiveData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_ReceiveData.FormattingEnabled = true;
            this.listBox_ReceiveData.ItemHeight = 12;
            this.listBox_ReceiveData.Location = new System.Drawing.Point(3, 192);
            this.listBox_ReceiveData.Name = "listBox_ReceiveData";
            this.listBox_ReceiveData.ScrollAlwaysVisible = true;
            this.listBox_ReceiveData.Size = new System.Drawing.Size(540, 256);
            this.listBox_ReceiveData.TabIndex = 2;
            // 
            // textBox_port
            // 
            this.textBox_port.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox_port.Location = new System.Drawing.Point(175, 79);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(61, 23);
            this.textBox_port.TabIndex = 3;
            this.textBox_port.Text = "50001";
            this.textBox_port.Leave += new System.EventHandler(this.textBox_port_Leave);
            // 
            // textBox_sendCMD
            // 
            this.textBox_sendCMD.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox_sendCMD.Location = new System.Drawing.Point(229, 144);
            this.textBox_sendCMD.Name = "textBox_sendCMD";
            this.textBox_sendCMD.Size = new System.Drawing.Size(229, 26);
            this.textBox_sendCMD.TabIndex = 4;
            this.textBox_sendCMD.Text = "AL,1";
            this.textBox_sendCMD.TextChanged += new System.EventHandler(this.textBox_sendCMD_TextChanged);
            // 
            // button_sendCMD
            // 
            this.button_sendCMD.Enabled = false;
            this.button_sendCMD.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button_sendCMD.Location = new System.Drawing.Point(464, 143);
            this.button_sendCMD.Name = "button_sendCMD";
            this.button_sendCMD.Size = new System.Drawing.Size(75, 29);
            this.button_sendCMD.TabIndex = 5;
            this.button_sendCMD.Text = "送信";
            this.button_sendCMD.UseVisualStyleBackColor = true;
            this.button_sendCMD.Click += new System.EventHandler(this.button_sendCMD_Click);
            // 
            // timerUpdate
            // 
            this.timerUpdate.Interval = 50;
            this.timerUpdate.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // listBox_ReceiveDataINFO
            // 
            this.listBox_ReceiveDataINFO.Enabled = false;
            this.listBox_ReceiveDataINFO.FormattingEnabled = true;
            this.listBox_ReceiveDataINFO.ItemHeight = 12;
            this.listBox_ReceiveDataINFO.Location = new System.Drawing.Point(6, 82);
            this.listBox_ReceiveDataINFO.Name = "listBox_ReceiveDataINFO";
            this.listBox_ReceiveDataINFO.ScrollAlwaysVisible = true;
            this.listBox_ReceiveDataINFO.Size = new System.Drawing.Size(296, 100);
            this.listBox_ReceiveDataINFO.TabIndex = 6;
            // 
            // textBox_ChangeIPAdressINFO
            // 
            this.textBox_ChangeIPAdressINFO.Enabled = false;
            this.textBox_ChangeIPAdressINFO.Location = new System.Drawing.Point(6, 18);
            this.textBox_ChangeIPAdressINFO.Name = "textBox_ChangeIPAdressINFO";
            this.textBox_ChangeIPAdressINFO.Size = new System.Drawing.Size(103, 19);
            this.textBox_ChangeIPAdressINFO.TabIndex = 7;
            this.textBox_ChangeIPAdressINFO.Text = "192.168.1.1";
            // 
            // textBox_portINFO
            // 
            this.textBox_portINFO.Enabled = false;
            this.textBox_portINFO.Location = new System.Drawing.Point(115, 18);
            this.textBox_portINFO.Name = "textBox_portINFO";
            this.textBox_portINFO.Size = new System.Drawing.Size(68, 19);
            this.textBox_portINFO.TabIndex = 8;
            this.textBox_portINFO.Text = "23";
            // 
            // button_ConnectINFO
            // 
            this.button_ConnectINFO.Enabled = false;
            this.button_ConnectINFO.Location = new System.Drawing.Point(221, 43);
            this.button_ConnectINFO.Name = "button_ConnectINFO";
            this.button_ConnectINFO.Size = new System.Drawing.Size(58, 23);
            this.button_ConnectINFO.TabIndex = 9;
            this.button_ConnectINFO.Text = "接続";
            this.button_ConnectINFO.UseVisualStyleBackColor = true;
            this.button_ConnectINFO.Click += new System.EventHandler(this.button_ConnectINFO_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(6, 6);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.listBox_ReceiveData);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_sendCMD);
            this.splitContainer1.Panel1.Controls.Add(this.button_sendCMD);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer1.Size = new System.Drawing.Size(868, 453);
            this.splitContainer1.SplitterDistance = 548;
            this.splitContainer1.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(172, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "手動送信";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Controls.Add(this.cb_AutoREPlot);
            this.groupBox2.Controls.Add(this.cb_AutoAll);
            this.groupBox2.Controls.Add(this.cb_AutoREncoder);
            this.groupBox2.Controls.Add(this.cb_AutoCompass);
            this.groupBox2.Controls.Add(this.cb_AutoGPS);
            this.groupBox2.Location = new System.Drawing.Point(10, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(520, 122);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "BServer 自動送信";
            // 
            // checkBox2
            // 
            this.checkBox2.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox2.Enabled = false;
            this.checkBox2.Location = new System.Drawing.Point(262, 70);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(104, 36);
            this.checkBox2.TabIndex = 22;
            this.checkBox2.Text = "Unknown [A5]";
            this.checkBox2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // cb_AutoREPlot
            // 
            this.cb_AutoREPlot.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_AutoREPlot.Enabled = false;
            this.cb_AutoREPlot.Location = new System.Drawing.Point(149, 70);
            this.cb_AutoREPlot.Name = "cb_AutoREPlot";
            this.cb_AutoREPlot.Size = new System.Drawing.Size(107, 36);
            this.cb_AutoREPlot.TabIndex = 21;
            this.cb_AutoREPlot.Text = "RE PlotX,Y [A4]";
            this.cb_AutoREPlot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_AutoREPlot.UseVisualStyleBackColor = true;
            // 
            // cb_AutoAll
            // 
            this.cb_AutoAll.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_AutoAll.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_AutoAll.Location = new System.Drawing.Point(17, 28);
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
            this.cb_AutoREncoder.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_AutoREncoder.Location = new System.Drawing.Point(149, 28);
            this.cb_AutoREncoder.Name = "cb_AutoREncoder";
            this.cb_AutoREncoder.Size = new System.Drawing.Size(107, 36);
            this.cb_AutoREncoder.TabIndex = 19;
            this.cb_AutoREncoder.Text = "REncoder [A1]";
            this.cb_AutoREncoder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_AutoREncoder.UseVisualStyleBackColor = true;
            // 
            // cb_AutoCompass
            // 
            this.cb_AutoCompass.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_AutoCompass.Location = new System.Drawing.Point(262, 28);
            this.cb_AutoCompass.Name = "cb_AutoCompass";
            this.cb_AutoCompass.Size = new System.Drawing.Size(104, 36);
            this.cb_AutoCompass.TabIndex = 18;
            this.cb_AutoCompass.Text = "Compass [A2]";
            this.cb_AutoCompass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_AutoCompass.UseVisualStyleBackColor = true;
            // 
            // cb_AutoGPS
            // 
            this.cb_AutoGPS.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_AutoGPS.Location = new System.Drawing.Point(372, 28);
            this.cb_AutoGPS.Name = "cb_AutoGPS";
            this.cb_AutoGPS.Size = new System.Drawing.Size(107, 36);
            this.cb_AutoGPS.TabIndex = 17;
            this.cb_AutoGPS.Text = "GPS [A3]";
            this.cb_AutoGPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_AutoGPS.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tb_LoginPW);
            this.groupBox4.Controls.Add(this.listBox_ReceiveDataINFO);
            this.groupBox4.Controls.Add(this.button_ConnectINFO);
            this.groupBox4.Controls.Add(this.tb_LoginID);
            this.groupBox4.Controls.Add(this.textBox_portINFO);
            this.groupBox4.Controls.Add(this.textBox_ChangeIPAdressINFO);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox4.Location = new System.Drawing.Point(0, 259);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(314, 192);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Telnet Log";
            // 
            // tb_LoginPW
            // 
            this.tb_LoginPW.Enabled = false;
            this.tb_LoginPW.Location = new System.Drawing.Point(119, 43);
            this.tb_LoginPW.Name = "tb_LoginPW";
            this.tb_LoginPW.PasswordChar = '*';
            this.tb_LoginPW.Size = new System.Drawing.Size(100, 19);
            this.tb_LoginPW.TabIndex = 12;
            this.tb_LoginPW.Text = "piyokou";
            // 
            // tb_LoginID
            // 
            this.tb_LoginID.Enabled = false;
            this.tb_LoginID.Location = new System.Drawing.Point(6, 43);
            this.tb_LoginID.Name = "tb_LoginID";
            this.tb_LoginID.Size = new System.Drawing.Size(107, 19);
            this.tb_LoginID.TabIndex = 11;
            this.tb_LoginID.Text = "noboru";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "BServer";
            // 
            // tb_LogFile
            // 
            this.tb_LogFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_LogFile.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_LogFile.Location = new System.Drawing.Point(100, 18);
            this.tb_LogFile.Name = "tb_LogFile";
            this.tb_LogFile.Size = new System.Drawing.Size(219, 20);
            this.tb_LogFile.TabIndex = 12;
            // 
            // btn_LogFileDir
            // 
            this.btn_LogFileDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_LogFileDir.Location = new System.Drawing.Point(325, 18);
            this.btn_LogFileDir.Name = "btn_LogFileDir";
            this.btn_LogFileDir.Size = new System.Drawing.Size(35, 23);
            this.btn_LogFileDir.TabIndex = 13;
            this.btn_LogFileDir.Text = "...";
            this.btn_LogFileDir.UseVisualStyleBackColor = true;
            this.btn_LogFileDir.Click += new System.EventHandler(this.btn_LogFileDir_Click);
            // 
            // cb_LogFile
            // 
            this.cb_LogFile.AutoSize = true;
            this.cb_LogFile.Checked = true;
            this.cb_LogFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_LogFile.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_LogFile.Location = new System.Drawing.Point(9, 18);
            this.cb_LogFile.Name = "cb_LogFile";
            this.cb_LogFile.Size = new System.Drawing.Size(85, 20);
            this.cb_LogFile.TabIndex = 15;
            this.cb_LogFile.Text = "SaveLog";
            this.cb_LogFile.UseVisualStyleBackColor = true;
            this.cb_LogFile.CheckedChanged += new System.EventHandler(this.cb_LogFile_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cb_MapLogFile);
            this.groupBox1.Controls.Add(this.tb_MapLogFile);
            this.groupBox1.Controls.Add(this.btn_MapLogFileDir);
            this.groupBox1.Controls.Add(this.cb_LogFile);
            this.groupBox1.Controls.Add(this.tb_LogFile);
            this.groupBox1.Controls.Add(this.btn_LogFileDir);
            this.groupBox1.Location = new System.Drawing.Point(12, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(864, 58);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log出力";
            // 
            // cb_MapLogFile
            // 
            this.cb_MapLogFile.AutoSize = true;
            this.cb_MapLogFile.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_MapLogFile.Location = new System.Drawing.Point(419, 18);
            this.cb_MapLogFile.Name = "cb_MapLogFile";
            this.cb_MapLogFile.Size = new System.Drawing.Size(89, 20);
            this.cb_MapLogFile.TabIndex = 18;
            this.cb_MapLogFile.Text = "SaveMap";
            this.cb_MapLogFile.UseVisualStyleBackColor = true;
            // 
            // tb_MapLogFile
            // 
            this.tb_MapLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_MapLogFile.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_MapLogFile.Location = new System.Drawing.Point(514, 18);
            this.tb_MapLogFile.Name = "tb_MapLogFile";
            this.tb_MapLogFile.Size = new System.Drawing.Size(205, 20);
            this.tb_MapLogFile.TabIndex = 16;
            // 
            // btn_MapLogFileDir
            // 
            this.btn_MapLogFileDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_MapLogFileDir.Location = new System.Drawing.Point(725, 18);
            this.btn_MapLogFileDir.Name = "btn_MapLogFileDir";
            this.btn_MapLogFileDir.Size = new System.Drawing.Size(35, 23);
            this.btn_MapLogFileDir.TabIndex = 17;
            this.btn_MapLogFileDir.Text = "...";
            this.btn_MapLogFileDir.UseVisualStyleBackColor = true;
            this.btn_MapLogFileDir.Click += new System.EventHandler(this.btn_MapLogFileDir_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage_TextLog);
            this.tabControl.Controls.Add(this.tabPage_MapLog);
            this.tabControl.Location = new System.Drawing.Point(0, 147);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(888, 491);
            this.tabControl.TabIndex = 17;
            // 
            // tabPage_TextLog
            // 
            this.tabPage_TextLog.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_TextLog.Controls.Add(this.splitContainer1);
            this.tabPage_TextLog.Location = new System.Drawing.Point(4, 22);
            this.tabPage_TextLog.Name = "tabPage_TextLog";
            this.tabPage_TextLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_TextLog.Size = new System.Drawing.Size(880, 465);
            this.tabPage_TextLog.TabIndex = 0;
            this.tabPage_TextLog.Text = "TextLog";
            // 
            // tabPage_MapLog
            // 
            this.tabPage_MapLog.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_MapLog.Controls.Add(this.btn_Clear);
            this.tabPage_MapLog.Controls.Add(this.groupBox3);
            this.tabPage_MapLog.Controls.Add(this.comboBox1);
            this.tabPage_MapLog.Controls.Add(this.cb_UsbGps);
            this.tabPage_MapLog.Controls.Add(this.picbox_LRFMap);
            this.tabPage_MapLog.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tabPage_MapLog.Location = new System.Drawing.Point(4, 22);
            this.tabPage_MapLog.Name = "tabPage_MapLog";
            this.tabPage_MapLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_MapLog.Size = new System.Drawing.Size(880, 465);
            this.tabPage_MapLog.TabIndex = 1;
            this.tabPage_MapLog.Text = "MapLog";
            // 
            // btn_Clear
            // 
            this.btn_Clear.Location = new System.Drawing.Point(777, 408);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(92, 39);
            this.btn_Clear.TabIndex = 7;
            this.btn_Clear.Text = "Map Reset";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.tb_PosY_REPlot);
            this.groupBox3.Controls.Add(this.tb_PosX_REPlot);
            this.groupBox3.Controls.Add(this.tb_PosY_Gps);
            this.groupBox3.Controls.Add(this.tb_PosX_Gps);
            this.groupBox3.Controls.Add(this.rb_REPlot);
            this.groupBox3.Controls.Add(this.rb_Gps);
            this.groupBox3.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox3.Location = new System.Drawing.Point(568, 80);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(301, 131);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "自己位置";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(197, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 16);
            this.label7.TabIndex = 9;
            this.label7.Text = "Y:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(197, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 16);
            this.label6.TabIndex = 8;
            this.label6.Text = "Y:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(118, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 16);
            this.label5.TabIndex = 7;
            this.label5.Text = "X:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(118, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "X:";
            // 
            // tb_PosY_REPlot
            // 
            this.tb_PosY_REPlot.Location = new System.Drawing.Point(216, 59);
            this.tb_PosY_REPlot.Name = "tb_PosY_REPlot";
            this.tb_PosY_REPlot.ReadOnly = true;
            this.tb_PosY_REPlot.Size = new System.Drawing.Size(52, 23);
            this.tb_PosY_REPlot.TabIndex = 5;
            // 
            // tb_PosX_REPlot
            // 
            this.tb_PosX_REPlot.Location = new System.Drawing.Point(139, 59);
            this.tb_PosX_REPlot.Name = "tb_PosX_REPlot";
            this.tb_PosX_REPlot.ReadOnly = true;
            this.tb_PosX_REPlot.Size = new System.Drawing.Size(52, 23);
            this.tb_PosX_REPlot.TabIndex = 4;
            // 
            // tb_PosY_Gps
            // 
            this.tb_PosY_Gps.Location = new System.Drawing.Point(216, 30);
            this.tb_PosY_Gps.Name = "tb_PosY_Gps";
            this.tb_PosY_Gps.ReadOnly = true;
            this.tb_PosY_Gps.Size = new System.Drawing.Size(52, 23);
            this.tb_PosY_Gps.TabIndex = 3;
            // 
            // tb_PosX_Gps
            // 
            this.tb_PosX_Gps.Location = new System.Drawing.Point(139, 30);
            this.tb_PosX_Gps.Name = "tb_PosX_Gps";
            this.tb_PosX_Gps.ReadOnly = true;
            this.tb_PosX_Gps.Size = new System.Drawing.Size(52, 23);
            this.tb_PosX_Gps.TabIndex = 2;
            // 
            // rb_REPlot
            // 
            this.rb_REPlot.AutoSize = true;
            this.rb_REPlot.Checked = true;
            this.rb_REPlot.Location = new System.Drawing.Point(19, 60);
            this.rb_REPlot.Name = "rb_REPlot";
            this.rb_REPlot.Size = new System.Drawing.Size(83, 20);
            this.rb_REPlot.TabIndex = 1;
            this.rb_REPlot.TabStop = true;
            this.rb_REPlot.Text = "R.E. Plot";
            this.rb_REPlot.UseVisualStyleBackColor = true;
            // 
            // rb_Gps
            // 
            this.rb_Gps.AutoSize = true;
            this.rb_Gps.Location = new System.Drawing.Point(19, 31);
            this.rb_Gps.Name = "rb_Gps";
            this.rb_Gps.Size = new System.Drawing.Size(92, 20);
            this.rb_Gps.TabIndex = 0;
            this.rb_Gps.Text = "USB GPS";
            this.rb_Gps.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(568, 16);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(179, 24);
            this.comboBox1.TabIndex = 2;
            // 
            // cb_UsbGps
            // 
            this.cb_UsbGps.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_UsbGps.Location = new System.Drawing.Point(765, 6);
            this.cb_UsbGps.Name = "cb_UsbGps";
            this.cb_UsbGps.Size = new System.Drawing.Size(104, 43);
            this.cb_UsbGps.TabIndex = 1;
            this.cb_UsbGps.Text = "USB GPS Connect";
            this.cb_UsbGps.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_UsbGps.UseVisualStyleBackColor = true;
            // 
            // picbox_LRFMap
            // 
            this.picbox_LRFMap.BackColor = System.Drawing.SystemColors.Window;
            this.picbox_LRFMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picbox_LRFMap.Location = new System.Drawing.Point(6, 5);
            this.picbox_LRFMap.Name = "picbox_LRFMap";
            this.picbox_LRFMap.Size = new System.Drawing.Size(556, 442);
            this.picbox_LRFMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picbox_LRFMap.TabIndex = 0;
            this.picbox_LRFMap.TabStop = false;
            // 
            // tb_LrfPort
            // 
            this.tb_LrfPort.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_LrfPort.Location = new System.Drawing.Point(175, 112);
            this.tb_LrfPort.Name = "tb_LrfPort";
            this.tb_LrfPort.Size = new System.Drawing.Size(61, 23);
            this.tb_LrfPort.TabIndex = 5;
            this.tb_LrfPort.Text = "10940";
            // 
            // tb_LrfIP
            // 
            this.tb_LrfIP.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_LrfIP.Location = new System.Drawing.Point(64, 112);
            this.tb_LrfIP.Name = "tb_LrfIP";
            this.tb_LrfIP.Size = new System.Drawing.Size(104, 23);
            this.tb_LrfIP.TabIndex = 4;
            this.tb_LrfIP.Text = "192.168.1.10";
            // 
            // cb_Lrf
            // 
            this.cb_Lrf.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_Lrf.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_Lrf.Location = new System.Drawing.Point(249, 109);
            this.cb_Lrf.Name = "cb_Lrf";
            this.cb_Lrf.Size = new System.Drawing.Size(82, 28);
            this.cb_Lrf.TabIndex = 3;
            this.cb_Lrf.Text = "接続";
            this.cb_Lrf.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_Lrf.UseVisualStyleBackColor = true;
            this.cb_Lrf.CheckedChanged += new System.EventHandler(this.cb_Lrf_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 119);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(26, 12);
            this.label8.TabIndex = 18;
            this.label8.Text = "LRF";
            // 
            // cb_LogStart
            // 
            this.cb_LogStart.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_LogStart.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_LogStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cb_LogStart.Location = new System.Drawing.Point(760, 105);
            this.cb_LogStart.Name = "cb_LogStart";
            this.cb_LogStart.Size = new System.Drawing.Size(113, 36);
            this.cb_LogStart.TabIndex = 19;
            this.cb_LogStart.Text = "●Log開始";
            this.cb_LogStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_LogStart.UseVisualStyleBackColor = true;
            this.cb_LogStart.CheckedChanged += new System.EventHandler(this.cb_LogStart_CheckedChanged);
            // 
            // timerMapUpdate
            // 
            this.timerMapUpdate.Tick += new System.EventHandler(this.timerMapUpdate_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 637);
            this.Controls.Add(this.cb_LogStart);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cb_Lrf);
            this.Controls.Add(this.tb_LrfPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_LrfIP);
            this.Controls.Add(this.textBox_ChangeIPAdress);
            this.Controls.Add(this.button_Connect);
            this.Controls.Add(this.textBox_port);
            this.Name = "MainForm";
            this.Text = "TcpLogger Ver0.60";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPage_TextLog.ResumeLayout(false);
            this.tabPage_MapLog.ResumeLayout(false);
            this.tabPage_MapLog.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_LRFMap)).EndInit();
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
        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.ListBox listBox_ReceiveDataINFO;
        private System.Windows.Forms.TextBox textBox_ChangeIPAdressINFO;
        private System.Windows.Forms.TextBox textBox_portINFO;
        private System.Windows.Forms.Button button_ConnectINFO;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox tb_LogFile;
        private System.Windows.Forms.Button btn_LogFileDir;
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
        private System.Windows.Forms.CheckBox cb_AutoREPlot;
        private System.Windows.Forms.CheckBox cb_MapLogFile;
        private System.Windows.Forms.TextBox tb_MapLogFile;
        private System.Windows.Forms.Button btn_MapLogFileDir;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage_TextLog;
        private System.Windows.Forms.TabPage tabPage_MapLog;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox cb_UsbGps;
        private System.Windows.Forms.PictureBox picbox_LRFMap;
        private System.Windows.Forms.CheckBox cb_Lrf;
        private System.Windows.Forms.TextBox tb_LrfPort;
        private System.Windows.Forms.TextBox tb_LrfIP;
        private System.Windows.Forms.Button btn_Clear;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rb_REPlot;
        private System.Windows.Forms.RadioButton rb_Gps;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_PosY_REPlot;
        private System.Windows.Forms.TextBox tb_PosX_REPlot;
        private System.Windows.Forms.TextBox tb_PosY_Gps;
        private System.Windows.Forms.TextBox tb_PosX_Gps;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cb_LogStart;
        private System.Windows.Forms.Timer timerMapUpdate;
    }
}

