using System.Collections.Generic;
using System.Drawing.Design;
using NetSCADA6.NSInterface.HMI.DrawObj;

using UndoMethods;

namespace NetSCADA6.NSInterface.HMI.Form
{
	public interface IStudio : IEnvironment
	{
		/// <summary>
		/// 控件名称管理类
		/// </summary>
		IObjName NameManager { get; }
		/// <summary>
		/// Undo/Redo管理类
		/// </summary>
		IUndoRedoManager Undo { get; }
		/// <summary>
		/// 窗体缩放比例
		/// </summary>
		float FormScale { set; get; }
		/// <summary>
		/// 正交模式
		/// </summary>
		bool IsOrtho { set; get; }
		/// <summary>
		/// 工具箱项选择
		/// </summary>
		/// <param name="item"></param>
		void SetCreateObjectState(ToolboxItem item);
		/// <summary>
		/// 工具箱项创建
		/// </summary>
		/// <param name="item"></param>
		void CreateToolboxItem(ToolboxItem item);
		/// <summary>
		/// 状态栏数据改变事件
		/// </summary>
		void StatusLabelChanged();
		/// <summary>
		/// 选择控件改变事件
		/// </summary>
		void SelectedObjChanged();

	}
}
