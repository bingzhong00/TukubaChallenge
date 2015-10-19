namespace VehicleRunner
{
    partial class LocPresumpForm {
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
            this.gb_LRF = new System.Windows.Forms.GroupBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.picbox_AreaMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_LRF)).BeginInit();
            this.gb_LRF.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.picbox_AreaMap.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.picbox_AreaMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.picbox_AreaMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.picbox_AreaMap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // picbox_LRF
            // 
            this.picbox_LRF.BackColor = System.Drawing.Color.White;
            this.picbox_LRF.Location = new System.Drawing.Point(8, 13);
            this.picbox_LRF.Name = "picbox_LRF";
            this.picbox_LRF.Size = new System.Drawing.Size(240, 240);
            this.picbox_LRF.TabIndex = 1;
            this.picbox_LRF.TabStop = false;
            this.picbox_LRF.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox2_Paint);
            // 
            // cb_LRFConnect
            // 
            this.cb_LRFConnect.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_LRFConnect.Location = new System.Drawing.Point(11, 293);
            this.cb_LRFConnect.Name = "cb_LRFConnect";
            this.cb_LRFConnect.Size = new System.Drawing.Size(109, 37);
            this.cb_LRFConnect.TabIndex = 3;
            this.cb_LRFConnect.Text = "LRF Connect";
            this.cb_LRFConnect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_LRFConnect.UseVisualStyleBackColor = true;
            this.cb_LRFConnect.CheckedChanged += new System.EventHandler(this.cb_LRFConnect_CheckedChanged);
            // 
            // lb_LRFResult
            // 
            this.lb_LRFResult.AutoSize = true;
            this.lb_LRFResult.Location = new System.Drawing.Point(137, 305);
            this.lb_LRFResult.Name = "lb_LRFResult";
            this.lb_LRFResult.Size = new System.Drawing.Size(38, 12);
            this.lb_LRFResult.TabIndex = 4;
            this.lb_LRFResult.Text = "Result";
            // 
            // cb_TimerUpdate
            // 
            this.cb_TimerUpdate.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_TimerUpdate.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_TimerUpdate.Location = new System.Drawing.Point(620, 525);
            this.cb_TimerUpdate.Name = "cb_TimerUpdate";
            this.cb_TimerUpdate.Size = new System.Drawing.Size(127, 44);
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
            this.btm_LRFScale.Location = new System.Drawing.Point(197, 342);
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
            this.label1.Location = new System.Drawing.Point(168, 350);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "mm";
            // 
            // tb_LRFScale
            // 
            this.tb_LRFScale.Location = new System.Drawing.Point(99, 346);
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
            this.label2.Location = new System.Drawing.Point(27, 349);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "LRF Scale";
            // 
            // btn_PositionReset
            // 
            this.btn_PositionReset.Location = new System.Drawing.Point(132, 50);
            this.btn_PositionReset.Name = "btn_PositionReset";
            this.btn_PositionReset.Size = new System.Drawing.Size(106, 29);
            this.btn_PositionReset.TabIndex = 12;
            this.btn_PositionReset.Text = "推定位置リセット";
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
            this.tb_LRFIpAddr.Location = new System.Drawing.Point(11, 264);
            this.tb_LRFIpAddr.Name = "tb_LRFIpAddr";
            this.tb_LRFIpAddr.Size = new System.Drawing.Size(118, 19);
            this.tb_LRFIpAddr.TabIndex = 15;
            this.tb_LRFIpAddr.Text = "192.168.1.10";
            // 
            // tb_LRFPort
            // 
            this.tb_LRFPort.Location = new System.Drawing.Point(139, 264);
            this.tb_LRFPort.Name = "tb_LRFPort";
            this.tb_LRFPort.Size = new System.Drawing.Size(63, 19);
            this.tb_LRFPort.TabIndex = 16;
            this.tb_LRFPort.Text = "10940";
            // 
            // gb_LRF
            // 
            this.gb_LRF.Controls.Add(this.tb_LRFPort);
            this.gb_LRF.Controls.Add(this.tb_LRFIpAddr);
            this.gb_LRF.Controls.Add(this.label2);
            this.gb_LRF.Controls.Add(this.tb_LRFScale);
            this.gb_LRF.Controls.Add(this.label1);
            this.gb_LRF.Controls.Add(this.btm_LRFScale);
            this.gb_LRF.Controls.Add(this.lb_LRFResult);
            this.gb_LRF.Controls.Add(this.cb_LRFConnect);
            this.gb_LRF.Controls.Add(this.picbox_LRF);
            this.gb_LRF.Location = new System.Drawing.Point(618, 12);
            this.gb_LRF.Name = "gb_LRF";
            this.gb_LRF.Size = new System.Drawing.Size(257, 380);
            this.gb_LRF.TabIndex = 17;
            this.gb_LRF.TabStop = false;
            this.gb_LRF.Text = "LRF入力";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_EleConpus);
            this.groupBox1.Controls.Add(this.cb_RotEnc);
            this.groupBox1.Controls.Add(this.btn_PositionReset);
            this.groupBox1.Controls.Add(this.cb_LocationPresumption);
            this.groupBox1.Location = new System.Drawing.Point(620, 425);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(244, 94);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "自己位置推定";
            // 
            // cb_EmgBrake
            // 
            this.cb_EmgBrake.AutoSize = true;
            this.cb_EmgBrake.Checked = true;
            this.cb_EmgBrake.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_EmgBrake.Location = new System.Drawing.Point(780, 410);
            this.cb_EmgBrake.Name = "cb_EmgBrake";
            this.cb_EmgBrake.Size = new System.Drawing.Size(72, 16);
            this.cb_EmgBrake.TabIndex = 19;
            this.cb_EmgBrake.Text = "緊急停止";
            this.cb_EmgBrake.UseVisualStyleBackColor = true;
            this.cb_EmgBrake.CheckedChanged += new System.EventHandler(this.cb_EmgBrake_CheckedChanged);
            // 
            // cb_UsbSirial
            // 
            this.cb_UsbSirial.FormattingEnabled = true;
            this.cb_UsbSirial.Location = new System.Drawing.Point(620, 575);
            this.cb_UsbSirial.Name = "cb_UsbSirial";
            this.cb_UsbSirial.Size = new System.Drawing.Size(102, 20);
            this.cb_UsbSirial.TabIndex = 21;
            // 
            // tb_SirialResive
            // 
            this.tb_SirialResive.Location = new System.Drawing.Point(735, 576);
            this.tb_SirialResive.Name = "tb_SirialResive";
            this.tb_SirialResive.Size = new System.Drawing.Size(131, 19);
            this.tb_SirialResive.TabIndex = 22;
            // 
            // cb_SirialConnect
            // 
            this.cb_SirialConnect.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_SirialConnect.Location = new System.Drawing.Point(778, 543);
            this.cb_SirialConnect.Name = "cb_SirialConnect";
            this.cb_SirialConnect.Size = new System.Drawing.Size(86, 26);
            this.cb_SirialConnect.TabIndex = 23;
            this.cb_SirialConnect.Text = "SH2制御出力";
            this.cb_SirialConnect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_SirialConnect.UseVisualStyleBackColor = true;
            // 
            // tm_UpdateHw
            // 
            this.tm_UpdateHw.Interval = 60;
            this.tm_UpdateHw.Tick += new System.EventHandler(this.tm_UpdateHw_Tick);
            // 
            // tb_ResiveData
            // 
            this.tb_ResiveData.Location = new System.Drawing.Point(45, 650);
            this.tb_ResiveData.Name = "tb_ResiveData";
            this.tb_ResiveData.Size = new System.Drawing.Size(522, 19);
            this.tb_ResiveData.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 653);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 25;
            this.label3.Text = "受信";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 625);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 12);
            this.label4.TabIndex = 26;
            this.label4.Text = "toBoxPC 送信";
            // 
            // tb_SendData
            // 
            this.tb_SendData.Location = new System.Drawing.Point(94, 625);
            this.tb_SendData.Name = "tb_SendData";
            this.tb_SendData.Size = new System.Drawing.Size(473, 19);
            this.tb_SendData.TabIndex = 27;
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
            this.tb_AccelVal.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_AccelVal.Location = new System.Drawing.Point(799, 620);
            this.tb_AccelVal.Name = "tb_AccelVal";
            this.tb_AccelVal.Size = new System.Drawing.Size(73, 31);
            this.tb_AccelVal.TabIndex = 29;
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
            this.tb_HandleVal.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_HandleVal.Location = new System.Drawing.Point(645, 620);
            this.tb_HandleVal.Name = "tb_HandleVal";
            this.tb_HandleVal.Size = new System.Drawing.Size(73, 31);
            this.tb_HandleVal.TabIndex = 31;
            // 
            // lbl_EmurateMode
            // 
            this.lbl_EmurateMode.AutoSize = true;
            this.lbl_EmurateMode.BackColor = System.Drawing.Color.Red;
            this.lbl_EmurateMode.Location = new System.Drawing.Point(624, 395);
            this.lbl_EmurateMode.Name = "lbl_EmurateMode";
            this.lbl_EmurateMode.Size = new System.Drawing.Size(100, 12);
            this.lbl_EmurateMode.TabIndex = 32;
            this.lbl_EmurateMode.Text = "エミュレーションMode";
            // 
            // LocPresumpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 675);
            this.Controls.Add(this.lbl_EmurateMode);
            this.Controls.Add(this.tb_HandleVal);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tb_AccelVal);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb_SendData);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_ResiveData);
            this.Controls.Add(this.cb_SirialConnect);
            this.Controls.Add(this.tb_SirialResive);
            this.Controls.Add(this.cb_UsbSirial);
            this.Controls.Add(this.cb_EmgBrake);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gb_LRF);
            this.Controls.Add(this.cb_TimerUpdate);
            this.Controls.Add(this.picbox_AreaMap);
            this.KeyPreview = true;
            this.Name = "LocPresumpForm";
            this.Text = "VehicleRunner Ver0.60";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Form1_PreviewKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.picbox_AreaMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_LRF)).EndInit();
            this.gb_LRF.ResumeLayout(false);
            this.gb_LRF.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.GroupBox gb_LRF;
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
    }
}

