using System.Drawing;

using NetSCADA6.HMI.NSDrawObj;
using NetSCADA6.NSInterface.HMI.DrawObj;

namespace NetSCADA6.HMI.NSDrawVector
{
	public partial class DrawRect : ICustomEdit
	{
		#region property
		private PointF[] _customDatas = new PointF[2];
		public PointF[] CustomDatas
		{
			get { return _customDatas; }
		}
		private PointF _customCenter;
		public PointF CustomCenter { get { return _customCenter; } }
		#endregion

		public void GenerateCustom()
		{
			PointF[] datas = CustomDatas;
			RectangleF rf = Rect;
			float x = XRound;
			float y = YRound;

			datas[0] = new PointF(rf.X + x, rf.Y);
			datas[1] = new PointF(rf.X, rf.Y + y);

			_customCenter = new PointF(rf.X + x, rf.Y + y);
		}
		private void OnMouseMove(PointF offset, int pos)
		{
			float x = XRoundBk;
			float y = YRoundBk;

			if (pos == 0)
				SetRound(x + offset.X, true);
			else
				SetRound(y + offset.Y, false);
		}

		public void MoveCustom(PointF point, int pos)
		{
			PointF p = Calculation.GetInvertPos(DataBk.Matrix, point);
			PointF mouse = Calculation.GetInvertPos(DataBk.Matrix, DataBk.MousePos);
			PointF off = new PointF(p.X - mouse.X, p.Y - mouse.Y);

			OnMouseMove(off, pos);
		}

	}
}
