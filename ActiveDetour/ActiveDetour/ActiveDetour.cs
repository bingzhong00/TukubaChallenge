using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using SCIP_library;

/*
 * Todo
 * ・ロータリーエンコーダから、自車の現在の速度を取り出したい（物体判定用）
 * 
 */

namespace ActiveDetour
{

    public class ActiveDetour
    {
        public URG_LRF urgLRF;

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

        // 緊急ブレーキ距離
        public const double EmgBrakgeRange = 1300.0;

        bool EmgBrakeFlg = false;
        int EmgBrakeCnt = 0;

        // 
        int AccelLowCnt = 0;            // スローダウンカウント
        double AccelValue = 0.0;        // アクセル値
        const double AccelMax = 0.7;    // アクセル最大値

        double HandleValue = 0.0;        // ハンドル値
        double HandleRangePer = 0.4;    // ハンドル値調整

        public HexPixelMap hexMap;
        Random r = new Random();

        public bool LRF_IPConnectFlg = false;

        public ActiveDetour()
        {
            hexMap = new HexPixelMap(mapWidth, mapHeight);
           
            
            urgLRF = new URG_LRF();

            //LRF_IPConnectFlg = urgLRF.IpOpen();
            urgLRF.LogFileOpen(Path.GetDirectoryName( Application.ExecutablePath)+"\\urgLog20150815.txt",1);
        }

        public bool ConnectLRF()
        {
            urgLRF.Close();

            LRF_IPConnectFlg = urgLRF.IpOpen();
            return LRF_IPConnectFlg;
        }


//        double[] oldLRFdata = new double[1080];
//        public double LRGMoveAvg;

        /// <summary>
        /// LRF情報 更新
        /// </summary>
        public void UpdateLRF()
        {
            double[] LRFdata = urgLRF.getScanData();

            /*
            if (LRFdata.Length > oldLRFdata.Length)
            {
                oldLRFdata = new double[LRFdata.Length];
            }*/

            // LRF to Map
            {
                double startDir = -135.0;
                double endDir = 135.0;
                //double numDir = 1080;
                double scale = 1.0 / pixelSize;

                double dirRange = (endDir - startDir);
                double radAdd = (dirRange / LRFdata.Length) * (Math.PI / 180.0);
                double rad = (startDir - 90.0) * (Math.PI / 180.0);

                int centerX = hexMap.W / 2;
                int centerY = hexMap.H * 3 / 4;

                //double rGTms = (20/1000);   // 取得間隔ms

                //double calcAvg = 0;
                //int calcAvgCnt = 0;

                //hexMap[centerX, centerY].marker = 1.0;
                
                for (int i = 0; i < LRFdata.Length; ++i)
                {
                    double lrf = LRFdata[i];
                    // エマージェンジーブレーキ判定
                    {
                        double ebAng = (rad * 180.0 / Math.PI) + 90.0;

                        if (ebAng > -20.0 && ebAng < 20.0)
                        {
                            if (lrf < EmgBrakgeRange)
                            {
                                EmgBrakeON();
                            }
                        }
                    }

                    // 有効範囲内のデータをマップに置き換え
                    if (lrf < LRFMaxRange && lrf > pixelSize*1.5)
                    {
                        double val = lrf * scale;
                        double addPow = 0.25;
                        int x = centerX + (int)-(val * Math.Cos(rad) + 0.5);
                        int y = centerY + (int)(val * Math.Sin(rad) + 0.5);

                        x = Math.Max(Math.Min(x, hexMap.W - 1), 0);
                        y = Math.Max(Math.Min(y, hexMap.H - 1), 0);

                        //hexMap[x, y].powVal += 0.25;

                        /*
                        // 傾向分析
                        // 自車は最高時速4KM　秒速1000mm
                        if( null != oldLRFdata )
                        {
                            double lrfDiff = oldLRFdata[i] - lrf;
                            if ( Math.Abs(lrfDiff) > 5000.0*rGTms )
                            {
                                // 5000mm以上の差はノイズ
                                addPow *= 0.5;
                                hexMap[x, y].markerType = HexPixel.MT.Noise;
                            }
                            else if (lrfDiff > 3000.0 * rGTms)
                            {
                                // 自分に近づく物体(危険)
                                // [自分の移動量(1000mm)より多く移動している]
                                addPow += 1.0;
                                hexMap[x, y].markerType = HexPixel.MT.Approching;
                            }
                            else
                            {
                                if (lrfDiff > LRGMoveAvg * 1.3 || lrfDiff < LRGMoveAvg * 0.7)
                                {
                                    // 移動してる物体
                                    addPow *= 2.0;
                                    hexMap[x, y].markerType = HexPixel.MT.Mover;
                                }
                            }

                            {
                                // 動かないもの
                                calcAvg += lrfDiff;
                                calcAvgCnt++;
                            }
                         
                        }
                         * */

                        hexMap[x, y].powVal += addPow;
                    }

                    //oldLRFdata[i] = lrf;
                    rad += radAdd;
                }

                // 移動量平均
                /*
                if (calcAvgCnt > 0)
                {
                    LRGMoveAvg = calcAvg / (double)calcAvgCnt;
                }
                */
                // レンジ範囲外
                // 1mの壁で埋める
                int nnum = (int)(((double)LRFdata.Length / dirRange) * (360.0 - dirRange));
                for (int i = 0; i < nnum; ++i)
                {
                    double val = 3000.0 * scale;
                    int x = centerX + (int)-(val * Math.Cos(rad) + 0.5);
                    int y = centerY + (int)(val * Math.Sin(rad) + 0.5);

                    x = Math.Max(Math.Min(x, hexMap.W - 1), 0);
                    y = Math.Max(Math.Min(y, hexMap.H - 1), 0);

                    hexMap[x, y].powVal += 0.5;
                    rad += radAdd;
                }
            }

            // 減衰
            hexMap.LossGein(0.90);

            //oldLRFdata = LRFdata;
        }

        public void Update()
        {
            //r.Next();
            //hexMap[r.Next(hexMap.W), r.Next(hexMap.H)].powVal = 1.0;

            if (EmgBrakeCnt <= 0) EmgBrakeFlg = false;
            else EmgBrakeCnt--;

            // A Startルート検索
            {
                int GoalX = centerX;
                int GoalY = 0;

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

                foreach (var p in pList)
                {
                    hexMap[p.x, p.y].recomentPow = 1.0;
                }

                // ハンドル動作に変換
                {
                    double hdl = 0.0;
                    int numList = Math.Min(pList.Count-1, 10);

                    for (int i = 0; i < numList; i++)
                    {
                        int dfx = pList[i + 1].x - pList[i].x;

                        // 最初に決めた向き以外は足し込まない
                        if( (dfx < 0 && hdl <= 0.0) || (dfx > 0 && hdl >= 0.0) )
                        {
                            hdl += (double)dfx * ((double)(10-i)*0.1);
                        }
                    }

                    // 調整
                    hdl *= HandleRangePer;

                    // ハンドルを大きくきったら減速
                    if (Math.Abs(hdl) > 0.7)
                    {
                        AccelLowCnt = 100;
                    }

                    // ハードウェア上　左右が逆
                    // +左方向 -右方向
                    HandleValue = -hdl;
                }
            }


            // Bmp生成
            hexMap.UpdateBitmap();
        }

        // ----------------------------------------------------------------------------------
        public void EmgBrakeON()
        {
            EmgBrakeFlg = true;
            AccelLowCnt = 20;
            EmgBrakeCnt = 50;
        }

        public double GetHandle()
        {
            return HandleValue;
        }



        public double GetAccel()
        {
            if (AccelLowCnt > 0)
            {
                AccelValue = AccelMax*0.4;
                AccelLowCnt--;
            }
            else
            {
                if (AccelValue < AccelMax)
                {
                    AccelValue += 0.01;
                }
            }

            return AccelValue;
        }

        public bool GetEmgBrake()
        {
            return EmgBrakeFlg;
        }

        public void SetHandleRange( double per )
        {
            HandleRangePer = per;
        }

        public Bitmap GetBmp()
        {
            return hexMap.img;
        }

    }
}
