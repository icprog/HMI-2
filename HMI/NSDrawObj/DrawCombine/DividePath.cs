using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace NetSCADA6.HMI.NSDrawObj
{
	/// <summary>
	/// 工具类，用于路径分割和连接
	/// </summary>
	internal static class DividePath
	{
		#region property
		public static readonly Pen OutlinePen = new Pen(Brushes.Black, GraphicsTool.WidenWidth);
		#endregion

		#region private functiong
		private static bool EqualPointF(PointF p1, PointF p2)
		{
			const float epsilon = 0.0001f;
			return Math.Abs(p1.X - p2.X) < epsilon && Math.Abs(p1.Y - p2.Y) < epsilon;
		}
		#endregion

		#region divide path
		private static GraphicsPath BasicDividePath(List<GraphicsPath> paths, IEnumerable<PointF> ins, PointF[] points, byte[] types, int begin, int len)
		{
			//divide figure
			PointF[] ps = new PointF[len];
			byte[] ts = new byte[len];
			Array.Copy(points, begin, ps, 0, len);
			Array.Copy(types, begin, ts, 0, len);
			GraphicsPath path = new GraphicsPath(ps, ts);

			//no interscetion
			if ((ins != null) && ins.Any(t => path.IsOutlineVisible(t, OutlinePen)))
				return path;

			paths.Add(path);
			return null;
		}
		//分割路径
		public static void GenerateDividePath(List<GraphicsPath> paths, PointF[] ins, PointF[] points, byte[] types, int begin, int len)
		{
			//basic generate
			GraphicsPath path = BasicDividePath(paths, ins, points, types, begin, len);
			if (path == null)
				return;
			PointF[] ps = path.PathPoints;
			byte[] ts = path.PathTypes;

			//divide intersection
			bool isFirst = ((ts[ts.Length - 1] & 0xF0) == 0x80 && (len > 2));//judge closed curve
			//closed,len point;not closed,len-1 point
			int count = isFirst ? len : len - 1;
			int bezierCount = 0;
			GraphicsPath calculatePath = new GraphicsPath();
			GraphicsPath dividePath = new GraphicsPath();
			GraphicsPath firstPath = new GraphicsPath();
			List<PointF> crossPoints = new List<PointF>();

			for (int i = 0; i < count; i++)
			{
				calculatePath.Reset();
				crossPoints.Clear();
				int bi = i;
				int ei = i + 1;
				int typeSign;
				if (i == len - 1)
				{
					ei = 0;
					typeSign = 1;
				}
				else
					typeSign = ts[ei] & 0x0F;

				//create calculatePath
				if (typeSign == 1)
					calculatePath.AddLine(ps[bi], ps[ei]);
				else if (typeSign == 3)
				{
					bezierCount++;
					if (bezierCount == 3)		//4 point,first point don't need 3
					{
						calculatePath.AddBezier(ps[i - 2], ps[i - 1], ps[i], ps[i + 1]);
						bezierCount = 0;
					}
					else
						continue;
				}

				PointF beginPoint = calculatePath.PathPoints[0];
				PointF endPoint = calculatePath.PathPoints[calculatePath.PointCount - 1];
				crossPoints.AddRange(ins.Where(t => calculatePath.IsOutlineVisible(t, OutlinePen) || EqualPointF(endPoint, t)));

				if (crossPoints.Count > 0)
				{
					List<float> tValues = CombineFunction.SortCurvePoint(calculatePath, crossPoints);

					//交点和线段前点重合的情况
					if (EqualPointF(beginPoint, crossPoints[0]))
					{
						if (isFirst)
							isFirst = false;
						else if (dividePath.PointCount > 0)
						{
							paths.Add(dividePath);
							dividePath = new GraphicsPath();
						}
					}

					GraphicsPath bezierPath = calculatePath;
					int pCount = crossPoints.Count;
					//n个交点，添加n+1条线段
					for (int j = 0; j <= pCount; j++)
					{
						PointF bp = PointF.Empty;
						PointF ep = PointF.Empty;
						if (j == 0)
						{
							bp = beginPoint;
							ep = crossPoints[j];
							if (EqualPointF(bp, ep))
								continue;
						}
						else if (j > 0 && j < pCount)
						{
							bp = crossPoints[j - 1];
							ep = crossPoints[j];
						}
						else if (j == pCount)
						{
							bp = crossPoints[j - 1];
							ep = endPoint;
							if (EqualPointF(bp, ep))
								continue;
						}

						GraphicsPath operatePath = isFirst ? firstPath : dividePath;
						if (tValues == null)		//line
							operatePath.AddLine(bp, ep);
						else						//bezier
						{
							//需要分割crossPoints.Count-1次，获取crossPoints.Count条曲线
							if (j == pCount)
								operatePath.AddPath(bezierPath, true);
							else
							{
								float t = CombineFunction.GetFactT(tValues, j);
								GraphicsPath dPath = CombineFunction.DivideBezierPath(ref bezierPath, t);
								if (dPath != null)
									operatePath.AddPath(dPath, true);
							}
						}

						if (isFirst)
							isFirst = false;
						else
						{
							if (j != pCount)
							{
								paths.Add(dividePath);
								dividePath = new GraphicsPath();
							}
						}
					}

					//交点和线段后点重合的情况
					if (EqualPointF(endPoint, crossPoints[crossPoints.Count - 1]))
					{
						if (isFirst)
							isFirst = false;
						else if (dividePath.PointCount > 0)
						{
							paths.Add(dividePath);
							dividePath = new GraphicsPath();
						}
					}
				}
				else
				{
					GraphicsPath operPath = isFirst ? firstPath : dividePath;
					operPath.AddPath(calculatePath, true);
				}

			}

			//last path
			if (firstPath.PointCount > 0)
				dividePath.AddPath(firstPath, true);
			if (dividePath.PointCount > 0)
				paths.Add(dividePath);
		}
		#endregion

		#region connect path
		private enum ConnentType
		{
			FrontAndFront,
			FrontAndBack,
			BackAndFront,
			BackAndBack
		}
		//是否是封闭路径
		private static bool IsClosedPath(GraphicsPath path)
		{
			if (path.PointCount <= 2)
				return false;
			if ((path.PathTypes[path.PointCount - 1] & 0xF0) == 0x80)
				return true;
			if (EqualPointF(path.PathPoints[0], path.PathPoints[path.PointCount - 1]))
				return true;

			return false;
		}
		private static GraphicsPath Connect(GraphicsPath p1, GraphicsPath p2, ConnentType type)
		{
			List<PointF> pList = new List<PointF>(100);
			List<byte> tList = new List<byte>(100);

			//连接两个路径，将重复点删除
			//注意：由于p1可能连续连接两次，所以p1未连接的端点要保持原样。
			switch (type)
			{
				case ConnentType.FrontAndFront:
					pList.AddRange(p1.PathPoints);
					pList.RemoveAt(0);
					pList.InsertRange(0, p2.PathPoints);
					pList.Reverse(0, p2.PointCount);

					tList.AddRange(p1.PathTypes);
					tList.RemoveAt(0);
					tList.InsertRange(0, p2.PathTypes);
					break;
				case ConnentType.FrontAndBack:
					pList.AddRange(p2.PathPoints);
					pList.AddRange(p1.PathPoints);
					pList.RemoveAt(p2.PointCount);

					tList.AddRange(p2.PathTypes);
					tList.AddRange(p1.PathTypes);
					tList.RemoveAt(p2.PointCount);
					break;
				case ConnentType.BackAndFront:
					pList.AddRange(p1.PathPoints);
					pList.AddRange(p2.PathPoints);
					pList.RemoveAt(p1.PointCount);

					tList.AddRange(p1.PathTypes);
					tList.AddRange(p2.PathTypes);
					tList.RemoveAt(p1.PointCount);
					break;
				case ConnentType.BackAndBack:
					pList.AddRange(p2.PathPoints);
					pList.Reverse();
					pList.RemoveAt(0);
					pList.InsertRange(0, p1.PathPoints);

					tList.AddRange(p2.PathTypes);
					tList.RemoveAt(0);
					tList.InsertRange(0, p1.PathTypes);
					break;
				default:
					break;
			}

			return new GraphicsPath(pList.ToArray(), tList.ToArray());
		}
		//合并相连的路径
		public static IEnumerable<GraphicsPath> ConnectPath(List<GraphicsPath> paths)
		{
			//可用路径列表
			List<GraphicsPath> openedList = new List<GraphicsPath>();
			List<GraphicsPath> closedList = new List<GraphicsPath>();
			int count = paths.Count;
			//包含3个或以上路径的交点列表
			List<PointF> multiCross = new List<PointF>();
			ConnentType frontType = ConnentType.FrontAndFront;
			ConnentType backType = ConnentType.BackAndFront;

			for (int i = 0; i < count; i++)
			{
				if (IsClosedPath(paths[i]))
					closedList.Add(paths[i]);
				else
					openedList.Add(paths[i]);
			}

			for (int i = 0; i < openedList.Count; i++)
			{
				GraphicsPath p = openedList[i];
				PointF begin = p.PathPoints[0];
				PointF end = p.PathPoints[p.PointCount - 1];
				int frontCount = 0;
				int backCount = 0;
				GraphicsPath front = null;
				GraphicsPath back = null;
				count = openedList.Count;

				for (int j = i + 1; j < count; j++)
				{
					PointF nBegin = openedList[j].PathPoints[0];
					PointF nEnd = openedList[j].PathPoints[openedList[j].PointCount - 1];

					if (EqualPointF(begin, nBegin) || EqualPointF(begin, nEnd))
					{
						frontCount++;
						front = openedList[j];
						frontType = EqualPointF(begin, nBegin) ? ConnentType.FrontAndFront : ConnentType.FrontAndBack;
					}
					if (EqualPointF(end, nBegin) || EqualPointF(end, nEnd))
					{
						backCount++;
						back = openedList[j];
						backType = EqualPointF(end, nBegin) ? ConnentType.BackAndFront : ConnentType.BackAndBack;
					}
				}

				//交点包含路径大于3，不操作
				if (frontCount > 1)
					multiCross.Add(begin);
				if (backCount > 1)
					multiCross.Add(end);
				bool hasFront = (frontCount == 1) && (!multiCross.Any(pf => EqualPointF(pf, begin)));
				bool hasBack = (backCount == 1) && (!multiCross.Any(pf => EqualPointF(pf, end)));
				bool isSamePath = (front == back);

				if (hasFront || hasBack)
				{
					GraphicsPath newPath = p;
					if (hasFront && hasBack && isSamePath)
					{
						newPath = Connect(newPath, front, frontType);
						openedList.Remove(front);
					}
					else
					{
						if (hasFront)
						{
							newPath = Connect(newPath, front, frontType);
							openedList.Remove(front);
						}
						if (hasBack)
						{
							newPath = Connect(newPath, back, backType);
							openedList.Remove(back);
						}
					}


					openedList.Remove(p);
					openedList.Insert(i, newPath);
					//新路径生成后有新的起始结束点，需要重新计算
					i--;
				}
			}

			openedList.AddRange(closedList);
			return openedList;
		}
		#endregion
	}
}
