using System;
using System.Drawing;
using NetSCADA6.NSInterface.HMI.DrawObj;

namespace NetSCADA6.HMI.NSHMIForm
{
	internal class FormPoint
	{
		public FormPoint(HMIForm container)
		{
			_container = container;
		}

		#region field
		private readonly HMIForm _container;
		private PointF _downMousePos;
		private Rectangle _downRect;
		#endregion

		#region public function
		public bool CanOperate(PointF point, ref ControlState state, ref int index)
		{
			const int interval = 5;
			Rectangle rect = ((Studio)_container.Studio).Rect;
			if (Math.Abs(point.Y - rect.Height) <= interval)
			{
				state = ControlState.FormHeight;
				return true;
			}
			if (Math.Abs(point.X - rect.Width) <= interval)
			{
				state = ControlState.FormWidth;
				return true;
			}
			return false;
		}
		public void MouseDown(PointF point)
		{
			_downMousePos = point;
			_downRect = ((Studio)_container.Studio).Rect;
		}
		public void ChangeFormSize(PointF point, ControlState state)
		{
			Rectangle rect = _downRect;

			switch (state)
			{
				case ControlState.FormWidth:
					rect.Width += (int) (point.X - _downMousePos.X);
					break;
				case ControlState.FormHeight:
					rect.Height += (int)(point.Y - _downMousePos.Y);
					break;
				default:
					break;
			}
			_container.Rect = rect;

		}
		#endregion
	}
}
