using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace NetSCADA6.Common.NSColorManger
{
    /// <summary>
    /// 画刷条
    /// </summary>
    internal partial class BaseGradientUserControl : UserControl
    {
        #region 属性、构造
        public BaseGradientUserControl()
        {
            InitializeComponent();
            _ColorBlendEx = new ColorBlendEx();

        }
        public ColorBlendEx ColorBlendEx
        {
            get
            {
                return _ColorBlendEx;
            }
            set
            {
                _ColorBlendEx = value;
            }
        }
        private ColorBlendEx _ColorBlendEx;
        public SolidUserControl SolidUserCtrl;  //
        LinearGradientBrush _brushLine; //渐变条
        #endregion

        #region 加载时
        //渐变区域
        public Rectangle ClientRect;
        private void BaseGradientUserControl_Load(object sender, EventArgs e)
        {
            UpdateClientRect();
            if (SolidUserCtrl != null)
                SolidUserCtrl.ColorChanged += SelectedColorChange;
        }
        /// <summary>
        /// 检查ColorBLend最少要有2个点,如果不满足则添加
        /// </summary>
        void Check2ColorBlend()
        {
            if (_ColorBlendEx.Count == 0)
            {
                _ColorBlendEx.Add(Color.Red, 0, ClientRect);
            }
            if (_ColorBlendEx.Count == 1)
            {
                _ColorBlendEx.Add(Color.Green, 1, ClientRect);
            }
        }
        #endregion

        #region 当前选择色发生变化通知
        void SelectedColorChange(Color clr)
        {
            ColorFloat cf = _ColorBlendEx.GetSelected();
            if (cf != null)
            {
                cf.Color = clr;
                if (ColorBlendChanged != null)
                    ColorBlendChanged();
             //   Invalidate();
            }
        }
        #endregion

        #region 鼠标事件
        bool _bLeftDown = false;
        private void BaseGradientUserControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bool bSel = _ColorBlendEx.MouseDown(e.Location);
                if (bSel)
                {
                    SolidUserCtrl.color = _ColorBlendEx.GetSelected().Color;
                }
                if (SolidUserCtrl == null)
                    return;
                if (!bSel && ClientRect.Contains(e.Location)) //添加一个新的颜色
                {
                    ColorFloat cf = new ColorFloat(SolidUserCtrl.color, (e.X - ClientRect.X) / (float)ClientRect.Width);
                    cf.Selected = true;
                    _ColorBlendEx.Add(cf, ClientRect);
                    _ColorBlendEx.Redraw();
                }

                Invalidate();
                _bLeftDown = true;
            }
        }

        ColorFloat _clrfloat = null;
        bool _isDelete = false; //是否删除过
        private void BaseGradientUserControl_MouseMove(object sender, MouseEventArgs e)
        { 
            if (!_bLeftDown) 
                return; 
            _ColorBlendEx.MouseMove(e.Location); 
            if (e.Location.Y >= (ClientRect.Bottom + 17) && _ColorBlendEx.Count > 2)
            {
                ColorFloat cf = _ColorBlendEx.GetSelected();
                if (cf != null && _isDelete == false)
                {
                    _clrfloat = cf;
                    _ColorBlendEx.Remove(cf);
                    _isDelete = true;
                }
            }
            else if (_clrfloat != null && ClientRectangle.Contains(e.Location)) //添加一个新的颜色
            {
                ColorFloat cf = new ColorFloat(_clrfloat.Color, (e.X - ClientRect.X) / (float)ClientRect.Width);
                cf.Selected = true;
                _clrfloat = null;
                _isDelete = false;
                _ColorBlendEx.Add(cf, ClientRect);
                _ColorBlendEx.GetSelected()._bMove = true;
                _ColorBlendEx.Redraw();
            }
            if (ColorBlendChanged != null)
                ColorBlendChanged();
            Invalidate();
        }

        private void BaseGradientUserControl_MouseUp(object sender, MouseEventArgs e)
        {
            _isDelete = false;
            _clrfloat = null;
            _bLeftDown = false;
            _ColorBlendEx.MouseUp(e.Location);
            Invalidate();
        }

        #endregion

        public delegate void ColorBlendChange();
        public ColorBlendChange ColorBlendChanged;

        #region 绘图功能
        HatchBrush hb = new HatchBrush((HatchStyle)50, Color.Black, Color.White);
        Bitmap bmp;
        private void BaseGradientUserControl_Paint(object sender, PaintEventArgs e)
        {
            if (bmp == null || bmp.Width != Width)
            {
                bmp = new Bitmap(Width, Height);
            }
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            _brushLine.InterpolationColors = _ColorBlendEx.GetData();

//             if (ColorBlendChanged != null)
//                 ColorBlendChanged();

            g.FillRectangle(hb, ClientRect);
            g.FillRectangle(_brushLine, ClientRect);
            g.DrawRectangle(Pens.Gray, ClientRect);
            _ColorBlendEx.Draw(g);
        }
        #endregion

        private void BaseGradientUserControl_SizeChanged(object sender, EventArgs e)
        {
            UpdateClientRect();
        }
        protected override void OnResize(EventArgs e)
        {
            //     UpdateClientRect();
        }
        void UpdateClientRect()
        {
            ClientRect = new Rectangle(7, 0, Width - 16, Height / 2);
            if (_brushLine == null)
            {
                Rectangle TempRt = new Rectangle(ClientRect.X/* - 1*/, ClientRect.Y, ClientRect.Width, ClientRect.Width);
                _brushLine = new LinearGradientBrush(TempRt, Color.Red, Color.Green, LinearGradientMode.Horizontal);
            }
            else
            {
                _brushLine.Dispose();
                Rectangle TempRt = new Rectangle(ClientRect.X/* - 1*/, ClientRect.Y, ClientRect.Width, ClientRect.Width);
                _brushLine = new LinearGradientBrush(TempRt, Color.Red, Color.Green, LinearGradientMode.Horizontal);
            }
            Check2ColorBlend();
            _brushLine.InterpolationColors = _ColorBlendEx.GetData();
            _ColorBlendEx.ResetClientRect(ClientRect);
            Invalidate();
        }

    }
}
