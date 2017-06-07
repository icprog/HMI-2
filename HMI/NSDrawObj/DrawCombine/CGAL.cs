using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace NetSCADA6.HMI.NSDrawObj
{
	/// <summary>
	/// CGAL算法
	/// </summary>
	internal static class CGAL
	{
		#region dll
		[DllImport("NSCGALFunctions.dll", EntryPoint = "CalculateIntersection")]
		private static extern int CalculateIntersection(int lineCount, PointF[] lines, byte[] types);
		[DllImport("NSCGALFunctions.dll", EntryPoint = "GetIntersection")]
		private static extern int GetIntersection(float[] intersX, float[] interY);
		#endregion

		/// <summary>
		/// 交点计算
		/// </summary>
		public static class Intersection
		{
			public static PointF[] Calculate(PointF[] points, byte[] types)
			{
				try
				{
					int count = CalculateIntersection(points.Length, points, types);

					if (count > 0)
					{
						float[] xs = new float[count];
						float[] ys = new float[count];
						GetIntersection(xs, ys);

						PointF[] inters = new PointF[count];
						for (int i = 0; i < count; i++)
							inters[i] = new PointF(xs[i], ys[i]);

						return inters;
					}
					return null;
				}
				catch (Exception)
				{
					return null;
				}

				
			}
		}
	}
}
