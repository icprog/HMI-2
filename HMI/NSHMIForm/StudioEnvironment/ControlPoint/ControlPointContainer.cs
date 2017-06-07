/********************************************************************************
** 作者： hdp
** 创始时间：2012.04.13
**
** 控制点索引说明
** 0--1--2
** 3-- --4
** 5--6--7
********************************************************************************/

using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using NetSCADA6.HMI.NSDrawObj;
using NetSCADA6.NSInterface.HMI.DrawObj;
using NetSCADA6.NSInterface.HMI.Form;

namespace NetSCADA6.HMI.NSHMIForm
{
    /// <summary>
    /// 控制点
    /// </summary>
    internal class ControlPointContainer
    {
        public ControlPointContainer(IHMIForm container)
        {
			Debug.Assert(container != null);

			_container = container;
			_selectObjs = new SelectObjectManager(this);

			_node = new NodePoint(_selectObjs);
			_custom = new CustomPoint(_selectObjs);
			_frame = new FramePoint(_selectObjs);
			_segment = new SegmentPoint(_selectObjs);
			_form = new FormPoint((HMIForm)_container);
        }

		#region const
		public const int PointSize = 8;
		#endregion

		#region field
		//节点
		private readonly NodePoint _node;
		//自定义点
		private readonly CustomPoint _custom;
		//边框点
		private readonly FramePoint _frame;
		//切割点
		private readonly SegmentPoint _segment;
		//窗体
    	private readonly FormPoint _form;
		#endregion

		#region property
		//选中的控件
		private readonly SelectObjectManager _selectObjs;
		public SelectObjectManager SelectObjs { get { return _selectObjs; } }
		private readonly IHMIForm _container;
		public IHMIForm Container
    	{
			get { return _container; }
    	}
		public float FormScale
		{
    		get { return Container.Studio.FormScale; }
		}
		#endregion

		#region private function
		//获取网格模式下鼠标指针值
		private PointF GetGridPointF(ControlState state, PointF point)
		{
			bool isGrid = ((Studio)_container.Studio).IsGrid;
			switch (state)
			{
				case ControlState.Move:
				case ControlState.FrameMove:
				case ControlState.Center:
				case ControlState.MoveNode:
				case ControlState.AddNode:
				case ControlState.FormWidth:
				case ControlState.FormHeight:
					if (isGrid)
						point = Tool.GetGridPointF(point);
					break;
				default:
					break;
			}

			return point;
		}
		private void Calculate()
		{
			_invalidateRect = RectangleF.Empty;
			_rotateCenterRect = RectangleF.Empty;

			switch (_selectObjs.StudioMode)
			{
				case EditMode.Normal:
					_frame.Calculate(FormScale, ref _invalidateRect, ref _rotateCenterRect);
					if (_selectObjs.IsCutomEdit)
						_custom.Calculate(FormScale, ref _invalidateRect);
					break;
				case EditMode.Node:
					_node.Calculate(FormScale, ref _invalidateRect);
					break;
				case EditMode.Segment:
					_segment.Calculate(FormScale, ref _invalidateRect);
					break;
			}

			int width = GraphicsTool.WidenWidth;
			_invalidateRect.Inflate(width, width);
			_rotateCenterRect.Inflate(width, width);
		}
		private void MouseOperation(PointF point)
		{
			bool isOrtho = ((Studio)_container.Studio).IsOrtho;
			point = GetGridPointF(_state, point);

			switch (_state)
            {
                case ControlState.Move:
                    _selectObjs.MouseMove(point);
                    break;
                case ControlState.FrameMove:
                    _selectObjs.MouseFrameMove(point, _pos, isOrtho);
                    break;
                case ControlState.Center:
                    _selectObjs.MouseRotatePoint(point);
                    break;
                case ControlState.Rotate:
					_selectObjs.MouseRotate(point, isOrtho);
                    break;
                case ControlState.Shear:
					_selectObjs.MouseShear(point, _pos, isOrtho);
                    break;
                case ControlState.Custom:
                    _selectObjs.MouseCustom(point, _pos);
                    break;
				case ControlState.MoveNode:
					_selectObjs.MoveNode(point, _pos);
					break;
				case ControlState.FormWidth:
				case ControlState.FormHeight:
					_form.ChangeFormSize(point, _state);
					break;
                default:
                    break;
            }
        }
		private void MouseDownOperation(PointF point)
		{
			point = GetGridPointF(_state, point);
			
			switch (_state)
			{
				case ControlState.AddNode:
					//已有节点的点，不重复添加
					if (!_node.IsVisible(point))
						_selectObjs.AddNode(point);
					break;
				case ControlState.DeleteNode:
					_selectObjs.DeleteNode(_pos);
					break;
				case ControlState.FormHeight:
				case ControlState.FormWidth:
					_form.MouseDown(point);
					break;
				default:
					_selectObjs.MouseDown(point, _state);
					break;
			}
		}

		private void GetState(PointF point, out ControlState state, out int index)
		{
			state = ControlState.None;
			index = 0;

			PointF pf = point;
			pf.X *= FormScale;
			pf.Y *= FormScale;

			#region 设置状态
			if (_selectObjs.IsSegmentEdit)
			{
				state = ControlState.Segment;
				return;
			}
			if (_selectObjs.IsNodeEdit)
			{
				if (_node.CanOperate(pf, ref state, ref index))
					return;
			}
			if (_frame.CanOperate(pf, ref state, ref index))
				return;
			if (_selectObjs.IsCutomEdit)
			{
				if (_custom.CanOperate(pf, ref state, ref index))
					return;
			}
			if (_selectObjs.IsVisible(point))
			{
				state = ControlState.Move;
				return;
			}
			if (_form.CanOperate(point, ref state, ref index))
				return;
			#endregion
		}
		#endregion

		#region public functin
		public static PointF TransFormData(Matrix m, float scale, PointF data)
		{
			PointF pf = data;
			pf = Calculation.GetMatrixPos(m, pf);
			pf.X *= scale;
			pf.Y *= scale;
			return pf;
		}
		#endregion

		#region IControlPoint
		#region property
		public bool IsSelected
        {
            get { return (!_selectObjs.IsEmpty); }
        }
        private bool _needInvalidate = true;
		/// <summary>
		/// 控件刷新时是否需要刷新控制点
		/// </summary>
        public bool NeedInvalidate
        {
            set { _needInvalidate = value; }
            get { return _needInvalidate; }
        }

    	private RectangleF _invalidateRect;
		public RectangleF InvalidateRect { get { return _invalidateRect; } }
    	private RectangleF _rotateCenterRect;
		public RectangleF RotateCenterRect { get { return _rotateCenterRect; } }
        #endregion

        #region public function
    	public bool Contains(IDrawObj obj)
        {
            return _selectObjs.List.Contains(obj);
        }
        /// <summary>
        /// 设置当前选中控件
        /// </summary>
        /// <param name="list"></param>
        public void ChangeSelectObj(List<IDrawObj> list)
        {
            //选中同一个控件，不做操作
			if (list!= null && list.Count == 1 && _selectObjs.IsSingle 
			        && list[0] == _selectObjs.Obj)
			    return;
			
			Invalidate();
			_selectObjs.Reset(list);
			Invalidate();
        }
        public void Draw(Graphics g)
        {
            if (_selectObjs.IsEmpty)
                return;

			//LastSelectObj
			if (_selectObjs.LastSelectedObj != null)
			{
				Pen pen = new Pen(Color.FromArgb(160, 255, 0, 0), 4);
				float scale = FormScale;
				GraphicsState state = g.Save();
				g.ScaleTransform(scale, scale);
				g.DrawPath(pen, _selectObjs.LastSelectedObj.Path);
				g.Restore(state);
			}

			if (_selectObjs.IsSegmentEdit)
				_segment.Draw(g);
			else if (_selectObjs.IsNodeEdit)
				_node.Draw(g);
			else
			{
				if (_selectObjs.IsCutomEdit)
					_custom.Draw(g);
				_frame.Draw(g);
			}
        }
        public void Invalidate()
        {
            Calculate();
			((Studio)_container.Studio).InvalidateControlPoint(this);
        }
		/// <summary>
		/// 计算后控制点刷新
		/// </summary>
		public void CalculateAndInvalidate()
		{
			_selectObjs.CalculateProperty();
			Invalidate();
		}
        #endregion
        #endregion

        #region mouse operation
        private ControlState _state;
        private int _pos;
        /// <summary>
        /// 是否在鼠标操作中
        /// </summary>
        private bool _mouseOperation;
        public bool MouseDown(PointF point)
        {
			GetState(point, out _state, out _pos);

            _mouseOperation = (_state != ControlState.None);
            if (_mouseOperation)
				MouseDownOperation(point);

            return (_state != ControlState.None && _state != ControlState.Move);
        }
        public void MouseUp(PointF point)
        {
			_mouseOperation = false;
            _selectObjs.MouseUp(point);
        }
        public void MouseMove(PointF point, out ControlState state, out int pos)
        {
            if (!_mouseOperation)
                GetState(point, out _state, out _pos);

            state = _state;
            pos = _pos;
            
            if (_mouseOperation)
				MouseOperation(point);
        }
		#endregion

    }
}
