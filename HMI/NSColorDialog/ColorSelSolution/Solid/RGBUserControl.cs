using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace NetSCADA6.Common.NSColorManger
{
    internal partial class RGBUserControl : UserControl
    {
        public RGBUserControl()
        {
            InitializeComponent();
            textBox1.KeyPress += textBox1_KeyPress;
            _ClientRect = new Rectangle(ClientRectangle.Left + 1, ClientRectangle.Top + 1, ClientRectangle.Width - 2, ClientRectangle.Height - 2);
            _GrayBlackGround = new Rectangle(_ClientRect.Left + 1, _ClientRect.Top + 2, _ClientRect.Width - 2, _ClientRect.Height - 8);
            _ColorBlackGround = new Rectangle(_ClientRect.Left + 1, _GrayBlackGround.Bottom + 1, _ClientRect.Width - 2, 3);
            _TextLocation = new Point(_GrayBlackGround.Left + 5, _GrayBlackGround.Top + _GrayBlackGround.Bottom - DefaultFont.Height);

            _brush = new LinearGradientBrush(ClientRectangle, Color.Transparent, Color.Transparent, LinearGradientMode.Horizontal);
            //////////////////////////////////////////////////////////////////////////Temp 
            Value = 180;
            Suffix = "%";
        }
        #region 属性、变量
        /// <summary>
        /// 显示后缀
        /// </summary>
        public string Suffix
        {
            get
            {
                return _suffix;
            }
            set
            {
                _suffix = value;
            }
        }
        private string _suffix = "";

        Rectangle _ClientRect;
        Rectangle _GrayBlackGround;
        Rectangle _ColorBlackGround;
        Point _TextLocation;
        public string rgb;
        private LinearGradientBrush _brush;
        private Color[] clrAry = new System.Drawing.Color[3];
        private ColorBlend blend = new ColorBlend();
        Color _color = Color.Black;
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                float v = 0;
                if (rgb == "r")
                {
                    clrAry[0] = Color.FromArgb(Color.A, 0, Color.G, Color.B);
                    clrAry[1] = Color.FromArgb(Color.A, Color.R, Color.G, Color.B);
                    clrAry[2] = Color.FromArgb(Color.A, 255, Color.G, Color.B);
                    v = Color.R;
                }
                else if (rgb == "g")
                {
                    clrAry[0] = Color.FromArgb(Color.A, Color.R, 0, Color.B);
                    clrAry[1] = Color.FromArgb(Color.A, Color.R, Color.G, Color.B);
                    clrAry[2] = Color.FromArgb(Color.A, Color.R, 255, Color.B);
                    v = Color.G;
                }
                else if (rgb == "b")
                {
                    clrAry[0] = Color.FromArgb(Color.A, Color.R, Color.G, 0);
                    clrAry[1] = Color.FromArgb(Color.A, Color.R, Color.G, Color.B);
                    clrAry[2] = Color.FromArgb(Color.A, Color.R, Color.G, 255);
                    v = Color.B;
                }
                else if (rgb == "a")
                {
                    clrAry[0] = Color.FromArgb(0, Color.R, Color.G, Color.B);
                    clrAry[1] = Color.FromArgb(Color.A, Color.R, Color.G, Color.B);
                    clrAry[2] = Color.FromArgb(255, Color.R, Color.G, Color.B);
                    v = Color.A;
                }
                blend.Positions = new float[] { 0, v / 255f, 1 };
                blend.Colors = clrAry;
                _value = (int)v;
                float temp1 = _value;
                temp1 = temp1 / 255f;
                temp1 *= 100;
                Percentage = temp1;
                _brush.InterpolationColors = blend;
                Invalidate();
            }
        }

        private int _value = 70;
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                float temp1 = _value;
                temp1 = temp1 / 255;
                temp1 *= 100;
                Percentage = temp1;
                Invalidate();
            }
        }
        private float _percentage = 100;
        public float Percentage
        {
            get
            {
                return (int)_percentage;
            }
            set
            {
                _percentage = value;
                if (_percentage < 0)
                    _percentage = 0;
                if (_percentage > 100)
                    _percentage = 100;

                float f = _percentage / 100 * (_ClientRect.Width - 2);
                _GrayBlackGround.Width = (int)f;
                Invalidate();
            }
        }

        public delegate void ValueChange(int value);
        public ValueChange valueChange;
        #endregion

        #region 鼠标事件
        bool _MouseDown = false;
        Point _ptStart = new Point();
        float _valueStart = 0;
        private void UserControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _ptStart = e.Location;
                _valueStart = Value;
                _MouseDown = true;
            }
        }

        private void UserControl1_MouseUp(object sender, MouseEventArgs e)
        {
            _MouseDown = false;
            if (!_cursorShow)
            {
                _cursorShow = true;
                Cursor.Position = PointToScreen(new Point(_GrayBlackGround.Right, _GrayBlackGround.Top + _GrayBlackGround.Height / 2));
                Cursor.Show();
            }
            if (e.Button == MouseButtons.Left && e.Location == _ptStart && _cursorShow)//进入编辑状态
            {
                TextBoxFun(true);
            }
        }
        bool _cursorShow = true;
        private void UserControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_MouseDown)
            {
                if (_cursorShow)
                {
                    _cursorShow = false;
                    Cursor.Hide();
                }
                float temp1 = (e.Location.X - _ptStart.X) / 2;
                double ret = (_valueStart + temp1);
                if (ret < 0) ret = 0;
                if (ret > 255) ret = 255;
                Value = (int)ret;

                if (valueChange != null)
                    valueChange(Value);

                textBox1.Text = Value.ToString();

            }
            Invalidate();
        }

        #endregion

        #region 绘图功能
        public static void DrawRoundRectangle(Graphics g, Pen pen, Rectangle rect, int cornerRadius)
        {
            using (GraphicsPath path = CreateRoundedRectanglePath(rect, cornerRadius))
            {
                g.DrawPath(pen, path);
            }
        }
        public static void FillRoundRectangleEx(Graphics g, Brush brush, Rectangle rect, int cornerRadius, bool Top) //top true 上边直角 false 下边直角
        {
            using (GraphicsPath path = CreateRoundedRectanglePathEx(rect, cornerRadius, Top))
            {
                g.FillPath(brush, path);
            }
        }
        internal static GraphicsPath CreateRoundedRectanglePathEx(Rectangle rect, int cornerRadius, bool Top)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            if (Top)
            {
                roundedRect.AddLine(rect.X, rect.Y, rect.Right, rect.Y);
                roundedRect.AddLine(rect.Right, rect.Y, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
                roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
                roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
                roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
                roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y);
                roundedRect.CloseFigure();
                return roundedRect;
            }
            else
                roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height);
            roundedRect.AddLine(rect.Right, rect.Bottom, rect.X, rect.Bottom);
            roundedRect.AddLine(rect.X, rect.Bottom, rect.X, rect.Y + cornerRadius * 2);
            roundedRect.CloseFigure();
            return roundedRect;
        }
        public void FillRoundRectangle(Graphics g, Brush brush, Rectangle rect, int cornerRadius)
        {
            using (GraphicsPath path = CreateRoundedRectanglePath(rect, cornerRadius))
            {
                g.FillPath(brush, path);
            }
        }
        internal static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cornerRadius)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
            roundedRect.CloseFigure();
            return roundedRect;
        }
        public bool DrawSwitch = true;
        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            if (DrawSwitch == false)
                return;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            FillRoundRectangle(e.Graphics, Brushes.White, _ClientRect, 4);//背景
            DrawRoundRectangle(e.Graphics, Pens.Gray, _ClientRect, 4);//边框 

            if (!textBox1.Visible)
            {
                //绘制当前RGB值的百分比  
                FillRoundRectangleEx(e.Graphics, Brushes.DarkGray, _GrayBlackGround, 1, false);//背景

                //绘制当前RGB的渐变色
                FillRoundRectangleEx(e.Graphics, _brush, _ColorBlackGround, 3, true);//背景

                //绘制文本
                int num = _value;
                string strClr = "";
                if (rgb == "a") strClr = "透明： ";
                if (rgb == "r") strClr = "红色： ";
                if (rgb == "g") strClr = "绿色： ";
                if (rgb == "b") strClr = "蓝色： ";
                e.Graphics.DrawString(strClr + num.ToString() + Suffix, DefaultFont, Brushes.Black, _TextLocation);
            }
        }
        #endregion

        #region TextBox


        /// <summary>
        /// true 开始编辑  false 结束编辑
        /// </summary>
        public void TextBoxFun(bool Show)
        {
            if (Show)
            {
                textBox1.Width = _ClientRect.Width - 15;
                textBox1.Text = _value.ToString();
                Cursor = Cursors.IBeam;
            }
            else
            {
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    Value = Convert.ToInt32(textBox1.Text);
                    if (Value > 255)
                        Value = 255;
                    if (Value < 0)
                        Value = 0;
                    if (valueChange != null)
                        valueChange(Value);
                }
                Cursor = Cursors.SizeAll;
            }
            textBox1.SelectAll();
            textBox1.Visible = Show;

            textBox1.Focus();
            Invalidate();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                TextBoxFun(false);
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            TextBoxFun(false);
        }
        #endregion

        private void RGBUserControl_Resize(object sender, EventArgs e)
        {
            _ClientRect = new Rectangle(ClientRectangle.Left + 1, ClientRectangle.Top + 1, ClientRectangle.Width - 2, ClientRectangle.Height - 2);
            _GrayBlackGround = new Rectangle(_ClientRect.Left + 1, _ClientRect.Top + 2, _ClientRect.Width - 2, _ClientRect.Height - 8);
            _ColorBlackGround = new Rectangle(_ClientRect.Left + 1, _GrayBlackGround.Bottom + 1, _ClientRect.Width - 2, 3);
            _TextLocation = new Point(_GrayBlackGround.Left + 5, _GrayBlackGround.Top + _GrayBlackGround.Bottom - DefaultFont.Height);

            _brush = new LinearGradientBrush(ClientRectangle, Color.Transparent, Color.Transparent, LinearGradientMode.Horizontal);

        }

    }
}
