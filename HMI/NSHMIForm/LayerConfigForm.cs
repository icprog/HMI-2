using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

using NetSCADA6.NSInterface.HMI.Form;

namespace NetSCADA6.HMI.NSHMIForm
{
	public partial class LayerConfigForm : Form
	{
		public LayerConfigForm(IHMIForm data)
		{
			InitializeComponent();

			Debug.Assert(data != null);

			_data = data;
		}

		/// <summary>
		/// 需要编辑的数据
		/// </summary>
		private IHMIForm _data;

		private void LayerConfigForm_Load(object sender, System.EventArgs e)
		{
			BitArray visibles = _data.Common.VisibleLayers;
			BitArray lockeds = _data.Common.LockedLayers;

			for (int i = 0; i < visibles.Count; i++)
				tableModelLayer.Rows[i].Cells[1].Checked = visibles[i];
			for (int i = 0; i < lockeds.Count; i++)
				tableModelLayer.Rows[i].Cells[2].Checked = lockeds[i];
		}
		private void LayerConfigForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (DialogResult == DialogResult.OK)
			{
				BitArray visibles = _data.Common.VisibleLayers;
				BitArray lockeds = _data.Common.LockedLayers;

				for (int i = 0; i < visibles.Count; i++)
					visibles[i] = tableModelLayer.Rows[i].Cells[1].Checked;
				for (int i = 0; i < lockeds.Count; i++)
					lockeds[i] = tableModelLayer.Rows[i].Cells[2].Checked;
			}
		}



	}
}
