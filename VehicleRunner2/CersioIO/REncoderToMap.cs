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
        /// <summary>
        /// 車軸幅mm
        /// </summary>
        const double ShaftLength = 150;

        /// <summary>
        /// ホイール直径
        /// </summary>
        const double WheelSize = 65;

        /// <summary>
        /// １回転分の分解能
        /// </summary>
        const double OneRotValue = 240;

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

        // 絶対値 回転量
        static double WheelRotateToLength(double rot)
        {
            return (Math.PI * WheelSize) * rot / OneRotValue;
        }

        // ２輪の移動量の差から角度を求める
        static double WheelDifferenceToDegree(double WRLng, double WLLng)
        {
            return (((WRLng - WLLng) / (Math.PI * ShaftLength * 2.0)) * Math.PI);
        }


        // エンコードデータから角度を算出
        // 0>右周りの角度
        // 0<左周りの角度
        static double REncodeDataToDegree(double[] WRdat, double[] WLdat)
        {
            return WheelDifferenceToDegree(WheelRotateToLength(WRdat), WheelRotateToLength(WLdat));
        }

        static double REncodeDataToDegree(double WRdat, double WLdat)
        {
            return WheelDifferenceToDegree(WheelRotateToLength(WRdat), WheelRotateToLength(WLdat));
        }

        /// <summary>
        /// 左右のホイールの移動量を、X,Y座標に変換
        /// </summary>
        /// <param name="resXYWR">右車輪のX,Y座標</param>
        /// <param name="resXYWL">左車輪のX,Y座標</param>
        /// <param name="resAng">現在の向き</param>
        /// <param name="reWR">右車輪のREncoderの値 絶対値</param>
        /// <param name="reWL">左車輪のREncoderの値 絶対値</param>
        /// <param name="reOldWR">右車輪のREncoderの値 絶対値 (前回値)</param>
        /// <param name="reOldWL">左車輪のREncoderの値 絶対値 (前回値)</param>
        public static void CalcWheelPlotXY(ref PointD resXYWR, ref PointD resXYWL, ref double resAng, double reWR, double reWL, double reOldWR, double reOldWL)
        {
            // 車輪　調整(ハードウェアに起因するもの)
            {
                reWL = reWL * 0.9917;
                reOldWL = reOldWL * 0.9917;

                reWR = reWR * 1.0;
                reOldWR = reOldWR * 1.0;
            }

            double mov;
            double deg = REncodeDataToDegree(reWR, reWL);

            // 右車輪 移動量
            mov = WheelRotateToLength(reWR - reOldWR);

            resXYWR.X += (mov * -Math.Cos(deg));
            resXYWR.Y += (mov * Math.Sin(deg));

            // 左車輪 移動量
            mov = WheelRotateToLength(reWL - reOldWL);

            resXYWL.X += (mov * -Math.Cos(deg));
            resXYWL.Y += (mov * Math.Sin(deg));

            resAng = deg;
        }

        /// <summary>
        /// 左右のホイールの移動量を、X,Y座標に変換
        /// </summary>
        /// <param name="xyWR">右車輪のX,Y座標</param>
        /// <param name="xyWL">左車輪のX,Y座標</param>
        /// <param name="reWR">右車輪のREncoderの値 絶対値</param>
        /// <param name="reWL">左車輪のREncoderの値 絶対値</param>
        public static void CalcWheelPlotXY(out PointD[] xyWR, out PointD[] xyWL, double[] reWR, double[] reWL )
        {
            xyWR = new PointD[reWR.Length];
            xyWL = new PointD[reWR.Length];

            // 初期位置は上向き
            // 動力のある左の車輪が0,0基準
            xyWR[0] = new PointD();
            xyWL[0] = new PointD();

            xyWR[0].X = 500; xyWR[0].Y = 0;
            xyWL[0].X = 0; xyWL[0].Y = 0;

            // MakeLine
            for (int i = 1; i < reWR.Length; i++)
            {
                double mov;
                double deg = REncodeDataToDegree(reWR[i], reWL[i]);

                xyWR[i] = new PointD();
                xyWL[i] = new PointD();

                // 右車輪
                mov = WheelRotateToLength(reWR[i] - reWR[i - 1]);

                xyWR[i].X = xyWR[i - 1].X + (mov * -Math.Cos(deg));
                xyWR[i].Y = xyWR[i - 1].Y + (mov * Math.Sin(deg));

                // 左車輪
                mov = WheelRotateToLength(reWL[i] - reWL[i - 1]);

                xyWL[i].X = xyWL[i - 1].X + (mov * -Math.Cos(deg));
                xyWL[i].Y = xyWL[i - 1].Y + (mov * Math.Sin(deg));
            }

            //x11 = x1 * Math.cos(alpha) - y1 * Math.sin(alpha);
            //y11 = x1 * Math.sin(alpha) + y1 * Math.cos(alpha);
        }

        /// <summary>
        /// Limited Slip Plot
        /// </summary>
        /// <param name="xyWR"></param>
        /// <param name="xyWL"></param>
        /// <param name="OreWR"></param>
        /// <param name="OreWL"></param>
        /// <param name="lspLv"></param>
        /// <param name="lsp"></param>
        public static void CalcWheelPlotXY_LSP(out PointD[] xyWR, out PointD[] xyWL, double[] OreWR, double[] OreWL, int lspLv, double lsp)
        {
            xyWR = new PointD[OreWR.Length];
            xyWL = new PointD[OreWR.Length];

            // 初期位置は上向き
            // 動力のある左の車輪が0,0基準
            xyWR[0] = new PointD();
            xyWL[0] = new PointD();

            xyWR[0].X = 500; xyWR[0].Y = 0;
            xyWL[0].X = 0; xyWL[0].Y = 0;


            double[] reWR = new double[OreWR.Length];
            double[] reWL = new double[OreWR.Length];

            // Copy Work
            {
                for (int i = 0; i < reWR.Length; i++)
                {
                    reWR[i] = OreWR[i];
                    reWL[i] = OreWL[i];
                }
            }

            // Limited Slip Pulse
            {
                int lv = lspLv;    // Lv10 , lsp 0.52

                for (int i = lv; i < reWR.Length; i++)
                {
                    if ((reWR[i] - reWR[i - lv]) / (reWL[i] - reWL[i - lv]) < lsp)
                    {
                        double def = (reWR[i] - reWR[i - lv]);
                        double odef = ((reWL[i] - reWL[i - lv]) - def) / (double)lv;
                        /*
                        for (int n = 1; n < lv; n++)
                        {
                            reWL[i - lv + n] = reWL[i - lv] + (def / (double)lv);
                        }
                        */

                        for (int n = (i - lv); n < reWR.Length; n++)
                        {
                            reWL[n] -= odef;
                        }
                    }
                }
            }



            // MakeLine
            for (int i = 1; i < reWR.Length; i++)
            {
                double mov;
                double deg = REncodeDataToDegree(reWR[i], reWL[i]);
                double ang = deg * Math.PI / 180.0;

                xyWR[i] = new PointD();
                xyWL[i] = new PointD();

                // 右車輪
                mov = WheelRotateToLength(reWR[i] - reWR[i - 1]);

                xyWR[i].X = xyWR[i - 1].X + (mov * -Math.Cos(ang));
                xyWR[i].Y = xyWR[i - 1].Y + (mov * Math.Sin(ang));

                // 左車輪
                mov = WheelRotateToLength(reWL[i] - reWL[i - 1]);

                xyWL[i].X = xyWL[i - 1].X + (mov * -Math.Cos(ang));
                xyWL[i].Y = xyWL[i - 1].Y + (mov * Math.Sin(ang));
            }

            //x11 = x1 * Math.cos(alpha) - y1 * Math.sin(alpha);
            //y11 = x1 * Math.sin(alpha) + y1 * Math.cos(alpha);
        }
    }
}
