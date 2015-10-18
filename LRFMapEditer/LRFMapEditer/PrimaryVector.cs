using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRFMapEditer
{
    // -------------------------------------------------------------------------
    // プライマリーベクター
    // VMap生成用に　必要なデータのみに
    // VecAが主

    // 計測で得られる障害物の辺と角度は、不変であるという考え方で
    // ２辺の角度を軸に前後の計測データと照合する。
    class PrimaryVector
    {
    public:
	    Vector3d	VecA;			// 辺
    //	Vector3d	VecB;

	    Vector3d	PosA;			// 位置
    //	Vector3d	PosB;		 

	    double		VecLngA;		// ベクトルの長さ
    //	double		VecLngB;
    //	double		VecRad;			// なす角　内積値（ベクトルの長さ*角度） ※VecRadに名前変更
    //	double		Ang;			// なす角　角度（度）

	    // 前回の計測との差分
	    bool		DiffFlg;		// 差分が計算できた
	    double		DiffAng;		// 角度差分(度)
	    double		DiffRadian;		// 角度差分（ラジアン）

	    Vector3d	DiffPos;		// 移動差分

	    int			StaticObsRank;	// 静的障害物重み付け (数値が高いほど静的な障害物)
    }
}
