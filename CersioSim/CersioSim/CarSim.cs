using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiom.Math;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

using LocationPresumption;

namespace CersioSim
{
    public class CarSim
    {
        // センサー系シミュレート変数
        // car senser emu
        public MapRangeFinder mrf;
        public MarkPoint mkp = new MarkPoint(0,0,0);
        public double PixelScale = 100.0;


        // 走行系シミュレート変数
        // car drive emu
        const double carWidth = 450.0;     // 左右のタイヤ間の距離 mm
        const double carWidthHf = carWidth/2.0;
        const double carHeight = 450.0;    // ホイールベース

        const double carTireSize = 120.0 * 2.0;    // タイヤ直径 見やすく2倍
        const double carTireSizeHf = carTireSize / 2.0;

        // 
        // 各タイヤの初期位置
        private Vector3 wdFL = new Vector3();
        private Vector3 wdFR = new Vector3();
        private Vector3 wdRL = new Vector3();
        private Vector3 wdRR = new Vector3();

        // クルマの中心点
        // 車の基準点を前輪軸の真ん中とする
        public Vector3 wdCarF;
        public Vector3 wdCarR;
        // クルマの向き
        public double wdCarAng = 0.0;

        // ハンドル角(度)
        public double carHandleAngMax = 30.0;  // +-30.0
        public double carHandleAng = 0.0;

        // アクセル
        // +で前進
        public double carAccVal = 0.0;

        /// <summary>
        /// クルマ初期化
        /// </summary>
        public void CarInit(double posx,double posy)
        {
            carHandleAng = 0.0;
            carAccVal = 0.0;
            wdCarAng = 0.0;

            wdCarF = new Vector3(posx, posy, 0.0);
            wdCarR = new Vector3(posx, posy+carHeight, 0.0);
            wdFL = new Vector3();
            wdFR = new Vector3();
            wdRL = new Vector3();
            wdRR = new Vector3();

            oldMS = DateTime.Now.Millisecond;

            calcTirePos(0);
        }

        public void CarInit(MarkPoint _mkp)
        {
            CarInit(_mkp.X, _mkp.Y);
            mkp.Set(_mkp);
        }

        public void SenserUpdate()
        {
            // マップ座標に変換
            mkp.LRFdata = mrf.Sense(new MarkPoint(mkp.X, mkp.Y, mkp.Theta));
        }

        /// <summary>
        /// MAP初期化
        /// </summary>
        /// <param name="fname"></param>
        public void MapInit(string fname)
        {
            // 30m
            mrf = new MapRangeFinder((30 * 1000),PixelScale, fname);
        }

        /// <summary>
        /// 2つのベクトルがなす角をかえす
        /// </summary>
        /// <param name="vecA"></param>
        /// <param name="vecB"></param>
        /// <returns></returns>
        public double VecToRad(Vector3 vecA, Vector3 vecB)
        {
            vecA.Normalize();
            vecB.Normalize();

            double rad = vecA.Dot(vecB);
            if (rad > 1.0) rad = 1.0;

            double dir = (double)(Math.Asin(rad) - (Math.PI / 2.0));

            if (double.IsNaN(dir))
            {
                Debug.Write("NAn");
            }

            Vector3 resVec = vecA.Cross(vecB);
            if (resVec.z > 0) dir = -dir;

            return dir;
        }

        long oldMS;
        /// <summary>
        ///  タイヤ位置計算
        /// </summary>
        public void calcTirePos( long timeTick )
        {
            long difMS = timeTick; //DateTime.Now.Millisecond - oldMS;
            //double moveRad = (wdCarAng + carHandleAng) * Math.PI / 180.0;

            double moveLength = ((double)((4 * 1000 * 1000) / 60 / 60) / 1000.0);      // 単位時間内の移動量 時速4Km計算

            moveLength = moveLength * -carAccVal * (double)difMS;

            oldMS = DateTime.Now.Millisecond;

            {
                Vector3 moveVec = new Vector3();
                Quaternion rotQt = new Quaternion();

                rotQt.RollInDegrees = wdCarAng + carHandleAng;
                moveVec.y = moveLength;
                moveVec = rotQt.ToRotationMatrix() * moveVec;

                // クルマの向きを求める
#if true
                {
                    Vector3 carVec = new Vector3();
                    Vector3 movedcarVec = new Vector3();

                    // 車体のベクトル
                    Quaternion rotRQt = new Quaternion();
                    rotRQt.RollInDegrees = wdCarAng;
                    carVec.y = carHeight;
                    carVec = rotRQt.ToRotationMatrix() * carVec;

                    movedcarVec = carVec + moveVec;
                    double addRad = VecToRad(movedcarVec, carVec);
                    wdCarAng += addRad * 180.0 / Math.PI;
                }
#else
            wdCarAng += carHandleAng;
#endif
            }

            // クルマの向きに対する移動を求める
            {
                Quaternion rotQt = new Quaternion();
                Vector3 moveVec = new Vector3();

                rotQt.RollInDegrees = wdCarAng;
                moveVec.y = moveLength;
                moveVec = rotQt.ToRotationMatrix() * moveVec;

                wdCarF += moveVec;

                // mkpへ反映
                {
                    mkp.X += moveVec.x;
                    mkp.Y += moveVec.y;
                    mkp.Theta = wdCarAng;
                }
                Debug.Write(wdCarF);
            }

            // フロント軸位置とクルマの向きを元に、車輪の位置を求める
            // フロント車軸位置に(クルマの向き*carHeight)で、リア車軸位置
            // フロント車軸位置にクルマの向き+90度の-+方向にフロント車輪がある
            // リア車軸位置にクルマの向き+90度の-+方向にリア車輪がある
        }


        public void DrawCar(Graphics g, double ScaleRealToPixel, double ViewScale, double viewX, double viewY)
        {
            // 左右車輪間のライン、前後車軸間のライン
            // クルマの向きと同じ角度で、車輪位置にタイヤライン (フロントはクルマの向き+ハンドル角)

            Vector3 carVec = new Vector3(0.0, carHeight, 0.0);
            //Quaternion rotRQt = new Quaternion();
            //rotRQt.RollInDegrees = wdCarAng;
            //carVec = rotRQt.ToRotationMatrix() * carVec;

            // 後輪が軸が正解？

            g.ResetTransform();
            g.ScaleTransform((float)ScaleRealToPixel, (float)ScaleRealToPixel);
            g.TranslateTransform((float)-viewX, (float)-viewY, MatrixOrder.Append);
            g.ScaleTransform((float)ViewScale, (float)ViewScale, MatrixOrder.Append);


            g.TranslateTransform((float)wdCarF.x, (float)wdCarF.y, MatrixOrder.Prepend);
            g.RotateTransform((float)wdCarAng, MatrixOrder.Prepend);

            // 車軸
            g.DrawLine(Pens.Red,
                        0.0f, -carVec.y,
                        carVec.x, 0.0f);

            // 前輪軸
            g.DrawLine(Pens.Red,
                        (float)-carWidthHf, -carVec.y,
                        (float)carWidthHf, -carVec.y);

            // 後輪軸
            g.DrawLine(Pens.Red,
                        (float)-carWidthHf, 0.0f,
                        (float)carWidthHf, 0.0f);

            // 後輪タイヤ
            g.DrawLine(Pens.Red,
                        (float)-carWidthHf, (float)-carTireSizeHf,
                        (float)-carWidthHf, (float)+carTireSizeHf);

            g.DrawLine(Pens.Red,
                        (float)+carWidthHf, (float)-carTireSizeHf,
                        (float)+carWidthHf, (float)+carTireSizeHf);

            // 前輪左
            g.TranslateTransform((float)-carWidthHf, (float)-carVec.y, MatrixOrder.Prepend);
            g.RotateTransform((float)carHandleAng, MatrixOrder.Prepend);

            g.DrawLine(Pens.Red,
                        0.0f, (float)-carTireSizeHf,
                        0.0f, (float)carTireSizeHf);

            // 前輪右
            g.ResetTransform();
            g.ScaleTransform((float)ScaleRealToPixel, (float)ScaleRealToPixel);
            g.TranslateTransform((float)-viewX, (float)-viewY, MatrixOrder.Append);
            g.ScaleTransform((float)ViewScale, (float)ViewScale, MatrixOrder.Append);


            g.TranslateTransform((float)wdCarF.x, (float)wdCarF.y, MatrixOrder.Prepend);
            g.RotateTransform((float)wdCarAng, MatrixOrder.Prepend);

            //
            g.TranslateTransform((float)carWidthHf, (float)-carVec.y, MatrixOrder.Prepend);
            g.RotateTransform((float)carHandleAng, MatrixOrder.Prepend);

            g.DrawLine(Pens.Red,
                        0.0f, (float)-carTireSizeHf,
                        0.0f, (float)carTireSizeHf);
        }

    }
}
