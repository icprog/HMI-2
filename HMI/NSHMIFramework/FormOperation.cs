using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using NetSCADA6.HMI.NSHMIForm;
using NetSCADA6.NSInterface.HMI.Form;
using WeifenLuo.WinFormsUI.Docking;

namespace NetSCADA6.HMI.NSHMIFramework
{
    /// <summary>
    /// 窗体管理类
    /// </summary>
	public class FormOperation
	{
		public FormOperation(HMIFramework framework)
		{
			Debug.Assert(framework != null);
			_framework = framework;
		}
		
		#region field
    	private readonly HMIFramework _framework;
		#endregion

		#region property
		private readonly List<IHMIForm> _openedList = new List<IHMIForm>();
        /// <summary>
        /// 打开的窗体列表
        /// </summary>
        public List<IHMIForm> OpenedList
        {
            get { return _openedList; }
        }
        #endregion

		#region private function
		private void FormClosed(object sender, FormClosedEventArgs e)
		{
			if (sender is IHMIForm)
				OpenedList.Remove(sender as IHMIForm);
		}
		private void Initialization(IHMIForm f,  string fullName)
		{
			f.FullName = fullName;
			_openedList.Add(f);
			((Form)f).FormClosed += FormClosed;
			f.Common.Initialization();
		}
		private void Show(IHMIForm form)
		{
			if (_framework.IsStudioMode)
				((DockContent)form).Show(((FrameworkManager)_framework.Manager).MainPanel);
			else
				((Form)form).Show();
		}
		private IHMIForm FindOpened(string fullName)
		{
			return OpenedList.FirstOrDefault(form => string.Compare(form.FullName, fullName, true) == 0);
		}
		private static void ChangeText(IHMIForm f, string newFullName)
		{
			string name = Path.GetFileNameWithoutExtension(f.FullName);
			//Text和文件名保持一致，才改名
			if (string.Compare(name, ((Form)f).Text, true) == 0)
				((Form) f).Text = Path.GetFileNameWithoutExtension(newFullName);
		}
    	#endregion

		#region public function
		public IHMIForm Create(string fullName)
        {
			HMIForm f = new HMIForm(_framework) {Text = Path.GetFileNameWithoutExtension(fullName)};
			Initialization(f, fullName);
			Show(f);
			f.Save();
            return f;
        }
        public bool Save(IHMIForm form)
        {
            form.Save();
        	return true;
        }
        public void SaveAll()
        {
            foreach (IHMIForm form in OpenedList)
            {
                Save(form);
            }
        }
        public IHMIForm Open(string fullName)
        {
        	IHMIForm f = FindOpened(fullName);
			if (f != null)
			{
				Show(f);
				return f;
			}

            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                using (Stream s = File.Open(fullName, FileMode.Open))
                {
					f = new HMIForm(_framework);
					(f as HMIForm).Deserialize(bf, s);
					Initialization(f, fullName);
                }
            }
			catch (Exception e)
			{
			    MessageBox.Show(e.Message);
			    return null;
			}

			Show(f);
			return f;
        }
        public bool IsOpened(string fullName)
        {
            return FindOpened(fullName) != null;
        }
		public bool Close(string fullName)
		{
			IHMIForm f = FindOpened(fullName);
			if (f != null)
			{
				((Form)f).Close();
				return true;
			}

			return false;
		}
		public bool CloseAll()
		{
			int count = OpenedList.Count;
			for (int i = 0; i < count; i++ )
				((Form)OpenedList[count - i - 1]).Close();

			return true;
		}
		public bool Rename(string newFullName, string oldFullName)
		{
			IHMIForm f = FindOpened(oldFullName);
			if (f != null)
			{
				ChangeText(f, newFullName);
				f.FullName = newFullName;
				return true;
			}

			return false;
		}
        #endregion
    }
}
