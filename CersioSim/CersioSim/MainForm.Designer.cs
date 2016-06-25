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
            this.tBarScale = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_ScaleVal = new System.Windows.Forms.Label();
            this.cb_TraceView = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_Status = new System.Windows.Forms.TabPage();
            this.grp_Control = new System.Windows.Forms.GroupBox();
            this.lbl_CtrlHandle = new System.Windows.Forms.Label();
            this.lbl_CtrlAcc = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lbl_LEDNo = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lbl_GPSGrandY = new System.Windows.Forms.Label();
            this.lbl_GPSGrandX = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblCompus = new System.Windows.Forms.Label();
            this.grp_RePlot = new System.Windows.Forms.GroupBox();
            this.lbl_ReL = new System.Windows.Forms.Label();
            this.lbl_ReR = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_RePlotAng = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_RePlotY = new System.Windows.Forms.Label();
            this.lbl_RePlotX = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage_CarStatus = new System.Windows.Forms.TabPage();
            this.lbl_PosRR = new System.Windows.Forms.Label();
            this.lbl_PosRL = new System.Windows.Forms.Label();
            this.lbl_PosRear = new System.Windows.Forms.Label();
            this.lbl_PosFR = new System.Windows.Forms.Label();
            this.lbl_PosFront = new System.Windows.Forms.Label();
            this.lbl_PosFL = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cb_LRFForm = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_SimArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_MsController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBarScale)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage_Status.SuspendLayout();
            this.grp_Control.SuspendLayout();
            this.grp_RePlot.SuspendLayout();
            this.tabPage_CarStatus.SuspendLayout();
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
            this.lbl_HandleVal.Location = new System.Drawing.Point(790, 420);
            this.lbl_HandleVal.Name = "lbl_HandleVal";
            this.lbl_HandleVal.Size = new System.Drawing.Size(73, 12);
            this.lbl_HandleVal.TabIndex = 2;
            this.lbl_HandleVal.Text = "lbl_HandleVal";
            // 
            // lbl_AccVal
            // 
            this.lbl_AccVal.AutoSize = true;
            this.lbl_AccVal.Location = new System.Drawing.Point(790, 394);
            this.lbl_AccVal.Name = "lbl_AccVal";
            this.lbl_AccVal.Size = new System.Drawing.Size(58, 12);
            this.lbl_AccVal.TabIndex = 3;
            this.lbl_AccVal.Text = "lbl_AccVal";
            // 
            // lbl_CarX
            // 
            this.lbl_CarX.AutoSize = true;
            this.lbl_CarX.Location = new System.Drawing.Point(17, 11);
            this.lbl_CarX.Name = "lbl_CarX";
            this.lbl_CarX.Size = new System.Drawing.Size(46, 12);
            this.lbl_CarX.TabIndex = 5;
            this.lbl_CarX.Text = "lbl_CarX";
            // 
            // lbl_CarY
            // 
            this.lbl_CarY.AutoSize = true;
            this.lbl_CarY.Location = new System.Drawing.Point(17, 30);
            this.lbl_CarY.Name = "lbl_CarY";
            this.lbl_CarY.Size = new System.Drawing.Size(46, 12);
            this.lbl_CarY.TabIndex = 6;
            this.lbl_CarY.Text = "lbl_CarY";
            // 
            // lbl_Speed
            // 
            this.lbl_Speed.AutoSize = true;
            this.lbl_Speed.Location = new System.Drawing.Point(790, 448);
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
            // tBarScale
            // 
            this.tBarScale.Location = new System.Drawing.Point(51, 498);
            this.tBarScale.Name = "tBarScale";
            this.tBarScale.Size = new System.Drawing.Size(299, 45);
            this.tBarScale.TabIndex = 11;
            this.tBarScale.Scroll += new System.EventHandler(this.tBarScale_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 508);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "Scale";
            // 
            // lbl_ScaleVal
            // 
            this.lbl_ScaleVal.AutoSize = true;
            this.lbl_ScaleVal.Location = new System.Drawing.Point(357, 508);
            this.lbl_ScaleVal.Name = "lbl_ScaleVal";
            this.lbl_ScaleVal.Size = new System.Drawing.Size(50, 12);
            this.lbl_ScaleVal.TabIndex = 13;
            this.lbl_ScaleVal.Text = "ScaleVal";
            // 
            // cb_TraceView
            // 
            this.cb_TraceView.AutoSize = true;
            this.cb_TraceView.Checked = true;
            this.cb_TraceView.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_TraceView.Location = new System.Drawing.Point(472, 504);
            this.cb_TraceView.Name = "cb_TraceView";
            this.cb_TraceView.Size = new System.Drawing.Size(73, 16);
            this.cb_TraceView.TabIndex = 14;
            this.cb_TraceView.Text = "View追従";
            this.cb_TraceView.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_Status);
            this.tabControl1.Controls.Add(this.tabPage_CarStatus);
            this.tabControl1.Location = new System.Drawing.Point(662, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(221, 348);
            this.tabControl1.TabIndex = 15;
            // 
            // tabPage_Status
            // 
            this.tabPage_Status.Controls.Add(this.grp_Control);
            this.tabPage_Status.Controls.Add(this.lbl_GPSGrandY);
            this.tabPage_Status.Controls.Add(this.lbl_GPSGrandX);
            this.tabPage_Status.Controls.Add(this.label8);
            this.tabPage_Status.Controls.Add(this.label7);
            this.tabPage_Status.Controls.Add(this.lblCompus);
            this.tabPage_Status.Controls.Add(this.grp_RePlot);
            this.tabPage_Status.Controls.Add(this.label6);
            this.tabPage_Status.Controls.Add(this.lbl_CarX);
            this.tabPage_Status.Controls.Add(this.lbl_CarY);
            this.tabPage_Status.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Status.Name = "tabPage_Status";
            this.tabPage_Status.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Status.Size = new System.Drawing.Size(213, 322);
            this.tabPage_Status.TabIndex = 0;
            this.tabPage_Status.Text = "センサー情報";
            this.tabPage_Status.UseVisualStyleBackColor = true;
            // 
            // grp_Control
            // 
            this.grp_Control.Controls.Add(this.lbl_CtrlHandle);
            this.grp_Control.Controls.Add(this.lbl_CtrlAcc);
            this.grp_Control.Controls.Add(this.label13);
            this.grp_Control.Controls.Add(this.label12);
            this.grp_Control.Controls.Add(this.lbl_LEDNo);
            this.grp_Control.Controls.Add(this.label11);
            this.grp_Control.Location = new System.Drawing.Point(9, 265);
            this.grp_Control.Name = "grp_Control";
            this.grp_Control.Size = new System.Drawing.Size(192, 49);
            this.grp_Control.TabIndex = 19;
            this.grp_Control.TabStop = false;
            this.grp_Control.Text = "コントロール";
            // 
            // lbl_CtrlHandle
            // 
            this.lbl_CtrlHandle.AutoSize = true;
            this.lbl_CtrlHandle.Location = new System.Drawing.Point(68, 30);
            this.lbl_CtrlHandle.Name = "lbl_CtrlHandle";
            this.lbl_CtrlHandle.Size = new System.Drawing.Size(22, 12);
            this.lbl_CtrlHandle.TabIndex = 22;
            this.lbl_CtrlHandle.Text = "Hdl";
            // 
            // lbl_CtrlAcc
            // 
            this.lbl_CtrlAcc.AutoSize = true;
            this.lbl_CtrlAcc.Location = new System.Drawing.Point(68, 15);
            this.lbl_CtrlAcc.Name = "lbl_CtrlAcc";
            this.lbl_CtrlAcc.Size = new System.Drawing.Size(25, 12);
            this.lbl_CtrlAcc.TabIndex = 21;
            this.lbl_CtrlAcc.Text = "Acc";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(19, 30);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(43, 12);
            this.label13.TabIndex = 20;
            this.label13.Text = "ハンドル";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(19, 15);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(42, 12);
            this.label12.TabIndex = 19;
            this.label12.Text = "アクセル";
            // 
            // lbl_LEDNo
            // 
            this.lbl_LEDNo.AutoSize = true;
            this.lbl_LEDNo.Location = new System.Drawing.Point(147, 15);
            this.lbl_LEDNo.Name = "lbl_LEDNo";
            this.lbl_LEDNo.Size = new System.Drawing.Size(26, 12);
            this.lbl_LEDNo.TabIndex = 18;
            this.lbl_LEDNo.Text = "LED";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(111, 15);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(26, 12);
            this.label11.TabIndex = 17;
            this.label11.Text = "LED";
            // 
            // lbl_GPSGrandY
            // 
            this.lbl_GPSGrandY.AutoSize = true;
            this.lbl_GPSGrandY.Location = new System.Drawing.Point(88, 249);
            this.lbl_GPSGrandY.Name = "lbl_GPSGrandY";
            this.lbl_GPSGrandY.Size = new System.Drawing.Size(64, 12);
            this.lbl_GPSGrandY.TabIndex = 16;
            this.lbl_GPSGrandY.Text = "GPSGrandY";
            // 
            // lbl_GPSGrandX
            // 
            this.lbl_GPSGrandX.AutoSize = true;
            this.lbl_GPSGrandX.Location = new System.Drawing.Point(88, 228);
            this.lbl_GPSGrandX.Name = "lbl_GPSGrandX";
            this.lbl_GPSGrandX.Size = new System.Drawing.Size(64, 12);
            this.lbl_GPSGrandX.TabIndex = 15;
            this.lbl_GPSGrandX.Text = "GPSGrandX";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 249);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "GPS緯度";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 228);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "GPS経度";
            // 
            // lblCompus
            // 
            this.lblCompus.AutoSize = true;
            this.lblCompus.Location = new System.Drawing.Point(88, 204);
            this.lblCompus.Name = "lblCompus";
            this.lblCompus.Size = new System.Drawing.Size(54, 12);
            this.lblCompus.TabIndex = 12;
            this.lblCompus.Text = "CompAng";
            // 
            // grp_RePlot
            // 
            this.grp_RePlot.Controls.Add(this.lbl_ReL);
            this.grp_RePlot.Controls.Add(this.lbl_ReR);
            this.grp_RePlot.Controls.Add(this.label10);
            this.grp_RePlot.Controls.Add(this.label9);
            this.grp_RePlot.Controls.Add(this.label5);
            this.grp_RePlot.Controls.Add(this.lbl_RePlotAng);
            this.grp_RePlot.Controls.Add(this.label3);
            this.grp_RePlot.Controls.Add(this.label4);
            this.grp_RePlot.Controls.Add(this.lbl_RePlotY);
            this.grp_RePlot.Controls.Add(this.lbl_RePlotX);
            this.grp_RePlot.Location = new System.Drawing.Point(9, 60);
            this.grp_RePlot.Name = "grp_RePlot";
            this.grp_RePlot.Size = new System.Drawing.Size(188, 129);
            this.grp_RePlot.TabIndex = 11;
            this.grp_RePlot.TabStop = false;
            this.grp_RePlot.Text = "ロータリーエンコーダ";
            // 
            // lbl_ReL
            // 
            this.lbl_ReL.AutoSize = true;
            this.lbl_ReL.Location = new System.Drawing.Point(68, 105);
            this.lbl_ReL.Name = "lbl_ReL";
            this.lbl_ReL.Size = new System.Drawing.Size(25, 12);
            this.lbl_ReL.TabIndex = 18;
            this.lbl_ReL.Text = "ReL";
            // 
            // lbl_ReR
            // 
            this.lbl_ReR.AutoSize = true;
            this.lbl_ReR.Location = new System.Drawing.Point(68, 88);
            this.lbl_ReR.Name = "lbl_ReR";
            this.lbl_ReR.Size = new System.Drawing.Size(27, 12);
            this.lbl_ReR.TabIndex = 17;
            this.lbl_ReR.Text = "ReR";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 105);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 12);
            this.label10.TabIndex = 16;
            this.label10.Text = "REnc L";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 88);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 12);
            this.label9.TabIndex = 15;
            this.label9.Text = "REnc R";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "向き";
            // 
            // lbl_RePlotAng
            // 
            this.lbl_RePlotAng.AutoSize = true;
            this.lbl_RePlotAng.Location = new System.Drawing.Point(59, 65);
            this.lbl_RePlotAng.Name = "lbl_RePlotAng";
            this.lbl_RePlotAng.Size = new System.Drawing.Size(75, 12);
            this.lbl_RePlotAng.TabIndex = 9;
            this.lbl_RePlotAng.Text = "lbl_RePlotAng";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "PlotX";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "PlotY";
            // 
            // lbl_RePlotY
            // 
            this.lbl_RePlotY.AutoSize = true;
            this.lbl_RePlotY.Location = new System.Drawing.Point(59, 44);
            this.lbl_RePlotY.Name = "lbl_RePlotY";
            this.lbl_RePlotY.Size = new System.Drawing.Size(62, 12);
            this.lbl_RePlotY.TabIndex = 8;
            this.lbl_RePlotY.Text = "lbl_RePlotY";
            // 
            // lbl_RePlotX
            // 
            this.lbl_RePlotX.AutoSize = true;
            this.lbl_RePlotX.Location = new System.Drawing.Point(59, 23);
            this.lbl_RePlotX.Name = "lbl_RePlotX";
            this.lbl_RePlotX.Size = new System.Drawing.Size(62, 12);
            this.lbl_RePlotX.TabIndex = 7;
            this.lbl_RePlotX.Text = "lbl_RePlotX";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 205);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "電子コンパス";
            // 
            // tabPage_CarStatus
            // 
            this.tabPage_CarStatus.Controls.Add(this.lbl_PosRR);
            this.tabPage_CarStatus.Controls.Add(this.lbl_PosRL);
            this.tabPage_CarStatus.Controls.Add(this.lbl_PosRear);
            this.tabPage_CarStatus.Controls.Add(this.lbl_PosFR);
            this.tabPage_CarStatus.Controls.Add(this.lbl_PosFront);
            this.tabPage_CarStatus.Controls.Add(this.lbl_PosFL);
            this.tabPage_CarStatus.Controls.Add(this.label14);
            this.tabPage_CarStatus.Location = new System.Drawing.Point(4, 22);
            this.tabPage_CarStatus.Name = "tabPage_CarStatus";
            this.tabPage_CarStatus.Size = new System.Drawing.Size(213, 322);
            this.tabPage_CarStatus.TabIndex = 1;
            this.tabPage_CarStatus.Text = "車体情報";
            this.tabPage_CarStatus.UseVisualStyleBackColor = true;
            // 
            // lbl_PosRR
            // 
            this.lbl_PosRR.AutoSize = true;
            this.lbl_PosRR.Location = new System.Drawing.Point(102, 168);
            this.lbl_PosRR.Name = "lbl_PosRR";
            this.lbl_PosRR.Size = new System.Drawing.Size(41, 12);
            this.lbl_PosRR.TabIndex = 6;
            this.lbl_PosRR.Text = "label20";
            // 
            // lbl_PosRL
            // 
            this.lbl_PosRL.AutoSize = true;
            this.lbl_PosRL.Location = new System.Drawing.Point(3, 168);
            this.lbl_PosRL.Name = "lbl_PosRL";
            this.lbl_PosRL.Size = new System.Drawing.Size(41, 12);
            this.lbl_PosRL.TabIndex = 5;
            this.lbl_PosRL.Text = "label19";
            // 
            // lbl_PosRear
            // 
            this.lbl_PosRear.AutoSize = true;
            this.lbl_PosRear.Location = new System.Drawing.Point(29, 123);
            this.lbl_PosRear.Name = "lbl_PosRear";
            this.lbl_PosRear.Size = new System.Drawing.Size(41, 12);
            this.lbl_PosRear.TabIndex = 4;
            this.lbl_PosRear.Text = "label18";
            // 
            // lbl_PosFR
            // 
            this.lbl_PosFR.AutoSize = true;
            this.lbl_PosFR.Location = new System.Drawing.Point(102, 51);
            this.lbl_PosFR.Name = "lbl_PosFR";
            this.lbl_PosFR.Size = new System.Drawing.Size(41, 12);
            this.lbl_PosFR.TabIndex = 3;
            this.lbl_PosFR.Text = "label17";
            // 
            // lbl_PosFront
            // 
            this.lbl_PosFront.AutoSize = true;
            this.lbl_PosFront.Location = new System.Drawing.Point(29, 86);
            this.lbl_PosFront.Name = "lbl_PosFront";
            this.lbl_PosFront.Size = new System.Drawing.Size(41, 12);
            this.lbl_PosFront.TabIndex = 2;
            this.lbl_PosFront.Text = "label16";
            // 
            // lbl_PosFL
            // 
            this.lbl_PosFL.AutoSize = true;
            this.lbl_PosFL.Location = new System.Drawing.Point(3, 51);
            this.lbl_PosFL.Name = "lbl_PosFL";
            this.lbl_PosFL.Size = new System.Drawing.Size(41, 12);
            this.lbl_PosFL.TabIndex = 1;
            this.lbl_PosFL.Text = "label15";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(72, 17);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 0;
            this.label14.Text = "車体座標";
            // 
            // cb_LRFForm
            // 
            this.cb_LRFForm.AutoSize = true;
            this.cb_LRFForm.Location = new System.Drawing.Point(577, 504);
            this.cb_LRFForm.Name = "cb_LRFForm";
            this.cb_LRFForm.Size = new System.Drawing.Size(75, 16);
            this.cb_LRFForm.TabIndex = 16;
            this.cb_LRFForm.Text = "LRF Form";
            this.cb_LRFForm.UseVisualStyleBackColor = true;
            this.cb_LRFForm.CheckedChanged += new System.EventHandler(this.cb_LRFForm_CheckedChanged);
            // 
            // CersioSimForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 543);
            this.Controls.Add(this.cb_LRFForm);
            this.Controls.Add(this.lbl_Speed);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cb_TraceView);
            this.Controls.Add(this.lbl_ScaleVal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tBarScale);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picbox_MsController);
            this.Controls.Add(this.lbl_AccVal);
            this.Controls.Add(this.lbl_HandleVal);
            this.Controls.Add(this.picbox_SimArea);
            this.Name = "CersioSimForm";
            this.Text = "TKBC2016 Cersio Simurator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CersioSimForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.picbox_SimArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_MsController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBarScale)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage_Status.ResumeLayout(false);
            this.tabPage_Status.PerformLayout();
            this.grp_Control.ResumeLayout(false);
            this.grp_Control.PerformLayout();
            this.grp_RePlot.ResumeLayout(false);
            this.grp_RePlot.PerformLayout();
            this.tabPage_CarStatus.ResumeLayout(false);
            this.tabPage_CarStatus.PerformLayout();
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
        private System.Windows.Forms.TrackBar tBarScale;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_ScaleVal;
        private System.Windows.Forms.CheckBox cb_TraceView;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_Status;
        private System.Windows.Forms.Label lblCompus;
        private System.Windows.Forms.GroupBox grp_RePlot;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbl_RePlotAng;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_RePlotY;
        private System.Windows.Forms.Label lbl_RePlotX;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbl_LEDNo;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lbl_GPSGrandY;
        private System.Windows.Forms.Label lbl_GPSGrandX;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbl_ReL;
        private System.Windows.Forms.Label lbl_ReR;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox grp_Control;
        private System.Windows.Forms.Label lbl_CtrlHandle;
        private System.Windows.Forms.Label lbl_CtrlAcc;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabPage tabPage_CarStatus;
        private System.Windows.Forms.Label lbl_PosRR;
        private System.Windows.Forms.Label lbl_PosRL;
        private System.Windows.Forms.Label lbl_PosRear;
        private System.Windows.Forms.Label lbl_PosFR;
        private System.Windows.Forms.Label lbl_PosFront;
        private System.Windows.Forms.Label lbl_PosFL;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox cb_LRFForm;
    }
}

