using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using NetSCADA6.Common.NSColorManger;
using System.Drawing.Drawing2D;

namespace NetSCADA6.Common.NSColorManger
{
    /// <summary>
    /// 填充编辑器
    /// </summary>
    public class PenDataEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, System.
            IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc =
                (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (edSvc != null)
            {
                PenData pd = (value as ICloneable).Clone() as PenData;
                PenDialog dlg = new PenDialog(pd);
                if (dlg.ShowDialog() == DialogResult.OK)
                    value = dlg.penData;
            }
            return value;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override void PaintValue(PaintValueEventArgs e)
        {
            Rectangle rect = e.Bounds;
            PenData pd = (e.Value as PenData);
            Pen pen = pd.CreatePen(rect, null);
            if (!pd.IsPiple)
                e.Graphics.DrawLine(pen, rect.Left+3, rect.Top + rect.Height / 2, rect.Right-5, rect.Top + rect.Height / 2);
            else
            {
                GraphicsPath path = new GraphicsPath();
                PointF[] pts = new PointF[2];
                pts[0] = new PointF(rect.Left + 3, rect.Top + rect.Height / 2);
                pts[1] = new PointF(rect.Right - 5, rect.Top + rect.Height / 2);
                path.AddLine(pts[0], pts[1]);
                path.CloseAllFigures();
                pd.DrawPath(e.Graphics, path);
                path.Dispose();
            }
            pen.Dispose();
        }
    }

}
