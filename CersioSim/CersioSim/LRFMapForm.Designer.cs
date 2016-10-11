namespace CersioSim
{
    partial class LRFMapForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.picboxLRF = new System.Windows.Forms.PictureBox();
            this.picboxMap = new System.Windows.Forms.PictureBox();
            this.tmr_Update = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picboxLRF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picboxMap)).BeginInit();
            this.SuspendLayout();
            // 
            // picboxLRF
            // 
            this.picboxLRF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picboxLRF.Location = new System.Drawing.Point(12, 17);
            this.picboxLRF.Name = "picboxLRF";
            this.picboxLRF.Size = new System.Drawing.Size(316, 302);
            this.picboxLRF.TabIndex = 0;
            this.picboxLRF.TabStop = false;
            this.picboxLRF.Click += new System.EventHandler(this.picboxLRF_Click);
            this.picboxLRF.Paint += new System.Windows.Forms.PaintEventHandler(this.picboxLRF_Paint);
            // 
            // picboxMap
            // 
            this.picboxMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picboxMap.Location = new System.Drawing.Point(343, 17);
            this.picboxMap.Name = "picboxMap";
            this.picboxMap.Size = new System.Drawing.Size(597, 495);
            this.picboxMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picboxMap.TabIndex = 1;
            this.picboxMap.TabStop = false;
            // 
            // tmr_Update
            // 
            this.tmr_Update.Interval = 25;
            this.tmr_Update.Tick += new System.EventHandler(this.tmr_Update_Tick);
            // 
            // LRFMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 529);
            this.Controls.Add(this.picboxMap);
            this.Controls.Add(this.picboxLRF);
            this.Name = "LRFMapForm";
            this.Text = "LRFMapForm";
            ((System.ComponentModel.ISupportInitialize)(this.picboxLRF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picboxMap)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picboxLRF;
        private System.Windows.Forms.PictureBox picboxMap;
        private System.Windows.Forms.Timer tmr_Update;
    }
}