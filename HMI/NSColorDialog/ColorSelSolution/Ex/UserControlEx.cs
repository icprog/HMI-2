using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace NetSCADA6.Common.NSColorManger
{
    internal class UserControlEx : UserControl
    {
        /// <summary>
        /// 控件比值
        /// </summary>
        private RectangleF[] _srcControlRectAry;
        /// <summary>
        /// 控件个数
        /// </summary>
        int _controlCount = 0;
        bool _firstRun = true; 

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _srcControlRectAry = new RectangleF[Controls.Count];
            for (int i = 0; i < Controls.Count; i++) //计算原始比值
            {
                _srcControlRectAry[i] = new RectangleF(Controls[i].Location.X / (float)Width,
                Controls[i].Location.Y / (float)Height,
                Controls[i].Width / (float)Width,
                Controls[i].Height / (float)Height);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            for (int i = 0; i < _controlCount; i++)
            {
                int x = (int)(_srcControlRectAry[i].X * Width);
                int y = (int)(_srcControlRectAry[i].Y * Height);
                int w = (int)(_srcControlRectAry[i].Width * Width);
                int h = (int)(_srcControlRectAry[i].Height * Height);
                Controls[i].Location = new Point(x, y);
                Controls[i].Size = new Size(w, h);
            }
        }
    }
}
