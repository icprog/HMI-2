using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace NetSCADA6.HMI.NSDrawObj.PropertyEdit
{
	/// <summary>
	/// PointF属性显示
	/// </summary>
	public class PointFConverter : ExpandableObjectConverter
	{
		#region field
		private int _digits;
		#endregion
		
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
				string[] v = ((string)value).Split(new [] { ',' });
				return new PointF(float.Parse(v[0]), float.Parse(v[1]));
			}
			return base.ConvertFrom(context, culture, value);
		}
		public override object ConvertTo(ITypeDescriptorContext context,
		   CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value is PointF)
			{
				//获取小数位数
				if (_digits == 0)
				{
					Attribute attr = context.PropertyDescriptor.Attributes[typeof (DecimalDigitsAttribute)];
					if (attr != null)
						_digits = ((DecimalDigitsAttribute) attr).Digits;
				}
				
				PointF p = (PointF)value;
				return Math.Round(p.X, _digits) + "," + Math.Round(p.Y, _digits);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
