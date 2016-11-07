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
        public MarkPoint mkp = new MarkPoint(0, 0, 0);
        public double PixelScale = 100.0;


        // 走行系シミュレート変数
        // car drive emu
        const double carWidth = 450.0;     // 左右のタイヤ間の距離 mm
        const double carWidthHf = carWidth / 2.0;
        const double carHeight = 450.0;    // ホイールベース

        const double carTireSize = 120.0 * 2.0;    // タイヤ直径 見やすく2倍
        const double carTireSizeHf = carTireSize / 2.0;

        // 
        // 各タイヤの初期位置
        public Vector3 wdFL = new Vector3();
        public Vector3 wdFR = new Vector3();
        public Vector3 wdRL = new Vector3();
        public Vector3 wdRR = new Vector3();

        public Vector3 wdRLOld = new Vector3();
        public Vector3 wdRROld = new Vector3();

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

        // ロータリーエンコーダ パルス値
        public double wheelPulseR = 0.0;
        public double wheelPulseL = 0.0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_pixelScale"></param>
        public CarSim(double _pixelScale)
        {
            PixelScale = _pixelScale;
        }

        /// <summary>
        /// クルマ初期化
        /// </summary>
        public void CarInit(double posx, double posy, double ang)
        {
            carHandleAng = 0.0;
            carAccVal = 0.0;

            wdCarAng = ang;

            wdCarF = new Vector3(posx, posy, 0.0);
            //wdCarR = new Vector3(posx, posy+carHeight, 0.0);
            {
                Vector3 carVec = new Vector3();

                // 車体のベクトル
                Quaternion rotRQt = new Quaternion();
                rotRQt.RollInDegrees = wdCarAng;
                carVec.y = carHeight;
                carVec = rotRQt.ToRotationMatrix() * carVec;

                wdCarR = new Vector3(carVec.x + posx, carVec.y + posy, carVec.z);
            }

            wdFL = new Vector3();
            wdFR = new Vector3();
            wdRL = new Vector3();
            wdRR = new Vector3();

            oldMS = DateTime.Now.Millisecond;

            calcTirePos(0);

            wdRLOld = new Vector3(wdRL.x, wdRL.y, wdRL.z);
            wdRROld = new Vector3(wdRR.x, wdRR.y, wdRR.z);

            wheelPulseR = 0.0;
            wheelPulseL = 0.0;
        }

        public void CarInit(MarkPoint _mkp)
        {
            CarInit(_mkp.X, _mkp.Y, _mkp.Theta);
            mkp.Set(_mkp);
        }

        /// <summary>
        /// センサー情報更新
        /// </summary>
        public void SenserUpdate( bool bLRF, bool bRE )
        {
            if (bLRF)
            {
                // LRF
                // マップ座標に変換
                mkp.LRFdata = mrf.Sense(new MarkPoint(mkp.X, mkp.Y, mkp.Theta));
            }

            if (bRE)
            {
                // R.E.
                CalcWheelPosToREPulse();
            }
        }

        /// <summary>
        /// MAP初期化
        /// </summary>
        /// <param name="fname"></param>
        public void MapInit(Bitmap mapBmp)
        {
            // 30m
            mrf = new MapRangeFinder((30 * 1000), PixelScale, mapBmp);
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
        /// タイヤ位置計算
        /// </summary>
        /// <param name="timeTick"></param>
        public void calcTirePos(long timeTick)
        {
            long difMS = timeTick; //DateTime.Now.Millisecond - oldMS;
            //double moveRad = (wdCarAng + carHandleAng) * Math.PI / 180.0;

            double moveLength = ((double)((4 * 1000 * 1000) / 60 / 60) / 1000.0);      // 単位時間内の移動量 時速4Km計算

            // 時間辺りの移動量を求める
            moveLength = moveLength * -carAccVal * (double)difMS;

            oldMS = DateTime.Now.Millisecond;

            {
                Vector3 moveVec = new Vector3();
                Quaternion rotQt = new Quaternion();

                rotQt.RollInDegrees = wdCarAng + carHandleAng;
                moveVec.y = moveLength;
                moveVec = rotQt.ToRotationMatrix() * moveVec;

                // ハンドリングの影響を加えて、クルマの向きを求める
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

                // フロント中心軸を移動量分加算
                wdCarF += moveVec;

                // mkpへ反映
                {
                    mkp.X += moveVec.x;
                    mkp.Y += moveVec.y;
                    mkp.Theta = wdCarAng;
                }
                //Debug.WriteLine(wdCarF);

                // 差分ように更新前の値を保存
                wdRLOld = new Vector3(wdRL.x, wdRL.y, wdRL.z);
                wdRROld = new Vector3(wdRR.x, wdRR.y, wdRR.z);

                // 各車輪の位置座標計算
                Vector3 shaftVec = new Vector3();
                Vector3 wheelFRvec = new Vector3();
                Vector3 wheelFLvec = new Vector3();
                Vector3 wheelRvec = new Vector3();
                Vector3 wheelLvec = new Vector3();

                // 前輪位置 算出
                wheelFRvec.x = carWidthHf;
                wheelFLvec.x = -carWidthHf;

                wheelFRvec = rotQt.ToRotationMatrix() * wheelFRvec;
                wheelFLvec = rotQt.ToRotationMatrix() * wheelFLvec;

                wdFR = wheelFRvec + wdCarF;
                wdFL = wheelFLvec + wdCarF;

                // バック側　中心位置
                shaftVec.y = carHeight;
                shaftVec = rotQt.ToRotationMatrix() * shaftVec;
                shaftVec += wdCarF;

                wdCarR = shaftVec;

                // 後輪位置算出
                wheelRvec.y = carHeight;
                wheelRvec.x = carWidthHf;

                wheelLvec.y = carHeight;
                wheelLvec.x = -carWidthHf;

                wheelRvec = rotQt.ToRotationMatrix() * wheelRvec;
                wheelLvec = rotQt.ToRotationMatrix() * wheelLvec;

                wdRR = wheelRvec + wdCarF;
                wdRL = wheelLvec + wdCarF;
            }

            // フロント軸位置とクルマの向きを元に、車輪の位置を求める
            // フロント車軸位置に(クルマの向き*carHeight)で、リア車軸位置
            // フロント車軸位置にクルマの向き+90度の-+方向にフロント車輪がある
            // リア車軸位置にクルマの向き+90度の-+方向にリア車輪がある
        }




        /// <summary>
        /// ホイールの移動量から
        /// ロータリーエンコーダ　回転パルス値を計算
        /// </summary>
        public void CalcWheelPosToREPulse()
        {
            const double WheelSize = 175;//172;    // ホイール直径
            const double OneRotValue = 240;   // １回転分の分解能

            Vector3 wheelLmov, wheelRmov;

            Real signL, signR;
            // 移動量と移動方向(+,-)を求める
            {
                Quaternion rotQt = new Quaternion();
                Vector3 moveVec = new Vector3();

                rotQt.RollInDegrees = wdCarAng;
                moveVec.y = 1.0;
                moveVec = rotQt.ToRotationMatrix() * moveVec;

                // 移動差分から、移動量を求める
                wheelLmov = new Vector3(wdRL.x - wdRLOld.x,
                                                 wdRL.y - wdRLOld.y,
                                                 wdRL.z - wdRLOld.z);

                wheelRmov = new Vector3(wdRR.x - wdRROld.x,
                                                 wdRR.y - wdRROld.y,
                                                 wdRR.z - wdRROld.z);

                if (moveVec.Dot(wheelLmov) > 0.0) signL = -1.0;
                else signL = 1.0;
                if (moveVec.Dot(wheelRmov) > 0.0) signR = -1.0;
                else signR = 1.0;
            }

            // 移動量(長さ) / ホイール１回転の長さ * １回転のパルス数
            wheelPulseL += (wheelLmov.Length / (WheelSize * Math.PI) * OneRotValue) * signL;
            wheelPulseR += (wheelRmov.Length / (WheelSize * Math.PI) * OneRotValue) * signR;

        }



        /// <summary>
        /// クルマ描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="ScaleRealToPixel"></param>
        /// <param name="ViewScale"></param>
        /// <param name="viewX"></param>
        /// <param name="viewY"></param>
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
