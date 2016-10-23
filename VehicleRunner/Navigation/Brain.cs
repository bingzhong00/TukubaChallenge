
#define EBS_HANDLE_LINK  // ハンドルの向き追従　緊急ブレーキ

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LocationPresumption;
using CersioIO;
using SCIP_library;     // LRFライブラリ
using Axiom.Math;       // Vector3D計算ライブラリ
using ActiveDetourNavigation;

using System.Drawing;

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
        public LocPreSumpSystem LocSys;


        public ModeControl ModeCtrl;

        /// <summary>
        /// マップファイル
        /// </summary>
        public MapData MapFile;

        /// <summary>
        /// ルート制御
        /// </summary>
        public Rooting RTS;

        /// <summary>
        /// 回避ルーティング
        /// </summary>
        private Rooting avoidRTS;

        /// <summary>
        /// ハンドル、アクセル操作
        /// </summary>
        static public double handleValue;
        static public double accValue;

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

        /// <summary>
        /// EBS 緊急ブレーキ
        /// </summary>
        public EmergencyBrake EBS = new EmergencyBrake();

        /// <summary>
        /// ブレーキ継続カウンタ
        /// </summary>
        public int EmgBrakeContinueCnt = 0;

        /// <summary>
        /// EHS 緊急時ハンドル動作
        /// </summary>
        public EmergencyHandring EHS = new EmergencyHandring();

        // 自己位置補正計算回数
        //private const int numParticleFilter = 10;

        // ログ 追記事項出力
        public static string addLogMsg;


        // バック動作中フラグ
        public bool bNowBackProcess = false;

        /// <summary>
        /// 回避ルートイメージ
        /// </summary>
        public Bitmap AvoidRootImage;

        public DateTime AvoidRootDispTime;

        // ===========================================================================================================

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ceCtrl"></param>
        public Brain(CersioCtrl ceCtrl, string mapFileName )
        {
            CarCtrl = ceCtrl;
            Reset(mapFileName);
        }

        public void Reset(string mapFileName)
        {
            ModeCtrl = new ModeControl();
            // 
            MapFile = MapData.LoadMapFile(mapFileName);
            // 
            RTS = new Rooting(MapFile);

            //  マップ画像ファイル名、実サイズの横[mm], 実サイズ縦[mm] (北向き基準)
            LocSys = new LocPreSumpSystem(MapFile.MapImageFileName, MapFile.RealWidth, MapFile.RealHeight);

            UpdateCnt = 0;

            goalFlg = false;

            handleValue = 0.0;
            accValue = 0.0;

            Reset_StartPosition();
        }

        /// <summary>
        /// スタート位置に座標情報をセット
        /// </summary>
        public void Reset_StartPosition()
        {
            // スタート位置をセット
            LocSys.SetStartPostion((int)MapFile.startPosition.x,
                                   (int)MapFile.startPosition.y,
                                   MapFile.startDir);

            // REをリセット
            CarCtrl.SendCommand_RE_Reset();

            CarCtrl.setREPlot_Start(MapFile.startPosition.x * LocSys.MapToRealScale,
                                     MapFile.startPosition.y * LocSys.MapToRealScale,
                                     MapFile.startDir);
        }

        // グリーン(補正要請)エリア侵入フラグ
        bool bGreenAreaFlg = false;


        /// <summary>
        /// 自律走行処理 定期更新
        /// </summary>
        /// <param name="useEBS">壁回避ブレーキ</param>
        /// <param name="useEHS">壁回避ハンドル</param>
        /// <param name="bIndoorMode">屋内モード</param>
        /// <param name="bCtrlOutput">CarCtrlに動作出力</param>
        /// <returns></returns>
        public bool AutonomousProc( bool useEBS, bool useEHS, bool bIndoorMode, bool bCtrlOutput )
        {
            // すべてのルートを回りゴールした。
            if (goalFlg)
            {
                // 停止情報送信
                CarCtrl.SetCommandAC(0.0, 0.0);
                // スマイル
                CarCtrl.SetHeadMarkLED((int)CersioCtrl.LED_PATTERN.SMILE);

                return goalFlg;
            }

            // ハンドルレンジ設定
            EHS.SetSensorRange(bIndoorMode);

            // 自走処理
            bNowBackProcess = Update(LocSys, useEBS, useEHS);

            // 走行可能状態
            if (bCtrlOutput)
            {
                // ルート計算から、目標ハンドル、目標アクセル値をもらう。
                CarCtrl.CalcHandleAccelControl(getHandleValue(), getAccelValue());

                // 送信
                CarCtrl.SetCommandAC();
            }

            goalFlg = RTS.getGoalFlg();
            return goalFlg;
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
        public bool Update(LocPreSumpSystem LocSys, bool useEBS, bool useEHS)
        {
            // LRFデータ取得
            double[] lrfData = LocSys.LRF.getData();

            // Rooting ------------------------------------------------------------------------------------------------
            ModeCtrl.update();

            if (ModeCtrl.GetActionMode() == ModeControl.ActionMode.CheckPoint)
            {
                // ルート進行
                // ルート算定
                // 現在座標更新
                RTS.setNowPostion((int)LocSys.GetResultLocationX(),
                                  (int)LocSys.GetResultLocationY(),
                                  (int)LocSys.GetResultAngle());

                // ルート計算
                RTS.calcRooting();

                // チェックポイント通過をLEDで伝える
                if (RTS.IsCheckPointPass())
                {
                    CarCtrl.SetHeadMarkLED((int)CersioCtrl.LED_PATTERN.WHITE_FLASH, true);
                }
            }
            else if (ModeCtrl.GetActionMode() == ModeControl.ActionMode.Avoid)
            {
                // 回避ルート進行
                // ルート算定
                // 現在座標更新
                avoidRTS.setNowPostion((int)LocSys.GetResultLocationX(),
                                       (int)LocSys.GetResultLocationY(),
                                       (int)LocSys.GetResultAngle());

                // ルート計算
                avoidRTS.calcRooting();

                // 回避中をLEDで伝える
                //CarCtrl.SetHeadMarkLED((int)CersioCtrl.LED_PATTERN.WHITE_FLASH, false);
            }


            // DriveControl -------------------------------------------------------------------------------------------

            // マップ情報取得
            {
                Grid nowGrid = LocSys.GetMapInfo(LocSys.R1);

                // マップ情報反映
                if (nowGrid == Grid.BlueArea)
                {
                    // スローダウン
                    EBS.AccelSlowDownCnt = 5;
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

            // 動作モードごとの走行情報更新
            // ハンドル、アクセルを調整
            if (ModeCtrl.GetActionMode() == ModeControl.ActionMode.CheckPoint)
            {
                // 通常時

                // エマージェンシー ブレーキチェック
                EBS.EmgBrk = false;
                if (null != lrfData && lrfData.Length > 0)
                {
                    //int BrakeLv = CheckEBS(lrfData);
                    // ノイズ除去モーとで非常停止発動
                    int BrakeLv = EBS.CheckEBS(LocSys.LRF.getData_UntiNoise(), UpdateCnt);

                    // 注意Lvに応じた対処(スローダウン指示)
                    // Lvで段階ごとに分けてるのは、揺れなどによるLRFのノイズ対策
                    // 瞬間的なノイズでいちいち止まらないように。
                    if (BrakeLv >= EmergencyBrake.SlowDownLv) EBS.AccelSlowDownCnt = 20;
                    //if (BrakeLv >= EmergencyBrake.StopLv) EBS.EmgBrk = true;        // 緊急停止
                }


                // ルートにそったハンドル、アクセル値を取得
                double handleTgt = RTS.getHandleValue();
                double accTgt = RTS.getAccelValue();

                // 壁回避
                if(useEHS)
                {
                    EHS.HandleVal = EHS.CheckEHS(LocSys.LRF.getData_UntiNoise());

                    if (0.0 != EHS.HandleVal)
                    {
                        // 回避ハンドル操作　
                        // ばたつき防止
                        handleTgt = CersioCtrl.nowSendHandleValue + ((EHS.HandleVal - CersioCtrl.nowSendHandleValue) * CersioCtrl.HandleControlPow);

                        //AccelSlowDownCnt = 5;       // 速度もさげる
                        CarCtrl.SetHeadMarkLED((int)CersioCtrl.LED_PATTERN.BLUE);
                    }
                }

                // スローダウン中 動作
                if (EBS.AccelSlowDownCnt > 0)
                {
                    // 遅くする
                    accValue = accTgt = RTS.getAccelValue() * EmergencyBrake.AccSlowdownRate;
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

                handleValue = handleTgt;
                accValue = accTgt;
            }
            else if (ModeCtrl.GetActionMode() == ModeControl.ActionMode.EmergencyStop)
            {
                // 緊急ブレーキ 中
                if (null != lrfData && lrfData.Length > 0)
                {
                    int BrakeLv = EBS.CheckEBS(LocSys.LRF.getData_UntiNoise(), UpdateCnt);

                    // 停止状態の確認
                    if (BrakeLv >= EmergencyBrake.StopLv)
                    {
                        // 10秒経過したらバック動作へ
                        if (ModeCtrl.GetModePassSeconds(10))
                        {
                            ModeCtrl.SetActionMode(ModeControl.ActionMode.MoveBack);
                        }
                    }
                    else
                    {
                        // 緊急ブレーキ解除
                        ModeCtrl.SetActionMode(ModeControl.ActionMode.CheckPoint);
                    }
                }

                // 赤
                CarCtrl.SetHeadMarkLED((int)CersioCtrl.LED_PATTERN.RED, true);
                accValue = 0.0;
            }
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
