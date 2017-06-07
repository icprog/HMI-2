using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace NetSCADA6.Common.NSColorManger
{
    /// <summary>
    /// 单色
    /// </summary>
    internal class NSSolidBrushInfo
    {
        public NSSolidBrushInfo() { }
        public NSSolidBrushInfo(Color clr) { Color = clr; }
        public Color Color = Color.Red;
        public NSSolidBrushInfo Clone()
        {
            NSSolidBrushInfo other = new NSSolidBrushInfo();
            other.Color = this.Color;
            return other;
        }
        int version = 1;
        public void Serialize(BinaryFormatter bf, Stream s)
        {
            bf.Serialize(s, version);
            bf.Serialize(s, Color);
        }
        public void Deserialize(BinaryFormatter bf, Stream s)
        {
            version = (int)bf.Deserialize(s);
            Color = (Color)bf.Deserialize(s);
        }
    }

}
