namespace NetSCADA6.Common.NSColorManger
{
    partial class SolidUserControl
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
            this.BaseColorControl1 = new NetSCADA6.Common.NSColorManger.BaseColorControl();
            this.PathGradientControl1 = new NetSCADA6.Common.NSColorManger.PathGradientControl();
            this.RGBUserControl_A = new NetSCADA6.Common.NSColorManger.RGBUserControl();
            this.RGBUserControl_B = new NetSCADA6.Common.NSColorManger.RGBUserControl();
            this.RGBUserControl_G = new NetSCADA6.Common.NSColorManger.RGBUserControl();
            this.RGBUserControl_R = new NetSCADA6.Common.NSColorManger.RGBUserControl();
            this.SuspendLayout();
            // 
            // BaseColorControl1
            // 
            this.BaseColorControl1.AutoScroll = true;
            this.BaseColorControl1.CurrentSelectColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BaseColorControl1.CurrentSelectHeight = 0;
            this.BaseColorControl1.Location = new System.Drawing.Point(136, 1);
            this.BaseColorControl1.Name = "BaseColorControl1";
            this.BaseColorControl1.Size = new System.Drawing.Size(30, 142);
            this.BaseColorControl1.TabIndex = 0;
            // 
            // PathGradientControl1
            // 
            this.PathGradientControl1.BaseColor = System.Drawing.Color.Red;
            this.PathGradientControl1.color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.PathGradientControl1.ColorLocation = new System.Drawing.Point(0, 0);
            this.PathGradientControl1.Location = new System.Drawing.Point(1, 1);
            this.PathGradientControl1.Name = "PathGradientControl1";
            this.PathGradientControl1.Size = new System.Drawing.Size(135, 142);
            this.PathGradientControl1.TabIndex = 2;
            // 
            // RGBUserControl_A
            // 
            this.RGBUserControl_A.Color = System.Drawing.Color.Black;
            this.RGBUserControl_A.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.RGBUserControl_A.Location = new System.Drawing.Point(173, 82);
            this.RGBUserControl_A.Name = "RGBUserControl_A";
            this.RGBUserControl_A.Percentage = 0F;
            this.RGBUserControl_A.Size = new System.Drawing.Size(110, 26);
            this.RGBUserControl_A.Suffix = "";
            this.RGBUserControl_A.TabIndex = 0;
            this.RGBUserControl_A.Value = 0;
            // 
            // RGBUserControl_B
            // 
            this.RGBUserControl_B.Color = System.Drawing.Color.Black;
            this.RGBUserControl_B.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.RGBUserControl_B.Location = new System.Drawing.Point(173, 55);
            this.RGBUserControl_B.Name = "RGBUserControl_B";
            this.RGBUserControl_B.Percentage = 0F;
            this.RGBUserControl_B.Size = new System.Drawing.Size(110, 26);
            this.RGBUserControl_B.Suffix = "";
            this.RGBUserControl_B.TabIndex = 0;
            this.RGBUserControl_B.Value = 0;
            // 
            // RGBUserControl_G
            // 
            this.RGBUserControl_G.Color = System.Drawing.Color.Black;
            this.RGBUserControl_G.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.RGBUserControl_G.Location = new System.Drawing.Point(173, 28);
            this.RGBUserControl_G.Name = "RGBUserControl_G";
            this.RGBUserControl_G.Percentage = 0F;
            this.RGBUserControl_G.Size = new System.Drawing.Size(110, 26);
            this.RGBUserControl_G.Suffix = "";
            this.RGBUserControl_G.TabIndex = 0;
            this.RGBUserControl_G.Value = 0;
            // 
            // RGBUserControl_R
            // 
            this.RGBUserControl_R.Color = System.Drawing.Color.Black;
            this.RGBUserControl_R.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.RGBUserControl_R.Location = new System.Drawing.Point(173, 1);
            this.RGBUserControl_R.Name = "RGBUserControl_R";
            this.RGBUserControl_R.Percentage = 0F;
            this.RGBUserControl_R.Size = new System.Drawing.Size(110, 26);
            this.RGBUserControl_R.Suffix = "";
            this.RGBUserControl_R.TabIndex = 0;
            this.RGBUserControl_R.Value = 0;
            // 
            // SolidUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BaseColorControl1);
            this.Controls.Add(this.PathGradientControl1);
            this.Controls.Add(this.RGBUserControl_A);
            this.Controls.Add(this.RGBUserControl_B);
            this.Controls.Add(this.RGBUserControl_G);
            this.Controls.Add(this.RGBUserControl_R);
            this.Name = "SolidUserControl";
            this.Size = new System.Drawing.Size(288, 146);
            this.ResumeLayout(false);

        }

        #endregion

        private RGBUserControl RGBUserControl_R;
        private RGBUserControl RGBUserControl_G;
        private RGBUserControl RGBUserControl_B;
        private RGBUserControl RGBUserControl_A;
        private PathGradientControl PathGradientControl1;
        private BaseColorControl BaseColorControl1;

    }
}
