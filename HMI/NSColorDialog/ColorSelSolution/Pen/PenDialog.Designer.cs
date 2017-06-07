namespace NetSCADA6.Common.NSColorManger
{
    partial class PenDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbb_dashstyle = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbb_startcap = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbb_endcap = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbb_linejoin = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbb_width = new System.Windows.Forms.ComboBox();
            this.cbb_dashcap = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new NetSCADA6.Common.NSColorManger.TextBoxEx();
            this.UserControl1 = new NetSCADA6.Common.NSColorManger.BrushUserControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.HScrollBarUserControl_Alpha = new NetSCADA6.Common.NSColorManger.HScrollBarUserControl();
            this.buttonEx_HighlightColor = new NetSCADA6.Common.NSColorManger.ButtonEx();
            this.buttonEx_BaseColor = new NetSCADA6.Common.NSColorManger.ButtonEx();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(615, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "示例";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(616, 107);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(157, 157);
            this.panel1.TabIndex = 8;
            this.panel1.Visible = false;
            // 
            // button_cancel
            // 
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.Location = new System.Drawing.Point(698, 351);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 7;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            // 
            // button_ok
            // 
            this.button_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_ok.Location = new System.Drawing.Point(617, 351);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 6;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "线形";
            // 
            // cbb_dashstyle
            // 
            this.cbb_dashstyle.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbb_dashstyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_dashstyle.FormattingEnabled = true;
            this.cbb_dashstyle.Location = new System.Drawing.Point(8, 35);
            this.cbb_dashstyle.Name = "cbb_dashstyle";
            this.cbb_dashstyle.Size = new System.Drawing.Size(119, 22);
            this.cbb_dashstyle.TabIndex = 1;
            this.cbb_dashstyle.Tag = "1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(131, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "粗细";
            // 
            // cbb_startcap
            // 
            this.cbb_startcap.FormattingEnabled = true;
            this.cbb_startcap.Location = new System.Drawing.Point(258, 37);
            this.cbb_startcap.Name = "cbb_startcap";
            this.cbb_startcap.Size = new System.Drawing.Size(119, 20);
            this.cbb_startcap.TabIndex = 10;
            this.cbb_startcap.Tag = "3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(256, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "起点";
            // 
            // cbb_endcap
            // 
            this.cbb_endcap.FormattingEnabled = true;
            this.cbb_endcap.Location = new System.Drawing.Point(383, 37);
            this.cbb_endcap.Name = "cbb_endcap";
            this.cbb_endcap.Size = new System.Drawing.Size(119, 20);
            this.cbb_endcap.TabIndex = 12;
            this.cbb_endcap.Tag = "4";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(381, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "末点";
            // 
            // cbb_linejoin
            // 
            this.cbb_linejoin.FormattingEnabled = true;
            this.cbb_linejoin.Location = new System.Drawing.Point(508, 37);
            this.cbb_linejoin.Name = "cbb_linejoin";
            this.cbb_linejoin.Size = new System.Drawing.Size(119, 20);
            this.cbb_linejoin.TabIndex = 14;
            this.cbb_linejoin.Tag = "5";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(506, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "连接";
            // 
            // cbb_width
            // 
            this.cbb_width.FormattingEnabled = true;
            this.cbb_width.Location = new System.Drawing.Point(133, 37);
            this.cbb_width.Name = "cbb_width";
            this.cbb_width.Size = new System.Drawing.Size(82, 20);
            this.cbb_width.TabIndex = 1;
            this.cbb_width.Tag = "2";
            // 
            // cbb_dashcap
            // 
            this.cbb_dashcap.FormattingEnabled = true;
            this.cbb_dashcap.Location = new System.Drawing.Point(633, 37);
            this.cbb_dashcap.Name = "cbb_dashcap";
            this.cbb_dashcap.Size = new System.Drawing.Size(119, 20);
            this.cbb_dashcap.TabIndex = 14;
            this.cbb_dashcap.Tag = "6";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(631, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "虚线";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbb_dashcap);
            this.groupBox1.Controls.Add(this.cbb_dashstyle);
            this.groupBox1.Controls.Add(this.cbb_linejoin);
            this.groupBox1.Controls.Add(this.cbb_width);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cbb_endcap);
            this.groupBox1.Controls.Add(this.cbb_startcap);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(761, 69);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "画笔";
            // 
            // textBox1
            // 
            this.textBox1.IsNegativeNumber = false;
            this.textBox1.Location = new System.Drawing.Point(221, 36);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(31, 21);
            this.textBox1.TabIndex = 15;
            this.textBox1.ValType = NetSCADA6.Common.NSColorManger.ENUM_TYPE.T_NUM;
            // 
            // UserControl1
            // 
            this.UserControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.UserControl1.EanbleImageDrawMode = false;
            this.UserControl1.Location = new System.Drawing.Point(6, 6);
            this.UserControl1.Name = "UserControl1";
            this.UserControl1.Size = new System.Drawing.Size(569, 254);
            this.UserControl1.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 87);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(597, 291);
            this.tabControl1.TabIndex = 12;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.UserControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(589, 265);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "线";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.HScrollBarUserControl_Alpha);
            this.tabPage2.Controls.Add(this.buttonEx_HighlightColor);
            this.tabPage2.Controls.Add(this.buttonEx_BaseColor);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(589, 265);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "管道";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 12;
            this.label8.Text = "透明";
            // 
            // HScrollBarUserControl_Alpha
            // 
            this.HScrollBarUserControl_Alpha.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.HScrollBarUserControl_Alpha.Location = new System.Drawing.Point(49, 77);
            this.HScrollBarUserControl_Alpha.Max = 255;
            this.HScrollBarUserControl_Alpha.Min = 0;
            this.HScrollBarUserControl_Alpha.Name = "HScrollBarUserControl_Alpha";
            this.HScrollBarUserControl_Alpha.Prefix = "";
            this.HScrollBarUserControl_Alpha.Size = new System.Drawing.Size(139, 23);
            this.HScrollBarUserControl_Alpha.Suffix = "";
            this.HScrollBarUserControl_Alpha.TabIndex = 9;
            this.HScrollBarUserControl_Alpha.Value = 255F;
            // 
            // buttonEx_HighlightColor
            // 
            this.buttonEx_HighlightColor.BackColor = System.Drawing.Color.White;
            this.buttonEx_HighlightColor.BaseColor = System.Drawing.Color.LightGray;
            this.buttonEx_HighlightColor.Color = System.Drawing.Color.White;
            this.buttonEx_HighlightColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonEx_HighlightColor.Location = new System.Drawing.Point(49, 48);
            this.buttonEx_HighlightColor.Name = "buttonEx_HighlightColor";
            this.buttonEx_HighlightColor.RoundStyle = NetSCADA6.Common.NSColorManger.RoundStyle.None;
            this.buttonEx_HighlightColor.Size = new System.Drawing.Size(107, 23);
            this.buttonEx_HighlightColor.TabIndex = 11;
            this.buttonEx_HighlightColor.Text = "高亮色";
            this.buttonEx_HighlightColor.UseVisualStyleBackColor = false;
            this.buttonEx_HighlightColor.Click += new System.EventHandler(this.buttonEx_HighlightColor_Click);
            // 
            // buttonEx_BaseColor
            // 
            this.buttonEx_BaseColor.BackColor = System.Drawing.Color.White;
            this.buttonEx_BaseColor.BaseColor = System.Drawing.Color.LightGray;
            this.buttonEx_BaseColor.Color = System.Drawing.Color.Black;
            this.buttonEx_BaseColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonEx_BaseColor.Location = new System.Drawing.Point(49, 19);
            this.buttonEx_BaseColor.Name = "buttonEx_BaseColor";
            this.buttonEx_BaseColor.RoundStyle = NetSCADA6.Common.NSColorManger.RoundStyle.None;
            this.buttonEx_BaseColor.Size = new System.Drawing.Size(107, 23);
            this.buttonEx_BaseColor.TabIndex = 10;
            this.buttonEx_BaseColor.Text = "基础色";
            this.buttonEx_BaseColor.UseVisualStyleBackColor = false;
            this.buttonEx_BaseColor.Click += new System.EventHandler(this.buttonEx_BaseColor_Click);
            // 
            // PenDialog
            // 
            this.AcceptButton = this.button_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_cancel;
            this.ClientSize = new System.Drawing.Size(788, 387);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(804, 425);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(804, 425);
            this.Name = "PenDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "线条";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private NetSCADA6.Common.NSColorManger.BrushUserControl UserControl1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbb_dashstyle;
        private TextBoxEx textBox1;
        private System.Windows.Forms.ComboBox cbb_dashcap;
        private System.Windows.Forms.ComboBox cbb_linejoin;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbb_endcap;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbb_startcap;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbb_width;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private HScrollBarUserControl HScrollBarUserControl_Alpha;
        private ButtonEx buttonEx_HighlightColor;
        private ButtonEx buttonEx_BaseColor;
        private System.Windows.Forms.Label label8;
    }
}