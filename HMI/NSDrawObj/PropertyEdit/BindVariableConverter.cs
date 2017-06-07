using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace NetSCADA6.HMI.NSDrawObj.PropertyEdit
{
	/// <summary>
	/// 变量绑定属性编辑器
	/// </summary>
	public class BindVariableEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override object EditValue(ITypeDescriptorContext context, System.
			IServiceProvider provider, object value)
		{
			IWindowsFormsEditorService edSvc = 
				(IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

			if (edSvc != null)
			{
				BindVariableDialog bindDialog = new BindVariableDialog(value);
				bindDialog.ShowDialog();
			}

			return value;
		}
	}

}
