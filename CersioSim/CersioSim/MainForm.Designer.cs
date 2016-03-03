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
            this.tmr_Update = new System.Windows.Forms.Timer(this.components);
            this.lbl_HandleVal = new System.Windows.Forms.Label();
            this.lbl_AccVal = new System.Windows.Forms.Label();
            this.lbl_CarX = new System.Windows.Forms.Label();
            this.lbl_CarY = new System.Windows.Forms.Label();
            this.lbl_Speed = new System.Windows.Forms.Label();
            this.picbox_MsController = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_SimArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_MsController)).BeginInit();
            this.SuspendLayout();
            // 
            // picbox_SimArea
            // 
            this.picbox_SimArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picbox_SimArea.Location = new System.Drawing.Point(12, 12);
            this.picbox_SimArea.Name = "picbox_SimArea";
            this.picbox_SimArea.Size = new System.Drawing.Size(640, 480);
            this.picbox_SimArea.TabIndex = 0;
            this.picbox_SimArea.TabStop = false;
            this.picbox_SimArea.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picbox_SimArea_MouseDown);
            this.picbox_SimArea.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picbox_SimArea_MouseMove);
            this.picbox_SimArea.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picbox_SimArea_MouseUp);
            // 
            // tmr_Update
            // 
            this.tmr_Update.Interval = 30;
            this.tmr_Update.Tick += new System.EventHandler(this.tmr_Update_Tick);
            // 
            // lbl_HandleVal
            // 
            this.lbl_HandleVal.AutoSize = true;
            this.lbl_HandleVal.Location = new System.Drawing.Point(658, 350);
            this.lbl_HandleVal.Name = "lbl_HandleVal";
            this.lbl_HandleVal.Size = new System.Drawing.Size(73, 12);
            this.lbl_HandleVal.TabIndex = 2;
            this.lbl_HandleVal.Text = "lbl_HandleVal";
            // 
            // lbl_AccVal
            // 
            this.lbl_AccVal.AutoSize = true;
            this.lbl_AccVal.Location = new System.Drawing.Point(658, 330);
            this.lbl_AccVal.Name = "lbl_AccVal";
            this.lbl_AccVal.Size = new System.Drawing.Size(58, 12);
            this.lbl_AccVal.TabIndex = 3;
            this.lbl_AccVal.Text = "lbl_AccVal";
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
            // lbl_Speed
            // 
            this.lbl_Speed.AutoSize = true;
            this.lbl_Speed.Location = new System.Drawing.Point(670, 87);
            this.lbl_Speed.Name = "lbl_Speed";
            this.lbl_Speed.Size = new System.Drawing.Size(36, 12);
            this.lbl_Speed.TabIndex = 8;
            this.lbl_Speed.Text = "Speed";
            // 
            // picbox_MsController
            // 
            this.picbox_MsController.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picbox_MsController.Location = new System.Drawing.Point(659, 394);
            this.picbox_MsController.Name = "picbox_MsController";
            this.picbox_MsController.Size = new System.Drawing.Size(111, 98);
            this.picbox_MsController.TabIndex = 9;
            this.picbox_MsController.TabStop = false;
            this.picbox_MsController.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picbox_MsController_MouseDown);
            this.picbox_MsController.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picbox_MsController_MouseMove);
            this.picbox_MsController.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picbox_MsController_MouseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(660, 377);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "MouseController";
            // 
            // CersioSimForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 562);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picbox_MsController);
            this.Controls.Add(this.lbl_Speed);
            this.Controls.Add(this.lbl_CarY);
            this.Controls.Add(this.lbl_CarX);
            this.Controls.Add(this.lbl_AccVal);
            this.Controls.Add(this.lbl_HandleVal);
            this.Controls.Add(this.picbox_SimArea);
            this.Name = "CersioSimForm";
            this.Text = "TKBC2015 Cersio Simurator";
            ((System.ComponentModel.ISupportInitialize)(this.picbox_SimArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_MsController)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picbox_SimArea;
        private System.Windows.Forms.Timer tmr_Update;
        private System.Windows.Forms.Label lbl_HandleVal;
        private System.Windows.Forms.Label lbl_AccVal;
        private System.Windows.Forms.Label lbl_CarX;
        private System.Windows.Forms.Label lbl_CarY;
        private System.Windows.Forms.Label lbl_Speed;
        private System.Windows.Forms.PictureBox picbox_MsController;
        private System.Windows.Forms.Label label1;
    }
}

