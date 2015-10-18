using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CersioIO
{
    public class PointD
    {
        public double X;
        public double Y;

        public PointD() { X = 0; Y = 0; }
    }

    public class REncoderToMap
    {
        const double ShaftLength = 400;//450;   // 車軸幅mm
        const double WheelSize = 175;//172;    // ホイール直径
        const double OneRotValue = 240;   // １回転分の分解能
        const double OneRotValueR = 248.54;   // １回転分の分解能
        // R 248.5

        // 回転から移動距離を出す
        // 差分
        static double WheelRotateToLength(double[] rot)
        {
            double rotSum = 0;

            for (int i = 0; i < rot.Length; i++)
            {
                rotSum += rot[i];
            }

            return WheelRotateToLength(rotSum);
        }
        static double WheelRotateToLengthR(double[] rot)
        {
            double rotSum = 0;

            for (int i = 0; i < rot.Length; i++)
            {
                rotSum += rot[i];
            }

            return WheelRotateToLengthR(rotSum);
        }

        // 絶対値 回転量
        static double WheelRotateToLength(double rot)
        {
            return (Math.PI * WheelSize) * rot / OneRotValue;
        }
        static double WheelRotateToLengthR(double rot)
        {
            return (Math.PI * WheelSize) * rot / OneRotValueR;
        }

        // ２輪の移動量の差から角度を求める
        static double WheelDifferenceToDegree(double WRLng, double WLLng)
        {
            return (((WRLng - WLLng) / (Math.PI * ShaftLength * 2.0)) * 360.0);
        }


        // エンコードデータから角度を算出
        // 0>右周りの角度
        // 0<左周りの角度
        static double REncodeDataToDegree(double[] WRdat, double[] WLdat)
        {
            return WheelDifferenceToDegree(WheelRotateToLengthR(WRdat), WheelRotateToLength(WLdat));
        }

        static double REncodeDataToDegree(double WRdat, double WLdat)
        {
            return WheelDifferenceToDegree(WheelRotateToLengthR(WRdat), WheelRotateToLength(WLdat));
        }

        /// <summary>
        /// 左右のホイールの移動量を、X,Y座標に変換
        /// </summary>
        /// <param name="xyWR">右車輪のX,Y座標</param>
        /// <param name="xyWL">左車輪のX,Y座標</param>
        /// <param name="reWR">右車輪のREncoderの値 絶対値</param>
        /// <param name="reWL">左車輪のREncoderの値 絶対値</param>
        public static void CalcWheelPlotXY(out PointD[] xyWR, out PointD[] xyWL, double[] reWR, double[] reWL)
        {
            xyWR = new PointD[reWR.Length];
            xyWL = new PointD[reWR.Length];

            // 初期位置は上向き
            // 動力のある左の車輪が0,0基準
            xyWR[0] = new PointD();
            xyWL[0] = new PointD();

            xyWR[0].X = 500; xyWR[0].Y = 0;
            xyWL[0].X = 0; xyWL[0].Y = 0;

            for (int i = 1; i < reWR.Length; i++)
            {
                double mov;
                double deg = REncodeDataToDegree(reWR[i], reWL[i]) - 90.0;
                double ang = deg * Math.PI / 180.0;

                xyWR[i] = new PointD();
                xyWL[i] = new PointD();

                // 右車輪
                mov = WheelRotateToLengthR(reWR[i] - reWR[i-1]);

                xyWR[i].X = xyWR[i-1].X + (mov * -Math.Cos(ang));
                xyWR[i].Y = xyWR[i-1].Y + (mov * Math.Sin(ang));

                // 左車輪
                mov = WheelRotateToLength(reWL[i] - reWL[i - 1]);

                xyWL[i].X = xyWL[i-1].X + (mov * -Math.Cos(ang));
                xyWL[i].Y = xyWL[i-1].Y + (mov * Math.Sin(ang));
            }

            //x11 = x1 * Math.cos(alpha) - y1 * Math.sin(alpha);
            //y11 = x1 * Math.sin(alpha) + y1 * Math.cos(alpha);
        }
    }
}
