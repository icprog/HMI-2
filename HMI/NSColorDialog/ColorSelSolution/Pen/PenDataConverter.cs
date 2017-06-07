using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using NetSCADA6.Common.NSColorManger;

namespace NetSCADA6.Common.NSColorManger
{

    public class PenConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context,
            Type sourceType)
        {
            return false;
        }
        public override object ConvertTo(ITypeDescriptorContext context,
           CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PenData)
            {
                RectangleF rf = new RectangleF(0, 0, 1, 1);
                Pen p = (value as PenData).CreatePen(rf, null);
                string s = string.Empty;
                if ((value as PenData).IsPiple)
                    s = "管道";
                else if (p == null)
                    s = "空";
                else if (p.Brush is SolidBrush)
                    s = "单色";
                else if (p.Brush is HatchBrush)
                    s = "图案";
                else if (p.Brush is TextureBrush)
                    s = "图片";
                else if (p.Brush is LinearGradientBrush)
                    s = "渐变";
                else if (p.Brush is PathGradientBrush)
                    s = "放射";

                if (p != null)
                    p.Dispose();
                return s;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

}
