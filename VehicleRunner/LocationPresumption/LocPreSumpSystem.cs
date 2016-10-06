


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using SCIP_library;
using Axiom.Math;

/* Todo
 * 
 * 
 */

// 角度表記区別
// Dir / Theta..角度(度数) -180 ～ 180
// Ang / Rad ..ラディアン -Pi ～ Pi



namespace LocationPresumption
{
    /// <summary>
    /// 自己位置推定 マップ座標変換計算　クラス
    /// </summary>
    public partial class LocPreSumpSystem
    {
        // LRF管理
        public LRF_Ctrl LRF = new LRF_Ctrl();

        public double MapToRealScale;                // マップサイズから メートル変換


        // エリアマップ管理
        /// <summary>全体マップ</summary>
        public WorldMap worldMap;
        /// <summary>表示用　エリアBMP</summary>
        public Bitmap AreaBmp = new Bitmap(1,1);
        //private bool bAreaMapUpdateReqest = true;

        /// <summary>自己位置情報　表示BMP</summary>
        public Bitmap AreaOverlayBmp;

        /// <summary>自己位置情報表示BMP 解像度（ピクセル）</summary>
        private const int OverlayBmpSize = 600;

        // 自己位置推定用 -----------------------
        /// <summary>マップレンジファインダ</summary>
        private MapRangeFinder MRF;

        /// <summary>現在のロボット位置  (マーカー色：レッド)</summary>
        public MarkPoint R1;
        /// <summary>REplotロボット位置 (マーカー色：パープル)</summary>
        public MarkPoint E1;
        /// <summary>REplot差分計算</summary>
        public MarkPoint oldE1;
        /// <summary>RE pulse & Compus (マーカー色：)</summary>
        public MarkPoint E2;

        /// <summary>vSlam or Amcl 自己位置推定結果 位置  (マーカー色：シアン)</summary>
        public MarkPoint V1;

        /// <summary>PF自己位置推定 (マーカー色：オレンジ)</summary>
        //public MarkPoint P1;

        /// <summary>地磁気コンパス値 </summary>
        public MarkPoint C1;
        /// <summary>GPS位置  (マーカー色：グリーン)</summary>
        public MarkPoint G1;
        /// <summaryGPS差分計算</summary>
        public MarkPoint oldG1;

        // ログ
        List<MarkPoint> R1Log;
        List<MarkPoint> V1Log;
        List<MarkPoint> E1Log;
        List<MarkPoint> G1Log;

        const int LogLine_maxDrawNum = 300; // 描画数上限


        /// <summary>
        /// コンパスの情報があるか？
        /// </summary>
        public bool bActiveCompass = false;

        // GPSスタート地点 
        public static double startPosGPSX = 0.0;
        public static double startPosGPSY = 0.0;
        public static int startPosGPS_MapX = 0;
        public static int startPosGPS_MapY = 0;

        /// <summary>
        /// GPSが受信して使える状態になっている
        /// </summary>
        public static bool bEnableGPS = false;
        // 角度あり GPS
        private bool bEnableDirGPS = false;

        // GPSからMap変換時の(手で合わせた)スケール（計算上は必要ないはず。。）
        //const double GPStoMapScale = 60.0;

        // 1分の距離[mm] 1.85225Km
        const double GPSScale = 1.85225 * 1000.0 * 1000.0;


        #region 設定パラメータ クラス
        /// <summary>
        /// 設定フラグ
        /// </summary>
        public class SettingParam
        {
            // 移動元センサー

            /// <summary>
            /// ロータリーエンコーダ　プロット座標
            /// </summary>
            public bool bMoveSrcRePlot;

            /// <summary>
            /// ＳＶＯ座標
            /// </summary>
            public bool bMoveSrcSVO;

            /// <summary>
            /// ＧＰＳ座標
            /// </summary>
            public bool bMoveSrcGPS;

            /// <summary>
            /// RE Pulse & 地磁気座標
            /// </summary>
            public bool bMoveSrcReCompus;

            /// <summary>
            /// RE Pulse & PF座標
            /// </summary>
            public bool bMoveSrcPF;

            // 向き

            /// <summary>
            /// ロータリーエンコーダ　プロット
            /// </summary>
            public bool bDirSrcRePlot;

            /// <summary>
            /// ＳＶＯ
            /// </summary>
            public bool bDirSrcSVO;

            /// <summary>
            /// ＧＰＳ
            /// </summary>
            public bool bDirSrcGPS;

            /// <summary>
            /// 地磁気
            /// </summary>
            public bool bDirSrcCompus;
        }
        #endregion

        /// <summary>
        /// 設定パラメータ
        /// </summary>
        public SettingParam Setting = new SettingParam();

        // -------------------------------------------------------------------------------------------------
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fnameMapBmp"></param>
        /// <param name="worldWith">実際のマップサイズ mm</param>
        /// <param name="worldHeight">実際のマップサイズ mm</param>
        public LocPreSumpSystem(string fnameMapBmp, double worldWith, double worldHeight)
        {
            worldMap = new WorldMap(fnameMapBmp);
            MapToRealScale = (worldWith / (double)worldMap.WorldSize.w);     // 実サイズ（ｍｍ）/ピクセル数　＝　１ピクセルを何mmとするか

            URG_LRF.setScale(1.0 / MapToRealScale);      // mm単位からピクセル単位へ スケール変換

            // エリア初期化
            // エリアサイズ策定 LRF最大距離をエリアのピクセル数とする
            int areaGridSize = (int)(LRF_Ctrl.LRFmaxRange_mm / MapToRealScale);
            worldMap.InitArea(areaGridSize, areaGridSize);

            AreaOverlayBmp = new Bitmap(OverlayBmpSize, OverlayBmpSize);

            // ログ領域
            R1Log = new List<MarkPoint>();
            V1Log = new List<MarkPoint>();
            E1Log = new List<MarkPoint>();
            G1Log = new List<MarkPoint>();
        }

        /// <summary>
        /// 自己位置推定  開始位置セット
        /// </summary>
        /// <param name="stWldX">マップ座標X</param>
        /// <param name="stWldY">マップ座標Y</param>
        /// <param name="stDir">角度</param>
        public void SetStartPostion(int stWldX, int stWldY, double stDir)
        {
            MRF = new MapRangeFinder(LRF_Ctrl.LRFmaxRange_mm / MapToRealScale);    // 仮想Map用 LRFクラス

            worldMap.UpdateAreaCenter(stWldX, stWldY);
            AreaBmp = worldMap.AreaGridMap.UpdateBitmap();
            //bAreaMapUpdateReqest = false;

            MRF.SetMap(worldMap.AreaGridMap);

            // ロボットマーカ設定
            R1 = new MarkPoint(stWldX, stWldY, stDir);        // 実ロボット位置 R.E,Compass,GPSでの位置+ LRF補正

            E1 = new MarkPoint(stWldX, stWldY, stDir);        // R.Encoderの位置
            oldE1 = new MarkPoint(stWldX, stWldY, stDir);     // 

            E2 = new MarkPoint(stWldX, stWldY, stDir);        // R.E Pluse & Compus の位置

            V1 = new MarkPoint(stWldX, stWldY, stDir);        // 推定位置ロボット
            C1 = new MarkPoint(0, 0, stDir);
            G1 = new MarkPoint(0, 0, 0);        // GPS
            oldG1 = new MarkPoint(stWldX, stWldY, stDir);     // 

            // パーティクルフィルター初期化
            {
                // サンプル数(ParticleSizeと同じで、すべてのパーティクルを対象とする)
                // LRFの有効距離を半径に変換(/2.0)、20%の距離で散らばる
                // +-5度の範囲
                ParticleFilter = new ParticleFilter(ParticleSize, (((LRF_Ctrl.LRFmaxRange_mm / 2.0) * 0.20) / MapToRealScale), 30.0 );        // サンプリング数、パーティクルのレンジ

                Particles = new List<Particle>();
                for (int i = 0; i < ParticleSize; ++i)
                {
                    // パーティクルマーカー
                    Particles.Add(new Particle(new MarkPoint(0,0,0), 0));
                }
            }

            // ローパスフィルター初期化
            ResetLPF_V1(V1);
        }


        /// <summary>
        /// ローパスフィルター初期化
        /// </summary>
        /// <param name="mkp"></param>
        public void ResetLPF_V1(MarkPoint mkp)
        {
            lpfV1X = new KalmanFilter.SimpleLPF(mkp.X);
            lpfV1Y = new KalmanFilter.SimpleLPF(mkp.Y);
            lpfV1Theta = new KalmanFilter.SimpleLPF(mkp.Theta);
        }

        /// <summary>
        /// エリアの移動
        /// </summary>
        private void MoveAreaCheck()
        {
            // エリアの端に近づいたか？
            if ((R1.X < worldMap.GridSize.w / 4 || R1.X > worldMap.GridSize.w * 3 / 4) ||
                (R1.Y < worldMap.GridSize.h / 4 || R1.Y > worldMap.GridSize.h * 3 / 4))
            {
                lock (AreaBmp)
                {
                    // R1の位置を新しいエリアの中心とする
                    worldMap.UpdateAreaCenter((int)(R1.X + 0.5), (int)(R1.Y + 0.5));

                    // エリアマップ更新
                    AreaBmp = worldMap.AreaGridMap.UpdateBitmap();
                }

                //bAreaMapUpdateReqest = true;
                MRF.SetMap(worldMap.AreaGridMap);
            }
        }

        /// <summary>
        /// マーカーに、ロータリーエンコーダPlot座標　反映
        /// </summary>
        /// <param name="reX">絶対座標X(mm)</param>
        /// <param name="reY">絶対座標Y(mm)</param>
        /// <param name="reTheta">角度（度）</param>
        /// <returns></returns>
        public bool Input_RotaryEncoder(double reX, double reY,double reTheta)
        {
            // 単位変換
            reX = reX / MapToRealScale;
            reY = reY / MapToRealScale;

            E1.X = reX;
            E1.Y = reY;
            E1.Theta = reTheta;

            return true;
        }

        /// <summary>
        /// ロータリーエンコーダ　マーカーリセット
        /// </summary>
        /// <param name="reX">mm</param>
        /// <param name="reY">mm</param>
        /// <param name="reTheta"></param>
        public void Reset_RotaryEncoder_Maker(double reX, double reY, double reTheta)
        {
            // 単位変換
            reX = reX / MapToRealScale;
            reY = reY / MapToRealScale;
             
            oldE1.X = E1.X = reX;
            oldE1.Y = E1.Y = reY;
            oldE1.Theta = E1.Theta = reTheta;
        }

        /// <summary>
        /// コンパス値取得
        /// </summary>
        /// <param name="reTheta"></param>
        /// <returns></returns>
        public bool Input_Compass(double reTheta)
        {
            bActiveCompass = true;
            C1.Theta = reTheta;
            return true;
        }


        /// <summary>
        /// GPS値取得
        /// </summary>
        /// <param name="landX"></param>
        /// <param name="landY"></param>
        /// <returns></returns>
        public bool Input_GPSData(double landX, double landY, double moveDir )
        {
            if (!bEnableGPS) return false;

            // MapのX,Y座標に変換
            double ido = (int)landY;
            double kdo = (int)landX;

            double mapY = (ido * 60.0 + (landY - ido)) * GPSScale;
            double mapX = (kdo * 60.0 + (landX - kdo)) * GPSScale * Math.Cos(ido * Math.PI / 180.0);

            // 単位変換
            mapX = (mapX - startPosGPSX) / MapToRealScale;
            mapY = -(mapY - startPosGPSY) / MapToRealScale;

            G1.X = mapX + startPosGPS_MapX;
            G1.Y = mapY + startPosGPS_MapY;
            G1.Theta = moveDir;
            bEnableDirGPS = true;

            return true;
        }

        public bool Input_GPSData(double landX, double landY)
        {
            if (!bEnableGPS) return false;

            // MapのX,Y座標に変換
            double ido = (int)landY;
            double kdo = (int)landX;

            double mapY = (ido * 60.0 + (landY - ido)) * GPSScale;
            double mapX = (kdo * 60.0 + (landX - kdo)) * GPSScale * Math.Cos(ido * Math.PI / 180.0);

            // 単位変換
            mapX = (mapX - startPosGPSX) / MapToRealScale;
            mapY = -(mapY - startPosGPSY) / MapToRealScale;

            G1.X = mapX + startPosGPS_MapX;
            G1.Y = mapY + startPosGPS_MapY;
            G1.Theta = 0.0;
            bEnableDirGPS = false;

            return true;
        }


        double oldRePulseR = 0.0;
        /// <summary>
        /// R.Eパルス値と向きから座標計算
        /// </summary>
        /// <param name="reL"></param>
        /// <param name="reR"></param>
        /// <param name="reTheta"></param>
        /// <returns></returns>
        public bool Input_RotaryEncoder_Pulse(double reL, double reR, double reTheta)
        {
            const double tiyeSize = 140.0;  // タイヤ直径 [mm]
            const double OnePuls = 250.0;   // 一周のパルス数

            // 移動量算出
            double moveLength = ((double)(reR - oldRePulseR) / OnePuls * (Math.PI * tiyeSize));

            oldRePulseR = reR;

            // 向きをもとに座標に変換
            double movX = 0.0;
            double movY = -moveLength;
            double pi = reTheta * Math.PI / 180.0;

            E2.X += (movX * Math.Cos(pi) - movY * Math.Sin(pi)) / MapToRealScale;
            E2.Y += (movX * Math.Sin(pi) + movY * Math.Cos(pi)) / MapToRealScale;
            E2.Theta = reTheta;

            return true;
        }

        /// <summary>
        /// マップ上開始位置のGPS情報
        /// </summary>
        /// <param name="landX"></param>
        /// <param name="landY"></param>
        /// <param name="mapX">マップ上の座標</param>
        /// <param name="mapY"></param>
        /// <param name="bTrustFlg">信頼性(スタート地点からのセットならば True)</param>
        public static void Set_GPSStart(double landX, double landY, int mapX, int mapY )
        {
            double ido = (int)landY;
            double kdo = (int)landX;

            startPosGPSY = (ido * 60.0 + (landY - ido)) * GPSScale;
            startPosGPSX = (kdo * 60.0 + (landX - kdo)) * GPSScale * Math.Cos(ido * Math.PI / 180.0);

            // マップの基準点をセット
            startPosGPS_MapX = mapX;
            startPosGPS_MapY = mapY;

            bEnableGPS = true;
        }



        /// <summary>
        /// マップ座標更新
        /// </summary>
        /// <returns>false..MAP外(未実装)</returns>
        public bool MapArea_Update()
        {
            bool bResult = true;   // マップ内か？

            // 自己位置推定位置がエリア内になるようにチェック
            MoveAreaCheck();

            return bResult;
        }

        /// <summary>
        /// 現在位置更新
        /// </summary>
        public void update_NowLocation()
        {
            MarkPoint RePlotMkp = updateMkp_FromREPlot();
            MarkPoint GpsMkp = updateMkp_FromGPS();

            // 移動情報 更新
            if (Setting.bMoveSrcRePlot)
            {
                // R.E Plot
                R1.X = RePlotMkp.X;
                R1.Y = RePlotMkp.Y;
            }
            else if (Setting.bMoveSrcGPS)
            {
                // GPS
                R1.X = GpsMkp.X;
                R1.Y = GpsMkp.Y;
            }
            else if (Setting.bMoveSrcSVO)
            {
                // Semi-direct Monocular Visual Odometry
                // ※未実装
            }
            else if (Setting.bMoveSrcReCompus)
            {
                // RE Pulse値(移動量) & Compusの向きでの座標
                R1.X = E2.X;
                R1.Y = E2.Y;
            }
            else if (Setting.bMoveSrcPF)
            {
                //  RE Pulse値(移動量) & PFの向きでの座標
                // ※未実装
            }

            // 向き情報 更新
            if (Setting.bDirSrcRePlot)
            {
                R1.Theta = RePlotMkp.Theta;
            }
            else if (Setting.bDirSrcGPS)
            {
                R1.Theta = GpsMkp.Theta;
            }
            else if (Setting.bDirSrcSVO)
            {
                // Semi-direct Monocular Visual Odometry
                // ※未実装
            }
            else if (Setting.bDirSrcCompus)
            {
                if (bActiveCompass)
                {
                    R1.Theta = C1.Theta;
                }
            }

            // 更新結果ログ保存
            UpdateLogData();
        }

        /// <summary>
        /// ロータリーエンコーダ  現在位置取得
        /// 差分を計算し現在位置更新
        /// </summary>
        private MarkPoint updateMkp_FromREPlot()
        {
            MarkPoint resMkp = new MarkPoint(R1);

            // 自己位置更新
            // ロータリーエンコーダの移動量差分を加えて、自己位置を更新
            resMkp.X += (E1.X - oldE1.X);
            resMkp.Y += (E1.Y - oldE1.Y);
            resMkp.Theta = E1.Theta; //  REの向き
            oldE1.Set(E1);

            return resMkp;
        }

        /// <summary>
        /// GPS 現在位置取得
        /// 差分を計算し現在位置更新
        /// </summary>
        private MarkPoint updateMkp_FromGPS()
        {
            MarkPoint resMkp = new MarkPoint(R1);

            // 自己位置更新
            // ロータリーエンコーダの移動量差分を加えて、自己位置を更新
            resMkp.X += (G1.X - oldG1.X);
            resMkp.Y += (G1.Y - oldG1.Y);
            resMkp.Theta = G1.Theta; //  REの向き
            oldG1.Set(G1);

            return resMkp;
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

                // ParticleFiler
                if (V1Log.Count == 0 || !V1.IsEqual(V1Log.Last()))
                {
                    V1Log.Add(new MarkPoint(V1.X, V1.Y, V1.Theta));
                }

                // RotaryEncoder
                if (E1Log.Count == 0 || !E1.IsEqual(E1Log.Last()))
                {
                    E1Log.Add(new MarkPoint(E1.X, E1.Y, E1.Theta));
                }

                // GPS
                if (bEnableGPS)
                {
                    if (G1Log.Count == 0 || !G1.IsEqual(G1Log.Last()))
                    {
                        G1Log.Add(new MarkPoint(G1.X, G1.Y, G1.Theta));
                    }
                }
            }
            catch (Exception ex)
            {
                // ログ時のエラーは無視
                Console.WriteLine(ex.Message);
            }

        }


        /// <summary>
        /// 位置補正執行
        /// </summary>
        /// <param name="bRevisionFromGPS">GPSの値をそのまま使う</param>
        public string LocalizeRevision( bool bRevisionFromGPS )
        {
            // 補正ログメッセージ
            string revisionMsg = "";

            // 補正後座標
            MarkPoint rev = new MarkPoint(V1);

            revisionMsg += "LocRevision:Issue / 座標補正を実行\n";
            revisionMsg += "LocRevision: beforR1 R1.X" + R1.X.ToString("f2") + "/R1.Y " + R1.Y.ToString("f2") + "/R1.Dir " + R1.Theta.ToString("f2") + "\n";

            // GPSが有効なら、GPSの値を元にPF補正
            if (bEnableGPS)
            {
                // 補正基準 位置,向き
                double RivLocX = G1.X;
                double RivLocY = G1.Y;
                double RivDir = (bActiveCompass ? C1.Theta : R1.Theta);    // コンパスが使えるなら、コンパスの値を使う

                if (bActiveCompass)
                {
                    // コンパスを使った
                    revisionMsg += "LocRevision:useCompass C1.Dir " + C1.Theta.ToString("f2") + "\n";
                }

                if (bRevisionFromGPS)
                {
                    // GPSの値をそのまま使う
                    rev.Set(RivLocX, RivLocY, RivDir);

                    // GPSを使った
                    revisionMsg += "LocRevision: useGPS G1.X" + G1.X.ToString("f2") + "/G1.Y " + G1.Y.ToString("f2") + "/G1.Dir " + G1.Theta.ToString("f2") + "\n";
                }
                else
                {
                    // 補正基準位置からパーティクルフィルター補正
                    rev.Set(RivLocX, RivLocY, RivDir);
                    // V1ローパスフィルターリセット
                    ResetLPF_V1(rev);

                    // 自己位置推定演算 10回
                    CalcLocalize(V1, true,10);

                    // PF
                    revisionMsg += "LocRevision: usePF  V1.X" + V1.X.ToString("f2") + "/V1.Y " + V1.Y.ToString("f2") + "/V1.Dir " + V1.Theta.ToString("f2") + "\n";
                }
            }
            else
            {
                // V1の値をそのまま使う
                revisionMsg += "LocRevision: V1toR1 V1.X" + V1.X.ToString("f2") + "/ V1.Y " + V1.Y.ToString("f2") + "/ V1.Dir " + V1.Theta.ToString("f2") + "\n";
            }

            // 結果を反映
            R1.Set(rev);
            E1.Set(rev);
            oldE1.Set(rev);

            return revisionMsg;
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
        /// 指定位置のマップ障害物情報をLRFの値として返す
        /// </summary>
        /// <param name="mapX"></param>
        /// <param name="mapY"></param>
        /// <param name="ang"></param>
        /// <returns></returns>
        public double[] GetMapLRF( int mapX, int mapY, double ang )
        {
            MarkPoint mkp = new MarkPoint(worldMap.GetAreaX(mapX), worldMap.GetAreaY(mapY), ang);

            return MRF.Sense(mkp);
        }
        public double[] GetMapLRF(MarkPoint mkp)
        {
            return GetMapLRF((int)(mkp.X+0.5), (int)(mkp.Y+0.5), mkp.Theta);
        }

        /// <summary>
        /// マップ情報取得
        /// </summary>
        /// <returns></returns>
        public Grid GetMapInfo_R1()
        {
            return worldMap.GetAreaMapInfo(R1.X, R1.Y);
        }
        public Grid GetMapInfo(MarkPoint mkp)
        {
            return worldMap.GetAreaMapInfo(mkp.X, mkp.Y);
        }


    }
}
