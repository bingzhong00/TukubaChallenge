using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LocationPresumption;

namespace CersioSim
{
    /// <summary>
    /// LRF　マップ自動生成フォーム
    /// </summary>
    public partial class LRFMapForm : Form
    {
        CarSim carSim;
        Bitmap LrfBmp;
        double ScaleToPixel;

        /// <summary>
        /// 自動マップ描画　最大レンジ（30m以上はつきぬけなので、描画しない）
        /// </summary>
        const double MAP_DRAW_MAX_RANGE = 99 * 1000;///25 * 1000;

        public LRFMapForm(CarSim _carSim,int mapWidth, int mapHeight,double _ScaleToPixel )
        {
            InitializeComponent();

            carSim = _carSim;
            LrfBmp = new Bitmap(mapWidth, mapHeight);
            ScaleToPixel = _ScaleToPixel;

            {
                Graphics g = Graphics.FromImage(LrfBmp);
                g.FillRectangle(Brushes.Black, 0, 0, mapWidth, mapHeight);

                g.Dispose();
            }

            //picboxLRF.Image = LrfBmp;

            //tmr_Update.Enabled = true;
        }

        public void tmr_Update_Tick(object sender, EventArgs e)
        {
            if (carSim.mkp.LRFdata != null)
            {
                Graphics g = Graphics.FromImage(LrfBmp);

                //double rScale = (1.0 / LocSys.RealToMapSclae);
                double rPI = Math.PI / 180.0;
                int pixelSize = 3;
                double mapScale = 1.00;
                double picScale = ScaleToPixel * mapScale;

                double ctrX = carSim.mkp.X * ScaleToPixel;
                double ctrY = carSim.mkp.Y * ScaleToPixel;

                // LRFの値を描画
                for (int i = 0; i < carSim.mkp.LRFdata.Length; i++)
                {
                    if (carSim.mkp.LRFdata[i] < MAP_DRAW_MAX_RANGE)
                    {
                        double val = carSim.mkp.LRFdata[i] * picScale;
                        double rad = (((-i + MapRangeFinder.AngleRangeHalf) - 90) + carSim.mkp.Theta) * rPI;

                        // LRFは右下から左回り
                        float x = (float)(ctrX + val * Math.Cos(rad));
                        float y = (float)(ctrY + val * Math.Sin(rad));

                        g.DrawLine(Pens.LightGray, (float)ctrX, (float)ctrY, x, y);
                        //g.FillRectangle(Brushes.Yellow, x, y, pixelSize, pixelSize);
                    }
                }

                g.Dispose();

                picboxMap.Image = LrfBmp;
                picboxMap.Invalidate();
                picboxLRF.Invalidate();
            }

        }

        private void picboxLRF_Click(object sender, EventArgs e)
        {

        }

        private void picboxLRF_Paint(object sender, PaintEventArgs e)
        {
            if (carSim.mkp.LRFdata != null)
            {
                Graphics g = e.Graphics;// .Gr Graphics.FromImage(LrfBmp);

                //double rScale = (1.0 / LocSys.RealToMapSclae);
                double rPI = Math.PI / 180.0;
                float pixelSize = 3;
                // 画面を30m(mm)
                double picScale = ((picboxLRF.Width)/(30.0 * 1000.0));

                double ctrX = picboxLRF.Width / 2.0;
                double ctrY = picboxLRF.Height / 2.0;


                g.FillRectangle(Brushes.Black, 0, 0, picboxLRF.Width, picboxLRF.Height);

                // LRFの値を描画
                for (int i = 0; i < carSim.mkp.LRFdata.Length; i++)
                {
                    double val = carSim.mkp.LRFdata[i] * picScale;// *rScale;
                    double rad = (-i + MapRangeFinder.AngleRangeHalf - 90) * rPI;

                    // LRFは右下から左回り
                    float x = (float)(ctrX + val * Math.Cos(rad));
                    float y = (float)(ctrY + val * Math.Sin(rad));

                    g.DrawLine(Pens.Cyan, (float)ctrX, (float)ctrY, x, y);
                    g.FillRectangle(Brushes.Yellow, x - ((float)pixelSize * 0.5f), y - ((float)pixelSize * 0.5f), pixelSize, pixelSize);
                }

                //g.Dispose();

                //picboxLRF.Image = LrfBmp;
                //picboxLRF.Invalidate();
            }
        }

        // ※車のセンサー情報からLRF画像表示

        // ※位置座標から、MAP画像書き込み（←LRF情報からのMAP生成の整合性確認）
        //　のちに、RE、地磁気のセンサー情報から自己位置座標算出（←この自己位置計算がキモの一つ） 
    }
}
