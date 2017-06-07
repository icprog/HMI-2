using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.Drawing.Drawing2D;
using NetSCADA6.Common.NSColorManger;
using NetSCADA6.HMI.NSDrawObj.PropertyEdit;
using NetSCADA6.NSInterface.HMI.DrawObj;
using NetSCADA6.NSInterface.HMI.Var;

namespace NetSCADA6.HMI.NSDrawObj
{
    /// <summary>
    /// 矢量控件，基类
    /// </summary>
	public abstract class DrawVector : DrawObj, IDrawVector, IDrawData
    {
		protected DrawVector()
		{
			_brush.InitData(Color.Green, Color.Yellow, 45);
			_pen.InitData(Color.Blue, 1f);
		}
		
		#region IDrawVector
		#region IDrawObj
		#region property
		private GraphicsPath _matrixPath = new GraphicsPath();
        public override GraphicsPath MatrixPath
        {
            get { return _matrixPath; }
        }
        private GraphicsPath _path = new GraphicsPath();
        public override GraphicsPath Path
        {
            get { return _path; }
        }
        public override float WidenWidth
        {
            get { return _pen.Data.Width/2 + GraphicsTool.WidenWidth; }
        }

        #endregion

		#region public function
		public override void Draw(Graphics g)
		{
			//优化：如果是单位矩阵，状态保存操作就不需要了
			bool isIdentity = Matrix.IsIdentity;
			GraphicsState state = null;
			if (!isIdentity)
			{
				state = g.Save();
				g.MultiplyTransform(Matrix, MatrixOrder.Prepend);
			}

			OnPaint(g);

			if (!isIdentity)
				g.Restore(state);
		}
		#endregion
		#endregion

		#region property
		private PenManager _pen = new PenManager();
		[Browsable(false)]
		public PenManager Pen
		{
			get { return _pen; }
		}

		[Category("外观")]
		[DisplayName("线条")]
		[Description("图形的线条样式")]
		public PenData PenData
		{
			set
			{
				_pen.Data = value;

				_pen.InitContent(Rect, BasePath);
				//线宽可能发生变化，需要重新计算刷新区域
				GenerateBound();
				Invalidate();
			}
			get { return _pen.Data; }
		}
		private BrushManager _brush = new BrushManager();
		[Browsable(false)]
    	public BrushManager Brush
    	{
			get { return _brush; }
    	}
		[Category("外观")]
		[DisplayName("填充")]
		[Description("图形的填充方式")]
        public BrushData BrushData
        {
			set
			{
				_brush.Data = value;

				_brush.InitContent(Rect, BasePath);
				Invalidate();
			}
			get { return _brush.Data; }
        }
        private float _rotateAngle;
		/// <summary>
		/// 旋转角度
		/// </summary>
		[Category("布局")]
		[DisplayName("旋转角度")]
		[Description("控件的旋转角度，范围[-360,360]")]
		[PropertyOrder(100)]
        public float RotateAngle
        {
            set
            {
                Calculation.LimitAngle(ref value);

                if (_rotateAngle == value)
                    return;

                _rotateAngle = value;
                LoadGeneratePathEvent(false);
            }
            get { return _rotateAngle; }
        }
        private float _shear;
        private void SetShear(float value)
        {
            Calculation.LimitShear(ref value);

            if (_shear == value)
                return;

            DataBk.State = ControlState.Shear;
            _shear = value;
            SetRect(CalculateOffset());
        }
		/// <summary>
		/// 水平倾斜
		/// </summary>
		[Category("布局")]
		[DisplayName("倾斜度")]
		[Description("控件的倾斜度，范围[-5,5]")]
		[PropertyOrder(103)]
        public float Shear
        {
            set
            {
                BackupData();
                SetShear(value);
            } 
            get { return _shear; }
        }
        private PointF _rotatePoint = new PointF(0.5f, 0.5f);
        private void SetRotatePoint(PointF value)
        {
            if (_rotatePoint == value)
                return;

            DataBk.State = ControlState.Center;
            _rotatePoint = value;
            SetRect(CalculateOffset());
        }
		/// <summary>
		/// 旋转中心点
		/// </summary>
		[Category("布局")]
		[DisplayName("旋转中心点，比例系数")]
		[Description("控件的旋转中心点，比例系数")]
		[PropertyOrder(101)]
		[DecimalDigits(3)]
		[TypeConverter(typeof(PointFConverter))]
        public PointF RotatePoint
        {
            set
            {
                BackupData();
                SetRotatePoint(value);
            }
            get { return _rotatePoint; }
        }
        private PointF _rotatePointPos;
		/// <summary>
		/// 旋转中心点，实际坐标
		/// </summary>
		[Category("布局")]
		[DisplayName("旋转中心点，实际坐标")]
		[Description("控件的旋转中心点，实际坐标")]
		[PropertyOrder(102)]
		[DecimalDigits(0)]
		[TypeConverter(typeof(PointFConverter))]
        public PointF RotatePointPos
        {
			set
			{
				BackupData();
				MouseRotatePoint(value);
			}
			get { return _rotatePointPos; }
        }
        private PointF _scalePoint = new PointF(0.5f, 0.5f);
		/// <summary>
		/// 缩放中心点，比例系数
		/// </summary>
		[Category("布局")]
		[DisplayName("缩放中心点，比例系数")]
		[Description("控件的缩放中心点，比例系数")]
		[PropertyOrder(104)]
		[DecimalDigits(3)]
		[TypeConverter(typeof(PointFConverter))]
        public PointF ScalePoint
        {
            set
            {
                Calculation.LimitScalePoint(ref value);

                if (_scalePoint == value)
                    return;

                _scalePoint = value;
                LoadGeneratePathEvent(false);
            }
            get { return _scalePoint; }
        }
        private float _xScale = 1;
        private void SetXScale(float value)
        {
            Calculation.LimitScale(ref value);

            if (_xScale == value)
                return;

            DataBk.State = ControlState.XScale;
            _xScale = value;
            SetRect(CalculateOffset());
        }
		/// <summary>
		/// X轴缩放比例
		/// </summary>
		[Browsable(false)]
        public float XScale
        {
            set
            {
                BackupData();
                SetXScale(value);
            }
            get { return _xScale; }
        }
        private float _yScale = 1;
        private void SetYScale(float value)
        {
            Calculation.LimitScale(ref value);
                
            if (_yScale == value)
                return;

            DataBk.State = ControlState.YScale;
            _yScale = value;
            SetRect(CalculateOffset());
        }
		/// <summary>
		/// Y轴缩放比例
		/// </summary>
		[Browsable(false)]
        public float YScale
        {
            set
            {
                BackupData();
                SetYScale(value);
            }
            get { return _yScale; }
        }
		private EditMode _editMode;
		public virtual EditMode EditMode
		{
			set
			{
				Invalidate();
				if (value == EditMode.Normal)
					_editMode = value;
				else if (value == EditMode.Segment && this is ISegmentEdit)
					_editMode = value;
				else if (value == EditMode.Node && this is INodeEdit)
					_editMode = value;
				Invalidate();
			}
			get { return _editMode; }
		}
		[Browsable(false)]
		public virtual bool CanCombine { get { return true; } }
		#endregion
        #endregion

        #region public function
        /// <summary>
        /// 边框移动，参数均是偏移量
        /// </summary>
        /// <param name="xOff"></param>
        /// <param name="yOff"></param>
        /// <param name="wOff"></param>
        /// <param name="hOff"></param>
        protected override void FrameMove(float xOff, float yOff, float wOff, float hOff)
        {
            DataBk.State = ControlState.FrameMove;
            DataBk.FixRate = Calculation.GetFixPointRate(xOff, yOff, wOff, hOff);
            NewRect = Calculation.OffsetRect(DataBk.Rect, xOff, yOff, wOff, hOff);
            SetRect(CalculateOffset());
        }
        protected RectangleF BaseBoundMove(float xOff, float yOff, float wOff, float hOff)
        {
            //计算边框移动后的数据
            RectangleF rf;
            Calculation.GetBoundMoveData(DataBk, wOff, hOff, out _rotateAngle, out _shear, out rf);
            NewRect = rf;

            DataBk.State = ControlState.BoundMove;
            DataBk.Offset = new PointF(xOff, yOff);
            return CalculateOffset();
        }
        public override void BoundMove(float xOff, float yOff, float wOff, float hOff)
        {
            SetRect(BaseBoundMove(xOff, yOff, wOff, hOff));
        }
        public override bool IsVisible(PointF point)
        {
            return Path.IsVisible(point);
        }
        protected internal RectangleF BaseGroupRotate(float angle, PointF center)
        {
            DataBk.State = ControlState.GroupRotate;
            DataBk.Offset = Calculation.GetRotateAtOffset(DataBk, center, angle);
            _rotateAngle = DataBk.RotateAngle + angle;
            Calculation.LimitAngle(ref _rotateAngle);
            return CalculateOffset();
        }
        public virtual void GroupRotate(float angle, PointF center)
        {
            SetRect(BaseGroupRotate(angle, center)); 
        }
		private static readonly Pen _outlinePen = new Pen(Brushes.Black, GraphicsTool.WidenWidth);
		public Pen GetOutlinePen()
		{
			if (Pen != null && Pen.Content != null && Pen.Content.Width > _outlinePen.Width)
				return Pen.Content;
			return _outlinePen;
		}
        #endregion

		#region calculate matrix
		//原始矩阵和成组矩阵相乘后的实际矩阵
        private readonly Matrix _groupMatrix = new Matrix();
        private Matrix _baseMatrix = new Matrix();
		[Browsable(false)]
        public virtual Matrix Matrix
        {
            get
            {
                if (GroupParant != null)
                    return _groupMatrix;

                return _baseMatrix;
            }
        }
        protected internal override void GenerateMatrix()
        {
            if (BasePath.PointCount == 0)
				return;
			
			_matrixPath.Reset();
            _matrixPath.AddPath(BasePath, true);
            _path.Reset();
            _path.AddPath(BasePath, true);
            
            Calculation.CalcMatrix(this, ref _baseMatrix, ref _rotatePointPos);
            //成组时，叠加矩阵
            if (GroupParant != null && GroupParant.Matrix != null)
            {
                _groupMatrix.Reset();
                _groupMatrix.Multiply(_baseMatrix);
                _groupMatrix.Multiply(GroupParant.Matrix, MatrixOrder.Append);
            }

            _matrixPath.Transform(_baseMatrix);
            _path.Transform(Matrix);
        }
        #endregion

        #region calculate mouse operation
        public void MouseRotatePoint(PointF point)
        {
        	PointF pf = Calculation.GetRotatePoint(DataBk.Matrix, point, DataBk.Rect);
			if (_isFlipX)
				pf.X = 1 - pf.X;
			if (_isFlipY)
				pf.Y = 1 - pf.Y;
			SetRotatePoint(pf);
        }
        public void MouseRotate(PointF point, bool isOrtho)
        {
            int angle  = (int)Calculation.GetRotateAngle(DataBk, point, RotatePointPos);
			if (isOrtho)
				angle = angle/15*15;
        	RotateAngle = angle;
        }
        public void MouseShear(PointF point, int pos, bool isOrtho)
        {
        	float shear = Calculation.GetShear(DataBk, point, pos);
			if (isOrtho)
				shear = (int)shear;
			SetShear(shear);
        }
        #endregion

        #region calculate offset
        /// <summary>
        /// 设置一个固定点，转换矩阵变化后，计算出偏移值，使得固定点的值保持不变
        /// </summary>
        protected RectangleF CalculateOffset()
        {
            bool isBound = (DataBk.State == ControlState.BoundMove);

            PointF oldPoint = Calculation.CalcFixPoint(DataBk, DataBk.FixRate, isBound);
            PointF newPoint = Calculation.CalcFixPoint(this, DataBk.FixRate, isBound);

            float xOff = oldPoint.X - newPoint.X + DataBk.Offset.X;
            float yOff = oldPoint.Y - newPoint.Y + DataBk.Offset.Y;

            return  Calculation.OffsetRect(NewRect, xOff, yOff, 0, 0);
        }
        protected internal override void BackupData()
        {
            base.BackupData();

            if (!IsVector)
                return;

            DataBk.RotateAngle = RotateAngle;
            DataBk.Shear = Shear;
            DataBk.RotatePoint = RotatePoint;
            DataBk.RotatePointPos = RotatePointPos;
            DataBk.XScale = XScale;
            DataBk.YScale = YScale;
            DataBk.ScalePoint = ScalePoint;

            DataBk.Matrix.Reset();
            DataBk.Matrix.Multiply(Matrix);

            DataBk.MatrixBound = Calculation.GetMatrixBounds(_baseMatrix, Rect);
            DataBk.Bound = Bound;

        	DataBk.IsFlipX = IsFlipX;
        	DataBk.IsFlipY = IsFlipY;
        }
        #endregion

		#region common
		#region serialize
		public override void Serialize(BinaryFormatter bf, Stream s)
        {
            base.Serialize(bf, s);
            
            const int version = 1;

            bf.Serialize(s, version);
            bf.Serialize(s, _rotateAngle);
            bf.Serialize(s, _shear);
            bf.Serialize(s, _rotatePoint);
            bf.Serialize(s, _scalePoint);
			bf.Serialize(s, _isFlipX);
			bf.Serialize(s, _isFlipY);

			_brush.Data.Serialize(bf, s);
			_pen.Data.Serialize(bf, s);
        }
        public override void Deserialize(BinaryFormatter bf, Stream s)
        {
            base.Deserialize(bf, s);
            
            int version = (int)bf.Deserialize(s);
            _rotateAngle = (float)bf.Deserialize(s);
            _shear = (float)bf.Deserialize(s);
            _rotatePoint = (PointF)bf.Deserialize(s);
            _scalePoint = (PointF)bf.Deserialize(s);
			_isFlipX = (bool)bf.Deserialize(s);
			_isFlipY = (bool)bf.Deserialize(s);

			_brush.Data.Deserialize(bf, s);
			_pen.Data.Deserialize(bf, s);
        }
        #endregion

        #region clone
        public override Object Clone()
        {
            var obj = (DrawVector)base.Clone();

            obj._matrixPath = (GraphicsPath)_matrixPath.Clone();
            obj._baseMatrix = _baseMatrix.Clone();
            obj._path = _path.Clone() as GraphicsPath;
        	obj._brush = _brush.Clone() as BrushManager;
        	obj._pen = _pen.Clone() as PenManager;
            return obj;
        }
        #endregion

		#region dispose
		protected override void DisposeResource()
		{
			if (Disposed)
			{
				_matrixPath.Dispose();
				_path.Dispose();

				if (_brush.Content != null)
					_brush.Content.Dispose();
				if (_pen.Content != null)
					_pen.Content.Dispose();

				_baseMatrix.Dispose();
				_groupMatrix.Dispose();
			}

			base.DisposeResource();
		}
		#endregion
		#endregion

		#region var
		private static readonly string[] _vectorPropertyNames = new[] { "DrawVector.Angle" };
		public new static string[] GetPropertyNames()
		{
			return _vectorPropertyNames;
		}
		public override void LoadDataChangedEvent(IPropertyExpression expression)
		{
			if (expression.Index.ClassType == (int)DrawType.Vector)
				DrawVectorDataChanged(expression);
			else
				base.LoadDataChangedEvent(expression);
		}
		private void DrawVectorDataChanged(IPropertyExpression expression)
		{
			switch (expression.Index.Index)
			{
				case 0:     //angle
					RotateAngle = (float)expression.DecimalValue;
					break;
				default:
					break;
			}
		}
        #endregion

        #region virtual property
        public override bool IsVector { get { return true; } }
        #endregion

        #region group
        //解组操作
        public override void UngroupOper()
        {
			if (IsVector)
            {
				_isFlipX = _isFlipX ^ GroupParant.IsFlipX;
				_isFlipY = _isFlipY ^ GroupParant.IsFlipY;
				RectangleF rect;
                Calculation.GetMatrixData(this, out _rotateAngle, out _shear, out rect);
				//上一行需要GroupParant，下一行需要GroupParant为null，所以这一行千万不能删
            	GroupParant = null;
                SetRect(rect);
            }

			base.UngroupOper();
        }
        #endregion

		#region paint
		protected override void OnPaint(Graphics g)
		{
			_brush.Draw(g, Rect, BasePath);
			_pen.Draw(g, Rect, BasePath);
		}
		#endregion

		#region protected function
		protected override void GeneratePath()
		{
			base.GeneratePath();

			_brush.Generate(Rect, BasePath);
			_pen.Generate(Rect, BasePath);
		}
		protected internal override void GenerateBound()
		{
			const float shearEpsilon = 0.01f;
			const float penMax = 2f;
			
			Pen p = _pen.Content;
			//画笔的宽度较大时，转角长度会很长，需要特别计算Bound
			if (p != null && p.Width > penMax)
			{
				//存在倾斜，需要特别计算
				Bound = (Math.Abs(_shear) > shearEpsilon) ? 
					Calculation.GetPenAndMatrixBounds(BasePath, p, Matrix) : Calculation.GetPenBounds(Path, p);
			}
			else
			{
				base.GenerateBound();
			}
		}
		protected override void InitContent()
		{
			if (_brush.Content == null)
				_brush.InitContent(Rect, BasePath);
			if (_pen.Content == null)
				_pen.InitContent(Rect, BasePath);
		}
		#endregion

		#region flip
		private void SetFlip(bool value, bool isX)
		{
			if (isX)
				_isFlipX = value;
			else
				_isFlipY = value;
			_rotateAngle = -_rotateAngle;
			Calculation.LimitAngle(ref _rotateAngle);
			_shear = -_shear;
			if (isX)
				_rotatePoint.X = 1 - RotatePoint.X;
			else
				_rotatePoint.Y = 1 - RotatePoint.Y;

			DataBk.State = ControlState.Center;
			SetRect(CalculateOffset());
		}
    	private bool _isFlipX;
		[Browsable(false)]
		public bool IsFlipX 
		{ 
			set
			{
				if (_isFlipX == value)
					return;
				BackupData();
				SetFlip(value, true);
			}
			get { return _isFlipX; }
		}
		private bool _isFlipY;
		[Browsable(false)]
		public bool IsFlipY
		{
			set
			{
				if (_isFlipY == value)
					return;
				BackupData();
				SetFlip(value, false);
			}
			get { return _isFlipY; }
		}
    	#endregion
    }
}
