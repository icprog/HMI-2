using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Drawing2D;

namespace NetSCADA6.Common.NSColorManger
{
    /// <summary>
    /// 管道
    /// </summary>
    [TypeConverterAttribute(typeof(ExpandableObjectConverterEx))]
    [DescriptionAttribute("展开以查看获取或设置图形线条的样式。")]
    [DisplayName("管道")]
    public class PipleData
    {
        public PipleData()
        { } 
        /// <summary>
        /// 高亮颜色(中心色)
        /// </summary> 
        public Color HighlightColor
        {
            get { return Color.FromArgb(Alpha, _highlightColor); }
            set { _highlightColor = Color.FromArgb(Alpha, value); }
        }
        private Color _highlightColor = Color.White;

        /// <summary>
        /// 基础颜色(外边色)
        /// </summary>  
        public Color BaseColor
        {
            get { return Color.FromArgb(Alpha, _baseColor); }
            set { _baseColor = Color.FromArgb(Alpha, value); }
        }
        private Color _baseColor = Color.Black;


        /// <summary>
        /// 管道透明
        /// </summary>
        public int Alpha
        {
            get { return _alpha; }
            set { _alpha = value; }
        }
        private int _alpha = 255;

        /// <summary>
        /// 宽度
        /// </summary>        
        public float Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }
        private float _width = 1;

        /// <summary>
        /// 起点
        /// </summary>
        public LineCap StartCap
        {
            get
            {
                return _startCap;
            }
            set
            {
                _startCap = value;
            }
        }
        private LineCap _startCap = LineCap.Round;
        /// <summary>
        /// 末点
        /// </summary>
        public LineCap EndCap
        {
            get
            {
                return _endCap;
            }
            set
            {
                _endCap = value;
            }
        }
        private LineCap _endCap = LineCap.Round;
        /// <summary>
        /// 连接
        /// </summary>
        public LineJoin LineJoin
        {
            get
            {
                return _lineJoin;
            }
            set
            {
                _lineJoin = value;
            }
        }
        private LineJoin _lineJoin = LineJoin.Round;


        #region 序列化、克隆
        int version = 1;
        public void Serialize(BinaryFormatter bf, Stream s)
        {
            bf.Serialize(s, version);
            bf.Serialize(s, this._baseColor);
            bf.Serialize(s, this._highlightColor);
            bf.Serialize(s, this._alpha);
            bf.Serialize(s, this._startCap);
            bf.Serialize(s, this._endCap);
            bf.Serialize(s, this._lineJoin);
            bf.Serialize(s, this._width); 

        }
        public void Deserialize(BinaryFormatter bf, Stream s)
        {
            version = (int)bf.Deserialize(s);
            _baseColor = (Color)bf.Deserialize(s);
            _highlightColor = (Color)bf.Deserialize(s);
            _alpha = (int)bf.Deserialize(s);
            _startCap = (LineCap)bf.Deserialize(s);
            _endCap = (LineCap)bf.Deserialize(s);
            _lineJoin = (LineJoin)bf.Deserialize(s);
            _width = (float)bf.Deserialize(s);
        }
        public object Clone()
        {
            PipleData p = new PipleData();
            p._alpha = this._alpha;
            p._highlightColor = this._highlightColor;
            p._baseColor = this._baseColor;
            p._startCap = this._startCap;
            p._endCap = this._endCap;
            p._lineJoin = this._lineJoin;
            p._width = this._width;  
            return p;
        }
        #endregion

    }
}
