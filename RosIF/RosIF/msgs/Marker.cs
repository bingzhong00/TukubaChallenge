//----------------------------------------------------------------
// <auto-generated>
//     This code was generated by the GenMsg. Version: 0.1.0.0
//     Don't change it manually.
//     2016-07-09T20:34:04+09:00
// </auto-generated>
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RosSharp.Message;
using RosSharp.Service;
using RosSharp.std_msgs;
using RosSharp.geometry_msgs;
namespace RosSharp.visualization_msgs
{
    ///<exclude/>
    public class Marker : IMessage
    {
        ///<exclude/>
        public Marker()
        {
            header = new Header();
            ns = string.Empty;
            pose = new Pose();
            scale = new Vector3();
            color = new ColorRGBA();
            points = new List<Point>();
            colors = new List<ColorRGBA>();
            text = string.Empty;
            mesh_resource = string.Empty;
        }
        ///<exclude/>
        public Marker(BinaryReader br)
        {
            Deserialize(br);
        }
        ///<exclude/>
        public const byte ARROW = 0;
        ///<exclude/>
        public const byte CUBE = 1;
        ///<exclude/>
        public const byte SPHERE = 2;
        ///<exclude/>
        public const byte CYLINDER = 3;
        ///<exclude/>
        public const byte LINE_STRIP = 4;
        ///<exclude/>
        public const byte LINE_LIST = 5;
        ///<exclude/>
        public const byte CUBE_LIST = 6;
        ///<exclude/>
        public const byte SPHERE_LIST = 7;
        ///<exclude/>
        public const byte POINTS = 8;
        ///<exclude/>
        public const byte TEXT_VIEW_FACING = 9;
        ///<exclude/>
        public const byte MESH_RESOURCE = 10;
        ///<exclude/>
        public const byte TRIANGLE_LIST = 11;
        ///<exclude/>
        public const byte ADD = 0;
        ///<exclude/>
        public const byte MODIFY = 0;
        ///<exclude/>
        public const byte DELETE = 2;
        ///<exclude/>
        public Header header { get; set; }
        ///<exclude/>
        public string ns { get; set; }
        ///<exclude/>
        public int id { get; set; }
        ///<exclude/>
        public int type { get; set; }
        ///<exclude/>
        public int action { get; set; }
        ///<exclude/>
        public Pose pose { get; set; }
        ///<exclude/>
        public Vector3 scale { get; set; }
        ///<exclude/>
        public ColorRGBA color { get; set; }
        ///<exclude/>
        public TimeSpan lifetime { get; set; }
        ///<exclude/>
        public bool frame_locked { get; set; }
        ///<exclude/>
        public List<Point> points { get; set; }
        ///<exclude/>
        public List<ColorRGBA> colors { get; set; }
        ///<exclude/>
        public string text { get; set; }
        ///<exclude/>
        public string mesh_resource { get; set; }
        ///<exclude/>
        public bool mesh_use_embedded_materials { get; set; }
        ///<exclude/>
        public string MessageType
        {
            get { return "visualization_msgs/Marker"; }
        }
        ///<exclude/>
        public string Md5Sum
        {
            get { return "18326976df9d29249efc939e00342cde"; }
        }
        ///<exclude/>
        public string MessageDefinition
        {
            get { return "uint8 ARROW=0\nuint8 CUBE=1\nuint8 SPHERE=2\nuint8 CYLINDER=3\nuint8 LINE_STRIP=4\nuint8 LINE_LIST=5\nuint8 CUBE_LIST=6\nuint8 SPHERE_LIST=7\nuint8 POINTS=8\nuint8 TEXT_VIEW_FACING=9\nuint8 MESH_RESOURCE=10\nuint8 TRIANGLE_LIST=11\nuint8 ADD=0\nuint8 MODIFY=0\nuint8 DELETE=2\nHeader header\nstring ns\nint32 id\nint32 type\nint32 action\nPose pose\nVector3 scale\nColorRGBA color\nduration lifetime\nbool frame_locked\nPoint[] points\nColorRGBA[] colors\nstring text\nstring mesh_resource\nbool mesh_use_embedded_materials"; }
        }
        ///<exclude/>
        public bool HasHeader
        {
            get { return false; }
        }
        ///<exclude/>
        public void Serialize(BinaryWriter bw)
        {
            header.Serialize(bw);
            bw.WriteUtf8String(ns);
            bw.Write(id);
            bw.Write(type);
            bw.Write(action);
            pose.Serialize(bw);
            scale.Serialize(bw);
            color.Serialize(bw);
            bw.WriteTimeSpan(lifetime);
            bw.Write(frame_locked);
            bw.Write(points.Count); for(int i=0; i<points.Count; i++) { points[i].Serialize(bw);}
            bw.Write(colors.Count); for(int i=0; i<colors.Count; i++) { colors[i].Serialize(bw);}
            bw.WriteUtf8String(text);
            bw.WriteUtf8String(mesh_resource);
            bw.Write(mesh_use_embedded_materials);
        }
        ///<exclude/>
        public void Deserialize(BinaryReader br)
        {
            header = new Header(br);
            ns = br.ReadUtf8String();
            id = br.ReadInt32();
            type = br.ReadInt32();
            action = br.ReadInt32();
            pose = new Pose(br);
            scale = new Vector3(br);
            color = new ColorRGBA(br);
            lifetime = br.ReadTimeSpan();
            frame_locked = br.ReadBoolean();
            points = new List<Point>(br.ReadInt32()); for(int i=0; i<points.Capacity; i++) { var x = new Point(br);points.Add(x);}
            colors = new List<ColorRGBA>(br.ReadInt32()); for(int i=0; i<colors.Capacity; i++) { var x = new ColorRGBA(br);colors.Add(x);}
            text = br.ReadUtf8String();
            mesh_resource = br.ReadUtf8String();
            mesh_use_embedded_materials = br.ReadBoolean();
        }
        ///<exclude/>
        public int SerializeLength
        {
            get { return header.SerializeLength + 4 + ns.Length + 4 + 4 + 4 + pose.SerializeLength + scale.SerializeLength + color.SerializeLength + 8 + 1 + 4 + points.Sum(x => x.SerializeLength) + 4 + colors.Sum(x => x.SerializeLength) + 4 + text.Length + 4 + mesh_resource.Length + 1; }
        }
        ///<exclude/>
        public bool Equals(Marker other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.header.Equals(header) && other.ns.Equals(ns) && other.id.Equals(id) && other.type.Equals(type) && other.action.Equals(action) && other.pose.Equals(pose) && other.scale.Equals(scale) && other.color.Equals(color) && other.lifetime.Equals(lifetime) && other.frame_locked.Equals(frame_locked) && other.points.SequenceEqual(points) && other.colors.SequenceEqual(colors) && other.text.Equals(text) && other.mesh_resource.Equals(mesh_resource) && other.mesh_use_embedded_materials.Equals(mesh_use_embedded_materials);
        }
        ///<exclude/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Marker)) return false;
            return Equals((Marker)obj);
        }
        ///<exclude/>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = 0;
                result = (result * 397) ^ header.GetHashCode();
                result = (result * 397) ^ ns.GetHashCode();
                result = (result * 397) ^ id.GetHashCode();
                result = (result * 397) ^ type.GetHashCode();
                result = (result * 397) ^ action.GetHashCode();
                result = (result * 397) ^ pose.GetHashCode();
                result = (result * 397) ^ scale.GetHashCode();
                result = (result * 397) ^ color.GetHashCode();
                result = (result * 397) ^ lifetime.GetHashCode();
                result = (result * 397) ^ frame_locked.GetHashCode();
                result = (result * 397) ^ points.GetHashCode();
                result = (result * 397) ^ colors.GetHashCode();
                result = (result * 397) ^ text.GetHashCode();
                result = (result * 397) ^ mesh_resource.GetHashCode();
                result = (result * 397) ^ mesh_use_embedded_materials.GetHashCode();
                return result;
            }
        }
    }
}
