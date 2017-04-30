#define CHECKPOINT_PASS     // 角度差がある場合にチェックポイントを飛ばす

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CersioIO;
using VRSystemConfig;

using Axiom.Math;

namespace Location
{
    /// <summary>
    /// 移動ルート管理
    /// </summary>
    public class Rooting
    {
        double tgtDir;      // 目的の方向
        double tgtDist;     // 目的地までの距離
        Vector3 tgtPos;     // 目的の座標

        /// <summary>
        /// 現在の向き
        /// </summary>
        double nowDir;

        /// <summary>
        /// 現在の位置 
        /// </summary>
        Vector3 nowPos;

        /// <summary>
        /// ステアリング方向
        /// </summary>
        double tgtStearingDir;

        // CheckPoint --------------------------------------
        int seqIdx = 0;         // チェックポイントインデックス
        int seqIdxOld = 0;      
        bool goalFlg = false;   // ゴールしたか？

        /// <summary>
        /// チェックポイントを通過した瞬間
        /// </summary>
        bool bCheckPointPass = false;

        /// <summary>
        /// チェックポイントに近づく距離(半径) [m]
        /// </summary>
        const double touchRange = VRSetting.touchRange;

        //  条件が悪い場合見送るチェックポイントの条件
        // (passRange以内の距離で、passOverDir以上の角度なら次のチェックポイントへ)
        const double passRange = touchRange * 3.0;  // パスできるチェックポイントの距離
        const double passOverDir = 130.0 * (Math.PI / 180.0);           // パスできるチェックポイントとの角度

        /// <summary>
        /// マップデータ
        /// マップイメージ
        /// チェックポイント
        /// </summary>
        MapData mapData;

        /// <summary>
        /// 実座標でのチェックポイント
        /// </summary>
        public List<Vector3> rosCheckPoint;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_mapData"></param>
        public Rooting(MapData _mapData, double scaleMapToM)
        {
            mapData = _mapData;

            // チェックポイント　マップ座標から実座標へ変換
            rosCheckPoint = new List<Vector3>();
            foreach (var onePos in mapData.checkPoint)
            {
                rosCheckPoint.Add(new Vector3( (onePos.x * scaleMapToM) - mapData.RealWidth * 0.5,
                                               (onePos.y * scaleMapToM) - mapData.RealHeight * 0.5,
                                               (onePos.z * scaleMapToM) ));
            }
        }

        /// <summary>
        /// 2つのベクトルがなす角をかえす
        /// </summary>
        /// <param name="vecA"></param>
        /// <param name="vecB"></param>
        /// <returns></returns>
        public static double VecToRad( Vector3 vecA, Vector3 vecB )
        {
            vecA.Normalize();
            vecB.Normalize();

            //double dir = (double)(Axiom.Math.Utility.ASin(vecA.Dot(vecB)) - Axiom.Math.Utility.HALF_PI);
            double dir = (double)Axiom.Math.Utility.ASin(vecA.Dot(vecB));
            Vector3 resVec = vecA.Cross(vecB);
            if (resVec.z < 0) dir = -dir;

            return dir;
        }

        /// <summary>
        /// 座標から角度を求める (y-1が上(0.0度))
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>角度</returns>
        public static double CalcPositionToAngle(double x, double y)
        {
            Vector3 tgtVec = new Vector3(x, y, 0);
            Vector3 nothVec = new Vector3(0, -1, 0);

            return (VecToRad(tgtVec, nothVec));
        }


        
        // ・チェックポイントリスト
        // ・ポイント追加、削除

        // ・チェックポイントシーケンサ
        // リセット
        public void ResetSeq()
        {
            SetCheckPoint(0);
            goalFlg = false;
        }

        /// <summary>
        /// チェックポイントリセット
        /// </summary>
        /// <param name="_setIdx"></param>
        public void SetCheckPoint(int _setIdx )
        {
            if (_setIdx < mapData.checkPoint.Length)
            {
                seqIdx = _setIdx;
                seqIdxOld = _setIdx;
            }
        }

        /// <summary>
        /// 現在位置をセット
        /// ワールドマップ座標
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="dir"></param>
        public void setNowPostion(double posX, double posY, double dir)
        {
            nowDir = dir;
            nowPos.x = posX;
            nowPos.y = posY;
        }

        /// <summary>
        /// 現在位置を取得
        /// ワールドマップ座標
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="dir"></param>
        public void getNowPostion(out int posX, out int posY, out double dir)
        {
            dir = nowDir;
            posX = (int)nowPos.x;
            posY = (int)nowPos.y;
        }

        /// <summary>
        /// チェックポイント取得
        /// </summary>
        /// <param name="_seqIdx"></param>
        /// <returns></returns>
        public Vector3 getCheckPoint(int _seqIdx)
        {
            Vector3 resVec = new Vector3();

            if (_seqIdx < mapData.checkPoint.Length)
            {
                resVec.x = rosCheckPoint[_seqIdx].x;// mapData.checkPoint[_seqIdx].x;
                resVec.y = rosCheckPoint[_seqIdx].y;//mapData.checkPoint[_seqIdx].y;
            }
            return resVec;
        }

        /// <summary>
        /// 目標への角度
        /// </summary>
        /// <param name="dir"></param>
        public double getNowTargetDir()
        {
            return tgtDir;
        }


        /// <summary>
        /// 現在の向き
        /// </summary>
        /// <returns>角度</returns>
        public double getNowDir()
        {
            return nowDir;
        }

        /// <summary>
        /// 目的方向へのハンドルの角度(向けたい方向)
        /// </summary>
        /// <returns>角度</returns>
        public double getNowTargetStearingDir()
        {
            return tgtStearingDir;
        }

        /// <summary>
        /// チェックポイントのインデックス
        /// </summary>
        /// <returns></returns>
        public int getCheckPointIdx()
        {
            return seqIdx;
        }

        /// <summary>
        /// チェックポイントの数
        /// </summary>
        /// <returns></returns>
        public int getNumCheckPoints()
        {
            return mapData.checkPoint.Length;
        }

        /// <summary>
        /// ゴールしたか？
        /// </summary>
        /// <returns></returns>
        public bool getGoalFlg()
        {
            return goalFlg;
        }

        /// <summary>
        /// チェックポイント通過したか？
        /// </summary>
        /// <returns></returns>
        public bool IsCheckPointPass()
        {
            return bCheckPointPass;
        }


        /// <summary>
        /// チェックポイント間ルーティング
        /// </summary>
        public void calcRooting()
        {
            bCheckPointPass = false;

            // ゴールしてたら計算しない
            if (goalFlg) return;

            calcCheckPoint();

            // チェックポイント短絡　判定
            // ターゲットがある程度の範囲内で、向きが大幅に違う場合パスする。
#if CHECKPOINT_PASS
            if (Math.Abs(getNowTargetDir() - nowDir) > passOverDir)
            {
                if (GetCheckPointDistance(nowPos) < passRange)
                {
                    //Brain.addLogMsg += "PassTarget:" + seqIdx.ToString() +",NowDir "+getNowTargetDir().ToString()+",TgtDir "+tgtDir.ToString() + "\n";
                    seqIdx++;
                    calcCheckPoint();
                }
            }
#endif

            // 更新されたか？
            if (seqIdxOld != seqIdx)
            {
                bCheckPointPass = true;
            }
            seqIdxOld = seqIdx;
        }

        /// <summary>
        /// チェックポイントを探す
        /// </summary>
        /// <returns>ゴール判定</returns>
        private bool calcCheckPoint()
        {
            // ゴールしてたら計算しない
            if (goalFlg || seqIdx >= mapData.checkPoint.Count()) return true;

            // 次のチェックポイントにすすむか？
            for (int i = 0; i < mapData.checkPoint.Count(); i++)
            {
                double dist = (getCheckPoint(seqIdx) - nowPos).Length;

                if (dist > touchRange) break;
                seqIdx++;
                if (seqIdx >= mapData.checkPoint.Count())
                {
                    goalFlg = true;
                    return true;
                }
            }

            // 目標座標
            tgtPos = getCheckPoint(seqIdx);

            // 目標方向
            {
                Vector3 tgtVec = tgtPos - nowPos;
                Vector3 nothVec = new Vector3(0, -1, 0);

                tgtDist = tgtVec.Length;

                tgtDir = VecToRad(tgtVec, nothVec);
            }
            return false;
        }
        
        /// <summary>
        /// 現在のチェックポイントまでの距離
        /// </summary>
        /// <param name="vPos"></param>
        /// <returns></returns>
        private double GetCheckPointDistance( Vector3 vPos )
        {
            //return (mapData.checkPoint[seqIdx] - nowPos).Length;
            return (rosCheckPoint[seqIdx] - nowPos).Length;
        }


        /// <summary>
        /// ハンドリング
        /// </summary>
        /// <returns></returns>
        public double getHandleValue()
        {
            return CalcHandleValue( tgtDir, nowDir, ref tgtStearingDir, tgtDist);
        }
 
        /// <summary>
        /// ハンドリング計算  ラジアン→ハンドル値(-1.0 ～ +1.0)
        /// </summary>
        /// <param name="targetHandleRad">行きたい角度（ラジアン）</param>
        /// <param name="nowHandleRad">現在の向き（ラジアン）</param>
        /// <returns></returns>
        public static double CalcHandleValue(double targetHandleRad, double nowHandleRad, ref double stearingDir, double targetDist )
        {
            // ラジアンから角度へ変換
            double tgtAng = targetHandleRad;// * 180.0 / Axiom.Math.Utility.PI;
            double nowAng = nowHandleRad;// * 180.0 / Axiom.Math.Utility.PI;
            double stearingAng = 0;

            // 向かいたい角度を計算
            {
                /*
                tgtAng = ((tgtAng + 180) % 360) - 180;
                nowAng = ((nowAng + 180) % 360) - 180;

                stearingAng = tgtAng - nowAng;
                if (Math.Abs(stearingAng) > 180)
                {
                    if (stearingAng > 0) stearingAng = (stearingAng - 360) % 180;
                    else                 stearingAng = (360 - stearingAng) % 180;
                }
                */

                // -2PI～2PIの差分に変換
                stearingAng = (tgtAng % (Math.PI*2.0)) - (nowAng % (Math.PI * 2.0));
                if (Math.Abs(stearingAng) > (Math.PI * 2.0))
                {
                    stearingAng = (stearingAng % (Math.PI * 2.0));
                }

                // -PI～PIに変換
                if (Math.Abs(stearingAng) > Math.PI)
                {
                    if (stearingAng < 0.0) stearingAng = stearingAng + (Math.PI * 2.0);
                    else stearingAng = stearingAng - (Math.PI * 2.0);
                }

                // ハンドルを切りたい角度
                stearingDir = stearingAng;// * Math.PI / 180.0;
            }

            // 最大ハンドル切れ角
            const double maxStearingAng = CersioIO.CersioCtrl.MaxHandleAngle * Math.PI / 180.0;

            // 最大きれ角までに制限
            if (stearingAng > maxStearingAng) stearingAng = maxStearingAng;
            if (stearingAng < -maxStearingAng) stearingAng = -maxStearingAng;

            // 角度から -1.0～1.0に変換
            return -(stearingAng * (1.0 / maxStearingAng));
        }

        /// <summary>
        /// アクセル値計算
        /// </summary>
        /// <returns></returns>
        public double getAccelValue()
        {
            const double maxSpeed = 1.0;

            // ゴールなら動かない
            if (goalFlg) return 0;

            // ターゲット付近は速度を落とす
            //if (tgtDist < 3.0) return maxSpeed*0.8;
            return maxSpeed;
        }
    }
}
