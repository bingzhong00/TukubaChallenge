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
        //double touchRange = 15.0;    // チェックポイントに近づく距離 (40)
        const double touchRange = 3.0;              // チェックポイントに近づく距離(半径) [Pixel]
        const double passRange = touchRange * 3.0;  // 条件が悪い場合見送るチェックポイントの距離

        /// <summary>
        /// 2つのベクトルがなす角をかえす
        /// </summary>
        /// <param name="vecA"></param>
        /// <param name="vecB"></param>
        /// <returns></returns>
        public double VecToRad( Vector3 vecA, Vector3 vecB )
        {
            vecA.Normalize();
            vecB.Normalize();

            double dir = (double)(Axiom.Math.Utility.ASin(vecA.Dot(vecB)) - Axiom.Math.Utility.HALF_PI);
            Vector3 resVec = vecA.Cross(vecB);
            if (resVec.z > 0) dir = -dir;

            return dir;
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
        public double getNowTargetDir(double dir)
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

            if (Math.Abs(getNowTargetDir(tgtDir)) > 130.0)
            {
                if (GetCheckPointDistance(nowPos) < passRange)
                {
                    seqIdx++;
                    calcCheckPoint();
                }
            }
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
            // 最大ハンドル切れ角
            const double maxStearingAng = 30.0;

            if (goalFlg) return 0;

            // ラジアンから角度へ変換
            double tgtAng = tgtDir * 180.0 / Axiom.Math.Utility.PI;
            double nowAng = nowDir * 180.0 / Axiom.Math.Utility.PI;
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

        public double getAccelValue()
        {
            const double maxSpeed = 0.8;

            if (goalFlg) return 0;

            if (tgtDist > 1.0) return maxSpeed;
            return maxSpeed * 0.5;
        }
    }
}
