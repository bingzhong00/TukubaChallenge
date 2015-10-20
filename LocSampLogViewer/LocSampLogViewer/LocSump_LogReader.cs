using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LocSampLogViewer
{
    public class LocSumpLogData
    {
        public long ms;

        // SendData
        public bool bSend;
        public double sendHandle;      // 送信：ハンドル
        public double sendACC;         // 送信：アクセル
        public int sendLED;            // 送信：LEDパターン

        // ResiveData
        public bool bREPlot;
        public double REPlotX;     // RE座標
        public double REPlotY;
        public double REPlotDir;

        public bool bCompus;
        public int CompusDir;      // コンパス角度

        public bool bGPS;
        public double GPSLandX;    // GPS座標
        public double GPSLandY;

        // LocSumpBrainData
        public bool bR1;
        public double R1_X;        // 自己位置推定
        public double R1_Y;
        public double R1_Dir;

        public bool bE1;
        public double E1_X;        // REPlot
        public double E1_Y;
        public double E1_Dir;
    }

    public class LocSumpLogReader
    {
        // ロータリーエンコーダ情報取得　クラス
        // tsukuba20151003152212.out
        private StreamReader fsr;
        private FileStream fst;

        // 識別子
        public const string LocPresumpLogID = "LocPresumpLog";
        /*
         * Log Format
         2015/10/17 15:09:08.151
        hwSendStr:/AC,0.01,0.12

        hwResiveStr:A4,174.8945,-2.71978620181e-14,0.0,0.0$A2,175.0494,343
        $
        handle:0.0100482320785522 / acc:0.12
        R1:X 150/ Y 150/ Dir 0
        V1:X 150/ Y 150/ Dir 0
        E1:X 150/ Y 150/ Dir 0

         */

        /// <summary>
        /// ログファイルオープン
        /// </summary>
        /// <param name="fname"></param>
        /// <returns></returns>
        public bool OpenFile( string fname )
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
        /// データの先頭チェック
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool IsLineHeader(string str)
        {
            string[] stArrayData = str.Split(':');
            if (stArrayData[0] == LocPresumpLogID) return true;
            return false;
        }

        /// <summary>
        /// データの先頭チェック(タイムスタンプ版)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool IsLineHeader_Time(string str)
        {
            int numSep1 = str.Length - str.Replace(":", "").Length;
            int numSep2 = str.Length - str.Replace("/", "").Length;

            if (numSep1 == 2 && numSep2 == 2) return true;
            return false;
        }

        /// <summary>
        /// データ数をカウント
        /// </summary>
        /// <param name="comStr">固有識別子</param>
        /// <returns></returns>
        public int getNumCommand(bool oldType=false)
        {
            seekHead();

            string str;
            int cntMD = 0;
            do {
                str = fsr.ReadLine();
                if (null == str) break;

                if (!oldType)
                {
                    if (IsLineHeader(str)) cntMD++;
                }
                else
                {
                    if (IsLineHeader_Time(str)) cntMD++;
                }
            } while(null!=str);

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
            bool oldVer = false;    // 識別子のない旧バージョンファイル

            if (fsr == null) return new LocSumpLogData[0];

            long allComNum;

            allComNum = getNumCommand();
            if (allComNum == 0)
            {
                allComNum = getNumCommand(true);
                oldVer = true;

                if (allComNum == 0)
                {
                    // 対応するログファイルではない
                    return null;
                }
            }

            LocSumpLogData[] resultData = new LocSumpLogData[allComNum];
            int dataIdx = -1;
            long startMs = 0;

	        do
	        {
		        str = fsr.ReadLine();

                if (str == null) break;
                if (str.Length == 0) continue;
                /*
                 2015/10/17 15:09:08.151
                hwSendStr:/AC,0.01,0.12

                hwResiveStr:A4,174.8945,-2.71978620181e-14,0.0,0.0$A2,175.0494,343
                $
                handle:0.0100482320785522 / acc:0.12
                R1:X 150/ Y 150/ Dir 0
                V1:X 150/ Y 150/ Dir 0
                E1:X 150/ Y 150/ Dir 0
                 */
                if ((!oldVer && IsLineHeader(str)) || (oldVer && IsLineHeader_Time(str)))
                {
                    dataIdx++;

                    string[] strTimeStump = str.Split(' ');
                    string[] strTime = strTimeStump[1].Split(':');
                    // 時、分、秒
                    long readMS = (int.Parse(strTime[0])*(60*60) + int.Parse(strTime[1])*60)*1000 + (int)(double.Parse(strTime[2])*1000);

                    resultData[dataIdx] = new LocSumpLogData();
                    if (dataIdx == 0)
                    {
                        startMs = readMS;
                        resultData[dataIdx].ms = 0;
                    }
                    else
                    {
                        resultData[dataIdx].ms = readMS - startMs;
                    }

                    continue;
                }
                LocSumpLogData lsData = resultData[dataIdx];

                string[] strSplitData = str.Split(':');

                switch (strSplitData[0])
                {
                    case "hwSendStr":
                        {
                            string[] strSendCommand = strSplitData[1].Split('/');
                            lsData.bSend = true;

                            foreach (var OneCommand in strSendCommand)
                            {
                                string[] dataWord = OneCommand.Split(',');

                                switch (dataWord[0])
                                {
                                    case "AC":
                                        lsData.sendHandle = double.Parse(dataWord[1]);
                                        lsData.sendACC = double.Parse(dataWord[2]);
                                        break;
                                    case "AL":
                                        lsData.sendLED = int.Parse(dataWord[1]);
                                        break;

                                    default:// ※未対応
                                        break;
                                }
                            }

                        }
                        break;
                    case "hwResiveStr":
                        {
                            string readLine = strSplitData[1];

                            bool bReadEnd = false;


                            do
                            {
                                string[] strSendCommand = readLine.Split('$');
                                if (readLine[readLine.Length - 1] == '$') bReadEnd = true;

                                foreach (var OneCommand in strSendCommand)
                                {
                                    string[] dataWord = OneCommand.Split(',');
                                    switch (dataWord[0])
                                    {
                                        case "A1": // RE
                                            // ※未対応
                                            break;
                                        case "A2": // Compus
                                            // dataWord[1] ms
                                            lsData.CompusDir = int.Parse(dataWord[2]);
                                            lsData.bCompus = true;
                                            break;
                                        case "A3": // GPS
                                            // dataWord[1] ms
                                            lsData.GPSLandX = double.Parse(dataWord[3]);
                                            lsData.GPSLandY = double.Parse(dataWord[4]);
                                            lsData.bGPS = true;
                                            break;
                                        case "A4": // RE PlotX,Y
                                            // dataWord[1] ms
                                            lsData.REPlotX = double.Parse(dataWord[2]);
                                            lsData.REPlotY = double.Parse(dataWord[3]);
                                            lsData.REPlotDir = double.Parse(dataWord[4]);
                                            lsData.bREPlot = true;
                                            break;
                                    }
                                }

                                if (!bReadEnd)
                                {
                                    readLine = fsr.ReadLine();
                                }
                            } while (!bReadEnd);

                        }
                        break;
                    case "handle":
                        // ※未対応
                        break;
                    case "R1":
                        {
                            lsData.bR1 = true;
                            string[] strSendCommand = strSplitData[1].Split('/');

                            foreach (var OneCommand in strSendCommand)
                            {
                                string[] dataWord = OneCommand.Trim().Split(' ');

                                switch (dataWord[0])
                                {
                                    case "X":
                                        lsData.R1_X = double.Parse(dataWord[1]);
                                        break;
                                    case "Y":
                                        lsData.R1_Y = double.Parse(dataWord[1]);
                                        break;
                                    case "Dir":
                                        lsData.R1_Dir = double.Parse(dataWord[1]);
                                        break;

                                    default:// ※未対応
                                        break;
                                }
                            }
                        }
                        break;
                    case "V1":
                        // ※未対応
                        break;
                    case "E1":
                        {
                            lsData.bE1 = true;
                            string[] strSendCommand = strSplitData[1].Split('/');

                            foreach (var OneCommand in strSendCommand)
                            {
                                string[] dataWord = OneCommand.Trim().Split(' ');

                                switch (dataWord[0])
                                {
                                    case "X":
                                        lsData.E1_X = double.Parse(dataWord[1]);
                                        break;
                                    case "Y":
                                        lsData.E1_Y = double.Parse(dataWord[1]);
                                        break;
                                    case "Dir":
                                        lsData.E1_Dir = double.Parse(dataWord[1]);
                                        break;

                                    default:// ※未対応
                                        break;
                                }
                            }
                        }
                        break;
                }

	        }
            while (true);

            return resultData;
        }

    }
}
