
namespace NetSCADA6.Common.NSColorManger
{
    partial class RGBUserControl
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
            this.textBox1 = new NetSCADA6.Common.NSColorManger.TextBoxEx();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.IsNegativeNumber = false;
            this.textBox1.Location = new System.Drawing.Point(9, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(41, 14);
            this.textBox1.TabIndex = 0;
            this.textBox1.ValType = NetSCADA6.Common.NSColorManger.ENUM_TYPE.T_NUM;
            this.textBox1.Visible = false;
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // RGBUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox1);
            this.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.DoubleBuffered = true;
            this.Name = "RGBUserControl";
            this.Size = new System.Drawing.Size(88, 24);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UserControl1_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.UserControl1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UserControl1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.UserControl1_MouseUp);
            this.Resize += new System.EventHandler(this.RGBUserControl_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBoxEx textBox1;


    }
}
