using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRunner
{
    class VRSetting
    {
        /// <summary>
        /// 設定ファイル名
        /// </summary>
        public static string FileName = "VRSetting.xml";


        /// <summary>
        /// bServer IP Address
        /// </summary>
        public string bServerIPAddr = "192.168.1.101";

        /// <summary>
        /// bServer エミュレータ IP Address
        /// </summary>
        private string bServerEmuIPAddr = "127.0.0.1";

        /// <summary>
        /// bServer Port
        /// </summary>
        public int bServerIPPort = 50001;

        // ハンドル、アクセル上限値
        public double HandleLimit = 1.0;
        public double AccLimit = 0.5;

        /// <summary>
        /// 起動時のマップファイル名
        /// </summary>
        public string defaultMapFileName = "../../../MapFile/utsubo201608/utsubo201608.xml";


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
    }

}
