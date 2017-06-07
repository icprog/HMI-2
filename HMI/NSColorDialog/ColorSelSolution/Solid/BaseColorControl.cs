using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace NetSCADA6.Common.NSColorManger
{
    internal partial class BaseColorControl : UserControlEx
    {
        #region 属性、构造
        public BaseColorControl()
        {
            InitializeComponent();
            brush = new LinearGradientBrush(ClientRectangle, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
            pntArr = new Point[3];
            for (int i = 0; i < pntArr.Length; i++)
            {
                pntArr[i] = new Point();
            }
        }
        private Color _currentSelectColor;
        public Color CurrentSelectColor
        {
            get
            {
                return _currentSelectColor;
            }
            set
            {

                _currentSelectColor = (value);
                _currentSelectHeight = ColorToHeight(_currentSelectColor);
                Invalidate();
            }
        }
        int _currentSelectHeight;
        public int CurrentSelectHeight
        {
            get
            {
                return _currentSelectHeight;
            }
            set
            {
                _currentSelectHeight = value;
                _currentSelectColor = HeightToColor(_currentSelectHeight);
                Invalidate();
            }
        }
        public delegate void ColorChange(Color c);
        public ColorChange BaseColorChange;
        #endregion

        #region 绘图功能
        LinearGradientBrush brush;
        public bool DrawSwitch = true;
        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            if (DrawSwitch == false)
                return;
            DrawBlackGround(e.Graphics);
            DrawTriangle(e.Graphics);
        }

        void DrawBlackGround(Graphics g)
        {
            Color[] clrAry = new System.Drawing.Color[7];
            clrAry[0] = Color.FromArgb(255, 255, 0, 0);
            clrAry[1] = Color.FromArgb(255, 255, 255, 0);
            clrAry[2] = Color.FromArgb(255, 0, 255, 0);
            clrAry[3] = Color.FromArgb(255, 0, 255, 255);
            clrAry[4] = Color.FromArgb(255, 0, 0, 255);
            clrAry[5] = Color.FromArgb(255, 255, 0, 255);
            clrAry[6] = Color.FromArgb(255, 255, 0, 0);
            float f = 1 / 6f;
            ColorBlend blend = new ColorBlend();
            blend.Positions = new float[] { f * 0, f * 1, f * 2, f * 3, f * 4, f * 5, 1 };
            blend.Colors = clrAry;
            brush.InterpolationColors = blend;
            g.FillRectangle(brush, ClientRectangle);
        }

        Point[] pntArr;
        void DrawTriangle(Graphics g)
        {
            int height = CurrentSelectHeight;
            if (CurrentSelectHeight > Height)
                height = Height;
            if (CurrentSelectHeight < 0)
                height = 0;
            pntArr[0].X = ClientRectangle.Left;
            pntArr[0].Y = height - 5;
            pntArr[1].X = ClientRectangle.Left;
            pntArr[1].Y = height + 5;
            pntArr[2].X = ClientRectangle.Left + 5;
            pntArr[2].Y = height;
            g.FillPolygon(new SolidBrush(Color.Black), pntArr);

            pntArr[0].X = ClientRectangle.Right;
            pntArr[0].Y = height - 5;
            pntArr[1].X = ClientRectangle.Right;
            pntArr[1].Y = height + 5;
            pntArr[2].X = ClientRectangle.Right - 5;
            pntArr[2].Y = height;
            g.FillPolygon(new SolidBrush(Color.Black), pntArr);
        }
        //绘制三角形        
        private void drawSjx(List<Point> Points)
        {
            Graphics graphics = this.CreateGraphics();
            Point[] pntArr = { Points[0], Points[1], Points[2] };
            graphics.FillPolygon(new SolidBrush(Color.Black), pntArr);
        }

        #endregion

        #region 功能转换
        public Color ConvertColor(Color clr)
        {
            float r = clr.R;
            float g = clr.G;
            float b = clr.B;
            if (r == g && r == b)
            {
                return _currentSelectColor;
            }
            float max = r > g ? r : g;
            max = max > b ? max : b;
            float min = r < g ? r : g;
            min = min < b ? min : b;
            float mid = 0;
            if (r < max && r > min)
            {
                mid = r;
            }
            else if (g < max && g > min)
            {
                mid = g;
            }
            else if (b < max && b > min)
            {
                mid = b;
            }
            else if (mid == 0)
            {
                if (r == b)
                {
                    mid = r;
                }
                if (b == g)
                {
                    mid = b;
                }
                if (g == r)
                {
                    mid = g;
                }
            }
            // First Convert
            if (max == 0)
                return Color.FromArgb(clr.A, 0, 0, 0);
            mid = mid * 255 / max;
            min = min * 255 / max;
            max = 255;

            if (min == 255)
                return Color.FromArgb(clr.A, 255, 255, 255);
            mid = 255 * (mid - min) / (255 - min);
            min = 0;
            if (mid > 255 || mid < 0)
            {
                mid = 0;
            }
            if (r >= g && r >= b)
            {
                r = (int)max;
                if (g >= b)
                { g = mid; b = min; }
                else
                { g = min; b = mid; }
            }
            else if (g >= r && g >= b)
            {
                g = (int)max;
                if (r >= b)
                { r = mid; b = min; }
                else
                { r = min; b = mid; }
            }
            else if (b >= g && b >= r)
            {
                b = (int)max;
                if (r >= g)
                { r = mid; g = min; }
                else
                { r = min; g = mid; }
            }
            return Color.FromArgb((int)r, (int)g, (int)b);
        }
        Color HeightToColor(int CurrentHeight)
        {
            float f = 1 / 6f;
            float fHeight = CurrentHeight / (float)ClientRectangle.Height;
            int s = (int)(fHeight / f);
            float actHeight = (fHeight - (s) * f) / f;
            int r = 255;
            int g = 0;
            int b = 0;
            Color clr = Color.Red;
            switch (s)
            {
                case 0:
                    {
                        r = 255;
                        g = (int)(255 * actHeight);//  
                        b = 0;
                        break;
                    }
                case 1:
                    {
                        r = (int)(255 * (1 - actHeight));
                        g = 255;
                        b = 0;
                        break;
                    }
                case 2:
                    {
                        r = 0;
                        g = 255;
                        b = (int)(255 * actHeight);
                        break;
                    }
                case 3:
                    {
                        r = 0;
                        g = (int)(255 * (1 - actHeight));
                        b = 255;
                        break;
                    }
                case 4:
                    {
                        r = (int)(255 * actHeight);
                        g = 0;
                        b = 255;
                        break;
                    }
                case 5:
                    {
                        r = 255;
                        g = 0;
                        b = (int)(255 * (1 - actHeight));
                        break;
                    }
            }
            if (r > 255) r = 255; if (g > 255) g = 255; if (b > 255) b = 255;
            if (r < 0) r = 0; if (g < 0) g = 0; if (b < 0) b = 0;
            clr = Color.FromArgb(255, r, g, b);
            return clr;
        }
        int ColorToHeight(Color CurrentColor)
        {
            int r = CurrentColor.R;
            int g = CurrentColor.G;
            int b = CurrentColor.B;
            float f = 1 / 6f;
            if (r == 255)
            {
                if (g != 0 && b == 0)
                {
                    int step = 0;
                    float cellHeight = ClientRectangle.Height * f;
                    float otherHeight = cellHeight * step;
                    float height = cellHeight / 255 * g + otherHeight;
                    return (int)height;
                }
                else if (b != 0 && g == 0)
                {
                    int step = 6;
                    float cellHeight = ClientRectangle.Height * f;
                    float otherHeight = cellHeight * step;
                    float height = otherHeight - cellHeight / 255 * b;
                    return (int)height;
                }
                return 0;
            }

            if (g == 255)
            {
                int step = 2;
                float cellHeight = ClientRectangle.Height * f;
                float otherHeight = cellHeight * step;
                if (b != 0 && r == 0)
                {
                    float height = cellHeight / 255 * b + otherHeight;
                    return (int)height;
                }
                else if (r != 0 && b == 0)
                {
                    float height = otherHeight - cellHeight / 255 * r;
                    return (int)height;
                }
                return (int)otherHeight;
            }

            if (b == 255)
            {
                int step = 4;
                float cellHeight = ClientRectangle.Height * f;
                float otherHeight = cellHeight * step;
                if (r != 0 && g == 0)
                {
                    float height = cellHeight / 255 * r + otherHeight;
                    return (int)height;
                }
                else if (g != 0 && r == 0)
                {
                    float height = otherHeight - cellHeight / 255 * g;
                    return (int)height;
                }
                return (int)otherHeight;
            }


            return 0;
        }
        #endregion

        #region 鼠标事件
        private void UserControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                CurrentSelectHeight = e.Location.Y;
                _currentSelectColor = HeightToColor(e.Location.Y);
                MouseLeftDown = true;

                if (BaseColorChange != null)
                {
                    BaseColorChange(_currentSelectColor);
                }
            }

        }
        bool MouseLeftDown = false;
        private void UserControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseLeftDown)
            {
                _currentSelectColor = HeightToColor(e.Location.Y);
                CurrentSelectHeight = e.Location.Y;
                if (BaseColorChange != null)
                {
                    BaseColorChange(_currentSelectColor);
                }
            }

        }
        private void UserControl1_MouseUp(object sender, MouseEventArgs e)
        {
            MouseLeftDown = false;
        }
        #endregion

        private void BaseColorControl_Resize(object sender, EventArgs e)
        {
            brush = new LinearGradientBrush(ClientRectangle, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
            Invalidate(true);
        }
    }
}
