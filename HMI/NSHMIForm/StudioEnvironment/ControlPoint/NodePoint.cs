using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using NetSCADA6.NSInterface.HMI.DrawObj;

namespace NetSCADA6.HMI.NSHMIForm
{
	/// <summary>
	/// 节点控制点类
	/// </summary>
	internal class NodePoint
	{
		public NodePoint(SelectObjectManager objects)
		{
			_objects = objects;
		}
		
		#region field
		// 数据数组
		private List<PointF> _datas;
		// 路径数组
		private GraphicsPath[] _paths;
		private readonly SelectObjectManager _objects;
		#endregion

		#region private function
		private bool IsVisible(PointF point, ref ControlState state, ref int index, bool isDeleteNode = false)
		{
			if (_datas == null)
				return false;

			int len = _datas.Count;
			for (int i = 0; i < len; i++)
			{
				if (_paths[i].IsVisible(point))
				{
					state = isDeleteNode ? ControlState.DeleteNode : ControlState.MoveNode;
					index = i;
					return true;
				}
			}

			return false;
		}
		#endregion

		#region public function
		public bool IsVisible(PointF point)
		{
			if (_datas == null)
				return false;

			int count = _datas.Count;
			for (int i = 0; i < count; i++)
			{
				if (_paths[i].IsVisible(point))
					return true;
			}

			return false;
		}

		public bool CanOperate(PointF pf, ref ControlState state, ref int index)
		{
			if (_objects.IsAddNode)
			{
				state = ControlState.AddNode;
				return true;
			}

			if (IsVisible(pf, ref state, ref index, _objects.IsDeleteNode))
				return true;
			if (_objects.IsVisible(pf))
			{
				state = ControlState.Move;
				return true;
			}

			return false;
		}
		public void Draw(Graphics g)
		{
			if (_datas == null)
				return;

			//注意：len必须使用Datas，Paths的len可能比Datas的要大
			int len = _datas.Count;
			for (int i = 0; i < len; i++)
			{
				g.FillPath(Brushes.WhiteSmoke, _paths[i]);
				g.DrawPath(Pens.Black, _paths[i]);
			}
		}
		#endregion

		#region calculate
		public void Calculate(float scale, ref RectangleF invalidateRect)
		{
			GenerateData(scale);
			GeneratePath();
			GenerateRect(ref invalidateRect);
		}
		// 设置数据
		private void GenerateData(float scale)
		{
			_datas = new List<PointF>(_objects.Node.NodeDatas);

			int len = _datas.Count;
			for (int i = 0; i < len; i++)
				_datas[i] = ControlPointContainer.TransFormData(_objects.Matrix, scale, _datas[i]);
		}
		// 设置路径
		private void GeneratePath()
		{
			if (_datas == null)
				return;

			const float size = ControlPointContainer.PointSize;
			int len = _datas.Count;
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

			PointF center;
			for (int i = 0; i < len; i++)
			{
				center = new PointF(_datas[i].X - size / 2, _datas[i].Y - size / 2);
				_paths[i].Reset();
				_paths[i].AddEllipse(new RectangleF(center.X, center.Y, size, size));
			}
		}
		// 设置区域
		private void GenerateRect(ref RectangleF invalidateRect)
		{
			if (_datas == null)
				return;
			const float size = ControlPointContainer.PointSize;
			RectangleF rf = RectangleF.Empty;
			for (int i = 0; i < _datas.Count; i++ )
				rf = (rf == RectangleF.Empty) 
					? new RectangleF(_datas[i].X - size / 2f, _datas[i].Y - size / 2f, size, size) : RectangleF.Union(rf, new RectangleF(_datas[i].X - size / 2f, _datas[i].Y - size / 2f, size, size));

			invalidateRect = (invalidateRect == RectangleF.Empty) ? rf : RectangleF.Union(invalidateRect, rf);
		}
		#endregion

	}
}
