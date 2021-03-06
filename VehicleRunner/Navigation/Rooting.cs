﻿#define CHECKPOINT_PASS     // 角度差がある場合にチェックポイントを飛ばす

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Axiom.Math;


namespace Navigation
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

        double tgtStearingDir;      // ステアリング方向

        // CheckPoint --------------------------------------
        int seqIdx = 0;         // チェックポイントインデックス
        int seqIdxOld = 0;      
        bool goalFlg = false;   // ゴールしたか？

        /// <summary>
        /// チェックポイントを通過した瞬間
        /// </summary>
        bool bCheckPointPass = false;

        // 
        const double touchRange = 10.0;              // チェックポイントに近づく距離(半径) [Pixel]

        //  条件が悪い場合見送るチェックポイントの条件
        // (passRange以内の距離で、passOverDir以上の角度なら次のチェックポイントへ)
        const double passRange = touchRange * 3.0;  // パスできるチェックポイントの距離
        const double passOverDir = 130.0;           // パスできるチェックポイントとの角度

        MapData rootData;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_mapData"></param>
        public Rooting(MapData _mapData)
        {
            rootData = _mapData;
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

            double dir = (double)(Axiom.Math.Utility.ASin(vecA.Dot(vecB)) - Axiom.Math.Utility.HALF_PI);
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

            return (VecToRad(tgtVec, nothVec)) * 180.0 / Axiom.Math.Utility.PI;
        }


        
        // ・チェックポイントリスト
        // ・ポイント追加、削除

        // ・チェックポイントシーケンサ
        // リセット
        public void ResetSeq()
        {
            seqIdx = 0;
            seqIdxOld = 0;
            goalFlg = false;
        }


        /// <summary>
        /// 現在位置をセット
        /// ワールドマップ座標
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="dir"></param>
        public void setNowPostion(int posX, int posY, double dir)
        {
            nowDir = dir * Axiom.Math.Utility.PI / 180.0;
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
            dir = nowDir * 180.0 / Axiom.Math.Utility.PI;
            posX = (int)nowPos.x;
            posY = (int)nowPos.y;
        }

        // 目標の座標取得
        public void getNowTarget(ref int posX, ref int posY)
        {
            if (!goalFlg && seqIdx < rootData.checkPoint.Length)
            {
                posX = (int)rootData.checkPoint[seqIdx].x;
                posY = (int)rootData.checkPoint[seqIdx].y;
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


        /// <summary>
        /// 現在の向き
        /// </summary>
        /// <returns>角度</returns>
        public double getNowDir()
        {
            return nowDir * 180.0 / Axiom.Math.Utility.PI;
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
        public int GetNowCheckPointIdx()
        {
            return seqIdx;
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
            if (goalFlg || seqIdx >= rootData.checkPoint.Count()) return true;

            // 次のチェックポイントにすすむか？
            for (int i = 0; i < rootData.checkPoint.Count(); i++)
            {
                if ((rootData.checkPoint[seqIdx] - nowPos).Length > touchRange) break;
                seqIdx++;
                if (seqIdx >= rootData.checkPoint.Count())
                {
                    goalFlg = true;
                    return true;
                }
            }

            // 目標座標
            tgtPos = rootData.checkPoint[seqIdx];

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
            return (rootData.checkPoint[seqIdx] - nowPos).Length;
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
            // 最大ハンドル切れ角
            const double maxStearingAng = CersioIO.CersioCtrl.MaxHandleAngle;

            // ラジアンから角度へ変換
            double tgtAng = targetHandleRad * 180.0 / Axiom.Math.Utility.PI;
            double nowAng = nowHandleRad * 180.0 / Axiom.Math.Utility.PI;
            double stearingAng = 0;

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

            // -360～360度の差分に変換
            stearingAng = (tgtAng%360) - (nowAng%360);
            if (Math.Abs(stearingAng) > 360.0)
            {
                stearingAng = (stearingAng % 360);
            }

            // -180～180に変換
            if (Math.Abs(stearingAng) > 180.0)
            {
                if (stearingAng < 0.0) stearingAng = stearingAng + 360;
                else stearingAng = stearingAng - 360;
            }

            // ハンドルを切りたい角度
            stearingDir = stearingAng;

            // 瞬間的に大きく切り過ぎないように調整
            //stearingAng *= 0.4;

            // 最大きれ角制限
            if (stearingAng > maxStearingAng) stearingAng = maxStearingAng;
            if (stearingAng < -maxStearingAng) stearingAng = -maxStearingAng;

            // 瞬間的に大きく切り過ぎないように距離に応じて調整(30%～100%)
            /*
            {
                // 距離が遠ければ、切る角度を浅く
                double perDist = 2000.0; // 4m
                double per = ((targetDist > perDist) ? perDist : targetDist);
                per = 1.0 - (per / perDist);

                stearingAng *= 0.3 + (per * 0.7);
            }*/

            // 角度から -1.0～1.0に変換
            return -(stearingAng * (1.0 / maxStearingAng));
        }

        /// <summary>
        /// ハンドリング計算  角度→ハンドル値(-1.0 ～ +1.0)
        /// </summary>
        /// <param name="targetHandleAng">向きたい角度</param>
        /// <param name="nowHandleAng">現在の向き(角度)</param>
        /// <returns></returns>
        public static double CalcHandleValueFromAng(double targetHandleAng, double nowHandleAng)
        {
            double stearingDir = 0.0;

            return CalcHandleValue( targetHandleAng * Axiom.Math.Utility.PI / 180.0,
                                    nowHandleAng    * Axiom.Math.Utility.PI / 180.0,
                                    ref stearingDir, 0.0 );
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
