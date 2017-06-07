using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

using NetSCADA6.NSInterface.HMI.DrawObj;

namespace NetSCADA6.HMI.NSDrawObj
{
	public partial class DrawCombine : DrawVector
	{
		public DrawCombine(IEnumerable<IDrawObj> objs = null)
		{
			if (objs == null)
				return;
			GraphicsPath path = new GraphicsPath();
			foreach (var obj in objs.Where(obj => obj.IsVector && ((DrawVector) obj).CanCombine))
			{
				path.AddPath(obj.Path, false);
			}
			_points = (PointF[])path.PathPoints.Clone();
			_types = (byte[])path.PathTypes.Clone();
		}
		
		#region field
		//数据
		private PointF[] _points;
		private byte[] _types;
		#endregion

		#region property
		public override DrawType Type { get { return DrawType.Combine; } }
		public override EditMode EditMode
		{
			set
			{
				Invalidate();
				base.EditMode = value;
				switch (EditMode)
				{
					case EditMode.Normal:
						break;
					case EditMode.Segment:
						InitializeSegment();
						break;
					case EditMode.Node:
						_nodeState = NodesState.Move;
						InitializeNode();
						break;
					default:
						break;
				}
				Invalidate();
			}
		}
		#endregion

		#region virtual function
		protected override void OnGeneratePath(ref GraphicsPath path)
		{
			if (EditMode == EditMode.Normal)
				ScaleNode(Rect, DataBk.Rect, _points, _pointsBk);
			BasePath.AddPath(new GraphicsPath(_points, _types), false);
			SetDirectRect(BasePath.GetBounds());
		}
		private PointF[] _pointsBk;
		protected internal override void BackupData()
		{
			base.BackupData();
			_pointsBk = (PointF[])_points.Clone();
		}
		protected override void OnPaint(Graphics g)
		{
			base.OnPaint(g);

			if (EditMode == EditMode.Segment)
				DrawSegment(g);
		}
		public override bool IsVisible(PointF point)
		{
			bool result = base.IsVisible(point);

			if (!result && Bound.Contains(point))
			{
				Pen p = GetOutlinePen();
				return Path.IsOutlineVisible(point, p);
			}

			return result;
		}

		#region serialize
		public override void Serialize(BinaryFormatter bf, Stream s)
		{
			base.Serialize(bf, s);

			const int version = 1;

			bf.Serialize(s, version);
			bf.Serialize(s, _points);
			bf.Serialize(s, _types);
		}
		public override void Deserialize(BinaryFormatter bf, Stream s)
		{
			base.Deserialize(bf, s);

			int version = (int)bf.Deserialize(s);
			_points = (PointF[])bf.Deserialize(s);
			_types = (byte[])bf.Deserialize(s);
		}
		#endregion

		#region clone
		public override object Clone()
		{
			var obj =  (DrawCombine)base.Clone();
			obj._points = (PointF[])_points.Clone();
			obj._types = (byte[])_types.Clone();

			return obj;
		}
		#endregion

		#region mouse
		protected override void OnMouseMove(MouseButtons button, float x, float y)
		{
			PointF pf = new PointF(Rect.X + x, Rect.Y + y);
			switch (EditMode)
			{
				case EditMode.Segment:
					SegmentMouseMove(pf);
					break;
			}
		}
		protected override void OnMouseDown(MouseButtons button, float x, float y)
		{
			PointF pf = new PointF(Rect.X + x, Rect.Y + y);
			switch (EditMode)
			{
				case EditMode.Segment:
					SegmentMouseDown(pf);
					break;
			}
		}
		protected override void OnMouseLeave()
		{
			switch (EditMode)
			{
				case EditMode.Segment:
					SegmentMouseLeave();
					break;
			}
		}
		#endregion
		#endregion
	}
}
