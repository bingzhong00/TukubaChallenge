using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace LRFMapEditer
{
    public class LayerData
    {
        public double[] LRFdata;

        public Bitmap MapBmp = null;
        public Point[] psLayerTriangle;

        public double lcX;      // ローカル上の座標
        public double lcY;      //
        public double lcAng;    // 角度
        public double lcPivotLength;

        public double wX;  // ワールド上の座標
        public double wY;
        public double wAng; // 角度

        public double mapScale;

        // LRFの設定値
        int centerIndex;                            // 中心インデックス
        int numDir;                                 // データ数
        const double startDir = -135.0;      // LRF開始角度
        const double endDir = 135.0;         // 終了角度

        //
        public bool useFlg;     // 使用フラグ

        public Color drawColor;

        public LayerData()
        {
            wX = 0.0;
            wY = 0.0;
            wAng = 0.0;
            lcPivotLength = 0.0;
            useFlg = false;
        }

        public LayerData(double[] lrfData) : this()
        {
            SetLRFData(lrfData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lrfData">LRFデータ</param>
        /// <param name="centerIdx">中心インデックス</param>
        /// <param name="stDir">開始角度</param>
        /// <param name="edDir">終了角度</param>
        public void SetLRFData( double[] lrfData )
        {
            LRFdata = lrfData;
            numDir = lrfData.Length;
            centerIndex = numDir / 2;
        }

        /// <summary>
        /// LRFのデータを画像に描画
        /// </summary>
        /// <param name="maxRange"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public Bitmap MakeMapBmp(double maxRange, double scale, int pixelSize, Color pixelColor, Color baseColor)
        {
            int bmpWH = (int)(((maxRange * 2.0) * scale) + 0.5);
            MapBmp = new Bitmap(bmpWH, bmpWH);
            drawColor = pixelColor;

            mapScale = scale;
            UpdateMapBmp(pixelSize, pixelColor, baseColor);

            // 高速描画用トライアングル
            psLayerTriangle = new Point[4];
            {
                int hfWH = bmpWH / 2;
                psLayerTriangle[0] = new Point(hfWH + 0, hfWH - 30/3);
                psLayerTriangle[1] = new Point(hfWH + 30/3, hfWH + 50/3);
                psLayerTriangle[2] = new Point(hfWH + 0, hfWH + 0);
                psLayerTriangle[3] = new Point(hfWH - 30/3, hfWH + 50/3);
            }

            return MapBmp;
        }

        /// <summary>
        /// 描画更新
        /// </summary>
        /// <param name="pixelSize">ピクセルサイズ</param>
        /// <param name="pixelColor">ピクセルカラー</param>
        /// <param name="baseColor">背景カラー</param>
        /// <param name="bDrawGide">ガイドライン描画</param>
        /// <param name="bLineMode">ライン描画モード</param>
        /// <param name="bTranparentInverse">抜き反転</param>
        /// <returns></returns>
        public bool UpdateMapBmp(int pixelSize,Color pixelColor,Color baseColor,bool bDrawGide = true, bool bLineMode=false, bool bTranparentInverse=false)
        {
            if (MapBmp == null) return false;

            int hPs = pixelSize / 2;
            int bmpSize = MapBmp.Width;

            double centerX = bmpSize / 2;
            double centerY = bmpSize / 2;
            Graphics g = Graphics.FromImage(MapBmp);
            drawColor = pixelColor;

            g.FillRectangle( new SolidBrush(baseColor), 0, 0, MapBmp.Width, MapBmp.Height);

            // LRFの距離データをピクセルにする
            double rad = (startDir - 90.0) * (Math.PI / 180.0);
            double radAdd = (endDir - startDir) / numDir * (Math.PI / 180.0);

            Brush brs = new SolidBrush(pixelColor);
            Pen pn = new Pen(pixelColor, pixelSize);

            for (int i = 0; i < LRFdata.Length; ++i)
            {
                double val = LRFdata[i] * mapScale;

                float x = (float)(centerX + val * -Math.Cos(rad));
                float y = (float)(centerY + val * Math.Sin(rad));

                if (bLineMode)
                {
                    g.DrawLine(pn, (float)centerX, (float)centerY, x, y);
                }
                else
                {
                    g.FillRectangle(brs, x - hPs, y - hPs, hPs + 1, hPs + 1);
                }
                rad += radAdd;
            }

            // 自身の位置を描画
            if( bDrawGide )
            {
                Point[] ps =
                {
                    new Point(0, -3),
                    new Point(3, 5),
                    new Point(0, 0),
                    new Point(-3,5)
                };

                //多角形を描画する
                g.ResetTransform();
                g.TranslateTransform((float)centerX, (float)centerY);

                g.DrawPolygon(Pens.Red, ps);
            }

            g.Dispose();

            // 抜き色指定
            if (bTranparentInverse)
            {
                // ピクセルカラーを抜き
                MapBmp.MakeTransparent(pixelColor);
            }
            else
            {
                // 背景カラーを抜き
                MapBmp.MakeTransparent(baseColor);
            }
            return true;
        }

        /// <summary>
        /// ローカル座標セット
        /// </summary>
        /// <param name="wldPosX"></param>
        /// <param name="wldPosY"></param>
        public void SetLocalPosAng(double lcPosX, double lcPosY, double lcalAng,double dist )
        {
            lcX = lcPosX;
            lcY = lcPosY;
            lcAng = lcalAng;
            lcPivotLength = dist;
        }

        public double GetLocalX()
        {
            return lcX;
        }
        public double GetLocalY()
        {
            return lcY;
        }

        public double GetLocalAng()
        {
            return lcAng;
        }

        /// <summary>
        /// ワールド座標算出
        /// </summary>
        /// <param name="wldParentX"></param>
        /// <param name="wldParentY"></param>
        /// <param name="wldParentAng"></param>
        public void CalcWorldPos(double wldParentX, double wldParentY, double wldParentAng)
        {
            double wRad = wldParentAng * Math.PI / 180.0;
            double cs = Math.Cos(wRad);
            double sn = Math.Sin(wRad);
            /*
            wX = wldParentX + lcX + (lcPivotLength * -Math.Sin(wRad));
            wY = wldParentY + lcY + (lcPivotLength * Math.Cos(wRad));
             * */
            wX = wldParentX + (lcX * cs - (lcY + lcPivotLength) * sn);
            wY = wldParentY + (lcX * sn + (lcY + lcPivotLength) * cs);

            wAng = wldParentAng + lcAng;
        }


        /// <summary>
        /// ローカル座標修正
        /// </summary>
        /// <param name="wldParentX"></param>
        /// <param name="wldParentY"></param>
        /// <param name="wldParentAng"></param>
        public void CalcFixWorldPos(double wldParentX, double wldParentY, double wldParentAng)
        {
            double wRad = -wldParentAng * Math.PI / 180.0;
            double cs = Math.Cos(wRad);
            double sn = Math.Sin(wRad);
            /*
            wX = wldParentX + lcX + (lcPivotLength * -Math.Sin(wRad));
            wY = wldParentY + lcY + (lcPivotLength * Math.Cos(wRad));
             * */
            double fixX = (lcX * cs - lcY* sn);
            double fixY = (lcX * sn + lcY* cs);

            lcX = fixX;
            lcY = fixY;

            wAng = wldParentAng + lcAng;
        }

        /// <summary>
        /// レイヤーデータ保存 書き込み
        /// </summary>
        /// <param name="strm"></param>
        public void Write(BinaryWriter strm)
        {
            strm.Write((int)12);        // 要素数 (可変データは1つと数える)

            strm.Write( lcX );
            strm.Write( lcY );
            strm.Write( lcAng );
            strm.Write( lcPivotLength );
            strm.Write(mapScale);

            strm.Write( centerIndex );
            strm.Write( numDir );
            strm.Write( startDir );
            strm.Write( endDir );

            strm.Write(useFlg);

            strm.Write(LRFdata.Length);

            foreach (var lrfval in LRFdata)
            {
                strm.Write(lrfval);
            }
        }

        /// <summary>
        /// レイヤーデータ　読み込み
        /// </summary>
        /// <param name="strm"></param>
        public void Read(BinaryReader strm)
        {
            strm.ReadInt32(); // 要素数

            lcX = strm.ReadDouble();
            lcY = strm.ReadDouble();
            lcAng = strm.ReadDouble();
            lcPivotLength = strm.ReadDouble();
            mapScale = strm.ReadDouble();

            centerIndex = strm.ReadInt32();
            numDir = strm.ReadInt32();
            strm.ReadDouble();          // startDir 今回は読み込まないデータ
            strm.ReadDouble();          // endDir

            useFlg = strm.ReadBoolean();

            int len = strm.ReadInt32();
            LRFdata = new double[len];

            for (int i = 0; i < len;i++ )
            {
                LRFdata[i] = strm.ReadDouble();
            }
        }
    }
}
