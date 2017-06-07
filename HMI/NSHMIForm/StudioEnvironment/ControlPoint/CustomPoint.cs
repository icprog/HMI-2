using System.Drawing;
using System.Drawing.Drawing2D;
using NetSCADA6.NSInterface.HMI.DrawObj;

namespace NetSCADA6.HMI.NSHMIForm
{
	/// <summary>
	/// 自定义控制点类
	/// </summary>
	internal class CustomPoint
	{
		public CustomPoint(SelectObjectManager objects)
		{
			_objects = objects;
		}

		#region field
		/// <summary>
		/// 数据数组
		/// </summary>
		private PointF[] _datas;
		/// <summary>
		/// 路径数组
		/// </summary>
		private GraphicsPath[] _paths;
		/// <summary>
		/// 中心点
		/// </summary>
		private PointF _center;
		private readonly SelectObjectManager _objects;
		#endregion

		#region public function
		public void Draw(Graphics g)
		{
			if (_datas == null)
				return;

			int len = _datas.Length;
			Pen p = new Pen(Color.Blue) { DashStyle = DashStyle.Dot };
			foreach (PointF pf in _datas)
			{
				g.DrawLine(p, pf, _center);
			}
			for (int i = 0; i < len; i++)
			{
				g.FillPath(Brushes.Yellow, _paths[i]);
				g.DrawPath(Pens.CadetBlue, _paths[i]);
			}
			p.Dispose();
		}
		public bool CanOperate(PointF point, ref ControlState state, ref int index)
		{
			if (_datas == null)
				return false;

			int len = _datas.Length;
			for (int i = 0; i < len; i++)
			{
				if (_paths[i].IsVisible(point))
				{
					state = ControlState.Custom;
					index = i;
					return true;
				}
			}

			return false;
		}
		#endregion

		#region claculate
		public void Calculate(float scale, ref RectangleF invalidateRect)
		{
			GenerateData(scale);
			GeneratePath();
			GenerateRect(ref invalidateRect);
		}
		private void GenerateData(float scale)
		{
			_datas = (PointF[])_objects.Custom.CustomDatas.Clone();
			for (int i = 0; i < _datas.Length; i++)
			{
				_datas[i] = ControlPointContainer.TransFormData(_objects.Matrix, scale, _datas[i]);
			}

			_center = _objects.Custom.CustomCenter;
			_center = ControlPointContainer.TransFormData(_objects.Matrix, scale, _center);
		}
		public void GeneratePath()
		{
			if (_datas == null)
				return;

			const float size = ControlPointContainer.PointSize;
			int len = _datas.Length;
			//init Paths
			//没有，则创建;创建长度不够，重新创建
			if (_paths == null)
			{
				_paths = new GraphicsPath[len];
				for (int i = 0; i < len; i++)
					_paths[i] = new GraphicsPath();
			}
			else if (_paths.Length < len)
			{
				foreach (GraphicsPath path in _paths)
				{
					path.Dispose();
				}

				_paths = new GraphicsPath[len];
				for (int i = 0; i < len; i++)
					_paths[i] = new GraphicsPath();
			}

			PointF rf;
			Matrix m = new Matrix();
			for (int i = 0; i < len; i++)
			{
				rf = new PointF(_datas[i].X - size / 2, _datas[i].Y - size / 2);
				_paths[i].Reset();
				_paths[i].AddRectangle(new RectangleF(rf.X, rf.Y, size, size));
				m.Reset();
				m.RotateAt(45, _datas[i]);
				_paths[i].Transform(m);
			}
			m.Dispose();
		}
		public void GenerateRect(ref RectangleF invalidateRect)
		{
			if (_datas == null)
				return;

			int len = _datas.Length;
			RectangleF rf = RectangleF.Empty;
			for (int i = 0; i < len; i++)
				rf = (rf == RectangleF.Empty) 
					? _paths[i].GetBounds() : RectangleF.Union(rf, _paths[i].GetBounds());

			invalidateRect = (invalidateRect == RectangleF.Empty) 
				? rf : RectangleF.Union(invalidateRect, rf);
		}
		#endregion
	}
}
