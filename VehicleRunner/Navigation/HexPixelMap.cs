using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using LocationPresumption;

namespace ActiveDetourNavigation
{
    public class HexPixel
    {
        // 障害値
        public double powVal;
        // 推奨ルート値
        public double recomentPow;

        public double marker;
        public MT markerType;

        public enum MT : int
        {
            None,
            Approching,
            Mover,
            Noise,
        }

        public HexPixel()
        {
            powVal = 0.0;
            recomentPow = 0.0;
            marker = 0.0;
            markerType = MT.None;
        }
    }


    public class HexPixelMap
    {
        public int W;
        public int H;
        public HexPixel[,] M;
        public Bitmap img = null;

        Point[] drawHexPoint = new Point[6];

        const double MaxPowVal = 8.0;

        /// <summary>
        /// アクセス関数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public HexPixel this[int x, int y]
        {
            get
            {
                if (0 <= x && x < W && 0 <= y && y < H)
                {
                    return M[x, y];
                }
                return null;
            }
            set
            {
                M[x, y] = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public HexPixelMap(int w, int h)
        {
            W = w;
            H = h;
            M = new HexPixel[W, H];

            img = new Bitmap(W * 10, H * 10);

            for (int x = 0; x < W; ++x)
            {
                for (int y = 0; y < H; ++y)
                {
                    this[x, y] = new HexPixel();
                }
            }

            // HexDraw座標点
            {
                double rad;
                double asp = 0.8;
                double dSize = 8.0;

                for (int i = 0; i < 6; i++)
                {
                    rad = ((double)i * (360.0 / 6.0)) * Math.PI / 180.0;
                    drawHexPoint[i].X = (int)(Math.Sin(rad) * dSize * asp + 0.5);
                    drawHexPoint[i].Y = (int)(Math.Cos(rad) * dSize + 0.5);
                }
            }
        }

        /// <summary>
        /// 減衰
        /// </summary>
        /// <param name="per"></param>
        public void LossGein(double per)
        {
            for (int x = 0; x < W; ++x)
            {
                for (int y = 0; y < H; ++y)
                {
                    // 上限
                    this[x, y].powVal = Math.Min(this[x, y].powVal, MaxPowVal);

                    this[x, y].powVal *= per;
                    this[x, y].recomentPow = 0.0;
                }
            }
        }
        /// 座標をインデックスに変換する
        public int ToIdx(int x, int y)
        {
            return x + (y * W);
        }

        /// 領域外かどうかチェックする
        public bool IsOutOfRange(int x, int y)
        {
            if (x < 0 || x >= W) { return true; }
            if (y < 0 || y >= H) { return true; }

            // 領域内
            return false;
        }

        int _outOfRange = -1; // 領域外を指定した時の値

        /// 値の取得
        // @param x X座標
        // @param y Y座標
        // @return 指定の座標の値（領域外を指定したら_outOfRangeを返す）
        public int Get(int x, int y)
        {
            if (IsOutOfRange(x, y))
            {
                return _outOfRange;
            }

            // 2...通行不可
            return ((this[x, y].powVal > 0.5f) ? 2 : 0);
        }

        /*
        /// 値の設定
        // @param x X座標
        // @param y Y座標
        // @param v 設定する値
        public void Set(int x, int y, int v)
        {
            if (IsOutOfRange(x, y))
            {
                // 領域外を指定した
                return;
            }

            _values[y * Width + x] = v;
        }*/


        /// <summary>
        /// BMP更新
        /// </summary>
        /// <returns></returns>
        public Bitmap UpdateBitmap()
        {
            if (null == img) return null;

            Graphics g = Graphics.FromImage(img);
            g.FillRectangle(Brushes.Black, 0, 0, img.Width, img.Height);
            for (int y = 0; y < H; ++y)
            {
                for (int x = 0; x < W; ++x)
                {
                    int colVal;

                    int dx = x * 10 + (2 - (y % 2) * 4);
                    int dy = y * 10;


                    // Draw Pow
                    colVal = Math.Min((int)(this[x, y].powVal * 255.0), 255);
                    if (colVal > 0)
                    {
                        Color col;

                        if (colVal < 200)
                        {
                            colVal = (int)((double)colVal / 200.0 * 255.0);
                            col = Color.FromArgb(colVal / 8, colVal / 8, colVal);
                        }
                        else
                        {
                            //colVal = (int)((double)(255-colVal) / 55.0 * 100.0) + 155;
                            col = Color.FromArgb(colVal, colVal, colVal);
                        }

                        if (this[x, y].markerType == HexPixel.MT.Approching)
                        {
                            // 危険物
                            col = Color.FromArgb(colVal, colVal / 8, colVal/8);
                        }
                        else if (this[x, y].markerType == HexPixel.MT.Mover)
                        {
                            // 移動体
                            col = Color.FromArgb(colVal/8, colVal, colVal / 8);
                        }
                        else if (this[x, y].markerType == HexPixel.MT.Noise)
                        {
                            // Noise
                            col = Color.FromArgb(colVal, colVal/8, colVal);
                        }
                        this[x, y].markerType = HexPixel.MT.None;


                        DrawHex(g, dx, dy, col, 8.0);
                    }


                    // Root
                    colVal = Math.Min((int)(this[x, y].recomentPow * 255.0), 255);
                    if (colVal > 0)
                    {
                        DrawHex(g, dx, dy, Color.FromArgb(colVal, colVal, colVal / 10), 8.0);
                    }

                    // Marker
                    colVal = Math.Min((int)(this[x, y].marker * 255.0), 255);
                    if (colVal > 0)
                    {
                        DrawHex(g, dx, dy, Color.FromArgb(colVal, colVal / 10, colVal / 10), 8.0);
                    }

                }
            }
            g.Dispose();

            return img;
        }

        /// <summary>
        /// 6角形　描画
        /// </summary>
        /// <param name="g">Grphics</param>
        /// <param name="x">中心座標</param>
        /// <param name="y">中心座標</param>
        /// <param name="col">色</param>
        /// <param name="dSize">サイズ</param>
        public void DrawHex(Graphics g, int x, int y, Color col, double dSize, bool andLine = false)
        {
            //Point[] po = new Point[6];
            g.ResetTransform();

            // dSizeが必要ならスケールで対応

            //ワールド変換行列を下に10平行移動する
            g.TranslateTransform(x, y);

            g.FillPolygon(new SolidBrush(col), drawHexPoint);

            /*
            if (andLine)
            {
                Color colLine = Color.FromArgb(80, 80, 80);
                g.DrawPolygon(new Pen(colLine), drawHexPoint);     
            }*/
        }
    }
}
