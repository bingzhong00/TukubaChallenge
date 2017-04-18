using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

using System.IO;

using Location;
using CersioIO;
using Navigation;

// .Todo
// ・センサーデータを受信したそのままを出力するログ用意

namespace VehicleRunner
{
    class VehicleRunnerForm_Log
    {
        // ログファイル名
        public static string saveLogFname;
        public static string saveGPSLogFname;
        public static string saveLRFLogFname;

        /// <summary>
        /// タイムスタンプファイル作成
        /// </summary>
        /// <returns></returns>
        public string GetTimeStampFileName(string strPrev, string strExt)
        {
            return strPrev + DateTime.Now.ToString("yyyyMMdd_HHmmss") + strExt;
        }

        public void init()
        {
            // ログファイル名生成
            saveLogFname = GetTimeStampFileName("./Log/LocSampLog", ".log");
            saveGPSLogFname = GetTimeStampFileName("./Log/GPSLog", ".log");
            saveLRFLogFname = GetTimeStampFileName("./Log/LRFLog", ".log");

            // ログファイルディレクトリ確認
            {
                string logDir = Path.GetDirectoryName(saveLogFname);
                if (!File.Exists(logDir))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(logDir);
                    }
                    catch
                    {
                        MessageBox.Show("ログファイルフォルダの作成に失敗: " + logDir);
                    }
                }
            }
        }

        /// <summary>
        /// マップイメージログ出力
        /// </summary>
        /// <param name="BrainCtrl"></param>
        /// <returns></returns>
        public bool Output_ImageLog(ref Brain BrainCtrl )
        {
            try
            {
                // 軌跡ログ出力
                if (!string.IsNullOrEmpty(saveLogFname))
                {
                    /*
                    MarkPoint tgtMaker = null;

                    // 次の目的地取得
                    {
                        int tgtPosX = 0;
                        int tgtPosY = 0;
                        double dir = 0;

                        BrainCtrl.RTS.getNowTarget(ref tgtPosX, ref tgtPosY);
                        BrainCtrl.RTS.getNowTargetDir(ref dir);

                        tgtMaker = new MarkPoint(tgtPosX, tgtPosY, dir);
                    }

                    {
                        Bitmap bmp = BrainCtrl.LocSys.MakeMakerLogBmp(false, tgtMaker);
                        if (null != bmp)
                        {
                            // 画像ファイル保存
                            string saveImageLogFname = Path.ChangeExtension(saveLogFname, "png");
                            bmp.Save(saveImageLogFname, System.Drawing.Imaging.ImageFormat.Png);
                        }
                    }
                    */
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return false;
            }

            return true;
        }

        /// <summary>
        /// LRFデータログ出力
        /// テキスト
        /// </summary>
        /// <param name="LRF_Data"></param>
        /// <returns></returns>
        public bool Output_LRFLog(double[] LRF_Data)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(saveLRFLogFname, true, System.Text.Encoding.GetEncoding("shift_jis"));

            sw.Write("LRFLog:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + System.Environment.NewLine);

            for (int i = 0; i < LRF_Data.Length; i++)
            {
                sw.Write(LRF_Data[i].ToString("F0") + ",");
            }
            sw.Write(System.Environment.NewLine);

            sw.Close();

            return true;
        }

        /// <summary>
        /// GPS受信ログ
        /// テキスト
        /// </summary>
        /// <param name="reciveStr"></param>
        /// <returns></returns>
        public bool Output_GPSLog( string reciveStr)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(saveGPSLogFname, true, System.Text.Encoding.GetEncoding("shift_jis"));
            sw.Write(reciveStr);
            sw.Close();

            return true;
        }

        /// <summary>
        /// VehicleRunnerログ出力
        /// </summary>
        public void Output_VRLog(ref Brain BrainCtrl, ref CersioCtrl CersioCt)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(saveLogFname, true, System.Text.Encoding.GetEncoding("shift_jis"));

            LocationSystem LocSys = BrainCtrl.LocSys;

            // 固有識別子 + 時間
            sw.Write("LocPresumpLog:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + System.Environment.NewLine);

            // ハードウェア情報
            if (CersioCt.TCP_IsConnected())
            {
                if (null != CersioCt.hwSendStr)
                {
	                sw.Write("hwSendStr:" + CersioCt.hwSendStr.Replace('\n', ' ') + System.Environment.NewLine);
				}
                if (null != CersioCt.hwResiveStr)
                {
                    sw.Write("hwResiveStr:" + CersioCt.hwResiveStr + System.Environment.NewLine);
                }
                sw.Write("handle:" + CersioCtrl.nowSendHandleValue + " / acc:" + CersioCtrl.nowSendAccValue + System.Environment.NewLine);
            }
            else
            {
                sw.Write("Comment:No Connect BoxPC" + System.Environment.NewLine);
            }
            

            // 位置情報
            {
                sw.Write("R1:X " + LocSys.R1.X.ToString("f3") +
                         "/Y " + LocSys.R1.Y.ToString("f3") +
                         "/ Dir " + LocSys.R1.Theta.ToString("f2") +
                         System.Environment.NewLine);
            }

            // 
            {
                // Rooting情報
                {
                    sw.Write("RTS_TargetIndex:" + BrainCtrl.LocSys.RTS.getNowCheckPointIdx() + System.Environment.NewLine);

                    int tgtPosX = 0;
                    int tgtPosY = 0;
                    double tgtDir = 0;

                    //BrainCtrl.LocSys.RTS.getNowTarget(ref tgtPosX, ref tgtPosY);
                    BrainCtrl.LocSys.RTS.getNowTargetDir(ref tgtDir);

                    sw.Write("RTS_TargetPos:X " + tgtPosX.ToString("f3") +
                             "/Y " + tgtPosY.ToString("f3") +
                             "/Dir " + tgtDir.ToString("f2") +
                             System.Environment.NewLine);

                    sw.Write("RTS_Handle:" + BrainCtrl.LocSys.RTS.getHandleValue().ToString("f2") + System.Environment.NewLine);
                }

                // CersioCtrl
                {
                    sw.Write("GoalFlg:" + (BrainCtrl.goalFlg ? "TRUE" : "FALSE") + System.Environment.NewLine);
                }

                // 特記事項メッセージ出力
                if (Brain.addLogMsg != null)
                {
                    sw.Write("AddLog:" + Brain.addLogMsg + System.Environment.NewLine);
                }
                Brain.addLogMsg = null;
            }
            // 改行
            sw.Write(System.Environment.NewLine);

            //閉じる
            sw.Close();
        }


        /// <summary>
        /// ログ用のバッファクリア
        /// </summary>
        /// <param name="BrainCtrl"></param>
        /// <param name="CersioCt"></param>
        /// <param name="LocSys"></param>
        public void LogBuffer_Clear(ref Brain BrainCtrl, ref CersioCtrl CersioCt)
        {
            if (null != CersioCt)
            {
                CersioCt.hwSendStr = "";
                CersioCt.hwResiveStr = "";
            }
        }

        /// <summary>
        /// Trace出力ファイルをTraceListenerに登録します
        /// </summary>
        /// <param name="filePath"></param>
        public static void RegistTraceLog(string filePath)
        {
            //DefaultTraceListenerオブジェクトを取得
            DefaultTraceListener drl;
            drl = (DefaultTraceListener)Trace.Listeners["Default"];
            //LogFileNameを変更する
            drl.LogFileName = filePath;
        }

    }
}
