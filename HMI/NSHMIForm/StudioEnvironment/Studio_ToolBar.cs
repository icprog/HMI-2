using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NetSCADA6.HMI.NSDrawObj;
using NetSCADA6.NSInterface.HMI.DrawObj;

namespace NetSCADA6.HMI.NSHMIForm
{
	internal partial class Studio
	{
		#region layout
		#region private functin
		private enum LayoutType { Left, Top }
		private static float GetSortValue(IDrawObj obj, LayoutType type)
		{
			switch (type)
			{
				case LayoutType.Left:
					return obj.Bound.Left;
				case LayoutType.Top:
					return obj.Bound.Top;
				default:
					break;
			}

			return 0;
		}
		private static void SortList(List<IDrawObj> sorts, LayoutType type)
		{
			int count = sorts.Count;
			for (int i = 0; i < count; i++)
			{
				for (int j = i + 1; j < count; j++)
				{
					if (GetSortValue(sorts[j], type) < GetSortValue(sorts[i], type))
					{
						IDrawObj replace = sorts[j];
						sorts[j] = sorts[i];
						sorts[i] = replace;
					}
				}
			}
		}
		private const int MoveSize = 5;
		private void IncrSpace(LayoutType type)
		{
			IDrawObj last = _controlPoint.SelectObjs.LastSelectedObj;
			int count = _selectedObjs.Count;

			List<IDrawObj> sorts = new List<IDrawObj>(_selectedObjs);
			SortList(sorts, type);

			int lastIndex = sorts.IndexOf(last);
			for (int i = 0; i < count; i++)
			{
				IDrawObj obj = sorts[i];
				int index = i - lastIndex;
				RectangleF rf = obj.Rect;
				if (type == LayoutType.Left)
					rf.X += index * MoveSize;
				else if (type == LayoutType.Top)
					rf.Y += index * MoveSize;
				obj.Rect = rf;
			}
			_controlPoint.CalculateAndInvalidate();
		}
		#endregion

		#region public function
		public void AlignLeft(object sender, EventArgs e)
		{
			IDrawObj last = _controlPoint.SelectObjs.LastSelectedObj;
			foreach (IDrawObj obj in _selectedObjs)
			{
				if (obj == last)
					continue;

				RectangleF rf = obj.Rect;
				rf.Offset(last.Bound.X - obj.Bound.X, 0);
				obj.Rect = rf;
			}
			_controlPoint.CalculateAndInvalidate();
		}
		public void AlignCenter(object sender, EventArgs e)
		{
			IDrawObj last = _controlPoint.SelectObjs.LastSelectedObj;
			float f = last.Bound.X + last.Bound.Width / 2;
			foreach (IDrawObj obj in _selectedObjs)
			{
				if (obj == last)
					continue;

				RectangleF rf = obj.Rect;
				float offset = f - (obj.Bound.X + obj.Bound.Width / 2);
				rf.Offset(offset, 0);
				obj.Rect = rf;
			}
			_controlPoint.CalculateAndInvalidate();
		}
		public void AlignRight(object sender, EventArgs e)
		{
			IDrawObj last = _controlPoint.SelectObjs.LastSelectedObj;
			foreach (IDrawObj obj in _selectedObjs)
			{
				if (obj == last)
					continue;

				RectangleF rf = obj.Rect;
				float offset = last.Bound.Right - obj.Bound.Right;
				rf.Offset(offset, 0);
				obj.Rect = rf;
			}
			_controlPoint.CalculateAndInvalidate();
		}
		public void AlignTop(object sender, EventArgs e)
		{
			IDrawObj last = _controlPoint.SelectObjs.LastSelectedObj;
			foreach (IDrawObj obj in _selectedObjs)
			{
				if (obj == last)
					continue;

				RectangleF rf = obj.Rect;
				float offset = last.Bound.Top - obj.Bound.Top;
				rf.Offset(0, offset);
				obj.Rect = rf;
			}
			_controlPoint.CalculateAndInvalidate();
		}
		public void AlignMiddle(object sender, EventArgs e)
		{
			IDrawObj last = _controlPoint.SelectObjs.LastSelectedObj;
			float f = last.Bound.Top + last.Bound.Height / 2;
			foreach (IDrawObj obj in _selectedObjs)
			{
				if (obj == last)
					continue;

				RectangleF rf = obj.Rect;
				float offset = f - (obj.Bound.Top + obj.Bound.Height / 2);
				rf.Offset(0, offset);
				obj.Rect = rf;
			}
			_controlPoint.CalculateAndInvalidate();
		}
		public void AlignBottom(object sender, EventArgs e)
		{
			IDrawObj last = _controlPoint.SelectObjs.LastSelectedObj;
			foreach (IDrawObj obj in _selectedObjs)
			{
				if (obj == last)
					continue;

				RectangleF rf = obj.Rect;
				float offset = last.Bound.Bottom - obj.Bound.Bottom;
				rf.Offset(0, offset);
				obj.Rect = rf;
			}
			_controlPoint.CalculateAndInvalidate();
		}
		public void SameWidth(object sender, EventArgs e)
		{
			IDrawObj last = _controlPoint.SelectObjs.LastSelectedObj;
			float f = last.Bound.Width;
			foreach (IDrawObj obj in _selectedObjs)
			{
				if (obj == last)
					continue;

				RectangleF rf = obj.Rect;
				rf.Size = new SizeF(f, rf.Height);
				obj.Rect = rf;
			}
			_controlPoint.CalculateAndInvalidate();
		}
		public void SameHeight(object sender, EventArgs e)
		{
			IDrawObj last = _controlPoint.SelectObjs.LastSelectedObj;
			float f = last.Bound.Height;
			foreach (IDrawObj obj in _selectedObjs)
			{
				if (obj == last)
					continue;

				RectangleF rf = obj.Rect;
				rf.Size = new SizeF(rf.Width, f);
				obj.Rect = rf;
			}
			_controlPoint.CalculateAndInvalidate();
		}
		public void SameSize(object sender, EventArgs e)
		{
			IDrawObj last = _controlPoint.SelectObjs.LastSelectedObj;
			foreach (IDrawObj obj in _selectedObjs)
			{
				if (obj == last)
					continue;

				RectangleF rf = obj.Rect;
				rf.Size = last.Bound.Size;
				obj.Rect = rf;
			}
			_controlPoint.CalculateAndInvalidate();
		}
		public void HSpace(object sender, EventArgs e)
		{
			int count = _selectedObjs.Count;
			float left = _selectedObjs[0].Bound.Left;
			float right = _selectedObjs[0].Bound.Right;
			float width = _selectedObjs[0].Bound.Width;
			for (int i = 1; i < count; i++)
			{
				RectangleF rf = _selectedObjs[i].Bound;
				if (left > rf.Left)
					left = rf.Left;
				if (right < rf.Right)
					right = rf.Right;
				width += rf.Width;
			}
			float finalWdith = width > (right - left) ? width : right - left;
			float interval = (finalWdith - width) / (count - 1);
			List<IDrawObj> sorts = new List<IDrawObj>(_selectedObjs);
			SortList(sorts, LayoutType.Left);

			float current = left;
			for (int i = 0; i < count; i++)
			{
				IDrawObj obj = sorts[i];
				RectangleF rf = obj.Rect;
				rf.X += current - obj.Bound.X;
				obj.Rect = rf;
				current += obj.Bound.Width + interval;
			}

			_controlPoint.CalculateAndInvalidate();
		}
		public void IncrHSpace(object sender, EventArgs e)
		{
			IncrSpace(LayoutType.Left);
		}
		public void DecrHSpace(object sender, EventArgs e)
		{
			IDrawObj last = _controlPoint.SelectObjs.LastSelectedObj;
			int count = _selectedObjs.Count;
			float standard = last.Bound.X;

			List<IDrawObj> sorts = new List<IDrawObj>(_selectedObjs);
			SortList(sorts, LayoutType.Left);

			int lastIndex = sorts.IndexOf(last);
			for (int i = 0; i < count; i++)
			{
				IDrawObj obj = sorts[i];
				int index = i - lastIndex;
				RectangleF rf = obj.Rect;
				if (Math.Abs(standard - rf.X) > Math.Abs(index * MoveSize))
					rf.X -= index * MoveSize;
				else
					rf.X = standard;
				obj.Rect = rf;
			}
			_controlPoint.CalculateAndInvalidate();
		}
		public void VSpace(object sender, EventArgs e)
		{
			int count = _selectedObjs.Count;
			float top = _selectedObjs[0].Bound.Top;
			float bottom = _selectedObjs[0].Bound.Bottom;
			float height = _selectedObjs[0].Bound.Height;
			for (int i = 1; i < count; i++)
			{
				RectangleF rf = _selectedObjs[i].Bound;
				if (top > rf.Top)
					top = rf.Top;
				if (bottom < rf.Bottom)
					bottom = rf.Bottom;
				height += rf.Height;
			}
			float finalHeight = height > (bottom - top) ? height : bottom - top;
			float interval = (finalHeight - height) / (count - 1);
			List<IDrawObj> sorts = new List<IDrawObj>(_selectedObjs);
			SortList(sorts, LayoutType.Top);

			float current = top;
			for (int i = 0; i < count; i++)
			{
				IDrawObj obj = sorts[i];
				RectangleF rf = obj.Rect;
				rf.Y += current - obj.Bound.Y;
				obj.Rect = rf;
				current += obj.Bound.Height + interval;
			}

			_controlPoint.CalculateAndInvalidate();
		}
		public void IncrVSpace(object sender, EventArgs e)
		{
			IncrSpace(LayoutType.Top);
		}
		public void DecrVSpace(object sender, EventArgs e)
		{
			IDrawObj last = _controlPoint.SelectObjs.LastSelectedObj;
			int count = _selectedObjs.Count;
			float standard = last.Bound.Y;

			List<IDrawObj> sorts = new List<IDrawObj>(_selectedObjs);
			SortList(sorts, LayoutType.Top);

			int lastIndex = sorts.IndexOf(last);
			for (int i = 0; i < count; i++)
			{
				IDrawObj obj = sorts[i];
				int index = i - lastIndex;
				RectangleF rf = obj.Rect;
				if (Math.Abs(standard - rf.Y) > Math.Abs(index * MoveSize))
					rf.Y -= index * MoveSize;
				else
					rf.Y = standard;
				obj.Rect = rf;
			}
			_controlPoint.CalculateAndInvalidate();
		}
		#endregion
		#endregion

		#region sequence of object
		public void Top(object sender, EventArgs e)
		{
			IDrawObj obj = _selectedObjs[0];
			Objs.Remove(obj);
			Objs.Add(obj);
			obj.Invalidate();
		}
		public void Last(object sender, EventArgs e)
		{
			IDrawObj obj = _selectedObjs[0];
			Objs.Remove(obj);
			Objs.Insert(0, obj);
			obj.Invalidate();
		}
		public void Front(object sender, EventArgs e)
		{
			IDrawObj obj = _selectedObjs[0];
			int index = Objs.IndexOf(obj);
			if (index != Objs.Count - 1)
			{
				Objs.Remove(obj);
				Objs.Insert(index + 1, obj);
				obj.Invalidate();
			}
		}
		public void Back(object sender, EventArgs e)
		{
			IDrawObj obj = _selectedObjs[0];
			int index = Objs.IndexOf(obj);
			if (index > 0)
			{
				Objs.Remove(obj);
				Objs.Insert(index - 1, obj);
				obj.Invalidate();
			}
		}
		#endregion

		#region flip
		public void FlipX(object sender, EventArgs e)
		{
			_controlPoint.SelectObjs.FlipX();
			RefreshProperty(); 
		}
		public void FlipY(object sender, EventArgs e)
		{
			_controlPoint.SelectObjs.FlipY();
			RefreshProperty(); 
		}
		#endregion

		#region group
		public void Group(object sender, EventArgs e)
		{
			if (_selectedObjs.Count > 1)
			{
				//需要排序，否则绘图的先后顺序会发生改变
				int[] indexA = new int[_selectedObjs.Count];
				for (int i = 0; i < indexA.Length; i++)
					indexA[i] = Objs.IndexOf(_selectedObjs[i]);
				Array.Sort(indexA);
				List<IDrawObj> groupL = indexA.Select(t => Objs[t]).ToList();
				DrawGroup group = new DrawGroup(groupL);
				_nameManager.CreateName(group);
				group.Layer = DefaultLayer;
				group.LoadInitializationEvent();

				foreach (IDrawObj obj in _selectedObjs)
				{
					Objs.Remove(obj);
				}
				Objs.Insert(indexA[0], group);
				_selectedObjs.Clear();
				if (group.CanSelect())
					_selectedObjs.Add(group);

				_controlPoint.ChangeSelectObj(_selectedObjs);
				Container.Invalidate();
			}
		}
		public void UnGroup(object sender, EventArgs e)
		{
			DrawGroup group = null;
			foreach (IDrawObj obj in _selectedObjs)
			{
				if (obj.Type != DrawType.Group)
					continue;

				group = (DrawGroup)obj;
				group.Ungroup();
				int index = Objs.IndexOf(group);
				Objs.Remove(group);
				Objs.InsertRange(index, group.ObjList);
			}

			if (group != null)
			{
				//去除锁定的控件
				int count = group.ObjList.Count;
				for (int i = count - 1; i >= 0; i--)
				{
					IDrawObj obj = group.ObjList[i];
					if (!group.ObjList[i].CanSelect())
						group.ObjList.Remove(obj);
				}

				_selectedObjs.Clear();
				_selectedObjs.AddRange(group.ObjList);

				_controlPoint.ChangeSelectObj(_selectedObjs);
				Container.Invalidate();
			}
		}
		#endregion

		#region state
		public void Ortho(object sender, EventArgs e)
		{
			IsOrtho = !IsOrtho;
			((ToolStripButton)sender).Checked = IsOrtho;
		}
		public void Grid(object sender, EventArgs e)
		{
			IsGrid = !IsGrid;
			((ToolStripButton)sender).Checked = IsGrid;
		}
		public void Scale(object sender, EventArgs e)
		{
			string s = ((ToolStripComboBox) (sender)).Text;
			s = s.Remove(s.Length - 1);
			FormScale = float.Parse(s) / 100f;
		}
		public void Layer(object sender, EventArgs e)
		{
			LayerConfigForm layer = new LayerConfigForm(Container);
			layer.ShowDialog();
			if (layer.DialogResult == DialogResult.OK)
			{
				_controlPoint.ChangeSelectObj(null);
				Container.Invalidate();
			}
		}
		#endregion

	}
}
