using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRSystemConfig
{
    static public class VRSetting
    {
        /// <summary>
        /// 設定ファイル名
        /// </summary>
        public const string FileName = "VRSetting.xml";

        /// <summary>
        /// 
        /// </summary>
        // public const Cersio carType;
#if true
        // Betz
        public const string CarName = "Benz";
        public const double TireSize = 240.0;   // タイヤ直径 [mm]
        public const double OnePulse = 240.0;   // １周のパルス値
        public const double PulseRateL = 1.0;   // 左車輪のパルス値の係数
        public const double PulseRateR = 1.0;   // 右車輪のパルス値の係数
        public const double AxleShaftLength = 550.0;   // アクスルシャフト長さ [mm]（左右の車輪のシャフトの長さ）
#else
        // Cersio
        public const string CarName = "Cersio";
        public const double TireSize = 65.0;   // タイヤ直径 [mm]
        public const double OnePulse = 240.0;   // １周のパルス値
        public const double PulseRateL = 0.9917;   // 左車輪のパルス値の係数
        public const double PulseRateR = 1.0;   // 右車輪のパルス値の係数
        public const double AxleShaftLength = 150.0; // アクスルシャフト長さ [mm]（左右の車輪のシャフトの長さ）
#endif

        /// <summary>
        /// bServer IP Address
        /// </summary>
        //public const string bServerIPAddr = "192.168.1.101";
        public const string bServerIPAddr = "192.168.1.46";  // dynabook emu

        /// <summary>
        /// bServer エミュレータ IP Address
        /// </summary>
        public const string bServerEmuIPAddr = "127.0.0.1";

        /// <summary>
        /// bServer Port
        /// </summary>
        public const int bServerIPPort = 50001;

        /// <summary> ハンドル上限値 </summary>
        public const double HandleLimit = 1.0;
        /// <summary> アクセル上限値 </summary>
        public const double AccLimit = 0.5;

        /// <summary>
        /// 起動時のマップファイル名
        /// </summary>
        //public const string defaultMapFileName = "../../../MapFile/utsubo20160812/utsubo20160812.xml";
        public const string defaultMapFileName = "../../../MapFile/husky/check_point20170806_215906.xml";

        /// <summary>
        /// チェックポイントに近づく距離(半径) [m]
        /// </summary>
        public const double touchRange = 0.5;


        /// <summary>
        /// 最大ハンドル角
        /// 角度から　左　1.0　～　右　-1.0の範囲に変換に使う
        /// 値が大きいほど、変化が緩やかになる
        /// </summary>
        public const double MaxHandleAngle = 20.0;

        // ハンドル、アクセルの変化係数
        public const double HandleControlPow = 0.25;//0.10;
        /// <summary> 加速時の緩やかさ </summary>
        public const double AccControlPowUP = 0.050;
        /// <summary> 減速時の緩やかさ </summary>
        public const double AccControlPowDOWN = 0.050;

        /// <summary> 移動速度[km] </summary>
        public const double AccSpeedKm = 1.4;

        /*
                /// <summary>
                /// Settingファイル読み込み
                /// </summary>
                /// <param name="fileName"></param>
                /// <returns></returns>
                public static VRSetting LoadMapFile()
                {
                    //保存するクラス(SampleClass)のインスタンスを作成
                    VRSetting settingData;

                    //XmlSerializerオブジェクトを作成
                    //オブジェクトの型を指定する
                    System.Xml.Serialization.XmlSerializer serializer =
                        new System.Xml.Serialization.XmlSerializer(typeof(VRSetting));

                    //書き込むファイルを開く（UTF-8 BOM無し）
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(FileName, new System.Text.UTF8Encoding(false)))
                    {
                        //シリアル化し、XMLファイルから読み込む
                        settingData = (VRSetting)serializer.Deserialize(sr);

                        //ファイルを閉じる
                        sr.Close();
                    }

                    return settingData;
                }

                /// <summary>
                /// Settingファイル保存
                /// </summary>
                /// <param name="fileName"></param>
                /// <returns></returns>
                public bool SaveMapFile()
                {
                    //XmlSerializerオブジェクトを作成
                    //オブジェクトの型を指定する
                    System.Xml.Serialization.XmlSerializer serializer =
                        new System.Xml.Serialization.XmlSerializer(typeof(VRSetting));

                    //書き込むファイルを開く（UTF-8 BOM無し）
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(FileName, false, new System.Text.UTF8Encoding(false)))
                    {
                        //シリアル化し、XMLファイルに保存する
                        serializer.Serialize(sw, this);
                        //ファイルを閉じる
                        sw.Close();
                    }

                    return true;
                }
        */
    }

}
