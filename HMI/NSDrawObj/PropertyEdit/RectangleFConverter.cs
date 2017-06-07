using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace NetSCADA6.HMI.NSDrawObj.PropertyEdit
{
	public class RectangleFConverter : ExpandableObjectConverter
	{
		/// <summary>
		/// RectangleF属性显示
		/// </summary>
		/// <param name="context"></param>
		/// <param name="sourceType"></param>
		/// <returns></returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context,
			Type sourceType)
		{
			if (sourceType == typeof(string))
				return true;
			return base.CanConvertFrom(context, sourceType);
		}
		public override object ConvertFrom(ITypeDescriptorContext context,
		   CultureInfo culture, object value)
		{
			if (value is string)
			{
				string[] v = ((string)value).Split(new char[] { ',' });
				return new RectangleF(float.Parse(v[0]), float.Parse(v[1]),
					float.Parse(v[2]), float.Parse(v[3]));
			}
			return base.ConvertFrom(context, culture, value);
		}
		public override object ConvertTo(ITypeDescriptorContext context,
		   CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value is RectangleF)
			{
				Rectangle rect = Rectangle.Round((RectangleF) value);
				return rect.X + "," + rect.Y + "," + rect.Width + "," + rect.Height;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(value, attributes);
			//排序
			return pdc.Sort(new string[] {"X", "Y", "Width", "Height"});
		}
	}
}
