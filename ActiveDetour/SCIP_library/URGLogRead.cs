using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.IO;

namespace SCIP_library
{
    /// <summary>
    /// ログデータ読み込み
    /// </summary>
    class URGLogRead
    {
        private StreamReader fsr;
        
        public static double scale = 1.0;
        public long skipNum = 0;
        public long skipCnt = 0;


        public static void SetScale(double scl)
        {
            scale = scl;
        }


        // データスキップ機能
        // MDをいくつかスキップさせる機能(時間短縮)
        public void SetSkipNum(long num)
        {
            skipNum = num;
        }

        /// <summary>
        /// char -> 数値変換
        /// </summary>
        /// <param name="code"></param>
        /// <param name="numByte"></param>
        /// <returns></returns>
        private int decode(char[] code, int numByte)
        {
	        int value = 0;
	        int i;

            for (i = 0; i < numByte; ++i)
            {
		        value <<= 6;
		        value &= ~0x3f;
		        value |= code[i] - 0x30;
	        }
	        return value;
        }

        /// <summary>
        /// ログファイルオープン
        /// </summary>
        /// <param name="fname"></param>
        /// <returns></returns>
        public bool OpenFile( string fname )
        {
            try
            {
                fsr = new StreamReader(
                        fname, Encoding.GetEncoding("Shift_JIS"));
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
        }

        /// <summary>
        /// データ取得
        /// </summary>
        /// <returns>double配列</returns>
        public double[] getScanData()
        {
	        string str;
	        string strCmd;

            if (fsr == null) return new double[0];

            skipCnt = skipNum;

	        do
	        {
		        str = fsr.ReadLine();
                if( str == null ) return new double[0];
                if (str.Length == 0) continue;


		        strCmd = str.Substring(0,2);

		        if( strCmd.Equals("MD") )
                {

                    int dtNum;
                    double[] resultData;


                    // MDxxxxyyyyaabcc
                    // xxxx...開始インデックス
                    // yyyy...終了インデックス
                    // aa  距離データをまとめる取得データ数
                    // b   スキャンを何周期に１回行うか (MD, MS コマンドのみ)
                    // cc  データ取得回数 (MD, MS コマンドのみ)

                    // インデックスからデータ個数を算出
                    {
                        int dtLen = int.Parse(str.Substring(6, 4)) - int.Parse(str.Substring(2, 4));

                        int MDaa = int.Parse(str.Substring(10, 2));
                        if (MDaa <= 0) MDaa = 1;

                        dtNum = dtLen / MDaa;
                    }

                    str = fsr.ReadLine();	// 結果
                    if (str.Equals("00P")) continue;

                    str = fsr.ReadLine();	//  タイムスタンプ (ms)
                    {
                        int tmstmp = decode(str.Substring(0, 3).ToCharArray(), 3);
                        /*
				        if( nextMS == 0 )
				        {
					        // 初期値
					        nextMS = tmstmp;
					        startMS -= tmstmp;
				        } else {
					        nextMS = tmstmp;
				        }
                         */

                    }

                    // 距離データ　デコード
                    {
                        string strBuf;
                        int dist;

                        strBuf = "";

                        resultData = new double[dtNum];

                        for (int i = 0; i < dtNum; i++)
                        {
                            if (strBuf.Length < 3)
                            {
                                str = fsr.ReadLine();
                                strBuf += str.Substring(0, str.Length - 1);
                            }
                            dist = decode(strBuf.Substring(0, 3).ToCharArray(), 3);

                            resultData[i] = (double)dist * scale;

                            strBuf = strBuf.Substring(3);
                        }
                    }

                    if (skipCnt-- > 0) continue;

                    // 次回持越し
                    return resultData;
                }
		        else if ( strCmd.Equals("QT"))
		        {
			        /*
			        レーザを消灯させます。
			        レーザを消灯させるとともに、距離測定コマンドである "MD", "MS" による距離測定も停止させます。 
			        */
			        str = fsr.ReadLine();	// 結果
		        }
		        else if ( strCmd.Equals("PP"))
		        {
                    str = fsr.ReadLine();   // 結果
			        /*
			        MODL:UTM-30LX-EW;I
			        DMIN:23;7
			        DMAX:60000;J
			        ARES:1440;^
			        AMIN:0;?
			        AMAX:1080;Z
			        AFRT:540;0
			        SCAN:2400;U
			        */
			        /*
			        MODL ... センサ型式情報
			        DMIN ... 最小計測可能距離 (mm)
			        DMAX ... 最大計測可能距離 (mm)
			        ARES ... 角度分解能(360度の分割数)
			        AMIN ... 最小計測可能方向値
			        AMAX ... 最大計測可能方向値
			        AFRT ... 正面方向値
			        SCAN ... 標準操作角速度 
			        */
			        fsr.ReadLine();
			        fsr.ReadLine();
			        fsr.ReadLine();
			        fsr.ReadLine();
			        fsr.ReadLine();
			        fsr.ReadLine();
			        fsr.ReadLine();
			        fsr.ReadLine();
		        }

		        /*
		        "TM0", "TM1", "TM2"
		        タイムスタンプモードとのモード遷移を行います。
		        タイムスタンプモードでは、センサは "TM1" に対して即座に現在のタイムスタンプを返します。これにより、PC と URG とのタイムスタンプ同期を行うことができます。
		        "TM0" ... タイムスタンプモードへの遷移を指示する
		        "TM1" ... URG のタイムスタンプを返す
		        "TM2" ... 通常のモードへの遷移を指示する

		        "SS"

		        通信ボーレートの変更を行います。
		        ボーレートが実際に変更されるのは、"SS" の応答完了後になります。
		        指定できるボーレートは以下の通りです。(ただし、PC 側のボーレートは、通常 115200 より高い値には設定できません。)
		        750000 (bps)
		        500000 (bps)
		        250000 (bps)
		        115200 (bps)
		        38400 (bps)
		        19200 (bps)

		        "BM"

		        レーザを点灯させます。
		        距離測定コマンドである、"GD", "GS" を使う場合には、あらかじめ、このコマンドを使ってレーザを点灯させておく必要があります。(同じ距離測定コマンドの "MD", "MS" では、測定開始時にレーザを自動で点灯させるため、"BM" を送信する必要はありません。)

		        "RS"

		        ステータス情報をリセットします。
		        リセットする情報は、レーザ点灯、モータの回転速度、通信ボーレートなどです。
		        レーザを消灯 (距離データ取得も停止)
		        通信ボーレートをデフォルト設定(通常は 19200) に変更
		        モータの回転速度を、通常速の 100 % に設定 ただし、このコマンドは、タイムアウトモードから通常モードへはモードを遷移させません。タイムアウトモードから通常モードへ状態を変更するのは、"TM2" コマンドのみです。
		        */
	        }
            while (true);
        }

    }
}
