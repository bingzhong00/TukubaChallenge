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
    public partial class LRFMapForm : Form
    {
        CarSim carSim;
        Bitmap LrfBmp;

        public LRFMapForm(CarSim _carSim )
        {
            InitializeComponent();

            carSim = _carSim;
            LrfBmp = new Bitmap(picboxLRF.Width, picboxLRF.Height);

            picboxLRF.Image = LrfBmp;

            //tmr_Update.Enabled = true;
        }

        public void tmr_Update_Tick(object sender, EventArgs e)
        {
            if (carSim.mkp.LRFdata != null)
            {
                /*
                Graphics g = Graphics.FromImage(LrfBmp);

                //double rScale = (1.0 / LocSys.RealToMapSclae);
                double rPI = Math.PI / 180.0;
                int pixelSize = 3;
                double picScale = (100.0f / 1000.0f) * 1.0;

                double ctrX = picboxLRF.Width / 2.0;
                double ctrY = picboxLRF.Height / 2.0;

                // LRFの値を描画
                for (int i = 0; i < carSim.mkp.LRFdata.Length; i++)
                {
                    double val = carSim.mkp.LRFdata[i] * picScale;// *rScale;
                    double rad = (i - MapRangeFinder.AngleRangeHalf - 90) * rPI;

                    // LRFは左下から右回り
                    float x = (float)(ctrX + val * Math.Cos(rad));
                    float y = (float)(ctrY + val * Math.Sin(rad));
                    g.FillRectangle(Brushes.Yellow, x, y, pixelSize, pixelSize);
                }

                g.Dispose();
                */
                //picboxLRF.Image = LrfBmp;
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
                int pixelSize = 3;
                double picScale = ((picboxLRF.Width)/(30.0 * 1000.0));

                double ctrX = picboxLRF.Width / 2.0;
                double ctrY = picboxLRF.Height / 2.0;


                g.FillRectangle(Brushes.Black, 0, 0, picboxLRF.Width, picboxLRF.Height);

                // LRFの値を描画
                for (int i = 0; i < carSim.mkp.LRFdata.Length; i++)
                {
                    double val = carSim.mkp.LRFdata[i] * picScale;// *rScale;
                    double rad = (i - MapRangeFinder.AngleRangeHalf - 90) * rPI;

                    // LRFは左下から右回り
                    float x = (float)(ctrX + val * Math.Cos(rad));
                    float y = (float)(ctrY + val * Math.Sin(rad));
                    g.FillRectangle(Brushes.Yellow, x, y, pixelSize, pixelSize);
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
