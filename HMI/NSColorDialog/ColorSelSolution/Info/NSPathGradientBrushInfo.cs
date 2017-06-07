using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; 
using System.Drawing.Drawing2D;
using System.Drawing;

namespace NetSCADA6.Common.NSColorManger
{

    /// <summary>
    /// 放射 路径画刷
    /// </summary>
    internal class NSPathGradientBrushInfo
    {
        public NSPathGradientBrushInfo()
        {
            Init();
        }
        void Init()
        {
            ColorBlend cb = new ColorBlend(3);
            Color[] clrs = new Color[3]; clrs[0] = Color.Black; clrs[1] = Color.White; clrs[2] = Color.Black;
            float[] floats = new float[3]; floats[0] = 0; floats[1] = 0.5f; floats[2] = 1;
            cb.Colors = clrs; cb.Positions = floats;

            LinearGradient = new NSLinearGradientBrushInfo(cb, 0);
        }
        public NSPathGradientBrushInfo(ColorBlend cb) { LinearGradient = new NSLinearGradientBrushInfo(cb, 0); }

        public NSPathGradientBrushInfo Clone()
        {
            NSPathGradientBrushInfo other = new NSPathGradientBrushInfo();
            other.LinearGradient = this.LinearGradient.Clone();
            /*         other.focus = this.focus;
                     other.effect = this.effect;*/
            return other;
        }
        int version = 1;
        public void Serialize(BinaryFormatter bf, Stream s)
        {
            bf.Serialize(s, version);
            LinearGradient.Serialize(bf, s);
        }
        public void Deserialize(BinaryFormatter bf, Stream s)
        {
            version = (int)bf.Deserialize(s);
            LinearGradient.Deserialize(bf, s);
        }
        public NSLinearGradientBrushInfo LinearGradient;

    }

}
