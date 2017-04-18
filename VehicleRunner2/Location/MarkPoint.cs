
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Location
{
    /// <summary>
    /// 位置座標
    /// </summary>
    public class MarkPoint
    {
        /// <summary> リアル座標 X [m] </summary>
        public double X;
        /// <summary> リアル座標 Y [m] </summary>
        public double Y;
        /// <summary> 向き -Pi ～ Pi </summary>
        public double Theta;

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
            X = _x;
            Y = _y;
            Theta = _theta;
        }

        public MarkPoint(MarkPoint mkp) : this(mkp.X, mkp.Y, mkp.Theta )
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
        /// マーカーと同じか比較
        /// </summary>
        /// <param name="B"></param>
        /// <returns></returns>
        public bool IsEqual(MarkPoint B)
        {
            if (X == B.X && Y == B.Y && Theta == B.Theta) return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="theta"></param>
        public void Set(double x, double y, double theta)
        {
            X = x;
            Y = y;
            Theta = theta;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="B"></param>
        public void Set(MarkPoint B)
        {
            Set(B.X, B.Y, B.Theta);
        }
    }

    // ---------------------------------------------------------------------------------------------------

    /// <summary>
    /// 描画位置座標
    /// </summary>
    public class DrawMarkPoint
    {
        /// <summary> Map座標 X </summary>
        public float X;
        /// <summary> Map座標 Y </summary>
        public float Y;
        /// <summary> 向き -180 ～ 180 </summary>
        public float Theta;

        /// <summary>
        /// ロボット抽象化クラス
        /// </summary>
        /// <param name="_x">座標x</param>
        /// <param name="_y">座標y</param>
        /// <param name="_theta">向き角度</param>
        /// <param name="_wdmap">ワールドマップ情報</param>
        public DrawMarkPoint(double _x, double _y, double _theta)
        {
            X = (float)_x;
            Y = (float)_y;
            Theta = (float)_theta;
        }

        public DrawMarkPoint(double _x, double _y, double _theta, LocationSystem locSys) : this(new MarkPoint(_x, _y, _theta), locSys)
        {
        }

        /// <summary>
        /// ROS座標からマップ座標系へ変換
        /// </summary>
        /// <param name="mkp"></param>
        /// <param name="mapScale"></param>
        public DrawMarkPoint(MarkPoint mkp, double mapScale=1.0) : this(mkp.X * mapScale, mkp.Y * mapScale, mkp.Theta*180.0/Math.PI)
        {
        }

        /// <summary>
        /// ROS座標からマップ座標系へ変換
        /// </summary>
        /// <param name="mkp"></param>
        /// <param name="mapScale"></param>
        public DrawMarkPoint(MarkPoint mkp, LocationSystem locSys)
        {
            X = (float)(mkp.X * locSys.mToMap);
            Y = (float)(mkp.Y * locSys.mToMap);
            Theta = (float)(mkp.Theta * 180.0 / Math.PI);
        }

        /*
        public DrawMarkPoint(DrawMarkPoint mkp) : this(mkp.X, mkp.Y, mkp.Theta)
        {
        }*/
    }
    }
