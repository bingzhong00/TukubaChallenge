namespace LRFMapEditer
{
    partial class MapEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapEditForm));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.pb_EditLayer = new System.Windows.Forms.PictureBox();
            this.MenuStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.NewMapLRFLoadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.lOADMapDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.sAVEMapDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sAVEMapImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportCheckPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.MapRotateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.sb_VMapLayer = new System.Windows.Forms.HScrollBar();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_NumLayer = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.num_PositionX = new System.Windows.Forms.NumericUpDown();
            this.num_PositionY = new System.Windows.Forms.NumericUpDown();
            this.num_Angle = new System.Windows.Forms.NumericUpDown();
            this.num_Layer = new System.Windows.Forms.NumericUpDown();
            this.tb_SkipFrame = new System.Windows.Forms.TextBox();
            this.tb_LogFileName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lbl_ViewPosition = new System.Windows.Forms.Label();
            this.tber_ViewScale = new System.Windows.Forms.TrackBar();
            this.lbl_ViewScale = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_UseLayer = new System.Windows.Forms.CheckBox();
            this.tmr_MapUpdate = new System.Windows.Forms.Timer(this.components);
            this.pb_VMap = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cb_REDisp = new System.Windows.Forms.CheckBox();
            this.tb_RELogFile = new System.Windows.Forms.TextBox();
            this.btn_RELoad = new System.Windows.Forms.Button();
            this.num_RERotate = new System.Windows.Forms.NumericUpDown();
            this.tabCtrl = new System.Windows.Forms.TabControl();
            this.tabPage_MapEdit = new System.Windows.Forms.TabPage();
            this.tabPage_CheckPoint = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_DisSelect = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_StartDir = new System.Windows.Forms.Label();
            this.lbl_StartY = new System.Windows.Forms.Label();
            this.lbl_StartX = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.btn_Delete = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbtn_Edit = new System.Windows.Forms.RadioButton();
            this.rbtn_AddPoint = new System.Windows.Forms.RadioButton();
            this.listbox_CheckPoint = new System.Windows.Forms.ListBox();
            this.pb_CPMap = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pb_EditLayer)).BeginInit();
            this.MenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_PositionX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_PositionY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Angle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Layer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tber_ViewScale)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_VMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_RERotate)).BeginInit();
            this.tabCtrl.SuspendLayout();
            this.tabPage_MapEdit.SuspendLayout();
            this.tabPage_CheckPoint.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_CPMap)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 763);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1384, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // pb_EditLayer
            // 
            this.pb_EditLayer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_EditLayer.Location = new System.Drawing.Point(14, 6);
            this.pb_EditLayer.Name = "pb_EditLayer";
            this.pb_EditLayer.Size = new System.Drawing.Size(350, 350);
            this.pb_EditLayer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_EditLayer.TabIndex = 2;
            this.pb_EditLayer.TabStop = false;
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripSeparator1,
            this.toolStripSplitButton1});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(1384, 25);
            this.MenuStrip.TabIndex = 6;
            this.MenuStrip.Text = "ts_Menu";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewMapLRFLoadToolStripMenuItem,
            this.toolStripSeparator5,
            this.lOADMapDataToolStripMenuItem,
            this.toolStripSeparator4,
            this.sAVEMapDataToolStripMenuItem,
            this.sAVEMapImageToolStripMenuItem,
            this.exportCheckPointToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(99, 22);
            this.toolStripDropDownButton1.Text = "ファイル(File)";
            // 
            // NewMapLRFLoadToolStripMenuItem
            // 
            this.NewMapLRFLoadToolStripMenuItem.Name = "NewMapLRFLoadToolStripMenuItem";
            this.NewMapLRFLoadToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.NewMapLRFLoadToolStripMenuItem.Text = "New(LRF LogFile 読み込み)";
            this.NewMapLRFLoadToolStripMenuItem.Click += new System.EventHandler(this.ts_LoadLogData_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(228, 6);
            // 
            // lOADMapDataToolStripMenuItem
            // 
            this.lOADMapDataToolStripMenuItem.Name = "lOADMapDataToolStripMenuItem";
            this.lOADMapDataToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.lOADMapDataToolStripMenuItem.Text = "LOAD MapData";
            this.lOADMapDataToolStripMenuItem.Click += new System.EventHandler(this.ts_LoadMapData_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(228, 6);
            // 
            // sAVEMapDataToolStripMenuItem
            // 
            this.sAVEMapDataToolStripMenuItem.Name = "sAVEMapDataToolStripMenuItem";
            this.sAVEMapDataToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.sAVEMapDataToolStripMenuItem.Text = "SAVE MapData";
            this.sAVEMapDataToolStripMenuItem.Click += new System.EventHandler(this.ts_SaveMapData_Click);
            // 
            // sAVEMapImageToolStripMenuItem
            // 
            this.sAVEMapImageToolStripMenuItem.Name = "sAVEMapImageToolStripMenuItem";
            this.sAVEMapImageToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.sAVEMapImageToolStripMenuItem.Text = "Export MapImage";
            this.sAVEMapImageToolStripMenuItem.Click += new System.EventHandler(this.ts_SaveMapImage_Click);
            // 
            // exportCheckPointToolStripMenuItem
            // 
            this.exportCheckPointToolStripMenuItem.Name = "exportCheckPointToolStripMenuItem";
            this.exportCheckPointToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.exportCheckPointToolStripMenuItem.Text = "Export CheckPoint";
            this.exportCheckPointToolStripMenuItem.Click += new System.EventHandler(this.exportCheckPointToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MapRotateToolStripMenuItem,
            this.toolStripSeparator2});
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(116, 22);
            this.toolStripSplitButton1.Text = "エディット(Edit)";
            // 
            // MapRotateToolStripMenuItem
            // 
            this.MapRotateToolStripMenuItem.Name = "MapRotateToolStripMenuItem";
            this.MapRotateToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.MapRotateToolStripMenuItem.Text = "Map回転";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(121, 6);
            // 
            // sb_VMapLayer
            // 
            this.sb_VMapLayer.Location = new System.Drawing.Point(238, 618);
            this.sb_VMapLayer.Maximum = 500;
            this.sb_VMapLayer.Name = "sb_VMapLayer";
            this.sb_VMapLayer.Size = new System.Drawing.Size(1134, 24);
            this.sb_VMapLayer.TabIndex = 7;
            this.sb_VMapLayer.ValueChanged += new System.EventHandler(this.sb_VMapLayer_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(161, 615);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 24);
            this.label1.TabIndex = 9;
            this.label1.Text = "/";
            // 
            // lb_NumLayer
            // 
            this.lb_NumLayer.AutoSize = true;
            this.lb_NumLayer.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb_NumLayer.Location = new System.Drawing.Point(189, 615);
            this.lb_NumLayer.Name = "lb_NumLayer";
            this.lb_NumLayer.Size = new System.Drawing.Size(46, 24);
            this.lb_NumLayer.TabIndex = 10;
            this.lb_NumLayer.Text = "000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(13, 367);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "移動  Mouse LButton+Move";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(13, 384);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "回転  Shift + Mouse Wheel";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(208, 366);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(182, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "View移動  Mouse RButton + Move";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Label5.Location = new System.Drawing.Point(6, 615);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(65, 24);
            this.Label5.TabIndex = 15;
            this.Label5.Text = "Layer";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(4, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 24);
            this.label6.TabIndex = 16;
            this.label6.Text = "Position X";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(4, 63);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(111, 24);
            this.label7.TabIndex = 17;
            this.label7.Text = "Position Y";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(46, 102);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 24);
            this.label8.TabIndex = 18;
            this.label8.Text = "Angle";
            // 
            // num_PositionX
            // 
            this.num_PositionX.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.num_PositionX.Location = new System.Drawing.Point(121, 18);
            this.num_PositionX.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.num_PositionX.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.num_PositionX.Name = "num_PositionX";
            this.num_PositionX.Size = new System.Drawing.Size(101, 31);
            this.num_PositionX.TabIndex = 22;
            this.num_PositionX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_PositionY
            // 
            this.num_PositionY.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.num_PositionY.Location = new System.Drawing.Point(121, 61);
            this.num_PositionY.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.num_PositionY.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.num_PositionY.Name = "num_PositionY";
            this.num_PositionY.Size = new System.Drawing.Size(101, 31);
            this.num_PositionY.TabIndex = 23;
            this.num_PositionY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_Angle
            // 
            this.num_Angle.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.num_Angle.Location = new System.Drawing.Point(121, 102);
            this.num_Angle.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.num_Angle.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.num_Angle.Name = "num_Angle";
            this.num_Angle.Size = new System.Drawing.Size(101, 31);
            this.num_Angle.TabIndex = 24;
            this.num_Angle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.num_Angle.ValueChanged += new System.EventHandler(this.num_Angle_ValueChanged);
            // 
            // num_Layer
            // 
            this.num_Layer.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.num_Layer.Location = new System.Drawing.Point(77, 613);
            this.num_Layer.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.num_Layer.Name = "num_Layer";
            this.num_Layer.Size = new System.Drawing.Size(78, 31);
            this.num_Layer.TabIndex = 26;
            this.num_Layer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.num_Layer.ValueChanged += new System.EventHandler(this.num_Layer_ValueChanged);
            // 
            // tb_SkipFrame
            // 
            this.tb_SkipFrame.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tb_SkipFrame.Location = new System.Drawing.Point(358, 430);
            this.tb_SkipFrame.Name = "tb_SkipFrame";
            this.tb_SkipFrame.Size = new System.Drawing.Size(84, 23);
            this.tb_SkipFrame.TabIndex = 30;
            this.tb_SkipFrame.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_SkipFrame_KeyDown);
            // 
            // tb_LogFileName
            // 
            this.tb_LogFileName.Location = new System.Drawing.Point(80, 33);
            this.tb_LogFileName.Name = "tb_LogFileName";
            this.tb_LogFileName.Size = new System.Drawing.Size(308, 19);
            this.tb_LogFileName.TabIndex = 27;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(2, 36);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(66, 12);
            this.label9.TabIndex = 26;
            this.label9.Text = "URGLogFile";
            // 
            // lbl_ViewPosition
            // 
            this.lbl_ViewPosition.AutoSize = true;
            this.lbl_ViewPosition.Location = new System.Drawing.Point(466, 36);
            this.lbl_ViewPosition.Name = "lbl_ViewPosition";
            this.lbl_ViewPosition.Size = new System.Drawing.Size(96, 12);
            this.lbl_ViewPosition.TabIndex = 29;
            this.lbl_ViewPosition.Text = "View x1000,y1000";
            // 
            // tber_ViewScale
            // 
            this.tber_ViewScale.LargeChange = 10;
            this.tber_ViewScale.Location = new System.Drawing.Point(676, 28);
            this.tber_ViewScale.Maximum = 300;
            this.tber_ViewScale.Minimum = 10;
            this.tber_ViewScale.Name = "tber_ViewScale";
            this.tber_ViewScale.Size = new System.Drawing.Size(692, 45);
            this.tber_ViewScale.SmallChange = 5;
            this.tber_ViewScale.TabIndex = 30;
            this.tber_ViewScale.TickFrequency = 5;
            this.tber_ViewScale.Value = 100;
            this.tber_ViewScale.ValueChanged += new System.EventHandler(this.tber_MapScale_ValueChanged);
            this.tber_ViewScale.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tber_MapScale_MouseUp);
            // 
            // lbl_ViewScale
            // 
            this.lbl_ViewScale.AutoSize = true;
            this.lbl_ViewScale.Location = new System.Drawing.Point(586, 36);
            this.lbl_ViewScale.Name = "lbl_ViewScale";
            this.lbl_ViewScale.Size = new System.Drawing.Size(84, 12);
            this.lbl_ViewScale.TabIndex = 31;
            this.lbl_ViewScale.Text = "ViewScale:100%";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cb_UseLayer);
            this.groupBox2.Controls.Add(this.num_Angle);
            this.groupBox2.Controls.Add(this.num_PositionY);
            this.groupBox2.Controls.Add(this.num_PositionX);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(6, 459);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(441, 147);
            this.groupBox2.TabIndex = 32;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Layer";
            // 
            // cb_UseLayer
            // 
            this.cb_UseLayer.AutoSize = true;
            this.cb_UseLayer.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cb_UseLayer.Location = new System.Drawing.Point(299, 18);
            this.cb_UseLayer.Name = "cb_UseLayer";
            this.cb_UseLayer.Size = new System.Drawing.Size(95, 28);
            this.cb_UseLayer.TabIndex = 25;
            this.cb_UseLayer.Text = "Enable";
            this.cb_UseLayer.UseVisualStyleBackColor = true;
            this.cb_UseLayer.CheckedChanged += new System.EventHandler(this.cb_UseLayer_CheckedChanged);
            this.cb_UseLayer.Click += new System.EventHandler(this.cb_UseLayer_Click);
            // 
            // tmr_MapUpdate
            // 
            this.tmr_MapUpdate.Tick += new System.EventHandler(this.tmr_MapUpdate_Tick);
            // 
            // pb_VMap
            // 
            this.pb_VMap.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pb_VMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_VMap.Location = new System.Drawing.Point(465, 8);
            this.pb_VMap.Name = "pb_VMap";
            this.pb_VMap.Size = new System.Drawing.Size(900, 600);
            this.pb_VMap.TabIndex = 5;
            this.pb_VMap.TabStop = false;
            this.pb_VMap.Click += new System.EventHandler(this.pb_VMap_Click);
            this.pb_VMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pb_VMap_MouseDown);
            this.pb_VMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pb_VMap_MouseMove);
            this.pb_VMap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pb_VMap_MouseUp);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.Location = new System.Drawing.Point(208, 384);
            this.label10.Name = "label10";
            this.label10.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label10.Size = new System.Drawing.Size(242, 12);
            this.label10.TabIndex = 33;
            this.label10.Text = "Viewスケール  Shift + Mouse  RButton + Move ";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label12.Location = new System.Drawing.Point(208, 401);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(136, 12);
            this.label12.TabIndex = 34;
            this.label12.Text = "Layer選択  Mouse Wheel ";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label13.Location = new System.Drawing.Point(13, 401);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(181, 12);
            this.label13.TabIndex = 35;
            this.label13.Text = "Layer間 距離  Ctrl + Mouse Wheel";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(2, 61);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(74, 12);
            this.label11.TabIndex = 36;
            this.label11.Text = "R.EncoderLog";
            // 
            // cb_REDisp
            // 
            this.cb_REDisp.AutoSize = true;
            this.cb_REDisp.Location = new System.Drawing.Point(468, 63);
            this.cb_REDisp.Name = "cb_REDisp";
            this.cb_REDisp.Size = new System.Drawing.Size(98, 16);
            this.cb_REDisp.TabIndex = 37;
            this.cb_REDisp.Text = "R.EncoderDisp";
            this.cb_REDisp.UseVisualStyleBackColor = true;
            this.cb_REDisp.CheckedChanged += new System.EventHandler(this.cb_REDisp_CheckedChanged);
            // 
            // tb_RELogFile
            // 
            this.tb_RELogFile.Location = new System.Drawing.Point(82, 58);
            this.tb_RELogFile.Name = "tb_RELogFile";
            this.tb_RELogFile.Size = new System.Drawing.Size(306, 19);
            this.tb_RELogFile.TabIndex = 38;
            // 
            // btn_RELoad
            // 
            this.btn_RELoad.Location = new System.Drawing.Point(396, 56);
            this.btn_RELoad.Name = "btn_RELoad";
            this.btn_RELoad.Size = new System.Drawing.Size(37, 23);
            this.btn_RELoad.TabIndex = 39;
            this.btn_RELoad.Text = "...";
            this.btn_RELoad.UseVisualStyleBackColor = true;
            this.btn_RELoad.Click += new System.EventHandler(this.btn_RELoad_Click);
            // 
            // num_RERotate
            // 
            this.num_RERotate.Location = new System.Drawing.Point(572, 62);
            this.num_RERotate.Maximum = new decimal(new int[] {
            359,
            0,
            0,
            0});
            this.num_RERotate.Name = "num_RERotate";
            this.num_RERotate.Size = new System.Drawing.Size(56, 19);
            this.num_RERotate.TabIndex = 40;
            this.num_RERotate.ValueChanged += new System.EventHandler(this.num_RERotate_ValueChanged);
            // 
            // tabCtrl
            // 
            this.tabCtrl.Controls.Add(this.tabPage_MapEdit);
            this.tabCtrl.Controls.Add(this.tabPage_CheckPoint);
            this.tabCtrl.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tabCtrl.Location = new System.Drawing.Point(0, 87);
            this.tabCtrl.Name = "tabCtrl";
            this.tabCtrl.SelectedIndex = 0;
            this.tabCtrl.Size = new System.Drawing.Size(1384, 676);
            this.tabCtrl.TabIndex = 41;
            this.tabCtrl.SelectedIndexChanged += new System.EventHandler(this.tabCtrl_SelectedIndexChanged);
            // 
            // tabPage_MapEdit
            // 
            this.tabPage_MapEdit.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_MapEdit.Controls.Add(this.groupBox2);
            this.tabPage_MapEdit.Controls.Add(this.sb_VMapLayer);
            this.tabPage_MapEdit.Controls.Add(this.label1);
            this.tabPage_MapEdit.Controls.Add(this.lb_NumLayer);
            this.tabPage_MapEdit.Controls.Add(this.Label5);
            this.tabPage_MapEdit.Controls.Add(this.tb_SkipFrame);
            this.tabPage_MapEdit.Controls.Add(this.label13);
            this.tabPage_MapEdit.Controls.Add(this.num_Layer);
            this.tabPage_MapEdit.Controls.Add(this.pb_EditLayer);
            this.tabPage_MapEdit.Controls.Add(this.label2);
            this.tabPage_MapEdit.Controls.Add(this.label12);
            this.tabPage_MapEdit.Controls.Add(this.label3);
            this.tabPage_MapEdit.Controls.Add(this.label10);
            this.tabPage_MapEdit.Controls.Add(this.label4);
            this.tabPage_MapEdit.Controls.Add(this.pb_VMap);
            this.tabPage_MapEdit.Location = new System.Drawing.Point(4, 26);
            this.tabPage_MapEdit.Name = "tabPage_MapEdit";
            this.tabPage_MapEdit.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_MapEdit.Size = new System.Drawing.Size(1376, 646);
            this.tabPage_MapEdit.TabIndex = 0;
            this.tabPage_MapEdit.Text = "MapEdit";
            // 
            // tabPage_CheckPoint
            // 
            this.tabPage_CheckPoint.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_CheckPoint.Controls.Add(this.groupBox3);
            this.tabPage_CheckPoint.Controls.Add(this.pb_CPMap);
            this.tabPage_CheckPoint.Location = new System.Drawing.Point(4, 26);
            this.tabPage_CheckPoint.Name = "tabPage_CheckPoint";
            this.tabPage_CheckPoint.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_CheckPoint.Size = new System.Drawing.Size(1376, 646);
            this.tabPage_CheckPoint.TabIndex = 1;
            this.tabPage_CheckPoint.Text = "CheckPoint";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_DisSelect);
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.btn_Delete);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.listbox_CheckPoint);
            this.groupBox3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox3.Location = new System.Drawing.Point(918, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(450, 391);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "チェックポイント設定";
            // 
            // btn_DisSelect
            // 
            this.btn_DisSelect.Location = new System.Drawing.Point(248, 103);
            this.btn_DisSelect.Name = "btn_DisSelect";
            this.btn_DisSelect.Size = new System.Drawing.Size(81, 32);
            this.btn_DisSelect.TabIndex = 13;
            this.btn_DisSelect.Text = "選択解除";
            this.btn_DisSelect.UseVisualStyleBackColor = true;
            this.btn_DisSelect.Click += new System.EventHandler(this.btn_DisSelect_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.lbl_StartDir);
            this.groupBox1.Controls.Add(this.lbl_StartY);
            this.groupBox1.Controls.Add(this.lbl_StartX);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox1.Location = new System.Drawing.Point(253, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(181, 79);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "スタート情報設定";
            // 
            // lbl_StartDir
            // 
            this.lbl_StartDir.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_StartDir.Location = new System.Drawing.Point(65, 48);
            this.lbl_StartDir.Name = "lbl_StartDir";
            this.lbl_StartDir.Size = new System.Drawing.Size(47, 17);
            this.lbl_StartDir.TabIndex = 7;
            this.lbl_StartDir.Text = "0";
            this.lbl_StartDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_StartY
            // 
            this.lbl_StartY.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_StartY.Location = new System.Drawing.Point(118, 20);
            this.lbl_StartY.Name = "lbl_StartY";
            this.lbl_StartY.Size = new System.Drawing.Size(51, 17);
            this.lbl_StartY.TabIndex = 6;
            this.lbl_StartY.Text = "0";
            this.lbl_StartY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_StartX
            // 
            this.lbl_StartX.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_StartX.Location = new System.Drawing.Point(65, 20);
            this.lbl_StartX.Name = "lbl_StartX";
            this.lbl_StartX.Size = new System.Drawing.Size(47, 17);
            this.lbl_StartX.TabIndex = 5;
            this.lbl_StartX.Text = "0";
            this.lbl_StartX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(33, 50);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(26, 12);
            this.label15.TabIndex = 1;
            this.label15.Text = "向き";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 0;
            this.label14.Text = "Ｘ，Ｙ座標";
            // 
            // btn_Delete
            // 
            this.btn_Delete.Location = new System.Drawing.Point(248, 340);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(81, 31);
            this.btn_Delete.TabIndex = 12;
            this.btn_Delete.Text = "Delete";
            this.btn_Delete.UseVisualStyleBackColor = true;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbtn_Edit);
            this.groupBox4.Controls.Add(this.rbtn_AddPoint);
            this.groupBox4.Location = new System.Drawing.Point(14, 35);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(228, 60);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Mode";
            // 
            // rbtn_Edit
            // 
            this.rbtn_Edit.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtn_Edit.Location = new System.Drawing.Point(122, 18);
            this.rbtn_Edit.Name = "rbtn_Edit";
            this.rbtn_Edit.Size = new System.Drawing.Size(77, 31);
            this.rbtn_Edit.TabIndex = 10;
            this.rbtn_Edit.Text = "Edit";
            this.rbtn_Edit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtn_Edit.UseVisualStyleBackColor = true;
            // 
            // rbtn_AddPoint
            // 
            this.rbtn_AddPoint.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtn_AddPoint.Checked = true;
            this.rbtn_AddPoint.Location = new System.Drawing.Point(20, 18);
            this.rbtn_AddPoint.Name = "rbtn_AddPoint";
            this.rbtn_AddPoint.Size = new System.Drawing.Size(71, 31);
            this.rbtn_AddPoint.TabIndex = 9;
            this.rbtn_AddPoint.TabStop = true;
            this.rbtn_AddPoint.Text = "Add";
            this.rbtn_AddPoint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtn_AddPoint.UseVisualStyleBackColor = true;
            this.rbtn_AddPoint.CheckedChanged += new System.EventHandler(this.rbtn_AddPoint_CheckedChanged);
            // 
            // listbox_CheckPoint
            // 
            this.listbox_CheckPoint.FormattingEnabled = true;
            this.listbox_CheckPoint.ItemHeight = 12;
            this.listbox_CheckPoint.Location = new System.Drawing.Point(14, 103);
            this.listbox_CheckPoint.Name = "listbox_CheckPoint";
            this.listbox_CheckPoint.Size = new System.Drawing.Size(228, 268);
            this.listbox_CheckPoint.TabIndex = 8;
            this.listbox_CheckPoint.SelectedIndexChanged += new System.EventHandler(this.listbox_CheckPoint_SelectedIndexChanged);
            // 
            // pb_CPMap
            // 
            this.pb_CPMap.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pb_CPMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_CPMap.Location = new System.Drawing.Point(8, 20);
            this.pb_CPMap.Name = "pb_CPMap";
            this.pb_CPMap.Size = new System.Drawing.Size(900, 600);
            this.pb_CPMap.TabIndex = 6;
            this.pb_CPMap.TabStop = false;
            this.pb_CPMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pb_CPMap_MouseDown);
            this.pb_CPMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pb_CPMap_MouseMove);
            this.pb_CPMap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pb_CPMap_MouseUp);
            // 
            // MapEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1284, 782);
            this.Controls.Add(this.tabCtrl);
            this.Controls.Add(this.num_RERotate);
            this.Controls.Add(this.btn_RELoad);
            this.Controls.Add(this.tb_RELogFile);
            this.Controls.Add(this.cb_REDisp);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tb_LogFileName);
            this.Controls.Add(this.lbl_ViewScale);
            this.Controls.Add(this.tber_ViewScale);
            this.Controls.Add(this.lbl_ViewPosition);
            this.Controls.Add(this.MenuStrip);
            this.Controls.Add(this.statusStrip);
            this.Name = "MapEditForm";
            this.Text = "LRF MapEditer Ver0.80";
            this.Load += new System.EventHandler(this.MapEditForm_Load);
            this.Shown += new System.EventHandler(this.MapEditForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MapEditForm_KeyDown);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MapEditForm_PreviewKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pb_EditLayer)).EndInit();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_PositionX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_PositionY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Angle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Layer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tber_ViewScale)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_VMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_RERotate)).EndInit();
            this.tabCtrl.ResumeLayout(false);
            this.tabPage_MapEdit.ResumeLayout(false);
            this.tabPage_MapEdit.PerformLayout();
            this.tabPage_CheckPoint.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_CPMap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.PictureBox pb_EditLayer;
        private System.Windows.Forms.ToolStrip MenuStrip;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem NewMapLRFLoadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lOADMapDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sAVEMapDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sAVEMapImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.HScrollBar sb_VMapLayer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_NumLayer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label Label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown num_PositionX;
        private System.Windows.Forms.NumericUpDown num_PositionY;
        private System.Windows.Forms.NumericUpDown num_Angle;
        private System.Windows.Forms.NumericUpDown num_Layer;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem MapRotateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tb_LogFileName;
        private System.Windows.Forms.Label lbl_ViewPosition;
        private System.Windows.Forms.TrackBar tber_ViewScale;
        private System.Windows.Forms.Label lbl_ViewScale;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Timer tmr_MapUpdate;
        private System.Windows.Forms.PictureBox pb_VMap;
        private System.Windows.Forms.CheckBox cb_UseLayer;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tb_SkipFrame;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox cb_REDisp;
        private System.Windows.Forms.TextBox tb_RELogFile;
        private System.Windows.Forms.Button btn_RELoad;
        private System.Windows.Forms.NumericUpDown num_RERotate;
        private System.Windows.Forms.TabControl tabCtrl;
        private System.Windows.Forms.TabPage tabPage_MapEdit;
        private System.Windows.Forms.TabPage tabPage_CheckPoint;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_Delete;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rbtn_Edit;
        private System.Windows.Forms.RadioButton rbtn_AddPoint;
        private System.Windows.Forms.ListBox listbox_CheckPoint;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_StartY;
        private System.Windows.Forms.Label lbl_StartX;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.PictureBox pb_CPMap;
        private System.Windows.Forms.ToolStripMenuItem exportCheckPointToolStripMenuItem;
        private System.Windows.Forms.Label lbl_StartDir;
        private System.Windows.Forms.Button btn_DisSelect;
    }
}

