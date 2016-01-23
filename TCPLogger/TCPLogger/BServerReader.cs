using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPLogger
{
    class BServerReader
    {
        public int hwCompass;
        public bool bhwCompass = false;

        public double hwGPS_LandX;
        public double hwGPS_LandY;
        public bool bhwGPS = false;

        public double hwREX;
        public double hwREY;
        public double hwREDir;
        public bool bhwREPlot = false;

        public double emuGPSX = 134.0000;
        public double emuGPSY = 35.0000;

        /// <summary>
        /// BServer受信内容解析
        /// </summary>
        /// <param name="hwResiveStr"></param>
        /// <returns></returns>
        public void TCP_ReciveCommand(string readStr)
        {
            {
                string[] rsvCmd = readStr.Split('$');

                for (int i = 0; i < rsvCmd.Length; i++)
                {
                    if (rsvCmd[i].Length <= 3) continue;

                    // ロータリーエンコーダから　速度を計算
                    if (rsvCmd[i].Substring(0, 3) == "A1,")
                    {
                        /*
                        const double tiyeSize = 140.0;  // タイヤ直径 [mm]
                        const double OnePuls = 260.0;   // 一周のパルス数
                        double ResiveMS;
                        //string[] splStr = (rsvCmd[i].Replace('[', ',').Replace(']', ',').Replace(' ', ',')).Split(',');
                        string[] splStr = rsvCmd[i].Split(',');

                        // 0 A1
                        double.TryParse(splStr[1], out ResiveMS); // ms? 万ミリ秒に思える
                        double.TryParse(splStr[2], out hwRErotR);        // Right Wheel
                        double.TryParse(splStr[3], out hwRErotL);        // Left Wheel

                        // 絶対値用計算 10000  万ミリ秒
                        SpeedMH = (int)(((double)(hwRErotR - oldWheelR) / OnePuls * Math.PI * tiyeSize) * 10000.0 / (ResiveMS - oldResiveMS));
                        // mm/sec
                        //SpeedMH = (int)(((double)wheelR / 260.0 * Math.PI * 140.0) * 10000.0 / 200.0);

                        oldResiveMS = ResiveMS;
                        oldWheelR = hwRErotR;

                        bhwRE = true;
                            * */
                    }
                    else if (rsvCmd[i].Substring(0, 3) == "A2,")
                    {
                        // コンパス情報
                        // A2,22.5068,210$
                        double ResiveMS;
                        int ResiveCmp;
                        string[] splStr = rsvCmd[i].Split(',');

                        // splStr[0] "A2"
                        // ミリ秒取得
                        double.TryParse(splStr[1], out ResiveMS); // ms? 万ミリ秒に思える
                        int.TryParse(splStr[2], out ResiveCmp);   // デジタルコンパス値
                        hwCompass = -ResiveCmp;     // 回転方向が+-逆なので合わせる
                        bhwCompass = true;
                    }
                    else if (rsvCmd[i].Substring(0, 3) == "A3,")
                    {
                        // GPS情報
                        // $A3,38.266,36.8002,140.11559$
                        double ResiveMS;
                        double ResiveLandX; // 緯度
                        double ResiveLandY; // 経度
                        string[] splStr = rsvCmd[i].Split(',');

                        // splStr[0] "A3"
                        // ミリ秒取得
                        double.TryParse(splStr[1], out ResiveMS); // ms? 万ミリ秒に思える

                        double.TryParse(splStr[2], out ResiveLandX);   // GPS値
                        double.TryParse(splStr[3], out ResiveLandY);
                        hwGPS_LandX = ResiveLandX;
                        hwGPS_LandY = ResiveLandY;
                        bhwGPS = true;
                    }
                    else if (rsvCmd[i].Substring(0, 3) == "A4,")
                    {
                        // ロータリーエンコーダ　絶対値
                        // 開始時　真北基準
                        /*
                            * コマンド
                            A4
                            ↓
                            戻り値
                            A4,絶対座標X,絶対座標Y,絶対座標上での向きR$

                            絶対座標X[mm]
                            絶対座標Y[mm]
                            絶対座標上での向き[rad]　-2π～2π
                            浮動小数点です。
                            */
                        double ResiveMS;
                        double ResiveX;
                        double ResiveY;
                        double ResiveRad;

                        string[] splStr = rsvCmd[i].Split(',');

                        // splStr[0] "A2"
                        // ミリ秒取得
                        double.TryParse(splStr[1], out ResiveMS); // ms? 万ミリ秒に思える
                        double.TryParse(splStr[2], out ResiveX);  // 雑体座標X mm
                        double.TryParse(splStr[3], out ResiveY);  // 雑体座標Y mm
                        double.TryParse(splStr[4], out ResiveRad);  // 向き -2PI 2PI

                        // 座標系変換
                        // 右上から右下へ
                        hwREX = ResiveX;
                        hwREY = ResiveY;
                        hwREDir = ResiveRad * 180.0 / Math.PI;

                        bhwREPlot = true;
                    }

                }
            }
        }

    }
}
