using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiom.Math;

namespace Navigation
{
    public class RootingData
    {
        /*
        //XMLファイルに保存するオブジェクトのためのクラス
        public class SampleClass
        {
            public int Number;
            public string Message;
        }

        class MainClass
        {
            //SampleClassオブジェクトをXMLファイルに保存する
            public static void Main()
            {
                //保存先のファイル名
                string fileName = @"C:\test\sample.xml";

                //保存するクラス(SampleClass)のインスタンスを作成
                SampleClass obj = new SampleClass();
                obj.Message = "テストです。";
                obj.Number = 123;

                //XmlSerializerオブジェクトを作成
                //オブジェクトの型を指定する
                System.Xml.Serialization.XmlSerializer serializer =
                    new System.Xml.Serialization.XmlSerializer(typeof(SampleClass));
                //書き込むファイルを開く（UTF-8 BOM無し）
                System.IO.StreamWriter sw = new System.IO.StreamWriter(
                    fileName, false, new System.Text.UTF8Encoding(false));
                //シリアル化し、XMLファイルに保存する
                serializer.Serialize(sw, obj);
                //ファイルを閉じる
                sw.Close();
            }
        }

        class MainClass
        {
            //XMLファイルをSampleClassオブジェクトに復元する
            public static void Main()
            {
                //保存元のファイル名
                string fileName = @"C:\test\sample.xml";

                //XmlSerializerオブジェクトを作成
                System.Xml.Serialization.XmlSerializer serializer =
                    new System.Xml.Serialization.XmlSerializer(typeof(SampleClass));
                //読み込むファイルを開く
                System.IO.StreamReader sr = new System.IO.StreamReader(
                    fileName, new System.Text.UTF8Encoding(false));
                //XMLファイルから読み込み、逆シリアル化する
                SampleClass obj = (SampleClass)serializer.Deserialize(sr);
                //ファイルを閉じる
                sr.Close();
            }
        }
        */


        // マップ情報
        // ファイルは、bmpかpng　24BitColorであること。

        /*
        // うつぼ公園テニス場
        static public string MapFileName = "./utubo01_1200x1300_red.png";
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

        static public string MapFileName = "./kh_map0716.png";
        static public double RealWidth = 100.0 * 1500.0;   // 実際のマップの横幅 [mm]
        static public double RealHeight = 100.0 * 1200.0;   // 実際のマップの縦幅 [mm]　真北を縦とする


        static public Vector3 startPosition = new Vector3(600, 212, 0);

        static public double startDir = -90;


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
    }
}
