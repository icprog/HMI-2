namespace NetSCADA6.Common.NSColorManger
{
    partial class PathGradientControl
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
            this.SuspendLayout();
            // 
            // PathGradientControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Name = "PathGradientControl";
            this.Size = new System.Drawing.Size(109, 110);
            this.SizeChanged += new System.EventHandler(this.PathGradientControl_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UserControl2_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.UserControl2_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UserControl2_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.UserControl2_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

    }
}
