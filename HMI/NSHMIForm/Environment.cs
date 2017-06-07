using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using NetSCADA6.HMI.NSDrawNodes;
using NetSCADA6.HMI.NSDrawNodes.DrawObj;
using NetSCADA6.HMI.NSDrawObj;
using NetSCADA6.HMI.NSDrawVector;
using NetSCADA6.NSInterface.HMI.DrawObj;
using NetSCADA6.NSInterface.HMI.Form;

namespace NetSCADA6.HMI.NSHMIForm
{
    /// <summary>
    /// 运行环境
    /// </summary>
	internal abstract class Environment : IEnvironment
    {
        protected Environment(HMIForm control)
        {
        	Debug.Assert(control != null);
			
			_container = control;
			_backBrush.InitData(Color.LightGray);
        }
        
        #region virtual function
		public virtual void MouseMove(object sender, MouseButtons button, PointF location)
        {
            LoadMouseEvent(button, location, MouseType.Move);
        }
		public virtual void MouseDown(object sender, MouseButtons button, PointF location)
        {
			LoadMouseEvent(button, location, MouseType.Down);
        }
		public virtual void MouseUp(object sender, MouseButtons button, PointF location)
        {
			LoadMouseEvent(button, location, MouseType.Up);
        }
		public virtual void MouseLeave()
		{
			if (_mouseMovingObj != null)
			{
				_mouseMovingObj.LoadMouseLeaveEvent();
				_mouseMovingObj = null;
			}
		}
		public virtual void Initialization()
		{
			ResetPath(_path);
			_backBrush.InitContent(ZeroLocationRect, _path);
			
			//控件初始化
			int count = _objs.Count;
			for (int i = 0; i < count; i++)
			{
				_objs[i].Parant = _container;
				_objs[i].LoadInitializationEvent();
			}
		}
		protected virtual float GetFormScale()
		{
			return 1;
		}
		public virtual void ShowStyle()
		{
			Container.Show();
		}
    	public abstract string GetCaption();
		#endregion

		#region property
		private readonly List<IDrawObj> _objs = new List<IDrawObj>();
		public List<IDrawObj> Objs
		{
			get { return _objs; }
		}
		private readonly HMIForm _container;
		public HMIForm Container
        {
            get { return _container; }
        }
    	private string _fullName;
    	public string FullName
    	{
			set
			{
				string oldFileName = System.IO.Path.GetFileNameWithoutExtension(_fullName);
				_fullName = value;
				string fileName = System.IO.Path.GetFileNameWithoutExtension(_fullName);
				if (string.IsNullOrEmpty(_caption) || string.Compare(_caption, oldFileName, true) == 0)
					_caption = fileName;
			}
			get { return _fullName; }
    	}
    	private string _caption = string.Empty;
    	/// <summary>
    	/// 标题栏文本
    	/// </summary>
    	public string Caption
    	{
			set { _caption = value; }
			get { return _caption; }
    	}

		private readonly BrushManager _backBrush = new BrushManager();
		public BrushManager BackBrush
		{
			get { return _backBrush; }
		}
		private readonly GraphicsPath _path = new GraphicsPath();
		public GraphicsPath Path
		{
			get { return _path; }
		}
		private Rectangle _rect = new Rectangle(0, 0, 1000, 800);
    	public virtual Rectangle Rect
    	{
			set
			{
				_rect = value;
				ResetPath(_path);
			}
			get { return _rect; }
    	}
		/// <summary>
		/// 坐标值为零值的Rectangle
		/// </summary>
    	internal Rectangle ZeroLocationRect
    	{
    		get
    		{
    			Rectangle r = _rect;
				r.Location = Point.Empty;
				return r;
    		}
    	}
		private FormStyle _style = FormStyle.Normal;
		/// <summary>
		/// 窗体样式
		/// </summary>
		public FormStyle Style
		{
			set { _style = value; }
			get { return _style; }
		}
    	private FormBorderStyle _borderStyle = FormBorderStyle.Sizable;
    	/// <summary>
    	/// 窗体边框样式
    	/// </summary>
    	public FormBorderStyle BorderStyle
    	{
			set { _borderStyle = value; }
			get { return _borderStyle; }
    	}
		#endregion

		#region private function

    	private IDrawObj _mouseMovingObj;
		private void LoadMouseEvent(MouseButtons button, PointF location, MouseType type)
        {
			IDrawObj draw = Objs.LastOrDefault(obj => obj.CanSelect(location));

			if (_mouseMovingObj != draw)
			{
				if (_mouseMovingObj != null)
					_mouseMovingObj.LoadMouseLeaveEvent();
				_mouseMovingObj = draw;
				if (_mouseMovingObj != null)
					_mouseMovingObj.LoadMouseEnterEvent();
			}

			if (_mouseMovingObj != null)
				_mouseMovingObj.LoadMouseEvent(button, location, type);
        }
		private void ResetPath(GraphicsPath path)
		{
			path.Reset();
			path.AddRectangle(ZeroLocationRect);
		}
        #endregion


		public IDrawObj CreateDrawObj(Type type)
		{
			ConstructorInfo[] infos = type.GetConstructors();
			if (infos.Length > 0)
				return (IDrawObj)infos[0].Invoke(null);

			return null;
		}
		public IDrawObj CreateDrawObj(DrawType type)
		{
			//todo add control
			IDrawObj obj;
			switch (type)
			{
				case DrawType.Group:
					obj = new DrawGroup();
					break;
				case DrawType.Combine:
					obj = new DrawCombine();
					break;
				case DrawType.Rect:
					obj = new DrawRect();
					break;
				case DrawType.Ellipse:
					obj = new DrawEllipse();
					break;
				case DrawType.Text:
					obj = new DrawText();
					break;
				case DrawType.StraightLine:
					obj = new DrawStraightLine();
					break;
				case DrawType.FoldLine:
					obj = new DrawFoldLine();
					break;
				case DrawType.Bezier:
					obj = new DrawBezier();
					break;
				case DrawType.Polygon:
					obj = new DrawPolygon();
					break;
				case DrawType.ClosedBezier:
					obj = new DrawClosedBezier();
					break;
				default:
					return null;
			}

			return obj;
		}


		#region common
		#region serialize
		public void Save()
		{
			if (string.IsNullOrWhiteSpace(FullName))
				return;

			BinaryFormatter bf = new BinaryFormatter();
			using (Stream s = File.Open(FullName, FileMode.Create))
			{
				Container.Serialize(bf, s);
			}
		}
		public void Serialize(BinaryFormatter bf, Stream s)
		{
			const int version = 1;

			bf.Serialize(s, version);

			int objCount = _objs.Count;
			bf.Serialize(s, objCount);
			for (int i = 0; i < objCount; i++)
			{
				IDrawObj obj = _objs[i];

				int type = (int)obj.Type;
				bf.Serialize(s, type);
				obj.Serialize(bf, s);
			}

			bf.Serialize(s, _rect);
			bf.Serialize(s, (int)_style);
			bf.Serialize(s, (int)BorderStyle);
			bf.Serialize(s, _defaultLayer);
			bf.Serialize(s, _visibleLayers);
			bf.Serialize(s, _lockedLayers);
			bf.Serialize(s, _caption);
			
			_backBrush.Data.Serialize(bf, s);
		}
		public void Deserialize(BinaryFormatter bf, Stream s)
		{
			int version = (int)bf.Deserialize(s);

			int objCount = (int)bf.Deserialize(s);
			_objs.Clear();
			_objs.Capacity = objCount;
			for (int i = 0; i < objCount; i++)
			{
				DrawType type = (DrawType)bf.Deserialize(s);
				IDrawObj obj = CreateDrawObj(type);
				//DrawGroup控件需要提前获取Form，用来创建子控件
				if (obj.Type == DrawType.Group)
					obj.Parant = Container;
				obj.Deserialize(bf, s);
				_objs.Add(obj);
			}

			_rect = (Rectangle)bf.Deserialize(s);
			_style = (FormStyle) bf.Deserialize(s);
			BorderStyle = (FormBorderStyle) bf.Deserialize(s);
			_defaultLayer = (int) bf.Deserialize(s);
			_visibleLayers = (BitArray) bf.Deserialize(s);
			_lockedLayers = (BitArray) bf.Deserialize(s);
			_caption = (string) bf.Deserialize(s);

			_backBrush.Data.Deserialize(bf, s);
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
				int count = _objs.Count;
				for (int i = 0; i < count; i++)
					_objs[i].Dispose();

				_disposed = true;
			}

		}
		~Environment()
		{
			DisposeResource();
		}
		#endregion
		#endregion

		#region print
		public void Print()
		{
			PrintDocument printDoc = new PrintDocument();
			printDoc.PrintPage += EnvironmentPrint;
			printDoc.Print();

			printDoc.Dispose();
		}
		public void PrintPreview()
		{
			PrintDocument printDoc = new PrintDocument();
			printDoc.PrintPage += EnvironmentPrint;
			PrintPreviewDialog dlg = new PrintPreviewDialog { Document = printDoc };
			dlg.ShowDialog();

			dlg.Dispose();
			printDoc.Dispose();
		}
		public void PrintDialog()
		{
			PrintDocument printDoc = new PrintDocument();
			printDoc.PrintPage += EnvironmentPrint;
			PrintDialog dlg = new PrintDialog { Document = printDoc };
			if (dlg.ShowDialog() == DialogResult.OK)
				printDoc.Print();

			dlg.Dispose();
			printDoc.Dispose();
		}

    	private int _currentPage;
    	private int _currentCopies;
		private void EnvironmentPrint(object sender, PrintPageEventArgs e)
		{
			//页面类型
			PaperKind kind = e.PageSettings.PaperSize.Kind;
			#warning 页面自定义大小时，只能打印一页，否则会报异常,原因不明.暂时不允许自定义页面打印
			if (kind == PaperKind.Custom)
			{
				Trace.Assert(false, "暂时不支持自定义页面打印");
				return;
			}
			
			//计算当前页数和绘制区域
			RectangleF marginBound = e.MarginBounds;
			RectangleF totalBound = Container.Rect;
			totalBound.Location = PointF.Empty;
			float fx = (totalBound.Width / marginBound.Width);
			int xPages = (int)fx;
			if (fx > xPages)
				xPages++;
			float fy = (totalBound.Height / marginBound.Height);
			int yPages = (int)fy;
			if (fy > yPages)
				yPages++;
			int totalPage = xPages * yPages;
			int xIndex = _currentPage % xPages;
			int yIndex = _currentPage / xPages;
			//绘制区域
			RectangleF paintBound =
				new RectangleF(marginBound.Width*xIndex, marginBound.Height*yIndex, marginBound.Width, marginBound.Height);
			//实际绘制区域
			RectangleF factBound = RectangleF.Intersect(paintBound, totalBound);
			//打印份数
			int copies = e.PageSettings.PrinterSettings.Copies;

			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TranslateTransform(-(paintBound.X - marginBound.X), -(paintBound.Y - marginBound.Y));
			g.SetClip(factBound);

			//绘制背景
			EnvironmentDrawBackground(g);
			//绘制控件
			int count = Objs.Count;
			for (int i = 0; i < count; i++)
			{
				if (!Objs[i].Visible)
					continue;
				
				if (Objs[i].Bound.IntersectsWith(factBound))
					Objs[i].Draw(g);
			}

			e.HasMorePages = true;
			if (copies > 1)
			{
				if (_currentCopies >= (copies - 1))
					_currentCopies = 0;
				else
				{
					_currentCopies++;
					return;
				}
			}
			if (_currentPage >= (totalPage - 1))
			{
				e.HasMorePages = false;
				_currentPage = 0;
			}
			else
				_currentPage++;
		}
		#endregion

		#region Paint
		protected virtual bool IsInvalidateObject(IDrawObj obj, Rectangle InvalidateRect)
		{
			return true;
		}
		public virtual void Paint(object sender, PaintEventArgs e)
		{
			EnvironmentPaint(e);
		}
		public virtual void DrawBackground(Graphics g)
		{
			EnvironmentDrawBackground(g);
		}

		private void EnvironmentDrawBackground(Graphics g)
		{
			g.Clear(Color.LightGray);
			_backBrush.Draw(g, ZeroLocationRect, _path);
		}
		private void EnvironmentPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			//设置光滑
			g.SmoothingMode = SmoothingMode.AntiAlias;

			int count = Objs.Count;
			for (int i = 0; i < count; i++)
			{
				if (Objs[i].FinalVisible && IsInvalidateObject(Objs[i], e.ClipRectangle))
					Objs[i].Draw(g);
			}
		}
		#endregion

		#region refresh
		protected RectangleF RectInvalidate { set; get; }
		public void TimeInvalidate()
		{
			if (RectInvalidate != Rectangle.Empty)
			{
				Container.Invalidate(Rectangle.Ceiling(RectInvalidate));
				RectInvalidate = Rectangle.Empty;
			}
		}
		public virtual void InvalidateObject(IDrawObj sender){}
		#endregion

		#region layer
    	private int _defaultLayer;
    	private const int _layerCount = 10;
		/// <summary>
		/// 当前层
		/// </summary>
    	public int DefaultLayer
    	{
    		set
    		{
    			if (value >= 0 && value <= _layerCount - 1)
    			{
					_defaultLayer = value;
    			}
    		}
			get { return _defaultLayer; }
    	}

    	private BitArray _visibleLayers = new BitArray(_layerCount, true);
    	/// <summary>
    	/// 层的可见信息
    	/// </summary>
    	public BitArray VisibleLayers
    	{
    		get { return _visibleLayers; }
    	}
		private BitArray _lockedLayers  = new BitArray(_layerCount, false);
    	/// <summary>
    	/// 层的锁定信息
    	/// </summary>
    	public BitArray LockedLayers
    	{
			get { return _lockedLayers; }
    	}
		public virtual void ObjectCanSelectChanged(IDrawObj obj)
		{
			obj.Invalidate();
		}
    	#endregion


    }

}
