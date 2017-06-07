// by hdp 2013.09.04

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using NetSCADA6.NSInterface.HMI.DrawObj;

namespace NetSCADA6.HMI.NSDrawObj
{
	/// <summary>
	/// 路径分割
	/// </summary>
	public partial class DrawCombine : ISegmentEdit
	{
		#region field
		private const int Invalid = -1;
		//分割完成的路径
		private readonly List<GraphicsPath> _paths = new List<GraphicsPath>();
		private GraphicsPath _selectedPath;
		#endregion

		#region property
		private PointF[] _intersections;
		/// <summary>
		/// 交点
		/// </summary>
		public PointF[] Intersections { get { return _intersections; } }
		#endregion

		#region private function
		private void Reset()
		{
			_selectedPath = null;
			foreach (var p in _paths)
			{
				p.Dispose();
			}
			_paths.Clear();
		}
		private GraphicsPath FindPath(PointF point)
		{
			//only 1 path,no operate;avoid empty paths
			if (_paths.Count < 2)	
				return null;

			Pen pen = GetOutlinePen();
			return _paths.FirstOrDefault(p => p.IsOutlineVisible(point, pen));
		}
		private void DeletePath(PointF point)
		{
			_selectedPath = FindPath(point);
			if (_selectedPath != null)
			{
				_paths.Remove(_selectedPath);
				_selectedPath.Dispose();
				_selectedPath = null;

				GenerateCombinePath();
			}
		}
		/// <summary>
		/// 生成新路径
		/// </summary>
		private void GenerateCombinePath()
		{
			IEnumerable<GraphicsPath> list = DividePath.ConnectPath(_paths);
			GraphicsPath path = new GraphicsPath();

			foreach (var p in list)
			{
				path.AddPath(p, false);
			}
			_points = (PointF[])path.PathPoints.Clone();
			_types = (byte[])path.PathTypes.Clone();
			LoadGeneratePathEvent();
		}
		private void DrawSegment(Graphics g)
		{
			//select path
			GraphicsPath path = _selectedPath;
			float width = Pen.Data.Width;
			if (width < 3)
				width = 3;
			Pen pen = new Pen(Color.FromArgb(150, 255, 0, 0), width);
			if (path != null)
				g.DrawPath(pen, path);
		}
		#endregion

		#region public function
		private void InitializeSegment()
		{
			Reset();

			_intersections = CGAL.Intersection.Calculate(_points, _types);
			int len = _points.Length;
			int begin = Invalid;
			if (len < 2)
				return;

			for (int i = 0; i < len; i++)
			{
				if (_types[i] != 0)
					continue;
				//有起始点，起始点和当前点间间距为2以上才需要生成路径
				if (begin != Invalid && (i - begin) >= 2)
					DividePath.GenerateDividePath(_paths, _intersections, _points, _types, begin, i - begin);

				begin = i;
			}
			//last point
			if (_types[len - 1] != 0 && begin != Invalid && (len - begin) >= 2)
				DividePath.GenerateDividePath(_paths, _intersections, _points, _types, begin, len - begin);
		}
		#endregion


		#region virtual function
		private void SegmentMouseMove(PointF pf)
		{
			GraphicsPath path = FindPath(pf);
			if (_selectedPath != path)
			{
				_selectedPath = path;
				Invalidate();
			}
		}
		private void SegmentMouseDown(PointF pf)
		{
			DeletePath(pf);
		}
		private void SegmentMouseLeave()
		{
			_selectedPath = null;
			Invalidate();
		}
		#endregion


	}
}
