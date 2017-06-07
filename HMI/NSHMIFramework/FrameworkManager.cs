using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using NetSCADA6.NSInterface.HMI.Form;
using NetSCADA6.NSInterface.HMI.Framework;
using NetSCADA6.NSStudio.NSIProject;
using WeifenLuo.WinFormsUI.Docking;

namespace NetSCADA6.HMI.NSHMIFramework
{
	/// <summary>
	/// Framework管理类，只在编辑版中使用
	/// </summary>
	public class FrameworkManager : IFrameworkManager
	{
		public FrameworkManager(HMIStudio studioManager)
		{
			_studioManager = studioManager;
		}

		#region property
		private readonly HMIStudio _studioManager;
		public HMIStudio StudioManager { get { return _studioManager; } }
		private readonly List<HMIFramework> _openedList = new List<HMIFramework>();
		/// <summary>
		/// 工程集合
		/// </summary>
		public List<HMIFramework> OpenedList
		{
			get { return _openedList; }
		}
		public DockPanel MainPanel { set; get; }
		#endregion

		#region public function
		public HMIFramework this[INSProjectDirector projectInfo]
		{
			get { return FindOpened(projectInfo); }
		}
		#endregion

		#region Framework
		public HMIFramework Open()
		{
			HMIFramework f = new HMIFramework(this); 
			_openedList.Add(f);
			return f;
		}
		public void Close(INSProjectDirector projectInfo)
		{
			HMIFramework f = FindOpened(projectInfo);
			if (f != null)
			{
				_openedList.Remove(f);
				f.Close();
			}
		}
		public HMIFramework FindOpened(INSProjectDirector projectInfo)
		{
			return _openedList.FirstOrDefault(t => projectInfo == t.ProjectInfo);
		}
		#endregion

		#region dispose
		private bool _disposed;
		protected bool Disposed
		{
			get { return _disposed; }
		}
		public void Dispose()
		{
			DisposeResource();

			GC.SuppressFinalize(this);
		}
		private void DisposeResource()
		{
			if (!_disposed)
			{
				while (_openedList.Count > 0)
				{
					_openedList[0].Close();
				}

				_openedList.Clear();
				_disposed = true;
			}
		}
		~FrameworkManager()
		{
			DisposeResource();
		}
		#endregion

		#region PropertyGrid
		/// <summary>
		/// 设置属性框选择控件
		/// </summary>
		public void SelectObjectFunction(object obj)
		{
			_studioManager.SelectedObject(obj);
		}
		/// <summary>
		/// 属性框刷新
		/// </summary>
		public void RefreshPropertyFunction()
		{
			_studioManager.RefreshProperty();
		}
		#endregion

		#region toolbox
		/// <summary>
		/// 工具箱复位到指针
		/// </summary>
		public void ResetToolboxPointerFunction()
		{
			_studioManager.ResetToolboxPointer();
		}
		private ToolboxItem _currentToolboxItem;
		public ToolboxItem CurrentToolboxItem
		{
			get { return _currentToolboxItem; }
		}
		public void SelectItemChanged(ToolboxItem item)
		{
			if (_currentToolboxItem != item)
			{
				_currentToolboxItem = item;
				SetCreateObjectState(_currentToolboxItem);
			}
		}
		private void SetCreateObjectState(ToolboxItem item)
		{
			if (_activeForm != null && item != null)
				_activeForm.Studio.SetCreateObjectState(_currentToolboxItem);
		}
		public void CreateToolboxItem()
		{
			if (_activeForm != null && _currentToolboxItem != null)
				_activeForm.Studio.CreateToolboxItem(_currentToolboxItem);
		}
		#endregion

		private IHMIForm _activeForm;
		/// <summary>
		/// 当前激活图形窗体
		/// </summary>
		public IHMIForm ActiveForm
		{
			get { return _activeForm; }
		}
		public void ActiveDocumentChanged(object sender, EventArgs e)
		{
			if (sender is DockPanel)
			{
				_activeForm = (sender as DockPanel).ActiveDocument as IHMIForm;
			}

			SetCreateObjectState(_currentToolboxItem);
		}

		#region fileNode
		private readonly FileNode _file = new FileNode();
		internal FileNode File { get { return _file; } }
		#endregion
	}
}
