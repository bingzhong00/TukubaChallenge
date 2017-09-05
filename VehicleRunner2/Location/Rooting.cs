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
        // TodoList.
        /*
         * ・現在位置、向きなどはパラメータでもらうようにして
         * 　このクラスでは保持、管理しないようにする。
         */ 

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
        int seqIdxOld = -1;      
        bool goalFlg = false;   // ゴールしたか？

        /// <summary>
        /// チェックポイントを更新した瞬間
        /// </summary>
        bool bCheckPointTrg = false;

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
        /// コンストラクタ
        /// マップデータからルート情報を生成
        /// </summary>
        /// <param name="_mapData"></param>
        public Rooting(MapData _mapData, double scaleMapToM)
        {
            mapData = _mapData;

            // チェックポイント　マップ座標から実座標へ変換
            rosCheckPoint = new List<Vector3>();
            rosCheckPoint.Add(new Vector3((mapData.startPosition.x * scaleMapToM) - mapData.RealWidth * 0.5,
                                           (-mapData.startPosition.y * scaleMapToM) + mapData.RealHeight * 0.5,
                                           (mapData.startPosition.z * scaleMapToM)));

            foreach (var onePos in mapData.checkPoint)
            {
                rosCheckPoint.Add(new Vector3( (onePos.x * scaleMapToM) - mapData.RealWidth * 0.5,
                                               (-onePos.y * scaleMapToM) + mapData.RealHeight * 0.5,
                                               (onePos.z * scaleMapToM) ));
            }

            ResetSeq();
            bCheckPointTrg = true;
        }

        /// <summary>
        /// 現在のチェックポイントデータからMapDataを生成
        /// </summary>
        /// <param name="scaleMapToM"></param>
        /// <returns></returns>
        public MapData GetMapdata(double scaleMapToM )
        {
            MapData outputMapdata = new MapData();
            outputMapdata = mapData;

            // チェックポイント　マップ座標から実座標へ変換
            outputMapdata.startPosition.x = ((rosCheckPoint[0].x + mapData.RealWidth * 0.5) / scaleMapToM);
            outputMapdata.startPosition.y = -((rosCheckPoint[0].y - mapData.RealHeight * 0.5) / scaleMapToM);
            outputMapdata.startPosition.z = (rosCheckPoint[0].z / scaleMapToM);

            outputMapdata.checkPoint = new Vector3[rosCheckPoint.Count-1];
            for(int i=1;  i<rosCheckPoint.Count; i++ )
            {
                outputMapdata.checkPoint[i-1].x = ((rosCheckPoint[i].x + mapData.RealWidth * 0.5) / scaleMapToM);
                outputMapdata.checkPoint[i-1].y = -((rosCheckPoint[i].y - mapData.RealHeight * 0.5) / scaleMapToM);
                outputMapdata.checkPoint[i-1].z = (rosCheckPoint[i].z / scaleMapToM);
            }

            return outputMapdata;
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

            //double dir = (double)Axiom.Math.Utility.ASin(vecA.Dot(vecB));
            double dir = (double)Axiom.Math.Utility.ACos(vecA.Dot(vecB));
            Vector3 resVec = vecA.Cross(vecB);
            if (resVec.z > 0.0) dir = -dir;

            return dir;
        }

        /// <summary>
        /// 座標から角度を求める (x+1が 0.0度)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>角度</returns>
        public static double WorldPosToRad(double x, double y)
        {
            Vector3 tgtVec = new Vector3(x, y, 0);
            //Vector3 nothVec = new Vector3(0, -1, 0);
            Vector3 eastVec = new Vector3(1, 0, 0);

            return (VecToRad(tgtVec, eastVec));
        }
        
        public static double WorldVectorToRad(Vector3 srcVec)
        {
            return (VecToRad(srcVec, new Vector3(1, 0, 0)));
        }

        /// <summary>
        /// A,B２点を結ぶ直線上でPにもっとも近い点を探す
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="P">現在位置</param>
        /// <returns></returns>
        Vector3 nearestPoint(Vector3 A, Vector3 B, Vector3 P)
        {
            Vector3 a = new Vector3();
            Vector3 b = new Vector3();
            double r;

            a.x = B.x - A.x;
            a.y = B.y - A.y;
            b.x = P.x - A.x;
            b.y = P.y - A.y;

            r = (a.x * b.x + a.y * b.y) / (a.x * a.x + a.y * a.y);

            if (r <= 0.0)
            {
                // Aより手前
                return A;
            }
            else if (r >= 1.0)
            {
                // Bより奥
                return B;
            }
            else
            {
                Vector3 result = new Vector3();
                result.x = A.x + r * a.x;
                result.y = A.y + r * a.y;
                return result;
            }
        }

        /// <summary>
        /// ２点の直線上で、ahead分先に進んだ地点を返す
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        Vector3 nearestAHeadPoint(Vector3 A, Vector3 B, Vector3 P, double aheadLength )
        {
            Vector3 a = new Vector3();
            Vector3 b = new Vector3();
            double r;

            a.x = B.x - A.x;
            a.y = B.y - A.y;
            b.x = P.x - A.x;
            b.y = P.y - A.y;

            r = (a.x * b.x + a.y * b.y) / (a.x * a.x + a.y * a.y);


            Vector3 result = new Vector3();
            Vector3 ABvecNorm = new Vector3();
            double ABvecLeng = a.Length;
            ABvecNorm.x = a.x / ABvecLeng;
            ABvecNorm.y = a.y / ABvecLeng;


            if (r <= 0.0)
            {
                // Aより手前
                // AからaHead分前に進んだ場所
                result.x = A.x + (aheadLength * ABvecNorm.x);
                result.y = A.y + (aheadLength * ABvecNorm.y);
            }
            else if (r >= 1.0)
            {
                // Bより奥
                // BからaHead分前に進んだ場所
                result.x = B.x + (aheadLength * ABvecNorm.x);
                result.y = B.y + (aheadLength * ABvecNorm.y);
            }
            else
            {
                // A->Bへの直線上の最寄りの点からAhead分進んだ場所
                double vecLeng = ((r * ABvecLeng) + aheadLength);
                result.x = A.x + (vecLeng * ABvecNorm.x);
                result.y = A.y + (vecLeng * ABvecNorm.y);
            }

            // B[目標地点] より先に進まない
            if (nearestPoint(A, B, result) == B)
            {
                return B;
            }
            return result;
        }

        /// <summary>
        /// チェックポイントショートカット判定
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        bool JadgeCheckPointPass(Vector3 A, Vector3 B, Vector3 P)
        {
            Vector3 a = new Vector3();
            Vector3 b = new Vector3();
            double r;

            a.x = B.x - A.x;
            a.y = B.y - A.y;
            b.x = P.x - A.x;
            b.y = P.y - A.y;

            r = (a.x * b.x + a.y * b.y) / (a.x * a.x + a.y * a.y);

            if (r >= 1.0)
            {
                // Bより奥
                return true;
            }
            return false;
        }

        // ・チェックポイントリスト
        // ・ポイント追加、削除

        // ・チェックポイントシーケンサ
        // リセット
        public void ResetSeq()
        {
            SetCheckPointIndex(1);
            goalFlg = false;
        }

        /// <summary>
        /// チェックポイントインデックス変更
        /// </summary>
        /// <param name="_setIdx"></param>
        public void SetCheckPointIndex(int _setIdx )
        {
            if (_setIdx < GetNumCheckPoints())
            {
                seqIdx = _setIdx;
            }

            // 更新されたか？
            if (seqIdxOld != seqIdx)
            {
                bCheckPointTrg = true;
            }
            seqIdxOld = seqIdx;
        }

        /// <summary>
        /// 現在位置をセット
        /// ワールドマップ座標
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="dir"></param>
        public void SetNowPostion(double posX, double posY, double dir)
        {
            nowDir = dir;
            nowPos.x = posX;
            nowPos.y = posY;
        }

        public Vector3 GetNowCheckPoint()
        {
            return GetCheckPoint(seqIdx);
        }

        /// <summary>
        /// チェックポイント取得
        /// </summary>
        /// <param name="_seqIdx"></param>
        /// <returns></returns>
        public Vector3 GetCheckPoint(int _seqIdx)
        {
            Vector3 resVec = new Vector3();

            if (_seqIdx < 0) _seqIdx = 0;
            if (_seqIdx >= rosCheckPoint.Count)
            {
                _seqIdx = rosCheckPoint.Count - 1;
            }

            resVec.x = rosCheckPoint[_seqIdx].x;
            resVec.y = rosCheckPoint[_seqIdx].y;
            return resVec;
        }

        /// <summary>
        /// チェックポイント変更
        /// </summary>
        /// <param name="_seqIdx"></param>
        /// <param name="pos"></param>
        public void SetCheckPoint(int _seqIdx, Vector3 pos )
        {
            if (_seqIdx < 0 || _seqIdx >= rosCheckPoint.Count) return;

            rosCheckPoint.Remove(rosCheckPoint[_seqIdx]);
            rosCheckPoint.Insert( _seqIdx, new Vector3(pos.x, pos.y, pos.z));
        }

        /// <summary>
        /// チェックポイント追加
        /// </summary>
        /// <param name="_seqIdx"></param>
        /// <param name="pos"></param>
        public void AddCheckPoint(int _seqIdx, Vector3 pos)
        {
            if (_seqIdx < 0 ) return;

            if (_seqIdx >= rosCheckPoint.Count)
            {
                // 最後尾に追加
                rosCheckPoint.Add( new Vector3(pos.x, pos.y, pos.z) );
            }
            else
            {
                rosCheckPoint.Insert(_seqIdx, new Vector3(pos.x, pos.y, pos.z));
            }
        }

        /// <summary>
        /// チェックポイント削除
        /// </summary>
        /// <param name="_seqIdx"></param>
        public void RemoveCheckPoint(int _seqIdx )
        {
            if (_seqIdx < 0 || _seqIdx >= rosCheckPoint.Count) return;

            rosCheckPoint.Remove(rosCheckPoint[_seqIdx]);
        }
        
        /// <summary>
        /// チェックポイントでの目標向きを求める
        /// 次のチェックポイントへのむき
        /// </summary>
        /// <returns></returns>
        public Vector3 GetCheckPointToWayPoint(int cpIdx)
        {
            //if ((cpIdx + 1) == mapData.checkPoint.Length)
            if ((cpIdx + 1) == rosCheckPoint.Count)
            {
                // ゴールひとつ手前
                // まっすぐゴールに向かう
                return CalcWayPoint(GetCheckPoint(cpIdx - 1), GetCheckPoint(cpIdx));
            }
            else
            {
                return CalcWayPoint(GetCheckPoint(cpIdx - 1), GetCheckPoint(cpIdx), GetCheckPoint(cpIdx + 1));
            }
        }

        public Vector3 GetCheckPointToWayPoint()
        {
            return GetCheckPointToWayPoint(seqIdx);
        }

        /// <summary>
        /// 現在の目標地点
        /// </summary>
        /// <returns></returns>
        public Vector3 GetNowTargetPositon()
        {
            return tgtPos;
        }

        /// <summary>
        /// 目標への角度
        /// </summary>
        /// <param name="dir"></param>
        public double GetNowTargetDir()
        {
            return tgtDir;
        }


        /// <summary>
        /// 現在の向き
        /// </summary>
        /// <returns>角度</returns>
        public double GetNowDir()
        {
            return nowDir;
        }

        /// <summary>
        /// 目的方向へのハンドルの角度(向けたい方向)
        /// </summary>
        /// <returns>角度</returns>
        public double GetNowTargetStearingDir()
        {
            return tgtStearingDir;
        }

        /// <summary>
        /// チェックポイントのインデックス
        /// </summary>
        /// <returns></returns>
        public int GetCheckPointIdx()
        {
            return seqIdx;
        }

        /// <summary>
        /// チェックポイントの数
        /// </summary>
        /// <returns></returns>
        public int GetNumCheckPoints()
        {
            return rosCheckPoint.Count;// mapData.checkPoint.Length;
        }

        /// <summary>
        /// ゴールしたか？
        /// </summary>
        /// <returns></returns>
        public bool GetGoalStatus()
        {
            return goalFlg;
        }

        /// <summary>
        /// チェックポイント通過したか？
        /// </summary>
        /// <returns></returns>
        public bool TrgCheckPoint()
        {
            return bCheckPointTrg;
        }


        /// <summary>
        /// チェックポイント間ルーティング
        /// </summary>
        public void CalcRooting(bool bCheckPointPass )
        {
            bCheckPointTrg = false;

            // ゴールしてたら計算しない
            if (goalFlg) return;

            // チェックポイント短絡　判定
            // ターゲットがある程度の範囲内で、向きが大幅に違う場合パスする。
            if (bCheckPointPass)
            {
                if (JadgeCheckPointPass(GetCheckPoint(seqIdx - 1), GetCheckPoint(seqIdx), nowPos))
                {
                    SetCheckPointIndex(seqIdx + 1);
                }
            }
            // 
            CalcCheckPoint();
        }

        /// <summary>
        /// チェックポイントを探す
        /// </summary>
        /// <returns>ゴール判定</returns>
        private bool CalcCheckPoint()
        {
            // ゴールしてたら計算しない
            if (goalFlg || seqIdx >= GetNumCheckPoints())
            {
                goalFlg = true;
                return true;
            }

            // チェックポイントに到達したか？
            {
                // チェックポイントとの距離を計算
                double dist = (GetCheckPoint(seqIdx) - nowPos).Length;

                if (dist < touchRange)
                {
                    int nextSeqIdx = seqIdx + 1;

                    // ゴール判定
                    if (nextSeqIdx >= GetNumCheckPoints())
                    {
                        goalFlg = true;
                        return true;
                    }
                    else
                    {
                        SetCheckPointIndex(nextSeqIdx);
                    }
                }
            }

            // チェックポイント間の直線上の2m先をターゲットとする
            tgtPos = nearestAHeadPoint(GetCheckPoint(seqIdx - 1), GetCheckPoint(seqIdx), nowPos, 2.0);

            // 目標方向
            {
                Vector3 tgtVec = tgtPos - nowPos;
                //Vector3 nothVec = new Vector3(0, -1, 0);
                Vector3 nothVec = new Vector3(1, 0, 0);

                tgtDist = tgtVec.Length;

                tgtDir = VecToRad(tgtVec, nothVec);
            }
            return false;
        }
        
        /// <summary>
        /// 現在位置から、指定のチェックポイントまでの距離
        /// </summary>
        /// <param name="vPos"></param>
        /// <returns></returns>
        private double GetCheckPointDistance( int cpIndex )
        {
            return (rosCheckPoint[cpIndex] - nowPos).Length;
        }

        /// <summary>
        /// ３つのチェックポイントから中間のチェックポイント通過時の向きを返す
        /// </summary>
        /// <param name="cpPrev"></param>
        /// <param name="cpNow"></param>
        /// <param name="cpNext"></param>
        /// <returns></returns>
        public double Calc3CheckPointAngle(Vector3 cpPrev, Vector3 cpNow, Vector3 cpNext)
        {
            Vector3 vec1 = cpNow - cpPrev;
            Vector3 vec2 = cpNext - cpNow;

            return WorldVectorToRad((vec1 + vec2));
        }

        public double Calc2CheckPointAngle(Vector3 cpPrev, Vector3 cpNow)
        {
            Vector3 vec1 = cpNow - cpPrev;
            return WorldVectorToRad(vec1);
        }

        // ROS用　ウェイポイント作成
        public Vector3 CalcWayPoint( Vector3 cpPrev, Vector3 cpNow, Vector3 cpNext)
        {
            double angle = Calc3CheckPointAngle(cpPrev, cpNow, cpNext);
            return new Vector3(cpNow.x, cpNow.y, angle);
        }

        // ROS用　ウェイポイント作成 ゴール直前
        public Vector3 CalcWayPoint(Vector3 cpPrev, Vector3 cpNow )
        {
            double angle = Calc2CheckPointAngle(cpPrev, cpNow);
            return new Vector3(cpNow.x, cpNow.y, angle);
        }

        /// <summary>
        /// チェックポイント削減
        /// </summary>
        /// <param name="reductAngleRange">許容角度 radian </param>
        /// <returns></returns>
        public int CheckPointReduction(double reductAngleRange)
        {
            if (rosCheckPoint.Count < 2) return rosCheckPoint.Count;

            List<Vector3> newCp = new List<Vector3>();
            double cpAngle;

            // スタート地点格納
            newCp.Add(rosCheckPoint[0]);
            cpAngle = Calc2CheckPointAngle(rosCheckPoint[0], rosCheckPoint[1]);
            for (int i=1; i<rosCheckPoint.Count-1; i++ )
            {
                double wkAngle = Calc2CheckPointAngle(rosCheckPoint[i], rosCheckPoint[i+1]);

                // 許容角度以上の差があればチェックポイント格納
                if (Math.Abs(wkAngle - cpAngle) > reductAngleRange)
                {
                    newCp.Add(rosCheckPoint[i]);
                    cpAngle = wkAngle;
                }
            }
            // ゴール地点格納
            newCp.Add(rosCheckPoint[rosCheckPoint.Count - 1]);

            rosCheckPoint = newCp;
            return rosCheckPoint.Count;
        }


        /// <summary>
        /// ハンドリング
        /// </summary>
        /// <returns></returns>
        public double GetHandleValue()
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
            return stearingAng * (1.0 / maxStearingAng);
        }

        /// <summary>
        /// アクセル値計算
        /// </summary>
        /// <returns></returns>
        public double GetAccelValue()
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
