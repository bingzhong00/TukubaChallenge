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
        // マップ情報
        // ファイルは、bmpかpng　24BitColorであること。

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
            new Vector3(425,890,0),     // 南西
            new Vector3(380,870,0),     
            new Vector3(316,654,0),
            new Vector3(287,569,0),     // 北西
            new Vector3(358,535,0),
            new Vector3(756,470,0),
            new Vector3(714,425,0),     // ゴール
        };
    }
}
