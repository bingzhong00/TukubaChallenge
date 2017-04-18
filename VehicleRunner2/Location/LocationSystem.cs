


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Axiom.Math;

/* Todo
 * 
 * 
 */

// 角度表記区別
// Dir / Theta..角度(度数) -180 ～ 180
// Ang / Rad ..ラディアン -Pi ～ Pi



namespace Location
{
    /// <summary>
    /// 自己位置管理 クラス
    /// </summary>
    public class LocationSystem
    {
        /// <summary>自己位置情報　表示BMP</summary>
        //public Bitmap AreaOverlayBmp;

        /// <summary>自己位置情報　表示BMP</summary>
        //public Bitmap AreaBmp;

        /// <summary>自己位置情報表示BMP 解像度（ピクセル）</summary>
        private const int OverlayBmpSize = 600;

        // -------------------------------------------------------------
        /// <summary>現在のロボット位置  (マーカー色：レッド)</summary>
        public MarkPoint R1;
        private MarkPoint oldR1; // 差分取得

        // ROS AMCL
        public MarkPoint A1;

        // ------------------------------------
        // ログ
        List<MarkPoint> R1Log;

        const int LogLine_maxDrawNum = 300; // 描画数上限

        //
        public static double LRFmaxRange_mm = 30*1000;

        /// <summary>
        /// マップイメージファイル
        /// </summary>
        public Bitmap mapBmp;

        /// <summary>
        /// Map座標、ROS座標 スケール変換
        /// </summary>
        public double MapTom;
        public double mToMap;

        /// <summary>
        /// ルート制御
        /// </summary>
        public Rooting RTS;

        /// <summary>
        /// マップファイル
        /// </summary>
        public MapData mapData;

        // -------------------------------------------------------------------------------------------------
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fnameMapBmp"></param>
        /// <param name="worldWith">実際のマップサイズ mm</param>
        /// <param name="worldHeight">実際のマップサイズ mm</param>
        public LocationSystem(string _mapFileName)
        {
            // 
            mapData = MapData.LoadMapFile(_mapFileName);

            mapBmp = new Bitmap(mapData.MapImageFileName);

            // スケール算出
            MapTom = (mapData.RealWidth / (double)mapBmp.Width);     // 実サイズ（m）/ピクセル数　＝　１ピクセルを何mとするか
            mToMap = 1.0 / MapTom;

            RTS = new Rooting(mapData,MapTom);

            //AreaOverlayBmp = new Bitmap(OverlayBmpSize, OverlayBmpSize);

            //WorldSize.w = mapBmp.Width;
            //WorldSize.h = mapBmp.Height;


            // ログ領域
            R1Log = new List<MarkPoint>();

            R1 = new MarkPoint(0, 0, 0);
            oldR1 = new MarkPoint(0, 0, 0);     // 距離取得用 前回位置

            A1 = new MarkPoint(0, 0, 0);        // ROS AMCL位置
        }

        /// <summary>
        /// ROS座標入力
        /// </summary>
        /// <param name="rosX"></param>
        /// <param name="rosY"></param>
        /// <param name="rosTheta"></param>
        public void Input_ROSPosition( double rosX, double rosY, double rosTheta)
        {
            A1.X = rosX;
            A1.Y = rosY;
            A1.Theta = rosTheta;
        }

        public void Reset_ROSPosition(double rosX, double rosY, double rosTheta)
        {
            Input_ROSPosition( rosX,  rosY,  rosTheta);

            R1.Set(A1);
            oldR1.Set(A1);
            R1.distance = 0.0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rosX"></param>
        /// <param name="rosY"></param>
        /// <param name="rosTheta"></param>
        public void Input_VRPosition(double rosX, double rosY, double rosTheta)
        {
            // mm座標系をROS座標に変換
            A1.X = rosX / 1000.0;
            A1.Y = rosY / 1000.0;
            A1.Theta = rosTheta;
        }

        public void Reset_VRPosition(double rosX, double rosY, double rosTheta)
        {
            Input_VRPosition(rosX, rosY, rosTheta);

            R1.Set(A1);
            oldR1.Set(A1);
            R1.distance = 0.0;
        }


        /// <summary>
        /// 現在位置更新
        /// 各センサー情報から、座標、向きを選択
        /// </summary>
        public void update_NowLocation()
        {
            R1.X = A1.X;
            R1.Y = A1.Y;
            R1.Theta = A1.Theta;

            // 移動距離算出
            {
                double dx = R1.X - oldR1.X;
                double dy = R1.Y - oldR1.Y;
                R1.distance += Math.Sqrt(dx * dx + dy * dy);
            }
            oldR1.Set(R1);

            // 更新結果ログ保存
            UpdateLogData();
        }

        /// <summary>
        /// 蓄積ログデータ更新
        /// </summary>
        public void UpdateLogData()
        {
            // 軌跡ログ
            try
            {
                if (R1Log.Count == 0 || !R1.IsEqual(R1Log.Last()))
                {
                    R1Log.Add(new MarkPoint(R1.X, R1.Y, R1.Theta));
                }
            }
            catch (Exception ex)
            {
                // ログ時のエラーは無視
                Console.WriteLine(ex.Message);
            }

        }



        // -------------------------------------------------------------------------------------------------------------------------------
        // 自己位置推定結果

        /// <summary>
        /// 自己位置推定座標 X (マップ座標)
        /// </summary>
        /// <returns></returns>
        public double GetResultLocationX()
        {
            return R1.X;
        }
        /// <summary>
        /// 自己位置推定座標 Y (マップ座標)
        /// </summary>
        /// <returns></returns>
        public double GetResultLocationY()
        {
            return R1.Y;
        }

        /// <summary>
        /// 自己位置推定角度
        /// </summary>
        /// <returns></returns>
        public double GetResultAngle()
        {
            return R1.Theta;
        }

        /// <summary>
        /// 移動距離を取得
        /// </summary>
        /// <returns>mm単位</returns>
        public double GetResultDistance_mm()
        {
            return R1.distance * 1000.0;
        }
    }
}
