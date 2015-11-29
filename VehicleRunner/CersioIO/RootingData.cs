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
        static public string MapFileName = "./utubo01_1200x1300_fix.png";
        static public double RealWidth = 100.0 * 1200.0;   // 実際のマップの横幅 [mm]
        static public double RealHeight = 100.0 * 1300.0;   // 実際のマップの縦幅 [mm]　真北を縦とする


        static public Vector3 startPosition = new Vector3(725,372, 0);

        static public double startDir = 160;


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
    }
}
