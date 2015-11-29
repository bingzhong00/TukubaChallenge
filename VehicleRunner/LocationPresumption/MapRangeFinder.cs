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

    public class MapRangeFinder {
        private GridMap Map;
        private double RangeMax;
        private double[] DeltaX;
        private double[] DeltaY;

        public const int AngleRange = 270;     // 認識角 270度
        public const int AngleRangeHalf = (AngleRange / 2);

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="map"></param>
        /// <param name="rangeMax"></param>
        public MapRangeFinder(double rangeMax) {
            //Map = map;
            RangeMax = rangeMax;

            // 傾きテーブル作成
            DeltaX = new double[360];
            DeltaY = new double[360];

            for (int theta = 0; theta < 360; ++theta)
            {
                DeltaX[theta] = 0.2 * Math.Cos((theta - 90) * Math.PI / 180.0);
                DeltaY[theta] = 0.2 * Math.Sin((theta - 90) * Math.PI / 180.0);


                // 1.0を基準にする(高速化)
                
                {
                    double r;
                  
                    if (Math.Abs(DeltaX[theta]) < Math.Abs(DeltaY[theta])) r = 1.0 / Math.Abs(DeltaY[theta]);
                    else r = 1.0 / Math.Abs(DeltaX[theta]);

                    DeltaX[theta] *= r;
                    DeltaY[theta] *= r;
                }
            }
        }

        public void SetMap(GridMap map)
        {
            Map = map;
        }

        /// <summary>
        /// LRFセンサーデータをGridMapから生成(代用)
        /// </summary>
        /// <param name="robot"></param>
        /// <returns></returns>
        public double[] Sense(GridMap map, double posX, double posY, double robotTheta)
        {
            double[] result = new double[AngleRange];
            int i;
            int angRng = AngleRange / 2;


            int iroboTheta = (int)(robotTheta+0.5);
            iroboTheta = iroboTheta - ((iroboTheta / 360) * 360);

            LocPreSumpSystem.swCNT_MRF.Start();
            try
            {
                for (i = -angRng; i < angRng; ++i)
                {
                    int theta = (-iroboTheta + i + 360 * 2) % 360;

                    // 障害物までの距離を取得
                    result[i + angRng] = map.MeasureDist(
                        posX,
                        posY,
                        DeltaX[theta],
                        DeltaY[theta],
                        RangeMax);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                LocPreSumpSystem.swCNT_MRF.Stop();
            }

            return result;
        }

        public double[] Sense(MarkPoint mkp )
        {
            return Sense(Map, mkp.X, mkp.Y, mkp.Theta);
        }
    }

}
