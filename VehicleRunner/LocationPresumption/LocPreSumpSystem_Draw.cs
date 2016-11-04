using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Axiom.Math;

namespace LocationPresumption
{
    /// <summary>
    /// 自己位置推定 マップ座標変換計算　クラス
    /// </summary>
    public partial class LocPreSumpSystem
    {
        // --------------------------------------------------------------------------------------------------------------------------------
        // 描画系

            // 処理カウンタ
        public System.Diagnostics.Stopwatch swCNT_Draw = new System.Diagnostics.Stopwatch();

        int locMapDrawCnt = 0;

        /// <summary>
        /// 自己位置情報表示 Bmp更新
        /// </summary>
        public void UpdateLocalizeBitmap(bool bParticle, bool bLineTrace)
        {
            swCNT_Draw.Reset();
            swCNT_Draw.Start();

            //lock (AreaBmp)
            {
                Graphics g = Graphics.FromImage(AreaOverlayBmp);

                // Overlayのスケール
                // エリア座標からオーバーレイエリアへの変換
                float olScale = (float)AreaOverlayBmp.Width / (float)AreaBmp.Width;

                // エリアマップ描画
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(AreaBmp, 0, 0, AreaOverlayBmp.Width, AreaOverlayBmp.Height);

                // パーティクル描画
                int size = 10;
                if (bParticle)
                {
                    for (int i = 0; i < Particles.Count; i++)
                    {
                        var p = Particles[i];
                        DrawMaker(g, olScale, p.Location, Brushes.LightBlue, 5);
                    }
                }

                // リアルタイム軌跡描画
                if (bLineTrace)
                {
                    DrawMakerLog_Area(g, olScale, R1Log, Color.Red.R, Color.Red.G, Color.Red.B);
                    DrawMakerLog_Area(g, olScale, V1Log, Color.Cyan.R, Color.Cyan.G, Color.Cyan.B);
                    DrawMakerLog_Area(g, olScale, E1Log, Color.Purple.R, Color.Purple.G, Color.Purple.B);
                    DrawMakerLog_Area(g, olScale, G1Log, Color.Green.R, Color.Green.G, Color.Green.B);
                }

                // 描画順を常にかえて、重なっても見えるようにする
                for (int i = 0; i < 5; i++)
                {
                    switch ((i + locMapDrawCnt) % 5)
                    {
                        case 0:
                            // RE想定ロボット位置描画
                            DrawMaker_Area(g, olScale, E1, Brushes.Purple, size);
                            break;
                        case 1:
                            // PF想定ロボット位置描画
                            DrawMaker_Area(g, olScale, V1, Brushes.Cyan, size);
                            break;
                        case 2:
                            // 実ロボット想定位置描画
                            DrawMaker_Area(g, olScale, R1, Brushes.Red, size);
                            break;
                        case 3:
                            // GPS位置描画
                            if (bEnableDirGPS)
                            {
                                DrawMaker_Area(g, olScale, G1, Brushes.Green, size);
                            }
                            else
                            {
                                DrawMakerND_Area(g, olScale, G1, Brushes.Green, size);
                            }
                            break;
                        case 4:
                            // RE想定ロボット位置描画
                            DrawMaker_Area(g, olScale, E2, Brushes.LightSalmon, size);
                            break;
                    }
                }

                g.Dispose();
            }

            locMapDrawCnt++;

            swCNT_Draw.Stop();
        }

        /// <summary>
        /// マーカー描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="robot"></param>
        /// <param name="brush"></param>
        /// <param name="size"></param>
        private void DrawMaker_Area(Graphics g, float fScale, MarkPoint robot, Brush brush, int size)
        {
            double mkX = worldMap.GetAreaX(robot.X) * fScale;
            double mkY = worldMap.GetAreaY(robot.Y) * fScale;
            double mkDir = robot.Theta - 90.0;

            var P1 = new PointF(
                (float)(mkX + size * Math.Cos(mkDir * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin(mkDir * Math.PI / 180.0)));
            var P2 = new PointF(
                (float)(mkX + size * Math.Cos((mkDir - 150) * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin((mkDir - 150) * Math.PI / 180.0)));
            var P3 = new PointF(
                (float)(mkX + size * Math.Cos((mkDir + 150) * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin((mkDir + 150) * Math.PI / 180.0)));

            g.FillPolygon(brush, new PointF[] { P1, P2, P3 });
        }

        private void DrawMakerND_Area(Graphics g, float fScale, MarkPoint robot, Brush brush, int size)
        {
            double mkX = worldMap.GetAreaX(robot.X) * fScale;
            double mkY = worldMap.GetAreaY(robot.Y) * fScale;
            double mkDir = 0.0;

            size /= 2;
            var P1 = new PointF(
                (float)(mkX + size * Math.Cos(mkDir * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin(mkDir * Math.PI / 180.0)));
            var P2 = new PointF(
                (float)(mkX + size * Math.Cos((mkDir + 90) * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin((mkDir + 90) * Math.PI / 180.0)));
            var P3 = new PointF(
                (float)(mkX + size * Math.Cos((mkDir + 180) * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin((mkDir + 180) * Math.PI / 180.0)));
            var P4 = new PointF(
                (float)(mkX + size * Math.Cos((mkDir + 270) * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin((mkDir + 270) * Math.PI / 180.0)));

            g.FillPolygon(brush, new PointF[] { P1, P2, P3, P4 });
        }

        /// <summary>
        /// マーカー描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="fScale"></param>
        /// <param name="robot"></param>
        /// <param name="brush"></param>
        /// <param name="size"></param>
        static public void DrawMaker(Graphics g, float fScale, MarkPoint robot, Brush brush, int size)
        {
            double mkX = robot.X * fScale;
            double mkY = robot.Y * fScale;
            double mkDir = robot.Theta - 90.0;

            var P1 = new PointF(
                (float)(mkX + size * Math.Cos(mkDir * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin(mkDir * Math.PI / 180.0)));
            var P2 = new PointF(
                (float)(mkX + size * Math.Cos((mkDir - 150) * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin((mkDir - 150) * Math.PI / 180.0)));
            var P3 = new PointF(
                (float)(mkX + size * Math.Cos((mkDir + 150) * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin((mkDir + 150) * Math.PI / 180.0)));

            g.FillPolygon(brush, new PointF[] { P1, P2, P3 });
        }

        static public void DrawMaker(Graphics g, Brush brush, double mkX, double mkY, double size)
        {
            double mkDir = 0;
            size *= 0.5;

            var P1 = new PointF(
                (float)(mkX + size * Math.Cos(mkDir * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin(mkDir * Math.PI / 180.0)));
            var P2 = new PointF(
                (float)(mkX + size * Math.Cos((mkDir + 90) * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin((mkDir + 90) * Math.PI / 180.0)));
            var P3 = new PointF(
                (float)(mkX + size * Math.Cos((mkDir + 180) * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin((mkDir + 180) * Math.PI / 180.0)));
            var P4 = new PointF(
                (float)(mkX + size * Math.Cos((mkDir + 270) * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin((mkDir + 270) * Math.PI / 180.0)));

            g.FillPolygon(brush, new PointF[] { P1, P2, P3, P4 });
        }

        static int worldMapDrawCnt = 0;
        /// <summary>
        /// ワールドマップ上のマーカー描画
        /// </summary>
        public void DrawWorldMap(Graphics g, float viewScale)
        {
            int mkSize = 8;
            // 描画順を常にかえて、重なっても見えるようにする
            for (int i = 0; i < 5; i++)
            {
                switch ((i + worldMapDrawCnt) % 5)
                {
                    case 0:
                        // RE位置描画
                        DrawMaker(g, viewScale, E1, Brushes.Purple, mkSize);
                        break;
                    case 1:
                        // PF位置描画
                        DrawMaker(g, viewScale, V1, Brushes.Cyan, mkSize);
                        break;
                    case 2:
                        // 実ロボット想定位置描画
                        DrawMaker(g, viewScale, R1, Brushes.Red, mkSize);
                        break;
                    case 3:
                        // GPS位置描画
                        if (bEnableDirGPS)
                        {
                            // USB GPS
                            DrawMaker(g, viewScale, G1, Brushes.Green, mkSize);
                        }
                        else
                        {
                            // bServer GPS角度なし
                            DrawMaker(g, Brushes.Green, G1.X * viewScale, G1.Y * viewScale, mkSize);
                        }
                        break;
                    case 4:
                        // RE Pulse位置描画
                        DrawMaker(g, viewScale, E2, Brushes.LightSalmon, mkSize);
                        break;
                }
            }

            // エリア枠描画
            g.DrawRectangle(Pens.Pink,
                             (worldMap.WldOffset.x * viewScale),
                             (worldMap.WldOffset.y * viewScale),
                             (worldMap.AreaGridSize.w * viewScale),
                             (worldMap.AreaGridSize.h * viewScale));

            worldMapDrawCnt++;
        }


        // ログ画像　描画 ------------------------------------------------------------------------------------------------
        #region "LogMap描画"

        /// <summary>
        /// ログ軌跡描画　ローカルエリアに変換
        /// </summary>
        /// <param name="g"></param>
        /// <param name="fScale"></param>
        /// <param name="mkLog"></param>
        /// <param name="colR"></param>
        /// <param name="colG"></param>
        /// <param name="colB"></param>
        private void DrawMakerLog_Area(Graphics g, float fScale, List<MarkPoint> mkLog, byte colR, byte colG, byte colB)
        {
            if (mkLog.Count < 2) return;

            int baseIdx = 0;
            int drawNum = mkLog.Count;

            if (drawNum > LogLine_maxDrawNum)
            {
                baseIdx = (mkLog.Count - 1) - LogLine_maxDrawNum;
                drawNum = LogLine_maxDrawNum;
            }

            Point[] ps = new Point[drawNum];

            for (int i = 0; i < drawNum; i++)
            {
                ps[i].X = (int)(worldMap.GetAreaX((int)mkLog[baseIdx + i].X) * fScale);
                ps[i].Y = (int)(worldMap.GetAreaY((int)mkLog[baseIdx + i].Y) * fScale);
            }

            //折れ線を引く
            g.DrawLines(new Pen(Color.FromArgb(colR, colG, colB)), ps);
        }

        /// <summary>
        /// ログ軌跡描画　ワールド
        /// </summary>
        /// <param name="g"></param>
        /// <param name="mkLog"></param>
        /// <param name="colR"></param>
        /// <param name="colG"></param>
        /// <param name="colB"></param>
        private void DrawMakerLogLine_World(Graphics g, List<MarkPoint> mkLog, byte colR, byte colG, byte colB)
        {
            Point[] ps = new Point[mkLog.Count];

            if (mkLog.Count <= 1) return;

            for (int i = 0; i < mkLog.Count; i++)
            {
                ps[i].X = (int)mkLog[i].X;
                ps[i].Y = (int)mkLog[i].Y;
            }

            //折れ線を引く
            g.DrawLines(new Pen(Color.FromArgb(colR, colG, colB)), ps);
        }

        /// <summary>
        /// ログ保存用　ワールドマップへの軌跡画像生成
        /// </summary>
        /// <returns></returns>
        public Bitmap MakeMakerLogBmp(bool bPointOn, MarkPoint marker)
        {
            if (R1Log.Count <= 0) return null;  // データなし

            Bitmap logBmp = new Bitmap(worldMap.mapBmp);
            Graphics g = Graphics.FromImage(logBmp);

            // 軌跡描画
            // 自己位置
            DrawMakerLogLine_World(g, R1Log, Color.Red.R, Color.Red.G, Color.Red.B);
            // パーティクルフィルター 自己位置推定
            DrawMakerLogLine_World(g, V1Log, Color.Cyan.R, Color.Cyan.G, Color.Cyan.B);
            // ロータリーエンコーダ座標
            DrawMakerLogLine_World(g, E1Log, Color.Purple.R, Color.Purple.G, Color.Purple.B);
            // GPS座標
            DrawMakerLogLine_World(g, G1Log, Color.Green.R, Color.Green.G, Color.Green.B);

            // 最終地点にマーカ表示
            if (R1Log.Count > 0)
            {
                DrawMaker_Area(g, 1.0f, R1Log[R1Log.Count - 1], Brushes.Red, 4);
            }
            if (V1Log.Count > 0)
            {
                DrawMaker_Area(g, 1.0f, V1Log[V1Log.Count - 1], Brushes.Cyan, 4);
            }
            if (E1Log.Count > 0)
            {
                DrawMaker_Area(g, 1.0f, E1Log[E1Log.Count - 1], Brushes.Purple, 4);
            }
            if (G1Log.Count > 0)
            {
                DrawMaker_Area(g, 1.0f, G1Log[G1Log.Count - 1], Brushes.Green, 4);
            }
            if (null != marker)
            {
                DrawMaker_Area(g, 1.0f, marker, Brushes.GreenYellow, 4);
            }

            // 一定期間ごとの位置と向き
            if (bPointOn)
            {
                // 自己位置
                //foreach (var p in R1Log)
                for (int i = 0; i < R1Log.Count; i++)
                {
                    if ((i % 30) != 0) continue;
                    DrawMaker_Area(g, 1.0f, R1Log[i], Brushes.Red, 4);
                }


                // LRF パーティクルフィルター
                //foreach (var p in V1Log)
                for (int i = 0; i < V1Log.Count; i++)
                {
                    if ((i % 30) != 0) continue;
                    DrawMaker_Area(g, 1.0f, V1Log[i], Brushes.Cyan, 3);
                }


                // ロータリーエンコーダ座標
                //foreach (var p in E1Log)
                for (int i = 0; i < E1Log.Count; i++)
                {
                    if ((i % 30) != 0) continue;
                    DrawMaker_Area(g, 1.0f, E1Log[i], Brushes.Purple, 4);
                }
            }

            g.Dispose();
            return logBmp;
        }
        #endregion
    }
}