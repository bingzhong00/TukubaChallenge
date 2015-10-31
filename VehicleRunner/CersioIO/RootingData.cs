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
        static public string MapFileName = "./OoSimizuKouen1100x1800Mod.png";
        static public double RealWidth  = 100.0 * 1100.0;   // 実際のマップの横幅 [mm]
        static public double RealHeight = 100.0 * 1800.0;   // 実際のマップの縦幅 [mm]　真北を縦とする

        // スタート位置、向き(-360～360度 / 北 0, +東, -西, 南180)
        static public Vector3 startPosition = new Vector3(788, 1428, 0);
        static public double startDir = 0;

#if true
        static public Vector3[] checkPoint = { new Vector3(788,1428, 0), // スタート位置
                    new Vector3(788,1300,0),
                    new Vector3(788,1100,0)
                                 /*
                    new Vector3(786,1371,0),
                    new Vector3(781,1307,0),
                    new Vector3(780,1257,0),
                    new Vector3(776,1215,0),
                    new Vector3(773,1143,0),
                    new Vector3(762,1045,0),
                    new Vector3(753,964,0),
                    new Vector3(742,883,0),
                    new Vector3(732,808,0),
                    new Vector3(724,752,0),
                    new Vector3(715,698,0),
                    new Vector3(707,649,0),
                    new Vector3(697,590,0),
                    new Vector3(694,539,0),
                    new Vector3(688,499,0),
                    new Vector3(679,467,0),
                    new Vector3(641,432,0),
                    new Vector3(607,395,0),
                    new Vector3(587,361,0),
                    new Vector3(553,335,0),
                    new Vector3(510,314,0),
                    new Vector3(477,300,0),
                    new Vector3(451,295,0),
                    new Vector3(428,294,0),
                    new Vector3(402,295,0),
                    new Vector3(386,295,0),
                    new Vector3(365,297,0),
                    new Vector3(340,303,0),
                    new Vector3(320,306,0),
                    new Vector3(300,315,0),
                    new Vector3(288,324,0),
                    new Vector3(279,335,0),
                    new Vector3(279,347,0),
                    new Vector3(280,361,0),
                    new Vector3(284,382,0),
                    new Vector3(290,404,0),
                    new Vector3(296,419,0),
                    new Vector3(305,441,0),
                    new Vector3(310,458,0),
                    new Vector3(318,474,0),
                    new Vector3(318,487,0),
                    new Vector3(316,501,0),
                    new Vector3(323,521,0),
                    new Vector3(326,535,0),
                    new Vector3(332,543,0),
                    new Vector3(347,551,0),
                    new Vector3(366,559,0),
                    new Vector3(386,566,0),
                    new Vector3(407,574,0),
                    new Vector3(426,579,0),
                    new Vector3(448,585,0),
                    new Vector3(468,588,0),
                    new Vector3(479,587,0),
                    new Vector3(501,587,0),
                    new Vector3(523,587,0),
                    new Vector3(549,587,0),
                    new Vector3(567,588,0),
                    new Vector3(592,585,0),
                    new Vector3(610,581,0),
                    new Vector3(616,567,0),
                    new Vector3(616,553,0),
                    new Vector3(612,528,0),
                    new Vector3(610,517,0),
                    new Vector3(609,509,0),
                    new Vector3(619,500,0),
                    new Vector3(635,494,0),
                    new Vector3(650,494,0),
                    new Vector3(664,501,0),
                    new Vector3(669,516,0),
                    new Vector3(670,526,0),
                    new Vector3(671,542,0),
                    new Vector3(672,556,0),
                    new Vector3(675,571,0),
                    new Vector3(675,586,0),
                    new Vector3(678,602,0),
                    new Vector3(680,618,0),
                    new Vector3(679,631,0),
                    new Vector3(675,645,0),
                    new Vector3(665,652,0),
                    new Vector3(651,653,0),
                    new Vector3(624,655,0),
                    new Vector3(608,655,0),
                    new Vector3(589,656,0),
                    new Vector3(563,657,0),*/
                               };
#else
                static public Vector3[] checkPoint = { //new Vector3(750, 330, 0), // スタート位置
                                 //new Vector3(0, 40, 0)};//, new Vector3(0, 10, 0), new Vector3(2, 20, 0), // 右下
                                 //new Vector3(-5, 30, 0)};//, new Vector3(610, 775, 0), new Vector3(490, 800, 0), new Vector3(416, 814, 0),// 左下

                                 new Vector3(0, -5, 0), new Vector3(0, -10, 0), new Vector3(2, -15, 0), // 右下
                                 new Vector3(-5, -38, 0), new Vector3(-25, -48, 0), new Vector3(-45, -59, 0)
                                       , new Vector3(-65, -68, 0), new Vector3(-90, -68, 0)};//, new Vector3(610, 775, 0), new Vector3(490, 800, 0), new Vector3(416, 814, 0),// 左下

//                                 new Vector3(-5, 0, 0), new Vector3(-10, 0, 0), new Vector3(-20, 0, 0), // 右下
//                                 new Vector3(-30, -5, 0)};//, new Vector3(610, 775, 0), new Vector3(490, 800, 0), new Vector3(416, 814, 0),// 左下

//                                 new Vector3(390, 725, 0), new Vector3(355, 490, 0), new Vector3(340, 422, 0), // 左上
//                                 new Vector3(783, 387, 0), new Vector3(658, 348, 0),  new Vector3(741, 345, 0),        // 右上
//                                 new Vector3(750, 330, 0) };

#endif

    }
}
