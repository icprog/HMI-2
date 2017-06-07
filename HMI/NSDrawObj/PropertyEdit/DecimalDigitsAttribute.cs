using System;

namespace NetSCADA6.HMI.NSDrawObj.PropertyEdit
{
	/// <summary>
	/// 小数位数属性,用于设置在属性框中显示的小数位数,仅应用于PointFConverter中
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class DecimalDigitsAttribute : Attribute
	{
		public DecimalDigitsAttribute(int digits)
		{
			if (digits >= 0)
				_digits = digits;
		}

		private int _digits;
		public int Digits
		{
			get { return _digits; }
		}
	}
}
