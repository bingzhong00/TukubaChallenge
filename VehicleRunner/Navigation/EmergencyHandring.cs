using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LocationPresumption;
using CersioIO;
using SCIP_library;     // LRFライブラリ

namespace Navigation
{
    /// <summary>
    /// 緊急ハンドル制御クラス
    /// </summary>
    public class EmergencyHandring
    {
        // 室内向け
        // EHS設定 壁検知　ハンドル制御
        // ハンドル　－が右
        public const int ind_stLAng = -30;     // 左側感知角度 -30～-10度
        public const int ind_edLAng = -10;

        public const int ind_stRAng = 10;      // 右側感知角度 10～30度
        public const int ind_edRAng = 30;

        public const double ind_MinRange = 400.0;     // 感知最小距離 [mm] 40cm
        public const double ind_MaxRange = 1000.0;    // 感知最大距離 [mm] 100cm

        public const double ind_WallRange = 450.0;   // 壁から離れる距離(LRFの位置から)[mm] 45cm (車体半分25cm+離れる距離20cm)

        // 屋外向け
        // EHS設定 壁検知　ハンドル制御
        // ハンドル　－が右
        public const int od_stRAng = -40;     // 右側感知角度 -40～-10度
        public const int od_edRAng = -10;

        public const int od_stLAng = 10;      // 左側感知角度 10～40度
        public const int od_edLAng = 40;

        public const double od_MinRange = 600.0;     // 感知最小距離 [mm] 60cm
        public const double od_MaxRange = 3000.0;    // 感知最大距離 [mm] 3000cm

        public const double od_WallRange = 600.0;   // 壁から離れる距離(LRFの位置から)[mm] 60cm (車体半分25cm+離れる距離35cm)

        // EHS設定 壁検知　ハンドル制御
        // ハンドル　－が右
        public static int stRAng;     // 右側感知角度 -35～-10度
        public static int edRAng;

        public static int stLAng;      // 左側感知角度 10～35度
        public static int edLAng;

        public static double MinRange;    // 感知最小距離 [mm]
        public static double MaxRange;    // 感知最大距離 [mm]

        public static double WallRange;   // 壁から離れる距離(LRFの位置から)[mm] 45cm (車体半分25cm+離れる距離20cm)


        public enum EHS_MODE
        {
            None = 0,       // 回避せず
            RightWallHit,   // 右壁を回避した
            LeftWallHit,    // 左壁を回避した
            CenterPass,     // 左右の壁があったので中央を通った
        }

        /// <summary>
        /// 感知した壁
        /// </summary>
        public static string[] ResultStr = { "None", "RightWallHit", "LeftWallHit", "CenterPass" };

        // 回避結果
        public EHS_MODE Result = EHS_MODE.None;

        // 回避結果ハンドル
        public double HandleVal;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EmergencyHandring()
        {
            // 初期センサー値設定
            SetSensorRange(false);
        }

        /// <summary>
        /// センサー値
        /// </summary>
        /// <param name="bIndoorMode">true..屋内モード / false...屋外モード</param>
        public void SetSensorRange(bool bIndoorMode)
        {
            if (bIndoorMode)
            {
                // 屋内用ハンドル操作
                EmergencyHandring.stRAng = EmergencyHandring.ind_stRAng;     // 右側感知角度 -35～-10度
                EmergencyHandring.edRAng = EmergencyHandring.ind_stRAng;

                EmergencyHandring.stLAng = EmergencyHandring.ind_stLAng;      // 左側感知角度 10～35度
                EmergencyHandring.edLAng = EmergencyHandring.ind_edLAng;

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
        }

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
                double lrfScale = URG_LRF.getScale();   // スケール変換値
                const int rangeCenterAng = 270 / 2; // 中央(角度)

                double LHitLength = 0.0;
                double LHitDir = 0.0;
                double RHitLength = 0.0;
                double RHitDir = 0.0;

                double minScaledRange = (MinRange * lrfScale);
                double maxScaleRange = (MaxRange * lrfScale);
                double minDistance;

                // LRFの値を調べる

                // 右側
                minDistance = maxScaleRange;
                for (int i = (stRAng + rangeCenterAng); i < (edRAng + rangeCenterAng); i++)
                {
                    // 範囲内なら反応
                    if (lrfData[i] > minScaledRange && lrfData[i] < maxScaleRange)
                    {
                        // もっとも近い障害物を選定
                        if (lrfData[i] < minDistance)
                        {
                            minDistance = lrfData[i];
                            RHitLength = lrfData[i];
                            RHitDir = (double)i;
                        }
                    }
                }

                // 左側
                minDistance = maxScaleRange;
                for (int i = (edLAng + rangeCenterAng); i >= (stLAng + rangeCenterAng); i--)
                {
                    // 範囲内なら反応
                    if (lrfData[i] > minScaledRange && lrfData[i] < maxScaleRange)
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
}
