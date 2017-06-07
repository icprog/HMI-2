using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using NetSCADA6.Common.NSColorManger;

namespace NetSCADA6.HMI.NSDrawNodes
{
    /// <summary>
    /// 辅助类
    /// </summary>
    internal static class MyTools
    {
        /// <summary>
        /// 运行时
        /// </summary>
        static bool IsStudioMode
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// 管道精度 
        /// </summary>
        public const float accuracy = 10.0f;
        /// <summary>
        /// 直线坐标转换至贝塞尔坐标
        /// </summary>
        /// <param name="nodeList"></param>
        /// <param name="close"></param>
        /// <returns></returns>
        public static List<PointF> ConvertBeziers(List<PointF> nodeList, bool close)
        {
            List<PointF> list = new List<PointF>();
            PointF[] ptfs = new PointF[4];
            ptfs[0] = nodeList[nodeList.Count - 1];
            ptfs[3] = nodeList[0];
            for (int i = 0; i < nodeList.Count; i++)
            {
                PointF data = nodeList[i]; //数据点
                PointF prev = nodeList[i]; //上一次数据点
                PointF next = nodeList[i];//下一次数据点
                if (i != 0)
                    prev = nodeList[i - 1];
                else if (close)
                    prev = nodeList[nodeList.Count - 1];
                if (i != nodeList.Count - 1)
                    next = nodeList[i + 1];
                else if (close)
                    next = nodeList[0];
                //数据准备完毕、开始计算控制点 
                //控制点要求:  控制线 controlpoint1 与controlpoint2 。   数据线 数据点 prev、next所连成的线。
                //1.控制线与数据线平行。
                //2.控制线的长度是数据线的二分之一。

                //求数据线长度
                float dataDis = MyTools.Distance(prev, next);
                //求数据线的夹角
                float dataAngle = GetLineHorizontalAngle(prev, next);
                //获取控制点
                PointF controlPoint1 = CenterRadiusPoint(data, dataAngle, dataDis / 4);
                PointF controlPoint2 = CenterRadiusPoint(data, dataAngle - 180, dataDis / 4);
                //添加控制点
                if (i != 0)
                    list.Add(controlPoint1);
                else if (close)
                {
                    ptfs[2] = controlPoint1;
                }
                list.Add(data);
                if (i != nodeList.Count - 1)
                    list.Add(controlPoint2);
                else if (close)
                {
                    ptfs[1] = controlPoint2;
                }
            }
            if (close)
            {
                list.Add(ptfs[1]);
                list.Add(ptfs[2]);
                list.Add(ptfs[3]);
            }
            return list;
        }

        /// <summary>
        /// 获取第index条的贝塞尔曲线。
        /// </summary>
        /// <param name="bzrs"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static PointF[] GetBzr(List<PointF> bzrs, int index)
        {
            if (index <= 0 || index >= bzrs.Count)
                return null;
            List<PointF> list = new List<PointF>();
            PointF data1 = bzrs[index * 3 - 3];
            PointF ctrl1 = bzrs[index * 3 - 2];
            PointF ctrl2 = bzrs[index * 3 - 1];
            PointF data2 = bzrs[index * 3];
            list.Add(data1);
            list.Add(ctrl1);
            list.Add(ctrl2);
            list.Add(data1);
            return list.ToArray();
        }


        /// <summary>
        /// 链表的拷贝 整体位移
        /// </summary>
        public static List<PointF> CopyListPoint(List<PointF> listPoint, int xOffset = 0, int yOffset = 0)
        {
            List<PointF> other = new List<PointF>();
            for (int i = 0; i < listPoint.Count; i++)
            {
                other.Add(new PointF(listPoint[i].X + xOffset, listPoint[i].Y + yOffset));
            }
            return other;
        }

        #region 点、线、圆弧、坐标

        /// 根据中心点、半径、角度，求半径另一端的坐标。注意用的是笛卡尔坐标系   
        /// </summary>   
        /// <param name="center">中心点</param>   
        /// <param name="angle">半径角度</param>   
        /// <param name="radius">半径长度</param>   
        /// <returns>半径另一端的坐标</returns>   
        public static PointF CenterRadiusPoint(PointF center, double angle, double radius)
        {
            PointF p = new PointF();
            double angleHude = angle * Math.PI / 180;/*角度变成弧度*/
            p.X = (float)(radius * Math.Cos(angleHude)) + center.X;
            p.Y = (float)(radius * Math.Sin(angleHude)) + center.Y;
            return p;
        }

        /// <summary>
        /// 获取等比目标坐标
        /// </summary>
        /// <param name="srcRf"></param>
        /// <param name="srcPoint"></param>
        /// <param name="defRf"></param>
        /// <returns></returns>
        public static PointF GetDesPoint(RectangleF srcRf, PointF srcPoint, RectangleF desRf)
        {
            if (desRf.Width != 0)
                desRf.Width = 1;
            if (desRf.Height != 0)
                desRf.Height = 1;
            float x = srcRf.Width / desRf.Width * (srcPoint.X - srcRf.X) + desRf.X;
            float y = srcRf.Height / desRf.Height * (srcPoint.Y - srcRf.Y) + desRf.Y;
            return new PointF(x, y);
        }


        /// <summary>
        ///  获取折线abc区域的相交点和圆弧开始角度和弧度
        /// </summary>
        /// <param name="a">3点连成一个折线  点A(旧点)</param>
        /// <param name="b">3点连成一个折线  点A(中心点)</param>
        /// <param name="c">3点连成一个折线  点C(当前点)</param>
        /// <param name="crossPoint">引用 夹角点</param> 
        /// <param name="sweepAngle">引用 圆弧开始弧度</param>
        /// <param name="sweepAngle">引用 圆弧弧度</param>
        /// <returns>真：焦点在上，圆弧在下，假：焦点在下，圆弧在上 </returns>
        public static bool GetValidStartPointAndOfAngle(PointF a, PointF b, PointF c, float thick, ref PointF crossPoint, ref float startAngle, ref float sweepAngle)//
        {
            float abAngle = GetLineHorizontalAngle(a, b);//开始角度
            float bcAngle = GetLineHorizontalAngle(c, b);//第二条线的角度
            sweepAngle = Math.Abs(180 - Math.Abs(abAngle - bcAngle));//夹角
            crossPoint = new PointF(0, 0);
            PointF[] oldPeak = GetLineRegion(a, b, thick);
            PointF[] controlPeak = GetLineRegion(b, c, thick);
            PointF startPoint = new PointF();
            if (Math.Abs(abAngle - bcAngle) < 180)
            {
                if (bcAngle - abAngle > 0)
                {
                    crossPoint = GetCrossPoint(oldPeak[0], oldPeak[1], controlPeak[0], controlPeak[1], b);
                    sweepAngle -= sweepAngle + sweepAngle;
                    startPoint = oldPeak[2];
                    startAngle = (float)GetStartAngle(b, startPoint);
                    return true;
                }
                else
                {
                    crossPoint = GetCrossPoint(oldPeak[3], oldPeak[2], controlPeak[3], controlPeak[2], b);
                    startPoint = oldPeak[1];
                    startAngle = (float)GetStartAngle(b, startPoint);
                    return false;
                }
            }
            else
            {
                if (bcAngle - abAngle < 0)
                {
                    crossPoint = GetCrossPoint(oldPeak[0], oldPeak[1], controlPeak[0], controlPeak[1], b);
                    sweepAngle -= sweepAngle + sweepAngle;
                    startPoint = oldPeak[2];
                    startAngle = (float)GetStartAngle(b, startPoint);
                    return true;
                }
                else
                {
                    crossPoint = GetCrossPoint(oldPeak[3], oldPeak[2], controlPeak[3], controlPeak[2], b);
                    startPoint = oldPeak[1];
                    startAngle = (float)GetStartAngle(b, startPoint);
                    return false;
                }
            }
        }

        /// <summary>
        /// 得到圆弧的开始角度
        /// </summary>
        /// <param name="center"></param>
        /// <param name="startPt"></param>
        /// <returns></returns>
        public static double GetStartAngle(PointF center, PointF startPt)
        {
            float dy = startPt.Y - center.Y;
            float dx = startPt.X - center.X;
            if (dy == 0)
            {
                if (startPt.X > center.X)
                {
                    return 0;
                }
                return 180;
            }
            if (dx == 0)
            {
                if (startPt.Y > center.Y)
                {
                    return 90;
                }
                return 270;
            }
            double k = ((double)dy) / dx;
            double angle = Math.Atan(k);
            if (k < 0)
            {
                angle = (Math.PI + angle) / Math.PI * 180;
                if (startPt.Y > center.Y)
                {
                    return angle;
                }
                return 180 + angle;
            }
            if (startPt.X > center.X)
            {
                return angle * 180 / Math.PI;
            }
            return 180 + angle * 180 / Math.PI;
        }

        /// <summary>
        /// 在点B上画一条水平线，得到线段AB与水平线的夹角
        /// </summary>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        /// <returns></returns>
        public static float GetLineHorizontalAngle(PointF pointA, PointF pointB)
        {
            float angle = 0;
            if (pointA.Y == pointB.Y)
            {
                if (pointA.X > pointB.X)
                    return 0;
                return 180;
            }
            if (pointA.X == pointB.X)
            {
                if (pointA.Y > pointB.Y)
                    return 90;
                return 270;
            }
            PointF pt = new PointF(pointA.X, pointB.Y);
            float dui = Distance(pt, pointA);
            float xie = Distance(pointA, pointB);
            angle = (float)Math.Asin(dui / xie);
            angle = angle / (float)Math.PI * (float)180.0;   //弧度转角度

            if (pointA.X > pointB.X && pointA.Y > pointB.Y)
            {
                return angle;
            }
            else if (pointA.X < pointB.X && pointA.Y > pointB.Y)
            {
                angle = 180 - angle;
                return angle;
            }
            else if (pointA.X < pointB.X && pointA.Y < pointB.Y)
            {
                angle += 180;
                return angle;
            }
            else if (pointA.X > pointB.X && pointA.Y < pointB.Y)
            {
                angle = 360 - angle;
                return angle;
            }
            return angle;
        }

        /// <summary>
        /// 求2点之间的距离
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Distance(PointF a, PointF b)
        {
            return Math.Abs((float)Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y)));
        }

        /// <summary>
        /// 获取线段相交的交点，如果平行或者无交点则返回dataCenter
        /// </summary> 
        public static PointF GetCrossPoint(PointF u1, PointF u2, PointF v1, PointF v2, PointF dataCenter)
        {
            if (ParallelLines(u1, u2, v1, v2) || !Intersect_in(u1, u2, v1, v2))//如果平行||无交点
                return dataCenter;
            PointF ret = new PointF();
            ret = u1;
            float t = ((u1.X - v1.X) * (v1.Y - v2.Y) - (u1.Y - v1.Y) * (v1.X - v2.X))
              / ((u1.X - u2.X) * (v1.Y - v2.Y) - (u1.Y - u2.Y) * (v1.X - v2.X));
            ret.X += (u2.X - u1.X) * t;
            ret.Y += (u2.Y - u1.Y) * t;
            return ret;
        }

        /// <summary>
        /// 判两点在线段同侧,点在线段上返回0
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        public static bool Same_side(PointF p1, PointF p2, PointF l1, PointF l2)
        {
            return Xmult(l1, p1, l2) * Xmult(l1, p2, l2) > 1e-8;
        }

        /// <summary>
        /// 判三点共线
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        public static bool Dots_inline(PointF p1, PointF p2, PointF p3)
        {
            return zero(Xmult(p1, p2, p3));
        }

        /// <summary>
        /// 判两线段相交,包括端点和部分重合
        /// </summary>
        /// <param name="u1"></param>
        /// <param name="u2"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool Intersect_in(PointF u1, PointF u2, PointF v1, PointF v2)
        {
            if (!Dots_inline(u1, u2, v1) || !Dots_inline(u1, u2, v2))
                return !Same_side(u1, u2, v1, v2) && !Same_side(v1, v2, u1, u2);
            return Dot_online_in(u1, v1, v2) || Dot_online_in(u2, v1, v2) || Dot_online_in(v1, u1, u2) || Dot_online_in(v2, u1, u2);
        }

        /// <summary>
        /// 计算交叉乘积(P1-P0)x(P2-P0)
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p0"></param>
        /// <returns></returns>
        public static float Xmult(PointF p1, PointF p2, PointF p0)
        {
            return (p1.X - p0.X) * (p2.Y - p0.Y) - (p2.X - p0.X) * (p1.Y - p0.Y);
        }

        public static bool zero(float x)
        {
            double eps = 1e-8;
            float result = x > 0 ? x : -x;
            return (result < eps);
        }

        /// <summary>
        /// 判点是否在线段上,包括端点
        /// </summary>
        /// <param name="p"></param>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        public static bool Dot_online_in(PointF p, PointF l1, PointF l2)
        {
            return zero(Xmult(p, l1, l2)) && (l1.X - p.X) * (l2.X - p.X) < 1e-8 && (l1.Y - p.Y) * (l2.Y - p.Y) < 1e-8;
        }

        /// <summary>
        /// 判两直线平行
        /// </summary>
        /// <param name="u1">线段a 头部坐标</param>
        /// <param name="u2">线段a 尾部坐标</param>
        /// <param name="v1">线段b 头部坐标</param>
        /// <param name="v2">线段b 尾部坐标</param>
        /// <returns>是否平行</returns>
        public static bool ParallelLines(PointF u1, PointF u2, PointF v1, PointF v2)
        {
            return zero((u1.X - u2.X) * (v1.Y - v2.Y) - (v1.X - v2.X) * (u1.Y - u2.Y));
        }

        /// <summary>
        /// 得到线段AB区域
        /// </summary>
        /// <param name="pointA">点A</param>
        /// <param name="pointB">点B</param>
        /// <param name="thick">粗细</param> 
        /// <returns>线段AB4个顶点</returns>
        public static PointF[] GetLineRegion(PointF pointA, PointF pointB, float thick)
        {
            PointF[] pTemp = new PointF[4];
            float width = pointB.X - pointA.X;
            float height = pointB.Y - pointA.Y;
            float sina = height / (float)Math.Sqrt(width * width + height * height);
            float cosa = width / (float)Math.Sqrt(width * width + height * height);
            float temp_sina = sina * thick / 2.0f;
            float temp_cosa = cosa * thick / 2.0f;

            pTemp[0].X = pointA.X + temp_sina;
            pTemp[0].Y = pointA.Y - temp_cosa;
            pTemp[3].X = pointA.X - temp_sina;
            pTemp[3].Y = pointA.Y + temp_cosa;
            pTemp[1].X = pointB.X + temp_sina;
            pTemp[1].Y = pointB.Y - temp_cosa;
            pTemp[2].X = pointB.X - temp_sina;
            pTemp[2].Y = pointB.Y + temp_cosa;
            return pTemp;
        }

        /// <summary>
        /// 手动计算折线ary的区域
        /// </summary>
        /// <param name="ary">折线</param>
        /// <param name="thick">粗细</param>
        /// <returns>折线路径区域</returns>
        public static GraphicsPath GetLinesPath(List<PointF> ary, float thick)
        {
            GraphicsPath path = new GraphicsPath();
            List<PointF> ListPoint = new List<PointF>();
            for (int i = 1; i < ary.Count - 1; i++)
            {
                PointF corssPoint = new PointF();
                float startAngle = 0;
                float sweepAngle = 0;
                PointF[] pts = GetLineRegion(ary[i - 1], ary[i], thick);
                bool crossUp01 = GetValidStartPointAndOfAngle(ary[i - 1], ary[i], ary[i + 1], thick, ref corssPoint, ref startAngle, ref sweepAngle);

                if (i == 1) //第一次 添加头部2个点
                {
                    PointF[] pts1 = GetLineRegion(ary[i - 1], ary[i], thick);
                    ListPoint.Insert(0, pts1[0]);
                    ListPoint.Add(pts1[3]);
                }
                if (crossUp01)
                {
                    PointF[] pts1 = GetLineRegion(ary[i], ary[i + 1], thick);
                    ListPoint.Insert(0, corssPoint);
                    ListPoint.Add(pts[2]);
                    ListPoint.Add(pts1[3]);
                }
                else
                {
                    PointF[] pts1 = GetLineRegion(ary[i], ary[i + 1], thick);
                    ListPoint.Insert(0, pts[1]);
                    ListPoint.Insert(0, pts1[0]);
                    ListPoint.Add(corssPoint);
                }
                if (i == ary.Count - 2)
                {
                    PointF[] pts1 = GetLineRegion(ary[i], ary[i + 1], thick);
                    ListPoint.Insert(0, pts1[1]);
                    ListPoint.Add(pts1[2]);
                }
            }
            if (ListPoint.Count == 0 && ary.Count == 2)
            {
                PointF[] pts = GetLineRegion(ary[0], ary[1], thick);
                path.AddPolygon(pts);
                return path;
            }
            path.AddPolygon(ListPoint.ToArray());
            return path;
        }

        /// <summary>
        ///  点point是否在折线的区域内
        /// </summary>
        /// <param name="point">逻辑坐标点</param>
        /// <param name="lines">折线(N条线)</param> 
        /// <param name="thick">线的粗细</param>
        /// <returns>True:在区域内,False:不在区域内</returns>
        public static bool IsVisible_PointInLines(PointF point, List<PointF> lines, float thick = 3)
        {
            GraphicsPath path = new GraphicsPath();
            float thick2 = thick / 2.0f;
            for (int i = 0; i < lines.Count - 1; i++)
            {
                PointF[] pts = GetLineRegion(lines[i], lines[i + 1], thick);
                path.AddLines(pts);
                path.CloseFigure();
                if (path.IsVisible(point))
                    return true;
                path.AddEllipse(lines[i].X - thick2, lines[i].Y - thick2, thick, thick);
                path.AddEllipse(lines[i + 1].X - thick2, lines[i + 1].Y - thick2, thick, thick);
                path.CloseFigure();
                if (path.IsVisible(point))
                    return true;
                path.Reset();
            }
            return false;
        }
        #endregion
    }
}
