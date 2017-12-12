
#define EBS_HANDLE_LINK  // ハンドルの向き追従　緊急ブレーキ

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Location;
using CersioIO;
using Axiom.Math;       // Vector3D計算ライブラリ

using System.Drawing;
using VRSystemConfig;

namespace Navigation
{
    /// <summary>
    /// セルシオ制御　頭脳クラス
    /// </summary>
    public class Brain
    {
        /// <summary>
        /// セルシオ制御クラス
        /// </summary>
        public CersioCtrl CarCtrl;

        /// <summary>
        /// 現在位置システム
        /// </summary>
        public LocationSystem LocSys;

        /// <summary>
        /// モード
        /// </summary>
        public ModeControl ModeCtrl;


        /// <summary>
        /// 更新カウンタ
        /// </summary>
        private long UpdateCnt;


        /// <summary>
        /// ゴール到達フラグ
        /// </summary>
        public bool goalFlg = false;

        /// <summary>
        /// 自己位置補正要請フラグ
        /// </summary>
        public bool bRevisionRequest = false;

        /// <summary> bServer接続状態フラグ </summary>
        private bool bServerConnectFlg = false;
        /// <summary> bServer接続タイミング　トリガ </summary>
        private bool trg_bServerConnect = false;

        /// <summary>自律モードフラグ </summary>
        bool bRunAutonomousOld;

        /// <summary>
        /// EBS 緊急ブレーキ
        /// </summary>
        //public EmergencyBrake EBS = new EmergencyBrake();

        /// <summary>
        /// ブレーキ継続カウンタ
        /// </summary>
        public int EmgBrakeContinueCnt = 0;

        /// <summary>
        /// EHS 緊急時ハンドル動作
        /// </summary>
        //public EmergencyHandring EHS = new EmergencyHandring();

        // ログ 追記事項出力
        public static string addLogMsg;
        public int noPowerTrainCnt;
        public int noFowrdCnt;
        private double backHandle;


        /// <summary>
        /// 回避ルートイメージ
        /// </summary>
        //public Bitmap AvoidRootImage;

        //public DateTime AvoidRootDispTime;

        // ===========================================================================================================

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ceCtrl"></param>
        public Brain(CersioCtrl ceCtrl)
        {
            CarCtrl = ceCtrl;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init(MapData mapData)
        {
            ModeCtrl = new ModeControl();
            // チェックポイントへ向かう
            ModeCtrl.SetActionMode(ModeControl.ActionMode.CheckPoint);

            // Locpresump
            //  マップ画像ファイル名、実サイズの横[mm], 実サイズ縦[mm] (北向き基準)
            LocSys = new LocationSystem(mapData);
            UpdateCnt = 0;

            // 現在座標リセット
            Reset_Rooting( true );
        }

        /// <summary>
        /// ルーティング情報をリセット
        /// </summary>
        public void Reset_Rooting( bool bResetSeq )
        {
            // ゴール判定 フラグOff
            goalFlg = false;

            // シーケンス初期化
            if (bResetSeq)
            {
                LocSys.RTS.ResetSeq();
            }
        }

        /// <summary>
        /// 自律走行処理 定期更新(100ms)
        /// </summary>
        /// <param name="bRunAutonomous"></param>
        /// <param name="bMoveBaseCtrl"></param>
        /// <param name="SpeedKmh"></param>
        /// <returns></returns>
        public bool AutonomousProc( bool bRunAutonomous, bool bMoveBaseCtrl, double SpeedKmh )
        {
            // 自立走行に切り替わった瞬間か？
            bool trgRunAutonomous = (bRunAutonomousOld != bRunAutonomous && bRunAutonomous) ? true : false;

            UpdateCnt++;

            // すべてのルートを回りゴールした。
            if (goalFlg)
            {
                CarCtrl.nowAccValue = 0.0;
                CarCtrl.nowHandleValue = 0.0f;
                // 停止
                CarCtrl.SetCommandAC(0.0, 0.0);

                // スマイル
                CarCtrl.SetHeadMarkLED(LEDControl.LED_PATTERN.Smile);

                return goalFlg;
            }

            
            // 現在座標更新
            LocSys.RTS.SetNowPostion(LocSys.GetResultLocationX(),
                              LocSys.GetResultLocationY(),
                              LocSys.GetResultAngle());

            // チェックポイント送信
            {
                // チェックポイントをROSへ指示(bServer接続時も配信)
                if (LocSys.RTS.TrgCheckPoint() || trg_bServerConnect || trgRunAutonomous )
                {
                    Vector3 checkPnt = LocSys.RTS.GetCheckPointToWayPoint();// LocSys.RTS.getNowCheckPoint();
                    CarCtrl.SetCommandAG(checkPnt.x, checkPnt.y, checkPnt.z );
                }

                // 再スタート時
                if (trgRunAutonomous)
                {
                    //  0.0を一度送信
                    ModeCtrl.SetActionMode(ModeControl.ActionMode.CheckPoint);
                    CarCtrl.SetCommandAC(0.0, 0.0);
                    CarCtrl.nowAccValue = 0.0;
                    CarCtrl.nowHandleValue = 0.0;
                    noFowrdCnt = 0;
                    noPowerTrainCnt = 0;
                }
            }

            // LED指示
            LedUpdate(bRunAutonomous);

            // モード更新
            ModeCtrl.Update(LocSys.GetResultDistance_mm());

            // ルート計算
            LocSys.RTS.CalcRooting(bRunAutonomous);

            if (bRunAutonomous)
            {
                // 走行指示出力
                /*
                // ROSの回転角度から、Benzハンドル角度の上限に合わせる
                double angLimit = (CarCtrl.hwMVBS_Ang * 180.0 / Math.PI);
                if (angLimit > 30.0) angLimit = 30.0;
                if (angLimit < -30.0) angLimit = -30.0;


                double moveAng = -(angLimit / 30.0);//-CarCtrl.hwMVBS_Ang;// * 0.3; 
                */
                double moveAng = CarCtrl.hwMVBS_Ang *2.0; 

                if (ModeCtrl.GetActionMode() == ModeControl.ActionMode.CheckPoint)
                {
                    // モード：チェックポイント目指す

                    if (bMoveBaseCtrl)
                    {
                        // move_baseコントロール
                        if (CarCtrl.hwMVBS_X > 0.0)
                        {
                            // move_baseから前進指示の場合
                            double speedRate = CarCtrl.hwMVBS_X;

                            // ハンドルで曲がるときは、速度を下げる
                            if (Math.Abs(moveAng) > 0.25) speedRate *= 0.75;

                            CarCtrl.CalcHandleAccelControl(moveAng, CarCtrl.CalcAccelFromSpeed(SpeedKmh * speedRate, true));
                            //CarCtrl.SendCalcHandleAccelControl(moveAng, getAccelValue()*0.5);
                            ModeCtrl.TmpCnt = 0;
                        }
                        else
                        {
                            // move_baseから前進指示なし
                            ModeCtrl.TmpCnt++;
                            // 0.5秒
                            if (ModeCtrl.TmpCnt > 5)
                            {
                                ModeCtrl.SetActionMode(ModeControl.ActionMode.EmergencyStop);
                            }
                        }

                        // ACコマンド送信
                        CarCtrl.SetCommandAC(CarCtrl.nowHandleValue, CarCtrl.nowAccValue);
                    }
                    else
                    {
                        // VR コントロール
                        // ルートにそったハンドル、アクセル値を取得
                        double handleTgt = LocSys.RTS.GetHandleValue();
                        //double accTgt = LocSys.RTS.GetAccelValue();

                        double speedRate = 1.0;

                        // ハンドルで曲がるときは、速度を下げる
                        if (Math.Abs(handleTgt) > 0.25) speedRate = 0.75;

                        CarCtrl.CalcHandleAccelControl(handleTgt, CarCtrl.CalcAccelFromSpeed(SpeedKmh * speedRate, true));

                        // ACコマンド送信
                        CarCtrl.SetCommandAC(CarCtrl.nowHandleValue, CarCtrl.nowAccValue);
                    }

                    // 共通処理

                    // 動輪の状態確認
                    if (CarCtrl.sendAccelValue >= 0.1)
                    {
                        // 動力出力指示にかかわらず、(速度50mm/s以下)
                        if (CarCtrl.nowSpeedMmSec <= 50.0)
                        {
                            // スタック(前進不能)カウンタ
                            noFowrdCnt++;
                            if (noFowrdCnt > 1000)
                            {
                                // バックで脱出を試みる
                                ModeCtrl.SetActionMode(ModeControl.ActionMode.MoveBack);
                                CarCtrl.nowAccValue = 0.0;
                                noFowrdCnt = 0;
                            }

                            // 動力出力指示にかかわらずほぼ静止(速度10mm/s以下)
                            if (CarCtrl.nowSpeedMmSec <= 10.0)
                            {
                                // 動力カット状態カウンタ
                                noPowerTrainCnt++;

                                // 10秒以上経過
                                if (noPowerTrainCnt > 100 )
                                {
                                    // 出力0%から再スタート
                                    CarCtrl.nowAccValue = 0.0;
                                    noPowerTrainCnt = 0;
                                }
                            }
                        }
                        else
                        {
                            // 指示どおり走行している状態
                            noPowerTrainCnt = 0;
                            noFowrdCnt = 0;
                        }
                    }
                    else
                    {
                        // 指示通り静止している状態
                    }

                }
                else if (ModeCtrl.GetActionMode() == ModeControl.ActionMode.EmergencyStop)
                {
                    if (ModeCtrl.GetActionCount() < 5)
                    {
                        // 徐行
                        CarCtrl.nowAccValue = 0.1;
                        CarCtrl.CalcHandleAccelControl(moveAng, CarCtrl.nowAccValue);
                    }
                    else
                    {
                        // move_baseから停止状態
                        //double speedRate = CarCtrl.hwMVBS_X;
                        //speedRate *= 0.75;

                        if (CarCtrl.hwMVBS_X > 0.0)
                        {
                            // 徐行解除
                            ModeCtrl.SetActionMode(ModeControl.ActionMode.CheckPoint);
                        }
                        else
                        {
                            ModeCtrl.TmpCnt++;

                            // その場、回転
                            if (Math.Abs(CarCtrl.hwMVBS_Ang) >= 0.01)
                            {
                                // 減速 [0.5km/h]
                                //CarCtrl.CalcHandleAccelControl(0.0, 0.0);
                                CarCtrl.CalcHandleAccelControl(moveAng, CarCtrl.CalcAccelFromSpeed(0.25, true));

                                if (ModeCtrl.TmpCnt > 20)
                                {
                                    // Back モード変更
                                    ModeCtrl.SetActionMode(ModeControl.ActionMode.MoveBack);
                                    CarCtrl.nowAccValue = 0.0;
                                }
                            }
                            else
                            {
                                // 停止
                                if (ModeCtrl.TmpCnt > 20)
                                {
                                    ModeCtrl.SetActionMode(ModeControl.ActionMode.EmergencyStop);
                                }
                            }
                        }
                    }

                    // ACコマンド送信
                    CarCtrl.SetCommandAC(CarCtrl.nowHandleValue, CarCtrl.nowAccValue);
                }
                else if (ModeCtrl.GetActionMode() == ModeControl.ActionMode.StackStop)
                {
                    // 完全停止
                    if (ModeCtrl.GetModePassSeconds() > 8)
                    {
                        ModeCtrl.SetActionMode(ModeControl.ActionMode.EmergencyStop);
                    }
                }
                else if (ModeCtrl.GetActionMode() == ModeControl.ActionMode.MoveBack)
                {
                    // モード：バック動作
                    if (ModeCtrl.GetActionCount() == 0)
                    {
                        // 逆ハンドル設定
                        if (Math.Abs(CarCtrl.nowTargetHandle) > 0.1)
                        {
                            backHandle = -CarCtrl.nowTargetHandle;
                        }
                        else
                        {
                            backHandle = -LocSys.RTS.GetHandleValue();
                        }
                    }

                    if (ModeCtrl.GetActionCount() < 5)
                    {
                        // スピコンブレーキ解除
                        CarCtrl.SetCommandAC(0.0, 0.0);
                    }
                    else
                    {
                        // 400mm バック
                        //if (ModeCtrl.GetModePassSeconds() > 3)
                        if (ModeCtrl.PassDistanceMm() > 400.0)
                        {
                            // 復旧
                            ModeCtrl.SetActionMode(ModeControl.ActionMode.CheckPoint);
                            CarCtrl.nowAccValue = 0.0;

                            // チェックポイント再送
                            {
                                Vector3 checkPnt = LocSys.RTS.GetCheckPointToWayPoint();
                                CarCtrl.SetCommandAG(checkPnt.x, checkPnt.y, checkPnt.z);
                            }
                        }
                        else
                        {
                            // ハンドル維持して、バック
                            CarCtrl.CalcHandleAccelControl(backHandle, CarCtrl.CalcAccelFromSpeed(SpeedKmh * 0.5, false));
                        }

                        // ACコマンド送信
                        CarCtrl.SetCommandAC(CarCtrl.nowHandleValue, CarCtrl.nowAccValue);
                    }
                }
                else
                {
                    // モード：未設定
                    ModeCtrl.SetActionMode(ModeControl.ActionMode.CheckPoint);
                }
            }
            else
            {
                // 走行指示出力しない
                CarCtrl.nowAccValue = 0.0;
                CarCtrl.nowHandleValue = 0.0f;

                ModeCtrl.SetActionMode(ModeControl.ActionMode.CheckPoint);
            }

            // 接続トリガ取得
            trg_bServerConnect = false;
            if (!bServerConnectFlg && CarCtrl.TCP_IsConnected()) trg_bServerConnect = true;

            bServerConnectFlg = CarCtrl.TCP_IsConnected();

            bRunAutonomousOld = bRunAutonomous;

            // ゴール状態取得
            goalFlg = LocSys.RTS.GetGoalStatus();

            return goalFlg;
        }

        /// <summary>
        /// 自己状態確認
        /// ※ヘルスチェック
        /// </summary>
        /// <returns></returns>
        public bool SystemProc()
        {
            // bServer接続確認

            // ROS IFで各センサー情報を確認？

            // 変化のないセンサーを監視？

            // バッテリーの電圧の取得

            return true;
        }

        /// <summary>
        /// 定期更新(100ms周期)
        /// </summary>
        /// <param name="LocSys"></param>
        /// <returns>true...バック中(緊急動作しない)</returns>
        /// 
        public bool LedUpdate( bool bRunAutonomous )
        {
            if (bRunAutonomous)
            {
                // 自律走行時
                if (ModeCtrl.GetActionMode() == ModeControl.ActionMode.CheckPoint)
                {
                    if (LocSys.RTS.TrgCheckPoint())
                    {
                        // チェックポイント通過
                        CarCtrl.SetHeadMarkLED(LEDControl.LED_PATTERN.CheckPointFlash, true);
                    }
                    else
                    {
                        // 通常時
                        if (CarCtrl.nowHandleValue >= 0.8)
                        {
                            // Right Turn
                            CarCtrl.SetHeadMarkLED(LEDControl.LED_PATTERN.RightTurn, false);
                        }
                        else if (CarCtrl.nowHandleValue <= -0.8)
                        {
                            // Left Turn
                            CarCtrl.SetHeadMarkLED(LEDControl.LED_PATTERN.LeftTurn, false);
                        }
                        else
                        {
                            // LED Normal
                            CarCtrl.SetHeadMarkLED(LEDControl.LED_PATTERN.Normal, false);
                        }
                    }
                }
                else if (ModeCtrl.GetActionMode() == ModeControl.ActionMode.EmergencyStop)
                {
                    // 異常停止状態
                    // 赤
                    CarCtrl.SetHeadMarkLED(LEDControl.LED_PATTERN.RedAlart, true);
                }
                else if (ModeCtrl.GetActionMode() == ModeControl.ActionMode.MoveBack)
                {
                    // バック中
                    // 黄色ハザード
                    CarCtrl.SetHeadMarkLED(LEDControl.LED_PATTERN.Warning, true);
                }
            }
            else
            {
                // 待機状態
                CarCtrl.SetHeadMarkLED(LEDControl.LED_PATTERN.Smile, true);
            }

            return true;
        }
    }
}
