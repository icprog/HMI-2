using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using NetSCADA6.NSInterface.HMI.DrawObj;

namespace NetSCADA6.HMI.NSDrawObj
{
	public partial class DrawCombine : INodeEdit
	{
		#region field
		//操作点数据类
		private struct PointData
		{
			public PointData(PointF point, int index)
			{
				Point = point;
				Index = index;
			}

			public readonly PointF Point;
			public readonly int Index;
		}
		//极坐标数据类
		private struct PolarData
		{
			public PolarData(PointF beginPoint, PointF endPoint)
			{
				float x = endPoint.X - beginPoint.X;
				float y = endPoint.Y - beginPoint.Y;
				Length = (float)Math.Sqrt(x * x + y * y);
				Radian = (float)Math.Atan2(y, x);
			}

			public float Length;
			public float Radian;

			public void Offset(float rate, float radianOff)
			{
				Length *= rate;
				Radian += radianOff;
			}
			public PointF GetPointF(PointF origin)
			{
				float x = (float)(Length * Math.Cos(Radian));
				float y = (float)(Length * Math.Sin(Radian));

				return new PointF(origin.X + x, origin.Y + y);
			}
		}
		#endregion

		#region property
		private NodesState _nodeState;
		public NodesState NodeState
		{
			get { return _nodeState; }
			set { _nodeState = value; }
		}
		public bool IsNodeCreating
		{
			get { return true; }
			set {  }
		}
		public bool IsNodeOperate
		{
			get { return true; }
		}
		public bool CreateSuccess
		{
			get { return true; }
		}
		public bool CreateFinish
		{
			get { return true; }
		}
		public int MinCount
		{
			get { return 0; }
		}
		private readonly List<PointF> _nodeDatas = new List<PointF>();
		public List<PointF> NodeDatas
		{
			get { return _nodeDatas; }
		}
		#endregion

		#region private function
		//数据只需要_opers就足够了，但是_nodeDatas是对外的接口，所以也需要同步构建
		private readonly List<PointData> _opers = new List<PointData>();
		private void InitializeNode()
		{
			GetOperatePoint(_points, _types, _opers);
			_nodeDatas.Clear();
			foreach (var pd in _opers)
			{
				_nodeDatas.Add(pd.Point);
			}
		}
		private static void GetOperatePoint(PointF[] points, byte[] types, List<PointData> opers)
		{
			opers.Clear();
			PointF[] ps = points;
			byte[] ts = types;
			int bezierCount = 0;

			for (int i = 0; i < ps.Length; i++)
			{
				int type = ts[i] & 0x0F;

				if (type == 0 || type == 1)
					opers.Add(new PointData(ps[i], i));
				else if (type == 3)
				{
					bezierCount++;
					if (bezierCount == 3)
					{
						opers.Add(new PointData(ps[i], i));
						bezierCount = 0;
					}
				}
			}
		}
		private static void ScaleNode(RectangleF current, RectangleF backup, PointF[] datas, PointF[] datasBk)
		{
			if (backup == RectangleF.Empty || datasBk == null || current == backup)
				return;

			float xOff = current.X - backup.X;
			float yOff = current.Y - backup.Y;
			float xScale = current.Width / backup.Width;
			float yScale = current.Height / backup.Height;

			PointF pf = PointF.Empty;
			for (int i = 0; i < datas.Length; i++)
			{
				pf.X = backup.X + (datasBk[i].X - backup.X) * xScale + xOff;
				pf.Y = backup.Y + (datasBk[i].Y - backup.Y) * yScale + yOff;
				datas[i] = pf;
			}
		}
		private static void OffsetDatas(PointF[] datas, PointF offset)
		{
			SizeF sf = new SizeF(offset);
			int count = datas.Length;
			for (int i = 0; i < count; i++)
				datas[i] = PointF.Add(datas[i], sf);
		}
		private static void MoveBezierPoint(PointF p0, ref PointF p1, ref PointF p2, PointF p3,
			PointF pNew, bool isBeginMove)
		{
			PointF fixPoint = isBeginMove ? p3 : p0;
			PointF movePoint = isBeginMove ? p0 : p3;
			PolarData originPolar = new PolarData(fixPoint, movePoint);
			PolarData modifyPolar = new PolarData(fixPoint, pNew);

			float rate = modifyPolar.Length / originPolar.Length;
			float radianOff = modifyPolar.Radian - originPolar.Radian;

			PolarData polar1 = new PolarData(fixPoint, p1);
			polar1.Offset(rate, radianOff);
			p1 = polar1.GetPointF(fixPoint);
			PolarData polar2 = new PolarData(fixPoint, p2);
			polar2.Offset(rate, radianOff);
			p2 = polar2.GetPointF(fixPoint);
		}
		private PointF CalculateNodeOffset(PointF[] newPoints)
		{
			//Rect可能发生变化，需要计算偏移
			GraphicsPath path = new GraphicsPath(newPoints, _types);
			RectangleF rf = path.GetBounds();
			path.Dispose();

			NewRect = rf;

			PointF p1 = Calculation.CalcMatrixPoint(DataBk, PointF.Empty);
			PointF p2 = Calculation.CalcMatrixPoint(this, PointF.Empty);

			return new PointF(p1.X - p2.X, p1.Y - p2.Y);
		}
		//删除单个点
		private static void RemoveSinglePoint(ref List<PointF> ps, ref List<byte> ts)
		{
			for (int i = 0; i < ts.Count - 1; i++)
			{
				int type = ts[i];
				int nextType = ts[i + 1];
				if (type == 0 && nextType == 0)
				{
					ps.RemoveAt(i);
					ts.RemoveAt(i);
				}
			}
			//last
			int last = ts.Count - 1;
			if (last > 0 && ts[last] == 0)
			{
				ps.RemoveAt(last);
				ts.RemoveAt(last);
			}
		}
		private static bool IsValidPath(PointF[] ps, byte[] ts)
		{
			GraphicsPath path = new GraphicsPath(ps, ts);
			RectangleF rf = path.GetBounds();
			return (rf.Width != 0 || rf.Height != 0);
		}
		#endregion

		#region public function
		public void MoveNode(PointF point, int pos)
		{
			PointF p = Calculation.GetInvertPos(DataBk.Matrix, point);
			PointF mouse = Calculation.GetInvertPos(DataBk.Matrix, DataBk.MousePos);
			PointF off = new PointF(p.X - mouse.X, p.Y - mouse.Y);

			int index = _opers[pos].Index;
			PointF[] ps = (PointF[])_pointsBk.Clone();
			byte[] ts = _types;
			int sign = ts[index] & 0x0F;
			PointF value = ps[index];
			int nextSign = -1;
			if (index + 1 < ps.Length)
				nextSign = ts[index + 1] & 0x0F;
			int preSign = -1;
			if (index - 1 > 0)
				preSign = ts[index - 1] & 0x0F;

			value.X += off.X;
			value.Y += off.Y;

			switch (sign)
			{
				case 0x00:
				case 0x01:
					if (nextSign == 3)
						MoveBezierPoint(ps[index], ref ps[index + 1], ref ps[index + 2], ps[index + 3],
										value, true);
					break;
				case 0x03:
					if (preSign == 3)
						MoveBezierPoint(ps[index - 3], ref ps[index - 2], ref ps[index - 1], ps[index],
										value, false);
					if (nextSign == 3)
						MoveBezierPoint(ps[index], ref ps[index + 1], ref ps[index + 2], ps[index + 3],
										value, true);
					break;
			}
			ps[index] = value;
			OffsetDatas(ps, CalculateNodeOffset(ps));

			_points = (PointF[])ps.Clone();
			InitializeNode();
			LoadGeneratePathEvent();
		}
		public void AddNode(PointF point)
		{
		}
		public void DeleteNode(int pos)
		{
			//不要全部删除
			if (_nodeDatas.Count <= 2)
				return;

			int index = _opers[pos].Index;
			List<PointF> ps = new List<PointF>(_points);
			List<byte> ts = new List<byte>(_types);

			int sign = ts[index] & 0x0F;
			bool isClose = (ts[index] & 0xF0) == 0x80;
			int nextSign = -1;
			if (index + 1 < ts.Count)
				nextSign = ts[index + 1] & 0x0F;

			if (sign == 0)
			{
				ps.RemoveAt(index);
				ts.RemoveAt(index);

				if (nextSign == 3)
				{
					ps.RemoveRange(index, 2);
					ts.RemoveRange(index, 2);
				}
				if (index < ts.Count)
					ts[index] = 0;
			}
			else if (sign == 1)
			{
				ps.RemoveAt(index);
				ts.RemoveAt(index);

				if (nextSign == 3)
				{
					ps.RemoveRange(index, 2);
					ts.RemoveRange(index, 2);
					if (index < ts.Count)
						ts[index] = 1;
				}

				if (isClose && (index < ts.Count) && ts[index] != 0)
					ts[index] &= 0x80;
			}
			else if (sign == 3)
			{
				ps.RemoveRange(index - 2, 3);
				ts.RemoveRange(index - 2, 3);
				
				if (nextSign == 3)
				{
					ps.RemoveRange(index - 2, 2);
					ts.RemoveRange(index - 2, 2);
					if (index - 2 < ts.Count)
						ts[index - 2] = 1;
				}

				if (isClose && (index - 2 < ts.Count) && ts[index - 2] != 0)
					ts[index - 2] &= 0x80;
			}

			RemoveSinglePoint(ref ps, ref ts);
			PointF[] pa = ps.ToArray();
			byte[] ta = ts.ToArray();
			//如果是无效的路径，不操作
			if (!IsValidPath(pa, ta))
				return;

			_points = pa;
			_types = ta;
			InitializeNode();
			LoadGeneratePathEvent();
		}
		public void CreatingPaint(Graphics g)
		{
		}
		#endregion
	}
}
