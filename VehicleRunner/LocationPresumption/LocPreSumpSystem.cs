﻿

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
 * BoxPCとの通信を任意でOn Offできるように
 */

namespace LocationPresumption
{
    /// <summary>
    /// 自己位置推定計算　クラス
    /// </summary>
    public class LocPreSumpSystem
    {
        // LRF
        public URG_LRF urgLRF = null;               // nullで仮想MAPモード
        public double[] LRF_Data = null;

        public double RealToMapSclae;                // マップサイズから メートル変換
        public double LRFmaxRange = 30.0 * 1000.0;   // LRFの有効範囲(単位 mm)

        // LRFノイズリダクション
        public double[] LRF_UntiNoiseData;
        private const int maxLrfDir = 270;

        // 割合 (1.0...ノイズ高め　/ 0.0...ノイズ低め　ただし反応がにぶい)
        double LRF_noiseRate = 0.50; 

        // エリアマップ管理
        public WorldMap worldMap;                  // 全体マップ
        public Bitmap AreaBmp;                     // 表示用　エリアBMP
        public Bitmap AreaOverlayBmp;              // 自己位置情報　表示BMP

        private const int OverlayBmpSize = 600;     // 自己位置情報表示BMP 解像度（ピクセル）

        // 自己位置推定用 -----------------------
        private MapRangeFinder MRF;             // マップレンジファインダー
        public MarkPoint R1;                    // 実体想定ロボット位置
        public MarkPoint E1;                    // RE想定ロボット位置
        public MarkPoint oldE1;                 // RE 差分計算用
        public MarkPoint V1;                    // PF想定ロボット位置
        public MarkPoint C1;                    // コンパス値
        public MarkPoint G1;                    // GPS

        /// <summary>
        /// ローパスフィルター
        /// </summary>
        private KalmanFilter.SimpleLPF lpfV1X = new KalmanFilter.SimpleLPF();
        private KalmanFilter.SimpleLPF lpfV1Y = new KalmanFilter.SimpleLPF();


        // REの絶対座標(差分計算用)
        double nowREX;
        double nowREY;
        double nowRETheta;


        // 左回転が+　右が-
        public const int ParticleSize = 100;        // パーティクル数
        private ParticleFilter Filter;              // サンプリング数、パーティクルのレンジ
        private List<Particle> Particles;           // パーティクルリスト

        // ログ
        List<MarkPoint> R1Log;
        List<MarkPoint> V1Log;
        List<MarkPoint> E1Log;
        List<MarkPoint> G1Log;
        
        // 
        float[] SinTbl = new float[360];
        float[] CosTbl = new float[360];

        /// <summary>
        /// コンパスの情報があるか？
        /// </summary>
        public bool bActiveCompass = false;

        // GPSスタート地点 
        public static double startPosGPSX = 0.0;
        public static double startPosGPSY = 0.0;
        public static bool bEnableGPS = false;

        // -------------------------------------------------------------------------------------------------
        //

        /// <summary>
        /// 
        /// </summary>
        public LocPreSumpSystem()
        {
            for(int i=0;i<360;i++)
            {
                SinTbl[i] = (float)Math.Sin((double)i * Math.PI / 180.0);
                CosTbl[i] = (float)Math.Cos((double)i * Math.PI / 180.0);
            }

            LRF_UntiNoiseData = new double[maxLrfDir];

            // 接触状態にならないように初期化
            for (int i = 0; i < LRF_UntiNoiseData.Length; i++)
            {
                LRF_UntiNoiseData[i] = LRFmaxRange;
            }
        }

        public float fSin( int ang )
        {
            int ln = (ang / 360)+1;
            ang = (ang + (360*ln)) % 360;

            return SinTbl[ang];
        }

        public float fCos(int ang)
        {
            int ln = (ang / 360) + 1;
            ang = (ang + (360 * ln)) % 360;

            return CosTbl[ang];
        }

        public void Close()
        {
            CloseURG();
        }

        /// <summary>
        /// ワールドマップ初期化
        /// </summary>
        /// <param name="fnameMapBmp"></param>
        /// <param name="worldWith">実際のマップサイズ mm</param>
        /// <param name="worldHeight">実際のマップサイズ mm</param>
        public void InitWorld( string fnameMapBmp, double worldWith, double worldHeight )
        {
            worldMap = new WorldMap(fnameMapBmp);
            RealToMapSclae = (worldWith / (double)worldMap.Worldize.w);     // 実サイズ（ｍｍ）/ピクセル数　＝　１ピクセルを何mmとするか

            URG_LRF.setScale(1.0 / RealToMapSclae);      // mm単位からピクセル単位へ スケール変換

            // エリア初期化
            // エリアサイズ策定 LRF最大距離をエリアのピクセル数とする
            int areaGridSize = (int)(LRFmaxRange / RealToMapSclae);
            worldMap.InitArea(areaGridSize, areaGridSize);

            AreaOverlayBmp = new Bitmap(OverlayBmpSize, OverlayBmpSize);

            // ログ領域
            R1Log = new List<MarkPoint>();
            V1Log = new List<MarkPoint>();
            E1Log = new List<MarkPoint>();
            G1Log = new List<MarkPoint>();
        }

        /// <summary>
        /// LRF初期化　ログデータモード
        /// </summary>
        /// <param name="logFilename"></param>
        public void InitURG(string logFilename)
        {
            urgLRF = new URG_LRF();
            urgLRF.LogFileOpen(logFilename);
        }

        /// <summary>
        /// LRF初期化 ＬＡＮモード
        /// </summary>
        /// <param name="IPAddr"></param>
        /// <param name="IPPort"></param>
        public void InitURG(string IPAddr, int IPPort)
        {
            urgLRF = new URG_LRF();
            urgLRF.IpOpen(IPAddr, IPPort);
        }

        /// <summary>
        /// LRF初期化　エミュレーションモード
        /// </summary>
        public void InitURG()
        {
            urgLRF = null;
        }

        /// <summary>
        /// LRFクローズ
        /// </summary>
        public void CloseURG()
        {
            if (null != urgLRF)
            {
                urgLRF.Close();
            }
        }

        /// <summary>
        /// 自己位置推定  開始位置セット
        /// </summary>
        /// <param name="stWldX"></param>
        /// <param name="stWldY"></param>
        public void SetStartPostion(int stWldX, int stWldY, double stDir)
        {
            MRF = new MapRangeFinder(LRFmaxRange / RealToMapSclae);    // 仮想Map用 LRFクラス

            worldMap.UpdateAreaCenter(stWldX, stWldY);
            AreaBmp = worldMap.AreaGridMap.UpdateBitmap();

            MRF.SetMap(worldMap.AreaGridMap);

            // ロボットマーカ設定
            R1 = new MarkPoint(worldMap.GetAreaX(stWldX), worldMap.GetAreaY(stWldY), stDir);        // 実ロボット位置 R.E,Compass,GPSでの位置+ LRF補正

            E1 = new MarkPoint(worldMap.GetAreaX(stWldX), worldMap.GetAreaY(stWldY), stDir);        // R.Encoderの位置
            oldE1 = new MarkPoint(worldMap.GetAreaX(stWldX), worldMap.GetAreaY(stWldY), stDir);     // 

            V1 = new MarkPoint(worldMap.GetAreaX(stWldX), worldMap.GetAreaY(stWldY), stDir);        // 推定位置ロボット
            C1 = new MarkPoint(worldMap.GetAreaX(0), worldMap.GetAreaY(0), stDir);
            G1 = new MarkPoint(worldMap.GetAreaX(0), worldMap.GetAreaY(0), 0);        // GPS

            // パーティクルフィルター初期化
            {
                // サンプル数(ParticleSizeと同じで、すべてのパーティクルを対象とする)
                // LRFの有効距離を半径に変換(/2.0)、20%の距離で散らばる
                // +-5度の範囲
                Filter = new ParticleFilter(ParticleSize*3/4, (((LRFmaxRange/2.0) * 0.20) / RealToMapSclae), 5.0 );        // サンプリング数、パーティクルのレンジ

                Particles = new List<Particle>();
                for (int i = 0; i < ParticleSize; ++i)
                {
                    // パーティクルマーカー
                    Particles.Add(new Particle(new MarkPoint(V1.X, V1.Y, V1.Theta), 1));
                }
            }

            // ローパスフィルター初期化
            ResetLPF_V1(V1);

            // RE更新差分計算用リセット
            nowREX = 0.0;
            nowREY = 0.0;
            nowRETheta = 0.0;
        }


        /// <summary>
        /// ローパスフィルター初期化
        /// </summary>
        /// <param name="mkp"></param>
        public void ResetLPF_V1(MarkPoint mkp)
        {
            lpfV1X = new KalmanFilter.SimpleLPF(mkp.X);
            lpfV1Y = new KalmanFilter.SimpleLPF(mkp.Y);
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
                // R1の位置を新しいエリアの中心とする
                worldMap.UpdateAreaCenter(worldMap.GetWorldX((int)(R1.X+0.5)), worldMap.GetWorldY((int)(R1.Y+0.5)));

                // エリア座標更新
                R1.X += (double)worldMap.WldOffsetDiff.x;
                R1.Y += (double)worldMap.WldOffsetDiff.y;

                V1.X += (double)worldMap.WldOffsetDiff.x;
                V1.Y += (double)worldMap.WldOffsetDiff.y;

                E1.X += (double)worldMap.WldOffsetDiff.x;
                E1.Y += (double)worldMap.WldOffsetDiff.y;

                oldE1.X += (double)worldMap.WldOffsetDiff.x;
                oldE1.Y += (double)worldMap.WldOffsetDiff.y;

                G1.X += (double)worldMap.WldOffsetDiff.x;
                G1.Y += (double)worldMap.WldOffsetDiff.y;

                // 更新後の座標にローパスフィルターをリセット
                ResetLPF_V1(V1);

                // エリアマップ更新
                AreaBmp = worldMap.AreaGridMap.UpdateBitmap();
                MRF.SetMap(worldMap.AreaGridMap);
            }
        }


        /// <summary>
        /// LRFデータ取得
        /// </summary>
        /// <returns></returns>
        public bool LRF_Update()
        {
            bool rt = false;

            if (null != urgLRF)
            {
                // LRFからデータ取得 (実データ or ログファイル)
                double[] newLRFData = urgLRF.getScanData();

                // データがあるか？
                if (null != newLRFData && newLRFData.Count() > 0)
                {
                    LRF_Data = newLRFData;
                    rt = true;
                }
            }
            else
            {
                // マップから生成 (LRFエミュレーションモード)
                LRF_Data = MRF.Sense(R1);
            }

            // lrfノイズ リダクション
            for (int i = 0; i < LRF_Data.Length; i++)
            {
                LRF_UntiNoiseData[i] = (LRF_UntiNoiseData[i] * (1.0 - LRF_noiseRate)) + (LRF_Data[i] * LRF_noiseRate);
            }

            return rt;
        }

        /// <summary>
        /// LRF接続状態取得
        /// </summary>
        /// <returns></returns>
        public bool IsLRFConnect()
        {
            if (null != urgLRF) return true;
            return false;
        }

        /// <summary>
        /// ロータリーエンコーダ値取得
        /// </summary>
        /// <param name="reX">絶対座標X(mm)</param>
        /// <param name="reY">絶対座標Y(mm)</param>
        /// <param name="reTheta">角度（度）</param>
        /// <returns></returns>
        public bool SetRotaryEncoderData(double reX, double reY,double reTheta)
        {
            // 単位変換
            reX = reX / RealToMapSclae;
            reY = reY / RealToMapSclae;

            // 前回値との差分で更新
            // (スタート位置、向きを反映)
            E1.X += (reX - nowREX);
            E1.Y += (reY - nowREY);
            //E1.Theta += (reTheta - nowRETheta);
            E1.Theta = reTheta;

            nowREX = reX;
            nowREY = reY;
            nowRETheta = reTheta;

            return true;
        }

        /// <summary>
        /// コンパス値取得
        /// </summary>
        /// <param name="reTheta"></param>
        /// <returns></returns>
        public bool SetCompassData(double reTheta)
        {
            bActiveCompass = true;
            C1.Theta = reTheta;
            return true;
        }

        const double GPSScale = 1.85225 * 1000.0 * 1000.0;
        const double GPSScaleX = 1.51985 * 1000.0 * 1000.0;    // 経度係数  35度時
        //const double GPSScaleY = 1.85225 * 1000.0 * 1000.0;    // 緯度係数

        /// <summary>
        /// GPS値取得
        /// </summary>
        /// <param name="landX"></param>
        /// <param name="landY"></param>
        /// <returns></returns>
        public bool SetGPSData(double landX, double landY)
        {
            if (!bEnableGPS) return false;

            double ido = (int)landY;
            double kdo = (int)landX;

            double mapY = (ido * 60.0 + (landY - ido)) * GPSScale;
            double mapX = (kdo * 60.0 + (landX - kdo)) * GPSScaleX;

            //mapX = (kdo * 60.0 + (landX - kdo)) * (GPSScale * Math.Cos(((ido * 60.0 + (landY - ido))/60.0) * Math.PI / 180.0));   // ※要テスト

            // 単位変換
            mapX = (mapX - startPosGPSX) / RealToMapSclae;
            mapY = -(mapY - startPosGPSY) / RealToMapSclae;

            G1.X = worldMap.GetAreaX((int)mapX);
            G1.Y = worldMap.GetAreaY((int)mapY);
            G1.Theta = 0.0;

            return true;
        }

        /// <summary>
        /// マップ上開始位置のGPS情報
        /// </summary>
        /// <param name="landX"></param>
        /// <param name="landY"></param>
        /// <returns></returns>
        public static void SetStartGPS(double landX, double landY)
        {
            double ido = (int)landY;
            double kdo = (int)landX;

            startPosGPSY = (ido * 60.0 + (landY - ido)) * GPSScale;
            startPosGPSX = (kdo * 60.0 + (landX - kdo)) * GPSScaleX;

            //startPosGPSX = (kdo * 60.0 + (landX - kdo)) * (GPSScale * Math.Cos(((ido * 60.0 + (landY - ido))/60.0) * Math.PI / 180.0));   // ※要テスト

            bEnableGPS = true;
        }



        long upDateCnt = 0;
        // 処理カウンタ
        public System.Diagnostics.Stopwatch swCNT_Update = new System.Diagnostics.Stopwatch();
        public static System.Diagnostics.Stopwatch swCNT_MRF = new System.Diagnostics.Stopwatch();


        /// <summary>
        /// 自己位置計算
        /// </summary>
        /// <param name="numPF"></param>
        public void CalcLocalize(MarkPoint mkp, int numPF = 1)
        {
            // 処理時間計測
            swCNT_Update.Reset();
            swCNT_Update.Start();

            // パーティクルフィルター演算
            {
                // 計算起点をセット
                ResetLPF_V1(mkp);
                V1.Set(mkp);

                for (int i = 0; i < numPF; i++)
                {
                    Filter.Localize(LRF_Data, MRF, V1, Particles);

                    // 結果にローパスフィルターをかける
                    V1.X = lpfV1X.update(V1.X);
                    V1.Y = lpfV1Y.update(V1.Y);
                }
            }

            // 処理時間計測完了
            swCNT_Update.Stop();
        }

        /// <summary>
        /// 自己位置推定 更新
        /// </summary>
        /// <returns>true...補正した, false..補正不要</returns>
        public bool Update()
        {
            bool bResult = false;   // 補正したか？

            swCNT_MRF.Reset();

            // 自己位置推定位置がエリア内になるようにチェック
            MoveAreaCheck();

            // 自己位置更新 (※後にBrainへ移動)
            // ロータリーエンコーダの移動量差分を加えて、自己位置を更新
            {
                R1.X += (E1.X - oldE1.X);
                R1.Y += (E1.Y - oldE1.Y);
                R1.Theta = E1.Theta; //  REの向き

                oldE1.Set(E1);
            }

            // 軌跡ログ
            try
            {
                MarkPoint logR1 = new MarkPoint(worldMap.GetWorldX(R1.X), worldMap.GetWorldY(R1.Y), R1.Theta);
                //if (R1Log.Count == 0 || !logR1.IsEqual(R1Log[R1Log.Count - 1])) // ※メモリを壊してるかも・・・
                {
                    R1Log.Add(logR1);
                }

                // ParticleFiler
                //if (usePF)
                {
                    MarkPoint logV1 = new MarkPoint(worldMap.GetWorldX(V1.X), worldMap.GetWorldY(V1.Y), V1.Theta);
                    //if (V1Log.Count == 0 || !logV1.IsEqual(V1Log[V1Log.Count - 1]))
                    {
                        V1Log.Add(logV1);
                    }
                }

                // RotaryEncoder
                MarkPoint logE1 = new MarkPoint(worldMap.GetWorldX(E1.X), worldMap.GetWorldY(E1.Y), E1.Theta);
                //if (E1Log.Count == 0 || !logE1.IsEqual(E1Log[E1Log.Count - 1]))
                {
                    E1Log.Add(logE1);
                }

                // GPS
                if (bEnableGPS)
                {
                    MarkPoint logG1 = new MarkPoint(worldMap.GetWorldX(G1.X), worldMap.GetWorldY(G1.Y), G1.Theta);
                    //if (E1Log.Count == 0 || !logE1.IsEqual(E1Log[E1Log.Count - 1]))
                    {
                        G1Log.Add(logG1);
                    }
                }
            }
            catch
            {
                // ログ時のエラーは無視
            }

            upDateCnt++;

            return bResult;
        }

        // -------------------------------------------------------------------------------------------------------------------------------
        // 自己位置推定結果

        /// <summary>
        /// 自己位置推定座標 X (マップ座標)
        /// </summary>
        /// <returns></returns>
        public double GetResultLocationX()
        {
            return worldMap.GetWorldX(R1.X);
        }
        /// <summary>
        /// 自己位置推定座標 Y (マップ座標)
        /// </summary>
        /// <returns></returns>
        public double GetResultLocationY()
        {
            return worldMap.GetWorldY(R1.Y);
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
            return MRF.Sense(mkp);
        }

        /// <summary>
        /// マップ情報取得
        /// </summary>
        /// <returns></returns>
        public Grid GetMapInfo_R1()
        {
            return worldMap.AreaGridMap[(int)R1.X, (int)R1.Y];
        }
        public Grid GetMapInfo(MarkPoint mkp)
        {
            return worldMap.AreaGridMap[(int)R1.X, (int)R1.Y];
        }

        // --------------------------------------------------------------------------------------------------------------------------------
        // 描画系

        // 処理カウンタ
        public System.Diagnostics.Stopwatch swCNT_Draw = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// 自己位置情報表示 Bmp更新
        /// </summary>
        public void UpdateLocalizeBitmap( bool bParticle,bool bLineTrace )
        {
            swCNT_Draw.Reset();
            swCNT_Draw.Start();

            Graphics g = Graphics.FromImage(AreaOverlayBmp);

            // Overlayのスケール
            // エリア座標からオーバーレイエリアへの変換
            float olScale = (float)AreaOverlayBmp.Width / (float)AreaBmp.Width;

            // エリアマップ描画
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(AreaBmp, 0, 0, AreaOverlayBmp.Width, AreaOverlayBmp.Height);

            // パーティクル描画
            int size = 10;
            if (bParticle)
            {
                for (int i = 0; i < Particles.Count / 4; i++)    // 少なめ(1/4)描画
                //for (int i = 0; i < Particles.Count; i++)
                {
                    var p = Particles[i];
                    DrawMakerFast(g, olScale, p.Location, Brushes.Blue, 5);
                }
            }

            // リアルタイム軌跡描画
            if( bLineTrace )
            {
                DrawMakerLog_Area(g, olScale, R1Log, Color.Red.R, Color.Red.G, Color.Red.B);
                DrawMakerLog_Area(g, olScale, V1Log, Color.Cyan.R, Color.Cyan.G, Color.Cyan.B);
                DrawMakerLog_Area(g, olScale, E1Log, Color.Purple.R, Color.Purple.G, Color.Purple.B);
                DrawMakerLog_Area(g, olScale, G1Log, Color.Green.R, Color.Green.G, Color.Green.B);
            }
             
            // 描画順を常にかえて、重なっても見えるようにする
            for (int i = 0; i < 3; i++)
            {
                switch ((i + upDateCnt) % 3)
                {
                    case 0:
                        // RE想定ロボット位置描画
                        DrawMaker(g, olScale, E1, Brushes.Purple, size);
                        break;
                    case 1:
                        // PF想定ロボット位置描画
                        DrawMaker(g, olScale, V1, Brushes.Cyan, size);
                        break;
                    case 2:
                        // 実ロボット想定位置描画
                        DrawMaker(g, olScale, R1, Brushes.Red, size);
                        break;
                }
            }

            g.Dispose();

            swCNT_Draw.Stop();
        }

        /// <summary>
        /// マーカー描画
        /// </summary>
        /// <param name="g"></param>
        /// <param name="robot"></param>
        /// <param name="brush"></param>
        /// <param name="size"></param>
        private void DrawMaker(Graphics g, float fScale, MarkPoint robot, Brush brush, int size )
        {
            double mkX = robot.X * fScale;
            double mkY = robot.Y * fScale;
            double mkDir = robot.Theta - 90.0;

            var P1 = new PointF(
                (float)(mkX + size * -Math.Cos(mkDir * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin(mkDir * Math.PI / 180.0)));
            var P2 = new PointF(
                (float)(mkX + size * -Math.Cos((mkDir - 150) * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin((mkDir - 150) * Math.PI / 180.0)));
            var P3 = new PointF(
                (float)(mkX + size * -Math.Cos((mkDir + 150) * Math.PI / 180.0)),
                (float)(mkY + size * Math.Sin((mkDir + 150) * Math.PI / 180.0)));

            g.FillPolygon(brush, new PointF[] { P1, P2, P3 });
        }

        /// <summary>
        /// マーカー描画　高速版
        /// </summary>
        /// <param name="g"></param>
        /// <param name="fScale"></param>
        /// <param name="robot"></param>
        /// <param name="brush"></param>
        /// <param name="size"></param>
        private void DrawMakerFast(Graphics g, float fScale, MarkPoint robot, Brush brush, int size)
        {
            float mkX = (float)(robot.X * fScale);
            float mkY = (float)(robot.Y * fScale);
            int mkDir = (int)(robot.Theta - 90.0);

            var P1 = new PointF(
                mkX + size * -fCos(mkDir),
                mkY + size * fSin(mkDir));
            var P2 = new PointF(
                mkX + size * -fCos(mkDir - 150),
                mkY + size * fSin(mkDir - 150));
            var P3 = new PointF(
                mkX + size * -fCos(mkDir + 150),
                mkY + size * fSin(mkDir + 150));

            g.FillPolygon(brush, new PointF[] { P1, P2, P3 });
            //g.DrawPolygon(brush, new PointF[] { P1, P2, P3 });
        }

        /// <summary>
        /// ログ軌跡描画　ローカルエリアに変換
        /// </summary>
        /// <param name="g"></param>
        /// <param name="fScale"></param>
        /// <param name="mkLog"></param>
        /// <param name="colR"></param>
        /// <param name="colG"></param>
        /// <param name="colB"></param>
        private void DrawMakerLog_Area(Graphics g, float fScale, List<MarkPoint> mkLog, byte colR, byte colG, byte colB)
        {
            Point[] ps = new Point[mkLog.Count];

            if (mkLog.Count < 2) return;

            for(int i=0; i<mkLog.Count; i++ )
            {
                ps[i].X = (int)(worldMap.GetAreaX((int)mkLog[i].X) * fScale);
                ps[i].Y = (int)(worldMap.GetAreaY((int)mkLog[i].Y) * fScale);
            }

            //折れ線を引く
            g.DrawLines(new Pen(Color.FromArgb(colR,colG,colB)), ps);
        }

        /// <summary>
        /// ログ軌跡描画　ワールド
        /// </summary>
        /// <param name="g"></param>
        /// <param name="mkLog"></param>
        /// <param name="colR"></param>
        /// <param name="colG"></param>
        /// <param name="colB"></param>
        private void DrawMakerLogLine_World(Graphics g, List<MarkPoint> mkLog, byte colR, byte colG, byte colB)
        {
            Point[] ps = new Point[mkLog.Count];

            if (mkLog.Count == 0) return;

            for (int i = 0; i < mkLog.Count; i++)
            {
                ps[i].X = (int)mkLog[i].X;
                ps[i].Y = (int)mkLog[i].Y;
            }

            //折れ線を引く
            g.DrawLines(new Pen(Color.FromArgb(colR, colG, colB)), ps);
        }

        /// <summary>
        /// ログ保存用　ワールドマップへの軌跡画像生成
        /// </summary>
        /// <returns></returns>
        public Bitmap MakeMakerLogBmp( bool bPointOn, MarkPoint marker )
        {
            if (R1Log.Count <= 0) return null;  // データなし

            Bitmap logBmp = new Bitmap(worldMap.mapBmp);
            Graphics g = Graphics.FromImage(logBmp);

            // 軌跡描画
            // 自己位置
            DrawMakerLogLine_World(g, R1Log, Color.Red.R, Color.Red.G, Color.Red.B);
            // パーティクルフィルター 自己位置推定
            DrawMakerLogLine_World(g, V1Log, Color.Cyan.R, Color.Cyan.G, Color.Cyan.B);
            // ロータリーエンコーダ座標
            DrawMakerLogLine_World(g, E1Log, Color.Purple.R, Color.Purple.G, Color.Purple.B);


            // 最終地点にマーカ表示
            if (R1Log.Count > 0)
            {
                DrawMaker(g, 1.0f, R1Log[R1Log.Count - 1], Brushes.Red, 4);
            }
            if (V1Log.Count > 0)
            {
                DrawMaker(g, 1.0f, V1Log[V1Log.Count - 1], Brushes.Cyan, 4);
            }
            if (E1Log.Count > 0)
            {
                DrawMaker(g, 1.0f, E1Log[E1Log.Count - 1], Brushes.Purple, 4);
            }
            if (null != marker)
            {
                DrawMaker(g, 1.0f, marker, Brushes.GreenYellow, 4);
            }

            // 一定期間ごとの位置と向き
            if (bPointOn)
            {
                // 自己位置
                //foreach (var p in R1Log)
                for (int i = 0; i < R1Log.Count; i++)
                {
                    if ((i % 30) != 0) continue;
                    DrawMaker(g, 1.0f, R1Log[i], Brushes.Red, 4);
                }

                
                // LRF パーティクルフィルター
                //foreach (var p in V1Log)
                for( int i=0; i<V1Log.Count; i++ )
                {
                    if ((i % 30) != 0) continue;
                    DrawMaker(g, 1.0f, V1Log[i], Brushes.Cyan, 3);
                }
                

                // ロータリーエンコーダ座標
                //foreach (var p in E1Log)
                for (int i = 0; i < E1Log.Count; i++)
                {
                    if ((i % 30) != 0) continue;
                    DrawMaker(g, 1.0f, E1Log[i], Brushes.Purple, 4);
                }
            }

            g.Dispose();
            return logBmp;
        }


    }
}
