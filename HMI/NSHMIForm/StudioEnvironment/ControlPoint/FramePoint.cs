using System.Drawing;
using System.Drawing.Drawing2D;
using NetSCADA6.NSInterface.HMI.DrawObj;

namespace NetSCADA6.HMI.NSHMIForm
{
	internal class FramePoint
	{
		public FramePoint(SelectObjectManager objects)
		{
			_objects = objects;

			for (int i = 0; i < FrameCount; i++)
				_framePath[i] = new GraphicsPath();
			for (int i = 0; i < RotateCount; i++)
				_rotatePath[i] = new GraphicsPath();
			for (int i = 0; i < ShearCount; i++)
				_shearPath[i] = new GraphicsPath();
		}

		#region const
		// 边框控制点数目
		private const int FrameCount = 8;
		// 旋转控制点数目
		private const int RotateCount = 4;
		// 倾斜控制点数目
		private const int ShearCount = 2;
		private const float FrameSize = ControlPointContainer.PointSize * 1.2f;
		private readonly RectangleF _pointRect = new RectangleF(0, 0, FrameSize, FrameSize);
		#endregion

		#region field
		// 边框控制点路径
		private readonly GraphicsPath[] _framePath = new GraphicsPath[FrameCount];
		private readonly GraphicsPath _rotateCenterPath = new GraphicsPath();
		private readonly GraphicsPath[] _rotatePath = new GraphicsPath[RotateCount];
		private readonly GraphicsPath[] _shearPath = new GraphicsPath[ShearCount];
		//边框线点
		private readonly PointF[] _framePoint = new PointF[5];
		private readonly SelectObjectManager _objects;
		#endregion

		#region calculate
		public void Calculate(float scale, ref RectangleF invalidateRect, ref RectangleF rotateRect)
		{
			GenerateData(scale);
			GeneratePath();
			GenerateRect(ref invalidateRect, ref rotateRect);
		}
		private PointF _rotatePointPos;
		// 边框控制点坐标数组
		private readonly PointF[] _frameArray = new PointF[FrameCount];
		// 设置坐标
		private void GenerateData(float scale)
		{
			PointF[] pfs = _frameArray;

			if (_objects.IsEmpty)
			{
				for (int i = 0; i < pfs.Length; i++)
					pfs[i] = PointF.Empty;
			}
			else
			{
				float size = FrameSize / scale;
				RectangleF rf = _objects.Rect;

				for (int i = 0; i < pfs.Length; i++)
				{
					int j = (i < 4) ? i : i + 1;
					int row = j / 3;
					int col = j % 3;

					pfs[i].X = rf.X + rf.Width / 2 * col + size * (col - 1);
					pfs[i].Y = rf.Y + rf.Height / 2 * row + size * (row - 1);

					pfs[i] = ControlPointContainer.TransFormData(_objects.Matrix, scale, pfs[i]);
				}
			}
			//rotate center
			_rotatePointPos = _objects.RotatePointPos;
			_rotatePointPos.X *= scale;
			_rotatePointPos.Y *= scale;
		}
		private void GeneratePath()
		{
			//frame path
			RectangleF rect = _pointRect;
			GraphicsPath path;
			int sCount = 0;
			int rCount = 0;
			int fCount = 0;
			for (int i = 0; i < FrameCount; i++)
			{
				path = _framePath[i];
				path.Reset();

				//frame
				rect.X = _frameArray[i].X - FrameSize / 2;
				rect.Y = _frameArray[i].Y - FrameSize / 2;
				path.AddRectangle(rect);

				switch (i)
				{
					//rotate
					case 0:
					case 2:
					case 5:
					case 7:
						path = _rotatePath[rCount++];
						_framePoint[fCount++] = _frameArray[i];
						break;
					//shear
					case 1:
					case 6:
						path = _shearPath[sCount++];
						break;
					default:
						path = null;
						break;
				}

				if (path != null)
				{
					path.Reset();
					path.AddRectangle(new RectangleF(_frameArray[i].X - FrameSize, _frameArray[i].Y - FrameSize, FrameSize*2, FrameSize*2));
				}

			}
			//改变矩形框点位置，方便画图
			PointF p = _framePoint[3];
			_framePoint[3] = _framePoint[2];
			_framePoint[2] = p;
			_framePoint[4] = _framePoint[0];

			//rotate center
			_rotateCenterPath.Reset();
			if (_objects.IsVector)
			{
				_rotateCenterPath.AddEllipse(_rotatePointPos.X - FrameSize * 0.5f, _rotatePointPos.Y - FrameSize * 0.5f,
					FrameSize, FrameSize);
			}
		}
		private void GenerateRect(ref RectangleF invalidateRect, ref RectangleF rotateRect)
		{
			if (_objects.IsEmpty)
				return;

			RectangleF rf = RectangleF.Empty;
			//frame
			for (int i = 0; i < FrameCount; i++)
				rf = (rf == RectangleF.Empty) 
					? _framePath[i].GetBounds() : RectangleF.Union(rf, _framePath[i].GetBounds());
			invalidateRect = (invalidateRect == RectangleF.Empty) 
				? rf : RectangleF.Union(invalidateRect, rf);

			//rotate center))
			if (_objects.IsVector)
			{
				rf = _rotateCenterPath.GetBounds();
				if (!invalidateRect.Contains(rf))
					rotateRect = rf;
			}
		}
		#endregion

		#region public function
		public bool CanOperate(PointF pf, ref ControlState state, ref int index)
		{
			if (_rotateCenterPath.IsVisible(pf))
			{
				state = ControlState.Center;
				return true;
			}

			for (int i = 0; i < _framePath.Length; i++)
			{
				if (_framePath[i].IsVisible(pf))
				{
					state = ControlState.FrameMove;
					index = i;
					return true;
				}
			}
			if (_objects.IsVector)
			{
				for (int i = 0; i < _rotatePath.Length; i++)
				{
					if (_rotatePath[i].IsVisible(pf))
					{
						state = ControlState.Rotate;
						index = i;
						return true;
					}
				}
				if (_objects.IsSingleVector)
				{
					for (int i = 0; i < _shearPath.Length; i++)
					{
						if (_shearPath[i].IsVisible(pf))
						{
							state = ControlState.Shear;
							index = i;
							return true;
						}
					}
					
				}
			}

			return false;
		}
		public void Draw(Graphics g)
		{
			if (_objects.IsEmpty)
				return;

			//draw frame line
			Pen line = new Pen(Color.DarkViolet) { DashStyle = DashStyle.Dash };
			g.DrawLines(line, _framePoint);

			//draw frame rect;
			for (int i = 0; i < FrameCount; i++)
			{
				g.FillPath(Brushes.Black, _framePath[i]);
				if (_objects.IsSingleVector && (i == 1 || i == 6))       //shear
					g.DrawPath(Pens.Yellow, _framePath[i]);
				else												//normal
					g.DrawPath(Pens.CadetBlue, _framePath[i]);
			}

			//draw rotate center
			g.DrawPath(Pens.Blue, _rotateCenterPath);
			RectangleF rf = _rotateCenterPath.GetBounds();
			rf.Inflate(-1, -1);
			g.DrawEllipse(Pens.White, rf);
		}
		#endregion
	}
}
