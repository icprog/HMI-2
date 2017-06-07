using System.Collections;
using System.Collections.Generic;

using NetSCADA6.NSInterface.HMI.DrawObj;

namespace NetSCADA6.NSInterface.HMI.Form
{
	public interface IEnvironment
	{
		#region property
		/// <summary>
		/// 控件列表
		/// </summary>
		List<IDrawObj> Objs { get; }
		/// <summary>
		/// 当前层
		/// </summary>
		int DefaultLayer { set; get; }
		/// <summary>
		/// 层的可见信息
		/// </summary>
		BitArray VisibleLayers { get; }
		/// <summary>
		/// 层的锁定信息
		/// </summary>
		BitArray LockedLayers { get; }
		#endregion


		/// <summary>
		/// 初始化
		/// </summary>
		void Initialization();
		/// <summary>
		/// 窗体根据Style属性显示，为normal、modal、topmost
		/// </summary>
		void ShowStyle();
		/// <summary>
		/// 创建控件
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		IDrawObj CreateDrawObj(DrawType type);
		/// <summary>
		/// 控件图层改变
		/// </summary>
		/// <param name="obj"></param>
		void ObjectCanSelectChanged(IDrawObj obj);

		#region refresh
		/// <summary>
		/// 控件刷新
		/// </summary>
		/// <param name="sender"></param>
		void InvalidateObject(IDrawObj sender);
		/// <summary>
		/// 整体刷新
		/// </summary>
		void TimeInvalidate();
		#endregion
	}
}
