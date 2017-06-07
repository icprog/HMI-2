using System.Drawing;

namespace NetSCADA6.NSInterface.HMI.DrawObj
{
	public interface ICustomEdit
	{
		#region property
		/// <summary>
		/// 中心点
		/// </summary>
		PointF CustomCenter { get; }
		/// <summary>
		/// 数据
		/// </summary>
		PointF[] CustomDatas { get; }
		#endregion

		#region function
		/// <summary>
		/// 鼠标移动自定义点
		/// </summary>
		/// <param name="point"></param>
		/// <param name="pos"></param>
		void MoveCustom(PointF point, int pos);
		/// <summary>
		/// 生成数据
		/// </summary>
		void GenerateCustom();
		#endregion
	}
}
