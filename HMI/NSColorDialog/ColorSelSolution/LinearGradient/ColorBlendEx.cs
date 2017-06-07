using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Diagnostics;

namespace NetSCADA6.Common.NSColorManger
{
    /// <summary>
    ///  颜色、浮点
    /// </summary>
    internal class ColorFloat
    {
        #region 构造函数
        public ColorFloat(Color clr, float pos)
        {
            _brush = new SolidBrush(clr);

            Color = clr; Position = pos;
            _ClientRectangle.Width = _width;
            _ClientRectangle.Height = _height;
            OpenUpdateBitmap = _path == null;
        }
        #endregion

        #region 属性  颜色、浮点
        public bool OpenUpdateBitmap = true;

        RectangleF _ClientRectangle = new RectangleF();
        public RectangleF ClientRectangle
        {
            get
            {
                return _ClientRectangle;
            }
            set
            {
                _ClientRectangle = value;
                Position = _ClientRectangle.Location.X / (float)_linearRectangle.Width;
            }
        }

        private bool _oldSelected = false;
        private bool _selected = false;
        /// <summary>
        /// 当前选择状态
        /// </summary>
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _oldSelected = _selected;
                _selected = value;
            }
        }


        private Rectangle _linearRectangle = new Rectangle();
        /// <summary>
        /// 渐变区域
        /// </summary>
        public Rectangle LinearRectangle
        {
            get
            {
                return _linearRectangle;
            }
            set
            {
                _linearRectangle = value;
                UpdateClientRectangle();
            }
        }
        public void UpdateClientRectangle()
        {
            float x = _linearRectangle.X;
            if (Position != 0)
                x += _linearRectangle.Width * Position;
            float y = _linearRectangle.Bottom;
            _ClientRectangle.Location = new PointF(x, y);
        }

        /// <summary>
        /// 颜色数据
        /// </summary>
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                if (_brush == null)
                    _brush = new SolidBrush(value);
                else
                    _brush.Color = value;
            }
        }
        Color _color = Color.Black;

        /// <summary>
        /// 浮点数据
        /// </summary>
        public float Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                UpdateClientRectangle();
            }
        }
        float _position = 0;

        #endregion

        #region 私有 画刷、画笔、路径、位图、矩形
        SolidBrush _brush;
        Bitmap _bmp;
        GraphicsPath _path;//画笔 整个路径
        GraphicsPath _path2;//画笔中间对应颜色的路径
        Color _oldColor = Color.Black;  //用于判断颜色是否发生变化
        //位图内路径的x,y,w,h;
        const int _width = 9;
        const int _height = 15;
        const int _x = 0;
        const int _y = 0;
        const int _OffsetBitmapWidth = 5; //实际显示位图偏移量X  
        const int _OffsetBitmapHeight = 2;//实际显示位图偏移量Y
        float tempX = 0;//鼠标选中拖拉偏移值
        #endregion

        #region 鼠标事件

        /// <summary>
        /// 点point是否在区域内
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains(Point point)
        {
            point.X += _OffsetBitmapHeight;
            point.Y += _OffsetBitmapHeight;
            tempX = point.X - (int)_ClientRectangle.X;
            RectangleF rt = _ClientRectangle;//偏移后
            rt.X -= _OffsetBitmapWidth - 1;
            rt.Y -= _OffsetBitmapHeight - 1;
            rt.Width += 2;
            rt.Height += 2;
            return rt.Contains(point.X, point.Y);
        }


        public bool MouseDown(Point MousePoint)
        {
            MousePoint.X += _OffsetBitmapHeight;
            MousePoint.Y += _OffsetBitmapHeight;
            tempX = MousePoint.X - (int)_ClientRectangle.X;
            RectangleF rt = _ClientRectangle;//偏移后
            rt.X -= _OffsetBitmapWidth - 1;
            rt.Y -= _OffsetBitmapHeight - 1;
            rt.Width += 2;
            rt.Height += 2;
            _bMove = rt.Contains(MousePoint.X, MousePoint.Y);
            Selected = _bMove;
            return _bMove;
        }
        public bool _bMove = false;
        public void MouseMove(Point MousePoint)
        {
            if (!_bMove)
                return;
            MousePoint.X += _OffsetBitmapHeight - (int)tempX;
            MousePoint.Y += _OffsetBitmapHeight;
            if (MousePoint.X < LinearRectangle.Left)
                MousePoint.X = LinearRectangle.Left;
            if (MousePoint.X > LinearRectangle.Right)
                MousePoint.X = LinearRectangle.Right;
            _ClientRectangle.X = MousePoint.X;
            _position = (MousePoint.X - LinearRectangle.Left) / (float)_linearRectangle.Width;
        }

        public void MouseUp(Point MousePoint)
        {
            _bMove = false;
        }
        #endregion

        #region 绘图功能
        public void Draw(Graphics g)
        {
            UpdateBitmap();
            g.DrawImage(_bmp, (_ClientRectangle.X - _OffsetBitmapWidth), (_ClientRectangle.Y - _OffsetBitmapHeight));
        }

        /// <summary>
        /// 当颜色发生变化，更新位图
        /// </summary>
        void UpdateBitmap()
        {
            if (_bmp == null)
            {
                _bmp = new Bitmap(_width + 2, _height + 2);
                if (_path == null || _path2 == null)
                {
                    const int y1f3 = 5;
                    Point[] pts = new Point[6];
                    pts[0] = new Point(_x + _width / 2, _y);
                    pts[1] = new Point(_x, _y + y1f3);
                    pts[2] = new Point(_x, _y + _height);
                    pts[3] = new Point(_x + _width, _y + _height);
                    pts[4] = new Point(_x + _width, _y + y1f3);
                    pts[5] = new Point(_x + _width / 2, _y);
                    _path = new GraphicsPath();
                    _path.AddLines(pts);
                    const int childOffset = 2;   //child 偏移量 颜色区域
                    const int childLeft = _x + childOffset;
                    const int childTop = _y + y1f3 + childOffset;
                    const int childRight = _x + _width - childOffset;
                    const int childBottom = _y + _height - childOffset;
                    Point[] childPts = new Point[5];
                    childPts[0] = new Point(childLeft, childTop);
                    childPts[1] = new Point(childRight, childTop);
                    childPts[2] = new Point(childRight, childBottom);
                    childPts[3] = new Point(childLeft, childBottom);
                    childPts[4] = new Point(childLeft, childTop);
                    _path2 = new GraphicsPath();
                    _path2.AddLines(childPts);
                }
                if (_brush == null)
                {
                    _brush = new SolidBrush(_color);
                }
            }
            if (_oldColor != _color || OpenUpdateBitmap || _selected != _oldSelected)
            {
                _brush.Color = _color;
                Graphics g = Graphics.FromImage(_bmp);
                g.SmoothingMode = SmoothingMode.HighQuality;
                Brush sb = _selected ? Brushes.DarkSlateGray : Brushes.White;
                g.Clear(Color.Transparent);
                g.FillPath(sb, _path); //底色 
                g.DrawPath(Pens.Gray, _path);
                g.FillPath(_brush, _path2);
                g.DrawPath(Pens.SlateGray, _path2);
                _oldColor = _color;
            }
            OpenUpdateBitmap = false;
        }
        #endregion
    }

    internal class ColorBlendEx
    {
        #region 构造 、属性
        public ColorBlendEx()
        {
        }
        public List<ColorFloat> DataList = new List<ColorFloat>();
        public int Count
        {
            get
            {
                if (DataList != null)
                    return DataList.Count;
                return 0;
            }
        }
        #endregion

        #region 添加、删除
        public void Add(Color clr, float pos, Rectangle linearRect)
        {
            ColorFloat cf = new ColorFloat(clr, pos);
            cf.LinearRectangle = linearRect;
            /*cf.Selected = true;
            for (int i = 0; i < DataList.Count; i++)
            {
                DataList[i].Selected = false;
            }*/
            DataList.Add(cf);

        }

        public void Add(ColorFloat cf, Rectangle linearRect)
        {
            cf.LinearRectangle = linearRect;
            if (cf.Selected)
            {
                for (int i = 0; i < DataList.Count; i++)
                {
                    DataList[i].Selected = false;
                }
            }
            DataList.Add(cf);
        }

        public void Remove(Color clr, float pos)
        {
            DataList.Remove(new ColorFloat(clr, pos));
        }

        public void Remove(ColorFloat cf)
        { 
            DataList.Remove(cf);
            SelectIndex = 0;
        }

        #endregion

        #region 鼠标事件、绘图

        public int SelectIndex
        {
            get
            {
                for (int i = 0; i < DataList.Count; i++)
                {
                    if (DataList[i].Selected)
                        return i;
                }
                return -1;
            }
            set
            {
                for (int i = 0; i < DataList.Count; i++)
                {
                    DataList[i].Selected = false;
                    if (i == value)
                        DataList[i].Selected = true;
                }
            }
        }

        public ColorFloat GetSelected()
        {
            for (int i = 0; i < DataList.Count; i++)
            {
                if (DataList[i].Selected)
                    return DataList[i];
            }
            return null;
        }

        /// <summary>
        /// 重绘画笔
        /// </summary>
        public void Redraw()
        {
            foreach (ColorFloat cf in DataList)
                cf.OpenUpdateBitmap = true;
        }

        public void Draw(Graphics g)
        {
            for (int i = 0; i < DataList.Count; i++)
            {
                DataList[i].Draw(g);
            }
        }

        /// <summary>
        /// 点point是否在区域内
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains(Point point)
        {
            for (int i = 0; i < DataList.Count; i++)
            { 
                bool ret = DataList[i].Contains(point);
                if (ret)
                    return true;
            }
            return false;
        }

        public bool MouseDown(Point MousePoint)
        {
            bool bReturn = false;
            for (int i = 0; i < DataList.Count; i++)
            {
                if (bReturn)
                {
                    DataList[i].Selected = false;
                    continue;
                }

                if (DataList[i].MouseDown(MousePoint))
                {
                    bReturn = true;
                }

            }
            return bReturn;
        }

        public void MouseMove(Point MousePoint)
        {
            for (int i = 0; i < DataList.Count; i++)
            {
                DataList[i].MouseMove(MousePoint);
            }
        }

        public ColorFloat MouseUp(Point MousePoint)
        {
            ColorFloat cf = null;
            for (int i = 0; i < DataList.Count; i++)
            {
                DataList[i].MouseUp(MousePoint);
            }
            return cf;
        }

        #endregion

        #region 数据处理转换

        public void ReversalData()
        {
            for (int i = 0; i < DataList.Count; i++)
            {
                DataList[i].Position = 1 - DataList[i].Position;
            }
            Bubble();
        }

        public void ResetClientRect(Rectangle rect)
        {
            for (int i = 0; i < DataList.Count; i++)
            {
                DataList[i].LinearRectangle = rect;
                DataList[i].UpdateClientRectangle();
            }
        }

        public void Reset(ColorBlend ColorBlend, Rectangle rect)
        {
            DataList.Clear();
            int i;
            for (i = 0; i < ColorBlend.Colors.Length; i++)
            {
                if (ColorBlend.Colors.Length > 3)
                {
                    if (i == 0 && ColorBlend.Positions[i] == 0)
                        continue;
                    if (i == ColorBlend.Colors.Length - 1 && ColorBlend.Positions[i] == 1)
                        continue;
                }
                ColorFloat cf = new ColorFloat(ColorBlend.Colors[i], ColorBlend.Positions[i]);
                Add(cf, rect);
            }
        }

        public ColorBlend GetData()
        {
            ColorBlend cb = new ColorBlend(DataList.Count);
            Bubble();
            cb.Colors = GetColors();
            cb.Positions = GetPositions(); 
            return cb;
        }

        /// <summary>
        /// 排序 DataList
        /// </summary> 
        void Bubble()
        {
            ColorFloat temp;
            for (int i = DataList.Count; i > 0; i--)
            {
                for (int j = 0; j < i - 1; j++)
                {
                    if (DataList[j].Position > DataList[j + 1].Position)
                    {
                        temp = DataList[j];
                        DataList[j] = DataList[j + 1];
                        DataList[j + 1] = temp;
                    }
                }
            }
        }

        private float[] GetPositions()
        {
            float[] Positions = new float[DataList.Count + 2];
            Positions[0] = 0;
            for (int i = 0; i < DataList.Count; i++)
            {
                Positions[i + 1] = DataList[i].Position;
            }
            Positions[DataList.Count + 1] = 1;
            return Positions;
        }

        private Color[] GetColors()
        {
            Color[] Colors = new Color[DataList.Count + 2];
            Colors[0] = DataList[0].Color;
            for (int i = 0; i < DataList.Count; i++)
            {
                Colors[i + 1] = DataList[i].Color;
            }
            Colors[DataList.Count + 1] = DataList[DataList.Count - 1].Color;
            return Colors;
        }
        #endregion
    }
}
