
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


namespace Navigation
{
    /// <summary>
    /// セルシオ制御　頭脳クラス
    /// </summary>
    public class Brain
    {
        // セルシオ制御クラス
        public CersioCtrl CarCtrl = null;

        // ルート制御
        public Rooting RTS = new Rooting();

        // ハンドル、アクセル操作
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

        #region EBS 緊急ブレーキ クラス定義
        /// <summary>
        /// EBS 緊急ブレーキ
        /// </summary>
        public class EmergencyBrake
        {
            /// <summary>
            /// 緊急停止レンジ  [mm]
            /// </summary>
            public const double BrakeRange = 600.0;

            /// <summary>
            /// 注意徐行レンジ [mm]
            /// </summary>
            public const double SlowRange = 1700.0;

            public const int stAng = -15;
            public const int edAng = 15;

            /// <summary>
            /// 注意Lv1 引き下げ待ち時間x(100ms)
            /// </summary>
            public const int LvDownCnt = 3;         
            /// <summary>
            /// 注意Lv 最大
            /// </summary>
            public const int CautionLvMax = 5;

            /// <summary>
            /// 注意Lv 徐行
            /// </summary>
            public const int SlowDownLv = 2;
            /// <summary>
            /// 注意Lv 停止
            /// </summary>
            public const int StopLv = 5;

            /// <summary>
            /// スローダウン時のアクセル上限
            /// </summary>
            public const double AccSlowdownRate = 0.5;


            /// <summary>
            /// エマージェンジーブレーキ発動　フラグ
            /// </summary>
            public bool EmgBrk = false;

            /// <summary>
            /// 
            /// </summary>
            public long hzUCNT = 0;

            /// <summary>
            /// 注意Lv
            /// </summary>
            public int CautionLv = 0; 

            /// <summary>
            /// ハンドルから向かい先の角度調整
            /// </summary>
            public double HandleDiffAngle = 0.0;

            /// <summary>
            /// スローダウンカウント
            /// </summary>
            public int AccelSlowDownCnt;


            /// <summary>
            /// 緊急ブレーキ判定
            /// </summary>
            /// <param name="lrfData">270度＝個のデータ</param>
            /// <returns>ブレーキLv 0..問題なし 9...非常停止</returns>
            public int CheckEBS(double[] lrfData, long UpdateCnt )
            {
                int rangeAngHalf = 270 / 2; // 中央(角度)
                int stAng = EmergencyBrake.stAng;
                int edAng = EmergencyBrake.edAng;

                bool bCauntion = false;
                bool bStop = false;

                // 時間経過で注意Lv引き下げ
                if ((UpdateCnt - hzUCNT) > LvDownCnt)
                {
                    hzUCNT = UpdateCnt;
                    if (CautionLv > 0) CautionLv--;
                }

                // ハンドル値によって、角度をずらす
#if EBS_HANDLE_LINK
                {
                    // ハンドル切れ角３０度　0.4は角度を控えめに40%
                    int diffDir = (int)((-handleValue * 30.0) * 0.3);

                    stAng += diffDir;
                    edAng += diffDir;

                    HandleDiffAngle = diffDir;
                }
#endif

                // LRFの値を調べる
                for (int i = (stAng + rangeAngHalf); i < (edAng + rangeAngHalf); i++)
                {
                    // 以下ならとまる
                    if (lrfData[i] < (BrakeRange * URG_LRF.getScale()))
                    {
                        bStop = true;
                        break;
                    }
                    if (lrfData[i] < (SlowRange * URG_LRF.getScale())) bCauntion = true;
                }

                // 注意Lv引き上げ
                if (bStop)
                {
                    CautionLv = CautionLvMax;
                    hzUCNT = UpdateCnt;
                }
                else if (bCauntion && CautionLv < (StopLv - 1))
                {
                    // 徐行の最大まで(注意状態だけなら止まりはしない)
                    CautionLv++;
                    hzUCNT = UpdateCnt;
                }

                return CautionLv;
            }
        }
        #endregion

        /// <summary>
        /// EBS 緊急ブレーキ
        /// </summary>
        public EmergencyBrake EBS = new EmergencyBrake();

        /// <summary>
        /// ブレーキ継続カウンタ
        /// </summary>
        public int EmgBrakeContinueCnt = 0;

        #region 緊急時ハンドル　クラス定義
        public class EmergencyHandring
        {
            // 室内向け
            // EHS設定 壁検知　ハンドル制御
            // ハンドル　－が右
            public const int ind_stLAng = -30;     // 左側感知角度 -35～-10度
            public const int ind_edLAng = -10;

            public const int ind_stRAng = 10;      // 右側感知角度 10～35度
            public const int ind_edRAng = 30;

            public const double ind_MinRange = 400.0;     // 感知最小距離 [mm] 30cm
            public const double ind_MaxRange = 1000.0;    // 感知最大距離 [mm] 150cm

            public const double ind_WallRange = 450.0;   // 壁から離れる距離(LRFの位置から)[mm] 45cm (車体半分25cm+離れる距離20cm)

            // EHS設定 壁検知　ハンドル制御
            // ハンドル　－が右
            public const int od_stRAng = -40;     // 右側感知角度 -35～-10度
            public const int od_edRAng = -10;

            public const int od_stLAng = 10;      // 左側感知角度 10～35度
            public const int od_edLAng = 40;

            public const double od_MinRange = 600.0;     // 感知最小距離 [mm] 30cm
            public const double od_MaxRange = 3000.0;    // 感知最大距離 [mm] 150cm

            public const double od_WallRange = 600.0;   // 壁から離れる距離(LRFの位置から)[mm] 45cm (車体半分25cm+離れる距離20cm)
            
            // EHS設定 壁検知　ハンドル制御
            // ハンドル　－が右
            public static int stRAng = -40;     // 右側感知角度 -35～-10度
            public static int edRAng = -10;

            public static int stLAng = 10;      // 左側感知角度 10～35度
            public static int edLAng = 40;

            public static double MinRange = 600.0;     // 感知最小距離 [mm] 30cm
            public static double MaxRange = 3000.0;    // 感知最大距離 [mm] 150cm

            public static double WallRange = 600.0;   // 壁から離れる距離(LRFの位置から)[mm] 45cm (車体半分25cm+離れる距離20cm)


            public enum EHS_MODE
            {
                None = 0,       // 回避せず
                RightWallHit,   // 右壁を回避した
                LeftWallHit,    // 左壁を回避した
                CenterPass,     // 左右の壁があったので中央を通った
            }

            public static string[] ResultStr = { "None", "RightWallHit", "LeftWallHit", "CenterPass" };

            // 回避結果
            public EHS_MODE Result = EHS_MODE.None;

            // 回避結果ハンドル
            public double HandleVal;


            /// <summary>
            /// 緊急時ハンドルコントロール
            /// </summary>
            /// <param name="lrfData">270度＝個数のデータを想定</param>
            /// <returns></returns>
            public double CheckEHS(double[] lrfData)
            {
                double rtHandleVal = 0.0;
                Result = EHS_MODE.None;


                // 指定の扇状角度の指定の距離範囲内に、障害物があればハンドルを切る。
                {
                    double lrfScale = URG_LRF.getScale();   // mm -> Pixelスケール変換値
                    const int rangeCenterAng = 270 / 2; // 中央(角度)

                    double LHitLength = 0.0;
                    double LHitDir = 0.0;
                    double RHitLength = 0.0;
                    double RHitDir = 0.0;

                    double minPixRange = (MinRange * lrfScale);
                    double maxPixRange = (MaxRange * lrfScale);
                    double minDistance;

                    // LRFの値を調べる

                    // 右側
                    minDistance = maxPixRange;
                    for (int i = (stRAng + rangeCenterAng); i < (edRAng + rangeCenterAng); i++)
                    {
                        // 範囲内なら反応
                        if (lrfData[i] > minPixRange && lrfData[i] < maxPixRange)
                        {
                            if (lrfData[i] < minDistance)
                            {
                                minDistance = lrfData[i];
                                RHitLength = lrfData[i];
                                RHitDir = (double)i;
                            }
                        }
                    }

                    // 左側
                    minDistance = maxPixRange;
                    for (int i = (edLAng + rangeCenterAng); i >= (stLAng + rangeCenterAng); i--)
                    {
                        // 以下なら反応
                        if (lrfData[i] > minPixRange && lrfData[i] < maxPixRange)
                        {
                            if (lrfData[i] < minDistance)
                            {
                                minDistance = lrfData[i];
                                LHitLength = lrfData[i];
                                LHitDir = (double)i;
                            }
                        }
                    }

                    // ハンドル角を計算
                    if (LHitLength != 0.0 && RHitLength != 0.0)
                    {
                        // 左右がHIT
                        double Lx, Ly;
                        double Rx, Ry;

                        Lx = Ly = 0.0;
                        LrfValToPosition(ref Lx, ref Ly, LHitDir, LHitLength);

                        Rx = Ry = 0.0;
                        LrfValToPosition(ref Rx, ref Ry, RHitDir, RHitLength);


                        double tgtAng = Rooting.CalcPositionToAngle((Lx - Rx) * 0.5, (Ly - Ry) * 0.5);
                        rtHandleVal = Rooting.CalcHandleValueFromAng(tgtAng, 0.0);

                        Result = EHS_MODE.CenterPass;
                    }
                    else if (LHitLength != 0.0)
                    {
                        // 左側がHit
                        double Lx, Ly;

                        Lx = Ly = 0.0;
                        LrfValToPosition(ref Lx, ref Ly, LHitDir, LHitLength);

                        Lx += WallRange;

                        double tgtAng = Rooting.CalcPositionToAngle(Lx, Ly);
                        rtHandleVal = Rooting.CalcHandleValueFromAng(tgtAng, 0.0);
                        Result = EHS_MODE.LeftWallHit;
                    }
                    else if (RHitLength != 0.0)
                    {
                        // 右側がHit
                        double Rx, Ry;

                        Rx = Ry = 0.0;
                        LrfValToPosition(ref Rx, ref Ry, RHitDir, RHitLength);

                        Rx -= WallRange;

                        double tgtAng = Rooting.CalcPositionToAngle(Rx, Ry);
                        rtHandleVal = Rooting.CalcHandleValueFromAng(tgtAng, 0.0);
                        Result = EHS_MODE.RightWallHit;
                    }
                }

                // 極小値は切り捨て
                rtHandleVal = (double)((int)(rtHandleVal * 100.0)) / 100.0;

                return rtHandleVal;
            }

            /// <summary>
            /// LRF上の角度と距離を　座標に変換
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="lrfDir"></param>
            /// <param name="lrfLength"></param>
            private void LrfValToPosition(ref double x, ref double y, double lrfDir, double lrfLength)
            {
                int rangeCenterAng = 270 / 2; // 中央(角度)

                double val = lrfLength;
                double rad = (lrfDir - rangeCenterAng - 90) * Math.PI / 180.0;

                x = lrfLength * Math.Cos(rad);
                y = lrfLength * Math.Sin(rad);
            }

        }

        #endregion

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

        // ===========================================================================================================

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ceCtrl"></param>
        public Brain(CersioCtrl ceCtrl)
        {
            CarCtrl = ceCtrl;
            UpdateCnt = 0;

            handleValue = 0.0;
            accValue = 0.0;
            goalFlg = false;
        }

        public void Reset()
        {
            RTS.ResetSeq();
            checkpntIdx = 0;
            cntAvoideMode = 0;
            goalFlg = false;
        }

        // グリーン(補正要請)エリア侵入フラグ
        bool bGreenAreaFlg = false;


        /// <summary>
        /// 自律走行処理 定期更新
        /// </summary>
        /// <param name="LocSys"></param>
        /// <param name="useEBS">壁回避ブレーキ</param>
        /// <param name="useEHS">壁回避ハンドル</param>
        /// <param name="bStraightMode">直進モード</param>
        /// <param name="bIndoorMode">屋内モード</param>
        /// <returns></returns>
        public bool AutonomousProc(LocPreSumpSystem LocSys, bool useEBS, bool useEHS, bool bStraightMode, bool bIndoorMode)
        {
            // エマージェンシーブレーキを使わないフラグ
            bool untiEBS = false;

            if( bIndoorMode )
            {
                // 屋内用ハンドル操作
                EmergencyHandring.stRAng = EmergencyHandring.ind_stLAng;     // 右側感知角度 -35～-10度
                EmergencyHandring.edRAng = EmergencyHandring.ind_stLAng;

                EmergencyHandring.stLAng = EmergencyHandring.ind_stRAng;      // 左側感知角度 10～35度
                EmergencyHandring.edLAng = EmergencyHandring.ind_edRAng;

                EmergencyHandring.MinRange = EmergencyHandring.ind_MinRange;     // 感知最小距離 [mm] 30cm
                EmergencyHandring.MaxRange = EmergencyHandring.ind_MaxRange;    // 感知最大距離 [mm] 150cm

                EmergencyHandring.WallRange = EmergencyHandring.ind_WallRange;   // 壁から離れる距離(LRFの位置から)[mm] 45cm (車体半分25cm+離れる距離20cm)
            }
            else
            {
                // 屋外用ハンドル操作
                EmergencyHandring.stRAng = EmergencyHandring.od_stRAng;     // 右側感知角度 -35～-10度
                EmergencyHandring.edRAng = EmergencyHandring.od_edRAng;

                EmergencyHandring.stLAng = EmergencyHandring.od_stLAng;      // 左側感知角度 10～35度
                EmergencyHandring.edLAng = EmergencyHandring.od_edLAng;

                EmergencyHandring.MinRange = EmergencyHandring.od_MinRange;     // 感知最小距離 [mm] 30cm
                EmergencyHandring.MaxRange = EmergencyHandring.od_MaxRange;    // 感知最大距離 [mm] 150cm

                EmergencyHandring.WallRange = EmergencyHandring.od_WallRange;   // 壁から離れる距離(LRFの位置から)[mm] 45cm (車体半分25cm+離れる距離20cm)

            }

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

                // 直進モード
                if (bStraightMode) handleTgt = 0.0;

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
