using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using NetSCADA6.HMI.NSDrawObj;
using NetSCADA6.NSInterface.HMI.DrawObj;
using NetSCADA6.NSInterface.HMI.Form;

using UndoMethods;

namespace NetSCADA6.HMI.NSHMIForm
{
    /// <summary>
    /// 编辑版环境
    /// </summary>
	internal partial class Studio : Environment, IStudio
    {
        public Studio(HMIForm control):base(control)
        {
			_create = new CreateDrawObject(this);
        	_controlPoint = new ControlPointContainer(Container);

        	InitScrollControl();
        }

        #region public function
		protected override bool IsInvalidateObject(IDrawObj obj, Rectangle InvalidateRect)
		{
			const int width = 3;
			RectangleF rect = GetScrollAndScaleRect(obj.Bound);
			rect.Inflate(width, width);
			return rect.IntersectsWith(InvalidateRect);
		}
        public override void Paint(object sender, PaintEventArgs e)
        {
        	Graphics g = e.Graphics;
        	g.SmoothingMode = SmoothingMode.AntiAlias;
        	float scale = FormScale;
        	Size size = Rect.Size;

			g.TranslateTransform(Container.AutoScrollPosition.X, Container.AutoScrollPosition.Y);
			GraphicsState state = g.Save();
			g.SetClip(new Rectangle(0, 0, (int)(size.Width * scale), (int)(size.Height * scale)));
			g.ScaleTransform(scale, scale);
			base.Paint(sender, e);
			g.Restore(state);

			_controlPoint.Draw(e.Graphics);
        }
        //绘制背景
        public override void DrawBackground(Graphics g)
        {
			g.Clear(SystemColors.ControlDarkDark);

			float scale = FormScale;
			Size size = Container.Rect.Size;
			GraphicsState state = g.Save();
			g.TranslateTransform(Container.AutoScrollPosition.X, Container.AutoScrollPosition.Y);
			g.SetClip(new Rectangle(0, 0, (int)(size.Width * scale), (int)(size.Height * scale)));
			g.ScaleTransform(scale, scale);

			base.DrawBackground(g);
			//分隔线
            const int interval = 100;
			for (int i = 0; i < size.Width / interval; i++)
				g.DrawLine(Pens.Gray, (i + 1) * interval, 0, (i + 1) * interval, size.Height);
			for (int j = 0; j < size.Height / interval; j++)
				g.DrawLine(Pens.Gray, 0, (j + 1) * interval, size.Width, (j + 1) * interval);

			g.Restore(state);
        }
		public override void ObjectCanSelectChanged(IDrawObj obj)
		{
			base.ObjectCanSelectChanged(obj);

			//如果是不可操作图层，从选择列表中删除
			if (!obj.CanSelect())
			{
				if (_selectedObjs.Contains(obj))
				{
					_selectedObjs.Remove(obj);
					_controlPoint.ChangeSelectObj(_selectedObjs);
				}
			}
		}
		public void StatusLabelChanged()
		{
			Point point;
			Size size;
			SelectObjectManager objs = _controlPoint.SelectObjs;

			if (objs.IsEmpty)	//form
			{
				point = Container.Rect.Location;
				size = Container.Rect.Size;
			}
			else							//drawobj
			{
				point = Point.Ceiling(objs.Rect.Location);
				size = Size.Ceiling(objs.Rect.Size);
			}

			Container.toolStripLocation.Text = string.Format("{0,-4},{1,-4}", point.X, point.Y);
			Container.toolStripSize.Text = string.Format("{0,-4}x{1,-4}", size.Width, size.Height);
		}
		public void SelectedObjChanged()
		{
			Container.SetToolStripEnabled();
		}

		#region mouse
		public override void MouseMove(object sender, MouseButtons button, PointF location)
		{
			PointF pf = GetRevertScrollAndScalePointF(location);
			if (_create.MouseMove(button, location, pf, _downPoint))
				return;

			base.MouseMove(sender, button, pf);

			//选择框
			if (_isSelectFrame)
			{
				DrawFrame(_downPoint, location, FrameStyle.Dashed, false);
				return;
			}

			ControlState state;
			int pos;
			_controlPoint.MouseMove(pf, out state, out pos);

			switch (state)
			{
				case ControlState.Move:
					Container.Cursor = Cursors.NoMove2D;
					break;
				case ControlState.Shear:
					Container.Cursor = Cursors.VSplit;
					break;
				case ControlState.Center:
				case ControlState.Custom:
					Container.Cursor = Cursors.Hand;
					break;
				case ControlState.Rotate:
					Container.Cursor = Cursors.No;
					break;
				case ControlState.FrameMove:
					Container.Cursor = Cursors.SizeAll;
					break;
				case ControlState.MoveNode:
				case ControlState.AddNode:
				case ControlState.DeleteNode:
					Container.Cursor = Cursors.Hand;
					break;
				case ControlState.FormWidth:
					Container.Cursor = Cursors.SizeWE;
					break;
				case ControlState.FormHeight:
					Container.Cursor = Cursors.SizeNS;
					break;
				case ControlState.Segment:
					break;
				default:
					Container.Cursor = Cursors.Default;
					break;
			}
		}
		public override void MouseDown(object sender, MouseButtons button, PointF location)
		{
			PointF pf = GetRevertScrollAndScalePointF(location);
			_downPoint = location;

			if (_create.MouseDown(button, location, pf))
				return;

			base.MouseDown(sender, button, pf);

			if (button != MouseButtons.Left)
				return;
			if (_controlPoint.MouseDown(pf))
				return;

			IDrawObj drawObj = Objs.LastOrDefault(obj => obj.CanSelect(pf));

			if (drawObj != null)
			{
				if (Control.ModifierKeys == Keys.Control)
				{
					if (_selectedObjs.Contains(drawObj))
						_selectedObjs.Remove(drawObj);
					else
						_selectedObjs.Add(drawObj);
				}
				else if (Control.ModifierKeys == Keys.Shift)
				{
					//最后选中的DrawObj位置移动到列表的最后
					if (_selectedObjs.Contains(drawObj))
						_selectedObjs.Remove(drawObj);
					_selectedObjs.Add(drawObj);
				}
				else
				{
					if (_selectedObjs.Contains(drawObj))
						_selectedObjs.Remove(drawObj);
					else
						_selectedObjs.Clear();
					_selectedObjs.Add(drawObj);
				}
			}
			else
			{
				_selectedObjs.Clear();
				_isSelectFrame = true;
				_rectBk = Rectangle.Empty;
			}

			_controlPoint.ChangeSelectObj(_selectedObjs);
			_controlPoint.MouseDown(pf);
		}
		public override void MouseUp(object sender, MouseButtons button, PointF location)
		{
			PointF pf = GetRevertScrollAndScalePointF(location);
			PointF revertDownPoint = GetRevertScrollAndScalePointF(_downPoint);

			if (_create.MouseUp(button, location, pf, revertDownPoint))
				return;

			base.MouseUp(sender, button, pf);

			if (_isSelectFrame)
			{
				DrawFrame(PointF.Empty, PointF.Empty, FrameStyle.Dashed, true);

				_selectedObjs.Clear();

				RectangleF rf = Tool.GetRect(Point.Round(_downPoint), Point.Round(location));
				rf = GetRevertScrollAndScaleRect(rf);

				foreach (IDrawObj obj in Objs)
				{
					if (obj.CanSelect(rf))
						_selectedObjs.Add(obj);
				}

				_controlPoint.ChangeSelectObj(_selectedObjs);
				_isSelectFrame = false;
			}

			_controlPoint.MouseUp(pf);
			RefreshProperty();
		}
		public void DoubleClick(object sender)
		{
			_create.End();
		}

		#endregion
		#endregion

		#region field
		private bool _isSelectFrame;
		private readonly List<IDrawObj> _selectedObjs = new List<IDrawObj>();
		#endregion

		#region property
		private readonly ObjName _nameManager = new ObjName();
    	/// <summary>
    	/// 控件名管理器
    	/// </summary>
    	public IObjName NameManager
    	{
    		get { return _nameManager; }
    	}
		private readonly CreateDrawObject _create;
		/// <summary>
		/// 控件构建类
		/// </summary>
    	public CreateDrawObject Create
    	{
			get { return _create; }
    	}
		private readonly UndoRedoManager _undo = new UndoRedoManager();
		/// <summary>
		/// Undo/Redo
		/// </summary>
    	public IUndoRedoManager Undo
    	{
			get { return _undo; }
    	}
    	private readonly ControlPointContainer _controlPoint;
		internal ControlPointContainer ControlPoint { get { return _controlPoint; } }
		public bool IsOrtho { get; set; }
		public bool IsGrid { get; set; }
		#endregion

		#region menu operation
		private static readonly List<IDrawObj> CopyList = new List<IDrawObj>();
        public void Copy()
        {
            CopyList.Clear();
            foreach (IDrawObj obj in _selectedObjs)
            {
                CopyList.Add(obj.Clone() as IDrawObj);
            }
        }
        public void Paste()
        {
            foreach (IDrawObj obj in CopyList)
            {
                CloneDrawObj(obj);
            }
        }
        public void Delete()
        {
            foreach (IDrawObj obj in _selectedObjs)
            {
                Objs.Remove(obj);
				obj.Invalidate();
            	_nameManager.RemoveName(obj);
            }
            _selectedObjs.Clear();
			_controlPoint.ChangeSelectObj(null);
        }
		/// <summary>
		/// 合并
		/// </summary>
		public void Combine()
		{
			if (_selectedObjs.Count > 1)
			{
				//需要排序，否则绘图的先后顺序会发生改变
				int[] indexA = new int[_selectedObjs.Count];
				for (int i = 0; i < indexA.Length; i++)
					indexA[i] = Objs.IndexOf(_selectedObjs[i]);
				Array.Sort(indexA);

				DrawCombine draw = new DrawCombine(_selectedObjs) {Parant = Container};
				_nameManager.CreateName(draw);
				draw.Layer = DefaultLayer;
				draw.LoadInitializationEvent();

				foreach (IDrawObj obj in _selectedObjs)
				{
					Objs.Remove(obj);
				}
				Objs.Insert(indexA[0], draw);
				_selectedObjs.Clear();
				if (draw.CanSelect())
					_selectedObjs.Add(draw);

				_controlPoint.ChangeSelectObj(_selectedObjs);
				Container.Invalidate();
			}
		}

        #endregion

        #region toolbox
        public IDrawObj StudioCreateDrawObj(Type type, RectangleF rf)
        {
        	const int limit = 5;
        	const int v = 100;
			if (rf.Width < limit || rf.Height < limit)
			{
				rf.Width = v;
				rf.Height = v;
			}
			
			Undo.StartTransaction("Create");
			IDrawObj obj = CreateDrawObj(type);
        	CreateDrawObjOper(obj);

			obj.Parant = Container;
			_nameManager.CreateName(obj);
			obj.Rect = rf;
			obj.Layer = DefaultLayer;
			obj.LoadInitializationEvent();

			Undo.EndTransaction();

            return obj;
        }
    	private void CreateDrawObjOper(IDrawObj obj)
    	{
			//Undo.Push(RemoveDrawObjOper, obj, "Create");
			Objs.Add(obj);
		}
		private void RemoveDrawObjOper(IDrawObj obj)
		{
			//Undo.Push(CreateDrawObjOper, obj, "Remove");
			Objs.Remove(obj);
		}
        #endregion

        #region private function
        private IDrawObj CloneDrawObj(IDrawObj origObj)
        {
			IDrawObj newObj = (IDrawObj)origObj.Clone();
            newObj.Parant = Container;
            _nameManager.CreateName(newObj);
			newObj.LoadInitializationEvent();
			Objs.Add(newObj);
			newObj.Invalidate();
            return newObj;
        }
		private PointF _downPoint;
		private Rectangle _rectBk;
		/// <summary>
		///  绘制方框
		/// </summary>
		/// <param name="point1"></param>
		/// <param name="point2"></param>
		/// <param name="style"></param>
		/// <param name="isEliminate">是否是消除绘制</param>
		internal void DrawFrame(PointF point1, PointF point2, FrameStyle style, bool isEliminate)
		{
			Rectangle rect = Tool.GetRect(
				Container.PointToScreen(Point.Round(point1)), Container.PointToScreen(Point.Round(point2)));

			if (IsGrid)
				rect = Tool.GetGridRect(rect);

			ControlPaint.DrawReversibleFrame(_rectBk, Color.Black, style);
			if (isEliminate)
				_rectBk = Rectangle.Empty;
			else
			{
				ControlPaint.DrawReversibleFrame(rect, Color.Black, style);
				_rectBk = rect;
			}
		}
        #endregion

		#region virtual function
		public override void Initialization()
		{
			base.Initialization();
			_nameManager.ResetDict(Objs);
			SetScrollSize();
		}
		protected override float GetFormScale()
		{
			return FormScale;
		}
		public override string GetCaption()
		{
			return System.IO.Path.GetFileNameWithoutExtension(FullName);
		}
		#endregion

		#region dispose
		protected override void DisposeResource()
		{
			if (!Disposed)
			{
				int count = CopyList.Count;
				for (int i = 0; i < count; i++)
					CopyList[i].Dispose();
				CopyList.Clear();
			}

			base.DisposeResource();
		}
		#endregion

		#region refresh
		public override void InvalidateObject(IDrawObj sender)
		{
			const int width = 3;
			Rectangle rect = Rectangle.Round(GetScrollAndScaleRect(sender.Bound));
			rect.Inflate(width, width);

			Container.Invalidate(rect);

			if (_controlPoint.NeedInvalidate && _controlPoint.Contains(sender))
				_controlPoint.Invalidate();
		}
		public void InvalidateControlPoint(ControlPointContainer control)
		{
			Rectangle rect = Rectangle.Round(control.InvalidateRect);
			rect.Offset(Container.AutoScrollPosition);
			Container.Invalidate(rect);
			rect = Rectangle.Round(control.RotateCenterRect);
			rect.Offset(Container.AutoScrollPosition);
			Container.Invalidate(rect);
		}
		#endregion

		#region toolbox
		public void SetCreateObjectState(ToolboxItem item)
		{
			Create.End();

			string name = item.TypeName + "," + item.AssemblyName;
			Type t = Type.GetType(name);
			if (t == null)
				return;
			Create.Begin(t);
		}
		public void CreateToolboxItem(ToolboxItem item)
		{
			Create.End();

			string name = item.TypeName + "," + item.AssemblyName;
			Type t = Type.GetType(name);
			if (t == null)
				return;
			StudioCreateDrawObj(t, new RectangleF(new PointF(100, 100), new SizeF(100, 100)));
			Container.Framework.Manager.ResetToolboxPointerFunction();
		}
		#endregion

		#region drawobj status changed
		private void RefreshProperty()
    	{
			Container.Framework.Manager.RefreshPropertyFunction();
			StatusLabelChanged();
		}
		private void SelectObject(object obj)
		{
			Container.Framework.Manager.SelectObjectFunction(obj);

			StatusLabelChanged();
			SelectedObjChanged();
		}
    	public void ResetSelectObjs()
    	{
    		SelectObjectManager selectObjs = _controlPoint.SelectObjs;

			if (selectObjs.IsEmpty)
				SelectObject(_controlPoint.Container);
			else if (selectObjs.IsSingle)
				SelectObject(selectObjs.List[0]);
			else
				SelectObject(selectObjs);
		}
		#endregion

		#region form scroll and scale
		public override Rectangle Rect
		{
			set 
			{ 
				base.Rect = value;
				SetScrollSize();
			}
		}
		private float _formScale = 1f;
		/// <summary>
		/// 窗体缩放比例
		/// </summary>
		public float FormScale
		{
			set
			{
				const float min = 0.25f;
				const float max = 4;
				value = (value < min) ? min : value;
				value = (value > max) ? max : value;

				if (value != _formScale)
				{
					_formScale = value;

					SetScrollSize();
					_controlPoint.Invalidate();
					Container.Invalidate();
				}
			}
			get { return _formScale; }
		}
		//用于设置在窗体中，使得窗体的滚动条显示
		private readonly Control _scrollControl = new Button();
		private void InitScrollControl()
		{
			_scrollControl.Enabled = false;
			_scrollControl.Location = Point.Empty;
			_scrollControl.Size = Size.Empty;
			_scrollControl.Parent = Container;
		}
		private void SetScrollSize()
		{
			Size size = Rect.Size;
			float scale = FormScale;
			const int offset = 50;
			_scrollControl.Location = new Point(
				(int)(size.Width*scale)+offset, (int)(size.Height*scale)+offset);
		}
		internal PointF GetRevertScrollAndScalePointF(PointF point)
		{
			return new PointF(
				(point.X - Container.AutoScrollPosition.X) / FormScale, 
				(point.Y - Container.AutoScrollPosition.Y) / FormScale);
		}
		private RectangleF GetScrollAndScaleRect(RectangleF rect)
		{
			rect.X = rect.X * FormScale + Container.AutoScrollPosition.X;
			rect.Y = rect.Y * FormScale + Container.AutoScrollPosition.Y;
			rect.Width *= FormScale;
			rect.Height *= FormScale;
			return rect;
		}
		private RectangleF GetRevertScrollAndScaleRect(RectangleF rect)
		{
			rect.X = (rect.X - Container.AutoScrollPosition.X) / FormScale;
			rect.Y = (rect.Y - Container.AutoScrollPosition.Y) / FormScale;
			rect.Width /= FormScale;
			rect.Height /= FormScale;
			return rect;
		}
    	#endregion


    }

}
