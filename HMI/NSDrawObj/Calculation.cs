using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NetSCADA6.HMI.NSDrawObj
{
    /// <summary>
    /// 矩阵计算类
    /// </summary>
	public static class Calculation
    {
        #region 矩阵计算
        private static readonly GraphicsPath Path = new GraphicsPath();
        private static readonly Matrix Matrix = new Matrix();
        /// <summary>
        /// 获取倾斜后的点位置,基准点为(0，0)
        /// </summary>
        /// <param name="point"></param>
        /// <param name="shear"></param>
        /// <returns></returns>
        private static PointF GetShearPoint(PointF point, float shear)
        {
            if (shear == 0)
                return point;

            Path.Reset();
            Path.AddLine(point, PointF.Empty);
            Matrix.Reset();
            Matrix.Shear(shear, 0);
            Path.Transform(Matrix);

            return Path.PathPoints[0];
        }
        /// <summary>
        /// 获取矩阵逆转换后的坐标
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static PointF GetInvertPos(Matrix matrix, PointF point)
        {
            if (matrix == null)
                return point;
            if (matrix.IsIdentity || !matrix.IsInvertible)
                return point;

            Matrix m = matrix.Clone();
            m.Invert();
            PointF pf = GetMatrixPos(m, point);
            m.Dispose();
            return pf;
        }
        /// <summary>
        /// 获取旋转后的坐标
        /// </summary>
        /// <param name="point"></param>
        /// <param name="center"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private static PointF GetRotatePos(PointF point, PointF center, float angle)
        {
            if (angle == 0)
                return point;

            Path.Reset();
            Path.AddLine(point, PointF.Empty);
            Matrix.Reset();
            Matrix.RotateAt(angle, center);
            Path.Transform(Matrix);

            return Path.PathPoints[0];
        }
        /// <summary>
        /// 获取矩阵转换后的点位置
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static PointF GetMatrixPos(Matrix matrix, PointF point)
        {
            if (matrix.IsIdentity)
                return point;

            Path.Reset();
            Path.AddLine(point, PointF.Empty);
            Path.Transform(matrix);

            return Path.PathPoints[0];
        }
        /// <summary>
        /// 获取旋转后移动的位置
        /// </summary>
        /// <param name="point"></param>
        /// <param name="center"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private static PointF GetRotateOffset(PointF point, PointF center, float angle)
        {
            PointF rotatePos = GetRotatePos(point, center, angle);
            return new PointF(rotatePos.X - point.X, rotatePos.Y - point.Y);
        }
        public static RectangleF GetMatrixBounds(Matrix matrix, RectangleF rect)
        {
            Path.Reset();
            Path.AddRectangle(rect);
            Path.Transform(matrix);
            return Path.GetBounds();
        }
		/// <summary>
		/// 获取pen加粗后的bound
		/// </summary>
		/// <param name="path"></param>
		/// <param name="p"></param>
		/// <returns></returns>
        public static RectangleF GetPenBounds(GraphicsPath path, Pen p)
        {
        	Matrix.Reset();
        	return path.GetBounds(Matrix, p);
        }
		/// <summary>
		/// 获取pen加粗matrix转换后的bound
		/// </summary>
		/// <param name="path"></param>
		/// <param name="p"></param>
		/// <param name="m"></param>
		/// <returns></returns>
		public static RectangleF GetPenAndMatrixBounds(GraphicsPath path, Pen p, Matrix m)
		{
			Path.Reset();
			
			RectangleF rf = path.GetBounds();
			rf.Inflate(p.Width / 2f, p.Width / 2f);
			Path.AddRectangle(rf);
			return Path.GetBounds(m, p);
		}

        #endregion

        #region 特定计算
        private static readonly GraphicsPath _funPath = new GraphicsPath();
        private static Matrix _funMatrix = new Matrix();
        /// <summary>
        /// 计算特定比例点坐标
        /// </summary>
        /// <param name="fixRate">特定点比例系数</param>
        /// <param name="isBound">是否是矩形边框特定点</param>
        /// <returns></returns>
        public static PointF CalcFixPoint(IDrawData data, PointF fixRate, bool isBound = false)
        {
            _funMatrix.Reset();
            PointF point = PointF.Empty;
            CalcMatrix(data, ref _funMatrix, ref point);

            
            RectangleF rf = data.Rect;
            if (data is DrawObj)
                rf = ((DrawObj) data).NewRect;
            PointF fixPos = new PointF(rf.X + rf.Width * fixRate.X, rf.Y + rf.Height * fixRate.Y);

            if (isBound)
            {
                rf = GetMatrixBounds(_funMatrix, rf);
                fixPos = new PointF(rf.X + rf.Width * fixRate.X, rf.Y + rf.Height * fixRate.Y);

                return fixPos;
            }

            return GetMatrixPos(_funMatrix, fixPos);
        }
		/// <summary>
		/// 计算矩阵转换后的点
		/// </summary>
		/// <param name="data"></param>
		/// <param name="point"></param>
		/// <returns></returns>
		public static PointF CalcMatrixPoint(IDrawData data, PointF point)
		{
			_funMatrix.Reset();
			PointF center = PointF.Empty;
			CalcMatrix(data, ref _funMatrix, ref center);

			return GetMatrixPos(_funMatrix, point);
		}
		//获取反转后的矩形数据
		private static void FlipRectPos(PointF[] pfs, bool isFlipX, bool isFlipY)
		{
			PointF change;
			if (isFlipX)
			{
				change = pfs[0];
				pfs[0] = pfs[1];
				pfs[1] = change;
				change = pfs[3];
				pfs[3] = pfs[2];
				pfs[2] = change;
			}
			if (isFlipY)
			{
				change = pfs[0];
				pfs[0] = pfs[3];
				pfs[3] = change;
				change = pfs[1];
				pfs[1] = pfs[2];
				pfs[2] = change;
			}
		}
        /// <summary>
        /// 根据控件形状获取其矩阵旋转角度，倾斜和实际大小。边框移动调用此函数
        /// </summary>
        /// <param name="data"></param>
        /// <param name="wOff"></param>
        /// <param name="hOff"></param>
        /// <param name="angle"></param>
        /// <param name="shear"></param>
        /// <param name="rect"></param>
        public static void GetBoundMoveData(CalcData data, float wOff, float hOff, out float angle, out float shear, out RectangleF rect)
        {
            _funMatrix.Reset();
            _funPath.Reset();

			PointF pf = PointF.Empty;
            CalcMatrix(data, ref _funMatrix, ref pf);
			_funPath.AddRectangle(data.Rect);
			_funPath.Transform(_funMatrix);

			//反转Flip
			PointF[] ps = _funPath.PathPoints;
        	FlipRectPos(ps, data.IsFlipX, data.IsFlipY);

            RectangleF bound = _funPath.GetBounds();
            float newW = bound.Width + wOff;
            float scaleW = newW / bound.Width;
            float newH = bound.Height + hOff;
            float scaleH = newH / bound.Height;

            for (int i = 0; i < ps.Length; i++)
            {
                ps[i].X = (ps[i].X - bound.X) * scaleW + bound.X;
                ps[i].Y = (ps[i].Y - bound.Y) * scaleH + bound.Y;
            }

            //计算旋转角度
            angle = (float)Math.Atan2(ps[1].Y - ps[0].Y, ps[1].X - ps[0].X);
            angle = (float)(angle * 180 / Math.PI);

            //获取反旋转后的数据
            _funPath.Reset();
            _funPath.AddLines(ps);
            bound = _funPath.GetBounds();
            PointF center = new PointF(bound.X + bound.Width * 0.5f, bound.Y + bound.Height * 0.5f);
            _funMatrix.Reset();
            _funMatrix.RotateAt(-angle, center);
            _funPath.Transform(_funMatrix);

            //计算倾斜
            ps = _funPath.PathPoints;
            float h = ps[3].Y - ps[0].Y;
            float wShear = ps[3].X - ps[0].X;
            shear = wShear / h;

            //获取反倾斜后的数据
            rect = new RectangleF {X = ps[0].X + wShear/2f, Y = ps[0].Y, Width = ps[1].X - ps[0].X, Height = h};
        }
        /// <summary>
        /// 根据实际矩阵获取旋转角度，倾斜和实际大小。解组时调用此函数
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="angle"></param>
        /// <param name="shear"></param>
        /// <param name="rect"></param>
        public static void GetMatrixData(DrawVector obj, out float angle, out float shear, out RectangleF rect)
        {
            _funMatrix.Reset();
            _funPath.Reset();
            _funPath.AddRectangle(obj.Rect);
            _funPath.Transform(obj.Matrix);
			RectangleF bound = _funPath.GetBounds();

			//反转Flip
			PointF[] ps = _funPath.PathPoints;
			FlipRectPos(ps, obj.IsFlipX, obj.IsFlipY);

            //计算旋转角度
            angle = (float)Math.Atan2(ps[1].Y - ps[0].Y, ps[1].X - ps[0].X);
            angle = (float)(angle * 180 / Math.PI);

            //获取反旋转后的数据
            PointF center = new PointF(bound.X + bound.Width * 0.5f, bound.Y + bound.Height * 0.5f);
            _funMatrix.Reset();
            _funMatrix.RotateAt(-angle, center);
            _funPath.Transform(_funMatrix);

            //计算倾斜
			ps = _funPath.PathPoints;
			FlipRectPos(ps, obj.IsFlipX, obj.IsFlipY);
			float h = ps[3].Y - ps[0].Y;
            float wShear = ps[3].X - ps[0].X;
            shear = wShear / h;

            //获取反倾斜后的数据
            rect = new RectangleF {X = ps[0].X + wShear/2f, Y = ps[0].Y, Width = ps[1].X - ps[0].X, Height = h};

        	//获取rect偏移
            PointF origCenter = new PointF(rect.Width * (obj.RotatePoint.X - 0.5f),
                rect.Height * (obj.RotatePoint.Y - 0.5f));
            PointF rotateCenter = GetShearPoint(origCenter, shear);
            PointF offset = GetRotatePos(PointF.Empty, rotateCenter, angle);
            float xOff = center.X - (rect.X + rect.Width/2 + offset.X);
            float yOff = center.Y - (rect.Y + rect.Height/2 + offset.Y);
            rect.X += xOff;
            rect.Y += yOff;
        }
        /// <summary>
        /// 生成矩阵
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="rotatePointPos"></param>
        public static void CalcMatrix(IDrawData data, ref Matrix matrix, ref PointF rotatePointPos)
        {
            matrix.Reset();

            RectangleF rect = data.Rect;
            if (data is DrawObj)
                rect = ((DrawObj)data).NewRect;

            RectangleF rf = GetScaleRect(data.ScalePoint, data.XScale, data.YScale, rect);
            PointF scalePoint = new PointF(rf.Left + rf.Width * data.ScalePoint.X,
                rf.Top + rf.Height * data.ScalePoint.Y);
            PointF rectCenter = new PointF(rf.Left + rf.Width * 0.5f,
                rf.Top + rf.Height * 0.5f);
            //计算旋转中心点
            PointF origCenter = new PointF(rf.Width * (data.RotatePoint.X - 0.5f),
                rf.Height * (data.RotatePoint.Y - 0.5f));
            PointF rotateCenter = GetShearPoint(origCenter, data.Shear);
            rotatePointPos = new PointF(rectCenter.X + rotateCenter.X,
                    rectCenter.Y + rotateCenter.Y);

            //本段代码次序不能改变
            //Matrix后面的操作先执行，切记
            //以中心点为基准旋转倾斜
            matrix.Translate(rectCenter.X, rectCenter.Y);
            matrix.RotateAt(data.RotateAngle, rotateCenter);
            matrix.Shear(data.Shear, 0);
            matrix.Translate(-rectCenter.X, -rectCenter.Y);
            //以缩放点为基准缩放
            matrix.Translate(scalePoint.X, scalePoint.Y);
            matrix.Scale(data.XScale, data.YScale);
            matrix.Translate(-scalePoint.X, -scalePoint.Y);
			//垂直水平翻转
			if (data.IsFlipX || data.IsFlipY)
			{
				float x = data.IsFlipX ? -1 : 1;
				float y = data.IsFlipY ? -1 : 1;
				matrix.Translate(rectCenter.X, rectCenter.Y);
				matrix.Scale(x, y);
				matrix.Translate(-rectCenter.X, -rectCenter.Y);
			}
        }
        /// <summary>
        /// 获取旋转点，比例系数
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="point"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static PointF GetRotatePoint(Matrix matrix, PointF point, RectangleF rect)
        {
            PointF pf = GetInvertPos(matrix, point);
            float rateX = (pf.X - rect.X) / rect.Width;
            float rateY = (pf.Y - rect.Y) / rect.Height;

            return new PointF(rateX, rateY);
        }
        /// <summary>
        /// 获取倾斜值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="point"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static float GetShear(CalcData data, PointF point, int pos)
        {
            float x1 = GetRotatePos(data.MousePos, data.RotatePointPos, -data.RotateAngle).X;
            float x2 = GetRotatePos(point, data.RotatePointPos, -data.RotateAngle).X;
            float xShear = (x2 - x1) * 2 / data.Rect.Height;

            if (pos == 0)
                xShear = -xShear;
			if (data.IsFlipY)
				xShear = -xShear;

            return data.Shear + xShear;
        }
        /// <summary>
        /// 获取边框移动后的偏移量
        /// </summary>
        /// <param name="data"></param>
        /// <param name="point"></param>
        /// <param name="pos"></param>
        /// <param name="xOff"></param>
        /// <param name="yOff"></param>
        /// <param name="wOff"></param>
        /// <param name="hOff"></param>
        public static void GetFrameOffset(CalcData data, PointF point, int pos, out float xOff, out float yOff, out float wOff, out float hOff, float orthoTan)
        {
            PointF begin = GetInvertPos(data.Matrix, data.MousePos);
            PointF end = GetInvertPos(data.Matrix, point);

            GetOffset(begin, end, pos, out xOff, out yOff, out wOff, out hOff, orthoTan);
            LimitOffset(data.Rect, ref xOff, ref yOff, ref wOff, ref hOff);
        }
        /// <summary>
        /// 获取旋转角
        /// </summary>
        /// <param name="data"></param>
        /// <param name="point"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        public static float GetRotateAngle(CalcData data, PointF point, PointF center)
        {
            double angle1 = Math.Atan2(data.MousePos.Y - center.Y, data.MousePos.X - center.X);
            double angle2 = Math.Atan2(point.Y - center.Y, point.X - center.X);
            float angle = (float)((angle2 - angle1) * 180 / Math.PI);

            return data.RotateAngle + angle;
        }
        /// <summary>
        /// 获取成组旋转的偏移
        /// </summary>
        /// <param name="data"></param>
        /// <param name="center"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static PointF GetRotateAtOffset(CalcData data, PointF center, float angle)
        {
            _funPath.Reset();
            _funPath.AddRectangle(data.Rect);
            _funPath.Transform(data.Matrix);
            PointF p1 = _funPath.PathPoints[0];
            PointF p2 = _funPath.PathPoints[2];
            PointF rectCenter = new PointF((p1.X + p2.X) / 2f, (p1.Y + p2.Y) / 2f);

            return GetRotateOffset(rectCenter, center, angle);
        }
        #endregion

        #region 通用计算
        /// <summary>
        /// 获取移动后的Rect
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="xOff"></param>
        /// <param name="yOff"></param>
        /// <param name="wOff"></param>
        /// <param name="hOff"></param>
        /// <returns></returns>
        public static RectangleF OffsetRect(RectangleF rect, float xOff, float yOff, float wOff, float hOff)
        {
            if (xOff == 0 && yOff == 0 && wOff == 0 && hOff == 0)
                return rect;

            rect.X += xOff;
            rect.Y += yOff;
            rect.Width += wOff;
            rect.Height += hOff;

            return rect;
        }
        /// <summary>
        /// 获取边框移动后的偏移量
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="pos"></param>
        /// <param name="xOff"></param>
        /// <param name="yOff"></param>
        /// <param name="wOff"></param>
        /// <param name="hOff"></param>
        public static void GetOffset(PointF begin, PointF end, int pos, 
			out float xOff, out float yOff, out float wOff, out float hOff, float orthoTan)
        {
            xOff = 0;
            yOff = 0;
            wOff = 0;
            hOff = 0;
            float xOffset = end.X - begin.X;
            float yOffset = end.Y - begin.Y;

			if (orthoTan != 0)
				yOffset = xOffset * orthoTan;

            switch (pos)
            {
                case 0:
                    xOff = xOffset;
                    yOff = yOffset;
                    wOff = -xOff;
                    hOff = -yOff;
                    break;
                case 1:
                    yOff = yOffset;
                    hOff = -yOffset;
                    break;
                case 2:
                    yOff = yOffset;
                    wOff = xOffset;
                    hOff = -yOffset;
                    break;
                case 3:
                    xOff = xOffset;
                    wOff = -xOffset;
                    break;
                case 4:
                    wOff = xOffset;
                    break;
                case 5:
                    xOff = xOffset;
                    wOff = -xOffset;
                    hOff = yOffset;
                    break;
                case 6:
                    hOff = yOffset;
                    break;
                case 7:
                    wOff = xOffset;
                    hOff = yOffset;
                    break;
                default:
                    break;
            }
        }
        public static PointF GetFixPointRate(float xOff, float yOff, float wOff, float hOff)
        {
            PointF p = new PointF(0.5f, 0.5f);
            if (xOff == 0 && yOff == 0)                       //TopLeft
            {
                p.X = 0;
                p.Y = 0;
            }
            else if ((xOff + wOff) == 0 && yOff == 0)        //TopRight
            {
                p.X = 1;
                p.Y = 0;
            }
            else if (xOff == 0 && (yOff + hOff) == 0)       //BottomLeft
            {
                p.X = 0;
                p.Y = 1;
            }
            else if ((xOff + wOff) == 0 && (yOff + hOff) == 0)//BottomRight
            {
                p.X = 1;
                p.Y = 1;
            }

            return p;
        }
        /// <summary>
        /// 获取缩放后的Rect
        /// </summary>
        /// <param name="center"></param>
        /// <param name="xScale"></param>
        /// <param name="yScale"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static RectangleF GetScaleRect(PointF center, float xScale, float yScale, RectangleF rect)
        {
            //无缩放，返回原值
            if (xScale == 1 && yScale == 1)
                return rect;

			rect.Width = rect.Width * xScale;
			rect.Height = rect.Height * yScale;
			rect.X = center.X + (rect.X - center.X)*xScale;
        	rect.Y = center.Y + (rect.Y - center.Y)*yScale;

			return rect;
        }
		/// <summary>
		/// 正交模式下，获取Rect的宽高比例值
		/// </summary>
		/// <param name="data"></param>
		/// <param name="pos"></param>
		/// <param name="orthoTan"></param>
		/// <returns></returns>
		public static float CalcOrthoTan(IDrawData data, int pos)
		{
			if (pos == 0 || pos == 2 || pos == 5 || pos == 7)
			{
				float tan = data.Rect.Height / data.Rect.Width;
				if (pos == 2 || pos == 5)
					tan = -tan;
				return tan;
			}

			return 0;
		}
        #endregion

        #region limit
        public static RectangleF LimitRect(ref RectangleF value)
        {
            //一开始是设为0.0001f,但是发现会在设置LinearGradientBrush时出现OutOfMemory
            const float min = 0.001f;
        	const float max = 10000f;
			value.X = (value.X > -max) ? value.X : -max;
			value.X = (value.X < max) ? value.X : max;
			value.Y = (value.Y > -max) ? value.Y : -max;
			value.Y = (value.Y < max) ? value.Y : max;
			value.Width = (value.Width > min) ? value.Width : min;
			value.Width = (value.Width < max) ? value.Width : max;
			value.Height = (value.Height > min) ? value.Height : min;
			value.Height = (value.Height < max) ? value.Height : max;

            return value;
        }
        public static float LimitAngle(ref float value)
        {
            //限制在[0,360)
            value = value % 360;
            if (value < 0)
                value += 360;

            return value;
        }
        public static void LimitOffset(RectangleF rect, ref float xOff, ref float yOff, ref float wOff, ref float hOff)
        {
            const float min = 1f;

            if (rect.Width - xOff < min)
                xOff = rect.Width - min;
            if (rect.Height - yOff < min)
                yOff = rect.Height - min;

            if (rect.Width + wOff < min)
                wOff = min - rect.Width;
            if (rect.Height + hOff < min)
                hOff = min - rect.Height;
        }
        public static void LimitShear(ref float value)
        {
            //范围[-5,5]
            const int max = 5;
            const int min = -max;

            value = (value >= min) ? value : min;
            value = (value <= max) ? value : max;
        }
        public static void LimitScale(ref float value)
        {
            //范围[0.1, 10]
            const float max = 10;
            const float min = 1 / max;
            value = (value >= min) ? value : min;
            value = (value <= max) ? value : max;
        }
        public static void LimitScalePoint(ref PointF value)
        {
            const int max = 10;
            const int min = -max;
            value.X = (value.X >= min) ? value.X : min;
            value.X = (value.X <= max) ? value.X : max;
            value.Y = (value.Y >= min) ? value.Y : min;
            value.Y = (value.Y <= max) ? value.Y : max;
        }
        #endregion


    }

}
