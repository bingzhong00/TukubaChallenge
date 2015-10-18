using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SCIP_library;

namespace LRFMapEditer
{
    public partial class NewMapLoadForm : Form
    {
        public URGLogRead UrgLogReader;

        private MapEditForm parentForm;

        public const int LRF_PixelSize = 2;     // 点の描画サイズ

        const double defDistanceLayer = 10.0;

        // LRF
        long maxLRFIdx = 0;
        //long nowLRFIdx = 0;

        long skipLrfIDX = 100;

        private LayerData LRFlayer;         // LRF ウィンドウのデータ


        /// <summary>
        /// 
        /// </summary>
        public NewMapLoadForm( Form prntForm )
        {
            InitializeComponent();

            parentForm = (MapEditForm)prntForm;
            tb_SkipFrame.Text = skipLrfIDX.ToString();
        }

        private void NewMapLoadForm_Load(object sender, EventArgs e)
        {
        }

        private void NewMapLoadForm_Shown(object sender, EventArgs e)
        {
            btn_LoadLRF_Click(null, null);
        }

        /// <summary>
        /// Formクローズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewMapLoadForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != UrgLogReader)
            {
                UrgLogReader.CloseFile();
            }
        }

        /// <summary>
        /// LRF ログ読み込み
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LoadLRF_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenDlg = new OpenFileDialog();

            OpenDlg.Filter = "URG LogFile (*.txt)|*.txt";

            var Result = OpenDlg.ShowDialog();
            if (Result != System.Windows.Forms.DialogResult.OK) return;

            LoadLRFLogFile(OpenDlg.FileName);
        }

        /// <summary>
        /// 全レイヤー生成
        /// </summary>
        private void AllNewLayer()
        {
            if (null == UrgLogReader)
            {
                MessageBox.Show("LRFデータがロードされていません", "Error");
                return;
            }

            parentForm.MapLyaer = new List<LayerData>();
            UrgLogReader.SetSkipNum(skipLrfIDX);
            LayerData lastLayer = null;

            double[] readLRFdata = UrgLogReader.getScanData(0);
            while (null != readLRFdata && readLRFdata.Length > 0)
            {
                // レイヤー作成
                LayerData newLayer = new LayerData(readLRFdata);
                newLayer.SetLocalPosAng( 0.0, 0.0, 0.0, -defDistanceLayer);
                newLayer.MakeMapBmp( MapEditForm.LRF_Range,
                                     MapEditForm.LRF_ScaleOfPixel,LRF_PixelSize,
                                     parentForm.colLayerPixel, parentForm.colLayerBase);

                // できた画像がほぼ同じなら、カットして、データ削減
                if (cb_StopLayerCut.Checked && null != lastLayer)
                {
                    // 90%以下なら静止中と判断
                    if (LayerMatching(newLayer, lastLayer) < 90)
                    {
                        parentForm.MapLyaer.Add(newLayer);
                        lastLayer = newLayer;
                    }
                }
                else
                {
                    parentForm.MapLyaer.Add(newLayer);
                    lastLayer = newLayer;
                }

                readLRFdata = UrgLogReader.getScanData();
            }

            // センタリング
            if (parentForm.MapLyaer.Count > 0)
            {
                LayerData firstLayer = parentForm.MapLyaer[0];

                firstLayer.SetLocalPosAng( -firstLayer.MapBmp.Width / 2,
                                           -firstLayer.MapBmp.Height / 2,
                                           0.0,
                                           0.0 );

                parentForm.UpdateLayerData();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fname"></param>
        private void LoadLRFLogFile(string fname)
        {
            if (null != UrgLogReader)
            {
                UrgLogReader.CloseFile();
                UrgLogReader = null;
            }

            try
            {
                UrgLogReader = new URGLogRead();
                UrgLogReader.OpenFile(fname);
                maxLRFIdx = UrgLogReader.getNumMD() - 1;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Load Fail " + fname);
            }

            parentForm.LRF_LogFileName = fname;
            tb_LogFileName.Text = fname;
            lbl_NumFrame.Text = maxLRFIdx.ToString();

            sb_LRFTime.Minimum = 0;
            sb_LRFTime.Maximum = (int)maxLRFIdx;
            num_LRFTime.Maximum = maxLRFIdx;

            UpdateLRFPicBox(0);
        }

        /// <summary>
        /// LRF ピクチャボックス更新
        /// </summary>
        /// <param name="lrfIdx"></param>
        private void UpdateLRFPicBox(long lrfIdx)
        {
            if (null != UrgLogReader)
            {
                if (UrgLogReader.getNextIndex() == lrfIdx)
                {
                    LRFlayer = new LayerData(UrgLogReader.getScanData());
                }
                else
                {
                    // インデックスがとんだ場合
                    LRFlayer = new LayerData(UrgLogReader.getScanData(lrfIdx));
                }

                LRFlayer.MakeMapBmp( MapEditForm.LRF_Range,
                                     MapEditForm.LRF_ScaleOfPixel, LRF_PixelSize,
                                     parentForm.colLayerPixel, parentForm.colLayerBase);

                pb_LRFLog.Image = LRFlayer.MapBmp;
                pb_LRFLog.Invalidate();
            }
        }

        private void sb_LRFTime_ValueChanged(object sender, EventArgs e)
        {
            num_LRFTime.Value = sb_LRFTime.Value;
            UpdateLRFPicBox((long)num_LRFTime.Value);
        }

        // 取り込み
        private void btn_Invert_Click(object sender, EventArgs e)
        {
            AllNewLayer();
            this.Close();
        }

        private void tb_SkipFrame_TextChanged(object sender, EventArgs e)
        {
            long.TryParse(tb_SkipFrame.Text, out skipLrfIDX);

            if (null != UrgLogReader)
            {
                UrgLogReader.SetSkipNum(skipLrfIDX);
            }
        }

        /// <summary>
        /// レイヤーマップを比べる
        /// </summary>
        /// <param name="aLayer"></param>
        /// <param name="bLayer"></param>
        /// <returns>合致度(%)を返す</returns>
        private int LayerMatching( LayerData aLayer,LayerData bLayer )
        {
            int matchCnt = 0;
            int matchNum = 0;

            for(int x=0; x<aLayer.MapBmp.Width; x++ )
            {
                for(int y=0; y<aLayer.MapBmp.Height; y++ )
                {
                    Color pixCol = aLayer.MapBmp.GetPixel(x, y);

                    if (pixCol.R != parentForm.colLayerBase.R ||
                        pixCol.G != parentForm.colLayerBase.G ||
                        pixCol.B != parentForm.colLayerBase.B )
                    {
                        matchNum++;
                        if (pixCol == bLayer.MapBmp.GetPixel(x, y))
                        {
                            matchCnt++;
                        }
                    }
                }
            }

            if (matchNum == 0) return 0;

            // 合致度を確認
            return (matchCnt * 100 / matchNum);
        }

    }
}
