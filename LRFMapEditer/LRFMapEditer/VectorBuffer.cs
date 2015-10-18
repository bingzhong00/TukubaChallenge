using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRFMapEditer
{
    // -------------------------------------------------------------------------
    // 計測１回分のデータ
    class VectorBuffer
    {
    public:
	    int			numVec;			// 座標の個数
	    Vector3d	*pPos;			// 座標
	    Vector3d	*pVec;			// ベクトルバッファ
	    double		*pVecLng;		// ベクトルの長さ

	    int				numPrmVec;	// プライマリベクタの数
	    PrimaryVector	*pPrmVec;	// プライマリベクタ配列

	    // ※WolrdPos Angは、VectorBufferの結果なので、保持し続けるものではないのかも。
	    //   削除予定で検討
	    Vector3d	DiffPos;		// 前からの差分座標
	    double		DiffAng;

	    Vector3d	WorldPos;		// World座標
	    double		WorldAng;

    public:
	    VectorBuffer(void);
	    ~VectorBuffer(void);

	    // 計測データをベクトルにしてとりこむ
	    bool init(int num);
	    int  InportLRFData(short* pData, int aMin, int aMax, int aRes, int aFrt);
	    void InportPrimaryVector( std::vector<PrimaryVector*> &vPrmVec );

	    int CalcWorldPosAng(VectorBuffer *pNearVB );
	    int VoteWorldPosAng(VectorBuffer *pNearVB);

	    void DrawWorldPrimaryVector();
	    void DrawInputPoint();
	    void DrawVector();

    private:
	    int IntensiveVec(int startIdx, Vector3d* pAddVec );
    }
}
