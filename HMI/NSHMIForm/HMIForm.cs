#define STUDIO

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using NetSCADA6.Common.NSColorManger;
using NetSCADA6.HMI.NSDrawNodes;
using NetSCADA6.HMI.NSDrawNodes.DrawObj;
using NetSCADA6.HMI.NSDrawVector;
using NetSCADA6.NSInterface.HMI.Form;
using NetSCADA6.NSInterface.HMI.Framework;
using NetSCADA6.NSStudio.NSDockPanel;
using UndoMethods;


namespace NetSCADA6.HMI.NSHMIForm
{
	/// <summary>
    /// HMI窗口
	/// </summary>
#if STUDIO
	public partial class HMIForm : NSDockContent, IHMIForm
#else
    public partial class HMIForm : Form, IHMIForm
#endif
	{
        public HMIForm(IFramework framework)
        {
			InitializeComponent();

			Debug.Assert(framework != null);
			_framework = framework;

            if (_framework.IsStudioMode)
            {
				_studio = new Studio(this);
				_common = _studio;

            	InitStudio();
            	
            }
            else
            {
				_run = new Run(this);
            	_common = _run;

            	InitRun();
            }

        }

        #region property
		/// <summary>
		/// 路径名称
		/// </summary>
		public string FullName
		{
			set
			{
				_common.FullName = value;
				ToolTipText = _common.FullName;
				Text = _common.GetCaption();
			}
			get { return _common.FullName; }
		}
		[Category("外观")]
		[DisplayName("背景填充")]
		public BrushData BackBrushData
		{
			set
			{
				_common.BackBrush.Data = value;

				_common.BackBrush.InitContent(_common.ZeroLocationRect, _common.Path);
				Invalidate();
			}
			get { return _common.BackBrush.Data; }
		}
		/// <summary>
		/// 窗体尺寸
		/// </summary>
		[Category("布局")]
		[DisplayName("窗体位置")]
		[TypeConverter(typeof(RectangleConverter))]
		public Rectangle Rect
		{
			set
			{
				_common.Rect = value;
				Invalidate();
			}
			get { return _common.Rect; }
		}
		/// <summary>
		/// 窗体边框样式
		/// </summary>
		[Category("布局")]
		[DisplayName("窗体边框样式")]
		public FormBorderStyle BorderStyle
		{
			set { _common.BorderStyle = value; }
			get { return _common.BorderStyle; }
		}
		/// <summary>
		/// 窗体边框样式
		/// </summary>
		[Category("布局")]
		[DisplayName("窗体类型")]
		public FormStyle Style
		{
			set { _common.Style = value; }
			get { return _common.Style; }
		}
		private IFramework _framework;
		[Browsable(false)]
		public IFramework Framework
		{
			get { return _framework; }
		}
		private readonly Environment _common;
		/// <summary>
		/// 公共环境
		/// </summary>
		public IEnvironment Common
		{
			get { return _common; }
		}
		private readonly Studio _studio;
		/// <summary>
		/// 编辑环境
		/// </summary>
		public IStudio Studio
		{
			get { return _studio; }
		}
		private readonly Run _run;
		/// <summary>
		/// 运行环境
		/// </summary>
		public IRun Run
		{
			get { return _run; }
		}
		/// <summary>
		/// 标题栏文本
		/// </summary>
		[DisplayName("标题栏文本")]
		public string Caption
		{
			set { _common.Caption = value; }
			get { return _common.Caption; }
		}
		#endregion

		#region field
		#endregion

		#region internal function
		private void InitStudio()
		{
			ContextMenuStrip = MenuStripControl;
			AllowDrop = true;
			AutoScroll = true;

			UndoInit();
			InitToolStrip();
		}
		private void InitRun()
		{
			toolStripLayout.Visible = false;
			statusStripHMI.Visible = false;
			ContextMenuStrip = null;
			AllowDrop = false;
			AutoScroll = false;

			Location = Rect.Location;
			Size = Rect.Size;
			FormBorderStyle = _common.BorderStyle;
		}
		#endregion

        #region serialize
//#if STUDIO
        public override void Save()
        {
        	_common.Save();
			base.Save();
        }
//#endif
        internal void Serialize(BinaryFormatter bf, Stream s)
        {
            const int version = 1;
            
            bf.Serialize(s, version);
			bf.Serialize(s, ControlBox);
			bf.Serialize(s, MaximizeBox);
			bf.Serialize(s, MinimizeBox);
			
			_common.Serialize(bf, s);
        }
        public void Deserialize(BinaryFormatter bf, Stream s)
        {
            int version = (int)bf.Deserialize(s);
			ControlBox = (bool)bf.Deserialize(s);
			MaximizeBox = (bool)bf.Deserialize(s);
			MinimizeBox = (bool)bf.Deserialize(s);

			_common.Deserialize(bf, s);
        }
        #endregion


		#region form event
		#region popup menu
		private void MenuItemCopy_Click(object sender, EventArgs e)
		{
			_studio.Copy();
		}
		private void MenuItemPaste_Click(object sender, EventArgs e)
		{
			_studio.Paste();
		}
		private void MenuItemDelete_Click(object sender, EventArgs e)
		{
			_studio.Delete();
		}
		private void MenuItemGroup_Click(object sender, EventArgs e)
		{
			_studio.Group(null, null);
		}
		private void MenuItemUngroup_Click(object sender, EventArgs e)
		{
			_studio.UnGroup(null, null);
		}
		private void MenuItemCombine_Click(object sender, EventArgs e)
		{
			_studio.Combine();
		}
		#endregion

		#region mouse
		private Point _mouseMovePoint;
		private void HMIForm_MouseMove(object sender, MouseEventArgs e)
		{
			toolStripMouse.Text = string.Format("{0,-4},{1,-4}", e.X, e.Y);

			//鼠标点击和其他一些操作会触发MouseMove事件，在此屏蔽鼠标没有移动的MouseMove事件)
			if (_mouseMovePoint != e.Location)
			{
			    _mouseMovePoint = e.Location;
			    _common.MouseMove(sender, e.Button, e.Location);
			}
		}
		private void HMIForm_MouseDown(object sender, MouseEventArgs e)
		{
			_common.MouseDown(sender, e.Button, e.Location);
		}
		private void HMIForm_MouseUp(object sender, MouseEventArgs e)
		{
			_common.MouseUp(sender, e.Button, e.Location);
		}
		private void HMIForm_DoubleClick(object sender, EventArgs e)
		{
			if (Framework.IsStudioMode)
				((Studio)_common).DoubleClick(sender);
		}
		private void HMIForm_MouseLeave(object sender, EventArgs e)
		{
			_common.MouseLeave();
		}
		#endregion

		#region paint
		private void HMIForm_Paint(object sender, PaintEventArgs e)
		{
			_common.Paint(sender, e);
		}
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			_common.DrawBackground(e.Graphics);
		}
		#endregion

		#region toolbar
		private void InitToolStrip()
		{
			if (toolStripScale.ComboBox != null) 
				toolStripScale.ComboBox.SelectedIndex = 3;

			SetToolStripToolTip();
			BlendToolStrip();
		}
		private void SetToolStripToolTip()
		{
			toolStripTop.ToolTipText = Rs.sTop;
			toolStripLast.ToolTipText = Rs.sLast;
			toolStripFront.ToolTipText = Rs.sFront;
			toolStripBack.ToolTipText = Rs.sBack;

			toolStripAlignLeft.ToolTipText = Rs.sAlignLeft;
			toolStripAlignCenter.ToolTipText = Rs.sAlignCenter;
			toolStripAlignRight.ToolTipText = Rs.sAlignRight;
			toolStripAlignTop.ToolTipText = Rs.sAlignTop;
			toolStripAlignMiddle.ToolTipText = Rs.sAlignMiddle;
			toolStripAlignBottom.ToolTipText = Rs.sAlignBottom;
			toolStripSameWidth.ToolTipText = Rs.sSameWidth;
			toolStripSameHeight.ToolTipText = Rs.sSameHeight;
			toolStripSameSize.ToolTipText = Rs.sSameSize;
			toolStripHSpace.ToolTipText = Rs.sHSpace;
			toolStripIncrHSpace.ToolTipText = Rs.sIncrHSpace;
			toolStripDecrHSpace.ToolTipText = Rs.sDecrHSpace;
			toolStripVSpace.ToolTipText = Rs.sVSpace;
			toolStripIncrVSpace.ToolTipText = Rs.sIncrVSpace;
			toolStripDecrVSpace.ToolTipText = Rs.sDecrVSpace;

			toolStripFlipX.ToolTipText = Rs.sFlipX;
			toolStripFlipY.ToolTipText = Rs.sFlipY;

			toolStripGroup.ToolTipText = Rs.sGroup;
			toolStripUnGroup.ToolTipText = Rs.sUnGroup;

			toolStripOrtho.ToolTipText = Rs.sOrtho;
			toolStripGrid.ToolTipText = Rs.sGrid;
			toolStripScale.ToolTipText = Rs.sScale;
			toolStripLayer.ToolTipText = Rs.sLayer;
		}
		private void BlendToolStrip()
		{
			toolStripTop.Click += _studio.Top;
			toolStripLast.Click += _studio.Last;
			toolStripFront.Click += _studio.Front;
			toolStripBack.Click += _studio.Back;
			
			toolStripAlignLeft.Click += _studio.AlignLeft;
			toolStripAlignCenter.Click += _studio.AlignCenter;
			toolStripAlignRight.Click += _studio.AlignRight;
			toolStripAlignTop.Click += _studio.AlignTop;
			toolStripAlignMiddle.Click += _studio.AlignMiddle;
			toolStripAlignBottom.Click += _studio.AlignBottom;
			toolStripSameWidth.Click += _studio.SameWidth;
			toolStripSameHeight.Click += _studio.SameHeight;
			toolStripSameSize.Click += _studio.SameSize;
			toolStripHSpace.Click += _studio.HSpace;
			toolStripIncrHSpace.Click += _studio.IncrHSpace;
			toolStripDecrHSpace.Click += _studio.DecrHSpace;
			toolStripVSpace.Click += _studio.VSpace;
			toolStripIncrVSpace.Click += _studio.IncrVSpace;
			toolStripDecrVSpace.Click += _studio.DecrVSpace;

			toolStripFlipX.Click += _studio.FlipX;
			toolStripFlipY.Click += _studio.FlipY;

			toolStripGroup.Click += _studio.Group;
			toolStripUnGroup.Click += _studio.UnGroup;

			toolStripOrtho.Click += _studio.Ortho;
			toolStripGrid.Click += _studio.Grid;
			toolStripScale.SelectedIndexChanged += _studio.Scale;
			toolStripLayer.Click += _studio.Layer;
		}
		internal void SetToolStripEnabled()
		{
			SelectObjectManager selectOjbs = _studio.ControlPoint.SelectObjs;
			int count = selectOjbs.List.Count;
			bool isVector = selectOjbs.IsVector;
			bool isSingleSelect = (count == 1);
			bool isMultiSelect = (count > 1);
			bool isMultiSelectVector = isMultiSelect && isVector;
			bool isSelectVector = (count > 0) && isVector;
			bool existGroup = selectOjbs.ExistGroup;

			toolStripTop.Enabled = isSingleSelect;
			toolStripLast.Enabled = isSingleSelect;
			toolStripFront.Enabled = isSingleSelect;
			toolStripBack.Enabled = isSingleSelect;

			toolStripAlignLeft.Enabled = isMultiSelectVector;
			toolStripAlignCenter.Enabled = isMultiSelectVector;
			toolStripAlignRight.Enabled = isMultiSelectVector;
			toolStripAlignTop.Enabled = isMultiSelectVector;
			toolStripAlignMiddle.Enabled = isMultiSelectVector;
			toolStripAlignBottom.Enabled = isMultiSelectVector;
			toolStripSameWidth.Enabled = isMultiSelectVector;
			toolStripSameHeight.Enabled = isMultiSelectVector;
			toolStripSameSize.Enabled = isMultiSelectVector;
			toolStripHSpace.Enabled = isMultiSelectVector;
			toolStripIncrHSpace.Enabled = isMultiSelectVector;
			toolStripDecrHSpace.Enabled = isMultiSelectVector;
			toolStripVSpace.Enabled = isMultiSelectVector;
			toolStripIncrVSpace.Enabled = isMultiSelectVector;
			toolStripDecrVSpace.Enabled = isMultiSelectVector;

			toolStripFlipX.Enabled = isSelectVector;
			toolStripFlipY.Enabled = isSelectVector;

			toolStripGroup.Enabled = isMultiSelect;
			toolStripUnGroup.Enabled = existGroup;
		}
		#endregion

		#region key
		private void HMIForm_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Z:
					if (e.Control)
					{
						if (Undo != null)
						{
						}
					}
					break;
				case Keys.Y:
					if (e.Control)
					{
						if (Undo != null)
						{
						}
					}
					break;
				default:
					break;
			}
		}
		#endregion

		private void HMIForm_Deactivate(object sender, EventArgs e)
		{
			if (Framework.IsStudioMode)
				_studio.Create.End();
		}
		private void HMIForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			//FormClosed先调用，Deactivate后调用
			HMIForm_Deactivate(null, null);
			_common.Dispose();
		}
		private void HMIForm_Activated(object sender, EventArgs e)
		{
			if (Framework.IsStudioMode)
				_studio.ResetSelectObjs();
		}
		private void HMIForm_Scroll(object sender, ScrollEventArgs e)
		{
			if (Framework.IsStudioMode)
			{
				ShowDebugMessage(AutoScrollPosition.ToString());
				Invalidate();
			}
		}
		protected override Point ScrollToControl(Control activeControl)
		{
			return AutoScrollPosition;
		}
		#endregion

		#region print
		private void ToolStripPrint_Click(object sender, EventArgs e)
		{
			_common.PrintDialog();
		}
		private void ToolStripPrintPreview_Click(object sender, EventArgs e)
		{
			_common.PrintPreview();
		}
		#endregion

		#region undo
		public IUndoRedoManager Undo
		{
			get
			{
				return _studio.Undo;
			}
		}
		private void UndoInit()
		{
			Undo.RedoStackStatusChanged += RedoStackStatusChanged;
			Undo.UndoStackStatusChanged += UndoStackStatusChanged;
		}
		private void UndoStackStatusChanged(bool hasItems)
		{
			//toolComboUndo.ComboBox.DataSource = Undo.GetUndoStackInformation();
		}
		void RedoStackStatusChanged(bool hasItems)
		{
			//toolComboRedo.ComboBox.DataSource = Undo.GetRedoStackInformation();
		}
		#endregion

		#region toolbox
		//todo add control
		private readonly static Type[] _vectorTypes = new[]
			{
				typeof (DrawRect),
				typeof (DrawEllipse),
				typeof (DrawText),

				typeof (DrawStraightLine),
				typeof (DrawFoldLine),
				typeof (DrawBezier),
				typeof (DrawPolygon),
				typeof (DrawClosedBezier)
			};
		public static Type[] GetVectorTypes()
		{
			return _vectorTypes;
		}
		#endregion

		#region drag
		private void HMIForm_DragOver(object sender, DragEventArgs e)
		{
			if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
				e.Effect = DragDropEffects.None;
		}
		private void HMIForm_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Effect != DragDropEffects.Copy)
				return;
			
			if (e.Data.GetDataPresent(typeof(ToolboxItem)))
			{
				_studio.Create.End();
				
				ToolboxItem item = (ToolboxItem)e.Data.GetData(typeof(ToolboxItem));
				string name = item.TypeName + "," + item.AssemblyName;
				Type t = Type.GetType(name);
				if (t == null)
					return;
				Point p = PointToClient(new Point(e.X, e.Y));
				_studio.StudioCreateDrawObj(t, new RectangleF(p, new SizeF(100, 100)));
				Framework.Manager.ResetToolboxPointerFunction();
			}
		}
		#endregion


		public void ShowDebugMessage(string message)
		{
			toolStripDebug.Text = message;
		}


	}
}
