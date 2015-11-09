using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiom.Math;

namespace CersioIO
{
    public class RootingData
    {
        // マップ情報
        // ファイルは、bmpかpng　24BitColorであること。
        static public string MapFileName = "./OoSimizuKouen20151108.png";
        static public double RealWidth = 100.0 * 1500.0;   // 実際のマップの横幅 [mm]
        static public double RealHeight = 100.0 * 2500.0;   // 実際のマップの縦幅 [mm]　真北を縦とする


        // スタート位置GPS
        //static public double GPS_LandX = 135.2929;
        //static public double GPS_LandY = 34.4105;
#if true
        static public Vector3 startPosition = new Vector3(1000,1000, 0);

        static public double startDir = 0;


        static public Vector3[] checkPoint =
        {
            new Vector3(1000,0,0),
            new Vector3(1000,-1000,0),  // 100m
            new Vector3(1000,-2000,0),
            new Vector3(1000,-3000,0),
            new Vector3(1000,-4000,0),
            new Vector3(1000,-5000,0),  // 500m
            new Vector3(1000,-6000,0),
            new Vector3(1000,-7000,0),
            new Vector3(1000,-8000,0),
            new Vector3(1000,-9000,0),
            new Vector3(1000,-10000,0), // 1000m
            new Vector3(1000,-11000,0),
            new Vector3(1000,-12000,0),
            new Vector3(1000,-13000,0),
            new Vector3(1000,-14000,0),
            new Vector3(1000,-15000,0),
            new Vector3(1000,-16000,0),
            new Vector3(1000,-17000,0),
            new Vector3(1000,-18000,0),
            new Vector3(1000,-19000,0),
            new Vector3(1000,-20000,0), // 2000m
            new Vector3(1000,-100000,0),    // 10km
        };
#else
        // スタート位置、向き(-360～360度 / 北 0, +東, -西, 南180)

        static public Vector3 startPosition = new Vector3(1170,1168, 0);

        static public double startDir = 0;


        static public Vector3[] checkPoint =
        {
            new Vector3(989,924,0),
            new Vector3(760,620,0),
            new Vector3(607,425,0),
            new Vector3(502,329,0),
            new Vector3(405,329,0),
            new Vector3(304,385,0),
            new Vector3(411,529,0),
            new Vector3(670,620,0),
            new Vector3(807,544,0),
            new Vector3(767,445,0),
            new Vector3(815,450,0),
            new Vector3(892,551,0),
            new Vector3(870,590,0),
            new Vector3(733,679,0),
            new Vector3(619,905,0),
            new Vector3(664,1020,0),
            new Vector3(952,1564,0),
            new Vector3(1027,1728,0),
            new Vector3(980,1940,0),
        };
#endif
    }
}
