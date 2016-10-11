using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;

using LocationPresumption;
using CersioIO;
using Navigation;

namespace VehicleRunner
{
    class VehicleRunnerForm_Draw
    {
        Bitmap worldMapBmp;

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

        static public void DrawMaker(Graphics g, Brush brush, double mkX, double mkY, double mkDir, double size)
        {
            //double nonScl = size / ViewScale;
            //if (nonScl <= 1.0) nonScl = 1.0;

            mkDir = mkDir - 90.0;

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

        /// <summary>
        /// 向きのないマーカー
        /// ひし形
        /// </summary>
        /// <param name="g"></param>
        /// <param name="brush"></param>
        /// <param name="mkX"></param>
        /// <param name="mkY"></param>
        /// <param name="size"></param>
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

        private void DrawString(Graphics g, int x, int y, string str, Brush brush, Brush bkBrush)
        {
            g.DrawString(str, drawFont, bkBrush, x + 1, y);
            g.DrawString(str, drawFont, bkBrush, x - 1, y);
            g.DrawString(str, drawFont, bkBrush, x, y - 1);
            g.DrawString(str, drawFont, bkBrush, x, y + 1);
            g.DrawString(str, drawFont, brush, x, y);
        }


        private void DrawMakerLine(Graphics g, float fScale, MarkPoint robotA, MarkPoint robotB, Pen pen, int size)
        {
            var P1 = new PointF(
                (float)(robotA.X * fScale),
                (float)(robotA.Y * fScale));
            var P2 = new PointF(
                (float)(robotB.X * fScale),
                (float)(robotB.Y * fScale));

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
                worldMapBmp = new Bitmap((int)(viewScale * worldBMP.Width + 0.5), (int)(viewScale * worldBMP.Height + 0.5));
                Graphics g = Graphics.FromImage(worldMapBmp);

                g.ResetTransform();
                //g.TranslateTransform(-ctrX, -ctrY, MatrixOrder.Append);
                //g.RotateTransform((float)layer.wAng, MatrixOrder.Append);
                g.ScaleTransform(viewScale, viewScale, MatrixOrder.Append);

                g.DrawImage(worldBMP, 0, 0);

                g.Dispose();
            }

            return worldMapBmp;
        }

        /// <summary>
        /// エリアマップ描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AreaMap_Draw_Area(Graphics g, ref CersioCtrl CersioCt, ref Brain BrainCtrl)
        {
            // 書き換えＢＭＰ（追加障害物）描画
            LocPreSumpSystem LocSys = BrainCtrl.LocSys;

            // エリアマップ描画
            //g.FillRectangle(Brushes.Black, 0, 0, picbox_AreaMap.Width, picbox_AreaMap.Height);

            g.DrawImage(LocSys.AreaOverlayBmp, 0, 0);


            // ターゲット描画
            if (null != CersioCt)
            {
                int tgtPosX, tgtPosY;
                double dir = 0;
                tgtPosX = tgtPosY = 0;
                float olScale = (float)LocSys.AreaOverlayBmp.Width / (float)LocSys.AreaBmp.Width;
                BrainCtrl.RTS.getNowTarget(ref tgtPosX, ref tgtPosY);
                BrainCtrl.RTS.getNowTargetDir(ref dir);
                MarkPoint tgtMk = new MarkPoint(LocSys.worldMap.GetAreaX(tgtPosX), LocSys.worldMap.GetAreaY(tgtPosY), dir);

                DrawMaker(g, olScale, tgtMk, Brushes.GreenYellow, 8);

                // ターゲットまでのライン
                DrawMakerLine(g, olScale,
                    new MarkPoint(LocSys.worldMap.GetAreaX(LocSys.R1.X), LocSys.worldMap.GetAreaY(LocSys.R1.Y), 0),
                    tgtMk,
                    Pens.Olive, 1);
            }

        }

        /// <summary>
        /// 方眼紙モード
        /// </summary>
        /// <param name="g"></param>
        /// <param name="BrainCtrl"></param>
        public void AreaMap_Draw_Ruler(Graphics g, ref Brain BrainCtrl, int canvWidth, int canvHeight )
        {
            LocPreSumpSystem LocSys = BrainCtrl.LocSys;
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

        //static int areaMapDrawCnt = 0;

        public void AreaMap_Draw_WorldMap(Graphics g, ref CersioCtrl CersioCt, ref Brain BrainCtrl)
        {
            LocPreSumpSystem LocSys = BrainCtrl.LocSys;

            // 全体マップ描画
            float viewScale;

            g.FillRectangle(Brushes.Black, 0, 0, worldMapBmp.Width, worldMapBmp.Height);

            if (((float)LocSys.worldMap.WorldSize.w / (float)worldMapBmp.Width) < ((float)LocSys.worldMap.WorldSize.h / (float)worldMapBmp.Height))
            {
                viewScale = (float)(1.0 / ((float)LocSys.worldMap.WorldSize.h / (float)worldMapBmp.Height));
            }
            else
            {
                viewScale = (float)(1.0 / ((float)LocSys.worldMap.WorldSize.w / (float)worldMapBmp.Width));
            }

            //g.ResetTransform();
            //g.TranslateTransform(-ctrX, -ctrY, MatrixOrder.Append);
            //g.RotateTransform((float)layer.wAng, MatrixOrder.Append);
            //g.ScaleTransform(viewScale, viewScale, MatrixOrder.Append);

            if (null != worldMapBmp)
            {
                g.DrawImage(worldMapBmp, 0, 0);
            }

            //g.ResetTransform();

            // 各マーカーの位置を描画
            LocSys.DrawWorldMap(g, viewScale);

            // ターゲット描画
            if (null != CersioCt)
            {
                int tgtPosX, tgtPosY;
                double dir = 0;
                tgtPosX = tgtPosY = 0;

                BrainCtrl.RTS.getNowTarget(ref tgtPosX, ref tgtPosY);
                BrainCtrl.RTS.getNowTargetDir(ref dir);
                MarkPoint tgtMk = new MarkPoint(tgtPosX, tgtPosY, dir + 180);

                DrawMaker(g, viewScale, tgtMk, Brushes.GreenYellow, 8);

                // ターゲットまでのライン
                DrawMakerLine(g, viewScale,
                    LocSys.R1,
                    tgtMk,
                    Pens.Olive, 1);
            }

        }


        public void AreaMap_Draw_Text(Graphics g, ref Brain BrainCtrl, long updateHwCnt)
        {
            LocPreSumpSystem LocSys = BrainCtrl.LocSys;

            g.ResetTransform();

            try
            {
                // Info
                DrawString(g, 0, drawFont.Height * 0,
                           "R1 X:" + ((int)(LocSys.R1.X + 0.5)).ToString("D4") +
                           ",Y:" + ((int)(LocSys.R1.Y + 0.5)).ToString("D4") +
                           ",角度:" + ((int)LocSys.R1.Theta).ToString("D3"),
                           Brushes.Red, Brushes.Black);
                /*
                DrawString(g, 0, drawFont.Height * 1,
                           "Compass:" + CersioCt.hwCompass.ToString("D3") + "/ ReDir:" + ((int)(CersioCt.hwREDir)).ToString("D3") +
                           ",ReX:" + ((int)(CersioCt.hwREX)).ToString("D4") + ",Y:" + ((int)(CersioCt.hwREY)).ToString("D4"),
                           Brushes.Blue, Brushes.White);
                */
                DrawString(g, 0, drawFont.Height * 1,
                           "RunCnt:" + updateHwCnt.ToString("D8") + "/ Goal:" + (BrainCtrl.goalFlg ? "TRUE" : "FALSE" + "/ Cp:" + BrainCtrl.RTS.GetNowCheckPointIdx().ToString()),
                           Brushes.Blue, Brushes.White);

                DrawString(g, 0, drawFont.Height * 2,
                           "LocProc:" + LocPreSumpSystem.swCNT_Update.ElapsedMilliseconds +
                           "ms /Draw:" + LocSys.swCNT_Draw.ElapsedMilliseconds +
                           "ms /MRF:" + LocPreSumpSystem.swCNT_MRF.ElapsedMilliseconds + "ms",
                           Brushes.Blue, Brushes.White);

                LocPreSumpSystem.swCNT_Update.Reset();
                LocPreSumpSystem.swCNT_MRF.Reset();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                int stX = 20;
                int stY = baseY + 8;
                int Wd = 200;
                int Ht = 15;

                float handleVal = (float)((Wd / 2) * (-CersioCtrl.nowSendHandleValue));
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

                // ACC
                stX = 40;
                stY = baseY + 30;
                Wd = 10;
                Ht = 40;

                float accVal = (float)(Ht * CersioCtrl.nowSendAccValue);
                g.FillRectangle(Brushes.Red, stX, stY + Ht - accVal, Wd, accVal);
                g.DrawRectangle(Pens.White, stX, stY, Wd, Ht);
                g.DrawString("Acc", fntMini, Brushes.White, stX - 35, stY + Ht - fntMini.GetHeight());

            }

            // Compus
            if (CersioCt.bhwCompass)
            {
                int ang = CersioCt.hwCompass;
                DrawMaker(g, Brushes.Red, 90, baseY + 50, ang, 12);
                g.DrawString(ang.ToString(), fntMini, Brushes.White, 90 - 25, baseY + 50);
                g.DrawString("Compus", fntMini, Brushes.White, 90 - 25, baseY + 50 + 14);
            }

            // RE Dir
            if (CersioCt.bhwREPlot)
            {
                double ang = CersioCt.hwREDir;
                DrawMaker(g, Brushes.Purple, 150, baseY + 50, ang, 12);
                g.DrawString(ang.ToString("F1"), fntMini, Brushes.White, 150 - 25, baseY + 50);
                g.DrawString("R.E.Dir", fntMini, Brushes.White, 150 - 25, baseY + 50 + 14);
            }

            // GPS Dir
            if (CersioCt.bhwGPS)
            {
                if (CersioCt.bhwUsbGPS)
                {
                    // USBGPSは角度あり
                    DrawMaker(g, Brushes.LimeGreen, 210, baseY + 50, CersioCt.hwGPS_MoveDir, 12);
                }
                else
                {
                    // bServerGPS 角度なし
                    DrawMaker(g, Brushes.LimeGreen, 210, baseY + 50, 12);
                }
                g.DrawString("GPSDir", fntMini, Brushes.White, 210 - 25, baseY + 50 + 14);
            }

            // 向けたいハンドル角度
            {
                double ang = BrainCtrl.RTS.getNowTargetStearingDir();
                DrawMaker(g, Brushes.Cyan, 40, baseY + 120, ang, 12);
                g.DrawString(ang.ToString("F1"), fntMini, Brushes.White, 40 - 25, baseY + 120);
                g.DrawString("Handle", fntMini, Brushes.White, 40 - 25, baseY + 120 + 14);
            }

            // 現在の向き
            {
                double ang = BrainCtrl.RTS.getNowDir();
                DrawMaker(g, Brushes.Red, 100, baseY + 120, ang, 12);
                g.DrawString(ang.ToString("F1"), fntMini, Brushes.White, 100 - 25, baseY + 120);
                g.DrawString("RTSNow", fntMini, Brushes.White, 100 - 25, baseY + 120 + 14);
            }

            // 相手の向き
            {
                double ang = BrainCtrl.RTS.getNowTargetDir();
                DrawMaker(g, Brushes.Purple, 160, baseY + 120, ang, 12);
                g.DrawString(ang.ToString("F1"), fntMini, Brushes.White, 160 - 25, baseY + 120);
                g.DrawString("RTSTgt", fntMini, Brushes.White, 160 - 25, baseY + 120 + 14);
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
        public void LRF_Draw_GuideLine(Graphics g, ref LocPreSumpSystem LocSys, int ctrX, int ctrY, double scale )
        {
            // ガイド描画
            // 30mを2m区切りで描画
            for (int i = 1; i <= 30 / 2; i++)
            {
                int cirSize = (int)((((i * 2000) * 2) / LocSys.MapToRealScale) * scale);

                g.DrawPie(Pens.LightGray,
                                    (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                    cirSize, cirSize,
                                    -135 - 90, 270);

                g.DrawString((i*2).ToString("F1")+"m", fntMini, Brushes.LightGray, (ctrX - cirSize / 2), (ctrY - cirSize / 2));
            }
        }

        /// <summary>
        /// LRFデータ描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="LRF_Data"></param>
        /// <param name="ctrX"></param>
        /// <param name="ctrY"></param>
        /// <param name="picScale"></param>
        public void LRF_Draw_Point(Graphics g, Brush colBursh,double[] LRF_Data, int ctrX, int ctrY, double picScale)
        {
           double rPI = Math.PI / 180.0;
            int pixelSize = 3;

            // LRFの値を描画
            for (int i = 0; i < LRF_Data.Length; i++)
            {
                double val = LRF_Data[i] * picScale;// *rScale;
                double rad = (-i + MapRangeFinder.AngleRangeHalf - 90) * rPI;

                // LRFは右下から左回り
                float x = (float)(ctrX + val * Math.Cos(rad));
                float y = (float)(ctrY + val * Math.Sin(rad));
                g.FillRectangle(colBursh, x, y, pixelSize, pixelSize);
            }
        }

        /// <summary>
        /// 緊急停止情報のLRFデータ
        /// </summary>
        /// <param name="g"></param>
        /// <param name="LocSys"></param>
        /// <param name="BrainCtrl"></param>
        /// <param name="lrfdata"></param>
        /// <param name="ctrX"></param>
        /// <param name="ctrY"></param>
        /// <param name="scale"></param>
        /// <param name="picScale"></param>
        public void LRF_Draw_PointEBS(Graphics g, ref LocPreSumpSystem LocSys, ref Brain BrainCtrl, double[] lrfdata, int ctrX, int ctrY, double scale, double picScale)
        {
            {
                int stAng = EmergencyBrake.stAng + (int)BrainCtrl.EBS.HandleDiffAngle;
                int edAng = EmergencyBrake.edAng + (int)BrainCtrl.EBS.HandleDiffAngle;

                int cirSize;

                if (BrainCtrl.EBS.CautionLv >= EmergencyBrake.StopLv)
                {
                    // ブレーキレンジ内
                    Brush colBrs = Brushes.Red;
                    cirSize = (int)((EmergencyBrake.BrakeRange * 2.0 / LocSys.MapToRealScale) * scale);
                    g.FillPie(colBrs, (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                      cirSize, cirSize,
                                      stAng - 90, (edAng - stAng));
                }
                else if (BrainCtrl.EBS.CautionLv >= EmergencyBrake.SlowDownLv)
                {
                    // スローダウンレンジ内
                    Brush colBrs = Brushes.Orange;
                    cirSize = (int)((EmergencyBrake.SlowRange * 2.0 / LocSys.MapToRealScale) * scale);
                    g.FillPie(colBrs, (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                      cirSize, cirSize,
                                      stAng - 90, (edAng - stAng));
                }


                {
                    Pen colPen = Pens.Yellow;

                    // スローダウン　レンジ枠
                    cirSize = (int)((EmergencyBrake.SlowRange * 2.0 / LocSys.MapToRealScale) * scale);
                    g.DrawPie(colPen, (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                      cirSize, cirSize,
                                      stAng - 90, (edAng - stAng));

                    // ブレーキ　レンジ枠
                    cirSize = (int)((EmergencyBrake.BrakeRange * 2.0 / LocSys.MapToRealScale) * scale);
                    g.DrawPie(colPen, (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                      cirSize, cirSize,
                                      stAng - 90, (edAng - stAng));
                }
            }

            // EHS範囲描画
            {
                int stAng;
                int edAng;

                int cirSize = (int)((EmergencyHandring.MaxRange * 2.0 / LocSys.MapToRealScale) * scale);

                Pen colPen = Pens.LightGreen;

                // 左側
                stAng = EmergencyHandring.stLAng;
                edAng = EmergencyHandring.edLAng;
                if (BrainCtrl.EHS.Result == EmergencyHandring.EHS_MODE.LeftWallHit ||
                    BrainCtrl.EHS.Result == EmergencyHandring.EHS_MODE.CenterPass)
                {
                    g.FillPie(Brushes.Red, (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                      cirSize, cirSize,
                                      stAng - 90, (edAng - stAng));
                }
                g.DrawPie(colPen, (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                                          cirSize, cirSize,
                                                          stAng - 90, (edAng - stAng));


                // 右側
                stAng = EmergencyHandring.stRAng;
                edAng = EmergencyHandring.edRAng;
                if (BrainCtrl.EHS.Result == EmergencyHandring.EHS_MODE.RightWallHit ||
                    BrainCtrl.EHS.Result == EmergencyHandring.EHS_MODE.CenterPass)
                {
                    g.FillPie(Brushes.Red, (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                      cirSize, cirSize,
                                      stAng - 90, (edAng - stAng));
                }
                g.DrawPie(colPen, (ctrX - cirSize / 2), (ctrY - cirSize / 2),
                                                              cirSize, cirSize,
                                                              stAng - 90, (edAng - stAng));
            }

            // ノイズリダクションLRF描画
            if (lrfdata != null)
            {
                LRF_Draw_Point(g, Brushes.Cyan, lrfdata, ctrX, ctrY, picScale*(1.0 / LocSys.MapToRealScale));
            }
        }
  


    }
}
