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
    internal partial class PathGradientControl : UserControl
    {
        public PathGradientControl()
        {
            InitializeComponent();
            InitBlackGround();
        }
        void InitBlackGround()
        {
            Point pt1 = new Point(ClientRectangle.Right, ClientRectangle.Top);
            Point pt2 = new Point(ClientRectangle.Right, ClientRectangle.Bottom);
            Point pt3 = new Point(ClientRectangle.Left, ClientRectangle.Bottom);
            Point pt4 = new Point(ClientRectangle.Left, ClientRectangle.Top);
            _ptary = new Point[] { pt1, pt2, pt3, pt4 };
            _PathGradientBrush = new PathGradientBrush(_ptary);
            _PathGradientBrush.CenterPoint = new PointF(ClientRectangle.Top, ClientRectangle.Right);
        }
        private Color _baseColor = Color.Red;
        public Color BaseColor
        {
            get
            {
                return _baseColor;
            }
            set
            {
                if (value != _baseColor)
                {
                    _baseColor = value;
                    Invalidate();
                }
            }
        }
        Color _color = Color.Brown;
        public Color color
        {
            set
            {
                if (_color != value)
                {
                    _color = value;
                }
                Invalidate();
            }
            get
            {
                return _color;
            }
        }

        #region 逻辑 、绘图
        public bool DrawSwitch = true;
        private void UserControl2_Paint(object sender, PaintEventArgs e)
        {
            if (DrawSwitch == false)
                return;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            DrawBlackGround(e.Graphics);
       //     e.Graphics.DrawString("颜色编辑器：", DefaultFont, Brushes.Black, 2, 2);
            DrawEllipse(e.Graphics);
        }
        Point[] _ptary;
        PathGradientBrush _PathGradientBrush;
        void DrawBlackGround(Graphics g)
        {
            _PathGradientBrush.SurroundColors = new Color[] { _baseColor, Color.FromArgb(255, 0, 0, 0), Color.FromArgb(255, 0, 0, 0), Color.FromArgb(255, 255, 255, 255) };

            g.FillRectangle(_PathGradientBrush, ClientRectangle);
        }

        private Point _ColorLocation = new Point(0, 0);
        private Rectangle _EllipseLocation = new Rectangle(125, 20, 10, 10);
        public Point ColorLocation
        {
            get
            {
                return _ColorLocation;
            }
            set
            {
                if (_bSetEnalbeLocation)
                {
                    _ColorLocation = value;
                    _EllipseLocation.X = _ColorLocation.X - 5;
                    _EllipseLocation.Y = _ColorLocation.Y - 5;
                }
            }
        }

        Pen _penWhite = new Pen(Color.White, 3);
        Pen _penBlack = new Pen(Color.Black, 1);
        void DrawEllipse(Graphics g)
        {
            g.DrawEllipse(_penWhite, _EllipseLocation);
            g.DrawEllipse(_penBlack, _EllipseLocation);
        }

        public Color GetCurrentLocationColor()
        {
            return LocationToColor(_ColorLocation);
        }

        Color LocationToColor(Point pt)
        {
            float fHeight = Height - 1.5f;
            if ((fHeight - pt.Y) < 0) fHeight = 0;
            Color baseClr = BaseColor;
            float f = (Width - pt.X) / (float)Width;
            int r = (int)((255 - baseClr.R) * f) + baseClr.R;
            int g = (int)((255 - baseClr.G) * f) + baseClr.G;
            int b = (int)((255 - baseClr.B) * f) + baseClr.B;

            baseClr = Color.FromArgb(255, r, g, b);
            f = (fHeight - pt.Y) / fHeight;
            r = (int)(baseClr.R * f);
            g = (int)(baseClr.G * f);
            b = (int)(baseClr.B * f);
            if (r < 0) r = 0; if (g < 0) g = 0; if (b < 0) b = 0;
            baseClr = Color.FromArgb(255, r, g, b);
            return Color.FromArgb(255, r, g, b);
        }

        public Point ColorToLocation(Color clr)
        {
            float r = clr.R;
            float g = clr.G;
            float b = clr.B;
            if (r == g && r == b)
            {
                return _ColorLocation;
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
                return new Point(Width, Height);


            float mid2 = mid * 255 / max;
            float min2 = min * 255 / max;

            float y = (1 - max / 255F) * Height;
            float x = (1 - min2 / 255f) * Width;


            return new Point((int)x, (int)y);
        }

        /// <summary>
        ///  0 最小：1中间：2和其他：最大
        /// </summary>
        /// <param name="i"></param>
        /// <param name="clr"></param>
        /// <returns></returns>
        int GetMinMidMax(int i, Color clr)
        {
            int r = clr.R;
            int g = clr.G;
            int b = clr.B;
            if (r == g && r == b)
            {
                return r;
            }
            int max = r > g ? r : g;
            max = max > b ? max : b;
            int min = r < g ? r : g;
            min = min < b ? min : b;
            int mid = 0;
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
            if (i == 0)
                return min;
            if (i == 1)
                return mid;
            if (i == 2)
                return max;

            return max;
        }

        public Color LocationToColor(int x, int y)
        {
            Color baseClr = BaseColor;
            baseClr = Color.FromArgb(255, 255, 100, 0);
            float f = (Width - x) / (float)Width;
            int r = (int)((255 - baseClr.R) * f) + baseClr.R;
            int g = (int)((255 - baseClr.G) * f) + baseClr.G;
            int b = (int)((255 - baseClr.B) * f) + baseClr.B;

            baseClr = Color.FromArgb(255, r, g, b);
            f = (Height - y) / (float)Height;
            r = (int)(baseClr.R * f);
            g = (int)(baseClr.G * f);
            b = (int)(baseClr.B * f);

            baseClr = Color.FromArgb(255, r, g, b);
            return Color.FromArgb(255, r, g, b);
        }

        #endregion


        #region 鼠标事件
        bool _bDown = false;
        bool _bSetEnalbeLocation = true;
        private void UserControl2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _bDown = true;
                if (ClientRectangle.Contains(e.Location))
                {
                    ColorLocation = e.Location;
                    _color = LocationToColor(ColorLocation);
                    _bSetEnalbeLocation = false;
                    if (colorChange != null)
                        colorChange(_color);
                    _bSetEnalbeLocation = true;
                    //      Invalidate();
                }
            }
        }

        private void UserControl2_MouseMove(object sender, MouseEventArgs e)
        {
            if (_bDown)
            {
                Point ptTemp = e.Location;
                if (ptTemp.X > ClientRectangle.Right)
                    ptTemp.X = ClientRectangle.Right;
                if (ptTemp.X < ClientRectangle.Left)
                    ptTemp.X = ClientRectangle.Left;
                if (ptTemp.Y > ClientRectangle.Bottom)
                    ptTemp.Y = ClientRectangle.Bottom;
                if (ptTemp.Y < ClientRectangle.Top)
                    ptTemp.Y = ClientRectangle.Top;
                _color = LocationToColor(ptTemp);
                ColorLocation = ptTemp;
                _bSetEnalbeLocation = false;
                if (colorChange != null)
                    colorChange(_color);
                _bSetEnalbeLocation = true;
                //      Invalidate();
            }
        }
        private void UserControl2_MouseUp(object sender, MouseEventArgs e)
        {
            _bDown = false;
            _bSetEnalbeLocation = true;
        }

        public delegate void ColorChange(Color clr);
        public ColorChange colorChange;

        #endregion

        private void PathGradientControl_SizeChanged(object sender, EventArgs e)
        {
            InitBlackGround();
        }

    }
}
