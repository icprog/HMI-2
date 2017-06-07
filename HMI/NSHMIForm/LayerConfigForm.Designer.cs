namespace NetSCADA6.HMI.NSHMIForm
{
	partial class LayerConfigForm
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
			XPTable.Models.Row row1 = new XPTable.Models.Row();
			XPTable.Models.Cell cell1 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell2 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell3 = new XPTable.Models.Cell();
			XPTable.Models.Row row2 = new XPTable.Models.Row();
			XPTable.Models.Cell cell4 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell5 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell6 = new XPTable.Models.Cell();
			XPTable.Models.Row row3 = new XPTable.Models.Row();
			XPTable.Models.Cell cell7 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell8 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell9 = new XPTable.Models.Cell();
			XPTable.Models.Row row4 = new XPTable.Models.Row();
			XPTable.Models.Cell cell10 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell11 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell12 = new XPTable.Models.Cell();
			XPTable.Models.Row row5 = new XPTable.Models.Row();
			XPTable.Models.Cell cell13 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell14 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell15 = new XPTable.Models.Cell();
			XPTable.Models.Row row6 = new XPTable.Models.Row();
			XPTable.Models.Cell cell16 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell17 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell18 = new XPTable.Models.Cell();
			XPTable.Models.Row row7 = new XPTable.Models.Row();
			XPTable.Models.Cell cell19 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell20 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell21 = new XPTable.Models.Cell();
			XPTable.Models.Row row8 = new XPTable.Models.Row();
			XPTable.Models.Cell cell22 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell23 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell24 = new XPTable.Models.Cell();
			XPTable.Models.Row row9 = new XPTable.Models.Row();
			XPTable.Models.Cell cell25 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell26 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell27 = new XPTable.Models.Cell();
			XPTable.Models.Row row10 = new XPTable.Models.Row();
			XPTable.Models.Cell cell28 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell29 = new XPTable.Models.Cell();
			XPTable.Models.Cell cell30 = new XPTable.Models.Cell();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.tableLayer = new XPTable.Models.Table();
			this.columnModelLayer = new XPTable.Models.ColumnModel();
			this.textName = new XPTable.Models.TextColumn();
			this.checkBoxVisible = new XPTable.Models.CheckBoxColumn();
			this.checkBoxLocked = new XPTable.Models.CheckBoxColumn();
			this.tableModelLayer = new XPTable.Models.TableModel();
			((System.ComponentModel.ISupportInitialize)(this.tableLayer)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(10, 242);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "确定";
			this.buttonOK.UseVisualStyleBackColor = true;
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(96, 242);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "取消";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// tableLayer
			// 
			this.tableLayer.ColumnModel = this.columnModelLayer;
			this.tableLayer.ColumnResizing = false;
			this.tableLayer.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayer.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.tableLayer.HeaderFont = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.tableLayer.Location = new System.Drawing.Point(0, 0);
			this.tableLayer.Name = "tableLayer";
			this.tableLayer.Size = new System.Drawing.Size(181, 232);
			this.tableLayer.TabIndex = 0;
			this.tableLayer.TableModel = this.tableModelLayer;
			// 
			// columnModelLayer
			// 
			this.columnModelLayer.Columns.AddRange(new XPTable.Models.Column[] {
            this.textName,
            this.checkBoxVisible,
            this.checkBoxLocked});
			// 
			// textName
			// 
			this.textName.Editable = false;
			this.textName.Selectable = false;
			this.textName.Sortable = false;
			this.textName.Width = 90;
			// 
			// checkBoxVisible
			// 
			this.checkBoxVisible.Selectable = false;
			this.checkBoxVisible.Sortable = false;
			this.checkBoxVisible.Text = "可见";
			this.checkBoxVisible.Width = 40;
			// 
			// checkBoxLocked
			// 
			this.checkBoxLocked.Selectable = false;
			this.checkBoxLocked.Sortable = false;
			this.checkBoxLocked.Text = "锁定";
			this.checkBoxLocked.Width = 40;
			// 
			// tableModelLayer
			// 
			this.tableModelLayer.RowHeight = 20;
			cell1.Text = "第0层";
			row1.Cells.AddRange(new XPTable.Models.Cell[] {
            cell1,
            cell2,
            cell3});
			cell4.Text = "第1层";
			cell6.Text = "";
			row2.Cells.AddRange(new XPTable.Models.Cell[] {
            cell4,
            cell5,
            cell6});
			cell7.Text = "第2层";
			cell9.Text = "";
			row3.Cells.AddRange(new XPTable.Models.Cell[] {
            cell7,
            cell8,
            cell9});
			cell10.Text = "第3层";
			row4.Cells.AddRange(new XPTable.Models.Cell[] {
            cell10,
            cell11,
            cell12});
			cell13.Text = "第4层";
			row5.Cells.AddRange(new XPTable.Models.Cell[] {
            cell13,
            cell14,
            cell15});
			cell16.Text = "第5层";
			cell18.Text = "";
			row6.Cells.AddRange(new XPTable.Models.Cell[] {
            cell16,
            cell17,
            cell18});
			cell19.Text = "第6层";
			row7.Cells.AddRange(new XPTable.Models.Cell[] {
            cell19,
            cell20,
            cell21});
			cell22.Text = "第7层";
			row8.Cells.AddRange(new XPTable.Models.Cell[] {
            cell22,
            cell23,
            cell24});
			cell25.Text = "第8层";
			row9.Cells.AddRange(new XPTable.Models.Cell[] {
            cell25,
            cell26,
            cell27});
			cell28.Text = "第9层";
			row10.Cells.AddRange(new XPTable.Models.Cell[] {
            cell28,
            cell29,
            cell30});
			this.tableModelLayer.Rows.AddRange(new XPTable.Models.Row[] {
            row1,
            row2,
            row3,
            row4,
            row5,
            row6,
            row7,
            row8,
            row9,
            row10});
			// 
			// LayerConfigForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(181, 278);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.tableLayer);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "LayerConfigForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "图层配置";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LayerConfigForm_FormClosing);
			this.Load += new System.EventHandler(this.LayerConfigForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.tableLayer)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private XPTable.Models.Table tableLayer;
		private XPTable.Models.ColumnModel columnModelLayer;
		private XPTable.Models.TableModel tableModelLayer;
		private XPTable.Models.TextColumn textName;
		private XPTable.Models.CheckBoxColumn checkBoxVisible;
		private XPTable.Models.CheckBoxColumn checkBoxLocked;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;

	}
}