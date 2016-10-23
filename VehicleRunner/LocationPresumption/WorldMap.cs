using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;

namespace LocationPresumption
{
    public class WorldMap
    {
        // マップ管理のすべてをもつ
        // Form1.csでMapに関連するものをこちらにもちこみ階層で管理する

        // 他のクラスも計算はすべてワールド座標
        // MapのLRFを取得するクラスを内臓し、その中はローカル座標で計算

        // ※動的なエリアマップのスケール変換　　計算量軽減重視、精度重視　モード切り替え

        public class PointI
        {
            public int x;
            public int y;

            public PointI()
            {
                x = 0;
                y = 0;
            }
        }

        public class SizeI
        {
            public int w;
            public int h;

            public SizeI()
            {
                w = 0;
                h = 0;
            }
        }


        public class PointD
        {
            public double x;
            public double y;

            public PointD()
            {
                x = 0;
                y = 0;
            }
        }

        // ----------------------------------------------
        public Bitmap mapBmp;
        public GridMap AreaGridMap;

        public PointI WldOffset = new PointI();
        //public PointI WldOffsetDiff = new PointI();

        public SizeI AreaGridSize = new SizeI();
        public SizeI WorldSize = new SizeI();

        /// <summary>
        /// ワールドマップ初期化
        /// </summary>
        /// <param name="mapFname">ワールドBMPファイル名</param>
        public WorldMap( string mapFname )
        {
            WldOffset = new PointI();
            AreaGridSize = new SizeI();
            mapBmp = new Bitmap(mapFname);

            WorldSize.w = mapBmp.Width;
            WorldSize.h = mapBmp.Height;
        }

        /// <summary>
        /// エリア作成
        /// </summary>
        /// <param name="areaW">エリア幅</param>
        /// <param name="areaH"></param>
        public void InitArea( int areaW, int areaH )
        {
            AreaGridSize.w = areaW;
            AreaGridSize.h = areaH;

            UpdateArea(0, 0);
        }

        /// <summary>
        /// エリア更新
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void UpdateArea(int wldX, int wldY)
        {

            //WldOffsetDiff.x = WldOffset.x - wldX;
            //WldOffsetDiff.y = WldOffset.y - wldY;

            WldOffset.x = wldX;
            WldOffset.y = wldY;

            MakeAreaGridMap();
        }

        /// <summary>
        /// エリア更新　中心点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void UpdateAreaCenter(int wldX, int wldY)
        {
            UpdateArea(wldX - (AreaGridSize.w / 2), wldY - (AreaGridSize.h / 2));
        }

        /// <summary>
        /// エリア座標取得
        /// </summary>
        /// <param name="worldX"></param>
        /// <returns></returns>
        public int GetAreaX(int worldX)
        {
            return (worldX - WldOffset.x);
        }
        public int GetAreaY(int worldY)
        {
            return (worldY - WldOffset.y);
        }
        public double GetAreaX(double worldX)
        {
            return (worldX - (double)WldOffset.x);
        }
        public double GetAreaY(double worldY)
        {
            return (worldY - (double)WldOffset.y);
        }

        /// <summary>
        /// エリア座標からワールド座標変換
        /// </summary>
        /// <param name="areaX"></param>
        /// <returns></returns>
        public int GetWorldX(int areaX)
        {
            return (areaX + WldOffset.x);
        }
        public int GetWorldY(int areaY)
        {
            return (areaY + WldOffset.y);
        }
        public double GetWorldX(double areaX)
        {
            return (areaX + (double)WldOffset.x);
        }
        public double GetWorldY(double areaY)
        {
            return (areaY + (double)WldOffset.y);
        }

        /// <summary>
        /// ローカルエリア内に収まっているか？
        /// </summary>
        /// <param name="wldX"></param>
        /// <param name="wldY"></param>
        /// <returns></returns>
        public bool IsInArea(int wldX, int wldY )
        {
            int laX = GetAreaX(wldX);
            int laY = GetAreaY(wldY);

            if ((laX < 0 || laX >= AreaGridSize.w) ||
                (laY < 0 || laY >= AreaGridSize.h))
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// GetAreaMapInfo()
        /// </summary>
        /// <param name="wldX"></param>
        /// <param name="wldY"></param>
        /// <returns></returns>
        public Grid GetAreaMapInfo(int wldX, int wldY)
        {
            if (!IsInArea(wldX, wldY)) return Grid.RangeOver;

            return AreaGridMap[GetAreaX(wldX), GetAreaY(wldY)];
        }

        public Grid GetAreaMapInfo(double wldX, double wldY)
        {
            return GetAreaMapInfo((int)(wldX+0.5),(int)(wldY+0.5));
        }

        /// <summary>
        /// ワールドBMPからエリアのGridMapを作成
        /// </summary>
        /// <returns></returns>
        public GridMap MakeAreaGridMap()
        {
            GridMap gm = new GridMap(AreaGridSize.w, AreaGridSize.h);

            BitmapAccess bmpAp = new BitmapAccess(mapBmp);
            bmpAp.BeginAccess();

            // bmpからGridMapを生成
            for (int x = 0; x < gm.W; ++x)
            {
                for (int y = 0; y < gm.H; ++y)
                {
                    Color c;
                    int adX = WldOffset.x + x;
                    int adY = WldOffset.y + y;

                    if (adX < 0 || adX >= mapBmp.Width ||
                        adY < 0 || adY >= mapBmp.Height)
                    {
                        // レンジ外
                        c = GridMap.GridColor_Fill;
                    }
                    else
                    {
                        c = bmpAp[adX, adY];
                    }

                    if (c == GridMap.GridColor_Fill)
                    {
                        // 障害物
                        gm.M[x, y] = Grid.Fill;
                    }
                    else if (c == GridMap.GridColor_Free)
                    {
                        // 通行可能
                        gm.M[x, y] = Grid.Free;
                    }
                    else if (c == GridMap.GridColor_RedArea)
                    {
                        // 通行不可　壁の中
                        gm.M[x, y] = Grid.RedArea;
                    }
                    else if (c == GridMap.GridColor_GreenArea)
                    {
                        // 通行可能 位置補正推奨
                        gm.M[x, y] = Grid.GreenArea;
                    }
                    else if (c == GridMap.GridColor_BlueArea)
                    {
                        // 通行可能　スローダウン
                        gm.M[x, y] = Grid.BlueArea;
                    }
                    else
                    {
                        // それ以外
                        gm.M[x, y] = Grid.Unknown;
                    }
                }
            }

            bmpAp.EndAccess();

            AreaGridMap = gm;

            return gm;
        }

    }
}
