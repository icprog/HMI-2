using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NetSCADA6.HMI.NSDrawObj
{
	internal class CombineFunction
	{
		#region field
		private const double Epsilon = 0.00001;
		private const int Invalid = -1;
		#endregion

		#region private function
		private static PointF _originPoint;
		//交点排序
		private static int CompareLineLength(PointF p1, PointF p2)
		{
			float x1 = p1.X - _originPoint.X;
			float y1 = p1.Y - _originPoint.Y;
			float x2 = p2.X - _originPoint.X;
			float y2 = p2.Y - _originPoint.Y;
			double len1 = x1 * x1 + y1 * y1;
			double len2 = x2 * x2 + y2 * y2;

			if (len1 > len2)
				return 1;
			if (len1 < len2)
				return -1;
			return 0;
		}
		private static double CalculateT(double a, double b, double c, double d, double sol)
		{
			double value = sol;
			double eps = 1;
			int count = 0;

			while (Math.Abs(eps) > Epsilon)
			{
				eps = a * Math.Pow(value, 3) + b * Math.Pow(value, 2) + c * value + d;
				value = value - eps / (3 * a * Math.Pow(value, 2) + 2 * b * value + c);

				#if DEBUG
				count++;
				if (count >= 100)
					throw new Exception("hdp.CombineFunction.CalculateT");
				#endif
			}

			return value;
		}
		private static PointF CalculatePoint(PointF p0, PointF p1, PointF p2, PointF p3, float t)
		{
			float t0 = 1 - t;
			double x = p0.X * Math.Pow(t0, 3) + 3 * p1.X * t * Math.Pow(t0, 2) + 3 * p2.X * Math.Pow(t, 2) * t0 + p3.X * Math.Pow(t, 3);
			double y = p0.Y * Math.Pow(t0, 3) + 3 * p1.Y * t * Math.Pow(t0, 2) + 3 * p2.Y * Math.Pow(t, 2) * t0 + p3.Y * Math.Pow(t, 3);
			return new PointF((float)x, (float)y);
		}
		private static bool IsRightAnswer(double a, double b, double c, double d, double t)
		{
			if (t < 0 || t > 1)		//limit[0,1]
				return false;
			
			const double eps = Epsilon * 10;
			double value = a*Math.Pow(t, 3) + b*Math.Pow(t, 2) + c*t + d;
			return Math.Abs(value) <= eps;
		}
		#endregion

		#region public function
		/// <summary>
		/// 获取贝塞尔曲线对应点的t值
		/// </summary>
		/// <param name="p0">控制点</param>
		/// <param name="p1">控制点</param>
		/// <param name="p2">控制点</param>
		/// <param name="p3">控制点</param>
		/// <param name="point">曲线点</param>
		/// <returns>t值</returns>
		public static float GetBezierT(PointF p0, PointF p1, PointF p2, PointF p3, PointF point)
		{
			double xp = point.X;
			double yp = point.Y;
			double x0 = p0.X;
			double y0 = p0.Y;
			double x1 = p1.X;
			double y1 = p1.Y;
			double x2 = p2.X;
			double y2 = p2.Y;
			double x3 = p3.X;
			double y3 = p3.Y;

			double xa = -x0 + 3 * x1 - 3 * x2 + x3;
			double xb = 3 * x0 - 6 * x1 + 3 * x2;
			double xc = -3 * x0 + 3 * x1;
			double xd = x0 - xp;
			double ya = -y0 + 3 * y1 - 3 * y2 + y3;
			double yb = 3 * y0 - 6 * y1 + 3 * y2;
			double yc = -3 * y0 + 3 * y1;
			double yd = y0 - yp;
			float t;
			double[] ds = new []{0, 0.5, 1};	//[0,1]

			foreach (double d in ds)
			{
				t = (float)CalculateT(xa, xb, xc, xd, d);
				if (IsRightAnswer(ya, yb, yc, yd, t))
					return t;
			}
			foreach (double d in ds)
			{
				t = (float)CalculateT(ya, yb, yc, yd, d);
				if (IsRightAnswer(xa, xb, xc, xd, t))
					return t;
			}

			return -1;
		}
		public static PointF[] DivideBezier(PointF p0, PointF p1, PointF p2, PointF p3, float t)
		{
			if (t > 1 || t < 0)
				return null;

			PointF[] ps = new PointF[7];
			float t0 = 1 - t;

			ps[0] = p0;
			ps[3] = CalculatePoint(p0, p1, p2, p3, t);
			ps[6] = p3;
			float x = (p1.X - p0.X) * t + p0.X;
			float y = (p1.Y - p0.Y) * t + p0.Y;
			ps[1] = new PointF(x, y);
			x = (p0.X - 2 * p1.X + p2.X) * t * t + 2 * ps[1].X - p0.X;
			y = (p0.Y - 2 * p1.Y + p2.Y) * t * t + 2 * ps[1].Y - p0.Y;
			ps[2] = new PointF(x, y);

			x = (p2.X - p3.X) * t0 + p3.X;
			y = (p2.Y - p3.Y) * t0 + p3.Y;
			ps[5] = new PointF(x, y);
			x = (p3.X - 2 * p2.X + p1.X) * t0 * t0 + 2 * ps[5].X - p3.X;
			y = (p3.Y - 2 * p2.Y + p1.Y) * t0 * t0 + 2 * ps[5].Y - p3.Y;
			ps[4] = new PointF(x, y);

			return ps;
		}
		//曲线点排序
		public static List<float> SortCurvePoint(GraphicsPath path, List<PointF> pfs)
		{
			//如果是贝塞尔，先把点计算为t值，然后排序
			if (path.PointCount == 4)
			{
				List<float> ts = new List<float>(pfs.Count);
				foreach (PointF pf in pfs)
				{
					float t = GetBezierT(path.PathPoints[0], path.PathPoints[1],
														path.PathPoints[2], path.PathPoints[3], pf);

#if DEBUG
					if (t == Invalid)
						throw new Exception("hdp.DividePath.SortCurvePoint");
#endif

					ts.Add(t);
				}

				ts.Sort();
				return ts;
			}

			//如果是直线，直接排序
			_originPoint = path.PathPoints[0];
			pfs.Sort(CompareLineLength);
			return null;
		}
		public static GraphicsPath DivideBezierPath(ref GraphicsPath path, float t)
		{
			if (t >= 1 || t <= 0)
				return null;

			PointF[] pfs = DivideBezier(path.PathPoints[0], path.PathPoints[1],
													   path.PathPoints[2], path.PathPoints[3], t);
			GraphicsPath p0 = new GraphicsPath();
			p0.AddBezier(pfs[0], pfs[1], pfs[2], pfs[3]);
			path = new GraphicsPath();
			path.AddBezier(pfs[3], pfs[4], pfs[5], pfs[6]);

			return p0;
		}
		public static float GetFactT(IList<float> ts, int index)
		{
			float t = ts[index];
			if (index > 0)
			{
				float tPre = ts[index - 1];
				t = (tPre == 1) ? 1 : (t - tPre) / (1 - tPre);
			}

			return t;
		}
		#endregion

	}
}
