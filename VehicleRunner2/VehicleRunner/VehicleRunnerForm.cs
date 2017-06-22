
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
using System.Diagnostics;

using Location;
using CersioIO;
using Navigation;
using VRSystemConfig;
using Axiom.Math;

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
        private const string defaultMapFile = VRSetting.defaultMapFileName;

        Random Rand = new Random();

        /// <summary>
        /// Form描画処理クラス
        /// </summary>
        private VehicleRunnerForm_Draw formDraw = new VehicleRunnerForm_Draw();

        /// <summary>
        /// 自律走行フラグ
        /// </summary>
        public bool bRunAutonomous = false;

        /// <summary>
        /// エミュ プロセス
        /// </summary>
        Process processCarEmu = null;

        // ハードウェア用 周期の短いカウンタ
        private int updateHwCnt = 0;

        /// <summary>
        /// アプリ終了フラグ
        /// </summary>
        private bool appExit = false;

        /// <summary>
        /// bServer IpAddr
        /// </summary>
        private string bServerAddr = VRSetting.bServerIPAddr;

        /// <summary>
        /// bServer エミュレータ
        /// </summary>
        private string bServerEmuAddr = VRSetting.bServerEmuIPAddr;

        // ロータリーエンコーダ座標計算用
        private PointD wlR = new PointD();
        private PointD wlL = new PointD();
        private double reOldR, reOldL;
        private double reAng;

        // ================================================================================================================

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VehicleRunnerForm()
        {
            InitializeComponent();

            // セルシオコントローラ初期化
            CersioCt = new CersioCtrl();

            // bServer Emu 接続開始
            CersioCt.Connect_bServer(bServerEmuAddr);

            // ブレイン起動
            BrainCtrl = new Brain(CersioCt, defaultMapFile);

            // マップウィンドウサイズのbmp作成
            formDraw.MakePictureBoxWorldMap( BrainCtrl.LocSys.mapBmp, picbox_AreaMap);

            // センサー値取得 スレッド起動
            Thread trdSensor = new Thread(new ThreadStart(ThreadSensorUpdate_bServer));
            trdSensor.IsBackground = true;
            trdSensor.Priority = ThreadPriority.AboveNormal;
            trdSensor.Start();

            // Accel Flag
            cb_AccelOff_CheckedChanged(this, null);

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

            // マップ名設定
            tb_MapName.Text = BrainCtrl.LocSys.mapData.MapName;

            // 画面更新
            PictureUpdate();

            // ハードウェア更新タイマ起動
            //tm_UpdateHw.Enabled = true;
            // 位置管理定期処理タイマー起動
            tm_Update.Enabled = true;

            //tm_SendCom.Enabled = true;

            // 設定されている車
            lbl_CarName.Text = VRSetting.CarName; 
        }

        /// <summary>
        /// フォームクローズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VehicleRunnerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
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
            tm_Update.Enabled = false;
        }


        // Draw -------------------------------------------------------------------------------------------
        enum MAP_MODE
        {
            AREA_MAP = 0,
            WORLD_MAP,
            MAX
        };

        MAP_MODE selAreaMapMode = MAP_MODE.AREA_MAP;

        /// <summary>
        /// エリアマップ描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picbox_AreaMap_Paint(object sender, PaintEventArgs e)
        {
            if (selAreaMapMode == MAP_MODE.AREA_MAP)
            {
                // エリアマップ描画
                formDraw.AreaMap_Draw_Area(e.Graphics, picbox_AreaMap, ref BrainCtrl.LocSys,
                                           (viewScrollX + viewMoveAddX), (viewScrollY + viewMoveAddY),
                                           selCpIndex );
                //formDraw.AreaMap_Draw_Ruler(e.Graphics, ref BrainCtrl, picbox_AreaMap.Width, picbox_AreaMap.Height);
            }
            else if (selAreaMapMode == MAP_MODE.WORLD_MAP)
            {
                // ワールドマップ描画
                formDraw.AreaMap_Draw_WorldMap(e.Graphics, picbox_AreaMap, ref BrainCtrl.LocSys);
            }

            // テキスト描画
            //formDraw.AreaMap_Draw_Text(e.Graphics, ref BrainCtrl, (long)updateHwCnt);
        }

        /// <summary>
        /// 表示切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picbox_AreaMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // エリア、ワールドマップ切り替え
                selAreaMapMode = (MAP_MODE)(((int)selAreaMapMode+1) % (int)MAP_MODE.MAX);
                viewScrollX = 0;
                viewScrollY = 0;
            }
        }


        // ---------------------------------------------------------------------------------------------------
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

        /// <summary>
        /// Form内のピクチャー更新
        /// </summary>
        private void PictureUpdate()
        {
            // 自己位置マーカー入りマップBmp描画
            //BrainCtrl.LocSys.UpdateLocalizeBitmap(true);

            // 各PictureBox更新
            this.picbox_AreaMap.Refresh();
            this.picbox_Indicator.Refresh();
        }

        /// <summary>
        /// VRunner リセット
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PositionReset_Click(object sender, EventArgs e)
        {
            BrainCtrl.Reset_StartPosition( true );
            PictureUpdate();
        }

        //--------------------------------------------------------------------------------------------------------------------
        // タイマイベント処理

        /// <summary>
        /// センサー値更新 スレッド
        /// (ROS-IF)
        /// </summary>
        private void ThreadSensorUpdate_bServer()
        {
            int oldTick = System.Environment.TickCount;

            while (!appExit)
            {
                // bServer ハードウェア(センサー)情報取得
                if (null != CersioCt)
                {
                    CersioCt.UpDate();
                }

                // Sleep
                // 20Hz
                {
                    int nowTick = System.Environment.TickCount;
                    int sleepTick = (oldTick+50) - nowTick;

                    oldTick = (nowTick + (sleepTick < 0 ? 0 : sleepTick));
                    if (sleepTick > 0)
                    {
                        Thread.Sleep(sleepTick);
                    }
                }
            }
        }


        /// <summary>
        /// 描画と親密な定期処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tm_Update_Tick(object sender, EventArgs e)
        {
            if (null != BrainCtrl && null != BrainCtrl.LocSys)
            {
                LocationSystem LocSys = BrainCtrl.LocSys;

                // tm_UpdateHw_Tick
                {
                    // 実走行時、bServerと接続が切れたら再接続
                    if (updateHwCnt % 100 == 0)
                    {
                        // 切断状態なら、自動接続
                        if (!CersioCt.TCP_IsConnected())
                        {
                            //
                            //CersioCt.Connect_bServer_Async(bServerEmuAddr);
                            CersioCt.Connect_bServer_Async(bServerAddr);
                        }
                    }

                    // ロータリーエンコーダ（タイヤ回転数）情報
                    if (CersioCt.bhwRE)
                    {
                        lbl_RErotR.Text = CersioCt.hwRErotR.ToString("f1");
                        lbl_RErotL.Text = CersioCt.hwRErotL.ToString("f1");
                    }

                    // AMCL
                    LocSys.Input_ROSPosition(CersioCt.hwAMCL_X, CersioCt.hwAMCL_Y, CersioCt.hwAMCL_Ang);
                    if (CersioCt.bhwTrgAMCL)
                    {
                        // 受信再開時の初期化
                        LocSys.Reset_ROSPosition(CersioCt.hwAMCL_X, CersioCt.hwAMCL_Y, CersioCt.hwAMCL_Ang);
                        CersioCt.bhwTrgAMCL = false;
                    }

                    // VehicleRunner Plot
                    {
                        PointD wlPos = new PointD();
                        REncoderToMap.CalcWheelPlotXY( ref wlR, ref wlL, ref reAng,
                                                       CersioCt.hwRErotR,
                                                       CersioCt.hwRErotL,
                                                       reOldR,
                                                       reOldL);

                        wlPos.X = (wlR.X + wlL.X) * 0.5;
                        wlPos.Y = (wlR.Y + wlL.Y) * 0.5;

                        LocSys.Input_REPosition(wlPos.X, wlPos.Y, -reAng);

                        reOldR = CersioCt.hwRErotR;
                        reOldL = CersioCt.hwRErotL;
                    }

                    // LED状態 画面表示
                    if (CersioCt.LEDCtrl.ptnHeadLED == -1)
                    {
                        lbl_LED.Text = "ND";
                    }
                    else
                    {
                        string ledStr = CersioCt.LEDCtrl.ptnHeadLED.ToString();

                        if (CersioCt.LEDCtrl.ptnHeadLED >= 0 && CersioCt.LEDCtrl.ptnHeadLED < LEDControl.LEDMessage.Count())
                        {
                            ledStr += "," + LEDControl.LEDMessage[CersioCt.LEDCtrl.ptnHeadLED];
                        }

                        if (!ledStr.Equals(lbl_LED.Text))
                        {
                            lbl_LED.Text = ledStr;
                        }
                    }

                    // 現在座標 表示
                    lbl_REPlotX.Text = LocSys.GetResultLocationX().ToString("F2");
                    lbl_REPlotY.Text = LocSys.GetResultLocationY().ToString("F2");
                    lbl_REPlotDir.Text = LocSys.GetResultAngle().ToString("F2");

                    // CheckPointIndex
                    if (BrainCtrl.LocSys.RTS.TrgCheckPoint())
                    {
                        // チェックポイント通過時に表示更新
                        numericUD_CheckPoint.Value = BrainCtrl.LocSys.RTS.getCheckPointIdx();
                    }

                    // BoxPC接続状態確認
                    if (CersioCt.TCP_IsConnected())
                    {
                        // 接続ＯＫ
                        tb_SendData.BackColor = Color.Lime;
                        tb_ResiveData.BackColor = Color.Lime;
                        lb_BServerConnect.Text = "bServer [" + CersioCt.TCP_GetConnectedAddr() + "] 接続OK";
                        lb_BServerConnect.BackColor = Color.Lime;
                    }
                    else
                    {
                        // 接続ＮＧ
                        tb_SendData.BackColor = SystemColors.Window;
                        tb_ResiveData.BackColor = SystemColors.Window;
                        lb_BServerConnect.Text = "bServer 未接続";
                        lb_BServerConnect.BackColor = SystemColors.Window;
                    }

                    // 送受信文字 画面表示
                    if (!string.IsNullOrEmpty(CersioCt.hwResiveStr))
                    {
                        tb_ResiveData.Text = CersioCt.hwResiveStr.Replace('\n', ' ');
                        CersioCt.hwResiveStr = "";
                    }
                    if (!string.IsNullOrEmpty(CersioCt.hwSendStr))
                    {
                        tb_SendData.Text = CersioCt.hwSendStr.Replace('\n', ' ');
                        CersioCt.hwSendStr = "";
                    }

                    updateHwCnt++;
                }

                // マップ上の現在位置更新
                // 現在位置を、AMCL,REPlotどちらを使うか
                LocationSystem.LOCATION_SENSOR selSensor = (rb_SelAMCL.Checked ? LocationSystem.LOCATION_SENSOR.AMCL : LocationSystem.LOCATION_SENSOR.REPlot);
                BrainCtrl.LocSys.update_NowLocation(selSensor);

                // 自律走行(緊急ブレーキ、壁よけ含む)処理 更新
                BrainCtrl.AutonomousProc( cb_InDoorMode.Checked, bRunAutonomous );

                // 距離計
                tb_Trip.Text = (LocSys.GetResultDistance_mm() * (1.0 / 1000.0)).ToString("f2");
            }

            // REからのスピード表示
            tb_RESpeed.Text = ((CersioCt.SpeedMmSec*3600.0)/(1000.0*1000.0)).ToString("f2");    // Km/Hour
            //tb_RESpeed.Text = (CersioCt.SpeedMmSec).ToString("f2"); // mm/Sec


            // ハンドル、アクセル値　表示
            tb_AccelVal.Text = CersioCt.nowSendAccValue.ToString("f2");
            tb_HandleVal.Text = CersioCt.nowSendHandleValue.ToString("f2");

            // 自律走行情報
            if (bRunAutonomous)
            {
                // 動作内容TextBox表示
                lbl_BackProcess.Text = "自律走行 モード";
            }
            else
            {
                lbl_BackProcess.Text = "モニタリング モード";
            }

            // bServerエミュレーション表記
            if (CersioCt.TCP_GetConnectedAddr() == bServerEmuAddr) lbl_bServerEmu.Visible = true;
            else                                                   lbl_bServerEmu.Visible = false;

            // 画面描画
            PictureUpdate();
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
                LocationSystem LocSys = BrainCtrl.LocSys;

                bRunAutonomous = true;
                cb_StartAutonomous.BackColor = Color.LimeGreen;
            }
            else
            {
                // 停止
                bRunAutonomous = false;

                CersioCt.SendCommand_Stop();
                cb_StartAutonomous.BackColor = SystemColors.Window;
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
                // 自己位置計算 一時停止
                tm_Update.Enabled = false;

                // Mapロード
                BrainCtrl = new Brain(CersioCt, dlg.FileName);

                // マップウィンドウサイズのbmp作成
                formDraw.MakePictureBoxWorldMap(BrainCtrl.LocSys.mapBmp, picbox_AreaMap);

                // マップ名設定
                tb_MapName.Text = BrainCtrl.LocSys.mapData.MapName;

                // 自己位置計算 再開
                tm_Update.Enabled = true;
            }
        }


        bool bMouseMove = false;
        int MouseStX, MouseStY;
        int viewMoveAddX, viewMoveAddY;
        int viewScrollX, viewScrollY;

        private void numericUD_CheckPoint_Click(object sender, EventArgs e)
        {
            //
            int idx = (int)numericUD_CheckPoint.Value;
            LocationSystem LocSys = BrainCtrl.LocSys;
            LocSys.RTS.SetCheckPoint(idx);
        }

        private void numericUD_CheckPoint_KeyPress(object sender, KeyPressEventArgs e)
        {
            numericUD_CheckPoint_Click(sender, null);
        }

        private void numericUD_DebugDir_Click(object sender, EventArgs e)
        {
            CersioCt.hwAMCL_Ang = (double)numericUD_DebugDir.Value;
        }

        private void numericUD_DebugX_Click(object sender, EventArgs e)
        {
            // 
            CersioCt.hwAMCL_X = (double)numericUD_DebugX.Value;
        }

        private void numericUD_DebugY_Click(object sender, EventArgs e)
        {
            //
            CersioCt.hwAMCL_Y = (double)numericUD_DebugY.Value;
        }

        // -----------------------------------------------------------------
        // マップスクロール
        private void picbox_AreaMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && selAreaMapMode == MAP_MODE.AREA_MAP)
            {
                //
                bMouseMove = true;
                MouseStX = e.X;
                MouseStY = e.Y;
                viewMoveAddX = 0;
                viewMoveAddY = 0;
            }
        }

        /// <summary>
        /// bServer Emu接続
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_bServerEmu_Click(object sender, EventArgs e)
        {
            string connectAddr = bServerAddr;

            if (null == processCarEmu || processCarEmu.HasExited)
            {
                if (Process.GetProcessesByName("CersioSim").Length == 0)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    // エミュレータ起動
                    string stCurrentDir = System.IO.Directory.GetCurrentDirectory();
                    stCurrentDir = stCurrentDir.Substring(0, stCurrentDir.IndexOf("\\VehicleRunner2"));
                    processCarEmu = Process.Start(stCurrentDir + "\\VehicleRunner2\\CersioSim\\bin\\Release\\CersioSim.exe", BrainCtrl.LocSys.mapData.MapFileName);

                    // 通信状態まで待つ
                    Application.DoEvents();
                    processCarEmu.WaitForInputIdle();
                    System.Threading.Thread.Sleep(6000);

                    Cursor.Current = Cursors.Default;
                }
            }

            // エミュレータIPアドレス
            connectAddr = bServerEmuAddr;

            // bServer接続
            CersioCt.Connect_bServer_Async(connectAddr);
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 選択中チェックポイント
        /// </summary>
        int selCpIndex = -1;

        private void picbox_AreaMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //
                if (bMouseMove)
                {
                    viewMoveAddX = (MouseStX - e.X);
                    viewMoveAddY = (MouseStY - e.Y);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
            }
            else
            {
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    LocationSystem LocSys = BrainCtrl.LocSys;
                    int msPosX = e.X - (picbox_AreaMap.Width / 2) + viewScrollX;
                    int msPosY = e.Y - (picbox_AreaMap.Height / 2) + viewScrollY;
                    MarkPoint _checkPos = new MarkPoint(new DrawMarkPoint(msPosX, msPosY, 0.0), LocSys);

                    selCpIndex = LocSys.GetCheckPointIndex(_checkPos.x, _checkPos.y);
                }
            }
        }

        private void picbox_AreaMap_MouseUp(object sender, MouseEventArgs e)
        {
            //
            if (e.Button == MouseButtons.Left && selAreaMapMode == MAP_MODE.AREA_MAP)
            {
                viewMoveAddX = 0;
                viewMoveAddY = 0;
                viewScrollX += (MouseStX - e.X);
                viewScrollY += (MouseStY - e.Y);
            }
            bMouseMove = false;
            selCpIndex = -1;
        }

        /// <summary>
        /// アクセルOff ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_AccelOff_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_AccelOff.Checked) CersioCt.bSendAccel = false;
            else CersioCt.bSendAccel = true;
        }
    }
}
