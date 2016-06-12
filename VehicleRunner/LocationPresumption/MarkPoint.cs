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
    public class MarkPoint
    {
        public double X;
        public double Y;
        public double Theta;

        public double[] LRFdata;

        /// <summary>
        /// ロボット抽象化クラス
        /// </summary>
        /// <param name="lrf">ＬＲＦ</param>
        /// <param name="x">座標x</param>
        /// <param name="y">座標y</param>
        /// <param name="theta">向き？角度</param>
        //public MarkPoint(MapRangeFinder mrf,  double x, double y, double theta)
        public MarkPoint( double x, double y, double theta)
        {
            X = x;
            Y = y;
            Theta = theta;
        }

        public MarkPoint(MarkPoint mkp) : this(mkp.X, mkp.Y, mkp.Theta)
        {
        }

        /// <summary>
        /// 他のマーカとの距離を返す
        /// </summary>
        /// <param name="B"></param>
        /// <returns></returns>
        public double GetDistance(MarkPoint B)
        {
            double dx = (X - B.X);
            double dy = (Y - B.Y);
            return Math.Sqrt((dx*dx) + (dy*dy));
        }

        /// <summary>
        /// LRF(周辺障害物距離)データを取得
        /// </summary>
        /// <param name="sencerData"></param>
        public void SetSenseData( double[] sencerData )
        {
            LRFdata = sencerData;
        }

        /// <summary>
        /// マーカーと同じか比較
        /// </summary>
        /// <param name="B"></param>
        /// <returns></returns>
        public bool IsEqual(MarkPoint B)
        {
            if (X == B.X && Y == B.Y && Theta == B.Theta) return true;
            return false;
        }

        public void Set(double x, double y, double theta)
        {
            X = x;
            Y = y;
            Theta = theta;
        }

        public void Set(MarkPoint B)
        {
            Set(B.X, B.Y, B.Theta);
        }

        /// <summary>
        /// 保持しているLRFデータを返す
        /// </summary>
        /// <returns></returns>
        public double[] GetSenseData()
        {
            return LRFdata;
        }
    }
}
