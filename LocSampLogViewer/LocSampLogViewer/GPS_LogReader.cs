using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LocSampLogViewer
{
    public class GPS_LogReader
    {
        // ロータリーエンコーダ情報取得　クラス
        // tsukuba20151003152212.out
        private StreamReader fsr;
        private FileStream fst;

        // 緯度、経度 １分の距離[mm]
        const double GPSScale = 1.852*1000.0*1000.0;
        const double GPSScaleX = 1.51985 * 1000.0 * 1000.0;    // 経度係数  35度時
        const double GPSScaleY = 1.85225 * 1000.0 * 1000.0;    // 緯度係数

        /*
         * Log Format
            $GPRMC,020848.000,A,3604.8100,N,14006.9366,E,0.03,15.03,171015,,,A*5E
            $GPGGA,020849.000,3604.8100,N,14006.9366,E,1,10,1.2,37.4,M,39.0,M,,0000*62
            $GPGSA,A,3,03,57,28,19,17,01,11,32,04,23,,,2.2,1.2,1.8*38
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

            string str;
            int cntMD = 0;
            do
            {
                str = fsr.ReadLine();
                if (null == str) break;

                if (str.Length > 0 && str.IndexOf("$GPRMC") >= 0)
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
            LocSumpLogData lsData;
            int dataIdx = -1;

            do
            {
                str = fsr.ReadLine();

                if (str == null) break;
                if (str.Length == 0) continue;

                string[] dataWord = str.Split(',');



                switch (dataWord[0])
                {
                    case "$GPRMC": // RE
                        {
                            double ido = 0.0;
                            dataIdx++;

                            resultData[dataIdx] = new LocSumpLogData();
                            lsData = resultData[dataIdx];

                            // 定世界時(UTC）での時刻。日本標準時は協定世界時より9時間進んでいる。hhmmss.ss
                            lsData.ms = ParseMS(dataWord[1]);

                            // dataWord[2] A,V  ステータス。V = 警告、A = 有効

                            {
                                // dataWord[3] 緯度。dddmm.mmmm
                                string[] dataGPS = dataWord[3].Split('.');
                                //ido = double.Parse(dataGPS[0]);
                                //lsData.GPSLandY = -(double.Parse(dataGPS[0]) + (double.Parse(dataGPS[1])/60.0)) * GPSScale;
                                string strDo = dataWord[3].Substring(0, dataGPS[0].Length - 2);
                                string strHun = dataWord[3].Substring(strDo.Length, dataWord[3].Length - strDo.Length);
                                lsData.GPSLandY = -(double.Parse(strDo) * 60.0 + (double.Parse(strHun))) * GPSScaleY;//GPSScale;
                                ido = double.Parse(strDo);
                            }
                            // dataWord[4] N,S N = 北緯、South = 南緯

                            {
                                // dataWord[5] 経度。dddmm.mmmm
                                string[] dataGPS = dataWord[5].Split('.');

                                //lsData.GPSLandX = (double.Parse(dataGPS[0]) + (double.Parse(dataGPS[1])) / 60.0) * (GPSScale * Math.Cos((ido / 100.0) * Math.PI / 180.0));
                                string strDo = dataWord[5].Substring(0, dataGPS[0].Length - 2);
                                string strHun = dataWord[5].Substring(strDo.Length, dataWord[5].Length - strDo.Length);
                                lsData.GPSLandX = (double.Parse(strDo) * 60.0 + (double.Parse(strHun))) * GPSScaleX;//(GPSScale * Math.Cos(ido * Math.PI / 180.0));
                            }
                            // dataWord[6] E = 東経、West = 西経

                            // dataWord[7] 地表における移動の速度。000.0～999.9[knot]
                            // dataWord[8] 地表における移動の真方位。000.0～359.9度
                            // dataWord[9] 協定世界時(UTC）での日付。ddmmyy
                            // dataWord[10] 磁北と真北の間の角度の差。000.0～359.9度 	
                            // dataWord[11] 磁北と真北の間の角度の差の方向。E = 東、W = 西 	
                            // dataWord[12] モード, N = データなし, A = Autonomous（自律方式）, D = Differential（干渉測位方式）, E = Estimated（推定）
                            // dataWord[13] チェックサム

                            lsData.bGPS = true;
                        }
                        break;
                    case "$GPGGA":
                        // ※未対応
                        break;
                    case "$GPGSA":
                        // ※未対応
                        break;
                }
                

                if (str.Length > 0 && str.Substring(str.Length - 1, 1) == "$")
                {
                    // １データ完了
                    if (dataIdx < resultData.Length - 1)
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

        private long ParseMS(string str)
        {
            return (long)(double.Parse(str) * 1000.0);
        }
    }
}
