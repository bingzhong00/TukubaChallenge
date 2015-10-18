using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRFMapEditer
{




    // ----------------------------------------------------------------------
    class VectorMap
    {
        public:
	        // マップ用プライマリーベクトル
	        std::vector<PrimaryVector*> worldPrm;

	        // 計測点ルート
	        std::vector<Vector3d*> measureRootPos;

        private:
	        VectorBuffer *pPrev2Vec;		// 2つ前のデータ
	        VectorBuffer *pPrevVec;			// 直前のデータ(動く障害物判定)
	        VectorBuffer *pNowVec;			// 今回のデータ(推定 現在 位置・角度判定)

        public:
	        VectorMap(void);
	        ~VectorMap(void);

	        void ChainVectorBuffer( VectorBuffer *pVecBuf );

	        void CombineVectorBufferToWorldMap( VectorBuffer *pAddVB );

	        bool CalcActiveObstacle( PrimaryVector *pPrm, VectorBuffer *pPrevVB, VectorBuffer *pNextVB );

	        void MakeMeasureRoot( VectorBuffer *pVB );

	        // LocationEstimate
	        LocEstArea* MakeArea_FromWorldPos( Vector3d worldPos, double areaSize );
	        void LocationEstimate( Vector3d *rstWorldPos, double *rstWorldAng, VectorBuffer *pLocVB );
	        int LocationEstimate_NearPos( Vector3d *rstWorldPos, double *rstWorldAng, VectorBuffer *pLocVB, Vector3d worldPos );

	        // Draw
	        void DrawWorldPrimaryVector();
	        void DrawAreaPrimaryVector(Vector3d worldPos);

	        // FileIO
	        bool Save( char *fname );
	        bool Load( char *fname );

        private:
	        bool WriteVector( FILE *fp, Vector3d& vec );
	        bool WriteDouble( FILE *fp, double val );
	        bool WriteInt( FILE *fp, int val );

	        Vector3d ReadVector( FILE *fp );
	        double ReadDouble( FILE *fp );
	        int ReadInt( FILE *fp );
    }
}
