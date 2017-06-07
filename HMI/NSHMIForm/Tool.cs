using System.Drawing;

namespace NetSCADA6.HMI.NSHMIForm
{
    internal static class Tool
    {
        /// <summary>
        /// 根据两个Point生成一个Rectangle
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
		public static Rectangle GetRect(Point p1, Point p2)
        {
            int xMin = (p1.X < p2.X) ? p1.X : p2.X;
            int xMax = (p1.X > p2.X) ? p1.X : p2.X;
            int yMin = (p1.Y < p2.Y) ? p1.Y : p2.Y;
            int yMax = (p1.Y > p2.Y) ? p1.Y : p2.Y;

            return Rectangle.FromLTRB(xMin, yMin, xMax, yMax);
        }
		/// <summary>
		/// 根据两个PointF生成一个RectangleF
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <returns></returns>
		public static RectangleF GetRectF(PointF p1, PointF p2)
		{
			float xMin = (p1.X < p2.X) ? p1.X : p2.X;
			float xMax = (p1.X > p2.X) ? p1.X : p2.X;
			float yMin = (p1.Y < p2.Y) ? p1.Y : p2.Y;
			float yMax = (p1.Y > p2.Y) ? p1.Y : p2.Y;

			return new RectangleF(xMin, yMin, xMax-xMin, yMax-yMin);
		}
		public static PointF GetGridPointF(PointF point)
		{
			float value = point.X;
			point.X = (int)value - ((int)value) % 10;
			value = point.Y;
			point.Y = (int)value - ((int)value) % 10;

			return point;
		}
		public static Rectangle GetGridRect(Rectangle rect)
		{
			rect.X = rect.X - rect.X % 10;
			rect.Y = rect.Y - rect.Y % 10;
			rect.Width = rect.Width - rect.Width % 10;
			rect.Height = rect.Height - rect.Height % 10;
			
			return rect;
		}
	}
}
