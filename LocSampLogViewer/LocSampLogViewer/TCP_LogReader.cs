using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LocSampLogViewer
{
    public class TCP_LogReader
    {
        // ロータリーエンコーダ情報取得　クラス
        // tsukuba20151003152212.out
        private StreamReader fsr;
        private FileStream fst;

        /*
         * Log Format
        A2,139.2375,347
        $
        A2,139.3464,347
        $A3,139.3468,36.7972,140.1156$

         */

        /// <summary>
        /// ログファイルオープン
        /// </summary>
        /// <param name="fname"></param>
        /// <returns></returns>
        public bool OpenFile(string fname)
        {
            try
            {
                fst = new FileStream(fname, FileMode.Open);
                fsr = new StreamReader(fst, Encoding.GetEncoding("Shift_JIS"));
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ログファイルクローズ
        /// </summary>
        public void CloseFile()
        {
            if (null != fsr)
            {
                fsr.Close();
                fsr = null;
            }
            if (null != fst)
            {
                fst.Close();
                fst = null;
            }
        }

        /// <summary>
        /// ファイルの先頭へシーク
        /// </summary>
        public void seekHead()
        {
            fst.Seek(0, SeekOrigin.Begin);
            fsr = new StreamReader(fst, Encoding.GetEncoding("Shift_JIS"));
        }

        /// <summary>
        /// データ数をカウント
        /// </summary>
        /// <returns></returns>
        public int getNumCommand()
        {
            seekHead();

            // 分の末尾が「$」かで判断

            string str;
            int cntMD = 0;
            do
            {
                str = fsr.ReadLine();
                if (null == str) break;

                if (str.Length > 0 && str.Substring(str.Length - 1, 1) == "$")
                {
                    cntMD++;
                }

            } while (null != str);

            seekHead();
            return cntMD;
        }

        /// <summary>
        /// データ取得
        /// </summary>
        /// <returns>double配列</returns>
        public LocSumpLogData[] getScanData()
        {
            string str;

            if (fsr == null) return new LocSumpLogData[0];

            long allComNum;

            allComNum = getNumCommand();
            if (allComNum == 0)
            {
                allComNum = getNumCommand();

                if (allComNum == 0)
                {
                    // 対応するログファイルではない
                    return null;
                }
            }

            LocSumpLogData[] resultData = new LocSumpLogData[allComNum];
            int dataIdx = 0;
            //            long startMs = 0;
            resultData[dataIdx] = new LocSumpLogData();
            LocSumpLogData lsData = resultData[dataIdx];

            do
            {
                str = fsr.ReadLine();

                if (str == null) break;
                if (str.Length == 0) continue;


                string[] strSplitData = str.Split('$');

                for (int i = 0; i < strSplitData.Length; i++)
                {
                    string[] dataWord = strSplitData[i].Split(',');

                    switch (dataWord[0])
                    {
                        case "A1": // RE
                            lsData.ms = ParseMS(dataWord[1]);
                            if (dataWord.Length > 2)
                            {
                                lsData.REL = long.Parse(dataWord[2]);
                                lsData.RER = long.Parse(dataWord[3]);
                            }
                            lsData.bRE = true;
                            break;
                        case "A2": // Compus
                            lsData.ms = ParseMS(dataWord[1]);
                            lsData.CompusDir = int.Parse(dataWord[2]);
                            lsData.bCompus = true;
                            break;
                        case "A3": // GPS
                            lsData.ms = ParseMS(dataWord[1]);
                            {
                                string[] dataGPS = dataWord[2].Split('.');
                                lsData.GPSLandX = double.Parse(dataGPS[0]) + (double.Parse(dataGPS[1]) / 60.0);
                            }
                            {
                                string[] dataGPS = dataWord[3].Split('.');
                                lsData.GPSLandY = double.Parse(dataGPS[0]) + (double.Parse(dataGPS[1]) / 60.0);
                            }
                            lsData.bGPS = true;
                            break;
                        case "A4": // RE PlotX,Y
                            lsData.ms = ParseMS(dataWord[1]);
                            lsData.REPlotX = double.Parse(dataWord[2]);
                            lsData.REPlotY = double.Parse(dataWord[3]);
                            lsData.REPlotDir = double.Parse(dataWord[4]);
                            lsData.bREPlot = true;
                            break;
                    }
                }

                if (str.Length > 0 && str.Substring(str.Length - 1, 1) == "$")
                {
                    // １データ完了
                    if (dataIdx < resultData.Length-1)
                    {
                        dataIdx++;

                        resultData[dataIdx] = new LocSumpLogData();
                        lsData = resultData[dataIdx];
                    }
                }

            }
            while (true);


            return resultData;
        }

        private long ParseMS( string str )
        {
            return (long)(double.Parse(str) * 1000.0);
        }

    }

}
