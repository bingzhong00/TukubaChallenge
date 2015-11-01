namespace LocSampLogViewer
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
            this.PicBox_Map = new System.Windows.Forms.PictureBox();
            this.PicBox_Sub = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Lbl_Time = new System.Windows.Forms.Label();
            this.btn_Rewind = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.Lbl_Handle = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Lbl_ACC = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Btn_Play = new System.Windows.Forms.CheckBox();
            this.Btn_Rec = new System.Windows.Forms.Button();
            this.Tb_MapName = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Lbl_E1_Dir = new System.Windows.Forms.Label();
            this.Lbl_E1_Y = new System.Windows.Forms.Label();
            this.Lbl_E1_X = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.Lbl_R1_Dir = new System.Windows.Forms.Label();
            this.Lbl_R1_Y = new System.Windows.Forms.Label();
            this.Lbl_REPlotDir = new System.Windows.Forms.Label();
            this.Lbl_REPlotY = new System.Windows.Forms.Label();
            this.Lbl_GPS_Y = new System.Windows.Forms.Label();
            this.Lbl_GPS_X = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.Lbl_RE = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Lbl_R1_X = new System.Windows.Forms.Label();
            this.Lbl_REPlotX = new System.Windows.Forms.Label();
            this.Lbl_Compus = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.Lbl_MapSize = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.Lbl_MapScale = new System.Windows.Forms.Label();
            this.Btn_MapLoadDlg = new System.Windows.Forms.Button();
            this.ConnectTabControl = new System.Windows.Forms.TabControl();
            this.tabPage_LogFile = new System.Windows.Forms.TabPage();
            this.label18 = new System.Windows.Forms.Label();
            this.Num_GPSAngle = new System.Windows.Forms.NumericUpDown();
            this.Btn_GPSDlg = new System.Windows.Forms.Button();
            this.Tb_GPSFileName = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.Btn_LoadTCPLogDlg = new System.Windows.Forms.Button();
            this.Tb_TCPLogFileName = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.Tb_LogFileName = new System.Windows.Forms.TextBox();
            this.Btn_LoadLogDlg = new System.Windows.Forms.Button();
            this.tabPage_Remote = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ScrlBar_Time = new System.Windows.Forms.HScrollBar();
            this.tmr_Update = new System.Windows.Forms.Timer(this.components);
            this.ScrlBar_ViewScale = new System.Windows.Forms.HScrollBar();
            this.Lbl_ViewPosScale = new System.Windows.Forms.Label();
            this.tb_SendUnknown = new System.Windows.Forms.TextBox();
            this.tb_ResiveUnknown = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.Lbl_LED = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox_Map)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox_Sub)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.ConnectTabControl.SuspendLayout();
            this.tabPage_LogFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Num_GPSAngle)).BeginInit();
            this.tabPage_Remote.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // PicBox_Map
            // 
            this.PicBox_Map.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PicBox_Map.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PicBox_Map.Location = new System.Drawing.Point(6, 129);
            this.PicBox_Map.Name = "PicBox_Map";
            this.PicBox_Map.Size = new System.Drawing.Size(516, 376);
            this.PicBox_Map.TabIndex = 0;
            this.PicBox_Map.TabStop = false;
            this.PicBox_Map.SizeChanged += new System.EventHandler(this.PicBox_Map_SizeChanged);
            this.PicBox_Map.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PicBox_Map_MouseDown);
            this.PicBox_Map.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PicBox_Map_MouseMove);
            this.PicBox_Map.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PicBox_Map_MouseUp);
            // 
            // PicBox_Sub
            // 
            this.PicBox_Sub.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PicBox_Sub.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PicBox_Sub.Location = new System.Drawing.Point(529, 129);
            this.PicBox_Sub.Name = "PicBox_Sub";
            this.PicBox_Sub.Size = new System.Drawing.Size(250, 80);
            this.PicBox_Sub.TabIndex = 2;
            this.PicBox_Sub.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 516);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Time:";
            // 
            // Lbl_Time
            // 
            this.Lbl_Time.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Lbl_Time.Location = new System.Drawing.Point(43, 516);
            this.Lbl_Time.Name = "Lbl_Time";
            this.Lbl_Time.Size = new System.Drawing.Size(92, 12);
            this.Lbl_Time.TabIndex = 5;
            this.Lbl_Time.Text = "00:00:00.000";
            // 
            // btn_Rewind
            // 
            this.btn_Rewind.Location = new System.Drawing.Point(11, 18);
            this.btn_Rewind.Name = "btn_Rewind";
            this.btn_Rewind.Size = new System.Drawing.Size(51, 27);
            this.btn_Rewind.TabIndex = 6;
            this.btn_Rewind.Text = "■";
            this.btn_Rewind.UseVisualStyleBackColor = true;
            this.btn_Rewind.Click += new System.EventHandler(this.btn_Rewind_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "Handle:";
            // 
            // Lbl_Handle
            // 
            this.Lbl_Handle.Location = new System.Drawing.Point(57, 21);
            this.Lbl_Handle.Name = "Lbl_Handle";
            this.Lbl_Handle.Size = new System.Drawing.Size(62, 12);
            this.Lbl_Handle.TabIndex = 9;
            this.Lbl_Handle.Text = "0.00";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(125, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "ACC:";
            // 
            // Lbl_ACC
            // 
            this.Lbl_ACC.Location = new System.Drawing.Point(169, 21);
            this.Lbl_ACC.Name = "Lbl_ACC";
            this.Lbl_ACC.Size = new System.Drawing.Size(62, 12);
            this.Lbl_ACC.TabIndex = 11;
            this.Lbl_ACC.Text = "0.00";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "R1 (推定位置):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "RE座標(A4):";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.Lbl_LED);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.tb_SendUnknown);
            this.groupBox1.Controls.Add(this.Lbl_ACC);
            this.groupBox1.Controls.Add(this.Lbl_Handle);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(529, 217);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(251, 82);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "送信情報";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.Btn_Play);
            this.groupBox2.Controls.Add(this.Btn_Rec);
            this.groupBox2.Controls.Add(this.btn_Rewind);
            this.groupBox2.Location = new System.Drawing.Point(529, 500);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(246, 51);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "LogPlayer";
            // 
            // Btn_Play
            // 
            this.Btn_Play.Appearance = System.Windows.Forms.Appearance.Button;
            this.Btn_Play.Location = new System.Drawing.Point(75, 18);
            this.Btn_Play.Name = "Btn_Play";
            this.Btn_Play.Size = new System.Drawing.Size(51, 27);
            this.Btn_Play.TabIndex = 23;
            this.Btn_Play.Text = ">";
            this.Btn_Play.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Btn_Play.UseVisualStyleBackColor = true;
            // 
            // Btn_Rec
            // 
            this.Btn_Rec.Enabled = false;
            this.Btn_Rec.ForeColor = System.Drawing.Color.DarkRed;
            this.Btn_Rec.Location = new System.Drawing.Point(180, 18);
            this.Btn_Rec.Name = "Btn_Rec";
            this.Btn_Rec.Size = new System.Drawing.Size(51, 27);
            this.Btn_Rec.TabIndex = 22;
            this.Btn_Rec.Text = "●";
            this.Btn_Rec.UseVisualStyleBackColor = true;
            // 
            // Tb_MapName
            // 
            this.Tb_MapName.Location = new System.Drawing.Point(6, 22);
            this.Tb_MapName.Name = "Tb_MapName";
            this.Tb_MapName.Size = new System.Drawing.Size(232, 19);
            this.Tb_MapName.TabIndex = 21;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.tb_ResiveUnknown);
            this.groupBox3.Controls.Add(this.Lbl_E1_Dir);
            this.groupBox3.Controls.Add(this.Lbl_E1_Y);
            this.groupBox3.Controls.Add(this.Lbl_E1_X);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.Lbl_R1_Dir);
            this.groupBox3.Controls.Add(this.Lbl_R1_Y);
            this.groupBox3.Controls.Add(this.Lbl_REPlotDir);
            this.groupBox3.Controls.Add(this.Lbl_REPlotY);
            this.groupBox3.Controls.Add(this.Lbl_GPS_Y);
            this.groupBox3.Controls.Add(this.Lbl_GPS_X);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.Lbl_RE);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.Lbl_R1_X);
            this.groupBox3.Controls.Add(this.Lbl_REPlotX);
            this.groupBox3.Controls.Add(this.Lbl_Compus);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(529, 305);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(246, 189);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "受信情報";
            // 
            // Lbl_E1_Dir
            // 
            this.Lbl_E1_Dir.Location = new System.Drawing.Point(195, 139);
            this.Lbl_E1_Dir.Name = "Lbl_E1_Dir";
            this.Lbl_E1_Dir.Size = new System.Drawing.Size(43, 12);
            this.Lbl_E1_Dir.TabIndex = 30;
            this.Lbl_E1_Dir.Text = "ND";
            // 
            // Lbl_E1_Y
            // 
            this.Lbl_E1_Y.Location = new System.Drawing.Point(138, 139);
            this.Lbl_E1_Y.Name = "Lbl_E1_Y";
            this.Lbl_E1_Y.Size = new System.Drawing.Size(43, 12);
            this.Lbl_E1_Y.TabIndex = 29;
            this.Lbl_E1_Y.Text = "ND";
            // 
            // Lbl_E1_X
            // 
            this.Lbl_E1_X.Location = new System.Drawing.Point(89, 139);
            this.Lbl_E1_X.Name = "Lbl_E1_X";
            this.Lbl_E1_X.Size = new System.Drawing.Size(43, 12);
            this.Lbl_E1_X.TabIndex = 28;
            this.Lbl_E1_X.Text = "ND";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(13, 139);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 12);
            this.label13.TabIndex = 27;
            this.label13.Text = "E1 (REPlot):";
            // 
            // Lbl_R1_Dir
            // 
            this.Lbl_R1_Dir.Location = new System.Drawing.Point(195, 118);
            this.Lbl_R1_Dir.Name = "Lbl_R1_Dir";
            this.Lbl_R1_Dir.Size = new System.Drawing.Size(43, 10);
            this.Lbl_R1_Dir.TabIndex = 26;
            this.Lbl_R1_Dir.Text = "ND";
            // 
            // Lbl_R1_Y
            // 
            this.Lbl_R1_Y.Location = new System.Drawing.Point(138, 118);
            this.Lbl_R1_Y.Name = "Lbl_R1_Y";
            this.Lbl_R1_Y.Size = new System.Drawing.Size(43, 12);
            this.Lbl_R1_Y.TabIndex = 25;
            this.Lbl_R1_Y.Text = "ND";
            // 
            // Lbl_REPlotDir
            // 
            this.Lbl_REPlotDir.Location = new System.Drawing.Point(195, 96);
            this.Lbl_REPlotDir.Name = "Lbl_REPlotDir";
            this.Lbl_REPlotDir.Size = new System.Drawing.Size(43, 12);
            this.Lbl_REPlotDir.TabIndex = 24;
            this.Lbl_REPlotDir.Text = "ND";
            // 
            // Lbl_REPlotY
            // 
            this.Lbl_REPlotY.Location = new System.Drawing.Point(138, 96);
            this.Lbl_REPlotY.Name = "Lbl_REPlotY";
            this.Lbl_REPlotY.Size = new System.Drawing.Size(43, 12);
            this.Lbl_REPlotY.TabIndex = 23;
            this.Lbl_REPlotY.Text = "ND";
            // 
            // Lbl_GPS_Y
            // 
            this.Lbl_GPS_Y.Location = new System.Drawing.Point(138, 72);
            this.Lbl_GPS_Y.Name = "Lbl_GPS_Y";
            this.Lbl_GPS_Y.Size = new System.Drawing.Size(43, 12);
            this.Lbl_GPS_Y.TabIndex = 22;
            this.Lbl_GPS_Y.Text = "ND";
            // 
            // Lbl_GPS_X
            // 
            this.Lbl_GPS_X.Location = new System.Drawing.Point(89, 72);
            this.Lbl_GPS_X.Name = "Lbl_GPS_X";
            this.Lbl_GPS_X.Size = new System.Drawing.Size(43, 12);
            this.Lbl_GPS_X.TabIndex = 21;
            this.Lbl_GPS_X.Text = "ND";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(29, 72);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 12);
            this.label12.TabIndex = 20;
            this.label12.Text = "GPS(A3):";
            // 
            // Lbl_RE
            // 
            this.Lbl_RE.Location = new System.Drawing.Point(89, 24);
            this.Lbl_RE.Name = "Lbl_RE";
            this.Lbl_RE.Size = new System.Drawing.Size(66, 12);
            this.Lbl_RE.TabIndex = 19;
            this.Lbl_RE.Text = "ND";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(32, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 12);
            this.label10.TabIndex = 18;
            this.label10.Text = "R.E.(A1):";
            // 
            // Lbl_R1_X
            // 
            this.Lbl_R1_X.Location = new System.Drawing.Point(89, 118);
            this.Lbl_R1_X.Name = "Lbl_R1_X";
            this.Lbl_R1_X.Size = new System.Drawing.Size(43, 12);
            this.Lbl_R1_X.TabIndex = 17;
            this.Lbl_R1_X.Text = "ND";
            // 
            // Lbl_REPlotX
            // 
            this.Lbl_REPlotX.Location = new System.Drawing.Point(89, 96);
            this.Lbl_REPlotX.Name = "Lbl_REPlotX";
            this.Lbl_REPlotX.Size = new System.Drawing.Size(43, 12);
            this.Lbl_REPlotX.TabIndex = 16;
            this.Lbl_REPlotX.Text = "ND";
            // 
            // Lbl_Compus
            // 
            this.Lbl_Compus.Location = new System.Drawing.Point(89, 48);
            this.Lbl_Compus.Name = "Lbl_Compus";
            this.Lbl_Compus.Size = new System.Drawing.Size(66, 12);
            this.Lbl_Compus.TabIndex = 15;
            this.Lbl_Compus.Text = "ND";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 48);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 14;
            this.label11.Text = "コンパス(A2):";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 50);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(49, 12);
            this.label15.TabIndex = 23;
            this.label15.Text = "MapSize:";
            // 
            // Lbl_MapSize
            // 
            this.Lbl_MapSize.AutoSize = true;
            this.Lbl_MapSize.Location = new System.Drawing.Point(64, 50);
            this.Lbl_MapSize.Name = "Lbl_MapSize";
            this.Lbl_MapSize.Size = new System.Drawing.Size(43, 12);
            this.Lbl_MapSize.TabIndex = 24;
            this.Lbl_MapSize.Text = "xxx,xxx";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(9, 74);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(82, 12);
            this.label17.TabIndex = 25;
            this.label17.Text = "MapScale[mm]:";
            // 
            // Lbl_MapScale
            // 
            this.Lbl_MapScale.AutoSize = true;
            this.Lbl_MapScale.Location = new System.Drawing.Point(97, 74);
            this.Lbl_MapScale.Name = "Lbl_MapScale";
            this.Lbl_MapScale.Size = new System.Drawing.Size(35, 12);
            this.Lbl_MapScale.TabIndex = 26;
            this.Lbl_MapScale.Text = "1/100";
            // 
            // Btn_MapLoadDlg
            // 
            this.Btn_MapLoadDlg.Location = new System.Drawing.Point(187, 47);
            this.Btn_MapLoadDlg.Name = "Btn_MapLoadDlg";
            this.Btn_MapLoadDlg.Size = new System.Drawing.Size(51, 24);
            this.Btn_MapLoadDlg.TabIndex = 27;
            this.Btn_MapLoadDlg.Text = "...";
            this.Btn_MapLoadDlg.UseVisualStyleBackColor = true;
            this.Btn_MapLoadDlg.Click += new System.EventHandler(this.Btn_MapLoadDlg_Click);
            // 
            // ConnectTabControl
            // 
            this.ConnectTabControl.Controls.Add(this.tabPage_LogFile);
            this.ConnectTabControl.Controls.Add(this.tabPage_Remote);
            this.ConnectTabControl.Location = new System.Drawing.Point(3, 4);
            this.ConnectTabControl.Multiline = true;
            this.ConnectTabControl.Name = "ConnectTabControl";
            this.ConnectTabControl.SelectedIndex = 0;
            this.ConnectTabControl.Size = new System.Drawing.Size(520, 117);
            this.ConnectTabControl.TabIndex = 28;
            // 
            // tabPage_LogFile
            // 
            this.tabPage_LogFile.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_LogFile.Controls.Add(this.label18);
            this.tabPage_LogFile.Controls.Add(this.Num_GPSAngle);
            this.tabPage_LogFile.Controls.Add(this.Btn_GPSDlg);
            this.tabPage_LogFile.Controls.Add(this.Tb_GPSFileName);
            this.tabPage_LogFile.Controls.Add(this.label16);
            this.tabPage_LogFile.Controls.Add(this.Btn_LoadTCPLogDlg);
            this.tabPage_LogFile.Controls.Add(this.Tb_TCPLogFileName);
            this.tabPage_LogFile.Controls.Add(this.label14);
            this.tabPage_LogFile.Controls.Add(this.label9);
            this.tabPage_LogFile.Controls.Add(this.Tb_LogFileName);
            this.tabPage_LogFile.Controls.Add(this.Btn_LoadLogDlg);
            this.tabPage_LogFile.Location = new System.Drawing.Point(4, 22);
            this.tabPage_LogFile.Name = "tabPage_LogFile";
            this.tabPage_LogFile.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_LogFile.Size = new System.Drawing.Size(512, 91);
            this.tabPage_LogFile.TabIndex = 1;
            this.tabPage_LogFile.Text = "LogFile";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(409, 65);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(29, 12);
            this.label18.TabIndex = 10;
            this.label18.Text = "角度";
            // 
            // Num_GPSAngle
            // 
            this.Num_GPSAngle.Location = new System.Drawing.Point(444, 63);
            this.Num_GPSAngle.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.Num_GPSAngle.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.Num_GPSAngle.Name = "Num_GPSAngle";
            this.Num_GPSAngle.Size = new System.Drawing.Size(62, 19);
            this.Num_GPSAngle.TabIndex = 9;
            // 
            // Btn_GPSDlg
            // 
            this.Btn_GPSDlg.Location = new System.Drawing.Point(348, 60);
            this.Btn_GPSDlg.Name = "Btn_GPSDlg";
            this.Btn_GPSDlg.Size = new System.Drawing.Size(43, 22);
            this.Btn_GPSDlg.TabIndex = 8;
            this.Btn_GPSDlg.Text = "...";
            this.Btn_GPSDlg.UseVisualStyleBackColor = true;
            this.Btn_GPSDlg.Click += new System.EventHandler(this.Btn_GPSDlg_Click);
            // 
            // Tb_GPSFileName
            // 
            this.Tb_GPSFileName.Location = new System.Drawing.Point(91, 62);
            this.Tb_GPSFileName.Name = "Tb_GPSFileName";
            this.Tb_GPSFileName.Size = new System.Drawing.Size(251, 19);
            this.Tb_GPSFileName.TabIndex = 7;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(8, 65);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(61, 12);
            this.label16.TabIndex = 6;
            this.label16.Text = "GPSLogger";
            // 
            // Btn_LoadTCPLogDlg
            // 
            this.Btn_LoadTCPLogDlg.Location = new System.Drawing.Point(463, 8);
            this.Btn_LoadTCPLogDlg.Name = "Btn_LoadTCPLogDlg";
            this.Btn_LoadTCPLogDlg.Size = new System.Drawing.Size(43, 22);
            this.Btn_LoadTCPLogDlg.TabIndex = 5;
            this.Btn_LoadTCPLogDlg.Text = "...";
            this.Btn_LoadTCPLogDlg.UseVisualStyleBackColor = true;
            this.Btn_LoadTCPLogDlg.Click += new System.EventHandler(this.Btn_LoadTCPLogDlg_Click);
            // 
            // Tb_TCPLogFileName
            // 
            this.Tb_TCPLogFileName.Location = new System.Drawing.Point(91, 10);
            this.Tb_TCPLogFileName.Name = "Tb_TCPLogFileName";
            this.Tb_TCPLogFileName.Size = new System.Drawing.Size(366, 19);
            this.Tb_TCPLogFileName.TabIndex = 4;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(8, 13);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(61, 12);
            this.label14.TabIndex = 3;
            this.label14.Text = "TCPLogger";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 39);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 12);
            this.label9.TabIndex = 2;
            this.label9.Text = "VehicleRunner";
            // 
            // Tb_LogFileName
            // 
            this.Tb_LogFileName.Location = new System.Drawing.Point(91, 36);
            this.Tb_LogFileName.Name = "Tb_LogFileName";
            this.Tb_LogFileName.Size = new System.Drawing.Size(366, 19);
            this.Tb_LogFileName.TabIndex = 1;
            // 
            // Btn_LoadLogDlg
            // 
            this.Btn_LoadLogDlg.Location = new System.Drawing.Point(463, 34);
            this.Btn_LoadLogDlg.Name = "Btn_LoadLogDlg";
            this.Btn_LoadLogDlg.Size = new System.Drawing.Size(43, 22);
            this.Btn_LoadLogDlg.TabIndex = 0;
            this.Btn_LoadLogDlg.Text = "...";
            this.Btn_LoadLogDlg.UseVisualStyleBackColor = true;
            this.Btn_LoadLogDlg.Click += new System.EventHandler(this.Btn_LoadLogDlg_Click);
            // 
            // tabPage_Remote
            // 
            this.tabPage_Remote.BackColor = System.Drawing.Color.LightCyan;
            this.tabPage_Remote.Controls.Add(this.label8);
            this.tabPage_Remote.Controls.Add(this.button5);
            this.tabPage_Remote.Controls.Add(this.textBox1);
            this.tabPage_Remote.Controls.Add(this.comboBox1);
            this.tabPage_Remote.Controls.Add(this.label7);
            this.tabPage_Remote.Controls.Add(this.label6);
            this.tabPage_Remote.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Remote.Name = "tabPage_Remote";
            this.tabPage_Remote.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Remote.Size = new System.Drawing.Size(512, 91);
            this.tabPage_Remote.TabIndex = 0;
            this.tabPage_Remote.Text = "RemoteView";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(352, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 12);
            this.label8.TabIndex = 5;
            this.label8.Text = "NoConnect";
            // 
            // button5
            // 
            this.button5.Enabled = false;
            this.button5.Location = new System.Drawing.Point(427, 9);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 28);
            this.button5.TabIndex = 4;
            this.button5.Text = "Connect";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(285, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.PasswordChar = '*';
            this.textBox1.Size = new System.Drawing.Size(61, 19);
            this.textBox1.TabIndex = 3;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(72, 9);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(158, 20);
            this.comboBox1.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(249, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "Pass";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "PID(Name)";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.Tb_MapName);
            this.groupBox4.Controls.Add(this.Btn_MapLoadDlg);
            this.groupBox4.Controls.Add(this.Lbl_MapScale);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.Lbl_MapSize);
            this.groupBox4.Location = new System.Drawing.Point(529, 9);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(244, 108);
            this.groupBox4.TabIndex = 29;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Map Info";
            // 
            // ScrlBar_Time
            // 
            this.ScrlBar_Time.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScrlBar_Time.Location = new System.Drawing.Point(7, 535);
            this.ScrlBar_Time.Name = "ScrlBar_Time";
            this.ScrlBar_Time.Size = new System.Drawing.Size(515, 17);
            this.ScrlBar_Time.TabIndex = 30;
            this.ScrlBar_Time.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ScrlBar_Time_Scroll);
            this.ScrlBar_Time.ValueChanged += new System.EventHandler(this.ScrlBar_Time_ValueChanged);
            // 
            // tmr_Update
            // 
            this.tmr_Update.Tick += new System.EventHandler(this.tmr_Update_Tick);
            // 
            // ScrlBar_ViewScale
            // 
            this.ScrlBar_ViewScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ScrlBar_ViewScale.Location = new System.Drawing.Point(302, 511);
            this.ScrlBar_ViewScale.Maximum = 300;
            this.ScrlBar_ViewScale.Minimum = 10;
            this.ScrlBar_ViewScale.Name = "ScrlBar_ViewScale";
            this.ScrlBar_ViewScale.Size = new System.Drawing.Size(217, 17);
            this.ScrlBar_ViewScale.TabIndex = 31;
            this.ScrlBar_ViewScale.Value = 100;
            this.ScrlBar_ViewScale.ValueChanged += new System.EventHandler(this.ScrlBar_ViewScale_ValueChanged);
            // 
            // Lbl_ViewPosScale
            // 
            this.Lbl_ViewPosScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_ViewPosScale.AutoSize = true;
            this.Lbl_ViewPosScale.Location = new System.Drawing.Point(150, 516);
            this.Lbl_ViewPosScale.Name = "Lbl_ViewPosScale";
            this.Lbl_ViewPosScale.Size = new System.Drawing.Size(32, 12);
            this.Lbl_ViewPosScale.TabIndex = 32;
            this.Lbl_ViewPosScale.Text = "View:";
            // 
            // tb_SendUnknown
            // 
            this.tb_SendUnknown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_SendUnknown.Location = new System.Drawing.Point(7, 57);
            this.tb_SendUnknown.Name = "tb_SendUnknown";
            this.tb_SendUnknown.Size = new System.Drawing.Size(236, 19);
            this.tb_SendUnknown.TabIndex = 12;
            // 
            // tb_ResiveUnknown
            // 
            this.tb_ResiveUnknown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_ResiveUnknown.Location = new System.Drawing.Point(2, 164);
            this.tb_ResiveUnknown.Name = "tb_ResiveUnknown";
            this.tb_ResiveUnknown.Size = new System.Drawing.Size(236, 19);
            this.tb_ResiveUnknown.TabIndex = 31;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(12, 40);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(28, 12);
            this.label19.TabIndex = 13;
            this.label19.Text = "LED:";
            // 
            // Lbl_LED
            // 
            this.Lbl_LED.Location = new System.Drawing.Point(57, 40);
            this.Lbl_LED.Name = "Lbl_LED";
            this.Lbl_LED.Size = new System.Drawing.Size(34, 12);
            this.Lbl_LED.TabIndex = 14;
            this.Lbl_LED.Text = "0.00";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.Lbl_ViewPosScale);
            this.Controls.Add(this.ScrlBar_ViewScale);
            this.Controls.Add(this.ScrlBar_Time);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.ConnectTabControl);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.Lbl_Time);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.PicBox_Sub);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PicBox_Map);
            this.Name = "MainForm";
            this.Text = "LogViewer Ver0.30";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PicBox_Map)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox_Sub)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ConnectTabControl.ResumeLayout(false);
            this.tabPage_LogFile.ResumeLayout(false);
            this.tabPage_LogFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Num_GPSAngle)).EndInit();
            this.tabPage_Remote.ResumeLayout(false);
            this.tabPage_Remote.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PicBox_Map;
        private System.Windows.Forms.PictureBox PicBox_Sub;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Lbl_Time;
        private System.Windows.Forms.Button btn_Rewind;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label Lbl_Handle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label Lbl_ACC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button Btn_Rec;
        private System.Windows.Forms.TextBox Tb_MapName;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label Lbl_R1_X;
        private System.Windows.Forms.Label Lbl_REPlotX;
        private System.Windows.Forms.Label Lbl_Compus;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label Lbl_MapSize;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label Lbl_MapScale;
        private System.Windows.Forms.Button Btn_MapLoadDlg;
        private System.Windows.Forms.TabControl ConnectTabControl;
        private System.Windows.Forms.TabPage tabPage_Remote;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPage_LogFile;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox Tb_LogFileName;
        private System.Windows.Forms.Button Btn_LoadLogDlg;
        private System.Windows.Forms.Label Lbl_GPS_X;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label Lbl_RE;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.HScrollBar ScrlBar_Time;
        private System.Windows.Forms.Label Lbl_GPS_Y;
        private System.Windows.Forms.Label Lbl_REPlotDir;
        private System.Windows.Forms.Label Lbl_REPlotY;
        private System.Windows.Forms.Label Lbl_R1_Dir;
        private System.Windows.Forms.Label Lbl_R1_Y;
        private System.Windows.Forms.Timer tmr_Update;
        private System.Windows.Forms.CheckBox Btn_Play;
        private System.Windows.Forms.Label Lbl_E1_Dir;
        private System.Windows.Forms.Label Lbl_E1_Y;
        private System.Windows.Forms.Label Lbl_E1_X;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.HScrollBar ScrlBar_ViewScale;
        private System.Windows.Forms.Label Lbl_ViewPosScale;
        private System.Windows.Forms.Button Btn_LoadTCPLogDlg;
        private System.Windows.Forms.TextBox Tb_TCPLogFileName;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button Btn_GPSDlg;
        private System.Windows.Forms.TextBox Tb_GPSFileName;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.NumericUpDown Num_GPSAngle;
        private System.Windows.Forms.TextBox tb_SendUnknown;
        private System.Windows.Forms.TextBox tb_ResiveUnknown;
        private System.Windows.Forms.Label Lbl_LED;
        private System.Windows.Forms.Label label19;
    }
}

