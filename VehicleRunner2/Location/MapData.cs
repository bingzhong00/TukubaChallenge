using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

using Axiom.Math;
using DmitryBrant.ImageFormats;

namespace Location
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
        /// ROS yamlファイル
        /// Imageファイル名とMApOriginを取得
        /// </summary>
        public string MapYamlFileName;

        /// <summary>
        /// Map画像ファイル
        /// マップ情報
        /// ファイルは、bmpかpng　24BitColorであること。
        /// </summary>
        public string MapImageFileName;

        /// <summary>
        /// 実際のマップの横幅 [m]
        /// </summary>
        public double RealWidth;

        /// <summary>
        /// 実際のマップの縦幅 [m]
        /// </summary>
        public double RealHeight;

        /// <summary>
        /// 解像度 １ピクセルのサイズ[m]
        /// </summary>
        public double Resolution;

        /// <summary>
        /// マップ原点　左上 実座標[m]
        /// </summary>
        public Vector3 MapOrign;

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

            // 読み込んだファイル名で、更新
            mapFile.MapFileName = fileName;

            if (!String.IsNullOrEmpty(mapFile.MapYamlFileName))
            {
                string newYamlPath = Path.GetDirectoryName(fileName) + "/" + mapFile.MapYamlFileName;

                // yamlファイル読み込み
                using (System.IO.StreamReader sr = new System.IO.StreamReader(newYamlPath, new System.Text.UTF8Encoding(false)))
                {
                    /*
                     * image: kandai201709091132.pgm
                        resolution: 0.050000
                        origin: [-125.000000, -125.000000, 0.000000]
                        negate: 0
                        occupied_thresh: 0.65
                        free_thresh: 0.196
                     */
                    while (!sr.EndOfStream)
                    {
                        var readLine = sr.ReadLine();
                        var readWords = readLine.Split(':');
                        if (readWords.Length >= 2)
                        {
                            if (readWords[0].Trim().ToLower() == "image")
                            {
                                string newPath = Path.GetDirectoryName(fileName) + "/" + readWords[1].Trim();
                                if (!File.Exists(newPath))
                                {
                                    new Exception("存在しないマップファイル (yaml) image:" + newPath);
                                }
                                mapFile.MapImageFileName = newPath;
                            }
                            else if (readWords[0].Trim().ToLower() == "resolution")
                            {
                                mapFile.Resolution = double.Parse(readWords[1].Trim());
                            }
                            else if (readWords[0].Trim().ToLower() == "origin")
                            {
                                string orignWord = readWords[1].Trim();
                                orignWord = orignWord.Substring(1,orignWord.Length-2); // [ ] 削除
                                var xyzWords = orignWord.Split(',');

                                mapFile.MapOrign.x = double.Parse(xyzWords[0]);
                                mapFile.MapOrign.y = double.Parse(xyzWords[1]);
                                mapFile.MapOrign.z = double.Parse(xyzWords[2]);
                            }
                            else if (readWords[0].Trim().ToLower() == "negate")
                            {
                                // no use
                            }
                            else if (readWords[0].Trim().ToLower() == "occupied_thresh")
                            {
                                // no use
                            }
                            else if (readWords[0].Trim().ToLower() == "free_thresh")
                            {
                                // no use
                            }
                        }
                    }
                    sr.Close();
                }
            }


            // MapImageのパスを、MapFileのディレクトリを基準として取得
            if ( !File.Exists(mapFile.MapImageFileName) )
            {
                // ファイルを見つけられない場合、MapFileのパスを使う
                string newPath = Path.GetDirectoryName(fileName) + "/" + mapFile.MapImageFileName;
                if (!File.Exists(newPath))
                {
                    new Exception("存在しないマップファイル<MapImageFileName>:" + mapFile.MapImageFileName );
                }
                mapFile.MapImageFileName = newPath;
            }

            // 過去のxmlファイル互換
            // 解像度がなければ、イメージファイルから算出
            // 中心点も算出
            if (mapFile.Resolution == 0.0)
            {
                // マップサイズ、ピクセルから計算
                // 原点はマップの中心

                // ※マップのピクセル数だけほしいが全部読むのは重い・・
                // 要：対策
                Bitmap mapBmp;
                mapBmp = BitmapExtensions.Load(mapFile.MapImageFileName);
                if (null == mapBmp)
                {
                    mapBmp = new Bitmap(mapFile.MapImageFileName);
                }

                // スケール算出
                mapFile.Resolution = (mapFile.RealWidth / (double)mapBmp.Width);     // 実サイズ（m）/ピクセル数　＝　１ピクセルを何mとするか

                mapFile.MapOrign.x = -mapFile.RealWidth * 0.5;
                mapFile.MapOrign.y = -mapFile.RealHeight * 0.5;
                mapFile.MapOrign.z = 0.0;
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
