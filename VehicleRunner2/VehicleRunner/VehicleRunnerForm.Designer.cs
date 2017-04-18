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
            this.num_R1Dir = new System.Windows.Forms.NumericUpDown();
            this.num_R1Y = new System.Windows.Forms.NumericUpDown();
            this.num_R1X = new System.Windows.Forms.NumericUpDown();
            this.label22 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.cb_InDoorMode = new System.Windows.Forms.CheckBox();
            this.lb_BServerConnect = new System.Windows.Forms.Label();
            this.gb_DriveControl = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_Trip = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_BackProcess = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.picbox_Indicator = new System.Windows.Forms.PictureBox();
            this.cb_ConnectBServerEmu = new System.Windows.Forms.CheckBox();
            this.btn_MapLoad = new System.Windows.Forms.Button();
            this.tb_MapName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_AccelOff = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_AreaMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_R1Dir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_R1Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_R1X)).BeginInit();
            this.gb_DriveControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_Indicator)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            // cb_StartAutonomous
            // 
            this.cb_StartAutonomous.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_StartAutonomous.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_StartAutonomous.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_StartAutonomous.Location = new System.Drawing.Point(607, 664);
            this.cb_StartAutonomous.Name = "cb_StartAutonomous";
            this.cb_StartAutonomous.Size = new System.Drawing.Size(239, 53);
            this.cb_StartAutonomous.TabIndex = 5;
            this.cb_StartAutonomous.Text = "自律走行\r\nStart";
            this.cb_StartAutonomous.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_StartAutonomous.UseVisualStyleBackColor = true;
            this.cb_StartAutonomous.CheckedChanged += new System.EventHandler(this.cb_Autonomous_CheckedChanged);
            // 
            // btn_VRReset
            // 
            this.btn_VRReset.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_VRReset.Location = new System.Drawing.Point(120, 91);
            this.btn_VRReset.Name = "btn_VRReset";
            this.btn_VRReset.Size = new System.Drawing.Size(108, 30);
            this.btn_VRReset.TabIndex = 12;
            this.btn_VRReset.Text = "リセット";
            this.btn_VRReset.UseVisualStyleBackColor = true;
            this.btn_VRReset.Click += new System.EventHandler(this.btn_PositionReset_Click);
            // 
            // tb_ResiveData
            // 
            this.tb_ResiveData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_ResiveData.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_ResiveData.Location = new System.Drawing.Point(42, 95);
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
            this.label3.Location = new System.Drawing.Point(7, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 25;
            this.label3.Text = "受信";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(7, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 26;
            this.label4.Text = "送信";
            // 
            // tb_SendData
            // 
            this.tb_SendData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_SendData.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_SendData.Location = new System.Drawing.Point(42, 66);
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
            this.label7.Location = new System.Drawing.Point(364, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 21);
            this.label7.TabIndex = 33;
            this.label7.Text = "SPEED";
            // 
            // tb_RESpeed
            // 
            this.tb_RESpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_RESpeed.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_RESpeed.Location = new System.Drawing.Point(449, 17);
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
            this.label8.Location = new System.Drawing.Point(550, 31);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 16);
            this.label8.TabIndex = 35;
            this.label8.Text = "km/h";
            // 
            // lbl_RErotL
            // 
            this.lbl_RErotL.AutoSize = true;
            this.lbl_RErotL.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_RErotL.Location = new System.Drawing.Point(58, 46);
            this.lbl_RErotL.Name = "lbl_RErotL";
            this.lbl_RErotL.Size = new System.Drawing.Size(28, 16);
            this.lbl_RErotL.TabIndex = 17;
            this.lbl_RErotL.Text = "ND";
            // 
            // lbl_RErotR
            // 
            this.lbl_RErotR.AutoSize = true;
            this.lbl_RErotR.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_RErotR.Location = new System.Drawing.Point(185, 46);
            this.lbl_RErotR.Name = "lbl_RErotR";
            this.lbl_RErotR.Size = new System.Drawing.Size(28, 16);
            this.lbl_RErotR.TabIndex = 16;
            this.lbl_RErotR.Text = "ND";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label17.Location = new System.Drawing.Point(9, 46);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(43, 16);
            this.label17.TabIndex = 15;
            this.label17.Text = "Re L:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label16.Location = new System.Drawing.Point(135, 46);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(44, 16);
            this.label16.TabIndex = 14;
            this.label16.Text = "Re R:";
            // 
            // lbl_REPlotDir
            // 
            this.lbl_REPlotDir.AutoSize = true;
            this.lbl_REPlotDir.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_REPlotDir.Location = new System.Drawing.Point(75, 125);
            this.lbl_REPlotDir.Name = "lbl_REPlotDir";
            this.lbl_REPlotDir.Size = new System.Drawing.Size(28, 16);
            this.lbl_REPlotDir.TabIndex = 13;
            this.lbl_REPlotDir.Text = "ND";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label18.Location = new System.Drawing.Point(9, 125);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(55, 16);
            this.label18.TabIndex = 12;
            this.label18.Text = "plotDir:";
            // 
            // lbl_REPlotY
            // 
            this.lbl_REPlotY.AutoSize = true;
            this.lbl_REPlotY.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_REPlotY.Location = new System.Drawing.Point(66, 101);
            this.lbl_REPlotY.Name = "lbl_REPlotY";
            this.lbl_REPlotY.Size = new System.Drawing.Size(28, 16);
            this.lbl_REPlotY.TabIndex = 11;
            this.lbl_REPlotY.Text = "ND";
            // 
            // lbl_REPlotX
            // 
            this.lbl_REPlotX.AutoSize = true;
            this.lbl_REPlotX.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_REPlotX.Location = new System.Drawing.Point(66, 76);
            this.lbl_REPlotX.Name = "lbl_REPlotX";
            this.lbl_REPlotX.Size = new System.Drawing.Size(28, 16);
            this.lbl_REPlotX.TabIndex = 10;
            this.lbl_REPlotX.Text = "ND";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label15.Location = new System.Drawing.Point(9, 101);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(45, 16);
            this.label15.TabIndex = 9;
            this.label15.Text = "plotY:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label14.Location = new System.Drawing.Point(9, 76);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(46, 16);
            this.label14.TabIndex = 8;
            this.label14.Text = "plotX:";
            // 
            // lbl_LED
            // 
            this.lbl_LED.AutoSize = true;
            this.lbl_LED.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_LED.Location = new System.Drawing.Point(58, 21);
            this.lbl_LED.Name = "lbl_LED";
            this.lbl_LED.Size = new System.Drawing.Size(28, 16);
            this.lbl_LED.TabIndex = 5;
            this.lbl_LED.Text = "ND";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.Location = new System.Drawing.Point(9, 21);
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
            // num_R1Dir
            // 
            this.num_R1Dir.Location = new System.Drawing.Point(62, 100);
            this.num_R1Dir.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.num_R1Dir.Minimum = new decimal(new int[] {
            3600,
            0,
            0,
            -2147483648});
            this.num_R1Dir.Name = "num_R1Dir";
            this.num_R1Dir.Size = new System.Drawing.Size(52, 19);
            this.num_R1Dir.TabIndex = 42;
            this.num_R1Dir.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_R1Y
            // 
            this.num_R1Y.Location = new System.Drawing.Point(27, 73);
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
            this.num_R1Y.Size = new System.Drawing.Size(88, 19);
            this.num_R1Y.TabIndex = 40;
            this.num_R1Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_R1X
            // 
            this.num_R1X.Location = new System.Drawing.Point(28, 51);
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
            this.num_R1X.Size = new System.Drawing.Size(87, 19);
            this.num_R1X.TabIndex = 39;
            this.num_R1X.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 105);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(31, 12);
            this.label22.TabIndex = 41;
            this.label22.Text = "角度:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(8, 75);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(14, 12);
            this.label20.TabIndex = 37;
            this.label20.Text = "Y:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(8, 53);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(14, 12);
            this.label19.TabIndex = 36;
            this.label19.Text = "X:";
            // 
            // cb_InDoorMode
            // 
            this.cb_InDoorMode.AutoSize = true;
            this.cb_InDoorMode.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_InDoorMode.Location = new System.Drawing.Point(610, 630);
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
            this.gb_DriveControl.Location = new System.Drawing.Point(2, 613);
            this.gb_DriveControl.Name = "gb_DriveControl";
            this.gb_DriveControl.Size = new System.Drawing.Size(600, 112);
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
            this.tb_Trip.Location = new System.Drawing.Point(449, 64);
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
            this.picbox_Indicator.Location = new System.Drawing.Point(607, 4);
            this.picbox_Indicator.Name = "picbox_Indicator";
            this.picbox_Indicator.Size = new System.Drawing.Size(240, 160);
            this.picbox_Indicator.TabIndex = 41;
            this.picbox_Indicator.TabStop = false;
            this.picbox_Indicator.Paint += new System.Windows.Forms.PaintEventHandler(this.picbox_Indicator_Paint);
            // 
            // cb_ConnectBServerEmu
            // 
            this.cb_ConnectBServerEmu.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_ConnectBServerEmu.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_ConnectBServerEmu.Location = new System.Drawing.Point(136, 13);
            this.cb_ConnectBServerEmu.Name = "cb_ConnectBServerEmu";
            this.cb_ConnectBServerEmu.Size = new System.Drawing.Size(89, 28);
            this.cb_ConnectBServerEmu.TabIndex = 45;
            this.cb_ConnectBServerEmu.Text = "Emu接続";
            this.cb_ConnectBServerEmu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_ConnectBServerEmu.UseVisualStyleBackColor = true;
            this.cb_ConnectBServerEmu.CheckedChanged += new System.EventHandler(this.cb_ConnectBServerEmu_CheckedChanged);
            // 
            // btn_MapLoad
            // 
            this.btn_MapLoad.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_MapLoad.Location = new System.Drawing.Point(120, 51);
            this.btn_MapLoad.Name = "btn_MapLoad";
            this.btn_MapLoad.Size = new System.Drawing.Size(108, 34);
            this.btn_MapLoad.TabIndex = 47;
            this.btn_MapLoad.Text = "MAP選択";
            this.btn_MapLoad.UseVisualStyleBackColor = true;
            this.btn_MapLoad.Click += new System.EventHandler(this.btn_MapLoad_Click);
            // 
            // tb_MapName
            // 
            this.tb_MapName.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_MapName.Location = new System.Drawing.Point(8, 22);
            this.tb_MapName.Name = "tb_MapName";
            this.tb_MapName.ReadOnly = true;
            this.tb_MapName.Size = new System.Drawing.Size(217, 23);
            this.tb_MapName.TabIndex = 48;
            this.tb_MapName.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tb_SendData);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tb_ResiveData);
            this.groupBox1.Controls.Add(this.lbl_bServerEmu);
            this.groupBox1.Controls.Add(this.cb_ConnectBServerEmu);
            this.groupBox1.Controls.Add(this.lb_BServerConnect);
            this.groupBox1.Location = new System.Drawing.Point(607, 308);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(240, 127);
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
            this.cb_AccelOff.Location = new System.Drawing.Point(734, 630);
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
            this.groupBox2.Controls.Add(this.num_R1Dir);
            this.groupBox2.Controls.Add(this.tb_MapName);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Controls.Add(this.num_R1Y);
            this.groupBox2.Controls.Add(this.btn_MapLoad);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.num_R1X);
            this.groupBox2.Controls.Add(this.btn_VRReset);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Location = new System.Drawing.Point(607, 170);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(240, 132);
            this.groupBox2.TabIndex = 51;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Map";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lbl_RErotL);
            this.groupBox3.Controls.Add(this.lbl_LED);
            this.groupBox3.Controls.Add(this.lbl_RErotR);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.lbl_REPlotDir);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.lbl_REPlotY);
            this.groupBox3.Controls.Add(this.lbl_REPlotX);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Location = new System.Drawing.Point(608, 446);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(239, 158);
            this.groupBox3.TabIndex = 52;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "状態";
            // 
            // VehicleRunnerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 729);
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
            this.Text = "VehicleRunner2 Ver0.20  Build:2017.04.01";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VehicleRunnerForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VehicleRunnerForm_FormClosed);
            this.Load += new System.EventHandler(this.VehicleRunnerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picbox_AreaMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_R1Dir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_R1Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_R1X)).EndInit();
            this.gb_DriveControl.ResumeLayout(false);
            this.gb_DriveControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_Indicator)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
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
        private System.Windows.Forms.NumericUpDown num_R1Dir;
        private System.Windows.Forms.NumericUpDown num_R1Y;
        private System.Windows.Forms.NumericUpDown num_R1X;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lbl_BackProcess;
        private System.Windows.Forms.Label lbl_bServerEmu;
        private System.Windows.Forms.CheckBox cb_InDoorMode;
        private System.Windows.Forms.CheckBox cb_ConnectBServerEmu;
        private System.Windows.Forms.Button btn_MapLoad;
        private System.Windows.Forms.TextBox tb_MapName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_Trip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cb_AccelOff;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}

