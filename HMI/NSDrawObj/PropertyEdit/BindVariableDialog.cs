using System.Collections.Generic;
using System.Windows.Forms;
using NetSCADA6.NSInterface.HMI.Var;

namespace NetSCADA6.HMI.NSDrawObj.PropertyEdit
{
	/// <summary>
	/// 变量绑定设置对话框
	/// </summary>
	public partial class BindVariableDialog : Form
	{
		public BindVariableDialog()
		{
			InitializeComponent();
		}
		public BindVariableDialog(object value)
		{
			InitializeComponent();

			if (value is List<IPropertyExpression>)
				_parameterList = (List<IPropertyExpression>)value;

			Init();
		}

		#region field
		private List<IPropertyExpression> _parameterList { set; get; }
		#endregion

		#region private function
		private void Init()
		{
			if (_parameterList == null)
				return;

			int count = _parameterList.Count;
			for (int i = 0; i < count; i++)
			{
				Grid.Rows.Add(_parameterList[i].PropertyName, _parameterList[i].Expression);
			}
		}
		private void Save()
		{
			int count = Grid.Rows.Count;
			for (int i = 0; i < count; i++)
			{
				object value = Grid.Rows[i].Cells[1].Value;
				if (value != null)
					_parameterList[i].Expression = value.ToString().Trim();
				else
					_parameterList[i].Expression = null;
			}
		}
		#endregion

		private void BindVariableDialog_FormClosed(object sender, FormClosedEventArgs e)
		{
			Save();
		}
	}
}
