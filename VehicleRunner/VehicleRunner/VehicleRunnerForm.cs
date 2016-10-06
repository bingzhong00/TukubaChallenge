
// 動作フラグ
//#define EMULATOR_MODE  // LRF エミュレーション起動

#define LOGWRITE_MODE   // ログファイル出力

#define LOGIMAGE_MODE   // イメージログ出力
//#define GPSLOG_OUTPUT   // GPSログ出力
//#define LRFLOG_OUTPUT   // LRFログ出力

//#define UnUseLRF          // LRFを使わない


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

using LocationPresumption;
using CersioIO;
using Navigation;

namespace VehicleRunner
{
    /// <summary>
    /// VehicleRunnerフォーム
    /// </summary>
    public partial class VehicleRunnerForm : Form
    {
        /// <summary>
        /// LRF領域表示ウィンドウモード
        /// </summary>
        public enum LRF_PICBOX_MODE {
            Normal = 0,
            EbsArea,
            end,
            Indicator,
            //end
        };

        /// <summary>
        /// セルシオ管理 クラス
        /// </summary>
        CersioCtrl CersioCt;

        /// <summary>
        /// 動作決定管理クラス
        /// </summary>
        Brain BrainCtrl;

        /// <summary>
        /// 起動時のマップファイル
        /// </summary>
        private const string defaultMapFile = "../../../MapFile/utsubo201608/utsubo201608.xml";

        /// <summary>
        /// USB GPSクラス
        /// </summary>
        UsbIOport usbGPS;

        Random Rand = new Random();

        double LRFViewScale = 1.0;

        private int selPicboxLRFmode = 1;

        /// <summary>
        /// Form描画処理クラス
        /// </summary>
        private VehicleRunnerForm_Draw formDraw = new VehicleRunnerForm_Draw();

        /// <summary>
        /// ログ処理クラス
        /// </summary>
        private VehicleRunnerForm_Log formLog = new VehicleRunnerForm_Log();


        /// <summary>
        /// 位置補正指示トリガー
        /// </summary>
        private bool bLocRivisionTRG = false;

        /// <summary>
        /// 自律走行フラグ
        /// </summary>
        public bool bRunAutonomous = false;

        /// <summary>
        /// マッピング、ロギングモード フラグ
        /// </summary>
        public bool bRunMappingAndLogging = false;

        // Form内のクラスを更新
        static int updateMainCnt = 0;

        // ハードウェア用 周期の短いカウンタ
        private int updateHwCnt = 0;

        /// <summary>
        /// アプリ終了フラグ
        /// </summary>
        private bool appExit = false;

        // ================================================================================================================

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VehicleRunnerForm()
        {
            InitializeComponent();

            formLog.init();

            // セルシオコントローラ初期化
            CersioCt = new CersioCtrl();

            // BoxPc(bServer)接続
            //CersioCt.ConnectBoxPC();

            // ブレイン起動
            BrainCtrl = new Brain(CersioCt, defaultMapFile);

            // 初期ボタン設定セット
            rb_Move_CheckedChanged(null, null);
            rb_Dir_CheckedChanged(null, null);

            tbar_AccRatio_Scroll(null, null);
            tbar_HdlRatio_Scroll(null, null);

            cb_VRRevision_CheckedChanged(null, null);

            // マップウィンドウサイズのbmp作成
            formDraw.MakePictureBoxWorldMap(BrainCtrl.LocSys.worldMap.mapBmp, picbox_AreaMap);

            // LRF 入力スケール調整反映
            tb_LRFScale.Text = trackBar_LRFViewScale.Value.ToString();
            //btm_LRFScale_Click(null, null);
            tb_LRFScale_TextChanged(null, null);


            // bServerエミュレーション表記
            lbl_bServerEmu.Visible = CersioCt.bServerEmu;


            // センサー値取得 スレッド起動
            Thread trdSensor = new Thread(new ThreadStart(ThreadSensorUpdate));
            trdSensor.IsBackground = true;
            trdSensor.Priority = ThreadPriority.AboveNormal;
            trdSensor.Start();

            // 位置座標更新　スレッド起動
            Thread trdLocalize = new Thread(new ThreadStart(ThreadLocalizationUpdate));
            trdLocalize.IsBackground = true;
            //trdSensor.Priority = ThreadPriority.AboveNormal;
            trdLocalize.Start();

#if EMULATOR_MODE
            // LRF エミュレーション
            tb_LRFIpAddr.Text = "127.0.0.10";
#endif

        }

        /// <summary>
        /// フォーム初期化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VehicleRunnerForm_Load(object sender, EventArgs e)
        {
            // 表示位置指定
            this.SetDesktopLocation(0, 0);

            // USB Connect Select
            try
            {
                // すべてのシリアル・ポート名を取得する
                string[] portsList = UsbIOport.GetDeviceList();

                // シリアルポートを毎回取得して表示するために表示の度にリストをクリアする
                cb_UsbSirial.Items.Clear();
                cmbbox_UsbSH2Connect.Items.Clear();

                int i = 0;
                foreach (string port in portsList)
                {
                    // 取得したシリアル・ポート名を出力する
                    cb_UsbSirial.Items.Add(port);
                    cmbbox_UsbSH2Connect.Items.Add(port);
                    i++;
                }

                if (cb_UsbSirial.Items.Count > 0)
                {
                    cb_UsbSirial.SelectedIndex = 0;
                    cmbbox_UsbSH2Connect.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("USB Port List Init Error! : " + ex.Message );
            }

            // フォームのパラメータ反映
            // 自己位置 更新方法
            //LocPreSumpSystem.bMoveUpdateGPS = cb_UseGPS_Move.Checked;

            cb_AlwaysPFCalc.Enabled = rb_UsePF_Revision.Checked;

            tb_MapName.Text = BrainCtrl.MapFile.MapName;

            // 画面更新
            PictureUpdate();

            // ハードウェア更新タイマ起動
            tm_UpdateHw.Enabled = true;
            // 位置管理定期処理タイマー起動
            tm_LocUpdate.Enabled = true;

            //tm_SendCom.Enabled = true;
        }

        /// <summary>
        /// フォームクローズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VehicleRunnerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            BrainCtrl.LocSys.LRF.Close();
            CersioCt.Disconnect();
        }

        /// <summary>
        /// フォームクローズ 完了前処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VehicleRunnerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // アプリ終了フラグ
            appExit = true;

            // タイマー停止
            //tm_SendCom.Enabled = false;
            tm_UpdateHw.Enabled = false;
            tm_LocUpdate.Enabled = false;

#if LOGIMAGE_MODE
            // マップログ出力
            formLog.Output_ImageLog(ref BrainCtrl);
#endif
        }


        // Draw -------------------------------------------------------------------------------------------
        int selAreaMapMode = 0;

        /// <summary>
        /// エリアマップ描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picbox_AreaMap_Paint(object sender, PaintEventArgs e)
        {
            // 書き換えＢＭＰ（追加障害物）描画
            if (selAreaMapMode == 0) formDraw.AreaMap_Draw_Area( e.Graphics, ref CersioCt, ref BrainCtrl);
            else if (selAreaMapMode == 1) formDraw.AreaMap_Draw_WorldMap(e.Graphics, ref CersioCt, ref BrainCtrl);

            formDraw.AreaMap_Draw_Text(e.Graphics, ref BrainCtrl, (long)updateHwCnt);
        }

        /// <summary>
        /// 表示切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picbox_AreaMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                // エリア、ワールドマップ切り替え
                selAreaMapMode = (++selAreaMapMode) % 2;
            }
        }


        // ---------------------------------------------------------------------------------------------------
#region "LRF,Indicatorエリア 描画"

        /// <summary>
        /// LRFウィンドウデータ描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picbox_LRF_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LocPreSumpSystem LocSys = BrainCtrl.LocSys;

            // LRF取得データを描画
            {
                int ctrX = picbox_LRF.Width / 2;
                int ctrY = picbox_LRF.Height * 6 / 8;

                float scale = 1.0f;

                // 背景色
                switch (selPicboxLRFmode)
                {
                    case (int)LRF_PICBOX_MODE.Normal:
                        ctrX = picbox_LRF.Width / 2;
                        ctrY = picbox_LRF.Height * 6 / 10;

                        picbox_LRF.BackColor = Color.Gray;//Color.White;
                        scale = 1.0f;
                        break;
                    case (int)LRF_PICBOX_MODE.EbsArea:
                        ctrX = picbox_LRF.Width / 2;
                        ctrY = picbox_LRF.Height * 6 / 8;

                        picbox_LRF.BackColor = Color.Black;

                        scale = 4.0f;

                        // EBSに反応があればズーム
                        //scale += ((float)CersioCt.BrainCtrl.EBS_CautionLv * 3.0f / (float)Brain.EBS_CautionLvMax);

                        // EHS
                        //if (CersioCt.BrainCtrl.EHS_Result != Brain.EHS_MODE.None) scale = 10.0f;
                        break;
                    case (int)LRF_PICBOX_MODE.Indicator:
                        picbox_LRF.BackColor = Color.Black;
                        break;
                }


                if (selPicboxLRFmode == (int)LRF_PICBOX_MODE.Normal ||
                    selPicboxLRFmode == (int)LRF_PICBOX_MODE.EbsArea)
                {
                    // ガイドライン描画
                    formDraw.LRF_Draw_GuideLine(g, ref LocSys, ctrX, ctrY, scale);
                }

                switch (selPicboxLRFmode)
                {
                    case (int)LRF_PICBOX_MODE.Normal:
                        // LRF描画
                        if (LocSys.LRF.getData() != null)
                        {
                            formDraw.LRF_Draw_Point(g, Brushes.Yellow, LocSys.LRF.getData(), ctrX, ctrY, (LRFViewScale / 1000.0f)*scale);
                        }

                        {
                            int iH = 80;

                            //g.FillRectangle(Brushes.Black, 0, picbox_LRF.Height - iH, picbox_LRF.Width, iH);
                            formDraw.DrawIngicator(g, picbox_LRF.Height - iH, ref CersioCt, ref BrainCtrl);
                        }
                        break;

                    case (int)LRF_PICBOX_MODE.EbsArea:
                        // EBS範囲描画
                        formDraw.LRF_Draw_PointEBS(g, ref LocSys, ref BrainCtrl, LocSys.LRF.getData_UntiNoise(), ctrX, ctrY, scale, (LRFViewScale / 1000.0f) * scale);
                        break;
                    case (int)LRF_PICBOX_MODE.Indicator:
                        picbox_LRF.BackColor = Color.Black;
                        formDraw.DrawIngicator(g, picbox_LRF.Height / 2 - 50, ref CersioCt, ref BrainCtrl);
                        break;
                }
            }
        }

        /// <summary>
        /// インジケーターウィンドウ描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picbox_Indicator_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // LRF取得データを描画
            int ctrX = picbox_Indicator.Width / 2;
            int ctrY = picbox_Indicator.Height * 6 / 8;

            picbox_Indicator.BackColor = Color.Black;
            formDraw.DrawIngicator(g, 0, ref CersioCt, ref BrainCtrl);
        }

#endregion

        /// <summary>
        /// 表示切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picbox_LRF_Click(object sender, EventArgs e)
        {
            // モード切り替え
            selPicboxLRFmode = (++selPicboxLRFmode) % (int)LRF_PICBOX_MODE.end;
            picbox_LRF.Invalidate();
        }

        /// <summary>
        /// LRF接続ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_LRFConnect_CheckedChanged(object sender, EventArgs e)
        {
            LocPreSumpSystem LocSys = BrainCtrl.LocSys;

            if (cb_LRFConnect.Checked)
            {
                // LRF接続

                // 元のカーソルを保持
                Cursor preCursor = Cursor.Current;
                // カーソルを待機カーソルに変更
                Cursor.Current = Cursors.WaitCursor;

                {
                    int intLrfPot;
                    if (int.TryParse(tb_LRFPort.Text, out intLrfPot))
                    {
                        // 指定のIP,Portでオープン
                        LocSys.LRF.Open(tb_LRFIpAddr.Text, intLrfPot);

                        if (LocSys.LRF.IsConnect())
                        {
                            // 接続OK
                            tb_LRFIpAddr.BackColor = Color.Lime;
                            tb_LRFPort.BackColor = Color.Lime;
                        }
                    }
                }

                // カーソルを元に戻す
                Cursor.Current = preCursor;
            }
            else
            {
                // LRF切断
                LocSys.LRF.Close();

                tb_LRFIpAddr.BackColor = SystemColors.Window;
                tb_LRFPort.BackColor = SystemColors.Window;
            }
        }

        /// <summary>
        /// Form内のピクチャー更新
        /// </summary>
        private void PictureUpdate()
        {
            // 自己位置マーカー入りマップBmp描画
            BrainCtrl.LocSys.UpdateLocalizeBitmap(cb_AlwaysPFCalc.Checked, true);

            // 各PictureBox更新
            this.picbox_AreaMap.Refresh();
            this.picbox_LRF.Refresh();
            this.picbox_Indicator.Refresh();
        }

        /// <summary>
        /// VRunner リセット
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PositionReset_Click(object sender, EventArgs e)
        {
            BrainCtrl.Reset_StartPosition();
            PictureUpdate();
        }

        //--------------------------------------------------------------------------------------------------------------------
        // タイマイベント処理

        /// <summary>
        /// センサー値更新 スレッド
        /// </summary>
        private void ThreadSensorUpdate()
        {
            while (!appExit)
            {
#if !UnUseLRF
                if (null != BrainCtrl.LocSys && null != BrainCtrl.LocSys.LRF)
                {
                    BrainCtrl.LocSys.LRF.Update();
                }
#endif
                // bServer ハードウェア(センサー)情報取得
                if (null != CersioCt)
                {
                    CersioCt.GetHWStatus(((usbGPS != null) ? true : false));
                }

                Thread.Sleep(20);
            }
        }

        /// <summary>
        /// ハードウェア系の更新
        /// (間隔短め 50MS)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tm_UpdateHw_Tick(object sender, EventArgs e)
        {
            LocPreSumpSystem LocSys = BrainCtrl.LocSys;

#if !UnUseLRF
            // LRF更新
            if (LocSys.LRF.IsConnect())
            {
                // LRFから取得
                if (LocSys.LRF.isGetDatas) lb_LRFResult.Text = "OK";    // 接続して、データも取得
                else lb_LRFResult.Text = "OK(noData)";      // 接続しているが、データ取得ならず
            }
            else
            {
                // 仮想マップから取得
                lb_LRFResult.Text = "Disconnect";
            }
#endif

            // 実走行時、bServerと接続が切れたら再接続
            if (updateHwCnt % 50 == 0)
            {
                // 状態を見て、自動接続
                if (!CersioCt.TCP_IsConnected())
                {
                    CersioCt.ConnectBoxPC();
                }
            }

            // ロータリーエンコーダ(Plot座標)情報
            if (CersioCt.bhwREPlot)
            {
                // 自己位置に REPlot情報を渡す
                LocSys.Input_RotaryEncoder(CersioCt.hwREX, CersioCt.hwREY, CersioCt.hwREDir);

                // 受信情報を画面に表示
                lbl_REPlotX.Text = CersioCt.hwREX.ToString("f1");
                lbl_REPlotY.Text = CersioCt.hwREY.ToString("f1");
                lbl_REPlotDir.Text = CersioCt.hwREDir.ToString("f1");
            }

            // ロータリーエンコーダ（タイヤ回転数）情報
            if (CersioCt.bhwRE)
            {
                if (CersioCt.bhwCompass)
                {
                    // 自己位置に情報を渡す
                    LocSys.Input_RotaryEncoder_Pulse(CersioCt.hwRErotL, CersioCt.hwRErotR, CersioCt.hwCompass);
                }

                lbl_RErotR.Text = CersioCt.hwRErotR.ToString("f1");
                lbl_RErotL.Text = CersioCt.hwRErotL.ToString("f1");
            }

            // 地磁気情報
            if (CersioCt.bhwCompass)
            {
                // 自己位置推定に渡す
                LocSys.Input_Compass(CersioCt.hwCompass);

                // 画面表示
                lbl_Compass.Text = CersioCt.hwCompass.ToString();
            }

            // GPS情報
            if (CersioCt.bhwGPS)
            {
                // 途中からでもGPSのデータを拾う
                if (!LocPreSumpSystem.bEnableGPS)
                {
                    // 応対座標算出のために、取得開始時点のマップ座標とGPS座標をリンク
                    LocPreSumpSystem.Set_GPSStart(CersioCt.hwGPS_LandX,
                                                  CersioCt.hwGPS_LandY,
                                                  (int)(LocSys.R1.X+0.5),
                                                  (int)(LocSys.R1.Y+0.5) );
                }

                // 自己位置推定に渡す
                if (CersioCt.bhwUsbGPS)
                {
                    // 角度情報あり
                    LocSys.Input_GPSData(CersioCt.hwGPS_LandX, CersioCt.hwGPS_LandY, CersioCt.hwGPS_MoveDir);
                }
                else
                {
                    // 角度情報なし
                    LocSys.Input_GPSData(CersioCt.hwGPS_LandX, CersioCt.hwGPS_LandY);
                }

                // 画面表示
                lbl_GPS_Y.Text = CersioCt.hwGPS_LandY.ToString("f5");
                lbl_GPS_X.Text = CersioCt.hwGPS_LandX.ToString("f5");
            }

            // ROS LRF取得
            if (rb_LRF_ROSnode.Checked)
            {
                // LRFのデータを外部から入力
                LocSys.LRF.SetExtData( CersioCt.GetROS_LRFdata());
            }

            // LED状態 画面表示
            if (CersioCt.ptnHeadLED == -1)
            {
                lbl_LED.Text = "ND";
            }
            else
            {
                string ledStr = CersioCt.ptnHeadLED.ToString();

                if (CersioCt.ptnHeadLED >= 0 && CersioCt.ptnHeadLED < CersioCtrl.LEDMessage.Count())
                {
                    ledStr += "," + CersioCtrl.LEDMessage[CersioCt.ptnHeadLED];
                }

                if (!ledStr.Equals(lbl_LED.Text))
                {
                    lbl_LED.Text = ledStr;
                }
            }

            // BoxPC接続状態確認
            if (CersioCt.TCP_IsConnected())
            {
                tb_SendData.BackColor = Color.Lime;
                tb_ResiveData.BackColor = Color.Lime;
                lb_BServerConnect.Text = "BServer 接続OK";
                lb_BServerConnect.BackColor = Color.Lime;
            }
            else
            {
                tb_SendData.BackColor = SystemColors.Window;
                tb_ResiveData.BackColor = SystemColors.Window;
                lb_BServerConnect.Text = "BServer 未接続";
                lb_BServerConnect.BackColor = SystemColors.Window;
            }


            // 送受信文字 画面表示
            if (null != CersioCt.hwResiveStr)
            {
                tb_ResiveData.Text = CersioCt.hwResiveStr.Replace('\n', ' ');
            }
            if (null != CersioCt.hwSendStr)
            {
                tb_SendData.Text = CersioCt.hwSendStr.Replace('\n', ' ');
            }

            // USB GPSからの取得情報画面表示
            if (null != usbGPS)
            {
                tb_SirialResive.Text = usbGPS.resiveStr;

                if (!string.IsNullOrEmpty(usbGPS.resiveStr))
                {
                    CersioCt.usbGPSResive.Add(usbGPS.resiveStr);
                }
                usbGPS.resiveStr = "";

                if (CersioCt.usbGPSResive.Count > 30)
                {
                    CersioCt.usbGPSResive.RemoveRange(0, CersioCt.usbGPSResive.Count - 30);
                }
            }


            updateHwCnt++;
        }

        /// <summary>
        /// 位置座標更新 スレッド
        /// </summary>
        private void ThreadLocalizationUpdate()
        {
            while (!appExit)
            {
                
                if (null != BrainCtrl && null != BrainCtrl.LocSys)
                {
                    // 現在位置更新
                    BrainCtrl.LocSys.update_NowLocation();

                    // パーティクルフィルター自己位置推定 演算
                    if (cb_VRRevision.Checked)
                    {
                        // 常時PF演算
                        if (cb_AlwaysPFCalc.Checked)
                        {
                            BrainCtrl.LocSys.ParticleFilter_Update();
                        }

                        // 自己位置補正執行判断
                        {
                            bool bRevisionIssue = false;

                            if((bLocRivisionTRG || BrainCtrl.bRevisionRequest))
                            {
                                bRevisionIssue = true;
                                bLocRivisionTRG = false;
                            }

                            // 一定時間で更新
                            /*
                            if ((updateMainCnt % 30) == 0 && !cb_DontAlwaysRivision.Checked)
                            {
                                Brain.addLogMsg += "LocRivision:Timer(時間ごとの位置補正)\n";
                                bRevisionIssue = true;
                            }
                            */

                            if (bRevisionIssue)
                            {
                                // 自己位置補正
                                string logMsg;
                                LocPreSumpSystem LocSys = BrainCtrl.LocSys;

                                logMsg = LocSys.LocalizeRevision(rb_UseGPS_Revision.Checked);

                                // REリセット
                                CersioCt.SendCommand_RE_Reset();

                                CersioCt.setREPlot_Start( LocSys.E1.X * LocSys.MapToRealScale,
                                                          LocSys.E1.Y * LocSys.MapToRealScale,
                                                          LocSys.E1.Theta );

                                Brain.addLogMsg += logMsg;
                            }
                        }
                    }

                    // MAP座標更新処理
                    //LocSys.MapArea_Update();
                }

                // bServer ハードウェア(センサー)情報取得
                if (null != BrainCtrl)
                {
                    // 自律走行処理 更新
                    if (bRunAutonomous)
                    {
                        // セルシオ コントロール
                        // 自己位置更新処理とセルシオ管理
                        BrainCtrl.AutonomousProc( cb_EmgBrake.Checked,
                                                  cb_EHS.Checked,
                                                  cb_StraghtMode.Checked,
                                                  cb_InDoorMode.Checked );
                    }
                }
                
                updateMainCnt++;
                
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 自己位置推定計算用　更新
        /// (間隔長め)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tm_LocUpdate_Tick(object sender, EventArgs e)
        {
            /*
            // 現在位置更新
            LocSys.update_NowLocation();

            // パーティクルフィルター自己位置推定 演算
            if (cb_VRRevision.Checked)
            {
                // 常時PF演算
                if (cb_AlwaysPFCalc.Checked)
                {
                    LocSys.ParticleFilter_Update();
                }

                // 自己位置補正執行判断
                {
                    bool bRevisionIssue = false;

                    if((bLocRivisionTRG || BrainCtrl.bRevisionRequest))
                    {
                        bRevisionIssue = true;
                        bLocRivisionTRG = false;
                    }

                    // 一定時間で更新
                    if ((updateMainCnt % 30) == 0 && !cb_DontAlwaysRivision.Checked)
                    {
                        Brain.addLogMsg += "LocRivision:Timer(時間ごとの位置補正)\n";
                        bRevisionIssue = true;
                    }

                    if (bRevisionIssue)
                    {
                        // 自己位置補正
                        string logMsg;

                        logMsg = LocSys.LocalizeRevision(rb_UseGPS_Revision.Checked);

                        // REリセット
                        CersioCt.SendCommand_RE_Reset( LocSys.E1.X * LocSys.RealToMapSclae,
                                                       LocSys.E1.Y * LocSys.RealToMapSclae,
                                                       LocSys.E1.Theta);

                        Brain.addLogMsg += logMsg;
                    }
                }
            }
            */

            // MAP座標更新処理
            // ※描画と演算のタイミングの整合をとる
            BrainCtrl.LocSys.MapArea_Update();
            

            // REからのスピード表示
            tb_RESpeed.Text = CersioCtrl.SpeedMH.ToString("f1");

            // 自律走行処理 更新
            if (bRunAutonomous)
            {
                /*
                // セルシオ コントロール
                // 自己位置更新処理とセルシオ管理
                BrainCtrl.AutonomousProc( LocSys,
                                          cb_EmgBrake.Checked, cb_EHS.Checked,
                                          cb_StraghtMode.Checked );
                                          */
                // 動作内容TextBox表示

                // ハンドル、アクセル値　表示
                tb_AccelVal.Text = CersioCtrl.nowSendAccValue.ToString("f2");
                tb_HandleVal.Text = CersioCtrl.nowSendHandleValue.ToString("f2");

                // エマージェンシーブレーキ 動作カラー表示
                {
                    if (BrainCtrl.EBS.EmgBrk && cb_EmgBrake.Checked) cb_EmgBrake.BackColor = Color.Red;
                    else cb_EmgBrake.BackColor = SystemColors.Control;

                    if (BrainCtrl.EHS.Result != Brain.EmergencyHandring.EHS_MODE.None && cb_EHS.Checked)
                    {
                        cb_EHS.BackColor = Color.Orange;
                    }
                    else cb_EHS.BackColor = SystemColors.Control;

                    // UntiEBS Cnt
                    lbl_BackCnt.Text = "EBS cnt:" + BrainCtrl.EmgBrakeContinueCnt.ToString();
                    lbl_BackProcess.Visible = BrainCtrl.bNowBackProcess;
                }
            }
            //updateMainCnt++;


#if LOGWRITE_MODE
            // 自律走行、またはロギング中
            if (bRunAutonomous || bRunMappingAndLogging)
            {

#if LRFLOG_OUTPUT
                // LRFログ出力 
                // データ量が多いので、周期の長い定期処理で実行
                if (LocSys.LRF.IsConnect() && null != LocSys.LRF.getData() )
                {
                    formLog.Output_LRFLog(LocSys.LRF.getData());
                }
#endif  // LRFLOG_OUTPUT

                // ログファイル出力
                formLog.Output_VRLog(ref BrainCtrl, ref CersioCt);

#if GPSLOG_OUTPUT
                if (null != usbGPS)
                {
                    // ログ出力
                    formLog.Output_GPSLog(usbGPS.resiveStr);
                }
#endif  // GPSLOG_OUTPUT

            }
#endif  // LOGWRITE_MODE

            // ログバッファクリア
            formLog.LogBuffer_Clear(ref BrainCtrl, ref CersioCt);


            // 画面描画
            PictureUpdate();
        }

        /// <summary>
        /// LRF Scale変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_LRFScale_TextChanged(object sender, EventArgs e)
        {
            double tval;
            double.TryParse(tb_LRFScale.Text, out tval);

            if (tval != 0.0)
            {
                LRFViewScale = tval;
            }
        }

        /// <summary>
        /// 位置補正 開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LocRevision_Click(object sender, EventArgs e)
        {
            // 強制位置補正
            bLocRivisionTRG = true;

            Brain.addLogMsg += "LocRivision:Button(ボタン押下による位置補正)\n";
        }

        /// <summary>
        /// usbGPS取得ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_SirialConnect_CheckedChanged(object sender, EventArgs e)
        {
            // 接続中なら一度切断
            if (null != usbGPS)
            {
                usbGPS.Close();
                usbGPS = null;
            }

            if (cb_SirialConnect.Checked)
            {
                // USB GPS接続
                usbGPS = new UsbIOport();
                string[] portSlipt = cb_UsbSirial.Text.Split(':');

                if (usbGPS.Open(portSlipt[0], 4800))
                {
                    // 接続成功
                    tb_SirialResive.BackColor = Color.Lime;
                }
                else
                {
                    // 接続失敗
                    tb_SirialResive.BackColor = SystemColors.Control;
                    tb_SirialResive.Text = "ConnectFail";
                    usbGPS = null;
                }
            }
            else
            {
                tb_SirialResive.BackColor = SystemColors.Control;
            }
        }

        /// <summary>
        /// モータードライバー SH2 USB接続
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_UsbSH2Connect_CheckedChanged(object sender, EventArgs e)
        {
            if (CersioCt != null) return;

            // 接続中なら切断
            if (null != CersioCt.UsbMotorDriveIO)
            {
                CersioCt.UsbMotorDriveIO.Close();
                CersioCt.UsbMotorDriveIO = null;
            }

            if (cb_UsbSH2Connect.Checked)
            {
                // 接続
                CersioCt.UsbMotorDriveIO = new DriveIOport();
                string[] portSlipt = cmbbox_UsbSH2Connect.Text.Split(':');

                if (CersioCt.UsbMotorDriveIO.Open(portSlipt[0], 57600))
                {
                    // 接続成功
                    cmbbox_UsbSH2Connect.BackColor = Color.Lime;
                }
                else
                {
                    // 接続失敗
                    cmbbox_UsbSH2Connect.BackColor = SystemColors.Control;
                    CersioCt.UsbMotorDriveIO = null;
                }
            }
            else
            {
                cmbbox_UsbSH2Connect.BackColor = SystemColors.Control;
            }

        }

        /// <summary>
        /// 移動量入力元変更
        /// ラジオボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rb_Move_CheckedChanged(object sender, EventArgs e)
        {
            LocPreSumpSystem LocSys = BrainCtrl.LocSys;

            // 移動入力元　センサー切り替え
            LocSys.Setting.bMoveSrcRePlot = rb_MoveREPlot.Checked;
            LocSys.Setting.bMoveSrcGPS  = rb_MoveGPS.Checked;
            LocSys.Setting.bMoveSrcSVO  = rb_MoveSVO.Checked;
            LocSys.Setting.bMoveSrcReCompus = rb_MoveREandCompus.Checked;
            LocSys.Setting.bMoveSrcPF = rb_MovePF.Checked;
        }

        /// <summary>
        /// 向き入力元変更
        /// ラジオボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rb_Dir_CheckedChanged(object sender, EventArgs e)
        {
            LocPreSumpSystem LocSys = BrainCtrl.LocSys;

            // 向き入力元　センサー切り替え
            LocSys.Setting.bDirSrcRePlot = rb_DirREPlot.Checked;
            LocSys.Setting.bDirSrcGPS = rb_DirGPS.Checked;
            LocSys.Setting.bDirSrcSVO  = rb_DirSVO.Checked;
            LocSys.Setting.bDirSrcCompus = rb_DirCompus.Checked;
        }

        /// <summary>
        /// スケールバー変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar_LRFViewScale_Scroll(object sender, EventArgs e)
        {
            tb_LRFScale.Text = trackBar_LRFViewScale.Value.ToString();
            tb_LRFScale_TextChanged(sender, null);
        }


        /// <summary>
        /// 自律走行モード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_Autonomous_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_StartAutonomous.Checked)
            {
                LocPreSumpSystem LocSys = BrainCtrl.LocSys;

                // 現在座標から開始

                // 開始時のリセット
                LocSys.SetStartPostion( (int)(LocSys.R1.X+0.5),
                                        (int)(LocSys.R1.Y+0.5),
                                        LocSys.R1.Theta);

                // GPS情報があれば GPSの初期値をセット
                if (CersioCt.bhwGPS)
                {
                    // 起点情報をセット
                    LocPreSumpSystem.Set_GPSStart(CersioCt.hwGPS_LandX,
                                                  CersioCt.hwGPS_LandY,
                                                  (int)(LocSys.R1.X + 0.5),
                                                  (int)(LocSys.R1.Y + 0.5) );
                }

                bRunAutonomous = true;
                cb_StartAutonomous.BackColor = Color.LimeGreen;
            }
            else
            {
                // 停止
                CersioCt.SendCommand_Stop();

                bRunAutonomous = false;
                cb_StartAutonomous.BackColor = SystemColors.Window;
            }
        }

        /// <summary>
        /// ログ・マッピングモード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_StartLogMapping_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_StartLogMapping.Checked)
            {
                formLog.init();
                bRunMappingAndLogging = true;
                cb_StartLogMapping.BackColor = Color.Red;
            }
            else
            {
                bRunMappingAndLogging = false;
                cb_StartLogMapping.BackColor = SystemColors.Window;
            }
        }

        /// <summary>
        /// タブ切り替え時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocPreSumpSystem LocSys = BrainCtrl.LocSys;

            num_R1X.Value = (int)LocSys.R1.X;
            num_R1Y.Value = (int)LocSys.R1.Y;
            num_R1Dir.Value = (int)LocSys.R1.Theta;
        }

        /// <summary>
        /// GPSから現在位置設定に代入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_GetGPStoR1_Click(object sender, EventArgs e)
        {
            LocPreSumpSystem LocSys = BrainCtrl.LocSys;

            num_R1X.Value = (int)LocSys.G1.X;
            num_R1Y.Value = (int)LocSys.G1.Y;
        }

        /// <summary>
        /// 地磁気から現在位置設定に代入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_GetCompustoR1_Click(object sender, EventArgs e)
        {
            LocPreSumpSystem LocSys = BrainCtrl.LocSys;

            num_R1Dir.Value = (int)LocSys.C1.Theta;
        }

        /// <summary>
        /// 現在位置設定をR1にセット
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ResetR1_Click(object sender, EventArgs e)
        {
            LocPreSumpSystem LocSys = BrainCtrl.LocSys;

            LocSys.R1.X = (double)num_R1X.Value;
            LocSys.R1.Y = (double)num_R1Y.Value;
            LocSys.R1.Theta = (double)num_R1Dir.Value;
        }

        /// <summary>
        /// LRF接続先選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rb_LRF_LAN_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == rb_LRF_ROSnode)
            {
                // ROS node
                lb_LRFResult.Enabled = false;
                tb_LRFIpAddr.Enabled = false;
                tb_LRFPort.Enabled = false;
                cb_LRFConnect.Enabled = false;

                // URG切断
                if (cb_LRFConnect.Checked)
                {
                    cb_LRFConnect.Checked = false;
                    cb_LRFConnect_CheckedChanged(sender, e);
                }
            }
            else if (sender == rb_LRF_LAN)
            {
                // LAN接続
                lb_LRFResult.Enabled = true;
                tb_LRFIpAddr.Enabled = true;
                tb_LRFPort.Enabled = true;
                cb_LRFConnect.Enabled = true;
            }
        }

        /// <summary>
        /// アクセル量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbar_AccRatio_Scroll(object sender, EventArgs e)
        {
            CersioCtrl.AccRate = tbar_AccRatio.Value * 0.1;
            lbl_AccRatio.Text = "アクセル上限:" + CersioCtrl.AccRate.ToString("f1");
        }

        private void tbar_HdlRatio_Scroll(object sender, EventArgs e)
        {
            CersioCtrl.HandleRate = tbar_HdlRatio.Value * 0.1;
            lbl_HdlRatio.Text = "ハンドル上限:" + CersioCtrl.HandleRate.ToString("f1");
        }

        /// <summary>
        /// 自己位置補正切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_VRRevision_CheckedChanged(object sender, EventArgs e)
        {
            gbox_Revision.Enabled = cb_VRRevision.Checked;
        }

        /// <summary>
        /// キャリブレーション
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCribration_Click(object sender, EventArgs e)
        {
            int reL, reR;

            int.TryParse(tbCaribrationREL.Text, out reL);
            int.TryParse(tbCaribrationRER.Text, out reR);

            // ※比率計算

            // コマンド送信
            CersioCt.SendCommand_RE_OneRotatePulse_Reset(reL, reR);
        }

        private void rb_UsePF_Revision_CheckedChanged(object sender, EventArgs e)
        {
            cb_AlwaysPFCalc.Enabled = rb_UsePF_Revision.Checked;
        }

        /// <summary>
        /// チェックボックス　ボタン色変え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_Color_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox item = (CheckBox)sender;
            if (item.Checked)
            {
                item.BackColor = Color.LimeGreen;
            }
            else
            {
                item.BackColor = SystemColors.Control;
            }
        }

        /// <summary>
        /// bServerエミュレータ切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_ConnectBServerEmu_CheckedChanged(object sender, EventArgs e)
        {
            if( cb_ConnectBServerEmu.Checked )
            {
                // エミュレータ接続
                CersioCt.RunBoxPC_Emulator();
            }
            else
            {
                // BoxPC接続
                CersioCt.ConnectBoxPC();
            }
        }

        /// <summary>
        /// ROS-IF接続
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_ConnectRosIF_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_ConnectRosIF.Checked)
            {
                CersioCt.Run_RosIF();
            }
        }

        /// <summary>
        /// Map選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_MapLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if(dlg.ShowDialog() ==  DialogResult.OK  )
            {
                // ロード
                BrainCtrl.Reset(dlg.FileName);
            }
        }
    }
}
