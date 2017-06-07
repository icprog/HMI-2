using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Drawing;

namespace NetSCADA6.Common.NSColorManger
{
    /// <summary>
    /// 渐变
    /// </summary>
    internal class NSLinearGradientBrushInfo
    {
        private NSLinearGradientBrushInfo() { }
        public NSLinearGradientBrushInfo(ColorBlend clrBnd, float angle) { ColorBlend = clrBnd; Angle = angle; }
        public NSLinearGradientBrushInfo Clone()
        {
            NSLinearGradientBrushInfo other = new NSLinearGradientBrushInfo();
            other.ColorBlend = new ColorBlend(ColorBlend.Colors.Length);
            for (int i = 0; i < ColorBlend.Colors.Length; i++)
            {
                other.ColorBlend.Colors[i] = ColorBlend.Colors[i];
                other.ColorBlend.Positions[i] = ColorBlend.Positions[i];
            }

            other.Angle = this.Angle;
            return other;
        }
        public ColorBlend ColorBlend
        {
            get;
            set;
        }
        public float Angle = 0;
        int version = 1;
        public void Serialize(BinaryFormatter bf, Stream s)
        {
            bf.Serialize(s, version);
            bf.Serialize(s, Angle);
            bf.Serialize(s, ColorBlend.Colors.Length);
            for (int i = 0; i < ColorBlend.Colors.Length; i++)
            {
                bf.Serialize(s, ColorBlend.Colors[i]);
                bf.Serialize(s, ColorBlend.Positions[i]);
            }
        }
        public void Deserialize(BinaryFormatter bf, Stream s)
        {
            version = (int)bf.Deserialize(s);
            Angle = (float)bf.Deserialize(s);
            int length = (int)bf.Deserialize(s);
            Color[] clrs = new Color[length];
            float[] pos = new float[length];
            for (int i = 0; i < length; i++)
            {
                clrs[i] = (Color)bf.Deserialize(s);
                pos[i] = (float)bf.Deserialize(s);
            }
            ColorBlend.Colors = clrs;
            ColorBlend.Positions = pos;
        }
    }

}
