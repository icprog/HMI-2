using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.ComponentModel;
using System.Drawing.Design;

namespace NetSCADA6.Common.NSColorManger
{
    /// <summary>
    /// 画笔数据集合与操作。
    /// </summary>
    [EditorAttribute(typeof(PenDataEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(PenConverter))]
    public class PenData : ICloneable
    {
        public PenData() { }

        #region 属性
        /// <summary>
        /// 画刷
        /// </summary>
        internal BrushData _brushData = new BrushData();
        /// <summary>
        /// 是否选择了管道
        /// </summary>
        public bool IsPiple
        {
            set { _isPiple = value; }
            get { return _isPiple; }

        }
        private bool _isPiple = false;


        /// <summary>
        /// 管道数据
        /// </summary>
        public PipleData PipleData
        {
            set
            {
                _pipleData = value;
            }
            get
            {
                return _pipleData;
            }
        }
        private PipleData _pipleData = new PipleData();
        #region 画笔参数
        /// <summary>
        /// 线形
        /// </summary>
        public DashStyle DashStyle
        {
            get
            {
                return _dashStyle;
            }
        }
        internal DashStyle _dashStyle = DashStyle.Solid;
        /// <summary>
        /// 宽度
        /// </summary>        
        public float Width
        {
            get
            {
                return _width;
            }
        }
        internal float _width = 1;
        /// <summary>
        /// 起点
        /// </summary>
        public LineCap StartCap
        {
            get
            {
                return _startCap;
            }
        }
        internal LineCap _startCap = LineCap.Round;
        /// <summary>
        /// 末点
        /// </summary>
        public LineCap EndCap
        {
            get
            {
                return _endCap;
            }
        }
        internal LineCap _endCap = LineCap.Round;
        /// <summary>
        /// 连接
        /// </summary>
        public LineJoin LineJoin
        {
            get
            {
                return _lineJoin;
            }
        }
        internal LineJoin _lineJoin = LineJoin.Round;
        /// <summary>
        /// 虚线
        /// </summary>
        public DashCap DashCap
        {
            get
            {
                return _dashCap;
            }
        }
        internal DashCap _dashCap = DashCap.Flat;
        #endregion

        #endregion

        public Pen CreatePen(RectangleF rf, GraphicsPath path = null)
        {
            Brush br = _brushData.CreateBrush(rf, path);
            Pen p = null;
            if (IsPiple)
            {
                p = new Pen(Color.Empty, _width);
                p.StartCap = _startCap;
                p.EndCap = _endCap;
                p.LineJoin = _lineJoin;
                return p;
            }
            if (br == null)
                return null;
            else
                p = new Pen(br);

            p.DashStyle = _dashStyle;
            p.Width = _width;
            p.StartCap = _startCap;
            p.EndCap = _endCap;
            p.LineJoin = _lineJoin;
            p.DashCap = _dashCap;
            return p;
        }
        private const float accuracy = 10.0f;

        /// <summary>
        /// 绘制管道
        /// </summary>
        /// <param name="g"></param>
        /// <param name="path"></param>
        public void DrawPath(Graphics g, GraphicsPath path)
        {
            if (IsPiple)
            {
                PipleData pipleData = PipleData;
                float tempWidthInterval = pipleData.Width / accuracy;
                float RInterval = (pipleData.HighlightColor.R - pipleData.BaseColor.R) / accuracy;
                float GInterval = (pipleData.HighlightColor.G - pipleData.BaseColor.G) / accuracy;
                float BInterval = (pipleData.HighlightColor.B - pipleData.BaseColor.B) / accuracy;

                Pen p = new Pen(pipleData.BaseColor, pipleData.Width);
                p.StartCap = pipleData.StartCap;
                p.EndCap = pipleData.EndCap;
                p.LineJoin = pipleData.LineJoin;
                p.Color = Color.FromArgb(pipleData.Alpha, pipleData.BaseColor);
                for (int i = 0; i < accuracy; i++)
                {
                    g.DrawPath(p, path);
                    p.Width -= tempWidthInterval;
                    p.Color = Color.FromArgb(pipleData.Alpha, p.Color.R + (int)RInterval, p.Color.G + (int)GInterval, p.Color.B + (int)BInterval);
                }
                p.Dispose();
            }
        }

        /// <summary>
        /// 初始化颜色
        /// </summary>
        /// <param name="color"></param>
        /// <param name="width"></param>
        public void Init(Color color, float width)
        {
            _brushData.InitSolidBrush(color);
            _width = width;
        }

        /// <summary>
        /// 初始化为空
        /// </summary>
        public void Init()
        {
            _brushData.InitNull();
        }

        internal Pen CreateExamplePen(RectangleF rf, GraphicsPath path = null)
        {
            Brush br = _brushData.CreateExampleBrush(rf, path);
            if (br == null)
                return null;
            Pen p = new Pen(br);
            p.DashStyle = _dashStyle;
            p.Width = _width;
            p.StartCap = _startCap;
            p.EndCap = _endCap;
            p.LineJoin = _lineJoin;
            p.DashCap = _dashCap;
            return p;
        }
        #region 序列化、克隆
        int version = 1;
        public void Serialize(BinaryFormatter bf, Stream s)
        {
            bf.Serialize(s, version);
            bf.Serialize(s, this._dashStyle);
            bf.Serialize(s, this._width);
            bf.Serialize(s, this._startCap);
            bf.Serialize(s, this._endCap);
            bf.Serialize(s, this._lineJoin);
            bf.Serialize(s, this._dashCap);
            bf.Serialize(s, this._isPiple);
            _brushData.Serialize(bf, s);
            _pipleData.Serialize(bf, s);

        }
        public void Deserialize(BinaryFormatter bf, Stream s)
        {
            version = (int)bf.Deserialize(s);
            _dashStyle = (DashStyle)bf.Deserialize(s);
            _width = (float)bf.Deserialize(s);
            _startCap = (LineCap)bf.Deserialize(s);
            _endCap = (LineCap)bf.Deserialize(s);
            _lineJoin = (LineJoin)bf.Deserialize(s);
            _dashCap = (DashCap)bf.Deserialize(s);
            _isPiple = (bool)bf.Deserialize(s);
            _brushData.Deserialize(bf, s);
            _pipleData.Deserialize(bf, s);
        }
        public object Clone()
        {
            PenData p = new PenData();
            p._dashStyle = this._dashStyle;
            p._width = this._width;
            p._startCap = this._startCap;
            p._endCap = this._endCap;
            p._lineJoin = this._lineJoin;
            p._dashCap = this._dashCap;
            p._isPiple = this._isPiple;
            p._brushData = (BrushData)this._brushData.Clone();
            p._pipleData = (PipleData)this.PipleData.Clone();
            return p;
        }

        #endregion

    }
}
