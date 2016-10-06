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
        //public const double pixelSize = 500;        // 1ピクセルに換算する実サイズ[単位mm] 500mm
        //public const double LRFMaxRange = 30000;    // LRF最大距離[単位mm]  30m = 30000mm

        // マップサイズ
        //public const int mapWidth = (int)(LRFMaxRange / pixelSize);
        //public const int mapHeight = (int)(LRFMaxRange / pixelSize);

        /// <summary>
        /// 1ドットのスケール
        /// </summary>
        public const int dotScale = 5;

        public HexPixelMap hexMap;

        public ActiveDetour(LocationPresumption.GridMap areaMap )
        {
            // GridMapの1/5 [100mm間隔から、500mmに変換]
            hexMap = new HexPixelMap(areaMap.W / dotScale, areaMap.H / dotScale);

            // GridMap[100mm間隔]からhexMap[500mm間隔]に変換
            for ( int iH = 0; iH < areaMap.H / dotScale; iH++ )
            {
                for (int iW = 0; iW < areaMap.W / dotScale; iW++)
                {
                    bool bFree = true;

                    // 500mm間隔の中に1つでも通行不可ならば、通行できない
                    for ( int nH = 0; nH< dotScale; nH++)
                    {
                        for (int nW = 0; nW < dotScale; nW++)
                        {
                            if (areaMap[iW * dotScale + nW, iH * dotScale + nH] != LocationPresumption.Grid.Free)
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
        /// <param name="startX">スタート位置</param>
        /// <param name="startY"></param>
        /// <param name="targetX">移動目標位置</param>
        /// <param name="targetY"></param>
        /// <returns></returns>
        public List<Vector3> Calc_DetourCheckPoint( int startX, int startY, int targetX, int targetY )
        {
            List<Vector3> resCheckPoint = new List<Vector3>();

            // A Startルート検索
            // 回避ルート
            var pList = new List<AStar.Point2>();

            // 斜め移動を許可
            var mgr = new AStar.ANodeMgr(hexMap, targetX, targetY, false);

            // スタート地点のノード取得
            // スタート地点なのでコストは「0」
            AStar.ANode node = mgr.OpenNode(startX, startY, 0, null);
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

                if (node.X == targetX && node.Y == targetY)
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
            if( pList.Count > 2)
            {
                AStar.Point2 dif,prev;
                dif.x = pList[1].x - pList[0].x;
                dif.y = pList[1].y - pList[0].y;
                prev = pList[1];

                for( int i=2; i<pList.Count; i++ )
                {
                    AStar.Point2 p = pList[i];
                    hexMap[p.x, p.y].recomentPow = 1.0;

                    // 移動方向が変わる地点でチェックポイントを作成
                    // 差分を求め、違えばチェックポイント生成
                    {
                        AStar.Point2 newDif;
                        newDif.x = p.x - prev.x;
                        newDif.y = p.y - prev.y;

                        if(newDif.x != dif.x || newDif.y != dif.y)
                        {
                            // ローカルエリアマップ座標のチェックポイントを生成
                            resCheckPoint.Add(new Vector3(prev.x * dotScale, prev.y * dotScale, 0.0));
                        }

                        dif.x = newDif.x;
                        dif.y = newDif.y;
                    }
                    prev = p;
                }
            }

            // Bmp生成
            hexMap.UpdateBitmap();

            return resCheckPoint;
        }
    }
}
