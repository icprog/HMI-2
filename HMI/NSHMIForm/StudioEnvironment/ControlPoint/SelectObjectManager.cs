using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using NetSCADA6.HMI.NSDrawObj;
using NetSCADA6.NSInterface.HMI.DrawObj;
using NetSCADA6.NSInterface.HMI.Form;
using NetSCADA6.NSInterface.HMI.Framework;
using UndoMethods;

namespace NetSCADA6.HMI.NSHMIForm
{
    /// <summary>
    /// 控件选择管理类
    /// </summary>
	internal class SelectObjectManager
    {
        public SelectObjectManager(ControlPointContainer controlPoint)
        {
        	Debug.Assert(controlPoint != null);
			_controlPoint = controlPoint;
        }

		#region field
    	private readonly ControlPointContainer _controlPoint;
		private readonly CalcData _dataBk = new CalcData();
		private ControlState _state;
		#endregion

		#region property
		#region edit setting
		private bool _isVector;
		/// <summary>
		/// 是否是矢量控件
		/// </summary>
		[Browsable(false)]
		public bool IsVector { get { return _isVector; } }
		/// <summary>
		///  是否是单个控件
		/// </summary>
		[Browsable(false)]
		public bool IsSingle
		{
			get { return (_list.Count == 1); }
		}
		/// <summary>
		///  是否是单个矢量控件
		/// </summary>
		[Browsable(false)]
		public bool IsSingleVector
		{
			get { return IsSingle && IsVector; }
		}
		/// <summary>
		/// 是否为空
		/// </summary>
		[Browsable(false)]
		public bool IsEmpty
		{
			get { return _list.Count == 0; }
		}
		[Browsable(false)]
		public IDrawObj Obj
		{
			get { return IsSingle ? _list[0] : null; }
		}
		[Browsable(false)]
		public IDrawVector Vector
		{
			get { return IsSingleVector ? (IDrawVector)_list[0] : null; }
		}

    	/// <summary>
    	/// 选择多个控件时，最后一个被选中的控件
    	/// </summary>
    	[Browsable(false)]
    	public IDrawObj LastSelectedObj { get; private set; }

    	[Browsable(false)]
		public INodeEdit Node
		{
			get
			{
				if (IsSingleVector && Vector is INodeEdit)
					return (INodeEdit)Vector;
				return null;
			}
		}
		[Browsable(false)]
		public ISegmentEdit Segment
		{
			get
			{
				if (IsSingleVector && Vector is ISegmentEdit)
					return (ISegmentEdit)Vector;
				return null;
			}
		}
		[Browsable(false)]
		public ICustomEdit Custom
		{
			get
			{
				if (IsCutomEdit)
					return (ICustomEdit)Vector;
				return null;
			}
		}
		/// <summary>
		/// 编辑模式
		/// </summary>
		[Browsable(false)]
		public EditMode StudioMode
		{
			get { return IsSingleVector ? Vector.EditMode : EditMode.Normal; }
		}
    	[Browsable(false)]
    	public bool IsCutomEdit
    	{
			get { return StudioMode == EditMode.Normal && IsSingleVector && (Vector is ICustomEdit); }
    	}
		[Browsable(false)]
		public bool IsNodeEdit { get { return StudioMode == EditMode.Node; } }
		[Browsable(false)]
		public bool IsSegmentEdit { get { return StudioMode == EditMode.Segment; } }
		[Browsable(false)]
		public bool IsAddNode
		{
			get { return (IsNodeEdit && Node.NodeState == NodesState.Add); }
		}
		[Browsable(false)]
		public bool IsDeleteNode
		{
			get { return (IsNodeEdit && Node.NodeState == NodesState.Delete); }
		}
		[Browsable(false)]
		public bool ExistGroup
		{
			get { return List.Any(obj => obj.Type == DrawType.Group); }
		}
		#endregion

		#region obj setting
		private RectangleF _rect;
		public RectangleF Rect { get { return IsSingle ? Obj.Rect : _rect; } }
        private float _shear;
		[Browsable(false)]
		public float Shear { get { return _shear; } }
        private float _rotateAngle;
		[Browsable(false)]
		public float RotateAngle 
        { 
            set
            {
                Calculation.LimitAngle(ref value);
                    
                if (_rotateAngle == value)
                    return;
                _rotateAngle = value;

                _matrix.Reset();
                _matrix.RotateAt(RotateAngle, RotatePointPos);
            }
			get { return _rotateAngle; }
        }
        private PointF _rotatePoint = new PointF(0.5f, 0.5f);
		[Browsable(false)]
		public PointF RotatePoint
        {
            set
            {
                if (_rotatePoint == value)
                    return;
                    
                _rotatePoint = value;
                SetRotatePointPos();
            }
			get { return _rotatePoint; }
        }
        private PointF _rotatePointPos;
		[Browsable(false)]
		public PointF RotatePointPos { get { return IsSingleVector ? Vector.RotatePointPos : _rotatePointPos; } }
        private readonly Matrix _matrix = new Matrix();
		[Browsable(false)]
		public Matrix Matrix { get { return IsSingleVector ? Vector.Matrix : _matrix; } }
        private readonly List<IDrawObj> _list = new List<IDrawObj>();
		[Browsable(false)]
		public ReadOnlyCollection<IDrawObj> List
        {
            get { return _list.AsReadOnly(); }
        }
		#endregion
        #endregion

		#region private function
		private void SetVectorSign()
		{
			_isVector = _list.All(obj => (obj.IsVector));
			if (IsEmpty)
				_isVector = false;
		}
		private void SetRotatePoint()
		{
			_rotatePoint = Calculation.GetRotatePoint(null, RotatePointPos, Rect);
		}
		private void SetRotatePointPos()
		{
			_rotatePointPos.X = Rect.X + Rect.Width * RotatePoint.X;
			_rotatePointPos.Y = Rect.Y + Rect.Height * RotatePoint.Y;
		}
		private void SetListRect()
		{
			RectangleF rf = RectangleF.Empty;
			foreach (IDrawObj obj in _list)
				rf = (rf == RectangleF.Empty) ? obj.Bound : RectangleF.Union(rf, obj.Bound);
			_rect = rf;
		}
		private void InitProperty()
		{
			if (IsEmpty || IsSingle)
			{
				_rotateAngle = 0;
				_rotatePoint = new PointF(0.5f, 0.5f);
				_shear = 0;
				_matrix.Reset();
			}
		}
		private void ResetProperty()
		{
			InitProperty();

			SetListRect();
			SetRotatePointPos();
		}
		private void Invalidate(bool needInvalidate = false)
		{
			_controlPoint.NeedInvalidate = needInvalidate;
			_controlPoint.Invalidate();
		}
		private void ResetLastSelectedObject(List<IDrawObj> objList)
		{
			LastSelectedObj = null;
			if (objList == null || objList.Count < 2)
				return;

			LastSelectedObj = objList[objList.Count - 1];
		}
		#endregion

		#region public function
        public void Reset(List<IDrawObj> objList)
        {
			ResetLastSelectedObject(objList);
        	Studio studio = (Studio)_controlPoint.Container.Studio;
			
			_list.Clear();
			if (objList == null)
			{
				studio.ResetSelectObjs();
				return;
			}

			_list.AddRange(objList);

			SetVectorSign();
			ResetProperty();

			studio.ResetSelectObjs();
        }
        public bool IsVisible(PointF point)
        {
			return _list.Any(obj => obj.CanSelect(point));
        }
		public void CalculateProperty()
		{
			SetListRect();
			SetRotatePointPos();
		}
        #endregion

        #region mouse operation
        public void MouseFrameMove(PointF point, int pos, bool isOrtho)
        {
			if (IsSingle)
				Obj.MouseFrameMove(point, pos);
            else
            {
                Invalidate();

				//正交模式
				float orthoTan = 0;
				if (isOrtho)
					orthoTan = Calculation.CalcOrthoTan(_dataBk, pos);

                float x, y, width, height;
				Calculation.GetOffset(_dataBk.MousePos, point, pos, out x, out y, out width, out height, orthoTan);
				Calculation.LimitOffset(_dataBk.Rect, ref x, ref y, ref width, ref height);

                float xOffRate = width / _dataBk.Rect.Width;
                float yOffRate = height / _dataBk.Rect.Height;

                foreach (IDrawObj obj in _list)
                {
                    RectangleF ojbRect = ((DrawObj)obj).DataBk.Bound;
                    float objX = x + (ojbRect.X - _dataBk.Rect.X) * xOffRate;
                    float objW = ojbRect.Width*xOffRate;
                    float objY = y + (ojbRect.Y - _dataBk.Rect.Y) * yOffRate;
                    float objH = ojbRect.Height*yOffRate;

                    obj.BoundMove(objX, objY, objW, objH);
                }

                ResetProperty();
                Invalidate(true);
            }
        }
        public void MouseRotatePoint(PointF point)
        {
            if (!IsVector)
                return;

			if (IsSingle)
				Vector.MouseRotatePoint(point);
            else
			{
				Invalidate();
				RotatePoint = Calculation.GetRotatePoint(_dataBk.Matrix, point, _dataBk.Rect);
				Invalidate(true);
			}
		}
        public void MouseRotate(PointF point, bool isOrtho)
        {
            if (!IsVector)
                return;

            if (IsSingle)
                Vector.MouseRotate(point, isOrtho);
            else
            {
                Invalidate();
                
                int angle = (int)Calculation.GetRotateAngle(_dataBk, point, RotatePointPos);
				//取15的整数
				if (isOrtho)
					angle = angle/15*15;
            	RotateAngle = angle;

                foreach (IDrawObj obj in _list)
                    ((DrawVector)obj).GroupRotate(RotateAngle, RotatePointPos);

                Invalidate(true);
            }
        }
        public void MouseShear(PointF point, int pos, bool isOrtho)
        {
            if (!IsSingleVector)
                return;

            Vector.MouseShear(point, pos, isOrtho);
        }
        public void MouseCustom(PointF point, int pos)
        {
			if (IsCutomEdit)
				Custom.MoveCustom(point, pos);
        }
		public void MoveNode(PointF point, int pos)
		{
			if (!IsNodeEdit)
				return;
			Invalidate();
			Node.MoveNode(point, pos);
			Invalidate(true);
		}
		public void AddNode(PointF point)
		{
			if (!IsNodeEdit)
				return;
			Invalidate();
			Node.AddNode(point);
			Invalidate(true);
		}
		public void DeleteNode(int pos)
		{
			if (!IsNodeEdit)
				return;
			Invalidate();
			Node.DeleteNode(pos);
			Invalidate(true);
		}
        public void MouseDown(PointF point, ControlState state)
        {
            foreach (IDrawObj o in _list)
                o.MouseDown(point);

            _state = state;
            _dataBk.Rect = _rect;
            _dataBk.MousePos = point;
        }
        public void MouseMove(PointF point)
        {
			IUndoRedoManager undo = null;
			if (_list.Count > 0)
			{
				IHMIForm form = _list[0].Parant;
				undo = form.Studio.Undo;
			}

            Invalidate();
            
			if (undo != null)
				undo.StartTransaction("Move");
            foreach (IDrawObj obj in _list)
                obj.MouseMove(point);
			if (undo != null)
				undo.EndTransaction();

            SetListRect();
            SetRotatePointPos();

            Invalidate(true);
        }
        public void MouseUp(PointF point)
        {
            Invalidate(true);
            
            _rotateAngle = 0;
            _matrix.Reset();

            if (_state == ControlState.Rotate)
            {
                SetListRect();
                SetRotatePoint();
                Invalidate(true);
            }

            ResetProperty();
        }
    	#endregion

        #region dispose
        private bool _disposed;
        public void Dispose()
        {
            DisposeResource();
            
            GC.SuppressFinalize(this);
        }
        private void DisposeResource()
        {
            if (!_disposed)
            {
            	_matrix.Dispose();

                _disposed = true;
            }
            
        }
		~SelectObjectManager()
		{
			DisposeResource();
		}
        #endregion

		#region flip
		public void FlipX()
		{
			if (IsSingle)
				Vector.IsFlipX = !Vector.IsFlipX;
			else
			{
				Invalidate();

				foreach (IDrawObj obj in _list)
				{
					IDrawVector v = (IDrawVector) obj;
					v.IsFlipX = !v.IsFlipX;

					float offset = Rect.Left + Rect.Right - v.Bound.Left*2 - v.Bound.Width;
					RectangleF rf = v.Rect;
					rf.X += offset;
					v.Rect = rf;
				}

				Invalidate(true);
			}
		}
		public void FlipY()
		{
			if (IsSingle)
				Vector.IsFlipY = !Vector.IsFlipY;
			else
			{
				Invalidate();

				foreach (IDrawObj obj in _list)
				{
					IDrawVector v = (IDrawVector)obj;
					v.IsFlipY = !v.IsFlipY;

					float offset = Rect.Top + Rect.Bottom - v.Bound.Top * 2 - v.Bound.Height;
					RectangleF rf = v.Rect;
					rf.Y += offset;
					v.Rect = rf;
				}

				Invalidate(true);
			}
		}
		#endregion
	}
}
