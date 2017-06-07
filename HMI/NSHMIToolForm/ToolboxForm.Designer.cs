namespace NetSCADA6.HMI.NSHMIToolForm
{
	partial class ToolboxForm
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
			this.ToolBoxControl = new Toolbox.Toolbox();
			this.SuspendLayout();
			// 
			// ToolBoxControl
			// 
			this.ToolBoxControl.DesignerHost = null;
			this.ToolBoxControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ToolBoxControl.FilePath = null;
			this.ToolBoxControl.Location = new System.Drawing.Point(0, 0);
			this.ToolBoxControl.Name = "ToolBoxControl";
			this.ToolBoxControl.SelectedCategory = null;
			this.ToolBoxControl.Size = new System.Drawing.Size(254, 314);
			this.ToolBoxControl.TabIndex = 0;
			// 
			// ToolboxForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(254, 314);
			this.Controls.Add(this.ToolBoxControl);
			this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.HideOnClose = true;
			this.Name = "ToolboxForm";
			this.Text = "工具箱";
			this.ResumeLayout(false);

		}

		#endregion

		public Toolbox.Toolbox ToolBoxControl;
	}
}