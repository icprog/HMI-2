using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NetSCADA6.HMI.NSDrawObj
{
    /// <summary>
    /// 图形需要的公共函数库
    /// </summary>
    public static class GraphicsTool
    {
        #region 基本函数
        private static Pen _pen = new Pen(Color.Black);
        private static GraphicsPath _path = new GraphicsPath();
        /// <summary>
        /// 获取文本尺寸
        /// </summary>
        /// <param name="font"></param>
        /// <param name="format"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static SizeF GetTextBounds(Font font, StringFormat format, string text)
        {
            _path.Reset();
            _path.AddString(text, font.FontFamily, (int)font.Style, font.Size, PointF.Empty, format);
            return _path.GetBounds().Size;
        }
        /// <summary>
        /// 获取弧度
        /// </summary>
        /// <param name="theta"></param>
        /// <returns></returns>
        public static float GetRadian(float theta)
        {
            return theta * (float)Math.PI / 180f;
        }
        /// <summary>
        /// 获取角度
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static float GetAngle(float radian)
        {
            return (float)(radian * 180f / Math.PI);
        }
        /// <summary>
        /// 获取加宽后的路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static GraphicsPath GetWidenPath(GraphicsPath path, float width)
        {
            _path.Reset();
            _path.AddPath(path, false);
            //path有且只有两个相同的点时(AddLine)，Widen时会有OutOfMemory异常
            if (path.PointCount == 2)
            {
                if (path.PathPoints[0] == path.PathPoints[1])
                    return _path;
            }

            _pen.Width = width;
            _path.Widen(_pen);

            return _path;
        }
        #endregion

        #region 异常保护
        /// <summary>
        /// Rect为空时，AddArc会报错，写个判断函数
        /// </summary>
        /// <param name="p"></param>
        /// <param name="r"></param>
        /// <param name="start"></param>
        /// <param name="sweep"></param>
        public static void AddArc(GraphicsPath p, RectangleF r, float start, float sweep)
        {
            if (r.Width != 0 && r.Height != 0)
                p.AddArc(r, start, sweep);
        }
        /// <summary>
        /// Rect为空时，DrawArc会报错，写个判断函数
        /// </summary>
        /// <param name="g"></param>
        /// <param name="p"></param>
        /// <param name="r"></param>
        /// <param name="start"></param>
        /// <param name="sweep"></param>
        public static void DrawArc(Graphics g, Pen p, RectangleF r, float start, float sweep)
        {
            if (r.Width != 0 || r.Height != 0)
                g.DrawArc(p, r, start, sweep);
        }
        #endregion

        #region const
        public static readonly int WidenWidth = 3;
        #endregion
    }
}
