using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

            // 
            mapFile.MapFileName = fileName;

            
            // MapImageのパスを、MapFileのディレクトリを基準として取得
            if( !File.Exists(mapFile.MapImageFileName) )
            {
                // ファイルを見つけられない場合、MapFileのパスを使う
                string newPath = Path.GetDirectoryName(fileName) + "/" + mapFile.MapImageFileName;
                if (!File.Exists(newPath))
                {
                    new Exception("MapImageFileNameが見つからない:" + mapFile.MapImageFileName );
                }
                mapFile.MapImageFileName = newPath;
            }
            

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
   
}
