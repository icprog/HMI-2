using System.Collections.Generic;
using System.Drawing;

namespace NetSCADA6.NSInterface.HMI.DrawObj
{
	public interface INodeEdit
	{
		#region property
		/// <summary>
		/// 编辑状态
		/// </summary>
		NodesState NodeState { set; get; }
		/// <summary>
		/// 是否是在用鼠标以节点方式创建创建
		/// </summary>
		bool IsNodeCreating { get; set; }
		/// <summary>
		/// 是否是使用鼠标编辑节点
		/// </summary>
		bool IsNodeOperate { get; }
		/// <summary>
		/// 节点控件是否创建成功
		/// </summary>
		bool CreateSuccess { get; }
		/// <summary>
		/// 节点控件是否创建完成。比如直线，只需要创建两个点，便自动完成
		/// </summary>
		bool CreateFinish { get; }
		/// <summary>
		/// 节点控件包含的最小节点数
		/// </summary>
		int MinCount { get; }
		/// <summary>
		/// 节点数据
		/// </summary>
		List<PointF> NodeDatas { get; }
		#endregion

		#region function
		void MoveNode(PointF point, int pos);
		void AddNode(PointF point);
		void DeleteNode(int pos);
		/// <summary>
		/// 鼠标创建控件时绘制图形
		/// </summary>
		/// <param name="g"></param>
		void CreatingPaint(Graphics g);
		#endregion
	}
}
