namespace NetSCADA6.Common.NSColorManger
{
    partial class LinearGradientUserControl
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
            NetSCADA6.Common.NSColorManger.ColorBlendEx colorBlendEx1 = new NetSCADA6.Common.NSColorManger.ColorBlendEx();
            this.HScrollBarUserControl1 = new NetSCADA6.Common.NSColorManger.HScrollBarUserControl();
            this.BaseGradientUserControl1 = new NetSCADA6.Common.NSColorManger.BaseGradientUserControl();
            this.SolidBrushUserControl1 = new NetSCADA6.Common.NSColorManger.SolidUserControl();
            this.SuspendLayout();
            // 
            // HScrollBarUserControl1
            // 
            this.HScrollBarUserControl1.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.HScrollBarUserControl1.Location = new System.Drawing.Point(3, 195);
            this.HScrollBarUserControl1.Max = 360;
            this.HScrollBarUserControl1.Min = 0;
            this.HScrollBarUserControl1.Name = "HScrollBarUserControl1";
            this.HScrollBarUserControl1.Prefix = "角度：";
            this.HScrollBarUserControl1.Size = new System.Drawing.Size(285, 27);
            this.HScrollBarUserControl1.Suffix = "°";
            this.HScrollBarUserControl1.TabIndex = 2;
            this.HScrollBarUserControl1.Value = 180F;
            // 
            // BaseGradientUserControl1
            // 
            colorBlendEx1.SelectIndex = -1;
            this.BaseGradientUserControl1.ColorBlendEx = colorBlendEx1;
            this.BaseGradientUserControl1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BaseGradientUserControl1.Location = new System.Drawing.Point(-1, 148);
            this.BaseGradientUserControl1.Name = "BaseGradientUserControl1";
            this.BaseGradientUserControl1.Size = new System.Drawing.Size(292, 41);
            this.BaseGradientUserControl1.TabIndex = 1;
            // 
            // SolidBrushUserControl1
            // 
            this.SolidBrushUserControl1.color = System.Drawing.Color.Brown;
            this.SolidBrushUserControl1.Location = new System.Drawing.Point(0, 0);
            this.SolidBrushUserControl1.Name = "SolidBrushUserControl1";
            this.SolidBrushUserControl1.Size = new System.Drawing.Size(288, 142);
            this.SolidBrushUserControl1.TabIndex = 0;
            // 
            // LinearGradientUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.HScrollBarUserControl1);
            this.Controls.Add(this.BaseGradientUserControl1);
            this.Controls.Add(this.SolidBrushUserControl1);
            this.DoubleBuffered = true;
            this.Name = "LinearGradientUserControl";
            this.Size = new System.Drawing.Size(291, 227);
            this.ResumeLayout(false);

        }

        #endregion

        private SolidUserControl SolidBrushUserControl1;
        private BaseGradientUserControl BaseGradientUserControl1;
        private HScrollBarUserControl HScrollBarUserControl1;
    }
}
