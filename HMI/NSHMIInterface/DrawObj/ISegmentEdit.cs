using System.Drawing;

namespace NetSCADA6.NSInterface.HMI.DrawObj
{
	/// <summary>
	/// 切割控件控制接口
	/// </summary>
	public interface ISegmentEdit
	{
		/// <summary>
		/// 交点集合
		/// </summary>
		PointF[] Intersections { get; }
	}
}
