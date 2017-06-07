using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetSCADA6.NSStudio.NSDockPanel;

namespace NetSCADA6.HMI.NSHMIToolForm
{
	public partial class ToolboxForm : NSDockContent
	{
		public ToolboxForm()
		{
			InitializeComponent();
		}

		#region public function
		public void AddTab(string tabName, Type[] itemTypes)
		{
			ToolBoxControl.AddTab(tabName, itemTypes);
		}
		public void Initialize()
		{
			ToolBoxControl.InitializeToolbox();
		}
		#endregion
	}
}
