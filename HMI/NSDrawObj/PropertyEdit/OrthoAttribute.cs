using System;
using NetSCADA6.NSInterface.HMI.DrawObj;

namespace NetSCADA6.HMI.NSDrawObj
{
	/// <summary>
	/// 正交模式属性，判断控件在正交模式下如何创建
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class OrthoAttribute : Attribute
	{
		public OrthoAttribute(OrthoMode state)
		{
			_state = state;
		}

		private readonly OrthoMode _state;
		public OrthoMode State { get { return _state; } }
	}
}
