namespace NetSCADA6.HMI.NSDrawObj.PropertyEdit
{
	partial class BindVariableDialog
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Grid = new System.Windows.Forms.DataGridView();
			this.buttonReturn = new System.Windows.Forms.Button();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
			this.SuspendLayout();
			// 
			// BindVariableGrid
			// 
			this.Grid.AllowUserToAddRows = false;
			this.Grid.AllowUserToDeleteRows = false;
			this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.Grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
			this.Grid.Dock = System.Windows.Forms.DockStyle.Top;
			this.Grid.Location = new System.Drawing.Point(0, 0);
			this.Grid.Name = "BindVariableGrid";
			this.Grid.RowHeadersVisible = false;
			this.Grid.RowTemplate.Height = 23;
			this.Grid.Size = new System.Drawing.Size(330, 299);
			this.Grid.TabIndex = 0;
			// 
			// buttonReturn
			// 
			this.buttonReturn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonReturn.Location = new System.Drawing.Point(245, 305);
			this.buttonReturn.Name = "buttonReturn";
			this.buttonReturn.Size = new System.Drawing.Size(75, 23);
			this.buttonReturn.TabIndex = 1;
			this.buttonReturn.Text = "返回";
			this.buttonReturn.UseVisualStyleBackColor = true;
			// 
			// Column1
			// 
			this.Column1.HeaderText = "属性名称";
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Column1.Width = 200;
			// 
			// Column2
			// 
			this.Column2.HeaderText = "绑定变量";
			this.Column2.Name = "Column2";
			this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// BindVariableDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(330, 334);
			this.Controls.Add(this.buttonReturn);
			this.Controls.Add(this.Grid);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BindVariableDialog";
			this.Text = "变量绑定对话框";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BindVariableDialog_FormClosed);
			((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView Grid;
		private System.Windows.Forms.Button buttonReturn;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column2;

	}
}