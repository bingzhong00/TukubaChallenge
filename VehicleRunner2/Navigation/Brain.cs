
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


        public ModeControl ModeCtrl;


        /// <summary>
        /// ハンドル、アクセル操作
        /// </summary>
        private double handleValue;
        private double accValue;

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


        // バック動作中フラグ
        public bool bNowBackProcess = false;

        /// <summary>
        /// 回避ルートイメージ
        /// </summary>
        public Bitmap AvoidRootImage;

        public DateTime AvoidRootDispTime;

        //string nowMapFileName;

        public MapData nowMapData;

        int boostAccelCnt = 0;
        // ===========================================================================================================

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ceCtrl"></param>
        public Brain(CersioCtrl ceCtrl, MapData mapData)
        {
            CarCtrl = ceCtrl;
            nowMapData = mapData;
            Reset();

            // チェックポイントへ向かう
            ModeCtrl.SetActionMode(ModeControl.ActionMode.CheckPoint);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            ModeCtrl = new ModeControl();

            // Locpresump
            //  マップ画像ファイル名、実サイズの横[mm], 実サイズ縦[mm] (北向き基準)
            LocSys = new LocationSystem(nowMapData);

            UpdateCnt = 0;

            handleValue = 0.0;
            accValue = 0.0;

            // 現在座標リセット
            Reset_StartPosition( true );
        }

        /// <summary>
        /// 座標情報をセット
        /// </summary>
        public void Reset_StartPosition( bool bResetSeq )
        {
            // ゴール判定 フラグOff
            goalFlg = false;

            // シーケンス初期化
            if (bResetSeq)
            {
                LocSys.RTS.ResetSeq();
            }
        }

        // グリーン(補正要請)エリア侵入フラグ
        bool bGreenAreaFlg = false;

        int backCnt = 0;

        /// <summary>
        /// 自律走行処理 定期更新
        /// </summary>
        /// <param name="bIndoorMode">屋内モード</param>
        /// <param name="bCtrlOutput">CarCtrlに動作出力</param>
        /// <returns></returns>
        public bool AutonomousProc( bool bRunAutonomous, bool bIndoorMode, bool bCtrlOutput, double SpeedKmh )
        {
            // すべてのルートを回りゴールした。
            if (goalFlg)
            {
                // 停止情報送信
                // バックでブレーキ
                if (backCnt < 100)
                {
                    CarCtrl.SetCommandAC(0.0, -1.0);
                }
                else
                {
                    CarCtrl.SetCommandAC(0.0, 0.0);
                }
                backCnt++;

                // スマイル
                CarCtrl.SetHeadMarkLED(LEDControl.LED_PATTERN.SMILE);

                return goalFlg;
            }
            else
            {
                backCnt = 0;

                // チェックポイントをROSへ指示(bServer接続時も配信)
                if (LocSys.RTS.TrgCheckPoint() || trg_bServerConnect)
                {
                    Vector3 checkPnt = LocSys.RTS.GetCheckPointToWayPoint();// LocSys.RTS.getNowCheckPoint();
                    CarCtrl.SetCommandAP(checkPnt.x, checkPnt.y, checkPnt.z );
                }
            }

            // ハンドルレンジ設定
            //EHS.SetSensorRange(bIndoorMode);

            // 自走処理
            bNowBackProcess = ModeUpdate(LocSys, bRunAutonomous);

            if (bCtrlOutput)
            {
                // 走行指示出力

                // ROSの回転角度から、Benzハンドル角度の上限に合わせる
                double angLimit = (CarCtrl.hwMVBS_Ang * 180.0 / Math.PI);
                if (angLimit > 30.0) angLimit = 30.0;
                if (angLimit < -30.0) angLimit = -30.0;


                double moveAng = -(angLimit / 30.0);//-CarCtrl.hwMVBS_Ang;// * 0.3; 

                if (CarCtrl.hwMVBS_X >= 0.5)
                {
                    // move_baseから前進指示の場合
                    CarCtrl.CalcHandleAccelSlowControl( moveAng, CarCtrl.CalcSpeedControl(VRSetting.AccSpeedKm) );
                    //CarCtrl.SendCalcHandleAccelControl(moveAng, getAccelValue()*0.5);
                }
                else
                {
                    // move_baseから停止状態
                    CarCtrl.CalcHandleAccelSlowControl( moveAng, 0.0f);
                }

                // ACコマンド送信
                CarCtrl.SetCommandAC(CarCtrl.nowSendHandleValue, CarCtrl.nowSendAccValue);
            }
            else
            {
                // 走行指示出力しない
                CarCtrl.nowSendAccValue = 0.0;
                CarCtrl.nowSendHandleValue = 0.0f;
            }

            // 接続トリガ取得
            trg_bServerConnect = false;
            if (!bServerConnectFlg && CarCtrl.TCP_IsConnected()) trg_bServerConnect = true;

            bServerConnectFlg = CarCtrl.TCP_IsConnected();

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
        /// <param name="useEBS">緊急ブレーキ</param>
        /// <param name="useEHS">壁避け動作</param>
        /// <param name="bStraightMode">直進モード</param>
        /// <returns>true...バック中(緊急動作しない)</returns>
        /// 
        public bool ModeUpdate(LocationSystem LocSys, bool bRunAutonomous )
        {
            // Rooting ------------------------------------------------------------------------------------------------
            ModeCtrl.update();

            if (ModeCtrl.GetActionMode() == ModeControl.ActionMode.CheckPoint)
            {
                // ルート進行
                // ルート算定
                // 現在座標更新
                LocSys.RTS.SetNowPostion(LocSys.GetResultLocationX(),
                                  LocSys.GetResultLocationY(),
                                  LocSys.GetResultAngle());

                // ルート計算
                LocSys.RTS.CalcRooting(bRunAutonomous);

                // チェックポイント通過をLEDで伝える
                if (LocSys.RTS.TrgCheckPoint())
                {
                    CarCtrl.SetHeadMarkLED(LEDControl.LED_PATTERN.WHITE_FLASH, true);
                }
            }

            // DriveControl -------------------------------------------------------------------------------------------

// マップ情報取得
#if false
            {
                Grid nowGrid = LocSys.GetMapInfo(LocSys.R1);

                // マップ情報反映
                if (nowGrid == Grid.BlueArea)
                {
                    // スローダウン
                    //EBS.AccelSlowDownCnt = 5;
                    Brain.addLogMsg += "ColorMap:Blue\n";
                }

                // 壁の中にいる
                if (nowGrid == Grid.RedArea )
                {
                    // 強制補正エリア
                    Brain.addLogMsg += "LocRivision:ColorMap[Red](マップ情報の位置補正)\n";
                    bRevisionRequest = true;
                }

                // 補正実行エリア
                {
                    if (nowGrid == Grid.GreenArea && !bGreenAreaFlg)
                    {
                        // 補正指示のエリアに入った
                        Brain.addLogMsg += "LocRivision:ColorMap[Green](マップ情報の位置補正)\n";
                        bGreenAreaFlg = true;

                        bRevisionRequest = true;
                    }

                    // 補正指示のエリアを抜けた判定
                    if (nowGrid != Grid.GreenArea)
                    {
                        bGreenAreaFlg = false;
                    }
                }
            }
#endif

            // 動作モードごとの走行情報更新
            // ハンドル、アクセルを調整
            if (ModeCtrl.GetActionMode() == ModeControl.ActionMode.CheckPoint)
            {
                // 通常時

                // ルートにそったハンドル、アクセル値を取得
                double handleTgt = LocSys.RTS.GetHandleValue();
                double accTgt = LocSys.RTS.GetAccelValue();

                if( Math.Abs( handleTgt ) > 0.25 )
                {
                    // ハンドルで曲がるときは、速度を下げる
                    accTgt *= 0.75;
                    //EBS.AccelSlowDownCnt = 5;
                }
                /*
                // スローダウン中 動作
                if (EBS.AccelSlowDownCnt > 0)
                {
                    // 遅くする
                    accValue = accTgt = RTS.getAccelValue() * 0.5;
                    EBS.AccelSlowDownCnt--;

                    // ＬＥＤパターンハザード
                    // スローダウンを知らせる
                    CarCtrl.SetHeadMarkLED((int)CersioCtrl.LED_PATTERN.HAZERD);
                }
                else
                {
                    // ＬＥＤパターン平常
                    CarCtrl.SetHeadMarkLED((int)CersioCtrl.LED_PATTERN.Normal);
                }


                // 緊急ブレーキ動作
                if (EBS.EmgBrk && useEBS)
                {
                    ModeCtrl.SetActionMode(ModeControl.ActionMode.EmergencyStop);
                }
                */

                handleValue = handleTgt;
                accValue = accTgt;
            }
            else if (ModeCtrl.GetActionMode() == ModeControl.ActionMode.EmergencyStop)
            {
                // 緊急停止状態
                // 赤
                CarCtrl.SetHeadMarkLED(LEDControl.LED_PATTERN.RED, true);
                accValue = 0.0;
            }
            /*
            else if (ModeCtrl.GetActionMode() == ModeControl.ActionMode.MoveBack)
            {
                // 復帰（バック）モード
                // 向かうべき方向とハンドルを逆に切り、バック
                double handleTgt = -RTS.getHandleValue();
                //double accTgt = RTS.getAccelValue();

                // 障害物をみて、ハンドルを切る方向を求める
                {
                    double ehsHandleVal = EHS.CheckEHS(LocSys.LRF.getData_UntiNoise());

                    if (useEHS && 0.0 != ehsHandleVal)
                    {
                        // 左右に壁を検知してたら、優先的によける
                        if (EHS.Result == EmergencyHandring.EHS_MODE.LeftWallHit ||
                            EHS.Result == EmergencyHandring.EHS_MODE.RightWallHit)
                        {
                            handleTgt = -ehsHandleVal;
                        }
                    }
                }

                handleValue = handleTgt;
                accValue = -EmergencyBrake.AccSlowdownRate; // スローダウンの速度でバック

                // ＬＥＤパターン
                // バック中
                CarCtrl.SetHeadMarkLED((int)CersioCtrl.LED_PATTERN.UnKnown1, true);


                // バック完了、回避ルートを動的計算
                if (ModeCtrl.GetModePassSeconds(10))
                {
                    ActiveDetour actDetour = new ActiveDetour(LocSys.worldMap.AreaGridMap, LocSys.LRF.getData());

                    // 現在位置と到達したい位置で、回避ルートを求める
                    {
                        int nowPosX, nowPosY;
                        double nowAng;
                        int tgtPosX = 0;
                        int tgtPosY = 0;
                        RTS.getNowPostion(out nowPosX, out nowPosY, out nowAng);
                        RTS.getNowTarget(ref tgtPosX, ref tgtPosY);

                        nowPosX = LocSys.worldMap.GetAreaX(nowPosX);
                        nowPosY = LocSys.worldMap.GetAreaX(nowPosY);

                        tgtPosX = LocSys.worldMap.GetAreaX(tgtPosX);
                        tgtPosY = LocSys.worldMap.GetAreaX(tgtPosY);

                        // 回避ルートを求める
                        List<Vector3> avoidCheckPoint = actDetour.Calc_DetourCheckPoint(nowPosX, nowPosY, tgtPosX, tgtPosY);

                        if (avoidCheckPoint.Count > 0)
                        {
                            // 回避ルートあり
                            // 新規ルートを作成
                            MapData avoidMap = new MapData();
                            avoidMap.checkPoint = avoidCheckPoint.ToArray();
                            avoidMap.MapName = "ActiveDetour AvoidMap";
                            avoidRTS = new Rooting(avoidMap);

                            // 回避情報を画面に表示
                            // 回避イメージ生成
                            AvoidRootImage = actDetour.getDetourRootImage();
                            // イメージ表示時間設定
                            AvoidRootDispTime = DateTime.Now.AddSeconds(15);

                            // 回避ルートモード
                            ModeCtrl.SetActionMode(ModeControl.ActionMode.Avoid);
                        }
                        else
                        {
                            // 回避ルートなし(回避不能)
                            // ※動作検討 
                            // とりあえず、チェックポイントへ向かう
                            ModeCtrl.SetActionMode(ModeControl.ActionMode.CheckPoint);
                        }
                    }
                }
            }
            */

            UpdateCnt++;

            // 回復モードか否か
            if (ModeCtrl.GetActionMode() != ModeControl.ActionMode.MoveBack) return false;
            return true;
        }


        /// <summary>
        /// ブレイン結果のハンドリング情報取得
        /// </summary>
        /// <returns></returns>
        public double getHandleValue()
        {
            return handleValue;
        }

        /// <summary>
        /// アクセル情報 取得
        /// </summary>
        /// <returns></returns>
        public double getAccelValue()
        {
            return accValue;
        }
    }
}
