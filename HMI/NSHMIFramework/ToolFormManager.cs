using NetSCADA6.HMI.NSHMIToolForm;

namespace NetSCADA6.HMI.NSHMIFramework
{
	/// <summary>
	/// 图形工具窗体管理类
	/// </summary>
	public class ToolFormManager
	{
		#region property
		private readonly ToolboxForm _toolbox = new ToolboxForm();
		/// <summary>
		/// 工具箱
		/// </summary>
		public ToolboxForm Toolbox
		{
			get { return _toolbox; }
		}
		#endregion
	}
}
