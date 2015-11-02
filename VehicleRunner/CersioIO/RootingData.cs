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


        // スタート位置GPS
        //static public double GPS_LandX = 135.2929;
        //static public double GPS_LandY = 34.4105;

        // スタート位置、向き(-360～360度 / 北 0, +東, -西, 南180)


        //C:\work\tsukuba\LRFmapEditer\LRFMapEditer20151019\utubo\utubo03.lme
        //
        static public Vector3 startPosition = new Vector3(726, 386, 0);
        static public double startDir = 167;

        static public Vector3[] checkPoint = {
            new Vector3(726,386,0),
//            new Vector3(721,408,0),
            new Vector3(718,442,0),
//            new Vector3(721,467,0),
            new Vector3(721,492,0),
//            new Vector3(724,516,0),
            new Vector3(730,543,0),
//            new Vector3(736,566,0),
            new Vector3(741,586,0),
//            new Vector3(748,610,0),
            new Vector3(754,635,0),
//            new Vector3(761,657,0),
            new Vector3(768,678,0),
//            new Vector3(773,700,0),
            new Vector3(780,720,0),
//            new Vector3(787,741,0),
            new Vector3(794,763,0),
//            new Vector3(801,787,0),
            new Vector3(807,811,0),
//            new Vector3(813,830,0),
            new Vector3(819,851,0),
//            new Vector3(820,868,0),
            new Vector3(814,882,0),
//            new Vector3(802,892,0),
            new Vector3(780,892,0),
//            new Vector3(755,890,0),
            new Vector3(729,894,0),
//            new Vector3(704,901,0),
            new Vector3(677,905,0),
//            new Vector3(648,915,0),
            new Vector3(623,921,0),
//            new Vector3(599,926,0),
            new Vector3(565,938,0),
//            new Vector3(533,946,0),
            new Vector3(506,957,0),
//            new Vector3(481,967,0),
            new Vector3(460,976,0),
//            new Vector3(439,981,0),
            new Vector3(418,975,0),
//            new Vector3(405,951,0),
            new Vector3(399,927,0),
            new Vector3(392,896,0),
            new Vector3(383,865,0),
            new Vector3(374,834,0),
            new Vector3(364,802,0),
            new Vector3(354,767,0),
            new Vector3(344,737,0),
            new Vector3(331,700,0),
            new Vector3(322,671,0),
            new Vector3(311,646,0),
            new Vector3(301,617,0),
            new Vector3(292,590,0),
            new Vector3(291,569,0),
            new Vector3(305,551,0),
            new Vector3(323,544,0),
            new Vector3(347,538,0),
            new Vector3(373,530,0),
            new Vector3(401,521,0),
            new Vector3(425,513,0),
            new Vector3(447,507,0),
            new Vector3(478,497,0),
            new Vector3(509,491,0),
            new Vector3(542,485,0),
            new Vector3(575,476,0),
            new Vector3(604,469,0),
            new Vector3(639,455,0),
            new Vector3(665,449,0),
            new Vector3(689,444,0),
            new Vector3(714,428,0),
            new Vector3(737,406,0),
            new Vector3(739,383,0),
};
        
    }
}
