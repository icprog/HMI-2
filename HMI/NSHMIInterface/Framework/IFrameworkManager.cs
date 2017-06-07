using System;
using NetSCADA6.NSInterface.HMI.Form;

namespace NetSCADA6.NSInterface.HMI.Framework
{
	/// <summary>
	/// 图形框架管理类,只在编辑版中使用
	/// </summary>
	public interface IFrameworkManager : IDisposable
	{
		/// <summary>
		/// 当前窗体
		/// </summary>
		IHMIForm ActiveForm { get; }
		
		/// <summary>
		/// 属性框，选择控件
		/// </summary>
		void SelectObjectFunction(object obj);
		/// <summary>
		/// 属性框， 刷新
		/// </summary>
		void RefreshPropertyFunction();
		/// <summary>
		/// 工具箱，复位到指针
		/// </summary>
		void ResetToolboxPointerFunction();

		#region function
		/// <summary>
		/// 当前窗口切换
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ActiveDocumentChanged(object sender, EventArgs e);
		#endregion


	}
}
