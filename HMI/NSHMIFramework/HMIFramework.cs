using System;
using System.Windows.Forms;

using NetSCADA6.NSInterface.HMI.Framework;
using NetSCADA6.NSInterface.NSVariable;
using NetSCADA6.NSStudio.NSIProject;
using NetSCADA6.NSStudio.NSMoudleInterface;

namespace NetSCADA6.HMI.NSHMIFramework
{
    public class HMIFramework : IFramework
    {
		public HMIFramework(IFrameworkManager manager = null)
		{
			_manager = manager;
			_isStudioMode = (_manager != null);
			
			Init();
        }

        #region property
        public  FormOperation Forms { set; get; }
    	private string _hmiPath;
    	/// <summary>
    	/// 图形目录
    	/// </summary>
    	public string HMIPath
    	{
			set { _hmiPath = value; }
			get { return _hmiPath; }
    	}
		private INSProjectDirector _projectInfo;
		/// <summary>
		/// 项目管理器
		/// </summary>
		public INSProjectDirector ProjectInfo { get { return _projectInfo; } }
        #endregion

        #region private function
        void Init()
        {
			Forms = new FormOperation(this);

        	_timerRefresh.Tick += TimerRefresh;
        	_timerRefresh.Interval = 50;
        	_timerRefresh.Enabled = true;
        }
        #endregion

		#region run mode
		#region var
		public void OnDataChanged(string name, double value)
        {
			int count = Forms.OpenedList.Count;
			for (int i = 0; i < count; i++)
				Forms.OpenedList[i].Run.OnDataChanged(name, value);
		}
        private void OnDataChanged(string name, string value)
        {
			int count = Forms.OpenedList.Count;
			for (int i = 0; i < count; i++)
				Forms.OpenedList[i].Run.OnDataChanged(name, value);
		}
		public void OnDataChanged(INSVariable variable)
		{
			if (variable.CurValue is string)
				OnDataChanged(variable.VarName, variable.CurValue as string);
			else
			{
				OnDataChanged(variable.VarName, Convert.ToDouble(variable.CurValue));
			}
		}
		private readonly Timer _timerRefresh = new Timer();
		//定时刷新
		private void TimerRefresh(object sender, EventArgs e)
		{
			int count = Forms.OpenedList.Count;
			for (int i = 0; i < count; i++)
				Forms.OpenedList[i].Common.TimeInvalidate();
		}
        #endregion
		#endregion

		#region IFramework
		#region property
		private readonly bool _isStudioMode;
    	public bool IsStudioMode
    	{
			get { return _isStudioMode; }
    	}
		private readonly IFrameworkManager _manager;
		public IFrameworkManager Manager
		{
			get { return _manager; }
		}
		#endregion
        #endregion

		#region dispose
		public void Close()
		{
			Dispose();
		}
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
				Forms.CloseAll();
				_timerRefresh.Dispose();
				//预防直接调用Framework的Close
				if (_manager != null)
					((FrameworkManager)_manager).OpenedList.Remove(this);

				_disposed = true;
			}

		}
		~HMIFramework()
		{
			DisposeResource();
		}
		#endregion

		#region studio
		public void SetMoudleInterface(INSMoudleInterface moudle)
		{
			FrameworkManager m = (FrameworkManager)Manager;
			if (m.MainPanel == null)
			{
				m.MainPanel = moudle.nDockPanel;
				m.StudioManager.PropertyGrid = moudle.nPropertyGrid;
			}

			_projectInfo = moudle.nProjectDirector;
			if (_projectInfo != null)
				_hmiPath = _projectInfo.GetDirectoryManager().GetWindowPath();
			m.File.InitNodes(moudle.nRootNode, this);
		}
		#endregion
	}
}
