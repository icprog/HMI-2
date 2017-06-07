using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using System.ComponentModel;

namespace TreeViewColumnsProject
{
	public partial class TreeViewColumns : UserControl
	{
		public TreeViewColumns()
		{
			InitializeComponent();

			this.BackColor = VisualStyleInformation.TextControlBorder;
			this.Padding = new Padding(1);
		}


		[Description("TreeView associated with the control"), Category("Behavior")]
		public TreeView TreeView
		{
			get
			{
				return this.treeView1;
			}
		}

		[Description("Columns associated with the control"), Category("Behavior")]
		public ListView.ColumnHeaderCollection Columns
		{
			get
			{
				return this.listView1.Columns;
			}
		}

		private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			this.treeView1.Focus();
		}

		private void treeView1_Click(object sender, EventArgs e)
		{
			Point p = this.treeView1.PointToClient(Control.MousePosition);
			TreeNode tn = this.treeView1.GetNodeAt(p);
			if (tn != null)
				this.treeView1.SelectedNode = tn;
		}

		private void listView1_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
		{
			this.treeView1.Focus();
			this.treeView1.Invalidate();
		}

		private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
		{
			//this.treeView1.Focus();
			//this.treeView1.Invalidate();
		}

		private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			e.DrawDefault = true;

			Rectangle rect = e.Bounds;

			if ((e.State & TreeNodeStates.Selected) != 0)
			{
				if ((e.State & TreeNodeStates.Focused) != 0)
					e.Graphics.FillRectangle(SystemBrushes.Highlight, rect);
				else
					e.Graphics.FillRectangle(SystemBrushes.Control, rect);
			}
			else
				e.Graphics.FillRectangle(Brushes.White, rect);

			//e.Graphics.DrawRectangle(SystemPens.Control, rect);

			for (int intColumn = 1; intColumn < this.listView1.Columns.Count; intColumn++)
			{
				rect.Offset(this.listView1.Columns[intColumn - 1].Width, 0);
				rect.Width = this.listView1.Columns[intColumn].Width;

				//e.Graphics.DrawRectangle(SystemPens.Control, rect);

				//if (intColumn == 1)
				//    e.Graphics.DrawImage(Resource.bbb, rect.Location);
				//else
				//{
				//    e.Graphics.DrawImage(Resource.aaa, rect.Location);
				//}
			}
		}

		private void treeView1_SizeChanged(object sender, EventArgs e)
		{
			int width = treeView1.Width;
			Columns[0].Width = width - 100;
		}

	}
}
