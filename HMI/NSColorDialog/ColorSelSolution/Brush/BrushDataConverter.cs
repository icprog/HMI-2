using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using NetSCADA6.Common.NSColorManger;

namespace NetSCADA6.Common.NSColorManger
{

	public class BrushConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context,
			Type sourceType)
		{
			return false;
		}
		public override object ConvertTo(ITypeDescriptorContext context,
		   CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value is BrushData)
			{ 
				string s = string.Empty;
                NSBrushType type = (value as BrushData).BrushType;
                if (type == NSBrushType.Null)
					s = "空";
                else if (type == NSBrushType.Solid)
					s = "单色";
                else if (type == NSBrushType.Hatch)
					s = "图案";
                else if (type == NSBrushType.Textrue)
					s = "图片";
                else if (type == NSBrushType.LinearGradient)
					s = "渐变";
                else if (type == NSBrushType.PathGradient)
					s = "放射"; 
				return s;
			} 
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}

}
