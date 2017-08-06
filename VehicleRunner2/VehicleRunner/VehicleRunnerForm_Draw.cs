using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;

using Location;
using CersioIO;
using Navigation;
using Axiom.Math;

namespace VehicleRunner
{
    class VehicleRunnerForm_Draw
    {
        /// <summary>自己位置情報　表示BMP</summary>
        private Bitmap worldMapBmp;

        /// <summary>自己位置情報　表示BMP</summary>
        private Bitmap areaMapBmp;

        Font drawFont = new Font("MS UI Gothic", 16, FontStyle.Bold);
        Font fntMini = new Font("MS UI Gothic", 9);


        /// <summary>
        /// マーカー描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="fScale"></param>
        /// <param name="robot"></param>
        /// <param name="brush"></param>
        /// <param name="size"></param>
        static public void DrawMaker(Graphics g, float fScale, Brush brush, DrawMarkPoint robot, int size)
        {
            double mkX = robot.x * fScale;
            double mkY = robot.y * fScale;
            double mkDir = robot.theta;

            DrawMaker(g, brush, mkX, mkY, mkDir, (double)size);
        }

        static public void DrawMaker(Graphics g, Brush brush, DrawMarkPoint robot, int size=8)
        {
            DrawMaker(g, 1.0f, brush, robot, size);
        }

        static public void DrawMaker(Graphics g, float fScale, Brush brush, double mkX, double mkY, double mkDir, int size)
        {
            DrawMaker(g, brush, mkX * fScale, mkY * fScale, mkDir, (double)size);
        }

        static public void DrawMaker(Graphics g, Brush brush, double mkX, double mkY, double mkDir, double size)
        {
            DrawMaker(g, brush, mkX, mkY, mkDir, size, 0.0);
        }

        static public void DrawMaker(Graphics g, Brush brush, double mkX, double mkY, double mkDir, double size, double centerDir)
        {
            //double nonScl = size / ViewScale;
            //if (nonScl <= 1.0) nonScl = 1.0;

            mkDir += centerDir;

            // 枠線
            {
                double sizeBig = size * 1.25;
                //double centerDir = 0;

                g.DrawEllipse(Pens.LightGray,
                               (float)(mkX - sizeBig), (float)(mkY - sizeBig),
                               (float)sizeBig * 2.0f, (float)sizeBig * 2.0f);

                // 0度基準線
                g.DrawLine(Pens.LightGray,
                           (float)mkX, (float)mkY,
                           (float)(mkX + sizeBig * Math.Cos(centerDir)), (float)(mkY + sizeBig * Math.Sin(centerDir)));

            }

            {
                var P1 = new PointF(
                    (float)(mkX + size * Math.Cos(mkDir)),
                    (float)(mkY + size * Math.Sin(mkDir)));
                var P2 = new PointF(
                    (float)(mkX + size * Math.Cos(mkDir - (150.0 * Math.PI / 180.0))),
                    (float)(mkY + size * Math.Sin(mkDir - (150.0 * Math.PI / 180.0))));
                var P3 = new PointF(
                    (float)(mkX + size * Math.Cos(mkDir + (150.0 * Math.PI / 180.0))),
                    (float)(mkY + size * Math.Sin(mkDir + (150.0 * Math.PI / 180.0))));

                g.FillPolygon(brush, new PointF[] { P1, P2, P3 });
            }
        }

        /// <summary>
        /// 向きのないマーカー
        /// ひし形
        /// </summary>
        /// <param name="g"></param>
        /// <param name="brush"></param>
        /// <param name="mkX"></param>
        /// <param name="mkY"></param>
        /// <param name="size"></param>
        static public void DrawMakerNoDir(Graphics g, Brush brush, double mkX, double mkY, int _size)
        {
            double mkDir = 0;
            double size = (double)_size * 0.5;

            var P1 = new PointF(
                (float)(mkX + size * Math.Cos(mkDir)),
                (float)(mkY + size * Math.Sin(mkDir)));
            var P2 = new PointF(
                (float)(mkX + size * Math.Cos(mkDir + (90.0 * Math.PI / 180.0))),
                (float)(mkY + size * Math.Sin(mkDir + (90.0 * Math.PI / 180.0))));
            var P3 = new PointF(
                (float)(mkX + size * Math.Cos(mkDir + (180.0 * Math.PI / 180.0))),
                (float)(mkY + size * Math.Sin(mkDir + (180.0 * Math.PI / 180.0))));
            var P4 = new PointF(
                (float)(mkX + size * Math.Cos(mkDir + (270.0 * Math.PI / 180.0))),
                (float)(mkY + size * Math.Sin(mkDir + (270.0 * Math.PI / 180.0))));

            g.FillPolygon(brush, new PointF[] { P1, P2, P3, P4 });
        }

        static public void DrawMakerNoDir(Graphics g, float fScale, Brush brush, double mkX, double mkY, int size)
        {
            DrawMakerNoDir(g, brush, mkX * fScale, mkY * fScale, size);
        }

        static public void DrawMakerNoDir(Graphics g, Brush brush, DrawMarkPoint robot, int size = 8)
        {
            DrawMakerNoDir(g, brush, robot.x, robot.y, size);
        }


        private void DrawString(Graphics g, int x, int y, string str, Brush brush, Brush bkBrush)
        {
            g.DrawString(str, drawFont, bkBrush, x + 1, y);
            g.DrawString(str, drawFont, bkBrush, x - 1, y);
            g.DrawString(str, drawFont, bkBrush, x, y - 1);
            g.DrawString(str, drawFont, bkBrush, x, y + 1);
            g.DrawString(str, drawFont, brush, x, y);
        }


        private void DrawMakerLine(Graphics g, float fScale, DrawMarkPoint robotA, DrawMarkPoint robotB, Pen pen, int size)
        {
            var P1 = new PointF(
                (float)(robotA.x * fScale),
                (float)(robotA.y * fScale));
            var P2 = new PointF(
                (float)(robotB.x * fScale),
                (float)(robotB.y * fScale));

            g.DrawLine(pen, P1, P2);
        }


        /// <summary>
        /// PictureBoxのサイズに合わせた　ワールドマップBMP作成
        /// </summary>
        /// <param name="worldBMP"></param>
        /// <returns></returns>
        public Bitmap MakePictureBoxWorldMap(Bitmap worldBMP, PictureBox picbox_AreaMap)
        {
            float viewScale = 1.0f;

            if (((float)worldBMP.Width / (float)picbox_AreaMap.Width) < ((float)worldBMP.Height / (float)picbox_AreaMap.Height))
            {
                viewScale = (float)(1.0 / ((float)worldBMP.Height / (float)picbox_AreaMap.Height));
            }
            else
            {
                viewScale = (float)(1.0 / ((float)worldBMP.Width / (float)picbox_AreaMap.Width));
            }

            {
                int picW = (int)(viewScale * worldBMP.Width + 0.5);
                int picH = (int)(viewScale * worldBMP.Height + 0.5);

                worldMapBmp = new Bitmap(picW, picH);
                Graphics g = Graphics.FromImage(worldMapBmp);

                g.ResetTransform();
                //g.TranslateTransform(-picW*0.5f, -picH*0.5f, MatrixOrder.Append);
                //g.RotateTransform((float)layer.wAng, MatrixOrder.Append);
                g.ScaleTransform(viewScale, viewScale, MatrixOrder.Append);

                g.DrawImage(worldBMP, 0, 0);

                g.Dispose();
            }

            return worldMapBmp;
        }
/*
        /// <summary>
        /// エリアマップ生成
        /// </summary>
        /// <param name="worldBMP"></param>
        /// <param name="picbox_AreaMap"></param>
        /// <returns></returns>
        public Bitmap MakePictureBoxAreaMap(Bitmap worldBMP, PictureBox picbox_AreaMap, ref LocationSystem LocSys, int scrollX, int scrollY)
        {
            {
                //int picW = (int)(viewScale * worldBMP.Width + 0.5);
                //int picH = (int)(viewScale * worldBMP.Height + 0.5);

                areaMapBmp = new Bitmap(picbox_AreaMap.Width, picbox_AreaMap.Height);
                Graphics g = Graphics.FromImage(areaMapBmp);

                DrawMarkPoint drawCenter = new DrawMarkPoint(LocSys.R1, LocSys);

                g.ResetTransform();
                g.TranslateTransform( (float)(-drawCenter.x - (worldBMP.Width * 0.5) + (picbox_AreaMap.Width*0.5)) - scrollX,
                                      (float)(-drawCenter.y - (worldBMP.Height * 0.5) + (picbox_AreaMap.Height * 0.5)) - scrollY,
                                      MatrixOrder.Append);
                //g.RotateTransform((float)layer.wAng, MatrixOrder.Append);
                //g.ScaleTransform(viewScale, viewScale, MatrixOrder.Append);

                g.DrawImage(worldBMP, 0, 0);

                g.Dispose();
            }

            return areaMapBmp;
        }
*/
        /// <summary>
        /// PictureBox内へのエリアマップ描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AreaMap_Draw_Area(Graphics g, PictureBox picbox_AreaMap, ref LocationSystem LocSys, int scrollX, int scrollY, int selCpIndex)
        {
            Bitmap worldBMP = LocSys.mapBmp;
            //Bitmap areaBMP = MakePictureBoxAreaMap(worldBMP, picbox_AreaMap, ref LocSys, scrollX, scrollY);
            //float viewScale = 1.0f;

            DrawMarkPoint drawCenter = new DrawMarkPoint(LocSys.R1, LocSys);

            //float olScale = (float)LocSys.AreaOverlayBmp.Width / (float)LocSys.AreaBmp.Width;

            // エリアマップ描画
            g.ResetTransform();
            //g.DrawImage(areaBMP, 0, 0);

            g.ResetTransform();
            g.TranslateTransform( (float)(-drawCenter.x - (worldBMP.Width * 0.5) + (picbox_AreaMap.Width * 0.5)) - scrollX,
                                  (float)(-drawCenter.y - (worldBMP.Height * 0.5) + (picbox_AreaMap.Height * 0.5)) - scrollY,
                                  MatrixOrder.Append);
            //g.RotateTransform((float)layer.wAng, MatrixOrder.Append);
            //g.ScaleTransform(1.0f, -1.0f, MatrixOrder.Append);

            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(worldBMP, 0, 0);

            // ------------------------

            g.ResetTransform();
            g.TranslateTransform( (float)(-drawCenter.x + (picbox_AreaMap.Width * 0.5)) - scrollX,
                                  (float)(-drawCenter.y + (picbox_AreaMap.Height * 0.5)) - scrollY,
                                  MatrixOrder.Append);
            //g.ScaleTransform(1.0f, -1.0f, MatrixOrder.Append);

            // 現在位置描画
            DrawMaker(g, Brushes.Red, new DrawMarkPoint(LocSys.R1, LocSys), 8);

            // チェックポイント描画
            {
                double prvPosX = LocSys.R1.x;
                double prvPosY = LocSys.R1.y;

                for (int i = LocSys.RTS.GetCheckPointIdx(); i < LocSys.RTS.GetNumCheckPoints(); i++)
                {
                    //Vector3 checkPnt = LocSys.RTS.GetCheckPoint(i);
                    Vector3 checkPnt = LocSys.RTS.GetCheckPointToWayPoint(i);

                    //BrainCtrl.RTS.getNowTargetDir(ref dir);
                    DrawMarkPoint checkPntMk = new DrawMarkPoint( new MarkPoint(checkPnt.x, checkPnt.y, 0.0), LocSys);
                    checkPntMk.theta = -checkPnt.z;

                    if (selCpIndex == i)
                    {
                        // 選択中のチェックポイント
                        DrawMaker(g, Brushes.Purple, checkPntMk, 8);
                    }
                    else
                    {
                        DrawMaker(g, Brushes.GreenYellow, checkPntMk, 8);
                    }

                    // ターゲットまでのライン
                    DrawMakerLine(g, 1.0f,
                        new DrawMarkPoint( new MarkPoint(prvPosX, prvPosY, 0), LocSys),
                        checkPntMk,
                        Pens.Olive, 1);

                    g.DrawString(i.ToString("D2"), fntMini, Brushes.Green, checkPntMk.x, checkPntMk.y );

                    prvPosX = checkPnt.x;
                    prvPosY = checkPnt.y;
                }
            }

            // ターゲット描画(ハンドル操作がVRの場合)
            {
                Vector3 nowtgtPos = LocSys.RTS.GetNowTargetPositon();
                DrawMarkPoint tgtMk = new DrawMarkPoint(new MarkPoint(nowtgtPos.x, nowtgtPos.y, 0.0), LocSys);

                DrawMakerNoDir(g, Brushes.DeepPink, tgtMk, 6);

                // ターゲットまでのライン
                DrawMakerLine(g, 1.0f,
                    new DrawMarkPoint(LocSys.R1, LocSys),
                    tgtMk,
                    Pens.DeepPink, 1);
            }

        }
        /*
                /// <summary>
                /// 方眼紙モード
                /// </summary>
                /// <param name="g"></param>
                /// <param name="BrainCtrl"></param>
                public void AreaMap_Draw_Ruler(Graphics g, ref Brain BrainCtrl, int canvWidth, int canvHeight )
                {
                    LocationSystem LocSys = BrainCtrl.LocSys;
                    int toRulerSize = 50;  // 5mのピクセル数

                    int dfX = LocSys.worldMap.GetWorldX(0) % toRulerSize;
                    int dfY = LocSys.worldMap.GetWorldY(0) % toRulerSize;

                    // 横線
                    for( int iy=0; iy< canvHeight / toRulerSize; iy++ )
                    {
                        var P1 = new PointF(0.0f, (float)(iy * toRulerSize));
                        var P2 = new PointF((float)canvWidth, (float)(iy * toRulerSize));

                        g.DrawLine(Pens.LightGray, P1, P2);
                    }
                    // 縦線
                    for (int ix = 0; ix < canvWidth / toRulerSize; ix++)
                    {
                        var P1 = new PointF((float)(ix* toRulerSize), 0.0f);
                        var P2 = new PointF((float)(ix * toRulerSize), (float)canvHeight);

                        g.DrawLine(Pens.LightGray, P1, P2);
                    }
                }
        */
        //static int areaMapDrawCnt = 0;

        /// <summary>
        /// PictureBox内へのワールドマップ描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="CersioCt"></param>
        /// <param name="BrainCtrl"></param>
        public void AreaMap_Draw_WorldMap(Graphics g, PictureBox picbox_AreaMap, ref LocationSystem LocSys)
        {
            // 全体マップ描画
            float viewScale = 1.0f;// viewScaleWorld;


            // マップ外カラー
            g.FillRectangle(Brushes.DarkGray, 0, 0, picbox_AreaMap.Width, picbox_AreaMap.Height);
            
            {
                float wdAspect = (float)LocSys.mapBmp.Width / (float)worldMapBmp.Width;
                float htAspect = (float)LocSys.mapBmp.Height / (float)worldMapBmp.Height;

                if (wdAspect < htAspect) viewScale = (float)(1.0 / htAspect);
                else                     viewScale = (float)(1.0 / wdAspect);
            }
            

            g.ResetTransform();
            //g.TranslateTransform(-ctrX, -ctrY, MatrixOrder.Append);
            //g.RotateTransform((float)layer.wAng, MatrixOrder.Append);
            //g.ScaleTransform(viewScale, viewScale, MatrixOrder.Append);

            if (null != worldMapBmp)
            {
                g.DrawImage(worldMapBmp, 0, 0);
            }

            g.ResetTransform();
            g.TranslateTransform( (float)(worldMapBmp.Width * 0.5),
                                  (float)(worldMapBmp.Height * 0.5),
                                  MatrixOrder.Append);

            //g.ResetTransform();

            // 各マーカーの位置を描画
            //LocSys.DrawWorldMap(g, viewScale);
            DrawMaker(g, viewScale, Brushes.Red, new DrawMarkPoint(LocSys.R1, LocSys), 10);

            // チェックポイント描画
            {
                double dir = 0;

                DrawMarkPoint prvPos = new DrawMarkPoint(LocSys.R1, LocSys);

                for (int i = LocSys.RTS.GetCheckPointIdx(); i < LocSys.RTS.GetNumCheckPoints(); i++)
                {
                    Vector3 tgtPos = LocSys.RTS.GetCheckPoint(i);
                    //BrainCtrl.RTS.getNowTargetDir(ref dir);
                    DrawMarkPoint tgtMk = new DrawMarkPoint( new MarkPoint(tgtPos.x, tgtPos.y, dir), LocSys);

                    // ターゲットまでのライン
                    DrawMakerLine(g, viewScale, prvPos, tgtMk, Pens.Olive, 1);

                    g.DrawString(i.ToString("D2"), fntMini, Brushes.Green, (int)(tgtMk.x * viewScale), (int)(tgtMk.y * viewScale));

                    prvPos.x = tgtMk.x;
                    prvPos.y = tgtMk.y;
                }
            }
        }


        /// <summary>
        /// インジケータ描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="baseY"></param>
        public void DrawIngicator(Graphics g, int baseY, ref CersioCtrl CersioCt, ref Brain BrainCtrl)
        {
            {
                // Handle
                {
                    int stX = 20;
                    int stY = baseY + 8;
                    int Wd = 200;
                    int Ht = 15;

                    float handleVal = (float)((Wd / 2) * (CersioCt.nowSendHandleValue));
                    if (handleVal > 0)
                    {
                        g.FillRectangle(Brushes.Red, stX + Wd / 2, stY, handleVal, Ht);
                    }
                    else
                    {
                        g.FillRectangle(Brushes.Red, stX + Wd / 2 + handleVal, stY, -handleVal, Ht);
                    }
                    g.DrawRectangle(Pens.White, stX, stY, Wd, Ht);
                    g.DrawLine(Pens.White, stX + Wd / 2, stY, stX + Wd / 2, stY + Ht);
                    g.DrawString("Handle", fntMini, Brushes.White, stX + Wd / 2 - 38 / 2, stY + Ht - fntMini.GetHeight());
                }

                // ACC
                {
                    int stX = 20;
                    int stY = baseY + 30;
                    int Wd = 200;
                    int Ht = 15;

                    float accVal = (float)((Wd / 2) * (CersioCt.nowSendAccValue));
                    if (accVal > 0)
                    {
                        // 前進
                        g.FillRectangle(Brushes.GreenYellow, stX + Wd / 2, stY, accVal, Ht);
                    }
                    else
                    {
                        // 後退
                        g.FillRectangle(Brushes.Yellow, stX + Wd / 2 + accVal, stY, -accVal, Ht);
                    }
                    g.DrawRectangle(Pens.White, stX, stY, Wd, Ht);
                    g.DrawLine(Pens.White, stX + Wd / 2, stY, stX + Wd / 2, stY + Ht);
                    g.DrawString("Accel", fntMini, Brushes.White, stX + Wd / 2 - 38 / 2, stY + Ht - fntMini.GetHeight());
                }
                /*
                stX = 40;
                stY = baseY + 30;
                Wd = 10;
                Ht = 40;

                float accVal = (float)(Ht * CersioCtrl.nowSendAccValue);
                g.FillRectangle(Brushes.Red, stX, stY + Ht - accVal, Wd, accVal);
                g.DrawRectangle(Pens.White, stX, stY, Wd, Ht);
                g.DrawString("Acc", fntMini, Brushes.White, stX - 35, stY + Ht - fntMini.GetHeight());
                */
            }

            int dirMarkBaseY = baseY + 80;

            // 現在の向き
            {
                int dx = 40;
                double ang = -BrainCtrl.LocSys.RTS.GetNowDir();
                DrawMaker(g, Brushes.Red, dx, dirMarkBaseY, ang, 12);
                g.DrawString(ang.ToString("F1"), fntMini, Brushes.White, dx - 25, baseY + 120);
                g.DrawString("CarDir", fntMini, Brushes.White, dx - 25, dirMarkBaseY + 20);
            }

            // 相手の向き
            {
                int dx = 100;
                double ang = -BrainCtrl.LocSys.RTS.GetNowTargetDir();
                DrawMaker(g, Brushes.Purple, dx, dirMarkBaseY, ang, 12);
                g.DrawString(ang.ToString("F1"), fntMini, Brushes.White, dx - 25, baseY + 120);
                g.DrawString("CPTarget", fntMini, Brushes.White, dx - 25, dirMarkBaseY + 20);
            }

            // 向けたいハンドル角度
            {
                int dx = 160;
                double ang = -BrainCtrl.LocSys.RTS.GetNowTargetStearingDir();
                DrawMaker(g, Brushes.Cyan, dx, dirMarkBaseY, ang, 12, -90.0 / 180.0 * Math.PI );
                g.DrawString(ang.ToString("F1"), fntMini, Brushes.White, dx - 25, baseY + 120);
                g.DrawString("Handle", fntMini, Brushes.White, dx - 25, dirMarkBaseY + 20);
            }
        }

        /// <summary>
        /// LRF用のガイドライン描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="LocSys"></param>
        /// <param name="ctrX"></param>
        /// <param name="ctrY"></param>
        /// <param name="scale"></param>
        public void LRF_Draw_GuideLine(Graphics g, ref LocationSystem LocSys, int ctrX, int ctrY, double scale )
        {
            // ガイド描画
            // 30mを2m区切りで描画
            for (int i = 1; i <= 30 / 2; i++)
            {
                int cirSize = (int)((((i * 2000) * 2) / (LocSys.MapTom*1000.0)) * scale);

                g.DrawPie(Pens.LightGray,
                                    (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                    cirSize, cirSize,
                                    -135 - 90, 270);

                g.DrawString((i*2).ToString("F1")+"m", fntMini, Brushes.LightGray, (ctrX - cirSize / 2), (ctrY - cirSize / 2));
            }
        }
    }
}
