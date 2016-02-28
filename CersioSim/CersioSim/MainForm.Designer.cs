namespace CersioSim
{
    partial class CersioSimForm
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
            this.picbox_SimArea = new System.Windows.Forms.PictureBox();
            this.btn_KeyInput = new System.Windows.Forms.Button();
            this.tmr_Update = new System.Windows.Forms.Timer(this.components);
            this.lbl_HandleVal = new System.Windows.Forms.Label();
            this.lbl_AccVal = new System.Windows.Forms.Label();
            this.tb_KeyInput = new System.Windows.Forms.TextBox();
            this.lbl_CarX = new System.Windows.Forms.Label();
            this.lbl_CarY = new System.Windows.Forms.Label();
            this.tbar_Speed = new System.Windows.Forms.TrackBar();
            this.lbl_Speed = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_SimArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbar_Speed)).BeginInit();
            this.SuspendLayout();
            // 
            // picbox_SimArea
            // 
            this.picbox_SimArea.Location = new System.Drawing.Point(12, 12);
            this.picbox_SimArea.Name = "picbox_SimArea";
            this.picbox_SimArea.Size = new System.Drawing.Size(640, 480);
            this.picbox_SimArea.TabIndex = 0;
            this.picbox_SimArea.TabStop = false;
            this.picbox_SimArea.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picbox_SimArea_MouseDown);
            this.picbox_SimArea.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picbox_SimArea_MouseMove);
            this.picbox_SimArea.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picbox_SimArea_MouseUp);
            // 
            // btn_KeyInput
            // 
            this.btn_KeyInput.Location = new System.Drawing.Point(681, 24);
            this.btn_KeyInput.Name = "btn_KeyInput";
            this.btn_KeyInput.Size = new System.Drawing.Size(75, 23);
            this.btn_KeyInput.TabIndex = 1;
            this.btn_KeyInput.Text = "KeyInput";
            this.btn_KeyInput.UseVisualStyleBackColor = true;
            this.btn_KeyInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btn_KeyInput_KeyDown);
            this.btn_KeyInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btn_KeyInput_KeyUp);
            // 
            // tmr_Update
            // 
            this.tmr_Update.Interval = 30;
            this.tmr_Update.Tick += new System.EventHandler(this.tmr_Update_Tick);
            // 
            // lbl_HandleVal
            // 
            this.lbl_HandleVal.AutoSize = true;
            this.lbl_HandleVal.Location = new System.Drawing.Point(23, 522);
            this.lbl_HandleVal.Name = "lbl_HandleVal";
            this.lbl_HandleVal.Size = new System.Drawing.Size(73, 12);
            this.lbl_HandleVal.TabIndex = 2;
            this.lbl_HandleVal.Text = "lbl_HandleVal";
            // 
            // lbl_AccVal
            // 
            this.lbl_AccVal.AutoSize = true;
            this.lbl_AccVal.Location = new System.Drawing.Point(399, 522);
            this.lbl_AccVal.Name = "lbl_AccVal";
            this.lbl_AccVal.Size = new System.Drawing.Size(58, 12);
            this.lbl_AccVal.TabIndex = 3;
            this.lbl_AccVal.Text = "lbl_AccVal";
            // 
            // tb_KeyInput
            // 
            this.tb_KeyInput.Location = new System.Drawing.Point(683, 53);
            this.tb_KeyInput.Name = "tb_KeyInput";
            this.tb_KeyInput.Size = new System.Drawing.Size(73, 19);
            this.tb_KeyInput.TabIndex = 4;
            this.tb_KeyInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_KeyInput_KeyDown);
            this.tb_KeyInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tb_KeyInput_KeyUp);
            // 
            // lbl_CarX
            // 
            this.lbl_CarX.AutoSize = true;
            this.lbl_CarX.Location = new System.Drawing.Point(681, 126);
            this.lbl_CarX.Name = "lbl_CarX";
            this.lbl_CarX.Size = new System.Drawing.Size(46, 12);
            this.lbl_CarX.TabIndex = 5;
            this.lbl_CarX.Text = "lbl_CarX";
            // 
            // lbl_CarY
            // 
            this.lbl_CarY.AutoSize = true;
            this.lbl_CarY.Location = new System.Drawing.Point(681, 153);
            this.lbl_CarY.Name = "lbl_CarY";
            this.lbl_CarY.Size = new System.Drawing.Size(46, 12);
            this.lbl_CarY.TabIndex = 6;
            this.lbl_CarY.Text = "lbl_CarY";
            // 
            // tbar_Speed
            // 
            this.tbar_Speed.Location = new System.Drawing.Point(487, 505);
            this.tbar_Speed.Minimum = -10;
            this.tbar_Speed.Name = "tbar_Speed";
            this.tbar_Speed.Size = new System.Drawing.Size(256, 45);
            this.tbar_Speed.TabIndex = 7;
            this.tbar_Speed.Scroll += new System.EventHandler(this.tbar_Scroll);
            // 
            // lbl_Speed
            // 
            this.lbl_Speed.AutoSize = true;
            this.lbl_Speed.Location = new System.Drawing.Point(679, 88);
            this.lbl_Speed.Name = "lbl_Speed";
            this.lbl_Speed.Size = new System.Drawing.Size(36, 12);
            this.lbl_Speed.TabIndex = 8;
            this.lbl_Speed.Text = "Speed";
            // 
            // CersioSimForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 562);
            this.Controls.Add(this.lbl_Speed);
            this.Controls.Add(this.tbar_Speed);
            this.Controls.Add(this.lbl_CarY);
            this.Controls.Add(this.lbl_CarX);
            this.Controls.Add(this.tb_KeyInput);
            this.Controls.Add(this.lbl_AccVal);
            this.Controls.Add(this.lbl_HandleVal);
            this.Controls.Add(this.btn_KeyInput);
            this.Controls.Add(this.picbox_SimArea);
            this.Name = "CersioSimForm";
            this.Text = "TKBC2015 Cersio Simurator";
            ((System.ComponentModel.ISupportInitialize)(this.picbox_SimArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbar_Speed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picbox_SimArea;
        private System.Windows.Forms.Button btn_KeyInput;
        private System.Windows.Forms.Timer tmr_Update;
        private System.Windows.Forms.Label lbl_HandleVal;
        private System.Windows.Forms.Label lbl_AccVal;
        private System.Windows.Forms.TextBox tb_KeyInput;
        private System.Windows.Forms.Label lbl_CarX;
        private System.Windows.Forms.Label lbl_CarY;
        private System.Windows.Forms.TrackBar tbar_Speed;
        private System.Windows.Forms.Label lbl_Speed;
    }
}

