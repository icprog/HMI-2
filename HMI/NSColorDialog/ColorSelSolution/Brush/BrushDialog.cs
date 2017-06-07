using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetSCADA6.Common.NSColorManger
{
    public partial class BrushDialog : Form
    {
        #region init\ok\cancel
        public BrushDialog(BrushData brushData)
        {
            InitializeComponent();
            UserControl1.brushData = (BrushData)brushData.Clone();
            UserControl1.BrushChanged += BrushDataChanged;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            panelRf = new RectangleF(panel1.Location.X, panel1.Location.Y, panel1.Width, panel1.Height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Brush br = UserControl1.brushData.CreateExampleBrush(panelRf);
            if (br != null)
            {
                e.Graphics.FillRectangle(br, panelRf);
                br.Dispose();
            }
        }
        #endregion
        #region public
        public BrushData BrushData
        {
            get
            {
                return UserControl1.brushData;
            }
        }

        RectangleF panelRf;
        void BrushDataChanged()
        {

            Invalidate();
        }
        #endregion

        private void button_ok_Click(object sender, EventArgs e)
        {

        }
    }
}
