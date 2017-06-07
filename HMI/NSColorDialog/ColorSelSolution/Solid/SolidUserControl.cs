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
    internal partial class SolidUserControl : UserControl
    {
        #region 属性、构造
        public SolidUserControl()
        {
            InitializeComponent();
            PathGradientControl1.colorChange += PathGradientControl1_ColorChange;
            BaseColorControl1.BaseColorChange += BaseColorChange;
            RGBUserControl_R.valueChange += ValueChangeR;
            RGBUserControl_G.valueChange += ValueChangeG;
            RGBUserControl_B.valueChange += ValueChangeB;
            RGBUserControl_A.valueChange += ValueChangeA;
            RGBUserControl_A.rgb = "a";
            RGBUserControl_R.rgb = "r";
            RGBUserControl_G.rgb = "g";
            RGBUserControl_B.rgb = "b";
        }

        private Color _color = Color.Brown;
        public Color color
        {
            get
            {
                return _color;
            }
            set
            {
                if (_color == value)
                    return;
                _color = value;
                if (ColorChanged != null)
                    ColorChanged(_color); 
                RGBUserControl_A.Color = _color;
                RGBUserControl_R.Color = _color;
                RGBUserControl_G.Color = _color;
                RGBUserControl_B.Color = _color;
                BaseColorControl1.CurrentSelectColor = BaseColorControl1.ConvertColor(_color);
                PathGradientControl1.BaseColor = BaseColorControl1.CurrentSelectColor;
                PathGradientControl1.color = _color;
                Point pt = PathGradientControl1.ColorToLocation(_color);
                PathGradientControl1.ColorLocation = pt;
            }
        }
        public Brush brush
        {
            get
            {
                return new SolidBrush(_color);
            }
        }


        /// <summary>
        /// 颜色变化事件
        /// </summary>
        /// <param name="clr"></param>
        public delegate void ColorChange(Color clr);
        public ColorChange ColorChanged;
        #endregion

        #region 颜色发生变化通知事件
        /// <summary>
        /// 路径色(渐变矩形)
        /// </summary> 
        void PathGradientControl1_ColorChange(Color clr)
        {
            DrawSwitch = false;
            _color = Color.FromArgb(_color.A, PathGradientControl1.color);
            RGBUserControl_A.Color = _color;
            RGBUserControl_R.Color = _color;
            RGBUserControl_G.Color = _color;
            RGBUserControl_B.Color = _color;

            Point pt = PathGradientControl1.ColorToLocation(_color);
            PathGradientControl1.ColorLocation = pt;
            PathGradientControl1.Invalidate();
            DrawSwitch = true; 
            if (ColorChanged != null)
                ColorChanged(_color); 
        }
        /// <summary>
        /// 基准色
        /// </summary> 
        void BaseColorChange(Color clr)
        {
            DrawSwitch = false;
            PathGradientControl1.BaseColor = clr;
            color = PathGradientControl1.GetCurrentLocationColor();
            DrawSwitch = true;
        }
        void ValueChangeR(int value)
        {
            DrawSwitch = false;
            color = Color.FromArgb(_color.A, value, _color.G, _color.B);
            DrawSwitch = true ;
        }
        void ValueChangeG(int value)
        {
            DrawSwitch = false;
            color = Color.FromArgb(_color.A, _color.R, value, _color.B);
            DrawSwitch = true;
        }
        void ValueChangeB(int value)
        {
            DrawSwitch = false;
            color = Color.FromArgb(_color.A, _color.R, _color.G, value);
            DrawSwitch = true;
        }
        void ValueChangeA(int value)
        {
            DrawSwitch = false;
            color = Color.FromArgb(value, _color.R, _color.G, _color.B);
            DrawSwitch = true;
        }
        #endregion
        bool DrawSwitch
        {
            set
            {
                BaseColorControl1.DrawSwitch = value;
                PathGradientControl1.DrawSwitch = value;
                RGBUserControl_A.DrawSwitch = value;
                RGBUserControl_R.DrawSwitch = value;
                RGBUserControl_G.DrawSwitch = value;
                RGBUserControl_B.DrawSwitch = value;
            }
        }
    }
}
