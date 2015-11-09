#define CHECKPOINT_PASS     // 角度差がある場合にチェックポイントを飛ばす

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Axiom.Math;


namespace CersioIO
{
    /// <summary>
    /// 移動ルート管理
    /// </summary>
    public class Rooting
    {
        //Matrix3 mat;
        //Quaternion qut;
        double tgtDir;      // 目的の方向
        double tgtDist;     // 目的地までの距離
        Vector3 tgtPos;     // 目的の座標

        double nowDir;      // 現在の向き
        Vector3 nowPos;     // 現在の位置

        // CheckPoint --------------------------------------
        int seqIdx = 0;         // チェックポイントインデックス
        bool goalFlg = false;   // ゴールしたか？

        // 
        const double touchRange = 10.0;              // チェックポイントに近づく距離(半径) [Pixel]

        //  条件が悪い場合見送るチェックポイントの条件
        // (passRange以内の距離で、passOverDir以上の角度なら次のチェックポイントへ)
        const double passRange = touchRange * 3.0;  // パスできるチェックポイントの距離
        const double passOverDir = 130.0;           // パスできるチェックポイントとの角度


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

            double dir = (double)(Axiom.Math.Utility.ASin(vecA.Dot(vecB)) - Axiom.Math.Utility.HALF_PI);
            Vector3 resVec = vecA.Cross(vecB);
            if (resVec.z > 0) dir = -dir;

            return dir;
        }

        /// <summary>
        /// 座標から角度を求める (y-1が上(0.0度))
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double CalcPositionToAngle(double x, double y)
        {
            Vector3 tgtVec = new Vector3(x, y, 0);
            Vector3 nothVec = new Vector3(0, -1, 0);

            return (VecToRad(tgtVec, nothVec)) * 180.0 / Axiom.Math.Utility.PI;
        }


        
        // ・チェックポイントリスト
        // ・ポイント追加、削除

        // ・チェックポイントシーケンサ
        // リセット
        public void ResetSeq()
        {
            seqIdx = 0;
            goalFlg = false;
        }

        /// <summary>
        /// 前方にチェックポイントを作り直す
        /// </summary>
        public void ResetStraightMode()
        {
            seqIdx = 0;
            goalFlg = false;

            RootingData.checkPoint = new Vector3[1];

            double wRad = -RootingData.startDir * Axiom.Math.Utility.PI / 180.0;
            double cs = Math.Cos(wRad);
            double sn = Math.Sin(wRad);

            double tgtX = 0.0;
            double tgtY = -500.0; // pixel 10cm x 500 = 50m 先を目指す

            double newCpX = RootingData.startPosition.x + (tgtX * cs - tgtY * sn);
            double newCpY = RootingData.startPosition.y + (tgtX * sn + tgtY * cs);

            // 新チェックポイント作成
            RootingData.checkPoint[0] = new Vector3(newCpX, newCpY, 0);

            // 目標座標 再計算
            calcCheckPoint();
        }

        // 現在位置セット
        public void setNowPostion(int posX, int posY, double dir)
        {
            nowDir = dir * Axiom.Math.Utility.PI / 180.0;
            nowPos.x = posX;
            nowPos.y = posY;
        }

        // 目標の座標取得
        public void getNowTarget(ref int posX, ref int posY)
        {
            if (!goalFlg)
            {
                posX = (int)RootingData.checkPoint[seqIdx].x;
                posY = (int)RootingData.checkPoint[seqIdx].y;
            }
        }

        /// <summary>
        /// 目標への角度
        /// </summary>
        /// <param name="dir">度</param>
        public void getNowTargetDir(ref double dir )
        {
            dir = tgtDir * 180.0 / Axiom.Math.Utility.PI;
        }
        public double getNowTargetDir()
        {
            double retDir = 0.0;
            getNowTargetDir(ref retDir);

            return retDir;
        }

        public int GetNowCheckPointIdx()
        {
            return seqIdx;
        }

        // ゴールしたか？
        public bool getGoalFlg()
        {
            return goalFlg;
        }

        // チェックポイント間ルーティング
        public void calcRooting()
        {
            // ゴールしてたら計算しない
            if (goalFlg) return;

            calcCheckPoint();

            // ターゲットがある程度の範囲内で、向きが大幅に違う場合パスする。
#if CHECKPOINT_PASS
            if (Math.Abs(getNowTargetDir() - (nowDir * 180.0 / Math.PI)) > passOverDir)
            {
                if (GetCheckPointDistance(nowPos) < passRange)
                {
                    Brain.addLogMsg += "PassTarget:" + seqIdx.ToString() +",NowDir "+getNowTargetDir().ToString()+",TgtDir "+tgtDir.ToString() + "\n";

                    seqIdx++;
                    calcCheckPoint();
                }
            }
#endif
        }

        /// <summary>
        /// チェックポイントを探す
        /// </summary>
        /// <returns>ゴール判定</returns>
        private bool calcCheckPoint()
        {
            // ゴールしてたら計算しない
            if (goalFlg || seqIdx >= RootingData.checkPoint.Count()) return true;

            // 次のチェックポイントにすすむか？
            for (int i = 0; i < RootingData.checkPoint.Count(); i++)
            {
                if ((RootingData.checkPoint[seqIdx] - nowPos).Length > touchRange) break;
                seqIdx++;
                if (seqIdx >= RootingData.checkPoint.Count())
                {
                    goalFlg = true;
                    return true;
                }
            }

            // 目標座標
            tgtPos = RootingData.checkPoint[seqIdx];

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
            return (RootingData.checkPoint[seqIdx] - nowPos).Length;
        }


        /// <summary>
        /// ハンドリング
        /// </summary>
        /// <returns></returns>
        public double getHandleValue()
        {
            return CalcHandleValue( tgtDir, nowDir );
        }
 
        /// <summary>
        /// ハンドリング計算  ラジアン→ハンドル値(-1.0 ～ +1.0)
        /// </summary>
        /// <param name="targetHandleRad">行きたい角度（ラジアン）</param>
        /// <param name="nowHandleRad">現在の向き（ラジアン）</param>
        /// <returns></returns>
        public static double CalcHandleValue(double targetHandleRad, double nowHandleRad )
        {
            // 最大ハンドル切れ角
            const double maxStearingAng = 30.0;

            // ラジアンから角度へ変換
            double tgtAng = targetHandleRad * 180.0 / Axiom.Math.Utility.PI;
            double nowAng = nowHandleRad * 180.0 / Axiom.Math.Utility.PI;
            double stearingAng = 0;

            tgtAng = ((tgtAng + 180) % 360) - 180;
            nowAng = ((nowAng + 180) % 360) - 180;

            stearingAng = tgtAng - nowAng;
            if (Math.Abs(stearingAng) > 180)
            {
                if (stearingAng > 0) stearingAng = (stearingAng - 360) % 180;
                else                 stearingAng = (360 - stearingAng) % 180;
            }

            // 最大きれ角
            if (stearingAng > maxStearingAng) stearingAng = maxStearingAng;
            if (stearingAng < -maxStearingAng) stearingAng = -maxStearingAng;

            //stearingAng = 0.0;

            return (stearingAng * (1.0 / maxStearingAng));
        }

        /// <summary>
        /// ハンドリング計算  角度→ハンドル値(-1.0 ～ +1.0)
        /// </summary>
        /// <param name="targetHandleAng">向きたい角度</param>
        /// <param name="nowHandleAng">現在の向き(角度)</param>
        /// <returns></returns>
        public static double CalcHandleValueFromAng(double targetHandleAng, double nowHandleAng)
        {
            return CalcHandleValue( targetHandleAng * Axiom.Math.Utility.PI / 180.0,
                                    nowHandleAng    * Axiom.Math.Utility.PI / 180.0 );
        }

        /// <summary>
        /// アクセル値計算
        /// </summary>
        /// <returns></returns>
        public double getAccelValue()
        {
            const double maxSpeed = 1.0;

            if (goalFlg) return 0;

            // ターゲット付近は速度を落とす
            if (tgtDist < 3.0) return maxSpeed*0.8;
            return maxSpeed;
        }
    }
}
