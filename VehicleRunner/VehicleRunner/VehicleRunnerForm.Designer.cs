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
            this.cb_LocationPresumption = new System.Windows.Forms.CheckBox();
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
            this.cb_UsbSirial = new System.Windows.Forms.ComboBox();
            this.tb_SirialResive = new System.Windows.Forms.TextBox();
            this.cb_SirialConnect = new System.Windows.Forms.CheckBox();
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
            this.lbl_MattingScore = new System.Windows.Forms.Label();
            this.tabPage_Emurate = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btn_LocRevision = new System.Windows.Forms.Button();
            this.RevisionProgBer = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_AreaMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_LRF)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage_Runner.SuspendLayout();
            this.tabPage_Emurate.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.picbox_AreaMap.Click += new System.EventHandler(this.picbox_AreaMap_Click);
            this.picbox_AreaMap.Paint += new System.Windows.Forms.PaintEventHandler(this.picbox_AreaMap_Paint);
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
            this.cb_TimerUpdate.Location = new System.Drawing.Point(626, 552);
            this.cb_TimerUpdate.Name = "cb_TimerUpdate";
            this.cb_TimerUpdate.Size = new System.Drawing.Size(127, 53);
            this.cb_TimerUpdate.TabIndex = 5;
            this.cb_TimerUpdate.Text = "START";
            this.cb_TimerUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_TimerUpdate.UseVisualStyleBackColor = true;
            this.cb_TimerUpdate.CheckedChanged += new System.EventHandler(this.cb_TimerUpdate_CheckedChanged);
            this.cb_TimerUpdate.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cb_TimerUpdate_PreviewKeyDown);
            // 
            // cb_LocationPresumption
            // 
            this.cb_LocationPresumption.AutoSize = true;
            this.cb_LocationPresumption.Location = new System.Drawing.Point(6, 24);
            this.cb_LocationPresumption.Name = "cb_LocationPresumption";
            this.cb_LocationPresumption.Size = new System.Drawing.Size(90, 16);
            this.cb_LocationPresumption.TabIndex = 6;
            this.cb_LocationPresumption.Text = "ParticleFilter";
            this.cb_LocationPresumption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_LocationPresumption.UseVisualStyleBackColor = true;
            this.cb_LocationPresumption.CheckedChanged += new System.EventHandler(this.cb_LocationPresumpring_CheckedChanged);
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
            this.groupBox1.Controls.Add(this.cb_LocationPresumption);
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
            this.cb_EmgBrake.Location = new System.Drawing.Point(770, 585);
            this.cb_EmgBrake.Name = "cb_EmgBrake";
            this.cb_EmgBrake.Size = new System.Drawing.Size(95, 20);
            this.cb_EmgBrake.TabIndex = 19;
            this.cb_EmgBrake.Text = "緊急停止";
            this.cb_EmgBrake.UseVisualStyleBackColor = true;
            this.cb_EmgBrake.CheckedChanged += new System.EventHandler(this.cb_EmgBrake_CheckedChanged);
            // 
            // cb_UsbSirial
            // 
            this.cb_UsbSirial.FormattingEnabled = true;
            this.cb_UsbSirial.Location = new System.Drawing.Point(11, 23);
            this.cb_UsbSirial.Name = "cb_UsbSirial";
            this.cb_UsbSirial.Size = new System.Drawing.Size(97, 20);
            this.cb_UsbSirial.TabIndex = 21;
            // 
            // tb_SirialResive
            // 
            this.tb_SirialResive.Location = new System.Drawing.Point(11, 49);
            this.tb_SirialResive.Name = "tb_SirialResive";
            this.tb_SirialResive.Size = new System.Drawing.Size(99, 19);
            this.tb_SirialResive.TabIndex = 22;
            // 
            // cb_SirialConnect
            // 
            this.cb_SirialConnect.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_SirialConnect.Location = new System.Drawing.Point(128, 45);
            this.cb_SirialConnect.Name = "cb_SirialConnect";
            this.cb_SirialConnect.Size = new System.Drawing.Size(86, 26);
            this.cb_SirialConnect.TabIndex = 23;
            this.cb_SirialConnect.Text = "SH2制御出力";
            this.cb_SirialConnect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_SirialConnect.UseVisualStyleBackColor = true;
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
            this.tb_ResiveData.Size = new System.Drawing.Size(512, 23);
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
            this.tb_SendData.Size = new System.Drawing.Size(473, 23);
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
            this.lbl_EmurateMode.Location = new System.Drawing.Point(768, 552);
            this.lbl_EmurateMode.Name = "lbl_EmurateMode";
            this.lbl_EmurateMode.Size = new System.Drawing.Size(100, 12);
            this.lbl_EmurateMode.TabIndex = 32;
            this.lbl_EmurateMode.Text = "エミュレーションMode";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(582, 655);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 24);
            this.label7.TabIndex = 33;
            this.label7.Text = "SPD";
            // 
            // tb_RESpeed
            // 
            this.tb_RESpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_RESpeed.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_RESpeed.Location = new System.Drawing.Point(645, 653);
            this.tb_RESpeed.Name = "tb_RESpeed";
            this.tb_RESpeed.ReadOnly = true;
            this.tb_RESpeed.Size = new System.Drawing.Size(73, 31);
            this.tb_RESpeed.TabIndex = 34;
            this.tb_RESpeed.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(724, 666);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 35;
            this.label8.Text = "mm/sec";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage_Runner);
            this.tabControl.Controls.Add(this.tabPage_Emurate);
            this.tabControl.Location = new System.Drawing.Point(626, 273);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(246, 264);
            this.tabControl.TabIndex = 36;
            // 
            // tabPage_Runner
            // 
            this.tabPage_Runner.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_Runner.Controls.Add(this.RevisionProgBer);
            this.tabPage_Runner.Controls.Add(this.btn_LocRevision);
            this.tabPage_Runner.Controls.Add(this.lbl_MattingScore);
            this.tabPage_Runner.Controls.Add(this.lb_LRFResult);
            this.tabPage_Runner.Controls.Add(this.tb_LRFPort);
            this.tabPage_Runner.Controls.Add(this.cb_LRFConnect);
            this.tabPage_Runner.Controls.Add(this.tb_LRFIpAddr);
            this.tabPage_Runner.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Runner.Name = "tabPage_Runner";
            this.tabPage_Runner.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Runner.Size = new System.Drawing.Size(238, 238);
            this.tabPage_Runner.TabIndex = 0;
            this.tabPage_Runner.Text = "Runner";
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
            // tabPage_Emurate
            // 
            this.tabPage_Emurate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tabPage_Emurate.Controls.Add(this.groupBox2);
            this.tabPage_Emurate.Controls.Add(this.label2);
            this.tabPage_Emurate.Controls.Add(this.tb_LRFScale);
            this.tabPage_Emurate.Controls.Add(this.btm_LRFScale);
            this.tabPage_Emurate.Controls.Add(this.label1);
            this.tabPage_Emurate.Controls.Add(this.groupBox1);
            this.tabPage_Emurate.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Emurate.Name = "tabPage_Emurate";
            this.tabPage_Emurate.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Emurate.Size = new System.Drawing.Size(238, 238);
            this.tabPage_Emurate.TabIndex = 1;
            this.tabPage_Emurate.Text = "Emurate";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cb_SirialConnect);
            this.groupBox2.Controls.Add(this.cb_UsbSirial);
            this.groupBox2.Controls.Add(this.tb_SirialResive);
            this.groupBox2.Location = new System.Drawing.Point(8, 139);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(220, 85);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SH2 USBConnect";
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
            // btn_LocRevision
            // 
            this.btn_LocRevision.Location = new System.Drawing.Point(6, 132);
            this.btn_LocRevision.Name = "btn_LocRevision";
            this.btn_LocRevision.Size = new System.Drawing.Size(117, 43);
            this.btn_LocRevision.TabIndex = 18;
            this.btn_LocRevision.Text = "位置補正";
            this.btn_LocRevision.UseVisualStyleBackColor = true;
            this.btn_LocRevision.Click += new System.EventHandler(this.btn_LocRevision_Click);
            // 
            // RevisionProgBer
            // 
            this.RevisionProgBer.Location = new System.Drawing.Point(134, 141);
            this.RevisionProgBer.Name = "RevisionProgBer";
            this.RevisionProgBer.Size = new System.Drawing.Size(85, 23);
            this.RevisionProgBer.TabIndex = 19;
            // 
            // VehicleRunnerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 688);
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
            this.Text = "VehicleRunner Ver0.75";
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
            this.tabPage_Emurate.ResumeLayout(false);
            this.tabPage_Emurate.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picbox_AreaMap;
        private System.Windows.Forms.PictureBox picbox_LRF;
        private System.Windows.Forms.CheckBox cb_LRFConnect;
        private System.Windows.Forms.Label lb_LRFResult;
        private System.Windows.Forms.CheckBox cb_TimerUpdate;
        private System.Windows.Forms.CheckBox cb_LocationPresumption;
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
        private System.Windows.Forms.ComboBox cb_UsbSirial;
        private System.Windows.Forms.TextBox tb_SirialResive;
        private System.Windows.Forms.CheckBox cb_SirialConnect;
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
        private System.Windows.Forms.TabPage tabPage_Emurate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbl_MattingScore;
        private System.Windows.Forms.ProgressBar RevisionProgBer;
        private System.Windows.Forms.Button btn_LocRevision;
    }
}

