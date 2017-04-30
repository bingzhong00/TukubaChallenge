
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Location
{
    /// <summary>
    /// 位置座標[ROS座標]
    /// </summary>
    public class MarkPoint
    {
        /// <summary> リアル座標 X [m] </summary>
        public double x;
        /// <summary> リアル座標 Y [m] </summary>
        public double y;
        /// <summary> 向き -Pi ～ Pi </summary>
        public double theta;

        /// <summary> 移動距離 </summary>
        public double distance;

        /// <summary>
        /// ロボット抽象化クラス
        /// </summary>
        /// <param name="_x">座標x</param>
        /// <param name="_y">座標y</param>
        /// <param name="_theta">向き角度</param>
        /// <param name="_wdmap">ワールドマップ情報</param>
        public MarkPoint( double _x, double _y, double _theta)
        {
            x = _x;
            y = _y;
            theta = _theta;
        }

        public MarkPoint(MarkPoint mkp) : this(mkp.x, mkp.y, mkp.theta )
        {
        }

        /// <summary>
        /// 他のマーカとの距離を返す
        /// </summary>
        /// <param name="B"></param>
        /// <returns></returns>
        public double GetDistance(MarkPoint B)
        {
            double dx = (x - B.x);
            double dy = (y - B.y);
            return Math.Sqrt((dx*dx) + (dy*dy));
        }

        /// <summary>
        /// マーカーと同じか比較
        /// </summary>
        /// <param name="B"></param>
        /// <returns></returns>
        public bool IsEqual(MarkPoint B)
        {
            if (x == B.x && y == B.y && theta == B.theta) return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="theta"></param>
        public void Set(double _x, double _y, double _theta)
        {
            x = _x;
            y = _y;
            theta = _theta;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="B"></param>
        public void Set(MarkPoint B)
        {
            Set(B.x, B.y, B.theta);
        }
    }

    // ---------------------------------------------------------------------------------------------------

    /// <summary>
    /// 描画位置座標
    /// </summary>
    public class DrawMarkPoint
    {
        /// <summary> Map座標 X </summary>
        public float x;
        /// <summary> Map座標 Y </summary>
        public float y;
        /// <summary> 向き -PI ～ PI </summary>
        public float theta;

        /// <summary>
        /// ロボット抽象化クラス
        /// </summary>
        /// <param name="_x">座標x</param>
        /// <param name="_y">座標y</param>
        /// <param name="_theta">向き角度</param>
        /// <param name="_wdmap">ワールドマップ情報</param>
        public DrawMarkPoint(double _x, double _y, double _theta)
        {
            x = (float)_x;
            y = (float)_y;
            theta = (float)_theta;
        }

        public DrawMarkPoint(double _x, double _y, double _theta, LocationSystem locSys) : this(new MarkPoint(_x, _y, _theta), locSys)
        {
        }

        /// <summary>
        /// ROS座標からマップ座標系へ変換
        /// </summary>
        /// <param name="mkp"></param>
        /// <param name="mapScale"></param>
        public DrawMarkPoint(MarkPoint mkp, double mapScale=1.0) : this(mkp.x * mapScale, mkp.y * mapScale, mkp.theta)
        {
        }

        /// <summary>
        /// ROS座標からマップ座標系へ変換
        /// </summary>
        /// <param name="mkp"></param>
        /// <param name="mapScale"></param>
        public DrawMarkPoint(MarkPoint mkp, LocationSystem locSys)
        {
            x = (float)(mkp.x * locSys.mToMap);
            y = (float)(mkp.y * locSys.mToMap);
            theta = (float)(mkp.theta);
        }

        /*
        public DrawMarkPoint(DrawMarkPoint mkp) : this(mkp.X, mkp.Y, mkp.Theta)
        {
        }*/
    }
    }
