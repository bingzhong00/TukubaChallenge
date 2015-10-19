using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Axiom.Math;


namespace ParticleFilterDLL
{
    /// <summary>
    /// 移動ルート管理
    /// </summary>
    public class Rooting
    {
        Vector3[] checkPoint = { new Vector3(75, 42, 0), // スタート位置
                                 new Vector3(104, 39, 0), new Vector3(165, 40, 0), // 右下
                                 new Vector3(165, 93, 0), new Vector3(160, 160, 0), // 右上
                                 new Vector3(107, 160, 0), new Vector3(66, 160, 0), new Vector3(23, 160, 0), // 左上
                                 new Vector3(22, 150, 0), new Vector3(23, 116, 0),  new Vector3(25, 44, 0),        // 左下
                                 new Vector3(75, 45, 0) };
        //Matrix3 mat;
        //Quaternion qut;
        double tgtDir;      // 目的の方向
        double tgtDist;     // 目的地までの距離
        Vector3 tgtPos;     // 目的の座標

        double nowDir;      // 現在の向き
        Vector3 nowPos;     // 現在の位置

        int seqIdx = 0;         // チェックポイントインデックス
        bool goalFlg = false;   // ゴールしたか？
        double touchRange = 4.0;    // チェックポイントに近づく距離

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
            if (resVec.z < 0) dir = -dir;

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
                posX = (int)checkPoint[seqIdx].x;
                posY = (int)checkPoint[seqIdx].y;
            }
        }

        // 目標への角度
        public void getNowTargetDir(ref double dir )
        {
            dir = tgtDir * 180.0 / Axiom.Math.Utility.PI;
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

            for( int i=0; i<checkPoint.Count(); i++ ){
                if ((checkPoint[seqIdx] - nowPos).Length > touchRange) break;
                seqIdx++;
                if (seqIdx >= checkPoint.Count())
                {
                    goalFlg = true;
                    return;
                }
            }

            tgtPos = checkPoint[seqIdx];

            Vector3 tgtVec = tgtPos - nowPos;
            Vector3 nothVec = new Vector3(1, 0, 0);

            tgtDist = tgtVec.Length;

            tgtDir = VecToRad(tgtVec, nothVec);
        }


        /// <summary>
        /// ハンドリング
        /// </summary>
        /// <returns></returns>
        public double getHandleValue()
        {
            // 最大ハンドル切れ角
            const double maxStearingAng = 10.0;

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
