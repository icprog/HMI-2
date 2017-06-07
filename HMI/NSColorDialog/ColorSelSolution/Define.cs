using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace NetSCADA6.Common.NSColorManger 
{
    /// <summary>
    /// 图片绘制模式
    /// </summary>
    public enum ImageDrawMode
    {
        /// <summary>
        /// 平铺
        /// </summary>
        [Description("线条")]
        Wrap,
        /// <summary>
        /// 居中
        /// </summary>
        [Description("居中")]
        Center,
        /// <summary>
        /// 拉伸
        /// </summary>
        [Description("拉伸")]
        Stretch
    }
     
    /// <summary>
    /// 画刷类型枚举
    /// </summary>
    internal enum NSBrushType
    {
        /// <summary>
        /// //纯色画刷
        /// </summary>
        Solid,
        /// <summary>
        /// 渐变画刷
        /// </summary>
        LinearGradient,
        /// <summary>
        /// 底纹画刷 
        /// </summary>
        Hatch,
        /// <summary>
        /// //(图片画刷)PathGradient
        /// </summary>
        Textrue,
        /// <summary>
        /// //路径(放射)
        /// </summary>
        PathGradient, 
        /// <summary>
        /// 无画刷
        /// </summary>
        Null
    }

}
