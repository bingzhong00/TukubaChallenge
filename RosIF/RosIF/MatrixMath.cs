using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosIF
{
    class MatrixMath
    {
        public class Vector3f
        {
            public float x;
            public float y;
            public float z;
        }

        //クォータニオン構造体
        public class Quaternion
        {
            public float w;
            public float x;
            public float y;
            public float z;
        }

        //マトリクス構造体
        public class MATRIX
        {
            public float _11, _12, _13, _14;
            public float _21, _22, _23, _24;
            public float _31, _32, _33, _34;
            public float _41, _42, _43, _44;

            public MATRIX()
            {
                //単位行列に初期化
                this._11 = this._22 = this._33 = this._44 = 1;
            }
        }


        //任意軸回転をクォータニオンにする
        public static Quaternion RotateToQuaternion(Vector3f vAxis, float Angle)
        {
            Quaternion q = new RosIF.MatrixMath.Quaternion();
            float radian = (float)(Angle * (Math.PI / 180.0) / 2.0);
            float s = (float)Math.Sin(radian);
            q.w = (float)Math.Cos(radian);
            q.x = vAxis.x * s;
            q.y = vAxis.y * s;
            q.z = vAxis.z * s;
            return q;
        }

        //クォータニオンを回転行列にする
        public static MATRIX QuaternionToMatrix(Quaternion q)
        {
            MATRIX ret = new RosIF.MatrixMath.MATRIX();
            float sx = q.x * q.x;
            float sy = q.y * q.y;
            float sz = q.z * q.z;
            float cx = q.y * q.z;
            float cy = q.x * q.z;
            float cz = q.x * q.y;
            float wx = q.w * q.x;
            float wy = q.w * q.y;
            float wz = q.w * q.z;

            ret._11 = 1.0f - 2.0f * (sy + sz);
            ret._12 = 2.0f * (cz + wz);
            ret._13 = 2.0f * (cy - wy);
            ret._21 = 2.0f * (cz - wz);
            ret._22 = 1.0f - 2.0f * (sx + sz);
            ret._23 = 2.0f * (cx + wx);
            ret._31 = 2.0f * (cy + wy);
            ret._32 = 2.0f * (cx - wx);
            ret._33 = 1.0f - 2.0f * (sx + sy);
            ret._41 = 0.0f;
            ret._42 = 0.0f;
            ret._43 = 0.0f;
            return ret;
        }

        //回転行列をクォータニオンにする
        public static Quaternion MatrixToQuaternion(MATRIX mat)
        {
            Quaternion q = new RosIF.MatrixMath.Quaternion();

            float s;
            float tr = mat._11 + mat._22 + mat._33 + 1.0f;
            if (tr >= 1.0f)
            {
                s = 0.5f / (float)Math.Sqrt(tr);
                q.w = 0.25f / s;
                q.x = (mat._23 - mat._32) * s;
                q.y = (mat._31 - mat._13) * s;
                q.z = (mat._12 - mat._21) * s;
                return q;
            }
            else
            {
                float max;
                if (mat._22 > mat._33)
                {
                    max = mat._22;
                }
                else
                {
                    max = mat._33;
                }

                if (max < mat._11)
                {
                    s = (float)Math.Sqrt(mat._11 - (mat._22 + mat._33) + 1.0f);
                    float x = s * 0.5f;
                    s = 0.5f / s;
                    q.x = x;
                    q.y = (mat._12 + mat._21) * s;
                    q.z = (mat._31 + mat._13) * s;
                    q.w = (mat._23 - mat._32) * s;
                    return q;
                }
                else if (max == mat._22)
                {
                    s = (float)Math.Sqrt(mat._22 - (mat._33 + mat._11) + 1.0f);
                    float y = s * 0.5f;
                    s = 0.5f / s;
                    q.x = (mat._12 + mat._21) * s;
                    q.y = y;
                    q.z = (mat._23 + mat._32) * s;
                    q.w = (mat._31 - mat._13) * s;
                    return q;
                }
                else
                {
                    s = (float)Math.Sqrt(mat._33 - (mat._11 + mat._22) + 1.0f);
                    float z = s * 0.5f;
                    s = 0.5f / s;
                    q.x = (mat._31 + mat._13) * s;
                    q.y = (mat._23 + mat._32) * s;
                    q.z = z;
                    q.w = (mat._12 - mat._21) * s;
                    return q;
                }
            }
        }

        /// <summary>
        /// クォータニオンからX軸の向き（ラジアン）へ変換
        /// </summary>
        /// <param name="qX"></param>
        /// <param name="qY"></param>
        /// <param name="qZ"></param>
        /// <param name="qW"></param>
        /// <returns></returns>
        public static float QuaternionToAngle( float qX, float qY, float qZ, float qW )
        {
            Quaternion qrt = new RosIF.MatrixMath.Quaternion { x = qX, y = qY, z = qZ, w = qW };
            MATRIX mat = QuaternionToMatrix(qrt);

            return (float)Math.Atan2(mat._12, mat._11);
        }


    }
}
