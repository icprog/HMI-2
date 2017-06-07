using System.Drawing;
using System.Linq;

namespace NetSCADA6.HMI.NSHMIForm
{
	/// <summary>
	/// 切割控件控制类
	/// </summary>
	internal class SegmentPoint
	{
		public SegmentPoint(SelectObjectManager objects)
		{
			_objects = objects;
		}

		#region field
		private readonly SelectObjectManager _objects;
		private PointF[] _intersections;
		#endregion

		#region calculate
		public void Calculate(float scale, ref RectangleF invalidateRect)
		{
			GenerateData(scale);
			GenerateRect(ref invalidateRect);
		}
		private void GenerateData(float scale)
		{
			if (_objects.Segment.Intersections == null)
			{
				_intersections = null;
				return;
			}
			
			_intersections = (PointF[])_objects.Segment.Intersections.Clone();
			for (int i = 0; i < _intersections.Length; i++)
			{
				_intersections[i] = ControlPointContainer.TransFormData(_objects.Matrix, scale, _intersections[i]);
			}
		}
		private void GenerateRect(ref RectangleF invalidateRect)
		{
			if (_objects.IsEmpty || _intersections == null)
				return;

			const float size = ControlPointContainer.PointSize;
			RectangleF rf = RectangleF.Empty;
			for (int i = 0; i < _intersections.Length; i++)
			{
				RectangleF rect = new RectangleF(_intersections[i].X - size/2, _intersections[i].Y - size/2, size, size);
				rf = (rf == RectangleF.Empty) ? rect : RectangleF.Union(rf, rect);
			}

			invalidateRect = (invalidateRect == RectangleF.Empty) 
				? rf : RectangleF.Union(invalidateRect, rf);
		}
		#endregion

		#region public function
		public void Draw(Graphics g)
		{
			if (_intersections == null)
				return;
			
			//intersctions
			const float size = ControlPointContainer.PointSize;
			foreach (PointF pf in _intersections)
			{
				g.FillEllipse(Brushes.GreenYellow, pf.X - size/2, pf.Y - size/2, size, size);
				g.DrawEllipse(Pens.Black, pf.X - size / 2, pf.Y - size / 2, size, size);
			}
		}
		#endregion

	}
}
