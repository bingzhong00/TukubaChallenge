using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Axiom.Math;

namespace ActiveDetour
{
    /// <summary>
    /// 動的回避
    /// </summary>
    public class ActiveDetour
    {
        // 実距離
        // 80, 5000
        public const double pixelSize = 500;        // 1ピクセルに換算する実サイズ[単位mm] 500mm
        public const double LRFMaxRange = 30000;    // LRF最大距離[単位mm]  30m = 30000mm

        // マップサイズ
        public const int mapWidth = (int)(LRFMaxRange / pixelSize);
        public const int mapHeight = (int)(LRFMaxRange / pixelSize);

        // 中心点　（Map上のLRFの位置）
        public const int centerX = mapWidth / 2;
        public const int centerY = mapHeight * 5 / 6;

        public HexPixelMap hexMap;

        public ActiveDetour(LocationPresumption.GridMap areaMap )
        {
            hexMap = new HexPixelMap(mapWidth, mapHeight);

            // GridMap[100mm間隔]からhexMap[500mm間隔]に変換
            for ( int iH = 0; iH < areaMap.H/5; iH++ )
            {
                for (int iW = 0; iW < areaMap.W/5; iW++)
                {
                    bool bFree = true;

                    // 500mm間隔の中に1つでも通行不可ならば、通行できない
                    for ( int nH = 0; nH<5; nH++)
                    {
                        for (int nW = 0; nW < 5; nW++)
                        {
                            if (areaMap[iW * 5 + nW, iH * 5 + nH] != LocationPresumption.Grid.Free)
                            {
                                bFree = false;
                            }
                        }
                    }

                    if( !bFree )
                    {
                        // 障害物あり
                        hexMap[iW, iH].powVal = 2.0;
                    }
                }

            }
        }

        /// <summary>
        /// 回避チェックポイント生成
        /// </summary>
        /// <returns></returns>
        public List<Vector3> Calc_DetourCheckPoint()
        {
            List<Vector3> resCheckPoint = new List<Vector3>();

            // A Startルート検索
            int GoalX = centerX;
            int GoalY = 0;

            // 回避ルート
            var pList = new List<AStar.Point2>();

            // 斜め移動を許可
            var mgr = new AStar.ANodeMgr(hexMap, GoalX, GoalY, false);

            // スタート地点のノード取得
            // スタート地点なのでコストは「0」
            AStar.ANode node = mgr.OpenNode(centerX, centerY, 0, null);
            mgr.AddOpenList(node);

            // 試行回数。1000回超えたら強制中断
            int cnt = 0;
            while (cnt < 1000)
            {
                mgr.RemoveOpenList(node);
                // 周囲を開く
                mgr.OpenAround(node);
                // 最小スコアのノードを探す.
                node = mgr.SearchMinScoreNodeFromOpenList();
                if (node == null)
                {
                    // 袋小路なのでおしまい.
                    //Debug.Log("Not found path.");
                    break;
                }
                if (node.X == GoalX && node.Y == GoalY)
                {
                    // ゴールにたどり着いた.
                    //Debug.Log("Success.");

                    mgr.RemoveOpenList(node);
                    //node.DumpRecursive();

                    // パスを取得する
                    node.GetPath(pList);

                    // 反転する
                    pList.Reverse();
                    break;
                }

                //yield return new WaitForSeconds(0.01f);
            }


            // 回避チェックポイント生成
            foreach (var p in pList)
            {
                hexMap[p.x, p.y].recomentPow = 1.0;

                // ※移動方向が変わる地点でチェックポイントを作成
                // 差分を求め、違えばチェックポイント生成

                // マップ座標に変換
                //resCheckPoint.Add
            }

            // Bmp生成
            hexMap.UpdateBitmap();

            return resCheckPoint;
        }
    }
}
