using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Axiom.Math;

namespace LocationPresumption
{
    /// <summary>
    /// ���Ȉʒu���� �}�b�v���W�ϊ��v�Z�@�N���X
    /// </summary>
    public partial class LocPreSumpSystem
    {
        // --------------------------------------------------------------------------------------------------------------------------------
        // �`��n

            // �����J�E���^
        public System.Diagnostics.Stopwatch swCNT_Draw = new System.Diagnostics.Stopwatch();

        int locMapDrawCnt = 0;

        /// <summary>
        /// ���Ȉʒu���\�� Bmp�X�V
        /// </summary>
        public void UpdateLocalizeBitmap(bool bParticle, bool bLineTrace)
        {
            swCNT_Draw.Reset();
            swCNT_Draw.Start();

            //lock (AreaBmp)
            {
                Graphics g = Graphics.FromImage(AreaOverlayBmp);

                // Overlay�̃X�P�[��
                // �G���A���W����I�[�o�[���C�G���A�ւ̕ϊ�
                float olScale = (float)AreaOverlayBmp.Width / (float)AreaBmp.Width;

                // �G���A�}�b�v�`��
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(AreaBmp, 0, 0, AreaOverlayBmp.Width, AreaOverlayBmp.Height);

                // �p�[�e�B�N���`��
                int size = 10;
                if (bParticle)
                {
                    for (int i = 0; i < Particles.Count; i++)
                    {
                        var p = Particles[i];
                        DrawMaker(g, olScale, p.Location, Brushes.LightBlue, 5);
                    }
                }

                // ���A���^�C���O�Օ`��
                if (bLineTrace)
                {
                    DrawMakerLog_Area(g, olScale, R1Log, Color.Red.R, Color.Red.G, Color.Red.B);
                    DrawMakerLog_Area(g, olScale, V1Log, Color.Cyan.R, Color.Cyan.G, Color.Cyan.B);
                    DrawMakerLog_Area(g, olScale, E1Log, Color.Purple.R, Color.Purple.G, Color.Purple.B);
                    DrawMakerLog_Area(g, olScale, G1Log, Color.Green.R, Color.Green.G, Color.Green.B);
                }

                // �`�揇����ɂ����āA�d�Ȃ��Ă�������悤�ɂ���
                for (int i = 0; i < 5; i++)
                {
                    switch ((i + locMapDrawCnt) % 5)
                    {
                        case 0:
                            // RE�z�胍�{�b�g�ʒu�`��
                            DrawMaker_Area(g, olScale, E1, Brushes.Purple, size);
                            break;
                        case 1:
                            // PF�z�胍�{�b�g�ʒu�`��
                            DrawMaker_Area(g, olScale, V1, Brushes.Cyan, size);
                            break;
                        case 2:
                            // �����{�b�g�z��ʒu�`��
                            DrawMaker_Area(g, olScale, R1, Brushes.Red, size);
                            break;
                        case 3:
                            // GPS�ʒu�`��
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
                            // RE�z�胍�{�b�g�ʒu�`��
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
        /// �}�[�J�[�`��
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
        /// �}�[�J�[�`��
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
        /// ���[���h�}�b�v��̃}�[�J�[�`��
        /// </summary>
        public void DrawWorldMap(Graphics g, float viewScale)
        {
            int mkSize = 8;
            // �`�揇����ɂ����āA�d�Ȃ��Ă�������悤�ɂ���
            for (int i = 0; i < 5; i++)
            {
                switch ((i + worldMapDrawCnt) % 5)
                {
                    case 0:
                        // RE�ʒu�`��
                        DrawMaker(g, viewScale, E1, Brushes.Purple, mkSize);
                        break;
                    case 1:
                        // PF�ʒu�`��
                        DrawMaker(g, viewScale, V1, Brushes.Cyan, mkSize);
                        break;
                    case 2:
                        // �����{�b�g�z��ʒu�`��
                        DrawMaker(g, viewScale, R1, Brushes.Red, mkSize);
                        break;
                    case 3:
                        // GPS�ʒu�`��
                        if (bEnableDirGPS)
                        {
                            // USB GPS
                            DrawMaker(g, viewScale, G1, Brushes.Green, mkSize);
                        }
                        else
                        {
                            // bServer GPS�p�x�Ȃ�
                            DrawMaker(g, Brushes.Green, G1.X * viewScale, G1.Y * viewScale, mkSize);
                        }
                        break;
                    case 4:
                        // RE Pulse�ʒu�`��
                        DrawMaker(g, viewScale, E2, Brushes.LightSalmon, mkSize);
                        break;
                }
            }

            // �G���A�g�`��
            g.DrawRectangle(Pens.Pink,
                             (worldMap.WldOffset.x * viewScale),
                             (worldMap.WldOffset.y * viewScale),
                             (worldMap.AreaGridSize.w * viewScale),
                             (worldMap.AreaGridSize.h * viewScale));

            worldMapDrawCnt++;
        }


        // ���O�摜�@�`�� ------------------------------------------------------------------------------------------------
        #region "LogMap�`��"

        /// <summary>
        /// ���O�O�Օ`��@���[�J���G���A�ɕϊ�
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

            //�܂��������
            g.DrawLines(new Pen(Color.FromArgb(colR, colG, colB)), ps);
        }

        /// <summary>
        /// ���O�O�Օ`��@���[���h
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

            //�܂��������
            g.DrawLines(new Pen(Color.FromArgb(colR, colG, colB)), ps);
        }

        /// <summary>
        /// ���O�ۑ��p�@���[���h�}�b�v�ւ̋O�Չ摜����
        /// </summary>
        /// <returns></returns>
        public Bitmap MakeMakerLogBmp(bool bPointOn, MarkPoint marker)
        {
            if (R1Log.Count <= 0) return null;  // �f�[�^�Ȃ�

            Bitmap logBmp = new Bitmap(worldMap.mapBmp);
            Graphics g = Graphics.FromImage(logBmp);

            // �O�Օ`��
            // ���Ȉʒu
            DrawMakerLogLine_World(g, R1Log, Color.Red.R, Color.Red.G, Color.Red.B);
            // �p�[�e�B�N���t�B���^�[ ���Ȉʒu����
            DrawMakerLogLine_World(g, V1Log, Color.Cyan.R, Color.Cyan.G, Color.Cyan.B);
            // ���[�^���[�G���R�[�_���W
            DrawMakerLogLine_World(g, E1Log, Color.Purple.R, Color.Purple.G, Color.Purple.B);
            // GPS���W
            DrawMakerLogLine_World(g, G1Log, Color.Green.R, Color.Green.G, Color.Green.B);

            // �ŏI�n�_�Ƀ}�[�J�\��
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

            // �����Ԃ��Ƃ̈ʒu�ƌ���
            if (bPointOn)
            {
                // ���Ȉʒu
                //foreach (var p in R1Log)
                for (int i = 0; i < R1Log.Count; i++)
                {
                    if ((i % 30) != 0) continue;
                    DrawMaker_Area(g, 1.0f, R1Log[i], Brushes.Red, 4);
                }


                // LRF �p�[�e�B�N���t�B���^�[
                //foreach (var p in V1Log)
                for (int i = 0; i < V1Log.Count; i++)
                {
                    if ((i % 30) != 0) continue;
                    DrawMaker_Area(g, 1.0f, V1Log[i], Brushes.Cyan, 3);
                }


                // ���[�^���[�G���R�[�_���W
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