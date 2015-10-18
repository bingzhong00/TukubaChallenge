namespace ActiveDetour
{
    partial class ActiveDetourMonitor
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tmr_upDate = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.tb_Handle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_Accel = new System.Windows.Forms.TextBox();
            this.lbl_LrfConnect = new System.Windows.Forms.Label();
            this.lbl_BoxPCConnect = new System.Windows.Forms.Label();
            this.tb_ResiveData = new System.Windows.Forms.TextBox();
            this.cb_EmgStop = new System.Windows.Forms.CheckBox();
            this.btn_LRFConnect = new System.Windows.Forms.Button();
            this.cb_AccelOff = new System.Windows.Forms.CheckBox();
            this.lbl_HandleRange = new System.Windows.Forms.Label();
            this.tb_HandleRange = new System.Windows.Forms.TrackBar();
            this.btn_BoxPcConnect = new System.Windows.Forms.Button();
            this.tmr_upDateLRF = new System.Windows.Forms.Timer(this.components);
            this.lbl_Speed = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_HandleRange)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(558, 413);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tmr_upDate
            // 
            this.tmr_upDate.Tick += new System.EventHandler(this.tmr_upDate_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(12, 448);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 33);
            this.label1.TabIndex = 1;
            this.label1.Text = "Handle";
            // 
            // tb_Handle
            // 
            this.tb_Handle.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_Handle.Location = new System.Drawing.Point(124, 445);
            this.tb_Handle.Name = "tb_Handle";
            this.tb_Handle.Size = new System.Drawing.Size(102, 39);
            this.tb_Handle.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(262, 445);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 33);
            this.label2.TabIndex = 3;
            this.label2.Text = "Accel";
            // 
            // tb_Accel
            // 
            this.tb_Accel.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_Accel.Location = new System.Drawing.Point(358, 445);
            this.tb_Accel.Name = "tb_Accel";
            this.tb_Accel.Size = new System.Drawing.Size(98, 39);
            this.tb_Accel.TabIndex = 4;
            // 
            // lbl_LrfConnect
            // 
            this.lbl_LrfConnect.AutoSize = true;
            this.lbl_LrfConnect.Location = new System.Drawing.Point(584, 133);
            this.lbl_LrfConnect.Name = "lbl_LrfConnect";
            this.lbl_LrfConnect.Size = new System.Drawing.Size(102, 12);
            this.lbl_LrfConnect.TabIndex = 5;
            this.lbl_LrfConnect.Text = "LRF Connect None";
            // 
            // lbl_BoxPCConnect
            // 
            this.lbl_BoxPCConnect.AutoSize = true;
            this.lbl_BoxPCConnect.Location = new System.Drawing.Point(588, 226);
            this.lbl_BoxPCConnect.Name = "lbl_BoxPCConnect";
            this.lbl_BoxPCConnect.Size = new System.Drawing.Size(114, 12);
            this.lbl_BoxPCConnect.TabIndex = 6;
            this.lbl_BoxPCConnect.Text = "BoxPc Connect None";
            // 
            // tb_ResiveData
            // 
            this.tb_ResiveData.Enabled = false;
            this.tb_ResiveData.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_ResiveData.Location = new System.Drawing.Point(12, 497);
            this.tb_ResiveData.Name = "tb_ResiveData";
            this.tb_ResiveData.Size = new System.Drawing.Size(748, 23);
            this.tb_ResiveData.TabIndex = 7;
            // 
            // cb_EmgStop
            // 
            this.cb_EmgStop.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_EmgStop.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_EmgStop.Location = new System.Drawing.Point(590, 31);
            this.cb_EmgStop.Name = "cb_EmgStop";
            this.cb_EmgStop.Size = new System.Drawing.Size(165, 55);
            this.cb_EmgStop.TabIndex = 8;
            this.cb_EmgStop.Text = "緊急停止";
            this.cb_EmgStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_EmgStop.UseVisualStyleBackColor = true;
            this.cb_EmgStop.CheckedChanged += new System.EventHandler(this.cb_EmgStop_CheckedChanged);
            // 
            // btn_LRFConnect
            // 
            this.btn_LRFConnect.Location = new System.Drawing.Point(627, 148);
            this.btn_LRFConnect.Name = "btn_LRFConnect";
            this.btn_LRFConnect.Size = new System.Drawing.Size(128, 40);
            this.btn_LRFConnect.TabIndex = 9;
            this.btn_LRFConnect.Text = "LRF Connect";
            this.btn_LRFConnect.UseVisualStyleBackColor = true;
            this.btn_LRFConnect.Click += new System.EventHandler(this.btn_LRFConnect_Click);
            // 
            // cb_AccelOff
            // 
            this.cb_AccelOff.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_AccelOff.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_AccelOff.Location = new System.Drawing.Point(462, 445);
            this.cb_AccelOff.Name = "cb_AccelOff";
            this.cb_AccelOff.Size = new System.Drawing.Size(106, 33);
            this.cb_AccelOff.TabIndex = 10;
            this.cb_AccelOff.Text = "AccelOFF";
            this.cb_AccelOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_AccelOff.UseVisualStyleBackColor = true;
            // 
            // lbl_HandleRange
            // 
            this.lbl_HandleRange.AutoSize = true;
            this.lbl_HandleRange.Location = new System.Drawing.Point(588, 339);
            this.lbl_HandleRange.Name = "lbl_HandleRange";
            this.lbl_HandleRange.Size = new System.Drawing.Size(72, 12);
            this.lbl_HandleRange.TabIndex = 11;
            this.lbl_HandleRange.Text = "HandleRange";
            // 
            // tb_HandleRange
            // 
            this.tb_HandleRange.LargeChange = 2;
            this.tb_HandleRange.Location = new System.Drawing.Point(586, 354);
            this.tb_HandleRange.Maximum = 30;
            this.tb_HandleRange.Name = "tb_HandleRange";
            this.tb_HandleRange.Size = new System.Drawing.Size(169, 45);
            this.tb_HandleRange.TabIndex = 12;
            this.tb_HandleRange.Value = 4;
            this.tb_HandleRange.Scroll += new System.EventHandler(this.tb_HandleRange_Scroll);
            // 
            // btn_BoxPcConnect
            // 
            this.btn_BoxPcConnect.Location = new System.Drawing.Point(627, 252);
            this.btn_BoxPcConnect.Name = "btn_BoxPcConnect";
            this.btn_BoxPcConnect.Size = new System.Drawing.Size(128, 40);
            this.btn_BoxPcConnect.TabIndex = 13;
            this.btn_BoxPcConnect.Text = "BoxPc Connect";
            this.btn_BoxPcConnect.UseVisualStyleBackColor = true;
            this.btn_BoxPcConnect.Click += new System.EventHandler(this.btn_BoxPcConnect_Click);
            // 
            // tmr_upDateLRF
            // 
            this.tmr_upDateLRF.Interval = 20;
            this.tmr_upDateLRF.Tick += new System.EventHandler(this.tmr_upDateLRF_Tick);
            // 
            // lbl_Speed
            // 
            this.lbl_Speed.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_Speed.Location = new System.Drawing.Point(579, 445);
            this.lbl_Speed.Name = "lbl_Speed";
            this.lbl_Speed.Size = new System.Drawing.Size(175, 32);
            this.lbl_Speed.TabIndex = 14;
            this.lbl_Speed.Text = "Speed:0m/h";
            this.lbl_Speed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ActiveDetourMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 532);
            this.Controls.Add(this.lbl_Speed);
            this.Controls.Add(this.btn_BoxPcConnect);
            this.Controls.Add(this.tb_HandleRange);
            this.Controls.Add(this.lbl_HandleRange);
            this.Controls.Add(this.cb_AccelOff);
            this.Controls.Add(this.btn_LRFConnect);
            this.Controls.Add(this.cb_EmgStop);
            this.Controls.Add(this.tb_ResiveData);
            this.Controls.Add(this.lbl_BoxPCConnect);
            this.Controls.Add(this.lbl_LrfConnect);
            this.Controls.Add(this.tb_Accel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_Handle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ActiveDetourMonitor";
            this.Text = "ActiveDetourMonitor";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_HandleRange)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer tmr_upDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_Handle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_Accel;
        private System.Windows.Forms.Label lbl_LrfConnect;
        private System.Windows.Forms.Label lbl_BoxPCConnect;
        private System.Windows.Forms.TextBox tb_ResiveData;
        private System.Windows.Forms.CheckBox cb_EmgStop;
        private System.Windows.Forms.Button btn_LRFConnect;
        private System.Windows.Forms.CheckBox cb_AccelOff;
        private System.Windows.Forms.Label lbl_HandleRange;
        private System.Windows.Forms.TrackBar tb_HandleRange;
        private System.Windows.Forms.Button btn_BoxPcConnect;
        private System.Windows.Forms.Timer tmr_upDateLRF;
        private System.Windows.Forms.Label lbl_Speed;
    }
}

