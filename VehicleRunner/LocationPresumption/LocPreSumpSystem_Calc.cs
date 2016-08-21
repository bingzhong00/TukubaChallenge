using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Axiom.Math;

namespace LocationPresumption
{
    /// <summary>
    /// 自己位置推定 マップ座標変換計算　クラス
    /// </summary>
    public partial class LocPreSumpSystem
    {
        /// <summary>
        /// ローパスフィルター
        /// </summary>
        private KalmanFilter.SimpleLPF lpfV1X = new KalmanFilter.SimpleLPF();
        private KalmanFilter.SimpleLPF lpfV1Y = new KalmanFilter.SimpleLPF();
        private KalmanFilter.SimpleLPF lpfV1Theta = new KalmanFilter.SimpleLPF();


        // 左回転が+　右が-
        public const int ParticleSize = 200;        // パーティクル数
        private ParticleFilter ParticleFilter;              // サンプリング数、パーティクルのレンジ
        private List<Particle> Particles;           // パーティクルリスト


        // 処理速度計測カウンタ
        public static System.Diagnostics.Stopwatch swCNT_Update = new System.Diagnostics.Stopwatch();
        public static System.Diagnostics.Stopwatch swCNT_MRF = new System.Diagnostics.Stopwatch();


        /// <summary>
        /// 任意座標でのＰＦ補正
        /// </summary>
        /// <param name="numPF"></param>
        public void CalcLocalize(MarkPoint mkp, bool useLowFilter, int numPF = 1)
        {
            // パーティクルフィルター演算

            if (useLowFilter)
            {
                KalmanFilter.SimpleLPF lpfX = null;
                KalmanFilter.SimpleLPF lpfY = null;
                KalmanFilter.SimpleLPF lpTheta = null;

                for (int i = 0; i < numPF; i++)
                {
                    MarkPoint locMkp = new MarkPoint(worldMap.GetAreaX(mkp.X), worldMap.GetAreaY(mkp.Y), mkp.Theta);

                    ParticleFilter.Localize(LRF.getData(), MRF, locMkp, Particles);

                    locMkp.X = worldMap.GetWorldX(locMkp.X);
                    locMkp.Y = worldMap.GetWorldY(locMkp.Y);

                    // 結果にローパスフィルターをかける
                    if (null == lpfX)
                    {
                        // 最初の値を初期値にする
                        lpfX = new KalmanFilter.SimpleLPF(locMkp.X);
                        lpfY = new KalmanFilter.SimpleLPF(locMkp.Y);
                        lpTheta = new KalmanFilter.SimpleLPF(locMkp.Theta);
                    }
                    else
                    {
                        lpfX.update(locMkp.X);
                        lpfY.update(locMkp.Y);
                        lpTheta.update(locMkp.Theta);
                    }
                }

                if (null != lpfX)
                {
                    mkp.X = lpfX.value();
                    mkp.Y = lpfY.value();
                    mkp.Theta = lpTheta.value();
                }
            }
            else
            {
                ParticleFilter.Localize(LRF.getData(), MRF, mkp, Particles);
            }
        }

        /// <summary>
        /// PF自己位置計算 継続
        /// </summary>
        /// <param name="mkp"></param>
        /// <param name="useLowFilter"></param>
        /// <param name="numPF"></param>
        public void ParticleFilterLocalize(bool useLPF)
        {
            MarkPoint locMkp = new MarkPoint(worldMap.GetAreaX(V1.X), worldMap.GetAreaY(V1.Y), V1.Theta);

            // パーティクルフィルター演算
            ParticleFilter.Localize(LRF.getData(), MRF, locMkp, Particles);

            if (useLPF)
            {
                // 結果にローパスフィルターをかける
                V1.X = lpfV1X.update(worldMap.GetWorldX(locMkp.X));
                V1.Y = lpfV1Y.update(worldMap.GetWorldY(locMkp.Y));
                V1.Theta = lpfV1Theta.update(locMkp.Theta);
            }
            else
            {
                V1.X = worldMap.GetWorldX(locMkp.X);
                V1.Y = worldMap.GetWorldY(locMkp.Y);
                V1.Theta = locMkp.Theta;
            }
        }

        /// <summary>
        /// 自己位置推定 更新
        /// </summary>
        /// <param name="useAlwaysPF">毎回パーティクルフィルターを計算する</param>
        /// <returns>true...補正した, false..補正不要</returns>
        public bool ParticleFilter_Update()
        {
            bool bResult = false;   // 補正したか？

            {
                // 処理時間計測
                swCNT_Update.Start();

                // PF演算
                ParticleFilterLocalize(true);

                // 処理時間計測完了
                swCNT_Update.Stop();
            }

            return bResult;
        }

    }
}