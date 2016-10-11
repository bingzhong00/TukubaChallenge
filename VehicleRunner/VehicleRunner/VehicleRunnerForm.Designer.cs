namespace VehicleRunner
{
    partial class VehicleRunnerForm {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.picbox_AreaMap = new System.Windows.Forms.PictureBox();
            this.picbox_LRF = new System.Windows.Forms.PictureBox();
            this.cb_LRFConnect = new System.Windows.Forms.CheckBox();
            this.lb_LRFResult = new System.Windows.Forms.Label();
            this.cb_StartAutonomous = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_LRFScale = new System.Windows.Forms.TextBox();
            this.btn_VRReset = new System.Windows.Forms.Button();
            this.tb_LRFIpAddr = new System.Windows.Forms.TextBox();
            this.tb_LRFPort = new System.Windows.Forms.TextBox();
            this.cb_EmgBrake = new System.Windows.Forms.CheckBox();
            this.tm_UpdateHw = new System.Windows.Forms.Timer(this.components);
            this.tb_ResiveData = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_SendData = new System.Windows.Forms.TextBox();
            this.tm_LocUpdate = new System.Windows.Forms.Timer(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.tb_AccelVal = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_HandleVal = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_RESpeed = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage_BoxPC = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lbl_RErotL = new System.Windows.Forms.Label();
            this.lbl_RErotR = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lbl_REPlotDir = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lbl_REPlotY = new System.Windows.Forms.Label();
            this.lbl_REPlotX = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lbl_LED = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbl_GPS_Y = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lbl_GPS_X = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbl_Compass = new System.Windows.Forms.Label();
            this.tabPage_Sensor = new System.Windows.Forms.TabPage();
            this.lbl_bServerEmu = new System.Windows.Forms.Label();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.rb_DirAMCL = new System.Windows.Forms.RadioButton();
            this.rb_DirCompus = new System.Windows.Forms.RadioButton();
            this.rb_DirSVO = new System.Windows.Forms.RadioButton();
            this.rb_DirGPS = new System.Windows.Forms.RadioButton();
            this.rb_DirREPlot = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb_MovePF = new System.Windows.Forms.RadioButton();
            this.rb_MoveREandCompus = new System.Windows.Forms.RadioButton();
            this.rb_MoveAMCL = new System.Windows.Forms.RadioButton();
            this.rb_MoveSVO = new System.Windows.Forms.RadioButton();
            this.rb_MoveGPS = new System.Windows.Forms.RadioButton();
            this.rb_MoveREPlot = new System.Windows.Forms.RadioButton();
            this.tabPage_LocSump = new System.Windows.Forms.TabPage();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_ResetR1 = new System.Windows.Forms.Button();
            this.num_R1Dir = new System.Windows.Forms.NumericUpDown();
            this.num_R1Y = new System.Windows.Forms.NumericUpDown();
            this.btn_GetCompustoR1 = new System.Windows.Forms.Button();
            this.num_R1X = new System.Windows.Forms.NumericUpDown();
            this.label22 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.btn_GetGPStoR1 = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cb_VRRevision = new System.Windows.Forms.CheckBox();
            this.gbox_Revision = new System.Windows.Forms.GroupBox();
            this.cb_DontAlwaysRivision = new System.Windows.Forms.CheckBox();
            this.rb_UseGPS_Revision = new System.Windows.Forms.RadioButton();
            this.btn_LocRevision = new System.Windows.Forms.Button();
            this.rb_UsePF_Revision = new System.Windows.Forms.RadioButton();
            this.cb_AlwaysPFCalc = new System.Windows.Forms.CheckBox();
            this.tabPage_Emulate = new System.Windows.Forms.TabPage();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.trackBar_LRFViewScale = new System.Windows.Forms.TrackBar();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.tbCaribrationRER = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.tbCaribrationREL = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.btnCribration = new System.Windows.Forms.Button();
            this.cb_InDoorMode = new System.Windows.Forms.CheckBox();
            this.cb_StraghtMode = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cmbbox_UsbSH2Connect = new System.Windows.Forms.ComboBox();
            this.cb_UsbSH2Connect = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_SirialConnect = new System.Windows.Forms.CheckBox();
            this.cb_UsbSirial = new System.Windows.Forms.ComboBox();
            this.tb_SirialResive = new System.Windows.Forms.TextBox();
            this.cb_StartLogMapping = new System.Windows.Forms.CheckBox();
            this.lb_BServerConnect = new System.Windows.Forms.Label();
            this.cb_EHS = new System.Windows.Forms.CheckBox();
            this.lbl_BackCnt = new System.Windows.Forms.Label();
            this.gb_DriveControl = new System.Windows.Forms.GroupBox();
            this.lbl_BackProcess = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.picbox_Indicator = new System.Windows.Forms.PictureBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.rb_LRF_LAN = new System.Windows.Forms.RadioButton();
            this.rb_LRF_ROSnode = new System.Windows.Forms.RadioButton();
            this.cb_ConnectBServerEmu = new System.Windows.Forms.CheckBox();
            this.cb_ConnectRosIF = new System.Windows.Forms.CheckBox();
            this.btn_MapLoad = new System.Windows.Forms.Button();
            this.tb_MapName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_AreaMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_LRF)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPage_BoxPC.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage_Sensor.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage_LocSump.SuspendLayout();
            this.groupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_R1Dir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_R1Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_R1X)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.gbox_Revision.SuspendLayout();
            this.tabPage_Emulate.SuspendLayout();
            this.groupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_LRFViewScale)).BeginInit();
            this.groupBox7.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gb_DriveControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_Indicator)).BeginInit();
            this.groupBox9.SuspendLayout();
            this.SuspendLayout();
            // 
            // picbox_AreaMap
            // 
            this.picbox_AreaMap.BackColor = System.Drawing.Color.White;
            this.picbox_AreaMap.Location = new System.Drawing.Point(2, 4);
            this.picbox_AreaMap.Name = "picbox_AreaMap";
            this.picbox_AreaMap.Size = new System.Drawing.Size(600, 600);
            this.picbox_AreaMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picbox_AreaMap.TabIndex = 0;
            this.picbox_AreaMap.TabStop = false;
            this.picbox_AreaMap.Paint += new System.Windows.Forms.PaintEventHandler(this.picbox_AreaMap_Paint);
            this.picbox_AreaMap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picbox_AreaMap_MouseClick);
            // 
            // picbox_LRF
            // 
            this.picbox_LRF.BackColor = System.Drawing.Color.White;
            this.picbox_LRF.Location = new System.Drawing.Point(607, 4);
            this.picbox_LRF.Name = "picbox_LRF";
            this.picbox_LRF.Size = new System.Drawing.Size(240, 240);
            this.picbox_LRF.TabIndex = 1;
            this.picbox_LRF.TabStop = false;
            this.picbox_LRF.Click += new System.EventHandler(this.picbox_LRF_Click);
            this.picbox_LRF.Paint += new System.Windows.Forms.PaintEventHandler(this.picbox_LRF_Paint);
            // 
            // cb_LRFConnect
            // 
            this.cb_LRFConnect.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_LRFConnect.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_LRFConnect.Location = new System.Drawing.Point(171, 72);
            this.cb_LRFConnect.Name = "cb_LRFConnect";
            this.cb_LRFConnect.Size = new System.Drawing.Size(104, 33);
            this.cb_LRFConnect.TabIndex = 3;
            this.cb_LRFConnect.Text = "URG接続";
            this.cb_LRFConnect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_LRFConnect.UseVisualStyleBackColor = true;
            this.cb_LRFConnect.CheckedChanged += new System.EventHandler(this.cb_LRFConnect_CheckedChanged);
            // 
            // lb_LRFResult
            // 
            this.lb_LRFResult.AutoSize = true;
            this.lb_LRFResult.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb_LRFResult.Location = new System.Drawing.Point(124, 24);
            this.lb_LRFResult.Name = "lb_LRFResult";
            this.lb_LRFResult.Size = new System.Drawing.Size(72, 16);
            this.lb_LRFResult.TabIndex = 4;
            this.lb_LRFResult.Text = "Connect";
            // 
            // cb_StartAutonomous
            // 
            this.cb_StartAutonomous.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_StartAutonomous.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_StartAutonomous.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_StartAutonomous.Location = new System.Drawing.Point(607, 551);
            this.cb_StartAutonomous.Name = "cb_StartAutonomous";
            this.cb_StartAutonomous.Size = new System.Drawing.Size(239, 53);
            this.cb_StartAutonomous.TabIndex = 5;
            this.cb_StartAutonomous.Text = "自律走行\r\nStart";
            this.cb_StartAutonomous.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_StartAutonomous.UseVisualStyleBackColor = true;
            this.cb_StartAutonomous.CheckedChanged += new System.EventHandler(this.cb_Autonomous_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(243, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "mm";
            // 
            // tb_LRFScale
            // 
            this.tb_LRFScale.Location = new System.Drawing.Point(185, 16);
            this.tb_LRFScale.Name = "tb_LRFScale";
            this.tb_LRFScale.Size = new System.Drawing.Size(54, 19);
            this.tb_LRFScale.TabIndex = 10;
            this.tb_LRFScale.Text = "1000";
            this.tb_LRFScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tb_LRFScale.TextChanged += new System.EventHandler(this.tb_LRFScale_TextChanged);
            // 
            // btn_VRReset
            // 
            this.btn_VRReset.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_VRReset.Location = new System.Drawing.Point(730, 445);
            this.btn_VRReset.Name = "btn_VRReset";
            this.btn_VRReset.Size = new System.Drawing.Size(117, 38);
            this.btn_VRReset.TabIndex = 12;
            this.btn_VRReset.Text = "リセット";
            this.btn_VRReset.UseVisualStyleBackColor = true;
            this.btn_VRReset.Click += new System.EventHandler(this.btn_PositionReset_Click);
            // 
            // tb_LRFIpAddr
            // 
            this.tb_LRFIpAddr.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_LRFIpAddr.Location = new System.Drawing.Point(9, 80);
            this.tb_LRFIpAddr.Name = "tb_LRFIpAddr";
            this.tb_LRFIpAddr.Size = new System.Drawing.Size(104, 20);
            this.tb_LRFIpAddr.TabIndex = 15;
            this.tb_LRFIpAddr.Text = "192.168.1.10";
            // 
            // tb_LRFPort
            // 
            this.tb_LRFPort.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_LRFPort.Location = new System.Drawing.Point(119, 80);
            this.tb_LRFPort.Name = "tb_LRFPort";
            this.tb_LRFPort.Size = new System.Drawing.Size(45, 20);
            this.tb_LRFPort.TabIndex = 16;
            this.tb_LRFPort.Text = "10940";
            // 
            // cb_EmgBrake
            // 
            this.cb_EmgBrake.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_EmgBrake.Checked = true;
            this.cb_EmgBrake.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_EmgBrake.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_EmgBrake.Location = new System.Drawing.Point(719, 15);
            this.cb_EmgBrake.Name = "cb_EmgBrake";
            this.cb_EmgBrake.Size = new System.Drawing.Size(120, 38);
            this.cb_EmgBrake.TabIndex = 19;
            this.cb_EmgBrake.Text = "緊急停止";
            this.cb_EmgBrake.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_EmgBrake.UseVisualStyleBackColor = true;
            this.cb_EmgBrake.CheckedChanged += new System.EventHandler(this.cb_Color_CheckedChanged);
            // 
            // tm_UpdateHw
            // 
            this.tm_UpdateHw.Interval = 50;
            this.tm_UpdateHw.Tick += new System.EventHandler(this.tm_UpdateHw_Tick);
            // 
            // tb_ResiveData
            // 
            this.tb_ResiveData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_ResiveData.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_ResiveData.Location = new System.Drawing.Point(9, 314);
            this.tb_ResiveData.Name = "tb_ResiveData";
            this.tb_ResiveData.ReadOnly = true;
            this.tb_ResiveData.Size = new System.Drawing.Size(261, 19);
            this.tb_ResiveData.TabIndex = 24;
            this.tb_ResiveData.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(7, 299);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 12);
            this.label3.TabIndex = 25;
            this.label3.Text = "BServer受信内容";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(7, 262);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 12);
            this.label4.TabIndex = 26;
            this.label4.Text = "BServer送信内容";
            // 
            // tb_SendData
            // 
            this.tb_SendData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_SendData.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_SendData.Location = new System.Drawing.Point(9, 277);
            this.tb_SendData.Name = "tb_SendData";
            this.tb_SendData.ReadOnly = true;
            this.tb_SendData.Size = new System.Drawing.Size(261, 19);
            this.tb_SendData.TabIndex = 27;
            this.tb_SendData.TabStop = false;
            // 
            // tm_LocUpdate
            // 
            this.tm_LocUpdate.Tick += new System.EventHandler(this.tm_LocUpdate_Tick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(6, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 27);
            this.label5.TabIndex = 28;
            this.label5.Text = "ACC";
            // 
            // tb_AccelVal
            // 
            this.tb_AccelVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_AccelVal.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_AccelVal.Location = new System.Drawing.Point(74, 66);
            this.tb_AccelVal.Name = "tb_AccelVal";
            this.tb_AccelVal.ReadOnly = true;
            this.tb_AccelVal.Size = new System.Drawing.Size(102, 34);
            this.tb_AccelVal.TabIndex = 29;
            this.tb_AccelVal.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(6, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 27);
            this.label6.TabIndex = 30;
            this.label6.Text = "HDL";
            // 
            // tb_HandleVal
            // 
            this.tb_HandleVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_HandleVal.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_HandleVal.Location = new System.Drawing.Point(74, 25);
            this.tb_HandleVal.Name = "tb_HandleVal";
            this.tb_HandleVal.ReadOnly = true;
            this.tb_HandleVal.Size = new System.Drawing.Size(102, 34);
            this.tb_HandleVal.TabIndex = 31;
            this.tb_HandleVal.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(183, 77);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 16);
            this.label7.TabIndex = 33;
            this.label7.Text = "SPD";
            // 
            // tb_RESpeed
            // 
            this.tb_RESpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_RESpeed.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_RESpeed.Location = new System.Drawing.Point(230, 72);
            this.tb_RESpeed.Name = "tb_RESpeed";
            this.tb_RESpeed.ReadOnly = true;
            this.tb_RESpeed.Size = new System.Drawing.Size(73, 23);
            this.tb_RESpeed.TabIndex = 34;
            this.tb_RESpeed.TabStop = false;
            this.tb_RESpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(311, 81);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 35;
            this.label8.Text = "mm/sec";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage_BoxPC);
            this.tabControl.Controls.Add(this.tabPage_Sensor);
            this.tabControl.Controls.Add(this.tabPage_LocSump);
            this.tabControl.Controls.Add(this.tabPage_Emulate);
            this.tabControl.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tabControl.Location = new System.Drawing.Point(853, 271);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(299, 446);
            this.tabControl.TabIndex = 36;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabPage_BoxPC
            // 
            this.tabPage_BoxPC.Controls.Add(this.groupBox8);
            this.tabPage_BoxPC.Location = new System.Drawing.Point(4, 26);
            this.tabPage_BoxPC.Name = "tabPage_BoxPC";
            this.tabPage_BoxPC.Size = new System.Drawing.Size(291, 416);
            this.tabPage_BoxPC.TabIndex = 4;
            this.tabPage_BoxPC.Text = "bServer";
            this.tabPage_BoxPC.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.groupBox4);
            this.groupBox8.Controls.Add(this.label13);
            this.groupBox8.Controls.Add(this.lbl_LED);
            this.groupBox8.Controls.Add(this.groupBox3);
            this.groupBox8.Controls.Add(this.label10);
            this.groupBox8.Controls.Add(this.lbl_Compass);
            this.groupBox8.Location = new System.Drawing.Point(3, 3);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(278, 405);
            this.groupBox8.TabIndex = 42;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "BoxPC ステータス";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lbl_RErotL);
            this.groupBox4.Controls.Add(this.lbl_RErotR);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.lbl_REPlotDir);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.lbl_REPlotY);
            this.groupBox4.Controls.Add(this.lbl_REPlotX);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox4.Location = new System.Drawing.Point(3, 46);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(231, 130);
            this.groupBox4.TabIndex = 29;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "RotalyEncoder";
            // 
            // lbl_RErotL
            // 
            this.lbl_RErotL.AutoSize = true;
            this.lbl_RErotL.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_RErotL.Location = new System.Drawing.Point(32, 26);
            this.lbl_RErotL.Name = "lbl_RErotL";
            this.lbl_RErotL.Size = new System.Drawing.Size(28, 16);
            this.lbl_RErotL.TabIndex = 17;
            this.lbl_RErotL.Text = "ND";
            // 
            // lbl_RErotR
            // 
            this.lbl_RErotR.AutoSize = true;
            this.lbl_RErotR.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_RErotR.Location = new System.Drawing.Point(159, 26);
            this.lbl_RErotR.Name = "lbl_RErotR";
            this.lbl_RErotR.Size = new System.Drawing.Size(28, 16);
            this.lbl_RErotR.TabIndex = 16;
            this.lbl_RErotR.Text = "ND";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label17.Location = new System.Drawing.Point(6, 26);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(20, 16);
            this.label17.TabIndex = 15;
            this.label17.Text = "L:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label16.Location = new System.Drawing.Point(132, 26);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(21, 16);
            this.label16.TabIndex = 14;
            this.label16.Text = "R:";
            // 
            // lbl_REPlotDir
            // 
            this.lbl_REPlotDir.AutoSize = true;
            this.lbl_REPlotDir.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_REPlotDir.Location = new System.Drawing.Point(72, 105);
            this.lbl_REPlotDir.Name = "lbl_REPlotDir";
            this.lbl_REPlotDir.Size = new System.Drawing.Size(28, 16);
            this.lbl_REPlotDir.TabIndex = 13;
            this.lbl_REPlotDir.Text = "ND";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label18.Location = new System.Drawing.Point(6, 105);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(55, 16);
            this.label18.TabIndex = 12;
            this.label18.Text = "plotDir:";
            // 
            // lbl_REPlotY
            // 
            this.lbl_REPlotY.AutoSize = true;
            this.lbl_REPlotY.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_REPlotY.Location = new System.Drawing.Point(63, 81);
            this.lbl_REPlotY.Name = "lbl_REPlotY";
            this.lbl_REPlotY.Size = new System.Drawing.Size(28, 16);
            this.lbl_REPlotY.TabIndex = 11;
            this.lbl_REPlotY.Text = "ND";
            // 
            // lbl_REPlotX
            // 
            this.lbl_REPlotX.AutoSize = true;
            this.lbl_REPlotX.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_REPlotX.Location = new System.Drawing.Point(63, 56);
            this.lbl_REPlotX.Name = "lbl_REPlotX";
            this.lbl_REPlotX.Size = new System.Drawing.Size(28, 16);
            this.lbl_REPlotX.TabIndex = 10;
            this.lbl_REPlotX.Text = "ND";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label15.Location = new System.Drawing.Point(6, 81);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(45, 16);
            this.label15.TabIndex = 9;
            this.label15.Text = "plotY:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label14.Location = new System.Drawing.Point(6, 56);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(46, 16);
            this.label14.TabIndex = 8;
            this.label14.Text = "plotX:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label13.Location = new System.Drawing.Point(4, 184);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 16);
            this.label13.TabIndex = 6;
            this.label13.Text = "地磁気:";
            // 
            // lbl_LED
            // 
            this.lbl_LED.AutoSize = true;
            this.lbl_LED.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_LED.Location = new System.Drawing.Point(203, 22);
            this.lbl_LED.Name = "lbl_LED";
            this.lbl_LED.Size = new System.Drawing.Size(28, 16);
            this.lbl_LED.TabIndex = 5;
            this.lbl_LED.Text = "ND";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lbl_GPS_Y);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.lbl_GPS_X);
            this.groupBox3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox3.Location = new System.Drawing.Point(3, 224);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(231, 73);
            this.groupBox3.TabIndex = 28;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "GPS  (USB接続時はUSB-GSP)";
            // 
            // lbl_GPS_Y
            // 
            this.lbl_GPS_Y.AutoSize = true;
            this.lbl_GPS_Y.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_GPS_Y.Location = new System.Drawing.Point(72, 48);
            this.lbl_GPS_Y.Name = "lbl_GPS_Y";
            this.lbl_GPS_Y.Size = new System.Drawing.Size(28, 16);
            this.lbl_GPS_Y.TabIndex = 3;
            this.lbl_GPS_Y.Text = "ND";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label12.Location = new System.Drawing.Point(6, 48);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 16);
            this.label12.TabIndex = 2;
            this.label12.Text = "Y(緯度):";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label11.Location = new System.Drawing.Point(6, 21);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 16);
            this.label11.TabIndex = 1;
            this.label11.Text = "X(経度):";
            // 
            // lbl_GPS_X
            // 
            this.lbl_GPS_X.AutoSize = true;
            this.lbl_GPS_X.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_GPS_X.Location = new System.Drawing.Point(72, 21);
            this.lbl_GPS_X.Name = "lbl_GPS_X";
            this.lbl_GPS_X.Size = new System.Drawing.Size(28, 16);
            this.lbl_GPS_X.TabIndex = 4;
            this.lbl_GPS_X.Text = "ND";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.Location = new System.Drawing.Point(158, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 16);
            this.label10.TabIndex = 0;
            this.label10.Text = "LED:";
            // 
            // lbl_Compass
            // 
            this.lbl_Compass.AutoSize = true;
            this.lbl_Compass.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_Compass.Location = new System.Drawing.Point(75, 184);
            this.lbl_Compass.Name = "lbl_Compass";
            this.lbl_Compass.Size = new System.Drawing.Size(28, 16);
            this.lbl_Compass.TabIndex = 7;
            this.lbl_Compass.Text = "ND";
            // 
            // tabPage_Sensor
            // 
            this.tabPage_Sensor.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_Sensor.Controls.Add(this.lbl_bServerEmu);
            this.tabPage_Sensor.Controls.Add(this.groupBox10);
            this.tabPage_Sensor.Controls.Add(this.groupBox1);
            this.tabPage_Sensor.Controls.Add(this.tb_SendData);
            this.tabPage_Sensor.Controls.Add(this.label4);
            this.tabPage_Sensor.Controls.Add(this.label3);
            this.tabPage_Sensor.Controls.Add(this.tb_ResiveData);
            this.tabPage_Sensor.Location = new System.Drawing.Point(4, 26);
            this.tabPage_Sensor.Name = "tabPage_Sensor";
            this.tabPage_Sensor.Size = new System.Drawing.Size(291, 416);
            this.tabPage_Sensor.TabIndex = 2;
            this.tabPage_Sensor.Text = "センサー入力";
            // 
            // lbl_bServerEmu
            // 
            this.lbl_bServerEmu.AutoSize = true;
            this.lbl_bServerEmu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lbl_bServerEmu.Location = new System.Drawing.Point(9, 217);
            this.lbl_bServerEmu.Name = "lbl_bServerEmu";
            this.lbl_bServerEmu.Size = new System.Drawing.Size(190, 16);
            this.lbl_bServerEmu.TabIndex = 30;
            this.lbl_bServerEmu.Text = "bServerエミュレーションモード";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.rb_DirAMCL);
            this.groupBox10.Controls.Add(this.rb_DirCompus);
            this.groupBox10.Controls.Add(this.rb_DirSVO);
            this.groupBox10.Controls.Add(this.rb_DirGPS);
            this.groupBox10.Controls.Add(this.rb_DirREPlot);
            this.groupBox10.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox10.Location = new System.Drawing.Point(9, 111);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(261, 95);
            this.groupBox10.TabIndex = 29;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "向き 入力ソース";
            // 
            // rb_DirAMCL
            // 
            this.rb_DirAMCL.AutoSize = true;
            this.rb_DirAMCL.Location = new System.Drawing.Point(107, 40);
            this.rb_DirAMCL.Name = "rb_DirAMCL";
            this.rb_DirAMCL.Size = new System.Drawing.Size(56, 16);
            this.rb_DirAMCL.TabIndex = 4;
            this.rb_DirAMCL.Text = "A.MCL";
            this.rb_DirAMCL.UseVisualStyleBackColor = true;
            // 
            // rb_DirCompus
            // 
            this.rb_DirCompus.AutoSize = true;
            this.rb_DirCompus.Location = new System.Drawing.Point(14, 62);
            this.rb_DirCompus.Name = "rb_DirCompus";
            this.rb_DirCompus.Size = new System.Drawing.Size(59, 16);
            this.rb_DirCompus.TabIndex = 3;
            this.rb_DirCompus.Text = "地磁気";
            this.rb_DirCompus.UseVisualStyleBackColor = true;
            this.rb_DirCompus.CheckedChanged += new System.EventHandler(this.rb_Dir_CheckedChanged);
            // 
            // rb_DirSVO
            // 
            this.rb_DirSVO.AutoSize = true;
            this.rb_DirSVO.Location = new System.Drawing.Point(107, 18);
            this.rb_DirSVO.Name = "rb_DirSVO";
            this.rb_DirSVO.Size = new System.Drawing.Size(121, 16);
            this.rb_DirSVO.TabIndex = 2;
            this.rb_DirSVO.Text = "S. Visual Odometry";
            this.rb_DirSVO.UseVisualStyleBackColor = true;
            this.rb_DirSVO.CheckedChanged += new System.EventHandler(this.rb_Dir_CheckedChanged);
            // 
            // rb_DirGPS
            // 
            this.rb_DirGPS.AutoSize = true;
            this.rb_DirGPS.Location = new System.Drawing.Point(14, 40);
            this.rb_DirGPS.Name = "rb_DirGPS";
            this.rb_DirGPS.Size = new System.Drawing.Size(45, 16);
            this.rb_DirGPS.TabIndex = 1;
            this.rb_DirGPS.Text = "GPS";
            this.rb_DirGPS.UseVisualStyleBackColor = true;
            this.rb_DirGPS.CheckedChanged += new System.EventHandler(this.rb_Dir_CheckedChanged);
            // 
            // rb_DirREPlot
            // 
            this.rb_DirREPlot.AutoSize = true;
            this.rb_DirREPlot.Checked = true;
            this.rb_DirREPlot.Location = new System.Drawing.Point(14, 18);
            this.rb_DirREPlot.Name = "rb_DirREPlot";
            this.rb_DirREPlot.Size = new System.Drawing.Size(62, 16);
            this.rb_DirREPlot.TabIndex = 0;
            this.rb_DirREPlot.TabStop = true;
            this.rb_DirREPlot.Text = "R.E.Plot";
            this.rb_DirREPlot.UseVisualStyleBackColor = true;
            this.rb_DirREPlot.CheckedChanged += new System.EventHandler(this.rb_Dir_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb_MovePF);
            this.groupBox1.Controls.Add(this.rb_MoveREandCompus);
            this.groupBox1.Controls.Add(this.rb_MoveAMCL);
            this.groupBox1.Controls.Add(this.rb_MoveSVO);
            this.groupBox1.Controls.Add(this.rb_MoveGPS);
            this.groupBox1.Controls.Add(this.rb_MoveREPlot);
            this.groupBox1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox1.Location = new System.Drawing.Point(9, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(261, 94);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "X,Y座標 入力ソース";
            // 
            // rb_MovePF
            // 
            this.rb_MovePF.AutoSize = true;
            this.rb_MovePF.Enabled = false;
            this.rb_MovePF.Location = new System.Drawing.Point(119, 71);
            this.rb_MovePF.Name = "rb_MovePF";
            this.rb_MovePF.Size = new System.Drawing.Size(103, 16);
            this.rb_MovePF.TabIndex = 5;
            this.rb_MovePF.TabStop = true;
            this.rb_MovePF.Text = "VR PrticleFilter";
            this.rb_MovePF.UseVisualStyleBackColor = true;
            // 
            // rb_MoveREandCompus
            // 
            this.rb_MoveREandCompus.AutoSize = true;
            this.rb_MoveREandCompus.Location = new System.Drawing.Point(13, 71);
            this.rb_MoveREandCompus.Name = "rb_MoveREandCompus";
            this.rb_MoveREandCompus.Size = new System.Drawing.Size(93, 16);
            this.rb_MoveREandCompus.TabIndex = 4;
            this.rb_MoveREandCompus.TabStop = true;
            this.rb_MoveREandCompus.Text = "R.E. && 地磁気";
            this.rb_MoveREandCompus.UseVisualStyleBackColor = true;
            // 
            // rb_MoveAMCL
            // 
            this.rb_MoveAMCL.AutoSize = true;
            this.rb_MoveAMCL.Location = new System.Drawing.Point(119, 49);
            this.rb_MoveAMCL.Name = "rb_MoveAMCL";
            this.rb_MoveAMCL.Size = new System.Drawing.Size(56, 16);
            this.rb_MoveAMCL.TabIndex = 3;
            this.rb_MoveAMCL.TabStop = true;
            this.rb_MoveAMCL.Text = "A.MCL";
            this.rb_MoveAMCL.UseVisualStyleBackColor = true;
            // 
            // rb_MoveSVO
            // 
            this.rb_MoveSVO.AutoSize = true;
            this.rb_MoveSVO.Location = new System.Drawing.Point(119, 27);
            this.rb_MoveSVO.Name = "rb_MoveSVO";
            this.rb_MoveSVO.Size = new System.Drawing.Size(121, 16);
            this.rb_MoveSVO.TabIndex = 2;
            this.rb_MoveSVO.TabStop = true;
            this.rb_MoveSVO.Text = "S. Visual Odometry";
            this.rb_MoveSVO.UseVisualStyleBackColor = true;
            this.rb_MoveSVO.CheckedChanged += new System.EventHandler(this.rb_Move_CheckedChanged);
            // 
            // rb_MoveGPS
            // 
            this.rb_MoveGPS.AutoSize = true;
            this.rb_MoveGPS.Location = new System.Drawing.Point(13, 49);
            this.rb_MoveGPS.Name = "rb_MoveGPS";
            this.rb_MoveGPS.Size = new System.Drawing.Size(45, 16);
            this.rb_MoveGPS.TabIndex = 1;
            this.rb_MoveGPS.TabStop = true;
            this.rb_MoveGPS.Text = "GPS";
            this.rb_MoveGPS.UseVisualStyleBackColor = true;
            this.rb_MoveGPS.CheckedChanged += new System.EventHandler(this.rb_Move_CheckedChanged);
            // 
            // rb_MoveREPlot
            // 
            this.rb_MoveREPlot.AutoSize = true;
            this.rb_MoveREPlot.Checked = true;
            this.rb_MoveREPlot.Location = new System.Drawing.Point(13, 27);
            this.rb_MoveREPlot.Name = "rb_MoveREPlot";
            this.rb_MoveREPlot.Size = new System.Drawing.Size(62, 16);
            this.rb_MoveREPlot.TabIndex = 0;
            this.rb_MoveREPlot.TabStop = true;
            this.rb_MoveREPlot.Text = "R.E.Plot";
            this.rb_MoveREPlot.UseVisualStyleBackColor = true;
            this.rb_MoveREPlot.CheckedChanged += new System.EventHandler(this.rb_Move_CheckedChanged);
            // 
            // tabPage_LocSump
            // 
            this.tabPage_LocSump.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_LocSump.Controls.Add(this.groupBox11);
            this.tabPage_LocSump.Controls.Add(this.groupBox6);
            this.tabPage_LocSump.Location = new System.Drawing.Point(4, 26);
            this.tabPage_LocSump.Name = "tabPage_LocSump";
            this.tabPage_LocSump.Size = new System.Drawing.Size(291, 416);
            this.tabPage_LocSump.TabIndex = 3;
            this.tabPage_LocSump.Text = "位置補正";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.button1);
            this.groupBox11.Controls.Add(this.btn_ResetR1);
            this.groupBox11.Controls.Add(this.num_R1Dir);
            this.groupBox11.Controls.Add(this.num_R1Y);
            this.groupBox11.Controls.Add(this.btn_GetCompustoR1);
            this.groupBox11.Controls.Add(this.num_R1X);
            this.groupBox11.Controls.Add(this.label22);
            this.groupBox11.Controls.Add(this.label20);
            this.groupBox11.Controls.Add(this.btn_GetGPStoR1);
            this.groupBox11.Controls.Add(this.label19);
            this.groupBox11.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox11.Location = new System.Drawing.Point(3, 190);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(273, 145);
            this.groupBox11.TabIndex = 46;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "現在座標(R1) 書き換え(強制補正)";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button1.Location = new System.Drawing.Point(9, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 23);
            this.button1.TabIndex = 46;
            this.button1.Text = "現在値 取得";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btn_ResetR1
            // 
            this.btn_ResetR1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btn_ResetR1.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_ResetR1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_ResetR1.Location = new System.Drawing.Point(151, 107);
            this.btn_ResetR1.Name = "btn_ResetR1";
            this.btn_ResetR1.Size = new System.Drawing.Size(108, 23);
            this.btn_ResetR1.TabIndex = 45;
            this.btn_ResetR1.Text = "書き換え";
            this.btn_ResetR1.UseVisualStyleBackColor = false;
            this.btn_ResetR1.Click += new System.EventHandler(this.btn_ResetR1_Click);
            // 
            // num_R1Dir
            // 
            this.num_R1Dir.Location = new System.Drawing.Point(175, 76);
            this.num_R1Dir.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.num_R1Dir.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.num_R1Dir.Name = "num_R1Dir";
            this.num_R1Dir.Size = new System.Drawing.Size(84, 19);
            this.num_R1Dir.TabIndex = 42;
            this.num_R1Dir.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_R1Y
            // 
            this.num_R1Y.Location = new System.Drawing.Point(160, 47);
            this.num_R1Y.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.num_R1Y.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.num_R1Y.Name = "num_R1Y";
            this.num_R1Y.Size = new System.Drawing.Size(99, 19);
            this.num_R1Y.TabIndex = 40;
            this.num_R1Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btn_GetCompustoR1
            // 
            this.btn_GetCompustoR1.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_GetCompustoR1.Location = new System.Drawing.Point(9, 86);
            this.btn_GetCompustoR1.Name = "btn_GetCompustoR1";
            this.btn_GetCompustoR1.Size = new System.Drawing.Size(108, 23);
            this.btn_GetCompustoR1.TabIndex = 44;
            this.btn_GetCompustoR1.Text = "地磁気値 取得";
            this.btn_GetCompustoR1.UseVisualStyleBackColor = true;
            this.btn_GetCompustoR1.Click += new System.EventHandler(this.btn_GetCompustoR1_Click);
            // 
            // num_R1X
            // 
            this.num_R1X.Location = new System.Drawing.Point(161, 18);
            this.num_R1X.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.num_R1X.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.num_R1X.Name = "num_R1X";
            this.num_R1X.Size = new System.Drawing.Size(98, 19);
            this.num_R1X.TabIndex = 39;
            this.num_R1X.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(133, 78);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(28, 12);
            this.label22.TabIndex = 41;
            this.label22.Text = "向き:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(134, 49);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(14, 12);
            this.label20.TabIndex = 37;
            this.label20.Text = "Y:";
            // 
            // btn_GetGPStoR1
            // 
            this.btn_GetGPStoR1.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_GetGPStoR1.Location = new System.Drawing.Point(9, 54);
            this.btn_GetGPStoR1.Name = "btn_GetGPStoR1";
            this.btn_GetGPStoR1.Size = new System.Drawing.Size(108, 23);
            this.btn_GetGPStoR1.TabIndex = 43;
            this.btn_GetGPStoR1.Text = "GPS値 取得";
            this.btn_GetGPStoR1.UseVisualStyleBackColor = true;
            this.btn_GetGPStoR1.Click += new System.EventHandler(this.btn_GetGPStoR1_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(134, 20);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(14, 12);
            this.label19.TabIndex = 36;
            this.label19.Text = "X:";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cb_VRRevision);
            this.groupBox6.Controls.Add(this.gbox_Revision);
            this.groupBox6.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox6.Location = new System.Drawing.Point(3, 5);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(273, 179);
            this.groupBox6.TabIndex = 35;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "補正条件";
            // 
            // cb_VRRevision
            // 
            this.cb_VRRevision.AutoSize = true;
            this.cb_VRRevision.Location = new System.Drawing.Point(9, 27);
            this.cb_VRRevision.Name = "cb_VRRevision";
            this.cb_VRRevision.Size = new System.Drawing.Size(138, 16);
            this.cb_VRRevision.TabIndex = 37;
            this.cb_VRRevision.Text = "VR上で補正処理を使う";
            this.cb_VRRevision.UseVisualStyleBackColor = true;
            this.cb_VRRevision.CheckedChanged += new System.EventHandler(this.cb_VRRevision_CheckedChanged);
            // 
            // gbox_Revision
            // 
            this.gbox_Revision.Controls.Add(this.cb_DontAlwaysRivision);
            this.gbox_Revision.Controls.Add(this.rb_UseGPS_Revision);
            this.gbox_Revision.Controls.Add(this.btn_LocRevision);
            this.gbox_Revision.Controls.Add(this.rb_UsePF_Revision);
            this.gbox_Revision.Controls.Add(this.cb_AlwaysPFCalc);
            this.gbox_Revision.Enabled = false;
            this.gbox_Revision.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.gbox_Revision.Location = new System.Drawing.Point(4, 59);
            this.gbox_Revision.Name = "gbox_Revision";
            this.gbox_Revision.Size = new System.Drawing.Size(261, 114);
            this.gbox_Revision.TabIndex = 36;
            this.gbox_Revision.TabStop = false;
            this.gbox_Revision.Text = "補正方法";
            // 
            // cb_DontAlwaysRivision
            // 
            this.cb_DontAlwaysRivision.AutoSize = true;
            this.cb_DontAlwaysRivision.Checked = true;
            this.cb_DontAlwaysRivision.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_DontAlwaysRivision.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_DontAlwaysRivision.Location = new System.Drawing.Point(11, 23);
            this.cb_DontAlwaysRivision.Name = "cb_DontAlwaysRivision";
            this.cb_DontAlwaysRivision.Size = new System.Drawing.Size(185, 17);
            this.cb_DontAlwaysRivision.TabIndex = 30;
            this.cb_DontAlwaysRivision.Text = "異常時のみ補正（壁めり込み）";
            this.cb_DontAlwaysRivision.UseVisualStyleBackColor = true;
            // 
            // rb_UseGPS_Revision
            // 
            this.rb_UseGPS_Revision.AutoSize = true;
            this.rb_UseGPS_Revision.Location = new System.Drawing.Point(7, 77);
            this.rb_UseGPS_Revision.Name = "rb_UseGPS_Revision";
            this.rb_UseGPS_Revision.Size = new System.Drawing.Size(45, 16);
            this.rb_UseGPS_Revision.TabIndex = 35;
            this.rb_UseGPS_Revision.TabStop = true;
            this.rb_UseGPS_Revision.Text = "GPS";
            this.rb_UseGPS_Revision.UseVisualStyleBackColor = true;
            // 
            // btn_LocRevision
            // 
            this.btn_LocRevision.Enabled = false;
            this.btn_LocRevision.Location = new System.Drawing.Point(163, 84);
            this.btn_LocRevision.Name = "btn_LocRevision";
            this.btn_LocRevision.Size = new System.Drawing.Size(90, 23);
            this.btn_LocRevision.TabIndex = 18;
            this.btn_LocRevision.Text = "補正執行";
            this.btn_LocRevision.UseVisualStyleBackColor = true;
            this.btn_LocRevision.Click += new System.EventHandler(this.btn_LocRevision_Click);
            // 
            // rb_UsePF_Revision
            // 
            this.rb_UsePF_Revision.AutoSize = true;
            this.rb_UsePF_Revision.Location = new System.Drawing.Point(7, 55);
            this.rb_UsePF_Revision.Name = "rb_UsePF_Revision";
            this.rb_UsePF_Revision.Size = new System.Drawing.Size(89, 16);
            this.rb_UsePF_Revision.TabIndex = 34;
            this.rb_UsePF_Revision.TabStop = true;
            this.rb_UsePF_Revision.Text = "ParticleFilter";
            this.rb_UsePF_Revision.UseVisualStyleBackColor = true;
            this.rb_UsePF_Revision.CheckedChanged += new System.EventHandler(this.rb_UsePF_Revision_CheckedChanged);
            // 
            // cb_AlwaysPFCalc
            // 
            this.cb_AlwaysPFCalc.AutoSize = true;
            this.cb_AlwaysPFCalc.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_AlwaysPFCalc.Location = new System.Drawing.Point(116, 55);
            this.cb_AlwaysPFCalc.Name = "cb_AlwaysPFCalc";
            this.cb_AlwaysPFCalc.Size = new System.Drawing.Size(93, 17);
            this.cb_AlwaysPFCalc.TabIndex = 28;
            this.cb_AlwaysPFCalc.Text = "常時PF演算";
            this.cb_AlwaysPFCalc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_AlwaysPFCalc.UseVisualStyleBackColor = true;
            // 
            // tabPage_Emulate
            // 
            this.tabPage_Emulate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tabPage_Emulate.Controls.Add(this.groupBox12);
            this.tabPage_Emulate.Controls.Add(this.groupBox7);
            this.tabPage_Emulate.Controls.Add(this.cb_InDoorMode);
            this.tabPage_Emulate.Controls.Add(this.cb_StraghtMode);
            this.tabPage_Emulate.Controls.Add(this.groupBox5);
            this.tabPage_Emulate.Controls.Add(this.groupBox2);
            this.tabPage_Emulate.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tabPage_Emulate.Location = new System.Drawing.Point(4, 26);
            this.tabPage_Emulate.Name = "tabPage_Emulate";
            this.tabPage_Emulate.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Emulate.Size = new System.Drawing.Size(291, 416);
            this.tabPage_Emulate.TabIndex = 1;
            this.tabPage_Emulate.Text = "設定";
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.trackBar_LRFViewScale);
            this.groupBox12.Controls.Add(this.label1);
            this.groupBox12.Controls.Add(this.tb_LRFScale);
            this.groupBox12.Location = new System.Drawing.Point(4, 302);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(266, 67);
            this.groupBox12.TabIndex = 57;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "LRF ViewScale";
            // 
            // trackBar_LRFViewScale
            // 
            this.trackBar_LRFViewScale.LargeChange = 1000;
            this.trackBar_LRFViewScale.Location = new System.Drawing.Point(6, 16);
            this.trackBar_LRFViewScale.Maximum = 10000;
            this.trackBar_LRFViewScale.Minimum = 100;
            this.trackBar_LRFViewScale.Name = "trackBar_LRFViewScale";
            this.trackBar_LRFViewScale.Size = new System.Drawing.Size(176, 45);
            this.trackBar_LRFViewScale.SmallChange = 100;
            this.trackBar_LRFViewScale.TabIndex = 26;
            this.trackBar_LRFViewScale.TickFrequency = 500;
            this.trackBar_LRFViewScale.Value = 1000;
            this.trackBar_LRFViewScale.Scroll += new System.EventHandler(this.trackBar_LRFViewScale_Scroll);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.tbCaribrationRER);
            this.groupBox7.Controls.Add(this.label21);
            this.groupBox7.Controls.Add(this.tbCaribrationREL);
            this.groupBox7.Controls.Add(this.label23);
            this.groupBox7.Controls.Add(this.btnCribration);
            this.groupBox7.Location = new System.Drawing.Point(3, 231);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(272, 60);
            this.groupBox7.TabIndex = 56;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "キャリブレーション";
            // 
            // tbCaribrationRER
            // 
            this.tbCaribrationRER.Location = new System.Drawing.Point(148, 16);
            this.tbCaribrationRER.Name = "tbCaribrationRER";
            this.tbCaribrationRER.Size = new System.Drawing.Size(53, 19);
            this.tbCaribrationRER.TabIndex = 55;
            this.tbCaribrationRER.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(7, 19);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(30, 12);
            this.label21.TabIndex = 52;
            this.label21.Text = "RE L";
            // 
            // tbCaribrationREL
            // 
            this.tbCaribrationREL.Location = new System.Drawing.Point(43, 18);
            this.tbCaribrationREL.Name = "tbCaribrationREL";
            this.tbCaribrationREL.Size = new System.Drawing.Size(53, 19);
            this.tbCaribrationREL.TabIndex = 54;
            this.tbCaribrationREL.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(110, 21);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(32, 12);
            this.label23.TabIndex = 53;
            this.label23.Text = "RE R";
            // 
            // btnCribration
            // 
            this.btnCribration.Location = new System.Drawing.Point(215, 16);
            this.btnCribration.Name = "btnCribration";
            this.btnCribration.Size = new System.Drawing.Size(55, 23);
            this.btnCribration.TabIndex = 51;
            this.btnCribration.Text = "セット";
            this.btnCribration.UseVisualStyleBackColor = true;
            this.btnCribration.Click += new System.EventHandler(this.btnCribration_Click);
            // 
            // cb_InDoorMode
            // 
            this.cb_InDoorMode.AutoSize = true;
            this.cb_InDoorMode.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_InDoorMode.Location = new System.Drawing.Point(155, 382);
            this.cb_InDoorMode.Name = "cb_InDoorMode";
            this.cb_InDoorMode.Size = new System.Drawing.Size(96, 20);
            this.cb_InDoorMode.TabIndex = 50;
            this.cb_InDoorMode.Text = "屋内モード";
            this.cb_InDoorMode.UseVisualStyleBackColor = true;
            // 
            // cb_StraghtMode
            // 
            this.cb_StraghtMode.AutoSize = true;
            this.cb_StraghtMode.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_StraghtMode.Location = new System.Drawing.Point(12, 384);
            this.cb_StraghtMode.Name = "cb_StraghtMode";
            this.cb_StraghtMode.Size = new System.Drawing.Size(96, 20);
            this.cb_StraghtMode.TabIndex = 49;
            this.cb_StraghtMode.Text = "直進モード";
            this.cb_StraghtMode.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cmbbox_UsbSH2Connect);
            this.groupBox5.Controls.Add(this.cb_UsbSH2Connect);
            this.groupBox5.Location = new System.Drawing.Point(6, 97);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(264, 58);
            this.groupBox5.TabIndex = 23;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "SH2 Connect";
            // 
            // cmbbox_UsbSH2Connect
            // 
            this.cmbbox_UsbSH2Connect.FormattingEnabled = true;
            this.cmbbox_UsbSH2Connect.Location = new System.Drawing.Point(6, 22);
            this.cmbbox_UsbSH2Connect.Name = "cmbbox_UsbSH2Connect";
            this.cmbbox_UsbSH2Connect.Size = new System.Drawing.Size(171, 20);
            this.cmbbox_UsbSH2Connect.TabIndex = 21;
            // 
            // cb_UsbSH2Connect
            // 
            this.cb_UsbSH2Connect.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_UsbSH2Connect.Location = new System.Drawing.Point(183, 18);
            this.cb_UsbSH2Connect.Name = "cb_UsbSH2Connect";
            this.cb_UsbSH2Connect.Size = new System.Drawing.Size(62, 26);
            this.cb_UsbSH2Connect.TabIndex = 22;
            this.cb_UsbSH2Connect.Text = "接続";
            this.cb_UsbSH2Connect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_UsbSH2Connect.UseVisualStyleBackColor = true;
            this.cb_UsbSH2Connect.CheckedChanged += new System.EventHandler(this.cb_UsbSH2Connect_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cb_SirialConnect);
            this.groupBox2.Controls.Add(this.cb_UsbSirial);
            this.groupBox2.Controls.Add(this.tb_SirialResive);
            this.groupBox2.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox2.Location = new System.Drawing.Point(6, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(264, 78);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "USB-GPSアンテナ接続";
            // 
            // cb_SirialConnect
            // 
            this.cb_SirialConnect.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_SirialConnect.Location = new System.Drawing.Point(168, 18);
            this.cb_SirialConnect.Name = "cb_SirialConnect";
            this.cb_SirialConnect.Size = new System.Drawing.Size(77, 26);
            this.cb_SirialConnect.TabIndex = 23;
            this.cb_SirialConnect.Text = "GPS接続";
            this.cb_SirialConnect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_SirialConnect.UseVisualStyleBackColor = true;
            this.cb_SirialConnect.CheckedChanged += new System.EventHandler(this.cb_SirialConnect_CheckedChanged);
            // 
            // cb_UsbSirial
            // 
            this.cb_UsbSirial.FormattingEnabled = true;
            this.cb_UsbSirial.Location = new System.Drawing.Point(11, 22);
            this.cb_UsbSirial.Name = "cb_UsbSirial";
            this.cb_UsbSirial.Size = new System.Drawing.Size(143, 21);
            this.cb_UsbSirial.TabIndex = 21;
            // 
            // tb_SirialResive
            // 
            this.tb_SirialResive.Location = new System.Drawing.Point(11, 52);
            this.tb_SirialResive.Name = "tb_SirialResive";
            this.tb_SirialResive.Size = new System.Drawing.Size(234, 20);
            this.tb_SirialResive.TabIndex = 22;
            // 
            // cb_StartLogMapping
            // 
            this.cb_StartLogMapping.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_StartLogMapping.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_StartLogMapping.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_StartLogMapping.Location = new System.Drawing.Point(607, 492);
            this.cb_StartLogMapping.Name = "cb_StartLogMapping";
            this.cb_StartLogMapping.Size = new System.Drawing.Size(239, 53);
            this.cb_StartLogMapping.TabIndex = 44;
            this.cb_StartLogMapping.Text = "ログ・Mapping\r\nStart";
            this.cb_StartLogMapping.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_StartLogMapping.UseVisualStyleBackColor = true;
            this.cb_StartLogMapping.CheckedChanged += new System.EventHandler(this.cb_StartLogMapping_CheckedChanged);
            // 
            // lb_BServerConnect
            // 
            this.lb_BServerConnect.AutoSize = true;
            this.lb_BServerConnect.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb_BServerConnect.Location = new System.Drawing.Point(869, 18);
            this.lb_BServerConnect.Name = "lb_BServerConnect";
            this.lb_BServerConnect.Size = new System.Drawing.Size(103, 16);
            this.lb_BServerConnect.TabIndex = 23;
            this.lb_BServerConnect.Text = "bServer接続";
            // 
            // cb_EHS
            // 
            this.cb_EHS.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_EHS.Checked = true;
            this.cb_EHS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_EHS.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_EHS.Location = new System.Drawing.Point(719, 62);
            this.cb_EHS.Name = "cb_EHS";
            this.cb_EHS.Size = new System.Drawing.Size(120, 34);
            this.cb_EHS.TabIndex = 38;
            this.cb_EHS.Text = "壁回避";
            this.cb_EHS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_EHS.UseVisualStyleBackColor = true;
            this.cb_EHS.CheckedChanged += new System.EventHandler(this.cb_Color_CheckedChanged);
            // 
            // lbl_BackCnt
            // 
            this.lbl_BackCnt.AutoSize = true;
            this.lbl_BackCnt.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_BackCnt.Location = new System.Drawing.Point(386, 52);
            this.lbl_BackCnt.Name = "lbl_BackCnt";
            this.lbl_BackCnt.Size = new System.Drawing.Size(75, 16);
            this.lbl_BackCnt.TabIndex = 39;
            this.lbl_BackCnt.Text = "EBS cnt:";
            // 
            // gb_DriveControl
            // 
            this.gb_DriveControl.Controls.Add(this.lbl_BackProcess);
            this.gb_DriveControl.Controls.Add(this.label9);
            this.gb_DriveControl.Controls.Add(this.cb_EHS);
            this.gb_DriveControl.Controls.Add(this.label7);
            this.gb_DriveControl.Controls.Add(this.lbl_BackCnt);
            this.gb_DriveControl.Controls.Add(this.tb_HandleVal);
            this.gb_DriveControl.Controls.Add(this.label6);
            this.gb_DriveControl.Controls.Add(this.tb_RESpeed);
            this.gb_DriveControl.Controls.Add(this.cb_EmgBrake);
            this.gb_DriveControl.Controls.Add(this.label5);
            this.gb_DriveControl.Controls.Add(this.tb_AccelVal);
            this.gb_DriveControl.Controls.Add(this.label8);
            this.gb_DriveControl.Location = new System.Drawing.Point(1, 613);
            this.gb_DriveControl.Name = "gb_DriveControl";
            this.gb_DriveControl.Size = new System.Drawing.Size(845, 112);
            this.gb_DriveControl.TabIndex = 40;
            this.gb_DriveControl.TabStop = false;
            this.gb_DriveControl.Text = "DriveControl";
            // 
            // lbl_BackProcess
            // 
            this.lbl_BackProcess.AutoSize = true;
            this.lbl_BackProcess.BackColor = System.Drawing.Color.Yellow;
            this.lbl_BackProcess.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_BackProcess.Location = new System.Drawing.Point(386, 27);
            this.lbl_BackProcess.Name = "lbl_BackProcess";
            this.lbl_BackProcess.Size = new System.Drawing.Size(96, 16);
            this.lbl_BackProcess.TabIndex = 41;
            this.lbl_BackProcess.Text = "バック動作中";
            this.lbl_BackProcess.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.Location = new System.Drawing.Point(182, 34);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(176, 19);
            this.label9.TabIndex = 40;
            this.label9.Text = "左　1.0　～　右-1.0";
            // 
            // picbox_Indicator
            // 
            this.picbox_Indicator.BackColor = System.Drawing.Color.White;
            this.picbox_Indicator.Location = new System.Drawing.Point(607, 248);
            this.picbox_Indicator.Name = "picbox_Indicator";
            this.picbox_Indicator.Size = new System.Drawing.Size(240, 160);
            this.picbox_Indicator.TabIndex = 41;
            this.picbox_Indicator.TabStop = false;
            this.picbox_Indicator.Paint += new System.Windows.Forms.PaintEventHandler(this.picbox_Indicator_Paint);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.rb_LRF_LAN);
            this.groupBox9.Controls.Add(this.rb_LRF_ROSnode);
            this.groupBox9.Controls.Add(this.cb_LRFConnect);
            this.groupBox9.Controls.Add(this.tb_LRFPort);
            this.groupBox9.Controls.Add(this.lb_LRFResult);
            this.groupBox9.Controls.Add(this.tb_LRFIpAddr);
            this.groupBox9.Location = new System.Drawing.Point(863, 130);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(280, 114);
            this.groupBox9.TabIndex = 43;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "LRF";
            // 
            // rb_LRF_LAN
            // 
            this.rb_LRF_LAN.AutoSize = true;
            this.rb_LRF_LAN.Location = new System.Drawing.Point(16, 51);
            this.rb_LRF_LAN.Name = "rb_LRF_LAN";
            this.rb_LRF_LAN.Size = new System.Drawing.Size(87, 16);
            this.rb_LRF_LAN.TabIndex = 28;
            this.rb_LRF_LAN.Text = "ConnectLAN";
            this.rb_LRF_LAN.UseVisualStyleBackColor = true;
            this.rb_LRF_LAN.CheckedChanged += new System.EventHandler(this.rb_LRF_LAN_CheckedChanged);
            // 
            // rb_LRF_ROSnode
            // 
            this.rb_LRF_ROSnode.AutoSize = true;
            this.rb_LRF_ROSnode.Checked = true;
            this.rb_LRF_ROSnode.Location = new System.Drawing.Point(16, 24);
            this.rb_LRF_ROSnode.Name = "rb_LRF_ROSnode";
            this.rb_LRF_ROSnode.Size = new System.Drawing.Size(88, 16);
            this.rb_LRF_ROSnode.TabIndex = 27;
            this.rb_LRF_ROSnode.TabStop = true;
            this.rb_LRF_ROSnode.Text = "ConnectROS";
            this.rb_LRF_ROSnode.UseVisualStyleBackColor = true;
            this.rb_LRF_ROSnode.CheckedChanged += new System.EventHandler(this.rb_LRF_LAN_CheckedChanged);
            // 
            // cb_ConnectBServerEmu
            // 
            this.cb_ConnectBServerEmu.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_ConnectBServerEmu.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_ConnectBServerEmu.Location = new System.Drawing.Point(1008, 12);
            this.cb_ConnectBServerEmu.Name = "cb_ConnectBServerEmu";
            this.cb_ConnectBServerEmu.Size = new System.Drawing.Size(120, 28);
            this.cb_ConnectBServerEmu.TabIndex = 45;
            this.cb_ConnectBServerEmu.Text = "Emu接続";
            this.cb_ConnectBServerEmu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_ConnectBServerEmu.UseVisualStyleBackColor = true;
            this.cb_ConnectBServerEmu.CheckedChanged += new System.EventHandler(this.cb_ConnectBServerEmu_CheckedChanged);
            // 
            // cb_ConnectRosIF
            // 
            this.cb_ConnectRosIF.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_ConnectRosIF.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_ConnectRosIF.Location = new System.Drawing.Point(1008, 46);
            this.cb_ConnectRosIF.Name = "cb_ConnectRosIF";
            this.cb_ConnectRosIF.Size = new System.Drawing.Size(120, 28);
            this.cb_ConnectRosIF.TabIndex = 46;
            this.cb_ConnectRosIF.Text = "RosIF 接続";
            this.cb_ConnectRosIF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_ConnectRosIF.UseVisualStyleBackColor = true;
            this.cb_ConnectRosIF.CheckedChanged += new System.EventHandler(this.cb_ConnectRosIF_CheckedChanged);
            // 
            // btn_MapLoad
            // 
            this.btn_MapLoad.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_MapLoad.Location = new System.Drawing.Point(607, 445);
            this.btn_MapLoad.Name = "btn_MapLoad";
            this.btn_MapLoad.Size = new System.Drawing.Size(115, 36);
            this.btn_MapLoad.TabIndex = 47;
            this.btn_MapLoad.Text = "MAP選択";
            this.btn_MapLoad.UseVisualStyleBackColor = true;
            this.btn_MapLoad.Click += new System.EventHandler(this.btn_MapLoad_Click);
            // 
            // tb_MapName
            // 
            this.tb_MapName.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_MapName.Location = new System.Drawing.Point(608, 416);
            this.tb_MapName.Name = "tb_MapName";
            this.tb_MapName.ReadOnly = true;
            this.tb_MapName.Size = new System.Drawing.Size(239, 23);
            this.tb_MapName.TabIndex = 48;
            this.tb_MapName.TabStop = false;
            // 
            // VehicleRunnerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1157, 729);
            this.Controls.Add(this.tb_MapName);
            this.Controls.Add(this.btn_MapLoad);
            this.Controls.Add(this.cb_ConnectRosIF);
            this.Controls.Add(this.cb_ConnectBServerEmu);
            this.Controls.Add(this.lb_BServerConnect);
            this.Controls.Add(this.btn_VRReset);
            this.Controls.Add(this.cb_StartAutonomous);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.picbox_Indicator);
            this.Controls.Add(this.cb_StartLogMapping);
            this.Controls.Add(this.gb_DriveControl);
            this.Controls.Add(this.picbox_LRF);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.picbox_AreaMap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "VehicleRunnerForm";
            this.Text = "VehicleRunner Ver2.10  Build:2016.10.01";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VehicleRunnerForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VehicleRunnerForm_FormClosed);
            this.Load += new System.EventHandler(this.VehicleRunnerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picbox_AreaMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_LRF)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPage_BoxPC.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage_Sensor.ResumeLayout(false);
            this.tabPage_Sensor.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage_LocSump.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_R1Dir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_R1Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_R1X)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.gbox_Revision.ResumeLayout(false);
            this.gbox_Revision.PerformLayout();
            this.tabPage_Emulate.ResumeLayout(false);
            this.tabPage_Emulate.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_LRFViewScale)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gb_DriveControl.ResumeLayout(false);
            this.gb_DriveControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_Indicator)).EndInit();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picbox_AreaMap;
        private System.Windows.Forms.PictureBox picbox_LRF;
        private System.Windows.Forms.CheckBox cb_LRFConnect;
        private System.Windows.Forms.Label lb_LRFResult;
        private System.Windows.Forms.CheckBox cb_StartAutonomous;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_LRFScale;
        private System.Windows.Forms.Button btn_VRReset;
        private System.Windows.Forms.TextBox tb_LRFIpAddr;
        private System.Windows.Forms.TextBox tb_LRFPort;
        private System.Windows.Forms.CheckBox cb_EmgBrake;
        private System.Windows.Forms.Timer tm_UpdateHw;
        private System.Windows.Forms.TextBox tb_ResiveData;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_SendData;
        private System.Windows.Forms.Timer tm_LocUpdate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_AccelVal;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_HandleVal;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_RESpeed;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage_Emulate;
        private System.Windows.Forms.Button btn_LocRevision;
        private System.Windows.Forms.CheckBox cb_EHS;
        private System.Windows.Forms.TabPage tabPage_Sensor;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbl_GPS_X;
        private System.Windows.Forms.Label lbl_GPS_Y;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lbl_LED;
        private System.Windows.Forms.Label lbl_REPlotDir;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lbl_REPlotY;
        private System.Windows.Forms.Label lbl_REPlotX;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lbl_Compass;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lbl_BackCnt;
        private System.Windows.Forms.Label lbl_RErotL;
        private System.Windows.Forms.Label lbl_RErotR;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TabPage tabPage_LocSump;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cb_SirialConnect;
        private System.Windows.Forms.ComboBox cb_UsbSirial;
        private System.Windows.Forms.TextBox tb_SirialResive;
        private System.Windows.Forms.CheckBox cb_AlwaysPFCalc;
        private System.Windows.Forms.CheckBox cb_DontAlwaysRivision;
        private System.Windows.Forms.ComboBox cmbbox_UsbSH2Connect;
        private System.Windows.Forms.CheckBox cb_UsbSH2Connect;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox gb_DriveControl;
        private System.Windows.Forms.Label lb_BServerConnect;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox gbox_Revision;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TrackBar trackBar_LRFViewScale;
        private System.Windows.Forms.PictureBox picbox_Indicator;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.CheckBox cb_StartLogMapping;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.RadioButton rb_DirSVO;
        private System.Windows.Forms.RadioButton rb_DirGPS;
        private System.Windows.Forms.RadioButton rb_DirREPlot;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rb_MoveSVO;
        private System.Windows.Forms.RadioButton rb_MoveGPS;
        private System.Windows.Forms.RadioButton rb_MoveREPlot;
        private System.Windows.Forms.RadioButton rb_DirCompus;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton rb_UsePF_Revision;
        private System.Windows.Forms.RadioButton rb_UseGPS_Revision;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Button btn_ResetR1;
        private System.Windows.Forms.NumericUpDown num_R1Dir;
        private System.Windows.Forms.Button btn_GetCompustoR1;
        private System.Windows.Forms.NumericUpDown num_R1Y;
        private System.Windows.Forms.Button btn_GetGPStoR1;
        private System.Windows.Forms.NumericUpDown num_R1X;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lbl_BackProcess;
        private System.Windows.Forms.Label lbl_bServerEmu;
        private System.Windows.Forms.RadioButton rb_DirAMCL;
        private System.Windows.Forms.RadioButton rb_MoveAMCL;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton rb_LRF_LAN;
        private System.Windows.Forms.RadioButton rb_LRF_ROSnode;
        private System.Windows.Forms.CheckBox cb_StraghtMode;
        private System.Windows.Forms.CheckBox cb_VRRevision;
        private System.Windows.Forms.CheckBox cb_InDoorMode;
        private System.Windows.Forms.RadioButton rb_MovePF;
        private System.Windows.Forms.RadioButton rb_MoveREandCompus;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox tbCaribrationRER;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox tbCaribrationREL;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button btnCribration;
        private System.Windows.Forms.TabPage tabPage_BoxPC;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.CheckBox cb_ConnectBServerEmu;
        private System.Windows.Forms.CheckBox cb_ConnectRosIF;
        private System.Windows.Forms.Button btn_MapLoad;
        private System.Windows.Forms.TextBox tb_MapName;
    }
}

