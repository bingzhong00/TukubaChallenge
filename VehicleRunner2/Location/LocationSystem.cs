


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

        // R.E. Plot
        public MarkPoint E1;

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

        public enum LOCATION_SENSOR
        {
            NONE,
            AMCL,
            REPlot,
        };

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

            R1 = new MarkPoint(0, 0, 0);
            oldR1 = new MarkPoint(0, 0, 0);     // 距離取得用 前回位置

            A1 = new MarkPoint(0, 0, 0);        // ROS AMCL位置
            E1 = new MarkPoint(0, 0, 0);        // Encoader Plot
        }

        /// <summary>
        /// ROS座標入力
        /// </summary>
        /// <param name="rosX"></param>
        /// <param name="rosY"></param>
        /// <param name="rosTheta"></param>
        public void Input_ROSPosition( double rosX, double rosY, double rosTheta)
        {
            // 移動距離算出
            {
                double dx = rosX - A1.x;
                double dy = rosY - A1.y;
                A1.distance += Math.Sqrt(dx * dx + dy * dy);
            }

            A1.x = rosX;
            A1.y = rosY;
            A1.theta = rosTheta;
        }

        public void Reset_ROSPosition(double rosX, double rosY, double rosTheta)
        {
            Input_ROSPosition( rosX,  rosY,  rosTheta);
            A1.distance = 0.0;
        }

        /// <summary>
        /// RE Position
        /// </summary>
        /// <param name="rosX"></param>
        /// <param name="rosY"></param>
        /// <param name="rosTheta"></param>
        public void Input_REPosition(double rosX, double rosY, double rosTheta)
        {
            // 移動距離算出
            {
                double dx = rosX - (E1.x / 1000.0);
                double dy = rosY - (E1.y / 1000.0);
                E1.distance += Math.Sqrt(dx * dx + dy * dy);
            }

            // mm座標系をROS座標に変換
            E1.x = rosX / 1000.0;
            E1.y = rosY / 1000.0;
            E1.theta = rosTheta;
        }

        public void Reset_REPosition(double rosX, double rosY, double rosTheta)
        {
            Input_REPosition(rosX, rosY, rosTheta);
            E1.distance = 0.0;
        }


        /// <summary>
        /// 現在位置更新
        /// 各センサー情報から、座標、向きを選択
        /// </summary>
        public void update_NowLocation(LOCATION_SENSOR selectLocSensor )
        {
            switch (selectLocSensor)
            {
                case LOCATION_SENSOR.AMCL:
                    R1.x = A1.x;
                    R1.y = A1.y;
                    R1.theta = A1.theta;
                    R1.distance = A1.distance;
                    break;
                case LOCATION_SENSOR.REPlot:
                    R1.x = E1.x;
                    R1.y = E1.y;
                    R1.theta = E1.theta;
                    R1.distance = E1.distance;
                    break;
            }
        }

        /// <summary>
        /// 指定ROS座標に近いチェックポイントを返す
        /// </summary>
        /// <param name="mapX"></param>
        /// <param name="mapY"></param>
        /// <returns></returns>
        public int GetCheckPointIndex(double rosX, double rosY)
        {
            Vector3 checkPos = new Vector3(rosX, rosY, 0.0);

            double minLen = 999.0;
            int idx = -1;
            for (int i = RTS.getCheckPointIdx(); i < RTS.getNumCheckPoints(); i++)
            {
                Vector3 tgtPos = RTS.getCheckPoint(i);
                double tgtLen = (tgtPos - checkPos).Length;

                // 一番近いチェックポイントをさがす
                if (tgtLen < minLen)
                {
                    minLen = tgtLen;
                    idx = i;
                }
            }

            // 1m以内ならそのインデックスを返す
            if (minLen < 1.0) return idx;
            return -1;
        }


        // -------------------------------------------------------------------------------------------------------------------------------
        // 自己位置推定結果

        /// <summary>
        /// 自己位置推定座標 X (マップ座標)
        /// </summary>
        /// <returns></returns>
        public double GetResultLocationX()
        {
            return R1.x;
        }
        /// <summary>
        /// 自己位置推定座標 Y (マップ座標)
        /// </summary>
        /// <returns></returns>
        public double GetResultLocationY()
        {
            return R1.y;
        }

        /// <summary>
        /// 自己位置推定角度
        /// </summary>
        /// <returns></returns>
        public double GetResultAngle()
        {
            return R1.theta;
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
