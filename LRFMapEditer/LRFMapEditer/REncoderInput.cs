using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CersioIO
{
    public class REncoderInput
    {
        // ロータリーエンコーダ情報取得　クラス
        // tsukuba20151003152212.out
        private StreamReader fsr;
        private FileStream fst;
        
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

        public void seekHead()
        {
            fst.Seek(0, SeekOrigin.Begin);
            fsr = new StreamReader(fst, Encoding.GetEncoding("Shift_JIS"));
        }

        // データ数をカウント
        public int getNumCommand(string comStr )
        {
            seekHead();

            string str;
            int cntMD = 0;
            do {
                str = fsr.ReadLine();
                if (null == str) break;

                string[] stArrayData = str.Split(' ');
                foreach (var readCom in stArrayData)
                {
                    if (readCom == comStr)
                    {
                        cntMD++;
                        break;
                    }
                }
            } while(null!=str);

            seekHead();
            return cntMD;
        }

        /// <summary>
        /// データ取得
        /// </summary>
        /// <returns>double配列</returns>
        public double[] getScanData()
        {
	        string str;

            if (fsr == null) return new double[0];

            long allComNum = getNumCommand("delta0");
            long comIdx=0;

            double[] resultData = new double[allComNum*4];

            // 1行目は捨てる
	        str = fsr.ReadLine();

	        do
	        {
		        str = fsr.ReadLine();

                if (str == null) break;
                if (str.Length == 0) continue;

                string[] stArrayData = str.Split(' ');
                //foreach (var strCmd in stArrayData)
                for(int i=0; i< stArrayData.Length;i++)
                {
                    string strCmd = stArrayData[i];

                    if (strCmd.Equals("delta0"))
                    {
                        //          0            1 2    3            4    5     6      7 8   9     10 11 12   13  14 15   16 17 18 
                        // 2015-10-03 15:22:56,461 - root            - INFO     - delta0 = 0.0 delta1 = 0.0 right = 0.0 left = 0.0
                        double tryValue, tryR, tryL;

                        double.TryParse(stArrayData[i+2], out tryValue);
                        resultData[comIdx * 4 + 0] = tryValue;

                        double.TryParse(stArrayData[i+5], out tryValue);
                        resultData[comIdx * 4 + 1] = tryValue;

                        double.TryParse(stArrayData[i + 8], out tryR);
                        resultData[comIdx * 4 + 2] = tryR;

                        double.TryParse(stArrayData[i + 11], out tryL);
                        resultData[comIdx * 4 + 3] = tryL;

                        // 停止は捨てる
                        if (comIdx > 0 && (tryR == resultData[(comIdx - 1) * 4 + 2] && tryL == resultData[(comIdx-1)* 4 + 3]))
                        {
                            comIdx--;
                        }

                        comIdx++;

                    }
                }
                /*
		        else if ( strCmd.Equals("QT"))
		        {
			        str = fsr.ReadLine();	// 結果
		        }
                */
	        }
            while (true);

            double[] resultDataRcnt = new double[comIdx * 4];
            for (int i = 0; i < comIdx * 4; i++)
            {
                resultDataRcnt[i] = resultData[i];
            }

            return resultDataRcnt;
        }

    }
}
