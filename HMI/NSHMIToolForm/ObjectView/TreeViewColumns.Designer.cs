using MyTreeview;

namespace TreeViewColumnsProject
{
	partial class TreeViewColumns
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node3");
			System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node4");
			System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node5");
			System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.treeView1 = new MyTreeview.MultiSelectTreeView();
			this.SuspendLayout();
			// 
			// listView1
			// 
			this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.listView1.Dock = System.Windows.Forms.DockStyle.Top;
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.Location = new System.Drawing.Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Scrollable = false;
			this.listView1.Size = new System.Drawing.Size(438, 18);
			this.listView1.TabIndex = 3;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.Visible = false;
			this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
			this.listView1.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.listView1_ColumnWidthChanged);
			this.listView1.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.listView1_ColumnWidthChanging);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "ColumnHeader 1";
			this.columnHeader1.Width = 130;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "ColumnHeader 2";
			this.columnHeader2.Width = 50;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "ColumnHeader3";
			this.columnHeader3.Width = 50;
			// 
			// treeView1
			// 
			this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
			this.treeView1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.treeView1.HideSelection = false;
			this.treeView1.Location = new System.Drawing.Point(0, 18);
			this.treeView1.Name = "treeView1";
			treeNode1.Name = "Node3";
			treeNode1.Text = "Node3";
			treeNode2.Name = "Node4";
			treeNode2.Text = "Node4";
			treeNode3.Name = "Node5";
			treeNode3.Text = "Node5";
			treeNode4.Name = "Node0";
			treeNode4.Text = "Node0";
			this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4});
			this.treeView1.Size = new System.Drawing.Size(438, 152);
			this.treeView1.TabIndex = 2;
			this.treeView1.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeView1_DrawNode);
			this.treeView1.SizeChanged += new System.EventHandler(this.treeView1_SizeChanged);
			this.treeView1.Click += new System.EventHandler(this.treeView1_Click);
			// 
			// TreeViewColumns
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.treeView1);
			this.Controls.Add(this.listView1);
			this.Name = "TreeViewColumns";
			this.Size = new System.Drawing.Size(438, 170);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private MultiSelectTreeView treeView1;
		private System.Windows.Forms.ColumnHeader columnHeader3;
	}
}
