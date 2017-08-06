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
            this.cb_AccelOff = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBoxCheckPointModifi = new System.Windows.Forms.CheckBox();
            this.ButtonCheckPointReduction = new System.Windows.Forms.Button();
            this.ButtonCheckPointFileSave = new System.Windows.Forms.Button();
            this.numericUD_CheckPoint = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelMoveBaseAng = new System.Windows.Forms.Label();
            this.labelMoveBaseX = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lbl_CarName = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rb_SelAMCL = new System.Windows.Forms.RadioButton();
            this.rb_SelREPlot = new System.Windows.Forms.RadioButton();
            this.radioButtonPointAdd = new System.Windows.Forms.RadioButton();
            this.radioButtonPointMove = new System.Windows.Forms.RadioButton();
            this.radioButtonPointDelete = new System.Windows.Forms.RadioButton();
            this.dataGridViewReceiveData = new System.Windows.Forms.DataGridView();
            this.ColumnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numericUpDownCtrlSpeed = new System.Windows.Forms.NumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_AreaMap)).BeginInit();
            this.gb_DriveControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_Indicator)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUD_CheckPoint)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReceiveData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCtrlSpeed)).BeginInit();
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
            this.btn_VRReset.Location = new System.Drawing.Point(233, 53);
            this.btn_VRReset.Name = "btn_VRReset";
            this.btn_VRReset.Size = new System.Drawing.Size(79, 25);
            this.btn_VRReset.TabIndex = 12;
            this.btn_VRReset.Text = "リスタート";
            this.btn_VRReset.UseVisualStyleBackColor = true;
            this.btn_VRReset.Click += new System.EventHandler(this.btn_PositionReset_Click);
            // 
            // tb_ResiveData
            // 
            this.tb_ResiveData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_ResiveData.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_ResiveData.Location = new System.Drawing.Point(42, 64);
            this.tb_ResiveData.Name = "tb_ResiveData";
            this.tb_ResiveData.ReadOnly = true;
            this.tb_ResiveData.Size = new System.Drawing.Size(262, 19);
            this.tb_ResiveData.TabIndex = 24;
            this.tb_ResiveData.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(11, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 25;
            this.label3.Text = "受信";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(11, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 26;
            this.label4.Text = "送信";
            // 
            // tb_SendData
            // 
            this.tb_SendData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_SendData.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_SendData.Location = new System.Drawing.Point(42, 42);
            this.tb_SendData.Name = "tb_SendData";
            this.tb_SendData.ReadOnly = true;
            this.tb_SendData.Size = new System.Drawing.Size(262, 19);
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
            this.label7.Location = new System.Drawing.Point(367, 59);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 21);
            this.label7.TabIndex = 33;
            this.label7.Text = "SPEED";
            // 
            // tb_RESpeed
            // 
            this.tb_RESpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_RESpeed.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_RESpeed.Location = new System.Drawing.Point(452, 54);
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
            this.label8.Location = new System.Drawing.Point(553, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 16);
            this.label8.TabIndex = 35;
            this.label8.Text = "km/h";
            // 
            // lbl_RErotL
            // 
            this.lbl_RErotL.AutoSize = true;
            this.lbl_RErotL.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_RErotL.Location = new System.Drawing.Point(58, 63);
            this.lbl_RErotL.Name = "lbl_RErotL";
            this.lbl_RErotL.Size = new System.Drawing.Size(28, 16);
            this.lbl_RErotL.TabIndex = 17;
            this.lbl_RErotL.Text = "ND";
            // 
            // lbl_RErotR
            // 
            this.lbl_RErotR.AutoSize = true;
            this.lbl_RErotR.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_RErotR.Location = new System.Drawing.Point(185, 63);
            this.lbl_RErotR.Name = "lbl_RErotR";
            this.lbl_RErotR.Size = new System.Drawing.Size(28, 16);
            this.lbl_RErotR.TabIndex = 16;
            this.lbl_RErotR.Text = "ND";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label17.Location = new System.Drawing.Point(9, 63);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(43, 16);
            this.label17.TabIndex = 15;
            this.label17.Text = "Re L:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label16.Location = new System.Drawing.Point(135, 63);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(44, 16);
            this.label16.TabIndex = 14;
            this.label16.Text = "Re R:";
            // 
            // lbl_REPlotDir
            // 
            this.lbl_REPlotDir.AutoSize = true;
            this.lbl_REPlotDir.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_REPlotDir.Location = new System.Drawing.Point(76, 133);
            this.lbl_REPlotDir.Name = "lbl_REPlotDir";
            this.lbl_REPlotDir.Size = new System.Drawing.Size(28, 16);
            this.lbl_REPlotDir.TabIndex = 13;
            this.lbl_REPlotDir.Text = "ND";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label18.Location = new System.Drawing.Point(10, 133);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(55, 16);
            this.label18.TabIndex = 12;
            this.label18.Text = "plotDir:";
            // 
            // lbl_REPlotY
            // 
            this.lbl_REPlotY.AutoSize = true;
            this.lbl_REPlotY.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_REPlotY.Location = new System.Drawing.Point(67, 109);
            this.lbl_REPlotY.Name = "lbl_REPlotY";
            this.lbl_REPlotY.Size = new System.Drawing.Size(28, 16);
            this.lbl_REPlotY.TabIndex = 11;
            this.lbl_REPlotY.Text = "ND";
            // 
            // lbl_REPlotX
            // 
            this.lbl_REPlotX.AutoSize = true;
            this.lbl_REPlotX.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_REPlotX.Location = new System.Drawing.Point(67, 84);
            this.lbl_REPlotX.Name = "lbl_REPlotX";
            this.lbl_REPlotX.Size = new System.Drawing.Size(28, 16);
            this.lbl_REPlotX.TabIndex = 10;
            this.lbl_REPlotX.Text = "ND";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label15.Location = new System.Drawing.Point(10, 109);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(45, 16);
            this.label15.TabIndex = 9;
            this.label15.Text = "plotY:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label14.Location = new System.Drawing.Point(10, 84);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(46, 16);
            this.label14.TabIndex = 8;
            this.label14.Text = "plotX:";
            // 
            // lbl_LED
            // 
            this.lbl_LED.AutoSize = true;
            this.lbl_LED.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_LED.Location = new System.Drawing.Point(194, 8);
            this.lbl_LED.Name = "lbl_LED";
            this.lbl_LED.Size = new System.Drawing.Size(28, 16);
            this.lbl_LED.TabIndex = 5;
            this.lbl_LED.Text = "ND";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.Location = new System.Drawing.Point(152, 8);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 16);
            this.label10.TabIndex = 0;
            this.label10.Text = "LED:";
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
            this.lb_BServerConnect.Location = new System.Drawing.Point(10, 20);
            this.lb_BServerConnect.Name = "lb_BServerConnect";
            this.lb_BServerConnect.Size = new System.Drawing.Size(103, 16);
            this.lb_BServerConnect.TabIndex = 23;
            this.lb_BServerConnect.Text = "bServer接続";
            // 
            // gb_DriveControl
            // 
            this.gb_DriveControl.Controls.Add(this.label20);
            this.gb_DriveControl.Controls.Add(this.label19);
            this.gb_DriveControl.Controls.Add(this.numericUpDownCtrlSpeed);
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
            this.gb_DriveControl.Location = new System.Drawing.Point(2, 510);
            this.gb_DriveControl.Name = "gb_DriveControl";
            this.gb_DriveControl.Size = new System.Drawing.Size(600, 132);
            this.gb_DriveControl.TabIndex = 40;
            this.gb_DriveControl.TabStop = false;
            this.gb_DriveControl.Text = "DriveControl";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(553, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 16);
            this.label2.TabIndex = 44;
            this.label2.Text = "m";
            // 
            // tb_Trip
            // 
            this.tb_Trip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_Trip.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_Trip.Location = new System.Drawing.Point(452, 95);
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
            this.label1.Location = new System.Drawing.Point(367, 98);
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
            this.btn_MapLoad.Location = new System.Drawing.Point(267, 21);
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
            this.tb_MapName.Size = new System.Drawing.Size(253, 23);
            this.tb_MapName.TabIndex = 48;
            this.tb_MapName.TabStop = false;
            // 
            // cb_AccelOff
            // 
            this.cb_AccelOff.AutoSize = true;
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
            this.groupBox2.Controls.Add(this.numericUD_CheckPoint);
            this.groupBox2.Controls.Add(this.checkBoxCheckPointModifi);
            this.groupBox2.Controls.Add(this.btn_VRReset);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.tb_MapName);
            this.groupBox2.Controls.Add(this.btn_MapLoad);
            this.groupBox2.Location = new System.Drawing.Point(606, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(312, 206);
            this.groupBox2.TabIndex = 51;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Map";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButtonPointDelete);
            this.groupBox4.Controls.Add(this.radioButtonPointMove);
            this.groupBox4.Controls.Add(this.radioButtonPointAdd);
            this.groupBox4.Controls.Add(this.ButtonCheckPointReduction);
            this.groupBox4.Controls.Add(this.ButtonCheckPointFileSave);
            this.groupBox4.Location = new System.Drawing.Point(8, 109);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(298, 84);
            this.groupBox4.TabIndex = 52;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "チェックポイント編集";
            // 
            // checkBoxCheckPointModifi
            // 
            this.checkBoxCheckPointModifi.AutoSize = true;
            this.checkBoxCheckPointModifi.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.checkBoxCheckPointModifi.Location = new System.Drawing.Point(8, 86);
            this.checkBoxCheckPointModifi.Name = "checkBoxCheckPointModifi";
            this.checkBoxCheckPointModifi.Size = new System.Drawing.Size(82, 17);
            this.checkBoxCheckPointModifi.TabIndex = 49;
            this.checkBoxCheckPointModifi.Text = "編集モード";
            this.checkBoxCheckPointModifi.UseVisualStyleBackColor = true;
            // 
            // ButtonCheckPointReduction
            // 
            this.ButtonCheckPointReduction.Location = new System.Drawing.Point(140, 51);
            this.ButtonCheckPointReduction.Name = "ButtonCheckPointReduction";
            this.ButtonCheckPointReduction.Size = new System.Drawing.Size(75, 23);
            this.ButtonCheckPointReduction.TabIndex = 51;
            this.ButtonCheckPointReduction.Text = "削減";
            this.ButtonCheckPointReduction.UseVisualStyleBackColor = true;
            // 
            // ButtonCheckPointFileSave
            // 
            this.ButtonCheckPointFileSave.Location = new System.Drawing.Point(236, 51);
            this.ButtonCheckPointFileSave.Name = "ButtonCheckPointFileSave";
            this.ButtonCheckPointFileSave.Size = new System.Drawing.Size(56, 23);
            this.ButtonCheckPointFileSave.TabIndex = 50;
            this.ButtonCheckPointFileSave.Text = "保存";
            this.ButtonCheckPointFileSave.UseVisualStyleBackColor = true;
            this.ButtonCheckPointFileSave.Click += new System.EventHandler(this.ButtonMapSave_Click);
            // 
            // numericUD_CheckPoint
            // 
            this.numericUD_CheckPoint.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numericUD_CheckPoint.Location = new System.Drawing.Point(152, 53);
            this.numericUD_CheckPoint.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUD_CheckPoint.Name = "numericUD_CheckPoint";
            this.numericUD_CheckPoint.Size = new System.Drawing.Size(75, 23);
            this.numericUD_CheckPoint.TabIndex = 44;
            this.numericUD_CheckPoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUD_CheckPoint.Click += new System.EventHandler(this.numericUD_CheckPoint_Click);
            this.numericUD_CheckPoint.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numericUD_CheckPoint_KeyPress);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label11.Location = new System.Drawing.Point(10, 55);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(88, 16);
            this.label11.TabIndex = 43;
            this.label11.Text = "CheckPoint:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridViewReceiveData);
            this.groupBox3.Controls.Add(this.tb_SendData);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.panel1);
            this.groupBox3.Controls.Add(this.lb_BServerConnect);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.tb_ResiveData);
            this.groupBox3.Location = new System.Drawing.Point(606, 216);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(312, 333);
            this.groupBox3.TabIndex = 52;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "ROS Status";
            // 
            // labelMoveBaseAng
            // 
            this.labelMoveBaseAng.AutoSize = true;
            this.labelMoveBaseAng.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelMoveBaseAng.Location = new System.Drawing.Point(185, 38);
            this.labelMoveBaseAng.Name = "labelMoveBaseAng";
            this.labelMoveBaseAng.Size = new System.Drawing.Size(28, 16);
            this.labelMoveBaseAng.TabIndex = 56;
            this.labelMoveBaseAng.Text = "ND";
            // 
            // labelMoveBaseX
            // 
            this.labelMoveBaseX.AutoSize = true;
            this.labelMoveBaseX.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelMoveBaseX.Location = new System.Drawing.Point(95, 38);
            this.labelMoveBaseX.Name = "labelMoveBaseX";
            this.labelMoveBaseX.Size = new System.Drawing.Size(28, 16);
            this.labelMoveBaseX.TabIndex = 55;
            this.labelMoveBaseX.Text = "ND";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label12.Location = new System.Drawing.Point(9, 38);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 16);
            this.label12.TabIndex = 54;
            this.label12.Text = "moveBase:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label13.Location = new System.Drawing.Point(10, 8);
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
            this.lbl_CarName.Location = new System.Drawing.Point(85, 8);
            this.lbl_CarName.Name = "lbl_CarName";
            this.lbl_CarName.Size = new System.Drawing.Size(45, 16);
            this.lbl_CarName.TabIndex = 45;
            this.lbl_CarName.Text = "Benz";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.labelMoveBaseAng);
            this.panel1.Controls.Add(this.rb_SelAMCL);
            this.panel1.Controls.Add(this.rb_SelREPlot);
            this.panel1.Controls.Add(this.labelMoveBaseX);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.lbl_REPlotDir);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.lbl_REPlotX);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.lbl_CarName);
            this.panel1.Controls.Add(this.lbl_REPlotY);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.lbl_RErotL);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.lbl_LED);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.lbl_RErotR);
            this.panel1.Location = new System.Drawing.Point(6, 89);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 227);
            this.panel1.TabIndex = 53;
            // 
            // rb_SelAMCL
            // 
            this.rb_SelAMCL.AutoSize = true;
            this.rb_SelAMCL.Checked = true;
            this.rb_SelAMCL.Location = new System.Drawing.Point(168, 133);
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
            this.rb_SelREPlot.Location = new System.Drawing.Point(247, 133);
            this.rb_SelREPlot.Name = "rb_SelREPlot";
            this.rb_SelREPlot.Size = new System.Drawing.Size(58, 16);
            this.rb_SelREPlot.TabIndex = 48;
            this.rb_SelREPlot.Text = "REPlot";
            this.rb_SelREPlot.UseVisualStyleBackColor = true;
            // 
            // radioButtonPointAdd
            // 
            this.radioButtonPointAdd.AutoSize = true;
            this.radioButtonPointAdd.Location = new System.Drawing.Point(11, 18);
            this.radioButtonPointAdd.Name = "radioButtonPointAdd";
            this.radioButtonPointAdd.Size = new System.Drawing.Size(71, 16);
            this.radioButtonPointAdd.TabIndex = 52;
            this.radioButtonPointAdd.TabStop = true;
            this.radioButtonPointAdd.Text = "新規追加";
            this.radioButtonPointAdd.UseVisualStyleBackColor = true;
            // 
            // radioButtonPointMove
            // 
            this.radioButtonPointMove.AutoSize = true;
            this.radioButtonPointMove.Location = new System.Drawing.Point(11, 38);
            this.radioButtonPointMove.Name = "radioButtonPointMove";
            this.radioButtonPointMove.Size = new System.Drawing.Size(47, 16);
            this.radioButtonPointMove.TabIndex = 53;
            this.radioButtonPointMove.TabStop = true;
            this.radioButtonPointMove.Text = "移動";
            this.radioButtonPointMove.UseVisualStyleBackColor = true;
            // 
            // radioButtonPointDelete
            // 
            this.radioButtonPointDelete.AutoSize = true;
            this.radioButtonPointDelete.Location = new System.Drawing.Point(11, 58);
            this.radioButtonPointDelete.Name = "radioButtonPointDelete";
            this.radioButtonPointDelete.Size = new System.Drawing.Size(47, 16);
            this.radioButtonPointDelete.TabIndex = 54;
            this.radioButtonPointDelete.TabStop = true;
            this.radioButtonPointDelete.Text = "削除";
            this.radioButtonPointDelete.UseVisualStyleBackColor = true;
            // 
            // dataGridViewReceiveData
            // 
            this.dataGridViewReceiveData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewReceiveData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnType,
            this.Column1,
            this.Column2});
            this.dataGridViewReceiveData.Location = new System.Drawing.Point(6, 99);
            this.dataGridViewReceiveData.Name = "dataGridViewReceiveData";
            this.dataGridViewReceiveData.RowTemplate.Height = 21;
            this.dataGridViewReceiveData.Size = new System.Drawing.Size(297, 206);
            this.dataGridViewReceiveData.TabIndex = 28;
            this.dataGridViewReceiveData.Visible = false;
            // 
            // ColumnType
            // 
            this.ColumnType.HeaderText = "型";
            this.ColumnType.Name = "ColumnType";
            // 
            // Column1
            // 
            this.Column1.HeaderText = "x";
            this.Column1.Name = "Column1";
            this.Column1.Width = 60;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "y";
            this.Column2.Name = "Column2";
            this.Column2.Width = 60;
            // 
            // numericUpDownCtrlSpeed
            // 
            this.numericUpDownCtrlSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownCtrlSpeed.DecimalPlaces = 1;
            this.numericUpDownCtrlSpeed.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numericUpDownCtrlSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownCtrlSpeed.Location = new System.Drawing.Point(452, 18);
            this.numericUpDownCtrlSpeed.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDownCtrlSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownCtrlSpeed.Name = "numericUpDownCtrlSpeed";
            this.numericUpDownCtrlSpeed.Size = new System.Drawing.Size(95, 31);
            this.numericUpDownCtrlSpeed.TabIndex = 45;
            this.numericUpDownCtrlSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownCtrlSpeed.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label19.Location = new System.Drawing.Point(553, 27);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(43, 16);
            this.label19.TabIndex = 46;
            this.label19.Text = "km/h";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label20.Location = new System.Drawing.Point(368, 30);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(84, 13);
            this.label20.TabIndex = 47;
            this.label20.Text = "ControlSpeed";
            // 
            // VehicleRunnerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 649);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.cb_AccelOff);
            this.Controls.Add(this.cb_StartAutonomous);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cb_InDoorMode);
            this.Controls.Add(this.picbox_Indicator);
            this.Controls.Add(this.gb_DriveControl);
            this.Controls.Add(this.picbox_AreaMap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "VehicleRunnerForm";
            this.Text = "VehicleRunner2 Ver0.30  Build:2017.07.25";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VehicleRunnerForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VehicleRunnerForm_FormClosed);
            this.Load += new System.EventHandler(this.VehicleRunnerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picbox_AreaMap)).EndInit();
            this.gb_DriveControl.ResumeLayout(false);
            this.gb_DriveControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_Indicator)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUD_CheckPoint)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReceiveData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCtrlSpeed)).EndInit();
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
        private System.Windows.Forms.CheckBox cb_InDoorMode;
        private System.Windows.Forms.Button btn_MapLoad;
        private System.Windows.Forms.TextBox tb_MapName;
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
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ButtonCheckPointFileSave;
        private System.Windows.Forms.CheckBox checkBoxCheckPointModifi;
        private System.Windows.Forms.Button ButtonCheckPointReduction;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label labelMoveBaseAng;
        private System.Windows.Forms.Label labelMoveBaseX;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RadioButton radioButtonPointDelete;
        private System.Windows.Forms.RadioButton radioButtonPointMove;
        private System.Windows.Forms.RadioButton radioButtonPointAdd;
        private System.Windows.Forms.DataGridView dataGridViewReceiveData;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown numericUpDownCtrlSpeed;
        private System.Windows.Forms.Label label20;
    }
}

