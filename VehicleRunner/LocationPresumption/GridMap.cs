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

using System.Drawing;
using System.Drawing.Imaging;

namespace LocationPresumption
{
    public enum Grid {
        Unknown = 0,
        Free      = 0x0000000F,  // 通行可能エリア
        Fill      = 0x000000F0,  // 静的障害物
        RangeOver = 0x00000F0F,  // レンジ外(通行可能属性を持つ)

        RedArea   = (0x00010000 | Fill),   // 異常地帯（壁の中 等）
        BlueArea  = (0x00020000 | Free),   // スローダウンエリア
        GreenArea = (0x00040000 | Free),   // 位置補正エリア
    }

    public class GridMap {
        public int W;
        public int H;
        public Grid[,] M;
        Bitmap img;

        public static Color GridColor_Fill = Color.FromArgb(0, 0, 0);
        public static Color GridColor_Free = Color.FromArgb(255, 255, 255);

        public static Color GridColor_RedArea = Color.FromArgb(255, 0, 0);
        public static Color GridColor_GreenArea = Color.FromArgb(0, 255, 0);
        public static Color GridColor_BlueArea = Color.FromArgb(0, 0, 255);

        /// <summary>
        /// アクセス関数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Grid this[int x, int y]
        {
            get {
                if (0 <= x && x < W && 0 <= y && y < H) {
                    return M[x, y];
                }
                // 画面外
                //return Grid.RangeOver;
                return Grid.Free;
            }

            set {
                M[x, y] = value;
            }
        }

        public GridMap(int w, int h)
        {
            W = w;
            H = h;
            M = new Grid[W, H];

            for (int x = 0; x < W; ++x) {
                for (int y = 0; y < H; ++y) {
                    this[x, y] = Grid.Free;
                }
            }

            img = new Bitmap(W, H);
        }

        /// <summary>
        /// bmpからGridMapを生成
        /// </summary>
        /// <param name="bitmap"></param>
        public GridMap(Bitmap bitmap)
        {
            W = bitmap.Width;
            H = bitmap.Height;
            M = new Grid[W, H];

            BitmapAccess access = new BitmapAccess(bitmap);
            access.BeginAccess();

            // bmpからGridMapを生成
            for (int x = 0; x < W; ++x) {
                for (int y = 0; y < H; ++y) {
                    //Color c = access[x, H-1-y];
                    Color c = access[x, y];
                    if (c == GridColor_Fill)
                    {
                        // 障害物
                        this[x, y] = Grid.Fill;
                    }
                    else if (c == GridColor_Free)
                    {
                        // 何もないところ
                        this[x, y] = Grid.Free;
                    }
                    else if (c == GridColor_RedArea)
                    {
                        // 異常地帯　壁の中
                        this[x, y] = Grid.RedArea;
                    }
                    else if (c == GridColor_GreenArea)
                    {
                        // 位置補正エリア
                        this[x, y] = Grid.GreenArea;
                    }
                    else if (c == GridColor_BlueArea)
                    {
                        // スローエリア
                        this[x, y] = Grid.BlueArea;
                    }
                    else
                    {
                        // それ以外
                        this[x, y] = Grid.Unknown;
                    }
                }
            }

            access.EndAccess();
        }

        /// <summary>
        /// GridMapからBMP更新
        /// </summary>
        /// <returns></returns>
        public Bitmap UpdateBitmap()
        {
            BitmapAccess MapAccess = new BitmapAccess(img);

            MapAccess.BeginAccess();
            for (int x = 0; x < W; ++x)
            {
                for (int y = 0; y < H; ++y)
                {
                    Color cl = Color.Yellow;
                    switch (this[x, y])
                    {
                        case Grid.Fill:
                            cl = Color.Black;
                            break;
                        case Grid.Free:
                            cl = Color.White;
                            break;
                        case Grid.RedArea:
                            cl = Color.Red;
                            break;
                        case Grid.BlueArea:
                            cl = Color.Blue;
                            break;
                        case Grid.GreenArea:
                            cl = Color.Green;
                            break;
                        case Grid.Unknown:
                            cl = Color.LightGray;
                            break;
                    }
                    MapAccess[x, y] = cl;
                }
            }
            MapAccess.EndAccess();

            return img;
        }

        /// <summary>
        /// GridMapに障害物の線を追加
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <param name="size"></param>
        public void DrawLine(int startX, int startY, int endX, int endY, int size) {
            LineDelegate f = delegate(GridMap map, int x, int y) {
                if (size == 0) {
                    map[x, y] = Grid.Fill;
                } else {
                    // 太さ(size)分　グリッドを埋める
                    for (int dx = -size; dx < size; ++dx) {
                        for (int dy = -size; dy < size; ++dy) {
                            if (0 < x + dx && x + dx < W && 0 < y + dy && y + dy < H) {
                                map[x + dx, y + dy] = Grid.Fill;
                            }
                        }
                    }
                }
            };
            DrawLine(startX, startY, endX, endY, f);
        }


        public delegate void LineDelegate(GridMap map, int x, int y);

        public void DrawLine(int startX, int startY, int endX, int endY, LineDelegate f)
        {
            int deltaX = 2 * (endX - startX);
            int deltaY = 2 * (endY - startY);
            int stepX = deltaX < 0 ? -1 : 1;
            int stepY = deltaY < 0 ? -1 : 1;
            int nextX = startX;
            int nextY = startY;
            int fraction;
            deltaX = Math.Abs(deltaX);
            deltaY = Math.Abs(deltaY);

            f(this, startX, startY);

            if (deltaX > deltaY) {
                fraction = deltaY - deltaX / 2;
                while (nextX != endX) {
                    if (fraction >= 0) {
                        nextY += stepY;
                        fraction -= deltaX;
                        f(this, nextX, nextY);
                    }
                    nextX += stepX;
                    fraction += deltaY;
                    //処理
                    f(this, nextX, nextY);
                }
            } else {
                fraction = deltaX - deltaY / 2;
                while (nextY != endY) {
                    if (fraction >= 0) {
                        nextX += stepX;
                        fraction -= deltaY;
                        f(this, nextX, nextY);
                    }
                    nextY += stepY;
                    fraction += deltaX;
                    //処理
                    f(this, nextX, nextY);
                }
            }
        }

        /// <summary>
        /// 障害物までの距離を計算
        /// </summary>
        /// <param name="startX">開始位置x</param>
        /// <param name="startY">開始位置Y</param>
        /// <param name="dx">移動量x</param>
        /// <param name="dy">移動量y</param>
        /// <param name="max">最大距離</param>
        /// <returns></returns>
        public double MeasureDist(double startX, double startY, double dx, double dy, double max)
        {
            double x = startX;
            double y = startY;
            double max2 = max * max;
            double d2 = 0;

            while ((this[(int)(x+0.5), (int)(y+0.5)] & Grid.Free) != 0 && d2 < max2)
            {
                x += dx;
                y += dy;
                d2 = (startX - x) * (startX - x) + (startY - y) * (startY - y);
            }
            return Math.Sqrt(d2);

        }

        /*
        public double MeasureDist_Fast(double startX, double startY, double dx, double dy, double max)
        {
            double x = startX;
            double y = startY;
            double max2 = (double)(max * max);
            double d2 = 0;

            double ax, ay;

            if (dx == 0.0) cx = 0;
            else if (dx == 0.0) cx = 0;
            {
                cx = 1.0 / dx;
            }

            while (this[(int)x, (int)y] == Grid.Free && d2 < max2)
            {
                x += dx;
                y += dy;
                d2 = (startX - x) * (startX - x) + (startY - y) * (startY - y);
            }
            return Math.Sqrt(d2);
        }*/

    }
}
