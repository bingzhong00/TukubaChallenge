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
            this.cb_StartAutonomous = new System.Windows.Forms.CheckBox();
            this.btn_VRReset = new System.Windows.Forms.Button();
            this.tb_ResiveData = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_SendData = new System.Windows.Forms.TextBox();
            this.tm_Update = new System.Windows.Forms.Timer(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.tb_AccelVal = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_HandleVal = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_RESpeed = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
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
            this.lbl_LED = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbl_bServerEmu = new System.Windows.Forms.Label();
            this.cb_InDoorMode = new System.Windows.Forms.CheckBox();
            this.lb_BServerConnect = new System.Windows.Forms.Label();
            this.gb_DriveControl = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_Trip = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_BackProcess = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.picbox_Indicator = new System.Windows.Forms.PictureBox();
            this.btn_MapLoad = new System.Windows.Forms.Button();
            this.tb_MapName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_AccelOff = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericUD_CheckPoint = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.numericUD_DebugDir = new System.Windows.Forms.NumericUpDown();
            this.rb_SelAMCL = new System.Windows.Forms.RadioButton();
            this.rb_SelREPlot = new System.Windows.Forms.RadioButton();
            this.label13 = new System.Windows.Forms.Label();
            this.lbl_CarName = new System.Windows.Forms.Label();
            this.btn_bServerEmu = new System.Windows.Forms.Button();
            this.numericUD_DebugX = new System.Windows.Forms.NumericUpDown();
            this.numericUD_DebugY = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_AreaMap)).BeginInit();
            this.gb_DriveControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_Indicator)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUD_CheckPoint)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUD_DebugDir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUD_DebugX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUD_DebugY)).BeginInit();
            this.SuspendLayout();
            // 
            // picbox_AreaMap
            // 
            this.picbox_AreaMap.BackColor = System.Drawing.Color.White;
            this.picbox_AreaMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picbox_AreaMap.Location = new System.Drawing.Point(2, 4);
            this.picbox_AreaMap.Name = "picbox_AreaMap";
            this.picbox_AreaMap.Size = new System.Drawing.Size(600, 500);
            this.picbox_AreaMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picbox_AreaMap.TabIndex = 0;
            this.picbox_AreaMap.TabStop = false;
            this.picbox_AreaMap.Paint += new System.Windows.Forms.PaintEventHandler(this.picbox_AreaMap_Paint);
            this.picbox_AreaMap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picbox_AreaMap_MouseClick);
            this.picbox_AreaMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picbox_AreaMap_MouseDown);
            this.picbox_AreaMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picbox_AreaMap_MouseMove);
            this.picbox_AreaMap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picbox_AreaMap_MouseUp);
            // 
            // cb_StartAutonomous
            // 
            this.cb_StartAutonomous.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_StartAutonomous.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_StartAutonomous.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_StartAutonomous.Location = new System.Drawing.Point(608, 589);
            this.cb_StartAutonomous.Name = "cb_StartAutonomous";
            this.cb_StartAutonomous.Size = new System.Drawing.Size(239, 53);
            this.cb_StartAutonomous.TabIndex = 5;
            this.cb_StartAutonomous.Text = "Autonomous\r\nStart";
            this.cb_StartAutonomous.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_StartAutonomous.UseVisualStyleBackColor = true;
            this.cb_StartAutonomous.CheckedChanged += new System.EventHandler(this.cb_Autonomous_CheckedChanged);
            // 
            // btn_VRReset
            // 
            this.btn_VRReset.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_VRReset.Location = new System.Drawing.Point(179, 62);
            this.btn_VRReset.Name = "btn_VRReset";
            this.btn_VRReset.Size = new System.Drawing.Size(62, 25);
            this.btn_VRReset.TabIndex = 12;
            this.btn_VRReset.Text = "リセット";
            this.btn_VRReset.UseVisualStyleBackColor = true;
            this.btn_VRReset.Click += new System.EventHandler(this.btn_PositionReset_Click);
            // 
            // tb_ResiveData
            // 
            this.tb_ResiveData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_ResiveData.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_ResiveData.Location = new System.Drawing.Point(42, 101);
            this.tb_ResiveData.Name = "tb_ResiveData";
            this.tb_ResiveData.ReadOnly = true;
            this.tb_ResiveData.Size = new System.Drawing.Size(183, 19);
            this.tb_ResiveData.TabIndex = 24;
            this.tb_ResiveData.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(11, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 25;
            this.label3.Text = "受信";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(11, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 26;
            this.label4.Text = "送信";
            // 
            // tb_SendData
            // 
            this.tb_SendData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_SendData.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_SendData.Location = new System.Drawing.Point(42, 72);
            this.tb_SendData.Name = "tb_SendData";
            this.tb_SendData.ReadOnly = true;
            this.tb_SendData.Size = new System.Drawing.Size(183, 19);
            this.tb_SendData.TabIndex = 27;
            this.tb_SendData.TabStop = false;
            // 
            // tm_Update
            // 
            this.tm_Update.Tick += new System.EventHandler(this.tm_Update_Tick);
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
            this.label7.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(364, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 21);
            this.label7.TabIndex = 33;
            this.label7.Text = "SPEED";
            // 
            // tb_RESpeed
            // 
            this.tb_RESpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_RESpeed.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_RESpeed.Location = new System.Drawing.Point(449, 28);
            this.tb_RESpeed.Name = "tb_RESpeed";
            this.tb_RESpeed.ReadOnly = true;
            this.tb_RESpeed.Size = new System.Drawing.Size(95, 31);
            this.tb_RESpeed.TabIndex = 34;
            this.tb_RESpeed.TabStop = false;
            this.tb_RESpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(550, 38);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 16);
            this.label8.TabIndex = 35;
            this.label8.Text = "km/h";
            // 
            // lbl_RErotL
            // 
            this.lbl_RErotL.AutoSize = true;
            this.lbl_RErotL.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_RErotL.Location = new System.Drawing.Point(61, 75);
            this.lbl_RErotL.Name = "lbl_RErotL";
            this.lbl_RErotL.Size = new System.Drawing.Size(28, 16);
            this.lbl_RErotL.TabIndex = 17;
            this.lbl_RErotL.Text = "ND";
            // 
            // lbl_RErotR
            // 
            this.lbl_RErotR.AutoSize = true;
            this.lbl_RErotR.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_RErotR.Location = new System.Drawing.Point(188, 75);
            this.lbl_RErotR.Name = "lbl_RErotR";
            this.lbl_RErotR.Size = new System.Drawing.Size(28, 16);
            this.lbl_RErotR.TabIndex = 16;
            this.lbl_RErotR.Text = "ND";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label17.Location = new System.Drawing.Point(12, 75);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(43, 16);
            this.label17.TabIndex = 15;
            this.label17.Text = "Re L:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label16.Location = new System.Drawing.Point(138, 75);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(44, 16);
            this.label16.TabIndex = 14;
            this.label16.Text = "Re R:";
            // 
            // lbl_REPlotDir
            // 
            this.lbl_REPlotDir.AutoSize = true;
            this.lbl_REPlotDir.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_REPlotDir.Location = new System.Drawing.Point(70, 81);
            this.lbl_REPlotDir.Name = "lbl_REPlotDir";
            this.lbl_REPlotDir.Size = new System.Drawing.Size(28, 16);
            this.lbl_REPlotDir.TabIndex = 13;
            this.lbl_REPlotDir.Text = "ND";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label18.Location = new System.Drawing.Point(4, 81);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(55, 16);
            this.label18.TabIndex = 12;
            this.label18.Text = "plotDir:";
            // 
            // lbl_REPlotY
            // 
            this.lbl_REPlotY.AutoSize = true;
            this.lbl_REPlotY.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_REPlotY.Location = new System.Drawing.Point(61, 57);
            this.lbl_REPlotY.Name = "lbl_REPlotY";
            this.lbl_REPlotY.Size = new System.Drawing.Size(28, 16);
            this.lbl_REPlotY.TabIndex = 11;
            this.lbl_REPlotY.Text = "ND";
            // 
            // lbl_REPlotX
            // 
            this.lbl_REPlotX.AutoSize = true;
            this.lbl_REPlotX.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_REPlotX.Location = new System.Drawing.Point(61, 32);
            this.lbl_REPlotX.Name = "lbl_REPlotX";
            this.lbl_REPlotX.Size = new System.Drawing.Size(28, 16);
            this.lbl_REPlotX.TabIndex = 10;
            this.lbl_REPlotX.Text = "ND";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label15.Location = new System.Drawing.Point(4, 57);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(45, 16);
            this.label15.TabIndex = 9;
            this.label15.Text = "plotY:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label14.Location = new System.Drawing.Point(4, 32);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(46, 16);
            this.label14.TabIndex = 8;
            this.label14.Text = "plotX:";
            // 
            // lbl_LED
            // 
            this.lbl_LED.AutoSize = true;
            this.lbl_LED.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_LED.Location = new System.Drawing.Point(61, 50);
            this.lbl_LED.Name = "lbl_LED";
            this.lbl_LED.Size = new System.Drawing.Size(28, 16);
            this.lbl_LED.TabIndex = 5;
            this.lbl_LED.Text = "ND";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.Location = new System.Drawing.Point(12, 50);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 16);
            this.label10.TabIndex = 0;
            this.label10.Text = "LED:";
            // 
            // lbl_bServerEmu
            // 
            this.lbl_bServerEmu.AutoSize = true;
            this.lbl_bServerEmu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lbl_bServerEmu.Location = new System.Drawing.Point(7, 47);
            this.lbl_bServerEmu.Name = "lbl_bServerEmu";
            this.lbl_bServerEmu.Size = new System.Drawing.Size(140, 12);
            this.lbl_bServerEmu.TabIndex = 30;
            this.lbl_bServerEmu.Text = "bServerエミュレーションモード";
            // 
            // cb_InDoorMode
            // 
            this.cb_InDoorMode.AutoSize = true;
            this.cb_InDoorMode.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_InDoorMode.Location = new System.Drawing.Point(614, 557);
            this.cb_InDoorMode.Name = "cb_InDoorMode";
            this.cb_InDoorMode.Size = new System.Drawing.Size(101, 20);
            this.cb_InDoorMode.TabIndex = 50;
            this.cb_InDoorMode.Text = "屋内モード";
            this.cb_InDoorMode.UseVisualStyleBackColor = true;
            // 
            // lb_BServerConnect
            // 
            this.lb_BServerConnect.AutoSize = true;
            this.lb_BServerConnect.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb_BServerConnect.Location = new System.Drawing.Point(6, 21);
            this.lb_BServerConnect.Name = "lb_BServerConnect";
            this.lb_BServerConnect.Size = new System.Drawing.Size(103, 16);
            this.lb_BServerConnect.TabIndex = 23;
            this.lb_BServerConnect.Text = "bServer接続";
            // 
            // gb_DriveControl
            // 
            this.gb_DriveControl.Controls.Add(this.label2);
            this.gb_DriveControl.Controls.Add(this.tb_Trip);
            this.gb_DriveControl.Controls.Add(this.label1);
            this.gb_DriveControl.Controls.Add(this.lbl_BackProcess);
            this.gb_DriveControl.Controls.Add(this.label9);
            this.gb_DriveControl.Controls.Add(this.label7);
            this.gb_DriveControl.Controls.Add(this.tb_HandleVal);
            this.gb_DriveControl.Controls.Add(this.label6);
            this.gb_DriveControl.Controls.Add(this.tb_RESpeed);
            this.gb_DriveControl.Controls.Add(this.label5);
            this.gb_DriveControl.Controls.Add(this.tb_AccelVal);
            this.gb_DriveControl.Controls.Add(this.label8);
            this.gb_DriveControl.Location = new System.Drawing.Point(2, 521);
            this.gb_DriveControl.Name = "gb_DriveControl";
            this.gb_DriveControl.Size = new System.Drawing.Size(600, 121);
            this.gb_DriveControl.TabIndex = 40;
            this.gb_DriveControl.TabStop = false;
            this.gb_DriveControl.Text = "DriveControl";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(550, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 16);
            this.label2.TabIndex = 44;
            this.label2.Text = "m";
            // 
            // tb_Trip
            // 
            this.tb_Trip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_Trip.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_Trip.Location = new System.Drawing.Point(449, 69);
            this.tb_Trip.Name = "tb_Trip";
            this.tb_Trip.ReadOnly = true;
            this.tb_Trip.Size = new System.Drawing.Size(95, 31);
            this.tb_Trip.TabIndex = 43;
            this.tb_Trip.TabStop = false;
            this.tb_Trip.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(364, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 21);
            this.label1.TabIndex = 42;
            this.label1.Text = "TRIP";
            // 
            // lbl_BackProcess
            // 
            this.lbl_BackProcess.AutoSize = true;
            this.lbl_BackProcess.BackColor = System.Drawing.Color.Yellow;
            this.lbl_BackProcess.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_BackProcess.Location = new System.Drawing.Point(182, 77);
            this.lbl_BackProcess.Name = "lbl_BackProcess";
            this.lbl_BackProcess.Size = new System.Drawing.Size(96, 16);
            this.lbl_BackProcess.TabIndex = 41;
            this.lbl_BackProcess.Text = "バック動作中";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.Location = new System.Drawing.Point(182, 38);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(135, 16);
            this.label9.TabIndex = 40;
            this.label9.Text = "左　1.0　～　右-1.0";
            // 
            // picbox_Indicator
            // 
            this.picbox_Indicator.BackColor = System.Drawing.Color.White;
            this.picbox_Indicator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picbox_Indicator.Location = new System.Drawing.Point(355, 12);
            this.picbox_Indicator.Name = "picbox_Indicator";
            this.picbox_Indicator.Size = new System.Drawing.Size(240, 120);
            this.picbox_Indicator.TabIndex = 41;
            this.picbox_Indicator.TabStop = false;
            this.picbox_Indicator.Paint += new System.Windows.Forms.PaintEventHandler(this.picbox_Indicator_Paint);
            // 
            // btn_MapLoad
            // 
            this.btn_MapLoad.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_MapLoad.Location = new System.Drawing.Point(202, 21);
            this.btn_MapLoad.Name = "btn_MapLoad";
            this.btn_MapLoad.Size = new System.Drawing.Size(39, 23);
            this.btn_MapLoad.TabIndex = 47;
            this.btn_MapLoad.Text = "...";
            this.btn_MapLoad.UseVisualStyleBackColor = true;
            this.btn_MapLoad.Click += new System.EventHandler(this.btn_MapLoad_Click);
            // 
            // tb_MapName
            // 
            this.tb_MapName.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_MapName.Location = new System.Drawing.Point(8, 21);
            this.tb_MapName.Name = "tb_MapName";
            this.tb_MapName.ReadOnly = true;
            this.tb_MapName.Size = new System.Drawing.Size(188, 23);
            this.tb_MapName.TabIndex = 48;
            this.tb_MapName.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_bServerEmu);
            this.groupBox1.Controls.Add(this.tb_SendData);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tb_ResiveData);
            this.groupBox1.Controls.Add(this.lbl_bServerEmu);
            this.groupBox1.Controls.Add(this.lb_BServerConnect);
            this.groupBox1.Location = new System.Drawing.Point(607, 103);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(243, 127);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "bServer";
            // 
            // cb_AccelOff
            // 
            this.cb_AccelOff.AutoSize = true;
            this.cb_AccelOff.Checked = true;
            this.cb_AccelOff.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_AccelOff.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_AccelOff.Location = new System.Drawing.Point(736, 555);
            this.cb_AccelOff.Name = "cb_AccelOff";
            this.cb_AccelOff.Size = new System.Drawing.Size(112, 20);
            this.cb_AccelOff.TabIndex = 45;
            this.cb_AccelOff.Text = "アクセルOFF";
            this.cb_AccelOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_AccelOff.UseVisualStyleBackColor = true;
            this.cb_AccelOff.CheckedChanged += new System.EventHandler(this.cb_AccelOff_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tb_MapName);
            this.groupBox2.Controls.Add(this.btn_MapLoad);
            this.groupBox2.Controls.Add(this.numericUD_CheckPoint);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.btn_VRReset);
            this.groupBox2.Location = new System.Drawing.Point(606, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(244, 95);
            this.groupBox2.TabIndex = 51;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Map";
            // 
            // numericUD_CheckPoint
            // 
            this.numericUD_CheckPoint.Location = new System.Drawing.Point(99, 65);
            this.numericUD_CheckPoint.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUD_CheckPoint.Name = "numericUD_CheckPoint";
            this.numericUD_CheckPoint.Size = new System.Drawing.Size(52, 19);
            this.numericUD_CheckPoint.TabIndex = 44;
            this.numericUD_CheckPoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUD_CheckPoint.Click += new System.EventHandler(this.numericUD_CheckPoint_Click);
            this.numericUD_CheckPoint.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericUD_CheckPoint_KeyPress);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label11.Location = new System.Drawing.Point(5, 65);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(88, 16);
            this.label11.TabIndex = 43;
            this.label11.Text = "CheckPoint:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panel1);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.lbl_CarName);
            this.groupBox3.Controls.Add(this.lbl_RErotL);
            this.groupBox3.Controls.Add(this.lbl_LED);
            this.groupBox3.Controls.Add(this.lbl_RErotR);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Location = new System.Drawing.Point(607, 238);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(243, 266);
            this.groupBox3.TabIndex = 52;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Car Status";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.numericUD_DebugY);
            this.panel1.Controls.Add(this.numericUD_DebugX);
            this.panel1.Controls.Add(this.numericUD_DebugDir);
            this.panel1.Controls.Add(this.rb_SelAMCL);
            this.panel1.Controls.Add(this.rb_SelREPlot);
            this.panel1.Controls.Add(this.lbl_REPlotX);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.lbl_REPlotY);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.lbl_REPlotDir);
            this.panel1.Location = new System.Drawing.Point(9, 113);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(225, 109);
            this.panel1.TabIndex = 53;
            // 
            // numericUD_DebugDir
            // 
            this.numericUD_DebugDir.DecimalPlaces = 2;
            this.numericUD_DebugDir.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUD_DebugDir.Location = new System.Drawing.Point(163, 78);
            this.numericUD_DebugDir.Maximum = new decimal(new int[] {
            314,
            0,
            0,
            131072});
            this.numericUD_DebugDir.Minimum = new decimal(new int[] {
            314,
            0,
            0,
            -2147352576});
            this.numericUD_DebugDir.Name = "numericUD_DebugDir";
            this.numericUD_DebugDir.Size = new System.Drawing.Size(52, 19);
            this.numericUD_DebugDir.TabIndex = 50;
            this.numericUD_DebugDir.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUD_DebugDir.Click += new System.EventHandler(this.numericUD_DebugDir_Click);
            // 
            // rb_SelAMCL
            // 
            this.rb_SelAMCL.AutoSize = true;
            this.rb_SelAMCL.Checked = true;
            this.rb_SelAMCL.Location = new System.Drawing.Point(7, 7);
            this.rb_SelAMCL.Name = "rb_SelAMCL";
            this.rb_SelAMCL.Size = new System.Drawing.Size(54, 16);
            this.rb_SelAMCL.TabIndex = 47;
            this.rb_SelAMCL.TabStop = true;
            this.rb_SelAMCL.Text = "AMCL";
            this.rb_SelAMCL.UseVisualStyleBackColor = true;
            // 
            // rb_SelREPlot
            // 
            this.rb_SelREPlot.AutoSize = true;
            this.rb_SelREPlot.Location = new System.Drawing.Point(84, 7);
            this.rb_SelREPlot.Name = "rb_SelREPlot";
            this.rb_SelREPlot.Size = new System.Drawing.Size(58, 16);
            this.rb_SelREPlot.TabIndex = 48;
            this.rb_SelREPlot.Text = "REPlot";
            this.rb_SelREPlot.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label13.Location = new System.Drawing.Point(13, 20);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(69, 16);
            this.label13.TabIndex = 49;
            this.label13.Text = "CarType:";
            // 
            // lbl_CarName
            // 
            this.lbl_CarName.AutoSize = true;
            this.lbl_CarName.BackColor = System.Drawing.Color.Yellow;
            this.lbl_CarName.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_CarName.Location = new System.Drawing.Point(88, 20);
            this.lbl_CarName.Name = "lbl_CarName";
            this.lbl_CarName.Size = new System.Drawing.Size(45, 16);
            this.lbl_CarName.TabIndex = 45;
            this.lbl_CarName.Text = "Benz";
            // 
            // btn_bServerEmu
            // 
            this.btn_bServerEmu.Location = new System.Drawing.Point(163, 42);
            this.btn_bServerEmu.Name = "btn_bServerEmu";
            this.btn_bServerEmu.Size = new System.Drawing.Size(75, 23);
            this.btn_bServerEmu.TabIndex = 31;
            this.btn_bServerEmu.Text = "Emu接続";
            this.btn_bServerEmu.UseVisualStyleBackColor = true;
            this.btn_bServerEmu.Click += new System.EventHandler(this.btn_bServerEmu_Click);
            // 
            // numericUD_DebugX
            // 
            this.numericUD_DebugX.DecimalPlaces = 2;
            this.numericUD_DebugX.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUD_DebugX.Location = new System.Drawing.Point(163, 28);
            this.numericUD_DebugX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUD_DebugX.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUD_DebugX.Name = "numericUD_DebugX";
            this.numericUD_DebugX.Size = new System.Drawing.Size(52, 19);
            this.numericUD_DebugX.TabIndex = 51;
            this.numericUD_DebugX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUD_DebugX.Click += new System.EventHandler(this.numericUD_DebugX_Click);
            // 
            // numericUD_DebugY
            // 
            this.numericUD_DebugY.DecimalPlaces = 2;
            this.numericUD_DebugY.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUD_DebugY.Location = new System.Drawing.Point(163, 53);
            this.numericUD_DebugY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUD_DebugY.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUD_DebugY.Name = "numericUD_DebugY";
            this.numericUD_DebugY.Size = new System.Drawing.Size(52, 19);
            this.numericUD_DebugY.TabIndex = 52;
            this.numericUD_DebugY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUD_DebugY.Click += new System.EventHandler(this.numericUD_DebugY_Click);
            // 
            // VehicleRunnerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 654);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.cb_AccelOff);
            this.Controls.Add(this.cb_StartAutonomous);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cb_InDoorMode);
            this.Controls.Add(this.picbox_Indicator);
            this.Controls.Add(this.gb_DriveControl);
            this.Controls.Add(this.picbox_AreaMap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "VehicleRunnerForm";
            this.Text = "VehicleRunner2 Ver0.22  Build:2017.04.23";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VehicleRunnerForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VehicleRunnerForm_FormClosed);
            this.Load += new System.EventHandler(this.VehicleRunnerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picbox_AreaMap)).EndInit();
            this.gb_DriveControl.ResumeLayout(false);
            this.gb_DriveControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_Indicator)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUD_CheckPoint)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUD_DebugDir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUD_DebugX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUD_DebugY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picbox_AreaMap;
        private System.Windows.Forms.CheckBox cb_StartAutonomous;
        private System.Windows.Forms.Button btn_VRReset;
        private System.Windows.Forms.TextBox tb_ResiveData;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_SendData;
        private System.Windows.Forms.Timer tm_Update;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_AccelVal;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_HandleVal;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_RESpeed;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbl_LED;
        private System.Windows.Forms.Label lbl_REPlotDir;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lbl_REPlotY;
        private System.Windows.Forms.Label lbl_REPlotX;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lbl_RErotL;
        private System.Windows.Forms.Label lbl_RErotR;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox gb_DriveControl;
        private System.Windows.Forms.Label lb_BServerConnect;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox picbox_Indicator;
        private System.Windows.Forms.Label lbl_BackProcess;
        private System.Windows.Forms.Label lbl_bServerEmu;
        private System.Windows.Forms.CheckBox cb_InDoorMode;
        private System.Windows.Forms.Button btn_MapLoad;
        private System.Windows.Forms.TextBox tb_MapName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_Trip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cb_AccelOff;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lbl_CarName;
        private System.Windows.Forms.NumericUpDown numericUD_CheckPoint;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.RadioButton rb_SelREPlot;
        private System.Windows.Forms.RadioButton rb_SelAMCL;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numericUD_DebugDir;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_bServerEmu;
        private System.Windows.Forms.NumericUpDown numericUD_DebugY;
        private System.Windows.Forms.NumericUpDown numericUD_DebugX;
    }
}

