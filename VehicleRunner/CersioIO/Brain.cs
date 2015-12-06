
#define EBS_HANDLE_LINK  // ハンドルの向き追従　緊急ブレーキ

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LocationPresumption;
using SCIP_library;     // LRFライブラリ
using Axiom.Math;       // Vector3D計算ライブラリ


// ※補正　方法のON,OFFフラグ作る。ここにboolをおいて、フォームから操作

namespace CersioIO
{
    public class Brain
    {
        public CersioCtrl CarCtrl = null;
        // ルート制御
        public Rooting RTS = new Rooting();

        // ハンドル、アクセル操作
        static public double handleValue;
        static public double accValue;

        // エマージェンジーブレーキ
        static public bool EmgBrk = false;

        long EBS_hzUCNT = 0;
        public int EBS_CautionLv = 0;               // 注意Lv

        // スローダウン
        static private int AccelSlowDownCnt;

        //
        private long UpdateCnt;

        //
        public int checkpntIdx;


        // マッチングスコア
        public int MatchingScore = 0;


        // EBS 緊急ブレーキ
        public const double EBS_BrakeRange = 600.0;   // 0.8m  [mm]  // 緊急停止
        public const double EBS_SlowRange = 1700.0;   // 1.7m [mm]    // 徐行

        public const int EBS_stAng = -15;
        public const int EBS_edAng = 15;
        public double EBS_HandleDiffAngle = 0.0;

        // 
        public const int EBS_LvDownCnt = 3;         // 注意Lv1 引き下げ待ち時間x(100ms)
        public const int EBS_CautionLvMax = 5;     // 注意Lv 最大

        public const int EBS_SlowDownLv = 2;       // 注意Lv 徐行
        public const int EBS_StopLv = 5;           // 注意Lv 停止

        // スローダウン時のアクセル上限
        public const double AccSlowdownRate = 0.5;

#if false
        // 室内向け
        // EHS設定 壁検知　ハンドル制御
        // ハンドル　－が右
        public const int EHS_stLAng = -30;     // 左側感知角度 -35～-10度
        public const int EHS_edLAng = -10;

        public const int EHS_stRAng = 10;      // 右側感知角度 10～35度
        public const int EHS_edRAng = 30;

        public const double EHS_MinRange = 400.0;     // 感知最小距離 [mm] 30cm
        public const double EHS_MaxRange = 1000.0;    // 感知最大距離 [mm] 150cm

        const double EHS_WallRange = 450.0;   // 壁から離れる距離(LRFの位置から)[mm] 45cm (車体半分25cm+離れる距離20cm)
#else
        // EHS設定 壁検知　ハンドル制御
        // ハンドル　－が右
        public const int EHS_stLAng = -40;     // 左側感知角度 -35～-10度
        public const int EHS_edLAng = -10;

        public const int EHS_stRAng = 10;      // 右側感知角度 10～35度
        public const int EHS_edRAng = 40;

        public const double EHS_MinRange = 600.0;     // 感知最小距離 [mm] 30cm
        public const double EHS_MaxRange = 3000.0;    // 感知最大距離 [mm] 150cm

        const double EHS_WallRange = 600.0;   // 壁から離れる距離(LRFの位置から)[mm] 45cm (車体半分25cm+離れる距離20cm)
#endif
        public enum EHS_MODE
        {
            None = 0,       // 回避せず
            RightWallHit,   // 右壁を回避した
            LeftWallHit,    // 左壁を回避した
            CenterPass,     // 左右の壁があったので中央を通った
        };

        public static string[] EHS_ResultStr = {"None","RightWallHit","LeftWallHit","CenterPass"};

        // 回避結果
        public EHS_MODE EHS_Result = EHS_MODE.None;

        // 回避結果ハンドル
        public double EhsHandleVal;

        // 自己位置補正計算回数
        //private const int numParticleFilter = 10;

        // ログ 追記事項出力
        public static string addLogMsg;


        // 回復（バック）モード
        int cntAvoideMode = 0;
        public int EmgBrakeContinueCnt = 0;
        bool unUseAvoideMode = false;    // 回復モード使わないフラグ

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
        }

        public void Reset()
        {
            RTS.ResetSeq();
            checkpntIdx = 0;
            cntAvoideMode = 0;
        }

        // グリーン(補正要請)エリア侵入フラグ
        bool bGreenAreaFlg = false;

        /// <summary>
        /// 定期更新(100ms周期)
        /// </summary>
        /// <param name="LocSys"></param>
        /// <param name="useEBS">緊急ブレーキ</param>
        /// <param name="useEHS">壁避け動作</param>
        /// <param name="bLocRivisionTRG">位置補正執行</param>
        /// <param name="useAlwaysPF">常時PF更新</param>
        /// <returns></returns>
        public bool Update(LocPreSumpSystem LocSys, bool useEBS, bool useEHS, bool bLocRivisionTRG, bool useAlwaysPF)
        {
            int myPosX = (int)LocSys.GetResultLocationX();
            int myPosY = (int)LocSys.GetResultLocationY();
            double myAngle = (int)LocSys.GetResultAngle();

            double[] lrfData = LocSys.LRF_Data;

            // 自己位置更新
            {
                LocSys.Update(useAlwaysPF, CarCtrl.bhwREPlot );

                Grid nowGrid = LocSys.GetMapInfo(LocSys.R1);

                // 距離で補正 (PFの信頼がある場合)
                /*
                    // 3m以上離れたら補正
                    if (R1.GetDistance(V1) > (3000.0 / RealToMapSclae))
                    {
                        // 補正差分
                        double dfX = V1.X - R1.X;
                        double dfY = V1.Y - R1.Y;

                        R1.X += dfX;
                        R1.Y += dfY;
                    }
                */

                
                // 一定時間で更新
                if ((UpdateCnt % 30) == 0 && UpdateCnt != 0 && LocPreSumpSystem.bTimeRivision)
                {
                    bLocRivisionTRG = true;
                    Brain.addLogMsg += "LocRivision:Time\n";
                }

                // 強制補正フラグが立っている　または　壁の中にいる状態であれば、補正
                if (bLocRivisionTRG ||                                // 強制補正フラグが立っている
                    nowGrid == Grid.RedArea ||                        // 壁の中にいる
                    (nowGrid == Grid.GreenArea && !bGreenAreaFlg))    // 補正実行エリア
                {
                    if (nowGrid == Grid.RedArea)
                    {
                        // 強制補正エリアに入った
                        Brain.addLogMsg += "ColorMap:Red\n";
                    }
                    else if (nowGrid == Grid.GreenArea)
                    {
                        // 補正指示のエリアに入った
                        bGreenAreaFlg = true;
                        Brain.addLogMsg += "ColorMap:Green\n";
                    }

                    // 自己位置推定　計算実行
                    {
                        double locX = LocSys.R1.X;
                        double locY = LocSys.R1.Y;
                        double locDir = (LocSys.bActiveCompass ? LocSys.C1.Theta : LocSys.R1.Theta);    // コンパスが使えるなら、コンパスの値を使う

                        // GPSの値を使う
                        if (LocPreSumpSystem.bRivisonGPS)
                        {
                            // 信頼できる状態ならGPSを採用
                            locX = LocSys.G1.X;
                            locY = LocSys.G1.Y;
                        }

                        if (LocPreSumpSystem.bRivisonPF)
                        {
                            // PFを常時更新でなければ、現在位置から計算
                            if (!useAlwaysPF)
                            {
                                // 現在地から、パーティクルフィルターで補正
                                LocSys.V1.Set(locX, locY, locDir);
                                LocSys.CalcLocalize(LocSys.V1, false);
                            }
                        }
                        else
                        {
                            LocSys.V1.Set(locX,locY,locDir);
                        }
                    }

                    // 補正情報をログ出力
                    // 補正前の座標
                    Brain.addLogMsg += "LocRivision: FromR1 X" + LocSys.R1.X.ToString("f2") + "/Y " + LocSys.R1.Y.ToString("f2") + "/Dir " + LocSys.R1.Theta.ToString("f2")+"\n";
                    if (LocSys.bActiveCompass)
                    {
                        // コンパスを使った
                        Brain.addLogMsg += "LocRivision:useCompass C1 Dir " + LocSys.C1.Theta.ToString("f2") + "\n";
                    }
                    if (LocPreSumpSystem.bRivisonGPS)
                    {
                        // GPSを使った
                        Brain.addLogMsg += "LocRivision: useGPS G1 X" + LocSys.G1.X.ToString("f2") + "/Y " + LocSys.G1.Y.ToString("f2") + "/Dir " + LocSys.G1.Theta.ToString("f2") + "\n";
                    }
                    // 補正後の座標
                    if (LocPreSumpSystem.bRivisonPF)
                    {
                        Brain.addLogMsg += "LocRivision: usePF toR1 X" + LocSys.V1.X.ToString("f2") + "/Y " + LocSys.V1.Y.ToString("f2") + "/Dir " + LocSys.V1.Theta.ToString("f2") + "\n";
                    }
                    else
                    {
                        Brain.addLogMsg += "LocRivision: toR1 X" + LocSys.V1.X.ToString("f2") + "/Y " + LocSys.V1.Y.ToString("f2") + "/Dir " + LocSys.V1.Theta.ToString("f2") + "\n";
                    }

                    // 結果を反映
                    LocSys.R1.Set(LocSys.V1);
                    LocSys.E1.Set(LocSys.V1);
                    LocSys.oldE1.Set(LocSys.V1);

                    // REリセット
                    CarCtrl.RE_Reset(LocSys.E1.X * LocSys.RealToMapSclae,
                                     LocSys.E1.Y * LocSys.RealToMapSclae,
                                     LocSys.E1.Theta);
                }



                // マップ情報反映
                // スローダウン
                if (LocSys.GetMapInfo(LocSys.R1) == Grid.BlueArea)
                {
                    AccelSlowDownCnt = 5;
                    Brain.addLogMsg += "ColorMap:Blue\n";
                }
                if (LocSys.GetMapInfo(LocSys.R1) != Grid.GreenArea )
                {
                    bGreenAreaFlg = false;
                }
            }

            // マッチングスコア計算
            CalcMatching(LocSys);

            // 現在座標更新
            RTS.setNowPostion(myPosX, myPosY, myAngle);

            // ルート計算
            RTS.calcRooting();


            // エマージェンシー ブレーキチェック
            EmgBrk = false;
            if (null != lrfData && lrfData.Length > 0)
            {
                //int BrakeLv = CheckEBS(lrfData);
                int BrakeLv = CheckEBS(LocSys.LRF_UntiNoiseData);  // ノイズ除去モーとで非常停止発動

                // 注意Lvに応じた対処(スローダウン指示)
                // Lvで段階ごとに分けてるのは、揺れなどによるLRFのノイズ対策
                // 瞬間的なノイズでいちいち止まらないように。
                if (BrakeLv >= EBS_SlowDownLv) AccelSlowDownCnt = 20;
                if (BrakeLv >= EBS_StopLv) EmgBrk = true;        // 緊急停止
            }


            // ハンドル、アクセルを調整
            if (cntAvoideMode == 0 || unUseAvoideMode )
            {
                // ルートにそったハンドル、アクセル値を取得
                double handleTgt = RTS.getHandleValue();
                double accTgt = RTS.getAccelValue();

                // 壁回避
                {
                    EhsHandleVal = CheckEHS(LocSys.LRF_UntiNoiseData);

                    if (useEHS && 0.0 != EhsHandleVal)
                    {
                        // 回避ハンドル操作　
                        // ばたつき防止
                        handleTgt = CersioCtrl.nowSendHandleValue + (EhsHandleVal - CersioCtrl.nowSendHandleValue) * ((CersioCtrl.HandleControlPow)*0.5);

                        //AccelSlowDownCnt = 5;       // 速度もさげる
                        CarCtrl.SetHeadMarkLED((int)CersioCtrl.LED_PATTERN.BLUE);
                    }
                }

                // スローダウン
                if (AccelSlowDownCnt > 0)
                {
                    accValue = accTgt = RTS.getAccelValue() * AccSlowdownRate;
                    AccelSlowDownCnt--;

                    // ＬＥＤパターンハザード
                    // スローダウンを知らせる
                    CarCtrl.SetHeadMarkLED((int)CersioCtrl.LED_PATTERN.HAZERD);
                }
                else
                {
                    // ＬＥＤパターン平常
                    CarCtrl.SetHeadMarkLED((int)CersioCtrl.LED_PATTERN.Normal);
                }


                if (EmgBrk)
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

                {
                    double ehsHandleVal = CheckEHS(LocSys.LRF_UntiNoiseData);

                    if (useEHS && 0.0 != ehsHandleVal)
                    {
                        // 左右に壁を検知してたら、優先的によける
                        if( EHS_Result == EHS_MODE.LeftWallHit || EHS_Result == EHS_MODE.RightWallHit )
                        {
                            handleTgt = ehsHandleVal;
                        }
                    }
                }

                // ハンドルを逆に切り、バック
                handleValue = -handleTgt;
                accValue = -AccSlowdownRate;

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


        /// <summary>
        /// 緊急ブレーキ判定
        /// </summary>
        /// <param name="lrfData">270度＝個のデータ</param>
        /// <returns>ブレーキLv 0..問題なし 9...非常停止</returns>
        public int CheckEBS(double[] lrfData)
        {
            int rangeAngHalf = 270 / 2; // 中央(角度)
            int stAng = EBS_stAng;
            int edAng = EBS_edAng;

            bool bCauntion = false;
            bool bStop = false;

            // 時間経過で注意Lv引き下げ
            if ((UpdateCnt - EBS_hzUCNT) > EBS_LvDownCnt)
            {
                EBS_hzUCNT = UpdateCnt;
                if (EBS_CautionLv > 0) EBS_CautionLv--;
            }

            // ハンドル値によって、角度をずらす
#if EBS_HANDLE_LINK
            {
                // ハンドル切れ角３０度　0.4は角度を控えめに40%
                int diffDir = (int)((-handleValue * 30.0) * 0.3);

                stAng += diffDir;
                edAng += diffDir;

                EBS_HandleDiffAngle = diffDir;
            }
#endif

            // LRFの値を調べる
            for (int i = (stAng + rangeAngHalf); i < (edAng + rangeAngHalf); i++)
            {
                // 以下ならとまる
                if (lrfData[i] < (EBS_BrakeRange * URG_LRF.getScale()))
                {
                    bStop = true;
                    break;
                }
                if (lrfData[i] < (EBS_SlowRange * URG_LRF.getScale())) bCauntion = true;
            }

            // 注意Lv引き上げ
            if (bStop)
            {
                EBS_CautionLv = EBS_CautionLvMax;
                EBS_hzUCNT = UpdateCnt;
            }
            else if (bCauntion && EBS_CautionLv < (EBS_StopLv - 1) )
            {
                // 徐行の最大まで(注意状態だけなら止まりはしない)
                EBS_CautionLv++;
                EBS_hzUCNT = UpdateCnt;
            }

            return EBS_CautionLv;
        }


        /// <summary>
        /// 緊急時ハンドルコントロール
        /// </summary>
        /// <param name="lrfData">270度＝個数のデータを想定</param>
        /// <returns></returns>
        public double CheckEHS(double[] lrfData)
        {
            double rtHandleVal = 0.0;
            EHS_Result = EHS_MODE.None;


            // 指定の扇状角度の指定の距離範囲内に、障害物があればハンドルを切る。
            {
                double lrfScale = URG_LRF.getScale();   // mm -> Pixelスケール変換値
                const int rangeCenterAng = 270 / 2; // 中央(角度)

                double LHitLength = 0.0;
                double LHitDir = 0.0;
                double RHitLength = 0.0;
                double RHitDir = 0.0;

                double minPixRange = (EHS_MinRange * lrfScale);
                double maxPixRange = (EHS_MaxRange * lrfScale);
                double minDistance;

                // LRFの値を調べる

                // 左側
                minDistance = maxPixRange;
                for (int i = (EHS_stLAng + rangeCenterAng); i < (EHS_edLAng + rangeCenterAng); i++)
                {
                    // 範囲内なら反応
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

                // 右側
                minDistance = maxPixRange;
                for (int i = (EHS_edRAng + rangeCenterAng); i >= (EHS_stRAng + rangeCenterAng); i--)
                {
                    // 以下なら反応
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

                    
                    double tgtAng = Rooting.CalcPositionToAngle((Lx-Rx)*0.5,(Ly-Ry)*0.5);
                    rtHandleVal = Rooting.CalcHandleValueFromAng(tgtAng, 0.0);

                    EHS_Result = EHS_MODE.CenterPass;
                }
                else if (LHitLength != 0.0 )
                {
                    // 左側がHit
                    double Lx, Ly;

                    Lx = Ly = 0.0;
                    LrfValToPosition(ref Lx, ref Ly, LHitDir, LHitLength);

                    Lx += EHS_WallRange;

                    double tgtAng = Rooting.CalcPositionToAngle( Lx, Ly);
                    rtHandleVal = Rooting.CalcHandleValueFromAng(tgtAng, 0.0);
                    EHS_Result = EHS_MODE.LeftWallHit;
                }
                else if (RHitLength != 0.0)
                {
                    // 右側がHit
                    double Rx, Ry;

                    Rx = Ry = 0.0;
                    LrfValToPosition(ref Rx, ref Ry, RHitDir, RHitLength);

                    Rx -= EHS_WallRange;

                    double tgtAng = Rooting.CalcPositionToAngle(Rx, Ry);
                    rtHandleVal = Rooting.CalcHandleValueFromAng(tgtAng, 0.0);
                    EHS_Result = EHS_MODE.RightWallHit;
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
        private void LrfValToPosition(ref double x, ref double y, double lrfDir, double lrfLength )
        {
            int rangeCenterAng = 270 / 2; // 中央(角度)

            double val = lrfLength;
            double rad = (lrfDir - rangeCenterAng - 90) * Math.PI / 180.0;

            x = lrfLength * Math.Cos(rad);
            y = lrfLength * Math.Sin(rad);
        }

        /// <summary>
        /// マップマッチングスコア 計算
        /// </summary>
        /// <param name="LocSys"></param>
        private void CalcMatching(LocPreSumpSystem LocSys)
        {
            double[] lrfData = LocSys.LRF_Data;

            // ロータリーエンコーダの位置のマップ情報をLRFデータとして取得
            double[] mapLrfData = LocSys.GetMapLRF(LocSys.E1);

            int num = lrfData.Length / 10;
            double[] LvAvgLRF = new double[10 + 1];
            double[] LvAvgMRF = new double[10 + 1];
            int Score = 0;

            // 角度を10段階に分けて、距離の平均を求める
            for (int i = 0; i < lrfData.Length; i++)
            {
                LvAvgLRF[i / num] += lrfData[i];
                LvAvgMRF[i / num] += mapLrfData[i];
            }

            for (int i = 0; i < 10; i++)
            {
                LvAvgLRF[i] /= num;
                LvAvgMRF[i] /= num;

                // 距離の平均の差分が　100mm以下ならスコアを加算
                if (Math.Abs(LvAvgLRF[i] - LvAvgMRF[i]) < 100.0)
                {
                    Score += 10;
                }
            }

            // スコアを判定
            MatchingScore = Score;
        }
    }
}
