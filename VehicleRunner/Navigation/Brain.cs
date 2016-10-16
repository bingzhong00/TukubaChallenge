
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


        /// <summary>
        /// マップファイル
        /// </summary>
        public MapData MapFile;

        /// <summary>
        /// ルート制御
        /// </summary>
        public Rooting RTS;

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
        /// チェックポイント通過　インデックス
        /// </summary>
        public int checkpntIdx;

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


        // 回復（バック）モード
        int cntAvoideMode = 0;
        bool unUseAvoideMode = false;    // 回復モード使わないフラグ

        // バック動作中フラグ
        public bool bNowBackProcess = false;

        /// <summary>
        /// 回避動作開始　トリガ
        /// </summary>
        private bool bActiveDetourTRG = false;

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
            // 
            MapFile = MapData.LoadMapFile(mapFileName);
            // 
            RTS = new Rooting(MapFile);

            //  マップ画像ファイル名、実サイズの横[mm], 実サイズ縦[mm] (北向き基準)
            LocSys = new LocPreSumpSystem(MapFile.MapImageFileName, MapFile.RealWidth, MapFile.RealHeight);

            UpdateCnt = 0;

            checkpntIdx = 0;
            cntAvoideMode = 0;
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
        /// <param name="bStraightMode">直進モード</param>
        /// <param name="bIndoorMode">屋内モード</param>
        /// <param name="bCtrlOutput">CarCtrlに動作出力</param>
        /// <returns></returns>
        public bool AutonomousProc( bool useEBS, bool useEHS, bool bStraightMode, bool bIndoorMode, bool bCtrlOutput )
        {
            // エマージェンシーブレーキを使わないフラグ
            bool untiEBS = false;

            // ハンドルレンジ設定
            EHS.SetSensorRange(bIndoorMode);

            // すべてのルートを回りゴールした。
            if (goalFlg)
            {
                // 停止情報送信
                CarCtrl.SetCommandAC(0.0, 0.0);
                // スマイル
                CarCtrl.SetHeadMarkLED((int)CersioCtrl.LED_PATTERN.SMILE);

                return goalFlg;
            }


            // 自走処理
            bNowBackProcess = untiEBS = Update(LocSys, useEBS, useEHS, bStraightMode);

            if (EBS.EmgBrk && useEBS && !untiEBS)
            {
                // 強制停止状態
                // エマージェンシー ブレーキ
                //handleValue = 0.0;
                CersioCtrl.nowSendAccValue = 0.0;

                // 停止情報送信
                CarCtrl.SetCommandAC(0.0, 0.0);

                // 赤
                CarCtrl.SetHeadMarkLED((int)CersioCtrl.LED_PATTERN.RED, true);
            }
            else
            {
                // 走行可能状態
                if (bCtrlOutput)
                {
                    // ルート計算から、目標ハンドル、目標アクセル値をもらう。
                    CarCtrl.CalcHandleAccelControl(getHandleValue(), getAccelValue());

                    // 送信
                    CarCtrl.SetCommandAC();
                }
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
        public bool Update(LocPreSumpSystem LocSys, bool useEBS, bool useEHS, bool bStraightMode)
        {
            // LRFデータ取得
            double[] lrfData = LocSys.LRF.getData();

            // Rooting ------------------------------------------------------------------------------------------------
            // ルート算定
            {
                // 現在座標更新
                RTS.setNowPostion((int)LocSys.GetResultLocationX(),
                                  (int)LocSys.GetResultLocationY(),
                                  (int)LocSys.GetResultAngle() );

                // ルート計算
                RTS.calcRooting();
            }


            // DriveControl -------------------------------------------------------------------------------------------

            // マップ情報取得
            {
                Grid nowGrid = LocSys.GetMapInfo(LocSys.R1);

                // マップ情報反映
                // スローダウン
                if (nowGrid == Grid.BlueArea)
                {
                    EBS.AccelSlowDownCnt = 5;
                    Brain.addLogMsg += "ColorMap:Blue\n";
                }

                if (nowGrid == Grid.RedArea )                       // 壁の中にいる
                {
                    // 強制補正エリア
                    Brain.addLogMsg += "LocRivision:ColorMap[Red](マップ情報の位置補正)\n";
                    bRevisionRequest = true;
                }

                if(nowGrid == Grid.GreenArea && !bGreenAreaFlg)    // 補正実行エリア
                {
                    // 補正指示のエリアに入った
                    Brain.addLogMsg += "LocRivision:ColorMap[Green](マップ情報の位置補正)\n";
                    bGreenAreaFlg = true;

                    bRevisionRequest = true;
                }

                // 緑を抜けた判定
                if (nowGrid != Grid.GreenArea)
                {
                    bGreenAreaFlg = false;
                }
            }

            // エマージェンシー ブレーキチェック
            EBS.EmgBrk = false;
            if (null != lrfData && lrfData.Length > 0)
            {
                //int BrakeLv = CheckEBS(lrfData);
                // ノイズ除去モーとで非常停止発動
                int BrakeLv = EBS.CheckEBS(LocSys.LRF.getData_UntiNoise(), UpdateCnt );

                // 注意Lvに応じた対処(スローダウン指示)
                // Lvで段階ごとに分けてるのは、揺れなどによるLRFのノイズ対策
                // 瞬間的なノイズでいちいち止まらないように。
                if (BrakeLv >= EmergencyBrake.SlowDownLv) EBS.AccelSlowDownCnt = 20;
                if (BrakeLv >= EmergencyBrake.StopLv) EBS.EmgBrk = true;        // 緊急停止
            }


            // ハンドル、アクセルを調整
            if (cntAvoideMode == 0 || unUseAvoideMode)
            {
                // ルートにそったハンドル、アクセル値を取得
                double handleTgt = RTS.getHandleValue();
                double accTgt = RTS.getAccelValue();

                // 直進モード
                if (bStraightMode) handleTgt = 0.0;

                // 壁回避
                {
                    EHS.HandleVal = EHS.CheckEHS(LocSys.LRF.getData_UntiNoise());

                    if (useEHS && 0.0 != EHS.HandleVal)
                    {
                        // 回避ハンドル操作　
                        // ばたつき防止
                        handleTgt = CersioCtrl.nowSendHandleValue + (EHS.HandleVal - CersioCtrl.nowSendHandleValue) * ((CersioCtrl.HandleControlPow) * 0.5);

                        //AccelSlowDownCnt = 5;       // 速度もさげる
                        CarCtrl.SetHeadMarkLED((int)CersioCtrl.LED_PATTERN.BLUE);
                    }
                }

                // スローダウン
                if (EBS.AccelSlowDownCnt > 0)
                {
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
                if (EBS.EmgBrk)
                {
                    EmgBrakeContinueCnt++;

                    if (EmgBrakeContinueCnt > 10 * 10)
                    {
                        // １０秒たったら、バックモードへ
                        cntAvoideMode = 40;
                        bActiveDetourTRG = true;
                    }
                }
                else
                {
                    EmgBrakeContinueCnt = 0;
                }

                handleValue = handleTgt;
                accValue = accTgt;
            }
            else
            {
                // 復帰（バック）モード
                double handleTgt = RTS.getHandleValue();
                //double accTgt = RTS.getAccelValue();

                if (bActiveDetourTRG)
                {
                    // ※
                    //ActiveDetour actDetour = new ActiveDetour();
                    bActiveDetourTRG = false;
                }


                // 直進モード
                if (bStraightMode) handleTgt = 0.0;

                // 障害物をみて、ハンドルを切る方向を求める
                {
                    double ehsHandleVal = EHS.CheckEHS(LocSys.LRF.getData_UntiNoise());

                    if (useEHS && 0.0 != ehsHandleVal)
                    {
                        // 左右に壁を検知してたら、優先的によける
                        if (EHS.Result == EmergencyHandring.EHS_MODE.LeftWallHit ||
                            EHS.Result == EmergencyHandring.EHS_MODE.RightWallHit)
                        {
                            handleTgt = ehsHandleVal;
                        }
                    }
                }

                // ハンドルを逆に切り、バック
                handleValue = -handleTgt;
                accValue = -EmergencyBrake.AccSlowdownRate;

                // ＬＥＤパターン
                CarCtrl.SetHeadMarkLED((int)CersioCtrl.LED_PATTERN.UnKnown1, true);

                cntAvoideMode--;
            }

            // チェックポイント通過をLEDで伝える
            {
                int nowIdx = RTS.GetNowCheckPointIdx();
                if (checkpntIdx != nowIdx)
                {
                    CarCtrl.SetHeadMarkLED((int)CersioCtrl.LED_PATTERN.WHITE_FLASH, true);
                    checkpntIdx = nowIdx;
                }
            }

            UpdateCnt++;

            // 回復モードか否か
            if (cntAvoideMode == 0 || unUseAvoideMode) return false;
            return true;
        }


        /// <summary>
        /// ブレインの結果　ハンドリング、アクセル情報
        /// </summary>
        /// <returns></returns>
        public double getHandleValue()
        {
            return handleValue;
        }

        public double getAccelValue()
        {
            return accValue;
        }
    }
}
