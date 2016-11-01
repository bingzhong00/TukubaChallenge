using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;

using System.Diagnostics;
using LocationPresumption;
using VRIpcLib;
using Navigation;

using Axiom.Math;


namespace CersioSim
{
    public partial class CersioSimForm : Form
    {
        public Bitmap SimAreaBmp;
        public Bitmap MapBmp;

        public MapData mapFileData;

        // mm からピクセルへの変換
        double ScalePixelToReal;    // mmを１ピクセルとする
        double ScaleRealToPixel;

        CarSim carSim;
        //MarkPoint carInitPos;
        MarkPoint carPos;

        // SLAM用フォーム
        // LRFデータをMap化
        LRFMapForm slamForm;

        // SLAMフォームを生成するか？
        private const bool useSlamForm = false;

        private double viewX;
        private double viewY;

        private double viewScale = 1.0;

        // 車の速度
        private double dbgSpeed = 1.0;// 8.0;

        /// <summary>
        /// bServerエミュレータ
        /// </summary>
        private bServer bSrv = new bServer();

        /// <summary>
        /// URG SCIPシミュレータ
        /// </summary>
        private SCIPsim UrgSim = new SCIPsim();

        /// <summary>
        /// ROS-IF Emu
        /// </summary>
        private IpcServer ipc;

        string defaultMapFileName = "../../../MapFile/syaoku201610/syaoku201610.xml";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CersioSimForm()
        {
            InitializeComponent();

            // マップファイル読み込み
            LoadMapFile(defaultMapFileName);

            if ( useSlamForm || cb_LRFForm.Checked)
            {
                // LRFフォーム生成
                slamForm = new LRFMapForm(carSim, MapBmp.Width, MapBmp.Height, ScaleRealToPixel);
                slamForm.Show();
            }

            // IP,Port表示
            lbl_bServerIP.Text = bSrv.listenIP + " : " + bSrv.listenPort.ToString();
            lbl_URGIP.Text = UrgSim.listenIP + " : " + UrgSim.listenPort.ToString();

            // bServer回線待ち
            bSrv.OpenAsync();

            // URG回線待ち
            UrgSim.OpenAsync();

            // ROS-IF Emu
            try
            {
                ipc = new IpcServer();
            }
            catch 
            {
                ipc = null;
            }

            tmr_Update.Enabled = true;
        }

        /// <summary>
        /// フォーム開始時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CersioSimForm_Load(object sender, EventArgs e)
        {
            tb_MapName.Text = mapFileData.MapName;
            tb_MapFileName.Text = Path.GetFileName(mapFileData.MapFileName);
            tb_MapImageFileName.Text = Path.GetFileName(mapFileData.MapImageFileName);
            tb_MapPixelScale.Text = ((int)ScalePixelToReal).ToString() + "mm";
        }

        /// <summary>
        /// 閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CersioSimForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bSrv.Close();

            UrgSim.Close();
        }

        /// <summary>
        /// クルマの中心にビューを移動
        /// </summary>
        public void SetView_CarCenter()
        {
            //画面中心に移動
            viewX = -(picbox_SimArea.Width / viewScale) / 2;
            viewY = -(picbox_SimArea.Height / viewScale) / 2;
            // クルマの位置
            //viewX += carPos.X * ScaleRealToPixel;
            //viewY += carPos.Y * ScaleRealToPixel;
            viewX += ((double)carSim.wdCarF.x) * ScaleRealToPixel;
            viewY += ((double)carSim.wdCarF.y) * ScaleRealToPixel;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadFile"></param>
        /// <returns></returns>
        private bool LoadMapFile(string loadFile )
        {
            // 
            mapFileData = MapData.LoadMapFile(loadFile);

            //シミュレーションマップエリア
            SimAreaBmp = new Bitmap(picbox_SimArea.Width, picbox_SimArea.Height);
            picbox_SimArea.Image = SimAreaBmp;

            // マップファイル読み込み
            //string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            //MapBmp = new Bitmap(path + "\\" + mapFile.MapImageFileName );
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            MapBmp = new Bitmap(mapFileData.MapImageFileName);

            // ピクセルスケール計算
            ScalePixelToReal = mapFileData.RealWidth / MapBmp.Width;    // １ピクセルを何mmにするか
            ScaleRealToPixel = 1.0 / ScalePixelToReal;

            SimCarInit();

            // クルマが中心にくるようにビューを設定する
            SetView_CarCenter();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SimCarInit()
        {
            // クルマの配置の初期状態
            MarkPoint carInitPos = new MarkPoint(mapFileData.startPosition.x * ScalePixelToReal,
                                                 mapFileData.startPosition.y * ScalePixelToReal,
                                                 mapFileData.startDir);


            // シムカー生成
            carPos = new MarkPoint(carInitPos.X, carInitPos.Y, carInitPos.Theta);

            carSim = new CarSim(ScalePixelToReal);
            carSim.CarInit(carInitPos);
            carSim.MapInit(MapBmp);
        }

        /// <summary>
        /// タイマー更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmr_Update_Tick(object sender, EventArgs e)
        {
            // ハンドル上限、下限
            if (carSim.carHandleAng > carSim.carHandleAngMax) carSim.carHandleAng = carSim.carHandleAngMax;
            if (carSim.carHandleAng < -carSim.carHandleAngMax) carSim.carHandleAng = -carSim.carHandleAngMax;

            if (Math.Abs(carSim.carAccVal) < 0.01) carSim.carAccVal = 0.0;
            if (Math.Abs(carSim.carHandleAng) < 0.01) carSim.carHandleAng = 0.0;

            lbl_HandleVal.Text = "ハンドル:" + carSim.carHandleAng.ToString("F2");
            lbl_AccVal.Text = "アクセル:" + carSim.carAccVal.ToString("F2");

            lbl_CarX.Text = "carX:" + ((double)carSim.mkp.X * ScaleRealToPixel).ToString("F2");
            lbl_CarY.Text = "carY:" + ((double)carSim.mkp.Y * ScaleRealToPixel).ToString("F2");
            lbl_Speed.Text = "Speed(Km):" + ((double)(4.0 * carSim.carAccVal)).ToString("F2");

            // CarSim更新
            // 位置情報更新
            carSim.calcTirePos(tmr_Update.Interval);

            // センサー情報更新
            carSim.SenserUpdate();


            // 車輪位置
            {
                lbl_PosFront.Text = "Front: " + ((double)carSim.wdCarF.x * ScaleRealToPixel).ToString("F2") + "/ "
                                              + ((double)carSim.wdCarF.y * ScaleRealToPixel).ToString("F2");

                lbl_PosRear.Text = "Rear: " + ((double)carSim.wdCarR.x * ScaleRealToPixel).ToString("F2") + "/ "
                                            + ((double)carSim.wdCarR.y * ScaleRealToPixel).ToString("F2");

                lbl_PosFL.Text = "FL: " + ((double)carSim.wdFL.x * ScaleRealToPixel).ToString("F2") + "/ "
                                        + ((double)carSim.wdFL.y * ScaleRealToPixel).ToString("F2");

                lbl_PosFR.Text = "FR: " + ((double)carSim.wdFR.x * ScaleRealToPixel).ToString("F2") + "/ "
                                        + ((double)carSim.wdFR.y * ScaleRealToPixel).ToString("F2");

                lbl_PosRL.Text = "RL: " + ((double)carSim.wdRL.x * ScaleRealToPixel).ToString("F2") + "/ "
                                        + ((double)carSim.wdRL.y * ScaleRealToPixel).ToString("F2");

                lbl_PosRR.Text = "RR: " + ((double)carSim.wdRR.x * ScaleRealToPixel).ToString("F2") + "/ "
                                        + ((double)carSim.wdRR.y * ScaleRealToPixel).ToString("F2");
            }


            // 画面移動
            if (cb_TraceView.Checked == true)
            {
                SetView_CarCenter();
            }

            // 描画更新
            DrawUpdateSimArea();
            picbox_SimArea.Invalidate();

            // マップフォーム処理
            if (null != slamForm)
            {
                slamForm.tmr_Update_Tick(sender, e);
            }

            // bServer受信処理　メッセージ処理
            if (bSrv.IsConected())
            {
                lbl_bServerIPTitle.ForeColor = Color.LimeGreen;

                if (bSrv.readMessage())
                {
                    // 受信コントロール情報
                    // アクセル、ハンドル値 セット
                    carSim.carHandleAng = -bSrv.ctrHandle * 30.0;
                    /*
                     * ステアリングの遅れをシミュレーション
                    {
                        const double stearingSpeed = 2.0;
                        double tgtHandle = -bSrv.ctrHandle * 30.0;

                        if (carSim.carHandleAng < tgtHandle) carSim.carHandleAng += stearingSpeed;
                        else carSim.carHandleAng -= stearingSpeed;

                        if (Math.Abs(carSim.carHandleAng - tgtHandle) < stearingSpeed)
                        {
                            carSim.carHandleAng = tgtHandle;
                        }
                    }
                    */

                    carSim.carAccVal = bSrv.ctrAccel;
                }
            }
            else
            {
                lbl_bServerIPTitle.ForeColor = Color.Black;

                // 受信待ちでなければ、待ち状態に移行
                if (!bSrv.IsListening())
                {
                    bSrv.OpenAsync();
                }
            }

            {
                // 返却センサーデータ セット

                // ロータリーエンコーダ　パルス値
                if (bSrv.bResetRE)
                {
                    carSim.wheelPulseR = 0.0;
                    carSim.wheelPulseL = 0.0;
                    bSrv.senReR = 0;
                    bSrv.senReL = 0;
                    bSrv.bResetRE = false;
                }

                {
                    // ロータリーエンコーダ　Plot
                    // リセットされた時点ですべて0,0,0(X,Y,Dir)として、タイヤの回転値から移動量、向きを加算
                    // ↑ Y+
                    //  → X+
                    // 向きは時計周りに-
                    /*
                    bSrv.senRePlotX = (carSim.mkp.X - carInitPos.X);
                    bSrv.senRePlotY = (carSim.mkp.Y - carInitPos.Y);
                    bSrv.senReAng = -(carSim.mkp.Theta * Math.PI / 180.0);
                    */

                    // パルス値 前回保存
                    bSrv.senReR_ = bSrv.senReR;
                    bSrv.senReL_ = bSrv.senReL;

                    bSrv.senReR = (long)carSim.wheelPulseR;
                    bSrv.senReL = (long)carSim.wheelPulseL;

                    // パルス値の差分から移動座標を計算
                    double resAng = 0;
                    REncoderToMap.CalcWheelPlotXY(ref bSrv.plotWheelR, ref bSrv.plotWheelL, ref resAng,
                                                  bSrv.senReR, bSrv.senReL,
                                                  bSrv.senReR_, bSrv.senReL_);

                    // 現在位置を両輪の中心点で計算
                    bSrv.senRePlotX_Out = ((bSrv.plotWheelR.X + bSrv.plotWheelL.X) * 0.5);
                    bSrv.senRePlotY_Out = ((bSrv.plotWheelR.Y + bSrv.plotWheelL.Y) * 0.5);

                    // 座標系を実機にあわせる
                    bSrv.senRePlotY_Out = -bSrv.senRePlotY_Out;
                    bSrv.senReAng_Out = -resAng;
                }

                // 電子コンパス
                {
                    bSrv.senCompusDir = ((int)carSim.mkp.Theta) % 360;
                    if (bSrv.senCompusDir < 0) bSrv.senCompusDir += 360;
                }

                // GPS
                {
                    // GPSからMap変換時の(手で合わせた)スケール（計算上は必要ないはずなんだが・・）
                    //const double GPStoMapScale = 60.0;

                    // 1分の距離[mm] 1.85225Km
                    const double GPSScale = 1.85225 * 1000.0 * 1000.0;

                    /*
                    // GPS 緯度(Y)、経度(X)
                    double ido = (int)landY;
                    double kdo = (int)landX;

                    double mapY = (ido * 60.0 + (landY - ido)) * GPSScale;
                    double mapX = (kdo * 60.0 + (landX - kdo)) * GPSScale * Math.Cos(ido * Math.PI / 180.0);
                    */
                    double ido = carSim.mkp.Y / GPSScale;
                    double kdo = carSim.mkp.X / (GPSScale * Math.Cos(ido * Math.PI / 180.0));

                    bSrv.senGpsLandY = -ido;
                    bSrv.senGpsLandX = kdo;
                }
            }

            // 画面表示　ラベル
            {
                // R.Enc
                lbl_RePlotX.Text = bSrv.senRePlotX_Out.ToString("f3");
                lbl_RePlotY.Text = bSrv.senRePlotY_Out.ToString("f3");
                lbl_RePlotAng.Text = "Rad:" + bSrv.senReAng_Out.ToString("f3") + " / 度:" + (bSrv.senReAng_Out*180.0/Math.PI).ToString("f3");
                lbl_ReR.Text = bSrv.senReR.ToString();
                lbl_ReL.Text = bSrv.senReL.ToString();

                // 地磁気
                lblCompus.Text = bSrv.senCompusDir.ToString("f2");

                // GPS
                lbl_GPSGrandX.Text = bSrv.senGpsLandX.ToString("f8");
                lbl_GPSGrandY.Text = bSrv.senGpsLandY.ToString("f8");

                // コントロール
                lbl_LEDNo.Text = bSrv.ctrLedPattern.ToString();
                lbl_CtrlAcc.Text = bSrv.ctrAccel.ToString();
                lbl_CtrlHandle.Text = bSrv.ctrHandle.ToString();
            }


            // LRFセンサー情報取得
            for (int i = 0; i < carSim.mkp.LRFdata.Length; i++)
            {
                UrgSim.lrfData[i] = (short)carSim.mkp.LRFdata[i];
            }
            UrgSim.numLrfData = carSim.mkp.LRFdata.Length;

            // ROS-IF経由でのLRFデータ送信
            try
            {
                if (ipc != null && ipc.RemoteObject.urgData.Length  > 0)
                {
                    int nSkip = ipc.RemoteObject.urgData.Length / carSim.mkp.LRFdata.Length;
                    for (int i = 0; i < carSim.mkp.LRFdata.Length; i++)
                    {
                        for (int n = 0; n < nSkip; n++)
                        {
                            ipc.RemoteObject.urgData[i * nSkip + n] = carSim.mkp.LRFdata[i];
                        }
                    }

                    //lbl_Speed.Text = "LRF:" + ipc.RemoteObject.urgData[0].ToString();

                    // debug
                    ipc.RemoteObject.vslamPlotX = carSim.mkp.LRFdata[0]; //carSim.mkp.X;
                    ipc.RemoteObject.vslamPlotY = carSim.mkp.Y;
                }
            } catch ( Exception ex )
            {
                //
            }
            
            // URGコマンド受信処理
            if (UrgSim.readMessage())
            {
                lbl_URGIPTitle.ForeColor = Color.LimeGreen;
            }
            else
            {
                lbl_URGIPTitle.ForeColor = Color.Black;
            }

        }

        /// <summary>
        /// 描画更新
        /// </summary>
        public void DrawUpdateSimArea()
        {
            Graphics g = Graphics.FromImage(SimAreaBmp);
            double pixScale = ScaleRealToPixel * viewScale;

            g.ResetTransform();
            g.FillRectangle(Brushes.Black, 0, 0, SimAreaBmp.Width, SimAreaBmp.Height);
            //g.DrawImage(MapBmp, 0, 0, SimAreaBmp.Width, SimAreaBmp.Height);
            
            g.TranslateTransform((float)(-viewX * viewScale), (float)(-viewY * viewScale), MatrixOrder.Append);
            g.ScaleTransform((float)(viewScale), (float)(viewScale));

            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(MapBmp, 0, 0);

            g.ResetTransform();
            g.ScaleTransform((float)pixScale, (float)pixScale);

            // グリッド線 (0.5倍以下なら表示しない)
            if( pixScale > ScaleRealToPixel*0.5 )
            {
                for (int x = 0; x <= (int)((SimAreaBmp.Width / pixScale) / 1000.0); x++)
                {
                    int dx = (x * 1000) + (((int)((-viewX * ScalePixelToReal) + 0.5)) % 1000);
                    g.DrawLine(Pens.DarkGray, dx, 0, dx, (int)(SimAreaBmp.Height / pixScale));
                }
                for (int y = 0; y <= (int)((SimAreaBmp.Height / pixScale) / 1000.0); y++)
                {
                    int dy = (y * 1000) + (((int)((-viewY * ScalePixelToReal) + 0.5)) % 1000);
                    g.DrawLine(Pens.DarkGray, 0, dy, (int)(SimAreaBmp.Width / pixScale), dy);
                }
            }


            g.TranslateTransform((float)-viewX, (float)-viewY, MatrixOrder.Append);

            carSim.DrawCar(g, ScaleRealToPixel, viewScale, viewX, viewY);
            
            g.Dispose();
        }


        // キー入力----------------------------------------------------------------------
        private void tbar_Scroll(object sender, EventArgs e)
        {
            //carSim.carAccVal = (double)tbar_Speed.Value * 0.1;
        }

        int stX, stY;
        int msX, msY;

        private bool wldMoveFlg = false;
        private bool viewMoveFlg = false;
        //private bool wldRotFlg = false;

        private void picbox_SimArea_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                // オブジェクト移動
                wldMoveFlg = true;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // View移動
                viewMoveFlg = true;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                // BGMap移動
                //bgMoveFlg = true;
            }

            // 移動前の座標を記憶
            msX = e.X;
            msY = e.Y;

            if (viewMoveFlg)
            {
                stX = (int)viewX;
                stY = (int)viewY;
                //stAng = ViewScale;
            }

        }

        private void picbox_SimArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (wldMoveFlg)
            {
                /*
                // オブジェクト移動
                int difX = e.X - msX;
                int difY = e.Y - msY;
                if (null != EditLayer && difX != 0 && difY != 0)
                {
                    EditLayer.lcX = stX + difX;
                    EditLayer.lcY = stY + difY;

                    num_PositionX.Value = (int)EditLayer.lcX;
                    num_PositionY.Value = (int)EditLayer.lcY;
                    UpdateTRG = true;
                }*/
            }
            else if (viewMoveFlg)
            {
                /*
                if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    Console.WriteLine("Shiftキーが押されています。");
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                    Console.WriteLine("Ctrlキーが押されています。");
                if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt)
                    Console.WriteLine("Altキーが押されています。");
                */

                // View移動
                int difX = e.X - msX;
                int difY = e.Y - msY;

                if (difX != 0 && difY != 0)
                {
                    if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                    {
                        // Shift+でスケール
                        //ViewScale = (float)stAng + (float)difX * 0.1f;
                        //SetViewScale(ViewScale);
                    }
                    else
                    {
                        viewX = stX + -difX;
                        viewY = stY + -difY;
                    }
                    //UpdateTRG = true;
                }
            }

        }

        private void picbox_SimArea_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                wldMoveFlg = false;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                viewMoveFlg = false;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                //bgMoveFlg = false;
            }

            // Mapの時差更新 OFF
            //UpdateTRG = true;

        }

        // マウスコントローラ
        bool bMsControlON = false;
        private void picbox_MsController_MouseDown(object sender, MouseEventArgs e)
        {
            System.Drawing.Point mp = picbox_MsController.PointToScreen(new System.Drawing.Point(picbox_MsController.Width / 2, picbox_MsController.Height / 2));

            //マウスポインタの位置を設定する
            System.Windows.Forms.Cursor.Position = mp;
            //System.Drawing.Point cp = picbox_MsController.PointToClient(mp);

            msX = picbox_MsController.Width / 2;
            msY = picbox_MsController.Height / 2;
            bMsControlON = true;
        }

        private void picbox_MsController_MouseMove(object sender, MouseEventArgs e)
        {
            if (bMsControlON)
            {
                int difX = e.X - msX;
                int difY = e.Y - msY;

                //マウスポインタの位置を枠からでないようにする
                {
                    int newMsX = e.X;
                    int newMsY = e.Y;
                    bool bAjastMs = false;
                    if (newMsX < 0) { newMsX = 0; bAjastMs = true; }
                    if (newMsX > picbox_MsController.Width) { newMsX = picbox_MsController.Width; bAjastMs = true; }
                    if (newMsY < 0) { newMsY = 0; bAjastMs = true; }
                    if (newMsY > picbox_MsController.Height) { newMsY = picbox_MsController.Height; bAjastMs = true; }

                    if (bAjastMs)
                    {
                        System.Drawing.Point mp = picbox_MsController.PointToScreen(new System.Drawing.Point(newMsX, newMsY));
                        System.Windows.Forms.Cursor.Position = mp;
                    }
                }

                carSim.carHandleAng = ((double)difX / (picbox_MsController.Width / 2.0)) * 30.0;
                carSim.carAccVal = (double)-difY / (picbox_MsController.Height / 2.0) * dbgSpeed;
            }
        }

        private void picbox_MsController_MouseUp(object sender, MouseEventArgs e)
        {
            carSim.carAccVal = 0.0;
            carSim.carHandleAng = 0.0;
            bMsControlON = false;
        }

        private void cb_LRFForm_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_LRFForm.Checked)
            {
                slamForm = new LRFMapForm(carSim, 1200, 1300, ScaleRealToPixel);
                slamForm.Show();
            }
            else
            {
                if (null != slamForm)
                {
                    slamForm.Close();
                    slamForm = null;
                }
            }
        }

        /// <summary>
        /// Mapファイル変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LoadMapFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                tmr_Update.Enabled = false;
                LoadMapFile(dlg.FileName);
                tmr_Update.Enabled = true;
            }
        }

        /// <summary>
        /// クルマ状態初期化ボタン
        /// 位置など
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CarInit_Click(object sender, EventArgs e)
        {
            SimCarInit();
        }

        /// <summary>
        /// ビューサイズ変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tBarScale_Scroll(object sender, EventArgs e)
        {
            viewScale = 1.0 + tBarScale.Value*0.25;
            lbl_ScaleVal.Text = viewScale.ToString("F2");
        }


    }
}
