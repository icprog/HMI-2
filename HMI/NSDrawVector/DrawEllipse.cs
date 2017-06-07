using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using NetSCADA6.HMI.NSDrawObj;
using NetSCADA6.NSInterface.HMI.DrawObj;

namespace NetSCADA6.HMI.NSDrawVector
{
    /// <summary>
    /// 圆形控件
    /// </summary>
	[ToolboxBitmap(typeof(DrawRect), "Resources.Rect.bmp")]
	[DisplayName("圆形")]
	[Ortho(OrthoMode.Square)]
	public class DrawEllipse : DrawVector
    {
    	#region protected function
        protected override void OnGeneratePath(ref GraphicsPath path)
        {
            path.AddEllipse(Rect);
        }
        #endregion

        #region serialize
        #endregion

        #region virtual property
        public override DrawType Type { get { return DrawType.Ellipse; } }
        #endregion
    }
}
