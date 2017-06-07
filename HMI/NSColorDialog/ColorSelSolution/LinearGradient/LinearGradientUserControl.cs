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
    internal partial class LinearGradientUserControl : UserControl
    {
        public LinearGradientUserControl()
        {
            InitializeComponent();
            BaseGradientUserControl1.SolidUserCtrl = SolidBrushUserControl1;
            BaseGradientUserControl1.ColorBlendChanged += EventLinearGradientBrushChanged;
            HScrollBarUserControl1.valueChange += EventLinearGradientBrushChanged;
        }

        public NSLinearGradientBrushInfo LinearGradientBrushInfo
        {
            get
            {
                return new NSLinearGradientBrushInfo(BaseGradientUserControl1.ColorBlendEx.GetData(), HScrollBarUserControl1.Value);
            }
            set
            {
                if (value != null)
                {
                    BaseGradientUserControl1.ColorBlendEx.Reset(value.ColorBlend, BaseGradientUserControl1.ClientRect);
                    BaseGradientUserControl1.ColorBlendEx.DataList[0].Selected = true;
                    SolidBrushUserControl1.color = value.ColorBlend.Colors[0];
                    HScrollBarUserControl1.Value = value.Angle;
                    BaseGradientUserControl1.Invalidate();
                }
            }
        }

        public bool AngleVisbale
        {
            get
            {
                return HScrollBarUserControl1.Visible;
            }
            set
            {
                HScrollBarUserControl1.Visible = value;
            }
        }

        #region 事件
        internal delegate void ValueChange(ColorBlend cb, float Angle);
        internal ValueChange ValueChanged;
        void EventLinearGradientBrushChanged()
        {
            if (ValueChanged != null) //通知外层事件
                ValueChanged(BaseGradientUserControl1.ColorBlendEx.GetData(), HScrollBarUserControl1.Value);
            BaseGradientUserControl1.Invalidate();
        }
        #endregion

        private void button_save_Click(object sender, EventArgs e)
        {

        }


    }
}
