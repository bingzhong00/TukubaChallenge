using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiom.Math;

namespace Navigation
{
    public class MapData
    {
        /// <summary>
        /// Map名
        /// </summary>
        public string MapName;

        /// <summary>
        /// Mapファイル
        /// </summary>
        public string MapFileName;

        /// <summary>
        /// Map画像ファイル
        /// </summary>
        public string MapImageFileName;

        /// <summary>
        /// 実際のマップの横幅 [mm]
        /// </summary>
        public double RealWidth;

        /// <summary>
        /// 実際のマップの縦幅 [mm]　真北を縦とする
        /// </summary>
        public double RealHeight;

        /// <summary>
        /// スタート位置
        /// ピクセル座標
        /// </summary>
        public Vector3 startPosition;

        /// <summary>
        /// スタートの向き
        /// </summary>
        public double startDir;

        /// <summary>
        /// チェックポイント配列
        /// </summary>
        public Vector3[] checkPoint;

        /// <summary>
        /// Mapファイル読み込み
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static MapData LoadMapFile(string fileName)
        {
            //保存するクラス(SampleClass)のインスタンスを作成
            MapData mapFile;// = new MapData();

            //XmlSerializerオブジェクトを作成
            //オブジェクトの型を指定する
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(MapData));

            //書き込むファイルを開く（UTF-8 BOM無し）
            using (System.IO.StreamReader sr = new System.IO.StreamReader(fileName, new System.Text.UTF8Encoding(false)))
            {
                //シリアル化し、XMLファイルから読み込む
                mapFile = (MapData)serializer.Deserialize(sr);

                //ファイルを閉じる
                sr.Close();
            }

            mapFile.MapFileName = fileName;

            return mapFile;
        }

        /// <summary>
        /// Mapファイル保存
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool SaveMapFile(string fileName)
        {
            MapFileName = fileName;

            //XmlSerializerオブジェクトを作成
            //オブジェクトの型を指定する
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(MapData));

            //書き込むファイルを開く（UTF-8 BOM無し）
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName, false, new System.Text.UTF8Encoding(false)))
            {
                //シリアル化し、XMLファイルに保存する
                serializer.Serialize(sw, this);
                //ファイルを閉じる
                sw.Close();
            }

            return true;
        }

    }

        // マップ情報
        // ファイルは、bmpかpng　24BitColorであること。

            /*
        // うつぼ公園テニス場
        static public string MapName = "うつぼ公園テニス場";
        static public string MapImageFileName = "./utubo01_1200x1300_red.png";
        static public double RealWidth = 100.0 * 1200.0;   // 実際のマップの横幅 [mm]
        static public double RealHeight = 100.0 * 1300.0;   // 実際のマップの縦幅 [mm]　真北を縦とする


        static public Vector3 startPosition = new Vector3(725,387, 0);

        static public double startDir = 180;


        static public Vector3[] checkPoint =
        {
            //new Vector3(725,387,0),
            new Vector3(743,560,0),
            new Vector3(790,765,0),
            new Vector3(817,870,0),     // 南東
            new Vector3(746,895,0),
            new Vector3(613,920,0),
            new Vector3(425,980,0),     // 南西
            new Vector3(380,870,0),     
            new Vector3(316,654,0),
            new Vector3(287,569,0),     // 北西
            new Vector3(358,535,0),
            new Vector3(476,500,0),
            new Vector3(666,445,0),
            new Vector3(756,470,0),
            new Vector3(714,425,0),     // ゴール
        };
        */

        /*
        static public string MapFileName = "./TukubaCourse2016_Ver001b.png";
        static public double RealWidth = 100.0 * 3000.0;   // 実際のマップの横幅 [mm]
        static public double RealHeight = 100.0 * 7500.0;   // 実際のマップの縦幅 [mm]　真北を縦とする


        static public Vector3 startPosition = new Vector3(2400, 6500, 0);

        static public double startDir = 0;


        static public Vector3[] checkPoint =
        {
            new Vector3(2377,6300,0),
            new Vector3(2357,5800,0),      // 大清水　北東
            new Vector3(2170,5800,0),   
            new Vector3(1980,5720,0),
            new Vector3(1900,5770,0),     // 大清水北西
            new Vector3(1940,6150,0), 
            new Vector3(1960,6330,0),
            new Vector3(1990,6750,0),
            new Vector3(2000,6910,0),     // テント付近
            new Vector3(1930,7000,0),
            new Vector3(1870,7070,0),
            new Vector3(1560,7180,0),
            new Vector3(1730,7200,0),   // 大清水出口
            new Vector3(1710,7170,0),
            new Vector3(1710,7000,0),
            new Vector3(1670,6970,0),
            new Vector3(1530,6990,0),   // 歩道到達
            new Vector3(1480,6940,0),
            new Vector3(1470,6660,0),
            new Vector3(0,0,0),
            new Vector3(0,0,0),
            new Vector3(0,0,0),

            new Vector3(714,425,0),     // ゴール
        };
        */
    
}
