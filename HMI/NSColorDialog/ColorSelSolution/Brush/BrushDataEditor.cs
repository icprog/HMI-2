using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using NetSCADA6.Common.NSColorManger;

namespace NetSCADA6.Common.NSColorManger
{
    /// <summary>
    /// 填充编辑器
    /// </summary>
    public class BrushDataEditor : UITypeEditor
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
                BrushData bd = (value as ICloneable).Clone() as BrushData;
                BrushDialog dlg = new BrushDialog(bd);
                if (dlg.ShowDialog() == DialogResult.OK)
                    value = dlg.BrushData;
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
            Brush brush = (e.Value as BrushData).CreateBrush(rect, null);
            if ((e.Value as BrushData).BrushType != NSBrushType.Textrue)
                e.Graphics.FillRectangle(brush, rect);
            else
            {
                e.Graphics.DrawImage((brush as TextureBrush).Image, rect);
            }
            brush.Dispose();
        }


    }

}
