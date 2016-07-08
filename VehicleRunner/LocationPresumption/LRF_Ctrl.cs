using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCIP_library;

namespace LocationPresumption
{
    /// <summary>
    /// レーザーレンジファインダー
    /// 管理クラス
    /// </summary>
    public class LRF_Ctrl
    {
        /// <summary>
        /// LRFの有効範囲(単位 mm)
        /// </summary>
        public const double LRFmaxRange_mm = 30.0 * 1000.0;   

        /// <summary>
        /// LRFのデータ取得範囲(角度)
        /// </summary>
        private const int maxLrfDir = 270;

        /// <summary>
        /// LRFノイズリダクション係数 (1.0...ノイズ高め　/ 0.0...ノイズ低め　ただし反応がにぶい)
        /// </summary>
        const double LRF_noiseRate = 0.40;//0.50;


        /// <summary>
        /// 
        /// </summary>
        public URG_LRF urgLRF = null;               // nullで仮想MAPモード

        public double[] LRF_Data = null;
        
        // LRFノイズリダクション
        public double[] LRF_UntiNoiseData;

        /// <summary>
        /// 
        /// </summary>
        public LRF_Ctrl()
        {
            LRF_UntiNoiseData = new double[maxLrfDir];

            // いきなり接触状態にならないように初期化
            for (int i = 0; i < LRF_UntiNoiseData.Length; i++)
            {
                LRF_UntiNoiseData[i] = LRFmaxRange_mm;
            }
        }

        /// <summary>
        /// LRF初期化　ログデータモード
        /// </summary>
        /// <param name="logFilename"></param>
        public void Open(string logFilename)
        {
            urgLRF = new URG_LRF();
            urgLRF.LogFileOpen(logFilename);
        }

        /// <summary>
        /// LRF初期化 ＬＡＮモード
        /// </summary>
        /// <param name="IPAddr"></param>
        /// <param name="IPPort"></param>
        public void Open(string IPAddr, int IPPort)
        {
            urgLRF = new URG_LRF();

            if (!urgLRF.IpOpen(IPAddr, IPPort))
            {
                // Open失敗
                urgLRF = null;
            }
        }

        /// <summary>
        /// LRF初期化　エミュレーションモード
        /// </summary>
        public void Open()
        {
            urgLRF = null;
        }

        /// <summary>
        /// LRFクローズ
        /// </summary>
        public void Close()
        {
            if (null != urgLRF)
            {
                urgLRF.Close();
            }
        }

        /// <summary>
        /// LRFデータ引き渡し
        /// </summary>
        /// <returns></returns>
        public double[] getData()
        {
            return LRF_Data;
        }

        /// <summary>
        /// LRFデータ引き渡し
        /// ノイズリダクションデータ
        /// </summary>
        /// <returns></returns>
        public double[] getData_UntiNoise()
        {
            return LRF_UntiNoiseData;
        }

        /// <summary>
        /// LRFデータ取得
        /// </summary>
        /// <returns>True...データを更新できた, False..更新失敗</returns>
        public bool Update()
        {
            bool rt = false;

            if (null != urgLRF)
            {
                // LRFからデータ取得 (実データ or ログファイル)
                double[] newLRFData = urgLRF.getScanData();

                // データがあるか？
                if (null != newLRFData && newLRFData.Count() > 0)
                {
                    LRF_Data = newLRFData;
                    rt = true;

                    // lrfノイズ リダクション
                    for (int i = 0; i < LRF_Data.Length; i++)
                    {
                        LRF_UntiNoiseData[i] = (LRF_UntiNoiseData[i] * (1.0 - LRF_noiseRate)) + (LRF_Data[i] * LRF_noiseRate);
                    }
                }
            }

            return rt;
        }

        /// <summary>
        /// LRF接続状態取得
        /// </summary>
        /// <returns></returns>
        public bool IsConnect()
        {
            if (null != urgLRF) return true;
            return false;
        }


    }
}
