using System;
using System.Windows.Forms;
using NetSCADA6.HMI.NSHMIForm;

namespace NetSCADA6.HMI.NSHMIFramework
{
	/// <summary>
	/// 图形编辑环境管理类
	/// </summary>
	public class HMIStudio
	{
		public HMIStudio()
		{
			_frameworks = new FrameworkManager(this);
			
			Init();
		}
		
		#region Property
		private readonly ToolFormManager _toolForms = new ToolFormManager();
		/// <summary>
		/// 工具窗体集合
		/// </summary>
		public ToolFormManager ToolForms
		{
			get { return _toolForms; }
		}
		private readonly FrameworkManager _frameworks;
		/// <summary>
		/// 图形框架集合
		/// </summary>
		public FrameworkManager Frameworks
		{
			get { return _frameworks; }
		}
		/// <summary>
		/// 属性框
		/// </summary>
		public PropertyGrid PropertyGrid { set; get; }
		#endregion

		#region private function
		private void Init()
		{
			ToolForms.Toolbox.AddTab(Rs.sVectorTabName, HMIForm.GetVectorTypes());
			ToolForms.Toolbox.Initialize();
			ToolForms.Toolbox.ToolBoxControl.SelectItemChanged += _frameworks.SelectItemChanged;
			ToolForms.Toolbox.ToolBoxControl.CreateToolboxItem += _frameworks.CreateToolboxItem;
		}
		#endregion

		#region public function
		public void SelectedObject(object obj)
		{
			PropertyGrid.SelectedObject = obj;
		}
		public void RefreshProperty()
		{
			PropertyGrid.Refresh();
		}
		public void ResetToolboxPointer()
		{
			_toolForms.Toolbox.ToolBoxControl.ResetPointer();
		}
		#endregion

		#region dispose
		private bool _disposed;
		public void Dispose()
		{
			DisposeResource();

			GC.SuppressFinalize(this);
		}
		private void DisposeResource()
		{
			if (!_disposed)
			{
				while (_frameworks.OpenedList.Count > 0)
					_frameworks.OpenedList[0].Dispose();

				_disposed = true;
			}

		}
		~HMIStudio()
		{
			DisposeResource();
		}
		#endregion

	}
}
