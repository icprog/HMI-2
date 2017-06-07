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
    public partial class HScrollBarUserControl : UserControl
    {
        public HScrollBarUserControl()
        {
            InitializeComponent();
            textBox1.KeyPress += textBox1_KeyPress;
            _ClientRect = new Rectangle(ClientRectangle.Left + 1, ClientRectangle.Top + 1, ClientRectangle.Width - 2, ClientRectangle.Height - 2);
            _GrayBlackGround = new Rectangle(_ClientRect.Left + 1, _ClientRect.Top + 2, _ClientRect.Width - 2, _ClientRect.Height - 4);
            _TextLocation = new Point(_GrayBlackGround.Left + 5, _GrayBlackGround.Top + _GrayBlackGround.Bottom - DefaultFont.Height);

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

        public string Prefix
        {
            get
            {
                return _Prefix;
            }
            set
            {
                _Prefix = value;
            }
        }
        private string _Prefix = "";
        Rectangle _ClientRect;
        Rectangle _GrayBlackGround;
        Point _TextLocation;

        private int _max = 100;
        public int Max
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
                UpdatePercentage();
            }
        }

        private int _min = 0;
        public int Min
        {
            get
            {
                return _min;
            }
            set
            {
                _min = value;
                UpdatePercentage();
            }
        }


        private float _value = 50;
        public float Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                if (_value > Max)
                    _value = Max;
                if (_value < Min)
                    _value = Min;
                UpdatePercentage();
            }
        }


        void UpdatePercentage()
        {
            float valueTemp = _value;
            float percentage = valueTemp / (float)(_max - _min) * 100;
            float f = percentage / 100 * (_ClientRect.Width - 2);
            _GrayBlackGround.Width = (int)f;
            Invalidate();
        }

        public delegate void ValueChange();
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
                if (ret < _min) ret = _min;
                if (ret > _max) ret = _max;
                Value = (int)ret;

                if (valueChange != null)
                    valueChange();

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
        public static void FillRoundRectangleEx(Graphics g, Brush brush, Rectangle rect, int cornerRadius) //top true 上边直角 false 下边直角
        {
            using (GraphicsPath path = CreateRoundedRectanglePathEx(rect, cornerRadius))
            {
                g.FillPath(brush, path);
            }
        }
        internal static GraphicsPath CreateRoundedRectanglePathEx(Rectangle rect, int cornerRadius)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddLine(rect.X + cornerRadius * 2, rect.Y, rect.Right - cornerRadius * 2, rect.Y);//modify - 
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);//modify
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
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
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270,90);
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
            roundedRect.CloseFigure();
            return roundedRect;
        }
        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            FillRoundRectangle(e.Graphics, Brushes.White, _ClientRect, 4);//背景
            DrawRoundRectangle(e.Graphics, Pens.Gray, _ClientRect, 4);//边框 

            if (!textBox1.Visible)
            {
                FillRoundRectangleEx(e.Graphics, Brushes.DarkGray, _GrayBlackGround, 2);//背景

                //绘制文本
                float text = _value;
                //  text = (int)(100 / (float)(Max - Min) * _value);
                e.Graphics.DrawString(Prefix + text + Suffix, DefaultFont, Brushes.Black, _TextLocation);
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
                    if (valueChange != null)
                        valueChange();
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
            _GrayBlackGround = new Rectangle(_ClientRect.Left + 1, _ClientRect.Top + 2, _ClientRect.Width - 2, _ClientRect.Height - 4);
            _TextLocation = new Point(_GrayBlackGround.Left + 5, _GrayBlackGround.Top + _GrayBlackGround.Bottom - DefaultFont.Height);
        } 
    }
}
