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
    internal partial class FlowCellUserControl : UserControl
    {
        public FlowCellUserControl(FlowCell CellPreset)
        {
            InitializeComponent();
            this.CellPreset = CellPreset;
        }

        /// <summary>
        /// 绘制画刷
        /// </summary>
        public FlowCell CellPreset;

        /// <summary>
        /// 设置边框大小(非绘制区域)
        /// </summary>
        public int Length
        {
            get
            {
                return Height;
            }
            set
            {
                Width = value;
                Height = value + 2;
            }
        }

        public bool _Selected = false;
        public bool Selected
        {
            set
            {
                _Selected = value;
                if (EventSelectTrue != null && _Selected == true)
                    EventSelectTrue(this);
            }
            get
            {
                return _Selected;
            }
        }
        public delegate void SelectTrue(FlowCellUserControl cellUC);
        public SelectTrue EventSelectTrue;


        public delegate void MenuDelete(FlowCellUserControl cellUC);
        public MenuDelete EventMenuDelete;

        /// <summary>
        /// 绘制区域
        /// </summary>
        public Rectangle ClientRect
        {
            get
            {
                /*  if (!Selected)
                  {*/
                _ClentRect.X = 0;
                _ClentRect.Y = 0;
                _ClentRect.Width = ClientRectangle.Width - 4;
                _ClentRect.Height = ClientRectangle.Height - 2;
                /*  }
                else
                 {
                     _ClentRect.X = 2;
                     _ClentRect.Y = 2;
                     _ClentRect.Width = ClientRectangle.Width - 4;
                     _ClentRect.Height = ClientRectangle.Height - 2;
                 }*/
                return _ClentRect;
            }
        }
        Rectangle _ClentRect = new Rectangle(0, 0, 0, 0);

        HatchBrush hb = new HatchBrush((HatchStyle)50, Color.Black, Color.White);
        Pen pen = new Pen(Brushes.Black, 1);
        protected override void OnPaint(PaintEventArgs e)
        {
            if (e.Graphics.SmoothingMode != SmoothingMode.HighQuality)
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            base.OnPaint(e);
            if (CellPreset.Brush != null)
            {
                e.Graphics.FillRectangle(hb, ClientRect);

                if (CellPreset.Brush is PathGradientBrush)
                {
                    Point[] pts = new Point[4];
                    pts[0] = new Point(ClientRect.Left, ClientRect.Top);
                    pts[1] = new Point(ClientRect.Right, ClientRect.Top);
                    pts[2] = new Point(ClientRect.Right, ClientRect.Bottom);
                    pts[3] = new Point(ClientRect.Left, ClientRect.Bottom);
                    GraphicsPath path = new GraphicsPath();
                    path.AddLines(pts);
                    e.Graphics.FillPath(CellPreset.Brush, path);

                }
                else if (CellPreset.Brush is LinearGradientBrush)
                {
                    LinearGradientBrush linearBr = (LinearGradientBrush)CellPreset.Brush; 
                    e.Graphics.FillRectangle(linearBr, ClientRect);
                }
                else
                    e.Graphics.FillRectangle(CellPreset.Brush, ClientRect);
            }
            if (Selected)
                e.Graphics.DrawRectangle(pen, ClientRect.X, ClientRect.Y, ClientRect.Width, ClientRect.Height);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
                Selected = true;
            Invalidate();
        }



        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (EventMenuDelete != null && Selected)
                EventMenuDelete(this);
        }


        private void toolTip1_Draw(object sender, DrawToolTipEventArgs e)
        {

    /*        e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds);
            e.Graphics.DrawRectangle(Pens.DarkGray, e.Bounds);
            if (!string.IsNullOrEmpty(CellPreset.Text))
            {
                e.Graphics.DrawString(CellPreset.Text, DefaultFont, Brushes.Black, e.Bounds);
            }
            else
            {
                e.Graphics.DrawString(toolTip1.GetToolTip(this), DefaultFont, Brushes.Black, e.Bounds);
            }*/
        }

        private void CellPresetUserControl_MouseEnter(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(CellPreset.Text))
            {
                str = CellPreset.Text;
            }
            else
            {
                str = CellPreset.Brush.ToString();
                if (CellPreset.Brush is HatchBrush)
                {
                    HatchBrush hb = ((HatchBrush)CellPreset.Brush);
                    str = Enum.GetName(typeof(HatchStyle), hb.HatchStyle);
                }
            }
 //           toolTip1.SetToolTip(this, str);
        }
    }
}
