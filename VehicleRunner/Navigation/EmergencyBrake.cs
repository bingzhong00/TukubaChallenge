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
        public int CheckEBS(double[] lrfData, long UpdateCnt)
        {
            int rangeAngHalf = 270 / 2; // 中央(角度)
            int stAng = EmergencyBrake.stAng;
            int edAng = EmergencyBrake.edAng;

            bool bCauntion = false;
            bool bStop = false;
            int cntCautuon = 0;
            int cntStop = 0;

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

            double BrakeScaledRange = BrakeRange * URG_LRF.getScale();
            double SlowScaledRange = SlowRange * URG_LRF.getScale();

            // LRFの値を調べる
            for (int i = (stAng + rangeAngHalf); i < (edAng + rangeAngHalf); i++)
            {
                // ブレーキレンジ内か？
                if (lrfData[i] < BrakeScaledRange)
                {
                    cntStop++;
                    bStop = true;
                }

                // 徐行レンジ内か？ 
                if (lrfData[i] < SlowScaledRange)
                {
                    cntCautuon++;
                    bCauntion = true;
                }
            }

            // 注意Lv引き上げ
            if (bStop)
            {
                // 停止
                CautionLv = CautionLvMax;
                hzUCNT = UpdateCnt;
            }
            else if (bCauntion && CautionLv < (StopLv - 1))
            {
                // 徐行の最大まで(注意状態だけなら止まりはしない)
                CautionLv += cntCautuon;
                hzUCNT = UpdateCnt;
            }

            return CautionLv;
        }
    }
}
