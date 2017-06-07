namespace NetSCADA6.Common.NSColorManger
{
    partial class BaseGradientUserControl
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
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // BaseGradientUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Name = "BaseGradientUserControl";
            this.Size = new System.Drawing.Size(235, 35);
            this.toolTip1.SetToolTip(this, "1.添加:单击色条 2.删除:选中色笔向下拖拉 3.移动:选中色笔后左右移动");
            this.Load += new System.EventHandler(this.BaseGradientUserControl_Load);
            this.SizeChanged += new System.EventHandler(this.BaseGradientUserControl_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.BaseGradientUserControl_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BaseGradientUserControl_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BaseGradientUserControl_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BaseGradientUserControl_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;


    }
}
