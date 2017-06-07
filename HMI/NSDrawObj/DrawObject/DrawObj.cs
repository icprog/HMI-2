using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

using NetSCADA6.HMI.NSDrawObj.PropertyEdit;
using NetSCADA6.HMI.NSDrawObj.Var;
using NetSCADA6.NSInterface.HMI.DrawObj;
using NetSCADA6.NSInterface.HMI.Form;
using NetSCADA6.NSInterface.HMI.Var;

namespace NetSCADA6.HMI.NSDrawObj
{
	[TypeConverter(typeof(PropertySorter))]
	public abstract class DrawObj : IDrawObj, ICloneable
    {
        protected DrawObj()
        {
			_paraManager = new ExpressionMananger(this);
		}

        #region IDrawObj
        #region property
		#region Rect
		/// <summary>
		/// 设置Rect时，需要先写入NewRect，然后计算，再将NewRect赋值给Rect
		/// </summary>
		/// <param name="rf"></param>
		protected virtual void SetNewRect(RectangleF rf)
		{
			Calculation.LimitRect(ref rf);
			_newRect = rf;
		}
        private RectangleF _newRect;
        protected internal RectangleF NewRect
        {
            set { SetNewRect(value); }
            get { return _newRect; }
        }
        protected void SetDirectRect(RectangleF rf)
        {
        	NewRect = rf;
			_rect = NewRect;
        }
        private RectangleF _rect;
        protected internal virtual void SetRect(RectangleF rect)
        {
			//if (Parant.Undo != null)
			//	Parant.Undo.Push(a => Rect = a, _rect, "Change Rectangle");
			NewRect = rect;
			LoadGeneratePathEvent();
        }
		[Category("布局")]
		[DisplayName("位置")]
		[Description("控件的位置")]
		[PropertyOrder(1)]
		[TypeConverter(typeof(RectangleFConverter))]
        public virtual RectangleF Rect
        {
            set
            {
				BackupData();
                SetRect(value);
            }
            get { return _rect; }
        }
		#endregion

        private GraphicsPath _basePath = new GraphicsPath();
		/// <summary>
		/// 原始控件路径
		/// </summary>
		[Browsable(false)]
        public GraphicsPath BasePath
        {get { return _basePath; } }
		[Browsable(false)]
        public virtual GraphicsPath MatrixPath
        { get { return _basePath; } }
		[Browsable(false)]
        public virtual GraphicsPath Path
        { get { return _basePath; } }
		private string _name;
		[Category("通用")]
		[DisplayName("名称")]
		[Description("控件的名称")]
		public string Name
		{
			set
			{
				#warning 需要验证名称有效
				value.Trim();
				if (!string.IsNullOrWhiteSpace(value))
				{
					if (string.Compare(_name, value, true) == 0)
						return;
					if (Parant.Studio.NameManager.ContainsName(value))
						return;

					_name = value;
					Parant.Studio.NameManager.AddName(value);
				}
			}
			get { return _name; }
		}
		[Browsable(false)]
        public IDrawGroup GroupParant { set; get; }
		[Browsable(false)]
        public virtual float WidenWidth
        {
            get { return GraphicsTool.WidenWidth; }
        }
    	private RectangleF _bound;
		[Browsable(false)]
		public RectangleF Bound
		{
			set { _bound = value; }
			get { return _bound; }
		}
        #endregion

        #region public function
		public virtual void Draw(Graphics g)
		{
			OnPaint(g);
		}
        public virtual void MouseFrameMove(PointF point, int pos)
        {
			//正交模式
			float orthoTan = 0;
			if (Parant.Studio.IsOrtho)
				orthoTan = Calculation.CalcOrthoTan(DataBk, pos);
			
			float x, y, width, height;
            Calculation.GetFrameOffset(DataBk, point, pos, out x, out y, out width, out height, orthoTan);
            FrameMove(x, y, width, height);
        }
        public virtual bool IsVisible(PointF point)
        {
            return _rect.Contains(point);
        }
        public virtual void BoundMove(float xOff, float yOff, float wOff, float hOff)
        {
            FrameMove(xOff, yOff, wOff, hOff);
        }
        public virtual void MouseDown(PointF point)
        {
        	BackupMousePos(point);
            BackupData();
        }
        public virtual void MouseMove(PointF point)
        {
            SetRect(Calculation.OffsetRect(DataBk.Rect, point.X - DataBk.MousePos.X, point.Y - DataBk.MousePos.Y, 0, 0));
        }
        #endregion
        #endregion

        #region refresh
        public void Invalidate()
        {
            if (Parant == null)
                return;

            if (GroupParant != null && GroupParant.GroupInvalideate)
                return;
            
            Parant.Common.InvalidateObject(this);
        }
        protected virtual void GeneratePath()
        {
            SetDirectRect(NewRect);
            _basePath.Reset();
            OnGeneratePath(ref _basePath);
        }
        protected internal virtual void GenerateMatrix() { }
        protected internal virtual void GenerateBound()
        {
            //如果使用区域，Invalidate函数效率很低，改为使用RectangleF
			_bound = Path.GetBounds();
        }
        #endregion

        #region protected function
        /// <summary>
        /// 边框移动
        /// </summary>
        /// <param name="xOff"></param>
        /// <param name="yOff"></param>
        /// <param name="wOff"></param>
        /// <param name="hOff"></param>
        protected virtual void FrameMove(float xOff, float yOff, float wOff, float hOff)
        {
            SetRect(Calculation.OffsetRect(DataBk.Rect, xOff, yOff, wOff, hOff));
        }
        #endregion

		#region common
		#region serialize
		public virtual void Serialize(BinaryFormatter bf, Stream s)
        {
            const int version = 1;

            bf.Serialize(s, version);
            bf.Serialize(s, _rect);
            bf.Serialize(s, _name);
			bf.Serialize(s, _visible);
			bf.Serialize(s, _layer);
			
			_paraManager.Serialize(bf, s);
        }
        public virtual void Deserialize(BinaryFormatter bf, Stream s)
        {
            int version = (int)bf.Deserialize(s);
            _rect = (RectangleF) bf.Deserialize(s);
            _name = (string) bf.Deserialize(s);
        	_visible = (bool) bf.Deserialize(s);
        	_layer = (int) bf.Deserialize(s);

			_paraManager.Deserialize(bf, s);
        }
        #endregion

        #region clone
        public virtual object Clone()
        {
            var obj =  (DrawObj)MemberwiseClone();
            obj._basePath = BasePath.Clone() as GraphicsPath;
            obj._dataBk = _dataBk.Clone() as CalcData;
        	obj._paraManager = _paraManager.Clone() as ExpressionMananger;
            return obj;
        }
        #endregion

		#region dispose
		private bool _disposed;
		protected bool Disposed
		{
			get { return _disposed; }
		}
		public void Dispose()
		{
			DisposeResource();

			GC.SuppressFinalize(this);
		}
		protected virtual void DisposeResource()
		{
			if (!_disposed)
			{
				_basePath.Dispose();
				_dataBk.Dispose();

				_disposed = true;
			}

		}
		~DrawObj()
		{
			DisposeResource();
		}
		#endregion
		#endregion

		#region group
		//解组操作
        public virtual void UngroupOper()
        {
            GroupParant = null;
        }
        #endregion

        #region var
		private static readonly string[] _objPropertyNames = new[] 
		{ "DrawObj.X", "DrawObj.Y", "DrawObj.Width", "DrawObj.Height" };
		public static string[] GetPropertyNames()
		{
			return _objPropertyNames;
		}
		[Browsable(false)]
		public virtual string[] PropertyNames { get { return null; } }
        private ExpressionMananger _paraManager;
		[Browsable(false)]
		public ExpressionMananger ParaManager
        {
            get { return _paraManager; }
        }
		public virtual void GenerateIndex(Dictionary<string, IPropertyIndex> dict)
		{
			_paraManager.GenerateIndex(dict);
		}
		public virtual void AnalyseExpression(Dictionary<string, IParameter> dict)
		{
			foreach (IPropertyExpression expression in _paraManager.List)
			{
				expression.ParameterList.Clear();
				string s = expression.Expression;
				if (string.IsNullOrWhiteSpace(s))
					continue;

				#warning 分析表达式，取出变量列表
				List<string> varNames = new List<string> { s };

				foreach (string name in varNames)
				{
					IParameter p;

					if (dict.ContainsKey(name))
						p = dict[s];
					else
					{
						p = new Parameter(s);
						dict.Add(s, p);
					}
					p.List.Add(expression);
					expression.ParameterList.Add(p);
				}
			}
		}
		protected virtual void OnDataChanged(IPropertyExpression expression){}
		public virtual void LoadDataChangedEvent(IPropertyExpression expression)
		{
			if (expression.Index.ClassType == (int)DrawType.Obj)
				DrawObjDataChanged(expression);
			else
				OnDataChanged(expression);
		}
		private void DrawObjDataChanged(IPropertyExpression expression)
		{
                switch (expression.Index.Index)
                {
                    case 0:     //x
                        {
                            RectangleF r = Rect;
                            r.X = (float)expression.DecimalValue;
                            Rect = r;
                        }
                        break;
                    case 1:     //y
                        {
                            RectangleF r = Rect;
                            r.Y = (float)expression.DecimalValue;
                            Rect = r;
                        }
                        break;
                    case 2:     //width
                        {
                            RectangleF r = Rect;
                            r.Width = (float)expression.DecimalValue;
                            Rect = r;
                        }
                        break;
                    case 3:     //height
                        {
                            RectangleF r = Rect;
                            r.Height = (float)expression.DecimalValue;
                            Rect = r;
                        }
                        break;
                    default:
                        break;
                       
                }
		}

        #endregion

        #region virtual property
		[Browsable(false)]
        public abstract DrawType Type { get; }
		[Browsable(false)]
        public virtual bool IsVector { get { return false; } }
		[Browsable(false)]
        public virtual IHMIForm Parant { set; get; }
        #endregion

        #region data backup
        private CalcData _dataBk = new CalcData();
		[Browsable(false)]
        public CalcData DataBk
        {
            get { return _dataBk; }
        }
        protected internal virtual void BackupData()
        {
            DataBk.Rect = Rect;
            DataBk.MatrixBound = Rect;
			DataBk.Bound = Rect;
        }
		protected internal virtual void BackupMousePos(PointF point)
		{
			DataBk.MousePos = point;
		}
        #endregion

        #region event
        protected virtual void OnSizeChanged() { }
        protected virtual void OnLocationChanged() { }
        protected virtual void OnMouseDown(MouseButtons button, float x, float y) { }
        protected virtual void OnMouseUp(MouseButtons button, float x, float y) { }
		protected virtual void OnMouseMove(MouseButtons button, float x, float y) { }
		protected virtual void OnMouseEnter() { }
		protected virtual void OnMouseLeave() { }
		protected virtual void OnPaint(Graphics g) { }
		protected virtual void OnInitialization() { }
		/// <summary>
        /// 生成控件路径
        /// </summary>
        /// <param name="path"></param>
        protected virtual void OnGeneratePath(ref GraphicsPath path)
        {
            path.AddRectangle(Rect);
        }
		/// <summary>
		/// 初始化Brush和Pen
		/// </summary>
		protected virtual void InitContent() { }
        

		public virtual void LoadMouseEvent(MouseButtons button, PointF location, MouseType type)
        {
            PointF pf = location;
            if (IsVector)
                pf = Calculation.GetInvertPos(((IDrawVector) this).Matrix, location);
            
            switch (type)
            {
                case MouseType.Down:
                    OnMouseDown(button, pf.X - Rect.X, pf.Y - Rect.Y);
                    break;
                case MouseType.Up:
                    OnMouseUp(button, pf.X - Rect.X, pf.Y - Rect.Y);
                    break;
                case MouseType.Move:
                    OnMouseMove(button, pf.X - Rect.X, pf.Y - Rect.Y);
                    break;
                default:
                    break;
            }
        }
        private RectangleF _rectBk;
        protected void LoadSizeEvent()
        {
            if (_rectBk.Location != _rect.Location)
                OnLocationChanged();
            if (_rectBk.Size != _rect.Size)
                OnSizeChanged();
            _rectBk = _rect;
        }
        protected internal virtual void LoadGeneratePathEvent(bool needCalculatePath = true)
        {
			Invalidate();

			if (needCalculatePath)
			    GeneratePath();
			GenerateMatrix();
			GenerateBound();

			Invalidate();

			if (needCalculatePath)
			    LoadSizeEvent();
        }
        public virtual void LoadInitializationEvent()
        {
            _newRect = _rect;

            GeneratePath();
            GenerateMatrix();
            GenerateBound();
        	InitContent();

			OnInitialization();
            LoadSizeEvent();
        }
		public void LoadMouseEnterEvent() {OnMouseEnter();}
		public void LoadMouseLeaveEvent() {OnMouseLeave();}
        #endregion

		#region Layer
		private bool _visible = true;
		/// <summary>
		/// 可见
		/// </summary>
		[Browsable(false)]
		public bool Visible
		{
			set
			{
				_visible = value;
				Parant.Common.ObjectCanSelectChanged(this);
			}
			get { return _visible; }
		}
		[Browsable(false)]
		public bool FinalVisible
		{
			get
			{
				//如果是成组控件，取顶层父控件的属性
				IDrawObj group = GroupParant;
				while (group != null)
				{
					if (group.GroupParant == null)
						return group.FinalVisible;
					
					group = group.GroupParant;
				}
				
				
				return Parant.Common.VisibleLayers[_layer] && _visible;
			}
		}
		/// <summary>
		/// 锁定
		/// </summary>
		[Browsable(false)]
		public bool Locked
		{
			get
			{
				//如果是成组控件，取顶层父控件的属性
				IDrawObj group = GroupParant;
				while (group != null)
				{
					if (group.GroupParant == null)
						return group.Locked;

					group = group.GroupParant;
				}

				
				return Parant.Common.LockedLayers[_layer];
			}
		}
		public bool CanSelect()
		{
			return (FinalVisible && !Locked);
		}
		public bool CanSelect(PointF point)
		{
			if (CanSelect())
				return IsVisible(point);

			return false;
		}
		public bool CanSelect(RectangleF rect)
		{
			if (CanSelect())
				return rect.Contains(Path.GetBounds());
			
			return false;
		}
		private int _layer;
		public int Layer
		{
			set
			{
				if (_layer >= 0 && _layer <= 9)
				{
					_layer = value;
					Parant.Common.ObjectCanSelectChanged(this);
				}
			}
			get { return _layer; }
		}
		#endregion

		#region temp
		/// <summary>
		/// 变量绑定列表
		/// </summary>
		[Category("变量")]
		[DisplayName("变量绑定")]
		[Description("用于将控件的属性和变量绑定")]
		[EditorAttribute(typeof(BindVariableEditor), typeof(UITypeEditor))]   
    	public List<IPropertyExpression> PropertyList
    	{
			get { return _paraManager.List; }
    	}
		#endregion


    }
}
