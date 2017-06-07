using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NetSCADA6.Common.NSColorManger
{

    /// <summary>
    /// 底纹
    /// </summary>
    internal class NSHatchBrushInfo
    {
        public NSHatchBrushInfo() { }
        public NSHatchBrushInfo(HatchStyle hs, Color bk, Color fore) { BackColor = bk; ForeColor = fore; Style = hs; }

        public NSHatchBrushInfo Clone()
        {
            NSHatchBrushInfo other = new NSHatchBrushInfo();
            other.BackColor = this.BackColor;
            other.ForeColor = this.ForeColor;
            other.BackAValue = this.BackAValue;
            other.ForeAValue = this.ForeAValue;
            other.Style = this.Style;
            return other;
        }
        public HatchStyle Style = HatchStyle.BackwardDiagonal;
        public Color BackColor = Color.White;
        public Color ForeColor = Color.Black;

        /// <summary>
        /// 透明度百分比 - 背景
        /// </summary>
        internal int BackAValue = 100;
        /// <summary>
        /// 透明度百分比 - 前景
        /// </summary>
        internal int ForeAValue = 100;

        int version = 1;
        public void Serialize(BinaryFormatter bf, Stream s)
        {
            bf.Serialize(s, version);
            bf.Serialize(s, BackColor);
            bf.Serialize(s, ForeColor);
            bf.Serialize(s, ForeAValue);
            bf.Serialize(s, BackAValue);
            bf.Serialize(s, Style);
        }
        public void Deserialize(BinaryFormatter bf, Stream s)
        {
            version = (int)bf.Deserialize(s);
            BackColor = (Color)bf.Deserialize(s);
            ForeColor = (Color)bf.Deserialize(s);
            ForeAValue = (int)bf.Deserialize(s);
            BackAValue = (int)bf.Deserialize(s);
            Style = (HatchStyle)bf.Deserialize(s);
        }

    }

}
