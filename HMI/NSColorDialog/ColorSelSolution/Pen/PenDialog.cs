using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace NetSCADA6.Common.NSColorManger
{
    public partial class PenDialog : Form
    {
        public PenDialog(PenData pd)
        {
            InitializeComponent();
            InitCmb();
            penData = (PenData)pd.Clone();
            UserControl1.brushData = penData._brushData;
            UserControl1.BrushChanged += BrushDataChanged;
            HScrollBarUserControl_Alpha.valueChange += HScrollBarUserControl_Alpha_ValueChanged; 
            UpdateData(true);
        }

        public PenData penData = new PenData();

        bool _initCmbING = true;
        void InitCmb()
        {
            _initCmbING = true;
            cbb_dashstyle.DrawMode = DrawMode.OwnerDrawVariable;
            cbb_dashstyle.DropDownStyle = ComboBoxStyle.DropDownList;
            cbb_dashstyle.DrawItem += cmb_DrawItem;
            cbb_dashstyle.SelectedIndexChanged += cbb_SelectedIndexChanged;
            cbb_dashstyle.Tag = "dashstyle";
            cbb_dashstyle.Items.Add(DashStyle.Solid);
            cbb_dashstyle.Items.Add(DashStyle.Dash);
            cbb_dashstyle.Items.Add(DashStyle.Dot);
            cbb_dashstyle.Items.Add(DashStyle.DashDot);
            cbb_dashstyle.Items.Add(DashStyle.DashDotDot);
            cbb_dashstyle.SelectedIndex = 0;

            cbb_width.DrawMode = DrawMode.OwnerDrawVariable;
            cbb_width.DropDownStyle = ComboBoxStyle.DropDownList;
            cbb_width.DrawItem += cmb_DrawItem;
            cbb_width.SelectedIndexChanged += cbb_SelectedIndexChanged;
            textBox1.TextChanged += textbox1_Changed;
            cbb_width.Tag = "width";
            cbb_width.Items.Add(1);
            cbb_width.Items.Add(2);
            cbb_width.Items.Add(3);
            cbb_width.Items.Add(4);
            cbb_width.Items.Add(5);
            cbb_width.Items.Add(6);
            cbb_width.Items.Add(7);
            cbb_width.Items.Add(8);
            cbb_width.SelectedIndex = 0;

            cbb_startcap.DrawMode = DrawMode.OwnerDrawVariable;
            cbb_startcap.DropDownStyle = ComboBoxStyle.DropDownList;
            cbb_startcap.DrawItem += cmb_DrawItem;
            cbb_startcap.SelectedIndexChanged += cbb_SelectedIndexChanged;
            cbb_startcap.Tag = "startcap";
            cbb_startcap.Items.Add(LineCap.Flat);
            cbb_startcap.Items.Add(LineCap.Square);
            cbb_startcap.Items.Add(LineCap.Round);
            cbb_startcap.Items.Add(LineCap.Triangle);
            cbb_startcap.Items.Add(LineCap.NoAnchor);
            cbb_startcap.Items.Add(LineCap.SquareAnchor);
            cbb_startcap.Items.Add(LineCap.RoundAnchor);
            cbb_startcap.Items.Add(LineCap.DiamondAnchor);
            cbb_startcap.Items.Add(LineCap.ArrowAnchor);
            cbb_startcap.Items.Add(LineCap.AnchorMask);
            cbb_startcap.SelectedIndex = 0;

            cbb_endcap.DrawMode = DrawMode.OwnerDrawVariable;
            cbb_endcap.DropDownStyle = ComboBoxStyle.DropDownList;
            cbb_endcap.DrawItem += cmb_DrawItem;
            cbb_endcap.SelectedIndexChanged += cbb_SelectedIndexChanged;
            cbb_endcap.Tag = "endcap";
            cbb_endcap.Items.Add(LineCap.Flat);
            cbb_endcap.Items.Add(LineCap.Square);
            cbb_endcap.Items.Add(LineCap.Round);
            cbb_endcap.Items.Add(LineCap.Triangle);
            cbb_endcap.Items.Add(LineCap.NoAnchor);
            cbb_endcap.Items.Add(LineCap.SquareAnchor);
            cbb_endcap.Items.Add(LineCap.RoundAnchor);
            cbb_endcap.Items.Add(LineCap.DiamondAnchor);
            cbb_endcap.Items.Add(LineCap.ArrowAnchor);
            cbb_endcap.Items.Add(LineCap.AnchorMask);
            cbb_endcap.SelectedIndex = 0;

            cbb_linejoin.DrawMode = DrawMode.OwnerDrawVariable;
            cbb_linejoin.DropDownStyle = ComboBoxStyle.DropDownList;
            cbb_linejoin.DrawItem += cmb_DrawItem;
            cbb_linejoin.SelectedIndexChanged += cbb_SelectedIndexChanged;
            cbb_linejoin.Tag = "linejoin";
            cbb_linejoin.Items.Add(LineJoin.Miter);
            cbb_linejoin.Items.Add(LineJoin.Bevel);
            cbb_linejoin.Items.Add(LineJoin.Round);
            cbb_linejoin.SelectedIndex = 0;

            cbb_dashcap.DrawMode = DrawMode.OwnerDrawVariable;
            cbb_dashcap.DropDownStyle = ComboBoxStyle.DropDownList;
            cbb_dashcap.DrawItem += cmb_DrawItem;
            cbb_dashcap.SelectedIndexChanged += cbb_SelectedIndexChanged;
            cbb_dashcap.Tag = "dashcap";
            cbb_dashcap.Items.Add(DashCap.Flat);
            cbb_dashcap.Items.Add(DashCap.Round);
            cbb_dashcap.Items.Add(DashCap.Triangle);
            cbb_dashcap.SelectedIndex = 0;
            _initCmbING = false;
        }

        void UpdateData(bool ToControl)
        {
            if (ToControl)
            {
                cbb_dashstyle.SelectedItem = penData._dashStyle;
                textBox1.Text = penData._width.ToString();
                cbb_width.SelectedItem = (int)penData._width;
                if ((int)penData._width >= 8)
                    cbb_width.SelectedItem = 8;
                cbb_startcap.SelectedItem = penData._startCap;
                cbb_endcap.SelectedItem = penData._endCap;
                cbb_linejoin.SelectedItem = penData._lineJoin;
                cbb_dashcap.SelectedItem = penData._dashCap;
                if (penData.IsPiple)
                    tabControl1.SelectedIndex = 1;
                else
                    tabControl1.SelectedIndex = 0;
                tabControl1_SelectedIndexChanged(null, null);
                HScrollBarUserControl_Alpha.Value = penData.PipleData.Alpha;

                buttonEx_BaseColor.Color = penData.PipleData.BaseColor;
                buttonEx_HighlightColor.Color = penData.PipleData.HighlightColor;
            }
            else
            {
                penData._dashStyle = (DashStyle)cbb_dashstyle.SelectedItem;
                if (textBox1.Text != "")
                    penData._width = int.Parse(textBox1.Text);
                penData._startCap = (LineCap)cbb_startcap.SelectedItem;
                penData._endCap = (LineCap)cbb_endcap.SelectedItem;
                penData._lineJoin = (LineJoin)cbb_linejoin.SelectedItem;
                penData._dashCap = (DashCap)cbb_dashcap.SelectedItem;

                penData.IsPiple = tabControl1.SelectedIndex == 1;
                penData.PipleData.Alpha = (int)HScrollBarUserControl_Alpha.Value;
                penData.PipleData.BaseColor = buttonEx_BaseColor.Color;
                penData.PipleData.HighlightColor = buttonEx_HighlightColor.Color;

                penData.PipleData.StartCap = (LineCap)cbb_startcap.SelectedItem;
                penData.PipleData.EndCap = (LineCap)cbb_endcap.SelectedItem;
                penData.PipleData.LineJoin = (LineJoin)cbb_linejoin.SelectedItem;
                penData.PipleData.Width = penData._width; 
            }
        }


        #region protected
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            panelRf = new Rectangle(panel1.Location.X, panel1.Location.Y, panel1.Width, panel1.Height);

            int w3 = (int)(panel1.Width / 3.0f);
            int h3 = (int)(panel1.Height / 3.0f);
            pts[0] = new PointF(panel1.Left + w3, panel1.Top + h3);
            pts[1] = new PointF(panel1.Left + w3 + w3, panel1.Top + h3);
            pts[2] = new PointF(panel1.Left + w3 + w3, panel1.Top + h3 + h3);
        }
        PointF[] pts = new PointF[3];
        Rectangle panelRf;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            if (!penData.IsPiple)
            {
                Pen p = penData.CreateExamplePen(panelRf);
                if (p != null)
                {
                    e.Graphics.DrawLines(p, pts);
                    p.Dispose();
                }
            }
            else
            {
                DrawPiple(e.Graphics, pts);
            }
        }
        #endregion

        /// <summary>
        /// 画刷控件里面数据发生变化
        /// </summary>
        void BrushDataChanged()
        {
            Invalidate(panelRf);
        }

        private void cmb_DrawItem(object sender, DrawItemEventArgs e)
        {

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            //鼠标选中在这个项上
            if ((e.State & DrawItemState.Selected) != 0)
            {
                //渐变画刷
                LinearGradientBrush brush = new LinearGradientBrush(e.Bounds, Color.FromArgb(255, 251, 237),
                                                 Color.FromArgb(255, 236, 181), LinearGradientMode.Vertical);
                //填充区域
                Rectangle borderRect = new Rectangle(3, e.Bounds.Y, e.Bounds.Width - 5, e.Bounds.Height - 2);

                e.Graphics.FillRectangle(brush, borderRect);

                //画边框
                Pen pen = new Pen(Color.FromArgb(229, 195, 101));
                e.Graphics.DrawRectangle(pen, borderRect);
            }
            else
            {
                SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255));
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            Pen _pen = new Pen(Color.Black);
            ComboBox cbb = (ComboBox)sender;
            string tag = cbb.Tag.ToString();
            if (tag == "dashstyle")
            {
                if (e.Index != -1)
                {
                    _pen.DashStyle = (DashStyle)(cbb_dashstyle.Items[e.Index]);

                    _pen.Width = 5;
                    e.Graphics.DrawLine(_pen, e.Bounds.Left + 3, e.Bounds.Top + 8, e.Bounds.Right - 4, e.Bounds.Top + 8);
                    bool enableDashCap = e.Index != 0;// 为0则可使用
                    cbb_dashcap.Enabled = enableDashCap;
                }
            }
            else if (tag == "width")
            {
                if (e.Index != -1)
                {
                    _pen.Width = (int)(cbb_width.Items[e.Index]);
                    if (_pen.Width == 8)
                        e.Graphics.DrawString("自定义", DefaultFont, _pen.Brush, e.Bounds.Left, e.Bounds.Top + 1);
                    else
                        e.Graphics.DrawLine(_pen, e.Bounds.Left + 3, e.Bounds.Top + 8, e.Bounds.Right - 4, e.Bounds.Top + 8);
                }
            }
            else if (tag == "startcap")
            {
                if (e.Index != -1)
                {

                    _pen.StartCap = (LineCap)(cbb_endcap.Items[e.Index]);
                    _pen.Width = 6;
                    e.Graphics.DrawLine(_pen, e.Bounds.Left + 43, e.Bounds.Top + 8, e.Bounds.Right - 4, e.Bounds.Top + 8);
                }
            }
            else if (tag == "endcap")
            {
                if (e.Index != -1)
                {
                    _pen.EndCap = (LineCap)(cbb_endcap.Items[e.Index]);
                    _pen.Width = 6;
                    e.Graphics.DrawLine(_pen, e.Bounds.Left + 3, e.Bounds.Top + 8, e.Bounds.Right - 40, e.Bounds.Top + 8);
                }
            }
            else if (tag == "linejoin")
            {
                if (e.Index != -1)
                {
                    _pen.LineJoin = (LineJoin)(cbb_linejoin.Items[e.Index]);
                    _pen.Width = 13;
                    Point[] ptslinejoin = new Point[3];
                    ptslinejoin[0] = new Point(e.Bounds.Left + 30, e.Bounds.Top + 8);
                    ptslinejoin[1] = new Point(e.Bounds.Left + 4 + e.Bounds.Width / 2, e.Bounds.Top + 8);
                    ptslinejoin[2] = new Point(e.Bounds.Left + 4 + e.Bounds.Width / 2, e.Bounds.Bottom);
                    e.Graphics.DrawLines(_pen, ptslinejoin);
                }
            }
            else if (tag == "dashcap")
            {
                if (e.Index != -1)
                {
                    _pen.DashStyle = DashStyle.Dot;
                    _pen.DashCap = (DashCap)(cbb_dashcap.Items[e.Index]);
                    _pen.Width = 8;
                    e.Graphics.DrawLine(_pen, e.Bounds.Left + 3, e.Bounds.Top + 8, e.Bounds.Right - 4, e.Bounds.Top + 8);
                }
            }
            if (_pen.Brush != null)
                _pen.Brush.Dispose();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            UpdateData(false);
        }


        private void textbox1_Changed(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                penData._width = int.Parse(textBox1.Text);
                Invalidate(panelRf);
            }
        }
        /// <summary>
        /// 下拉框、编辑框，反生改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_initCmbING)
                return;
            string tag = (string)((ComboBox)sender).Tag;
            if (tag == "width")
            {
                textBox1.Enabled = ((int)cbb_width.SelectedItem >= 8);
                if ((int)cbb_width.SelectedItem < 8)//非自定义
                {
                    textBox1.Text = cbb_width.SelectedItem.ToString();
                    penData._width = (int)cbb_width.SelectedItem;
                }
            }
            if (tag == "dashstyle")
                penData._dashStyle = (DashStyle)cbb_dashstyle.SelectedItem;
            if (tag == "startcap")
                penData._startCap = (LineCap)cbb_startcap.SelectedItem;
            if (tag == "endcap")
                penData._endCap = (LineCap)cbb_endcap.SelectedItem;
            if (tag == "linejoin")
                penData._lineJoin = (LineJoin)cbb_linejoin.SelectedItem;
            if (tag == "dashcap")
                penData._dashCap = (DashCap)cbb_dashcap.SelectedItem;
            Invalidate(panelRf);
        }

        #region piple
        /// <summary>
        /// 切换线条与管道的tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            penData.IsPiple = tabControl1.SelectedIndex == 1;
            bool bEnalbe = tabControl1.SelectedIndex != 1;
            cbb_dashstyle.Enabled = bEnalbe;
            cbb_dashcap.Enabled = bEnalbe;
            
            Invalidate(panelRf);
        }

        /// <summary>
        /// 管道颜色按钮
        /// </summary>
        private void buttonEx_BaseColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = penData.PipleData.BaseColor;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                penData.PipleData.BaseColor = dlg.Color;
                buttonEx_BaseColor.Color = dlg.Color;
                Invalidate(panelRf);
            }
        }
        /// <summary>
        /// 管道颜色按钮
        /// </summary>
        private void buttonEx_HighlightColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = penData.PipleData.HighlightColor;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                penData.PipleData.HighlightColor = dlg.Color;
                buttonEx_HighlightColor.Color = dlg.Color;
                Invalidate(panelRf);
            }
        }
        /// <summary>
        /// 管道透明拖拉
        /// </summary>
        void HScrollBarUserControl_Alpha_ValueChanged()
        {
            penData.PipleData.Alpha = (int)HScrollBarUserControl_Alpha.Value;
            Invalidate(panelRf);
        }

        public void DrawPiple(Graphics g, PointF[] AryPoint)
        {
            if (AryPoint.Length < 2)
                return;
            float _accuracy = 10.0f;
            float tempWidthInterval = penData._width / _accuracy;
            float RInterval = (penData.PipleData.HighlightColor.R - penData.PipleData.BaseColor.R) / _accuracy;
            float GInterval = (penData.PipleData.HighlightColor.G - penData.PipleData.BaseColor.G) / _accuracy;
            float BInterval = (penData.PipleData.HighlightColor.B - penData.PipleData.BaseColor.B) / _accuracy;

            Pen pen = new Pen(penData.PipleData.BaseColor, 1);
            pen.Width = penData._width;
            pen.StartCap = penData._startCap;
            pen.EndCap = penData._endCap;
            pen.LineJoin = penData._lineJoin;
            for (int i = 0; i < _accuracy; i++)
            {
                g.DrawLines(pen, AryPoint);
                pen.Width -= tempWidthInterval;
                pen.Color = Color.FromArgb(penData.PipleData.Alpha, pen.Color.R + (int)RInterval, pen.Color.G + (int)GInterval, pen.Color.B + (int)BInterval);
            }
            pen.Dispose();
        }
        #endregion
    }
}
