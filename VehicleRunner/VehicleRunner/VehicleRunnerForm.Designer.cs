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
            this.cb_TimerUpdate = new System.Windows.Forms.CheckBox();
            this.btm_LRFScale = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_LRFScale = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_PositionReset = new System.Windows.Forms.Button();
            this.cb_RotEnc = new System.Windows.Forms.CheckBox();
            this.cb_EleConpus = new System.Windows.Forms.CheckBox();
            this.tb_LRFIpAddr = new System.Windows.Forms.TextBox();
            this.tb_LRFPort = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
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
            this.lbl_EmurateMode = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_RESpeed = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage_Runner = new System.Windows.Forms.TabPage();
            this.cb_StraightMode = new System.Windows.Forms.CheckBox();
            this.btn_LocRevision = new System.Windows.Forms.Button();
            this.lbl_MattingScore = new System.Windows.Forms.Label();
            this.tabPage_LocSump = new System.Windows.Forms.TabPage();
            this.cb_TimeRivision = new System.Windows.Forms.CheckBox();
            this.cb_RivisionPF = new System.Windows.Forms.CheckBox();
            this.cb_LocationPresumption = new System.Windows.Forms.CheckBox();
            this.cb_RivisonGPS = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_SirialConnect = new System.Windows.Forms.CheckBox();
            this.cb_UsbSirial = new System.Windows.Forms.ComboBox();
            this.tb_SirialResive = new System.Windows.Forms.TextBox();
            this.tabPage_Status = new System.Windows.Forms.TabPage();
            this.lbl_RErotL = new System.Windows.Forms.Label();
            this.lbl_RErotR = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lbl_REDir = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lbl_REY = new System.Windows.Forms.Label();
            this.lbl_REX = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lbl_Compass = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lbl_LED = new System.Windows.Forms.Label();
            this.lbl_GPS_X = new System.Windows.Forms.Label();
            this.lbl_GPS_Y = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tabPage_Emulate = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.cb_EHS = new System.Windows.Forms.CheckBox();
            this.tm_SendCom = new System.Windows.Forms.Timer(this.components);
            this.lbl_BackCnt = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_AreaMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_LRF)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage_Runner.SuspendLayout();
            this.tabPage_LocSump.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage_Status.SuspendLayout();
            this.tabPage_Emulate.SuspendLayout();
            this.SuspendLayout();
            // 
            // picbox_AreaMap
            // 
            this.picbox_AreaMap.BackColor = System.Drawing.Color.White;
            this.picbox_AreaMap.Location = new System.Drawing.Point(12, 12);
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
            this.picbox_LRF.Location = new System.Drawing.Point(626, 27);
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
            this.cb_LRFConnect.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_LRFConnect.Location = new System.Drawing.Point(6, 57);
            this.cb_LRFConnect.Name = "cb_LRFConnect";
            this.cb_LRFConnect.Size = new System.Drawing.Size(135, 49);
            this.cb_LRFConnect.TabIndex = 3;
            this.cb_LRFConnect.Text = "LRF Connect";
            this.cb_LRFConnect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_LRFConnect.UseVisualStyleBackColor = true;
            this.cb_LRFConnect.CheckedChanged += new System.EventHandler(this.cb_LRFConnect_CheckedChanged);
            // 
            // lb_LRFResult
            // 
            this.lb_LRFResult.AutoSize = true;
            this.lb_LRFResult.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb_LRFResult.Location = new System.Drawing.Point(148, 73);
            this.lb_LRFResult.Name = "lb_LRFResult";
            this.lb_LRFResult.Size = new System.Drawing.Size(56, 16);
            this.lb_LRFResult.TabIndex = 4;
            this.lb_LRFResult.Text = "Result";
            // 
            // cb_TimerUpdate
            // 
            this.cb_TimerUpdate.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_TimerUpdate.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_TimerUpdate.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_TimerUpdate.Location = new System.Drawing.Point(626, 557);
            this.cb_TimerUpdate.Name = "cb_TimerUpdate";
            this.cb_TimerUpdate.Size = new System.Drawing.Size(127, 53);
            this.cb_TimerUpdate.TabIndex = 5;
            this.cb_TimerUpdate.Text = "START";
            this.cb_TimerUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_TimerUpdate.UseVisualStyleBackColor = true;
            this.cb_TimerUpdate.CheckedChanged += new System.EventHandler(this.cb_TimerUpdate_CheckedChanged);
            this.cb_TimerUpdate.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cb_TimerUpdate_PreviewKeyDown);
            // 
            // btm_LRFScale
            // 
            this.btm_LRFScale.Location = new System.Drawing.Point(180, 8);
            this.btm_LRFScale.Name = "btm_LRFScale";
            this.btm_LRFScale.Size = new System.Drawing.Size(49, 27);
            this.btm_LRFScale.TabIndex = 8;
            this.btm_LRFScale.Text = "Set";
            this.btm_LRFScale.UseVisualStyleBackColor = true;
            this.btm_LRFScale.Click += new System.EventHandler(this.btm_LRFScale_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(139, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "mm";
            // 
            // tb_LRFScale
            // 
            this.tb_LRFScale.Location = new System.Drawing.Point(70, 12);
            this.tb_LRFScale.Name = "tb_LRFScale";
            this.tb_LRFScale.Size = new System.Drawing.Size(63, 19);
            this.tb_LRFScale.TabIndex = 10;
            this.tb_LRFScale.Text = "1000";
            this.tb_LRFScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tb_LRFScale.TextChanged += new System.EventHandler(this.tb_LRFScale_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "LRF Scale";
            // 
            // btn_PositionReset
            // 
            this.btn_PositionReset.Location = new System.Drawing.Point(132, 50);
            this.btn_PositionReset.Name = "btn_PositionReset";
            this.btn_PositionReset.Size = new System.Drawing.Size(80, 29);
            this.btn_PositionReset.TabIndex = 12;
            this.btn_PositionReset.Text = "位置リセット";
            this.btn_PositionReset.UseVisualStyleBackColor = true;
            this.btn_PositionReset.Click += new System.EventHandler(this.btn_PositionReset_Click);
            // 
            // cb_RotEnc
            // 
            this.cb_RotEnc.AutoSize = true;
            this.cb_RotEnc.Checked = true;
            this.cb_RotEnc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_RotEnc.Location = new System.Drawing.Point(6, 48);
            this.cb_RotEnc.Name = "cb_RotEnc";
            this.cb_RotEnc.Size = new System.Drawing.Size(77, 16);
            this.cb_RotEnc.TabIndex = 13;
            this.cb_RotEnc.Text = "Rエンコーダ";
            this.cb_RotEnc.UseVisualStyleBackColor = true;
            // 
            // cb_EleConpus
            // 
            this.cb_EleConpus.AutoSize = true;
            this.cb_EleConpus.Checked = true;
            this.cb_EleConpus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_EleConpus.Location = new System.Drawing.Point(6, 72);
            this.cb_EleConpus.Name = "cb_EleConpus";
            this.cb_EleConpus.Size = new System.Drawing.Size(84, 16);
            this.cb_EleConpus.TabIndex = 14;
            this.cb_EleConpus.Text = "電子コンパス";
            this.cb_EleConpus.UseVisualStyleBackColor = true;
            // 
            // tb_LRFIpAddr
            // 
            this.tb_LRFIpAddr.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_LRFIpAddr.Location = new System.Drawing.Point(6, 23);
            this.tb_LRFIpAddr.Name = "tb_LRFIpAddr";
            this.tb_LRFIpAddr.Size = new System.Drawing.Size(135, 23);
            this.tb_LRFIpAddr.TabIndex = 15;
            this.tb_LRFIpAddr.Text = "192.168.1.10";
            // 
            // tb_LRFPort
            // 
            this.tb_LRFPort.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_LRFPort.Location = new System.Drawing.Point(151, 23);
            this.tb_LRFPort.Name = "tb_LRFPort";
            this.tb_LRFPort.Size = new System.Drawing.Size(63, 23);
            this.tb_LRFPort.TabIndex = 16;
            this.tb_LRFPort.Text = "10940";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_EleConpus);
            this.groupBox1.Controls.Add(this.cb_RotEnc);
            this.groupBox1.Controls.Add(this.btn_PositionReset);
            this.groupBox1.Location = new System.Drawing.Point(11, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(218, 94);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "自己位置推定";
            // 
            // cb_EmgBrake
            // 
            this.cb_EmgBrake.AutoSize = true;
            this.cb_EmgBrake.Checked = true;
            this.cb_EmgBrake.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_EmgBrake.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_EmgBrake.Location = new System.Drawing.Point(773, 557);
            this.cb_EmgBrake.Name = "cb_EmgBrake";
            this.cb_EmgBrake.Size = new System.Drawing.Size(95, 20);
            this.cb_EmgBrake.TabIndex = 19;
            this.cb_EmgBrake.Text = "緊急停止";
            this.cb_EmgBrake.UseVisualStyleBackColor = true;
            this.cb_EmgBrake.CheckedChanged += new System.EventHandler(this.cb_EmgBrake_CheckedChanged);
            // 
            // tm_UpdateHw
            // 
            this.tm_UpdateHw.Interval = 50;
            this.tm_UpdateHw.Tick += new System.EventHandler(this.tm_UpdateHw_Tick);
            // 
            // tb_ResiveData
            // 
            this.tb_ResiveData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_ResiveData.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_ResiveData.Location = new System.Drawing.Point(55, 655);
            this.tb_ResiveData.Name = "tb_ResiveData";
            this.tb_ResiveData.ReadOnly = true;
            this.tb_ResiveData.Size = new System.Drawing.Size(469, 23);
            this.tb_ResiveData.TabIndex = 24;
            this.tb_ResiveData.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(10, 661);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 25;
            this.label3.Text = "受信";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 631);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 12);
            this.label4.TabIndex = 26;
            this.label4.Text = "toBoxPC 送信";
            // 
            // tb_SendData
            // 
            this.tb_SendData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_SendData.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_SendData.Location = new System.Drawing.Point(94, 625);
            this.tb_SendData.Name = "tb_SendData";
            this.tb_SendData.ReadOnly = true;
            this.tb_SendData.Size = new System.Drawing.Size(430, 23);
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
            this.label5.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(733, 623);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 24);
            this.label5.TabIndex = 28;
            this.label5.Text = "ACC";
            // 
            // tb_AccelVal
            // 
            this.tb_AccelVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_AccelVal.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_AccelVal.Location = new System.Drawing.Point(799, 620);
            this.tb_AccelVal.Name = "tb_AccelVal";
            this.tb_AccelVal.ReadOnly = true;
            this.tb_AccelVal.Size = new System.Drawing.Size(73, 31);
            this.tb_AccelVal.TabIndex = 29;
            this.tb_AccelVal.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(582, 620);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 24);
            this.label6.TabIndex = 30;
            this.label6.Text = "HDL";
            // 
            // tb_HandleVal
            // 
            this.tb_HandleVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_HandleVal.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_HandleVal.Location = new System.Drawing.Point(645, 620);
            this.tb_HandleVal.Name = "tb_HandleVal";
            this.tb_HandleVal.ReadOnly = true;
            this.tb_HandleVal.Size = new System.Drawing.Size(73, 31);
            this.tb_HandleVal.TabIndex = 31;
            this.tb_HandleVal.TabStop = false;
            // 
            // lbl_EmurateMode
            // 
            this.lbl_EmurateMode.AutoSize = true;
            this.lbl_EmurateMode.BackColor = System.Drawing.Color.Red;
            this.lbl_EmurateMode.Location = new System.Drawing.Point(634, 542);
            this.lbl_EmurateMode.Name = "lbl_EmurateMode";
            this.lbl_EmurateMode.Size = new System.Drawing.Size(100, 12);
            this.lbl_EmurateMode.TabIndex = 32;
            this.lbl_EmurateMode.Text = "エミュレーションMode";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(530, 654);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 24);
            this.label7.TabIndex = 33;
            this.label7.Text = "SPD";
            // 
            // tb_RESpeed
            // 
            this.tb_RESpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_RESpeed.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_RESpeed.Location = new System.Drawing.Point(594, 652);
            this.tb_RESpeed.Name = "tb_RESpeed";
            this.tb_RESpeed.ReadOnly = true;
            this.tb_RESpeed.Size = new System.Drawing.Size(73, 31);
            this.tb_RESpeed.TabIndex = 34;
            this.tb_RESpeed.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(673, 671);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 35;
            this.label8.Text = "mm/sec";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage_Runner);
            this.tabControl.Controls.Add(this.tabPage_LocSump);
            this.tabControl.Controls.Add(this.tabPage_Status);
            this.tabControl.Controls.Add(this.tabPage_Emulate);
            this.tabControl.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tabControl.Location = new System.Drawing.Point(626, 273);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(246, 264);
            this.tabControl.TabIndex = 36;
            // 
            // tabPage_Runner
            // 
            this.tabPage_Runner.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_Runner.Controls.Add(this.cb_StraightMode);
            this.tabPage_Runner.Controls.Add(this.btn_LocRevision);
            this.tabPage_Runner.Controls.Add(this.lbl_MattingScore);
            this.tabPage_Runner.Controls.Add(this.lb_LRFResult);
            this.tabPage_Runner.Controls.Add(this.tb_LRFPort);
            this.tabPage_Runner.Controls.Add(this.cb_LRFConnect);
            this.tabPage_Runner.Controls.Add(this.tb_LRFIpAddr);
            this.tabPage_Runner.Location = new System.Drawing.Point(4, 26);
            this.tabPage_Runner.Name = "tabPage_Runner";
            this.tabPage_Runner.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Runner.Size = new System.Drawing.Size(238, 234);
            this.tabPage_Runner.TabIndex = 0;
            this.tabPage_Runner.Text = "Runner";
            // 
            // cb_StraightMode
            // 
            this.cb_StraightMode.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_StraightMode.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_StraightMode.Location = new System.Drawing.Point(6, 170);
            this.cb_StraightMode.Name = "cb_StraightMode";
            this.cb_StraightMode.Size = new System.Drawing.Size(135, 36);
            this.cb_StraightMode.TabIndex = 20;
            this.cb_StraightMode.Text = "直進ルートセット";
            this.cb_StraightMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_StraightMode.UseVisualStyleBackColor = true;
            this.cb_StraightMode.CheckedChanged += new System.EventHandler(this.cb_StraightMode_CheckedChanged);
            // 
            // btn_LocRevision
            // 
            this.btn_LocRevision.Location = new System.Drawing.Point(6, 121);
            this.btn_LocRevision.Name = "btn_LocRevision";
            this.btn_LocRevision.Size = new System.Drawing.Size(117, 43);
            this.btn_LocRevision.TabIndex = 18;
            this.btn_LocRevision.Text = "位置補正";
            this.btn_LocRevision.UseVisualStyleBackColor = true;
            this.btn_LocRevision.Click += new System.EventHandler(this.btn_LocRevision_Click);
            // 
            // lbl_MattingScore
            // 
            this.lbl_MattingScore.AutoSize = true;
            this.lbl_MattingScore.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_MattingScore.Location = new System.Drawing.Point(12, 209);
            this.lbl_MattingScore.Name = "lbl_MattingScore";
            this.lbl_MattingScore.Size = new System.Drawing.Size(111, 16);
            this.lbl_MattingScore.TabIndex = 17;
            this.lbl_MattingScore.Text = "MatchingScore:";
            // 
            // tabPage_LocSump
            // 
            this.tabPage_LocSump.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_LocSump.Controls.Add(this.cb_TimeRivision);
            this.tabPage_LocSump.Controls.Add(this.cb_RivisionPF);
            this.tabPage_LocSump.Controls.Add(this.cb_LocationPresumption);
            this.tabPage_LocSump.Controls.Add(this.cb_RivisonGPS);
            this.tabPage_LocSump.Controls.Add(this.groupBox2);
            this.tabPage_LocSump.Location = new System.Drawing.Point(4, 26);
            this.tabPage_LocSump.Name = "tabPage_LocSump";
            this.tabPage_LocSump.Size = new System.Drawing.Size(238, 234);
            this.tabPage_LocSump.TabIndex = 3;
            this.tabPage_LocSump.Text = "補正";
            // 
            // cb_TimeRivision
            // 
            this.cb_TimeRivision.AutoSize = true;
            this.cb_TimeRivision.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_TimeRivision.Location = new System.Drawing.Point(13, 138);
            this.cb_TimeRivision.Name = "cb_TimeRivision";
            this.cb_TimeRivision.Size = new System.Drawing.Size(115, 17);
            this.cb_TimeRivision.TabIndex = 30;
            this.cb_TimeRivision.Text = "一定間隔で補正";
            this.cb_TimeRivision.UseVisualStyleBackColor = true;
            this.cb_TimeRivision.CheckedChanged += new System.EventHandler(this.cb_TimeRivision_CheckedChanged);
            // 
            // cb_RivisionPF
            // 
            this.cb_RivisionPF.AutoSize = true;
            this.cb_RivisionPF.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_RivisionPF.Location = new System.Drawing.Point(13, 178);
            this.cb_RivisionPF.Name = "cb_RivisionPF";
            this.cb_RivisionPF.Size = new System.Drawing.Size(108, 17);
            this.cb_RivisionPF.TabIndex = 29;
            this.cb_RivisionPF.Text = "PFを補正に使う";
            this.cb_RivisionPF.UseVisualStyleBackColor = true;
            this.cb_RivisionPF.CheckedChanged += new System.EventHandler(this.cb_RivisionPF_CheckedChanged);
            // 
            // cb_LocationPresumption
            // 
            this.cb_LocationPresumption.AutoSize = true;
            this.cb_LocationPresumption.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_LocationPresumption.Location = new System.Drawing.Point(13, 201);
            this.cb_LocationPresumption.Name = "cb_LocationPresumption";
            this.cb_LocationPresumption.Size = new System.Drawing.Size(152, 17);
            this.cb_LocationPresumption.TabIndex = 28;
            this.cb_LocationPresumption.Text = "ParticleFilter常時更新";
            this.cb_LocationPresumption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_LocationPresumption.UseVisualStyleBackColor = true;
            // 
            // cb_RivisonGPS
            // 
            this.cb_RivisonGPS.AutoSize = true;
            this.cb_RivisonGPS.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_RivisonGPS.Location = new System.Drawing.Point(13, 113);
            this.cb_RivisonGPS.Name = "cb_RivisonGPS";
            this.cb_RivisonGPS.Size = new System.Drawing.Size(118, 17);
            this.cb_RivisonGPS.TabIndex = 26;
            this.cb_RivisonGPS.Text = "GPSを補正に使う";
            this.cb_RivisonGPS.UseVisualStyleBackColor = true;
            this.cb_RivisonGPS.CheckedChanged += new System.EventHandler(this.cb_RivisonGPS_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cb_SirialConnect);
            this.groupBox2.Controls.Add(this.cb_UsbSirial);
            this.groupBox2.Controls.Add(this.tb_SirialResive);
            this.groupBox2.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox2.Location = new System.Drawing.Point(6, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(220, 85);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "USBConnect";
            // 
            // cb_SirialConnect
            // 
            this.cb_SirialConnect.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_SirialConnect.Location = new System.Drawing.Point(127, 23);
            this.cb_SirialConnect.Name = "cb_SirialConnect";
            this.cb_SirialConnect.Size = new System.Drawing.Size(86, 26);
            this.cb_SirialConnect.TabIndex = 23;
            this.cb_SirialConnect.Text = "GPS受信";
            this.cb_SirialConnect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_SirialConnect.UseVisualStyleBackColor = true;
            this.cb_SirialConnect.CheckedChanged += new System.EventHandler(this.cb_SirialConnect_CheckedChanged);
            // 
            // cb_UsbSirial
            // 
            this.cb_UsbSirial.FormattingEnabled = true;
            this.cb_UsbSirial.Location = new System.Drawing.Point(11, 27);
            this.cb_UsbSirial.Name = "cb_UsbSirial";
            this.cb_UsbSirial.Size = new System.Drawing.Size(97, 21);
            this.cb_UsbSirial.TabIndex = 21;
            // 
            // tb_SirialResive
            // 
            this.tb_SirialResive.Location = new System.Drawing.Point(11, 55);
            this.tb_SirialResive.Name = "tb_SirialResive";
            this.tb_SirialResive.Size = new System.Drawing.Size(202, 20);
            this.tb_SirialResive.TabIndex = 22;
            // 
            // tabPage_Status
            // 
            this.tabPage_Status.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_Status.Controls.Add(this.lbl_RErotL);
            this.tabPage_Status.Controls.Add(this.lbl_RErotR);
            this.tabPage_Status.Controls.Add(this.label17);
            this.tabPage_Status.Controls.Add(this.label16);
            this.tabPage_Status.Controls.Add(this.lbl_REDir);
            this.tabPage_Status.Controls.Add(this.label18);
            this.tabPage_Status.Controls.Add(this.lbl_REY);
            this.tabPage_Status.Controls.Add(this.lbl_REX);
            this.tabPage_Status.Controls.Add(this.label15);
            this.tabPage_Status.Controls.Add(this.label14);
            this.tabPage_Status.Controls.Add(this.lbl_Compass);
            this.tabPage_Status.Controls.Add(this.label13);
            this.tabPage_Status.Controls.Add(this.lbl_LED);
            this.tabPage_Status.Controls.Add(this.lbl_GPS_X);
            this.tabPage_Status.Controls.Add(this.lbl_GPS_Y);
            this.tabPage_Status.Controls.Add(this.label12);
            this.tabPage_Status.Controls.Add(this.label11);
            this.tabPage_Status.Controls.Add(this.label10);
            this.tabPage_Status.Location = new System.Drawing.Point(4, 26);
            this.tabPage_Status.Name = "tabPage_Status";
            this.tabPage_Status.Size = new System.Drawing.Size(238, 234);
            this.tabPage_Status.TabIndex = 2;
            this.tabPage_Status.Text = "Status";
            // 
            // lbl_RErotL
            // 
            this.lbl_RErotL.AutoSize = true;
            this.lbl_RErotL.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_RErotL.Location = new System.Drawing.Point(75, 180);
            this.lbl_RErotL.Name = "lbl_RErotL";
            this.lbl_RErotL.Size = new System.Drawing.Size(28, 16);
            this.lbl_RErotL.TabIndex = 17;
            this.lbl_RErotL.Text = "ND";
            // 
            // lbl_RErotR
            // 
            this.lbl_RErotR.AutoSize = true;
            this.lbl_RErotR.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_RErotR.Location = new System.Drawing.Point(75, 159);
            this.lbl_RErotR.Name = "lbl_RErotR";
            this.lbl_RErotR.Size = new System.Drawing.Size(28, 16);
            this.lbl_RErotR.TabIndex = 16;
            this.lbl_RErotR.Text = "ND";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(5, 180);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(59, 16);
            this.label17.TabIndex = 15;
            this.label17.Text = "RErotL:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(4, 159);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(60, 16);
            this.label16.TabIndex = 14;
            this.label16.Text = "RErotR:";
            // 
            // lbl_REDir
            // 
            this.lbl_REDir.AutoSize = true;
            this.lbl_REDir.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_REDir.Location = new System.Drawing.Point(84, 133);
            this.lbl_REDir.Name = "lbl_REDir";
            this.lbl_REDir.Size = new System.Drawing.Size(28, 16);
            this.lbl_REDir.TabIndex = 13;
            this.lbl_REDir.Text = "ND";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(4, 133);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(74, 16);
            this.label18.TabIndex = 12;
            this.label18.Text = "REplotDir:";
            // 
            // lbl_REY
            // 
            this.lbl_REY.AutoSize = true;
            this.lbl_REY.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_REY.Location = new System.Drawing.Point(75, 108);
            this.lbl_REY.Name = "lbl_REY";
            this.lbl_REY.Size = new System.Drawing.Size(28, 16);
            this.lbl_REY.TabIndex = 11;
            this.lbl_REY.Text = "ND";
            // 
            // lbl_REX
            // 
            this.lbl_REX.AutoSize = true;
            this.lbl_REX.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_REX.Location = new System.Drawing.Point(75, 86);
            this.lbl_REX.Name = "lbl_REX";
            this.lbl_REX.Size = new System.Drawing.Size(28, 16);
            this.lbl_REX.TabIndex = 10;
            this.lbl_REX.Text = "ND";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(4, 108);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(64, 16);
            this.label15.TabIndex = 9;
            this.label15.Text = "REplotY:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(4, 86);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 16);
            this.label14.TabIndex = 8;
            this.label14.Text = "REplotX:";
            // 
            // lbl_Compass
            // 
            this.lbl_Compass.AutoSize = true;
            this.lbl_Compass.Location = new System.Drawing.Point(84, 206);
            this.lbl_Compass.Name = "lbl_Compass";
            this.lbl_Compass.Size = new System.Drawing.Size(28, 16);
            this.lbl_Compass.TabIndex = 7;
            this.lbl_Compass.Text = "ND";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 206);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(72, 16);
            this.label13.TabIndex = 6;
            this.label13.Text = "Compass:";
            // 
            // lbl_LED
            // 
            this.lbl_LED.AutoSize = true;
            this.lbl_LED.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_LED.Location = new System.Drawing.Point(53, 7);
            this.lbl_LED.Name = "lbl_LED";
            this.lbl_LED.Size = new System.Drawing.Size(28, 16);
            this.lbl_LED.TabIndex = 5;
            this.lbl_LED.Text = "ND";
            // 
            // lbl_GPS_X
            // 
            this.lbl_GPS_X.AutoSize = true;
            this.lbl_GPS_X.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_GPS_X.Location = new System.Drawing.Point(67, 54);
            this.lbl_GPS_X.Name = "lbl_GPS_X";
            this.lbl_GPS_X.Size = new System.Drawing.Size(28, 16);
            this.lbl_GPS_X.TabIndex = 4;
            this.lbl_GPS_X.Text = "ND";
            // 
            // lbl_GPS_Y
            // 
            this.lbl_GPS_Y.AutoSize = true;
            this.lbl_GPS_Y.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_GPS_Y.Location = new System.Drawing.Point(67, 33);
            this.lbl_GPS_Y.Name = "lbl_GPS_Y";
            this.lbl_GPS_Y.Size = new System.Drawing.Size(28, 16);
            this.lbl_GPS_Y.TabIndex = 3;
            this.lbl_GPS_Y.Text = "ND";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label12.Location = new System.Drawing.Point(4, 33);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 16);
            this.label12.TabIndex = 2;
            this.label12.Text = "GPS Y:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label11.Location = new System.Drawing.Point(4, 54);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 16);
            this.label11.TabIndex = 1;
            this.label11.Text = "GPS X:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.Location = new System.Drawing.Point(4, 7);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 16);
            this.label10.TabIndex = 0;
            this.label10.Text = "LED:";
            // 
            // tabPage_Emulate
            // 
            this.tabPage_Emulate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tabPage_Emulate.Controls.Add(this.label2);
            this.tabPage_Emulate.Controls.Add(this.tb_LRFScale);
            this.tabPage_Emulate.Controls.Add(this.btm_LRFScale);
            this.tabPage_Emulate.Controls.Add(this.label1);
            this.tabPage_Emulate.Controls.Add(this.groupBox1);
            this.tabPage_Emulate.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tabPage_Emulate.Location = new System.Drawing.Point(4, 26);
            this.tabPage_Emulate.Name = "tabPage_Emulate";
            this.tabPage_Emulate.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Emulate.Size = new System.Drawing.Size(238, 234);
            this.tabPage_Emulate.TabIndex = 1;
            this.tabPage_Emulate.Text = "Emulate";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.Location = new System.Drawing.Point(623, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 16);
            this.label9.TabIndex = 37;
            this.label9.Text = "LRF";
            // 
            // cb_EHS
            // 
            this.cb_EHS.AutoSize = true;
            this.cb_EHS.Checked = true;
            this.cb_EHS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_EHS.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_EHS.Location = new System.Drawing.Point(773, 583);
            this.cb_EHS.Name = "cb_EHS";
            this.cb_EHS.Size = new System.Drawing.Size(78, 20);
            this.cb_EHS.TabIndex = 38;
            this.cb_EHS.Text = "壁回避";
            this.cb_EHS.UseVisualStyleBackColor = true;
            // 
            // tm_SendCom
            // 
            this.tm_SendCom.Interval = 20;
            this.tm_SendCom.Tick += new System.EventHandler(this.tm_SendCom_Tick);
            // 
            // lbl_BackCnt
            // 
            this.lbl_BackCnt.AutoSize = true;
            this.lbl_BackCnt.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_BackCnt.Location = new System.Drawing.Point(734, 663);
            this.lbl_BackCnt.Name = "lbl_BackCnt";
            this.lbl_BackCnt.Size = new System.Drawing.Size(75, 16);
            this.lbl_BackCnt.TabIndex = 39;
            this.lbl_BackCnt.Text = "EBS cnt:";
            // 
            // VehicleRunnerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 688);
            this.Controls.Add(this.lbl_BackCnt);
            this.Controls.Add(this.cb_EHS);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.picbox_LRF);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tb_RESpeed);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lbl_EmurateMode);
            this.Controls.Add(this.tb_HandleVal);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tb_AccelVal);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb_SendData);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_ResiveData);
            this.Controls.Add(this.cb_EmgBrake);
            this.Controls.Add(this.cb_TimerUpdate);
            this.Controls.Add(this.picbox_AreaMap);
            this.KeyPreview = true;
            this.Name = "VehicleRunnerForm";
            this.Text = "VehicleRunner Ver2015-1108B Pinball";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Form1_PreviewKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.picbox_AreaMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_LRF)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPage_Runner.ResumeLayout(false);
            this.tabPage_Runner.PerformLayout();
            this.tabPage_LocSump.ResumeLayout(false);
            this.tabPage_LocSump.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage_Status.ResumeLayout(false);
            this.tabPage_Status.PerformLayout();
            this.tabPage_Emulate.ResumeLayout(false);
            this.tabPage_Emulate.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picbox_AreaMap;
        private System.Windows.Forms.PictureBox picbox_LRF;
        private System.Windows.Forms.CheckBox cb_LRFConnect;
        private System.Windows.Forms.Label lb_LRFResult;
        private System.Windows.Forms.CheckBox cb_TimerUpdate;
        private System.Windows.Forms.Button btm_LRFScale;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_LRFScale;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_PositionReset;
        private System.Windows.Forms.CheckBox cb_RotEnc;
        private System.Windows.Forms.CheckBox cb_EleConpus;
        private System.Windows.Forms.TextBox tb_LRFIpAddr;
        private System.Windows.Forms.TextBox tb_LRFPort;
        private System.Windows.Forms.GroupBox groupBox1;
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
        private System.Windows.Forms.Label lbl_EmurateMode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_RESpeed;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage_Runner;
        private System.Windows.Forms.TabPage tabPage_Emulate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbl_MattingScore;
        private System.Windows.Forms.Button btn_LocRevision;
        private System.Windows.Forms.CheckBox cb_StraightMode;
        private System.Windows.Forms.CheckBox cb_EHS;
        private System.Windows.Forms.Timer tm_SendCom;
        private System.Windows.Forms.TabPage tabPage_Status;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbl_GPS_X;
        private System.Windows.Forms.Label lbl_GPS_Y;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lbl_LED;
        private System.Windows.Forms.Label lbl_REDir;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lbl_REY;
        private System.Windows.Forms.Label lbl_REX;
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
        private System.Windows.Forms.CheckBox cb_RivisonGPS;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cb_SirialConnect;
        private System.Windows.Forms.ComboBox cb_UsbSirial;
        private System.Windows.Forms.TextBox tb_SirialResive;
        private System.Windows.Forms.CheckBox cb_LocationPresumption;
        private System.Windows.Forms.CheckBox cb_RivisionPF;
        private System.Windows.Forms.CheckBox cb_TimeRivision;
    }
}

