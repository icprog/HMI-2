using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using NetSCADA6.HMI.NSDrawObj;
using NetSCADA6.NSInterface.HMI.DrawObj;

namespace NetSCADA6.HMI.NSHMIForm
{
	/// <summary>
	/// 控件创建管理类
	/// </summary>
	internal class CreateDrawObject
	{
		public CreateDrawObject(Studio container)
		{
			Debug.Assert(container != null);
			_studio = container;
		}

		#region enum
		/// <summary>
		/// 控件创建状态
		/// </summary>
		internal enum CreateState
		{
			NotCreate,			//未创建
			BeginCreate,		//预备创建
			HasCreate,			//已创建
		}
		#endregion

		#region field
		/// <summary>
		/// 控件创建过程状态
		/// </summary>
		private CreateState _state;
		/// <summary>
		/// 创建控件类型
		/// </summary>
		private Type _type;
		/// <summary>
		/// 节点控件,默认为null
		/// </summary>
		private IDrawVector _nodeObject;
		private readonly Studio _studio;
		// 是否是以节点控件方式创建
		private bool _isNodeCreate;
		#endregion

		#region ortho
		// 正交模式状态
		private OrthoMode _orthoState;
		//返回正交模式下正方形模式的点位置
		private PointF GetOrthoSquarePoint(PointF begin, PointF end)
		{
			if (_orthoState != OrthoMode.Square)
				return end;
			
			float w = end.X - begin.X;
			float h = end.Y - begin.Y;
			float absW = Math.Abs(w);
			float absH = Math.Abs(h);
			float dis = (absW < absH) ? absW : absH; 

			PointF pf = begin;
			if (w > 0)
				pf.X += dis;
			else
				pf.X -= dis;
			if (h > 0)
				pf.Y += dis;
			else
				pf.Y -= dis;

			return pf;
		}
		//返回正交模式下水平垂直模式的点位置
		private PointF GetOrthoHoriorVert(PointF begin, PointF end)
		{
			if (_orthoState != OrthoMode.HoriOrVert)
				return end;

			float w = end.X - begin.X;
			float h = end.Y - begin.Y;
			float absW = Math.Abs(w);
			float absH = Math.Abs(h);
			float absW1 = absW*(float) Math.Tan(Math.PI*30/180);
			float absW2 = absW*(float) Math.Tan(Math.PI*60/180);

			PointF pf = begin;
			if (absH <= absW1)
				pf.X = end.X;
			else if (absH > absW1 && absH < absW2)
			{
				pf.X += w;
				if (h >= 0)
					pf.Y += absW;
				else
					pf.Y -= absW;
			}
			else if (absH >= absW2)
				pf.Y = end.Y;

			return pf;
		}
		#endregion


		#region public function
		/// <summary>
		/// 开始创建控件
		/// </summary>
		/// <param name="type"></param>
		public void Begin(Type type)
		{
			End();

			_state = CreateState.BeginCreate;
			_type = type;
			_isNodeCreate = _type.IsSubclassOf(typeof(NSDrawNodes.DrawNodes));

			//ortho
			_orthoState = OrthoMode.Invalid;
			if (_studio.IsOrtho)
			{
				object[] attrs = _type.GetCustomAttributes(true);
				for (int i = 0; i < attrs.Length; i++)
				{
					if (attrs[i] is OrthoAttribute)
					{
						_orthoState = ((OrthoAttribute)attrs[i]).State;
						break;
					}
				}
			}
		}
		/// <summary>
		/// 创建控件终止
		/// </summary>
		public void End()
		{
			if (_nodeObject != null)
			{
				INodeEdit node = (INodeEdit)_nodeObject;

				//最后一个点为动态移动点，需要删除
				int count = node.NodeDatas.Count;
				if (count > 0)
					node.DeleteNode(count - 1);

				//创建成功
				if (node.CreateSuccess)
				{
					node.IsNodeCreating = false;
					_studio.ControlPoint.ChangeSelectObj(new List<IDrawObj>{_nodeObject});
					_nodeObject.Invalidate();
				}
				else
					_studio.Objs.Remove(_nodeObject);

				_nodeObject = null;
				_studio.Container.Framework.Manager.ResetToolboxPointerFunction();
			}

			_studio.Container.Cursor = Cursors.Default;
			_state = CreateState.NotCreate;
			_orthoState = OrthoMode.Invalid;
		}
		public bool MouseDown(MouseButtons button, PointF location, PointF revertPoint)
		{
			if (_studio.IsGrid)
				revertPoint = Tool.GetGridPointF(revertPoint);
			
			if (_state == CreateState.BeginCreate)
			{
				_state = CreateState.HasCreate;

				if (_nodeObject != null)
				{
					End();
					return true;
				}

				if (_isNodeCreate)
				{
					_nodeObject = (IDrawVector)_studio.CreateDrawObj(_type);
					_nodeObject.Parant = _studio.Container;
					((ObjName)_studio.NameManager).CreateName(_nodeObject);
					_studio.Objs.Add(_nodeObject);
					INodeEdit node = (INodeEdit) _nodeObject;
					node.IsNodeCreating = true;
					node.AddNode(revertPoint);
					node.AddNode(revertPoint);
					_nodeObject.LoadInitializationEvent();
				}

				return true;
			}

			if (_state == CreateState.HasCreate)
			{
				if (_isNodeCreate && _nodeObject != null)
				{
					INodeEdit node = (INodeEdit)_nodeObject;
					int count = node.NodeDatas.Count;
					PointF lastPoint = node.NodeDatas[count - 2];

					revertPoint = GetOrthoHoriorVert(lastPoint, revertPoint);
					//预防在同一位置重复添加点
					if (Point.Ceiling(lastPoint) == Point.Ceiling(revertPoint))
						return true;

					node.MoveNode(revertPoint, count - 1);
					node.AddNode(revertPoint);

					//添加点后创建成功，则直接结束
					if (node.CreateFinish)
					{
						End();
						return true;
					}
				}

				return true;
			}

			return false;
		}
		public bool MouseUp(MouseButtons button, PointF location, PointF revertPoint, PointF revertDownPoint)
		{
			if (_studio.IsGrid)
			{
				revertPoint = Tool.GetGridPointF(revertPoint);
				revertDownPoint = Tool.GetGridPointF(revertDownPoint);
			}
			
			if (_state != CreateState.NotCreate)
			{
				if (!_isNodeCreate)
				{
					if (_state == CreateState.HasCreate)
					{
						revertPoint = GetOrthoSquarePoint(revertDownPoint, revertPoint);
						_studio.DrawFrame(PointF.Empty, PointF.Empty, FrameStyle.Thick, true);

						IDrawObj obj = _studio.StudioCreateDrawObj(_type, Tool.GetRectF(revertDownPoint, revertPoint));
						if (obj != null)
							obj.Invalidate();
						End();
						_studio.Container.Framework.Manager.ResetToolboxPointerFunction();
					}

				}

				return true;
			}

			return false;
		}
		public bool MouseMove(MouseButtons button, PointF location, PointF revertPoint, PointF downPoint)
		{
			if (_studio.IsGrid)
				revertPoint = Tool.GetGridPointF(revertPoint);
			
			if (_state != CreateState.NotCreate)
			{
				_studio.Container.Cursor = Cursors.Cross;

				if (_isNodeCreate)
				{
					if (_nodeObject != null)
					{
						INodeEdit node = (INodeEdit)_nodeObject;
						int count = node.NodeDatas.Count;
						PointF lastPoint = node.NodeDatas[count - 2];

						revertPoint = GetOrthoHoriorVert(lastPoint, revertPoint);
						node.MoveNode(revertPoint, count - 1);
					}
				}
				else
				{
					location = GetOrthoSquarePoint(downPoint, location);
					if (_state == CreateState.HasCreate)
						_studio.DrawFrame(downPoint, location, FrameStyle.Thick, false);
				}

				return true;

			}

			return false;
		}
		#endregion
	}
}
