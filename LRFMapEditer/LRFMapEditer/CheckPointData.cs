using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LRFMapEditer
{
    public class CheckPointData
    {
        public double wdPosX;
        public double wdPosY;

        public CheckPointData()
        {
            wdPosX = 0.0;
            wdPosY = 0.0;
        }
        public CheckPointData(double x, double y)
        {
            wdPosX = x;
            wdPosY = y;
        }

        /// <summary>
        /// データ 書き込み
        /// </summary>
        /// <param name="strm"></param>
        public void Write(BinaryWriter strm)
        {
            strm.Write((int)2); // 要素数

            strm.Write(wdPosX);
            strm.Write(wdPosY);
        }

        /// <summary>
        /// データ　読み込み
        /// </summary>
        /// <param name="strm"></param>
        public void Read(BinaryReader strm)
        {
            strm.ReadInt32(); // 要素数

            wdPosX = strm.ReadDouble();
            wdPosY = strm.ReadDouble();
        }
    }
}
