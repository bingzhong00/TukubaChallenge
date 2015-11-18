// Copyright (c) 2011 TAJIMA Yoshiyuki 
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
// 
//   1. Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//   2. Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE FREEBSD PROJECT ``AS IS'' AND ANY EXPRESS OR 
// IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
// MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
// SHALL THE FREEBSD PROJECT OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
// OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE 
// OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
// OF THE POSSIBILITY OF SUCH DAMAGE.
// 
// The views and conclusions contained in the software and documentation are those 
// of the authors and should not be interpreted as representing official policies, 
// either expressed or implied, of the FreeBSD Project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LocationPresumption
{
    public class ParticleFilter
    {
        static Random Rand = new Random();

        int ResamplingNumber;

        MarkPoint[] ResamplingSet;


        /// <summary>
        /// 散らばる範囲
        /// </summary>
        double PtclDefRange;
        double PtclRange;

        double PtclDirRange;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="resamplingNumber">パーティクルの有効サンプル数（パーティクルと同じか少ない数を指定？）</param>
        public ParticleFilter(int resamplingNumber, double ParticleRange=5, double DirRange=10.0 )
        {
            ResamplingNumber = resamplingNumber;
            ResamplingSet = new MarkPoint[ResamplingNumber];

            PtclDefRange = PtclRange = ParticleRange;
            PtclDirRange = DirRange;
        }

        /// <summary>
        /// 距離レンジ変更（％）
        /// </summary>
        /// <param name="rangePer">0.0～1.0(100%)～</param>
        public void SetRagePercent(double rangePer)
        {
            PtclRange = PtclDefRange * rangePer;
        }

        /// <summary>
        /// 角度レンジ変更　（度）
        /// </summary>
        /// <param name="rangeDir"></param>
        public void SetDirectionRage(double rangeDir)
        {
            PtclDirRange = rangeDir;
        }

        /// <summary>
        /// LRF計測値と、パーティクル位置のマップとの差分をとる？
        /// </summary>
        /// <param name="real"></param>
        /// <param name="particle"></param>
        /// <returns>整合性？</returns>
        private double Likefood(double[] real, double[] particle)
        {
            double sum = 0;
            for (int i = 0; i < real.Length; ++i)
            {
                sum += Math.Abs(real[i] - particle[i]);
                //sum += (real[i] - particle[i]) * (real[i] - particle[i]);
                //sum += Math.Cos(real[i] 
            }

            // 0.0～1.0までの数値に変換 誤差が大きいほど0に近くなる
            return 1.0 / (1 + sum);
        }

        /// <summary>
        /// パーティクル生成
        /// </summary>
        /// <param name="v">基点</param>
        /// <param name="sigma">散らばりのフレ幅[pixel]</param>
        /// <param name="sigmaAng">散らばりのフレ幅[角度]</param>
        /// <param name="p">格納先</param>
        private void MakeParticle(MarkPoint v, double sigma, double sigmaAng, MarkPoint p)
        {
            var dx = NormalDistribution(sigma); // m単位
            var dy = NormalDistribution(sigma);
            var dtheta = NormalDistribution(sigmaAng);

            // 座標、回転角をふれ幅内で生成
            p.X = v.X + dx;
            p.Y = v.Y + dy;
            p.Theta = v.Theta + dtheta;
        }

        /// <summary>
        /// 分布させる？
        /// </summary>
        /// <param name="sigma">ふれ幅？</param>
        /// <returns></returns>
        private double NormalDistribution(double sigma)
        {
            var alpha = Rand.NextDouble();  // ※0.0が出るのはよくないのでは？
            var beta = Rand.NextDouble() * Math.PI * 2;

            double tmp;
            tmp = Math.Sin(beta);  // -1 ～ 1を生成
            // Log(0.0～1.0) = -1.0～0.0 二次曲線
            return sigma * Math.Sqrt(-2 * Math.Log(alpha)) * tmp;
        }

        /// <summary>
        /// 自己位置推定計算
        /// </summary>
        /// <param name="LRF_Data">ＬＲＦ実取得データ</param>
        /// <param name="MRF">MAPデータ</param>
        /// <param name="mkp">想定ロボット位置(計算基点)</param>
        /// <param name="Particles">パーティクル作業バッファ</param>
        public void Localize(double[] LRF_Data, MapRangeFinder MRF, MarkPoint mkp, List<Particle> Particles)
        {
            double sum = 0;

            // パーティクルをばらまく
            for (int i = 0; i < Particles.Count; ++i)
            {
                // 散らばらせる
                MakeParticle(mkp, PtclRange, PtclDirRange, Particles[i].Location);

                // 散らばり％ = w ?
                // マップデータとLRFのデータを比べる
                Particles[i].W = Likefood(LRF_Data, MRF.Sense(Particles[i].Location));
                sum += Particles[i].W;
            }

            // W順に並べ替え
            Particles.Sort((a, b) => (int)((a.W - b.W)*1000.0));

            // パーティクルから 整合性の高いデータを取り出す？(リサンプリング)
            for (int n = 0; n < ResamplingNumber; ++n)
            {
                // ※変なランダムになっているだけでは？
                // W値高いものからとったほうが精度が上がるのでは？？
                double selectVal = Rand.NextDouble() * sum;
                double subsum = 0;
                for (int i = 0; i < Particles.Count; ++i)
                {
                    subsum += Particles[i].W;
                    if (selectVal < subsum)
                    {
                        ResamplingSet[n] = Particles[i].Location;
                        break;
                    }
                }
            }
            
            // パーティクルの平均値から、仮想ロボットの位置を補正
            double newX = 0;
            double newY = 0;
            double newTheta = 0;
            for (int n = 0; n < ResamplingNumber; ++n) {
                newX += ResamplingSet[n].X;
                newY += ResamplingSet[n].Y;
                newTheta += ResamplingSet[n].Theta;
            }

            mkp.X = newX / ResamplingNumber;
            mkp.Y = newY / ResamplingNumber;
            mkp.Theta = newTheta / ResamplingNumber;
        }
    }
}
