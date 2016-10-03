//----------------------------------------------------------------
// <auto-generated>
//     This code was generated by the GenMsg. Version: 0.1.0.0
//     Don't change it manually.
//     2016-08-22T22:34:16+09:00
// </auto-generated>
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RosSharp.Message;
using RosSharp.Service;
using RosSharp.std_msgs;
namespace RosSharp.rosgraph_msgs
{
    ///<exclude/>
    public class Clock : IMessage
    {
        ///<exclude/>
        public Clock()
        {
        }
        ///<exclude/>
        public Clock(BinaryReader br)
        {
            Deserialize(br);
        }
        ///<exclude/>
        public DateTime clock { get; set; }
        ///<exclude/>
        public string MessageType
        {
            get { return "rosgraph_msgs/Clock"; }
        }
        ///<exclude/>
        public string Md5Sum
        {
            get { return "a9c97c1d230cfc112e270351a944ee47"; }
        }
        ///<exclude/>
        public string MessageDefinition
        {
            get { return "time clock"; }
        }
        ///<exclude/>
        public bool HasHeader
        {
            get { return false; }
        }
        ///<exclude/>
        public void Serialize(BinaryWriter bw)
        {
            bw.WriteDateTime(clock);
        }
        ///<exclude/>
        public void Deserialize(BinaryReader br)
        {
            clock = br.ReadDateTime();
        }
        ///<exclude/>
        public int SerializeLength
        {
            get { return 8; }
        }
        ///<exclude/>
        public bool Equals(Clock other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.clock.Equals(clock);
        }
        ///<exclude/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Clock)) return false;
            return Equals((Clock)obj);
        }
        ///<exclude/>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = 0;
                result = (result * 397) ^ clock.GetHashCode();
                return result;
            }
        }
    }
}
