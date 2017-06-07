namespace NetSCADA6.HMI.NSHMIForm
{
	partial class HMIForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

        	try
        	{
				base.Dispose(disposing);
        	}
        	catch (System.Exception)
        	{
        	}
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HMIForm));
			this.MenuStripControl = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuItemPaste = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuItemGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuItemUngroup = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuItemCombine = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuItemProperty = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripLayout = new System.Windows.Forms.ToolStrip();
			this.toolStripOrtho = new System.Windows.Forms.ToolStripButton();
			this.toolStripGrid = new System.Windows.Forms.ToolStripButton();
			this.toolStripScale = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripLayer = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripAlignLeft = new System.Windows.Forms.ToolStripButton();
			this.toolStripAlignCenter = new System.Windows.Forms.ToolStripButton();
			this.toolStripAlignRight = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripAlignTop = new System.Windows.Forms.ToolStripButton();
			this.toolStripAlignMiddle = new System.Windows.Forms.ToolStripButton();
			this.toolStripAlignBottom = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSameWidth = new System.Windows.Forms.ToolStripButton();
			this.toolStripSameHeight = new System.Windows.Forms.ToolStripButton();
			this.toolStripSameSize = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripHSpace = new System.Windows.Forms.ToolStripButton();
			this.toolStripIncrHSpace = new System.Windows.Forms.ToolStripButton();
			this.toolStripDecrHSpace = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripVSpace = new System.Windows.Forms.ToolStripButton();
			this.toolStripIncrVSpace = new System.Windows.Forms.ToolStripButton();
			this.toolStripDecrVSpace = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripTop = new System.Windows.Forms.ToolStripButton();
			this.toolStripLast = new System.Windows.Forms.ToolStripButton();
			this.toolStripFront = new System.Windows.Forms.ToolStripButton();
			this.toolStripBack = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripFlipX = new System.Windows.Forms.ToolStripButton();
			this.toolStripFlipY = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripGroup = new System.Windows.Forms.ToolStripButton();
			this.toolStripUnGroup = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			this.statusStripHMI = new System.Windows.Forms.StatusStrip();
			this.toolStripSize = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripSizeImage = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripLocation = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripLocationImage = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripMouse = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripMouseImage = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripDebug = new System.Windows.Forms.ToolStripStatusLabel();
			this.menuStripHMI = new System.Windows.Forms.MenuStrip();
			this.打印ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripPrint = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripPrintPreview = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuStripControl.SuspendLayout();
			this.toolStripLayout.SuspendLayout();
			this.statusStripHMI.SuspendLayout();
			this.menuStripHMI.SuspendLayout();
			this.SuspendLayout();
			// 
			// MenuStripControl
			// 
			this.MenuStripControl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemCopy,
            this.MenuItemPaste,
            this.MenuItemDelete,
            this.toolStripMenuItem2,
            this.MenuItemGroup,
            this.MenuItemUngroup,
            this.MenuItemCombine,
            this.toolStripMenuItem3,
            this.MenuItemProperty});
			this.MenuStripControl.Name = "contextMenuStrip1";
			this.MenuStripControl.Size = new System.Drawing.Size(153, 192);
			// 
			// MenuItemCopy
			// 
			this.MenuItemCopy.Name = "MenuItemCopy";
			this.MenuItemCopy.Size = new System.Drawing.Size(152, 22);
			this.MenuItemCopy.Text = "复制";
			this.MenuItemCopy.Click += new System.EventHandler(this.MenuItemCopy_Click);
			// 
			// MenuItemPaste
			// 
			this.MenuItemPaste.Name = "MenuItemPaste";
			this.MenuItemPaste.Size = new System.Drawing.Size(152, 22);
			this.MenuItemPaste.Text = "粘贴";
			this.MenuItemPaste.Click += new System.EventHandler(this.MenuItemPaste_Click);
			// 
			// MenuItemDelete
			// 
			this.MenuItemDelete.Name = "MenuItemDelete";
			this.MenuItemDelete.Size = new System.Drawing.Size(152, 22);
			this.MenuItemDelete.Text = "删除";
			this.MenuItemDelete.Click += new System.EventHandler(this.MenuItemDelete_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 6);
			// 
			// MenuItemGroup
			// 
			this.MenuItemGroup.Image = ((System.Drawing.Image)(resources.GetObject("MenuItemGroup.Image")));
			this.MenuItemGroup.ImageTransparentColor = System.Drawing.Color.White;
			this.MenuItemGroup.Name = "MenuItemGroup";
			this.MenuItemGroup.Size = new System.Drawing.Size(94, 22);
			this.MenuItemGroup.Text = "成组";
			this.MenuItemGroup.Click += new System.EventHandler(this.MenuItemGroup_Click);
			// 
			// MenuItemUngroup
			// 
			this.MenuItemUngroup.Image = ((System.Drawing.Image)(resources.GetObject("MenuItemUngroup.Image")));
			this.MenuItemUngroup.ImageTransparentColor = System.Drawing.Color.White;
			this.MenuItemUngroup.Name = "MenuItemUngroup";
			this.MenuItemUngroup.Size = new System.Drawing.Size(152, 22);
			this.MenuItemUngroup.Text = "解组";
			this.MenuItemUngroup.Click += new System.EventHandler(this.MenuItemUngroup_Click);
			// 
			// MenuItemCombine
			// 
			this.MenuItemCombine.Name = "MenuItemCombine";
			this.MenuItemCombine.Size = new System.Drawing.Size(152, 22);
			this.MenuItemCombine.Text = "合并";
			this.MenuItemCombine.Click += new System.EventHandler(this.MenuItemCombine_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(149, 6);
			// 
			// MenuItemProperty
			// 
			this.MenuItemProperty.Image = ((System.Drawing.Image)(resources.GetObject("MenuItemProperty.Image")));
			this.MenuItemProperty.ImageTransparentColor = System.Drawing.Color.White;
			this.MenuItemProperty.Name = "MenuItemProperty";
			this.MenuItemProperty.Size = new System.Drawing.Size(152, 22);
			this.MenuItemProperty.Text = "属性";
			// 
			// toolStripLayout
			// 
			this.toolStripLayout.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripOrtho,
            this.toolStripGrid,
            this.toolStripScale,
            this.toolStripLayer,
            this.toolStripSeparator2,
            this.toolStripAlignLeft,
            this.toolStripAlignCenter,
            this.toolStripAlignRight,
            this.toolStripSeparator3,
            this.toolStripAlignTop,
            this.toolStripAlignMiddle,
            this.toolStripAlignBottom,
            this.toolStripSeparator4,
            this.toolStripSameWidth,
            this.toolStripSameHeight,
            this.toolStripSameSize,
            this.toolStripSeparator5,
            this.toolStripHSpace,
            this.toolStripIncrHSpace,
            this.toolStripDecrHSpace,
            this.toolStripSeparator6,
            this.toolStripVSpace,
            this.toolStripIncrVSpace,
            this.toolStripDecrVSpace,
            this.toolStripSeparator7,
            this.toolStripTop,
            this.toolStripLast,
            this.toolStripFront,
            this.toolStripBack,
            this.toolStripSeparator1,
            this.toolStripFlipX,
            this.toolStripFlipY,
            this.toolStripSeparator8,
            this.toolStripGroup,
            this.toolStripUnGroup,
            this.toolStripSeparator9});
			this.toolStripLayout.Location = new System.Drawing.Point(0, 0);
			this.toolStripLayout.Name = "toolStripLayout";
			this.toolStripLayout.Size = new System.Drawing.Size(895, 25);
			this.toolStripLayout.TabIndex = 2;
			// 
			// toolStripOrtho
			// 
			this.toolStripOrtho.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripOrtho.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.Ortho;
			this.toolStripOrtho.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripOrtho.Name = "toolStripOrtho";
			this.toolStripOrtho.Size = new System.Drawing.Size(23, 22);
			// 
			// toolStripGrid
			// 
			this.toolStripGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripGrid.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.Grid;
			this.toolStripGrid.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripGrid.Name = "toolStripGrid";
			this.toolStripGrid.Size = new System.Drawing.Size(23, 22);
			// 
			// toolStripScale
			// 
			this.toolStripScale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.toolStripScale.Items.AddRange(new object[] {
            "400%",
            "200%",
            "150%",
            "100%",
            "75%",
            "50%",
            "25%"});
			this.toolStripScale.Name = "toolStripScale";
			this.toolStripScale.Size = new System.Drawing.Size(75, 25);
			// 
			// toolStripLayer
			// 
			this.toolStripLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripLayer.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.layer;
			this.toolStripLayer.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripLayer.Name = "toolStripLayer";
			this.toolStripLayer.Size = new System.Drawing.Size(23, 22);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripAlignLeft
			// 
			this.toolStripAlignLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripAlignLeft.Enabled = false;
			this.toolStripAlignLeft.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.AlignLeft;
			this.toolStripAlignLeft.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripAlignLeft.Name = "toolStripAlignLeft";
			this.toolStripAlignLeft.Size = new System.Drawing.Size(23, 22);
			this.toolStripAlignLeft.Text = "toolStripButton1";
			// 
			// toolStripAlignCenter
			// 
			this.toolStripAlignCenter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripAlignCenter.Enabled = false;
			this.toolStripAlignCenter.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.AlignCenter;
			this.toolStripAlignCenter.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripAlignCenter.Name = "toolStripAlignCenter";
			this.toolStripAlignCenter.Size = new System.Drawing.Size(23, 22);
			this.toolStripAlignCenter.Text = "toolStripButton2";
			// 
			// toolStripAlignRight
			// 
			this.toolStripAlignRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripAlignRight.Enabled = false;
			this.toolStripAlignRight.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.AlignRight;
			this.toolStripAlignRight.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripAlignRight.Name = "toolStripAlignRight";
			this.toolStripAlignRight.Size = new System.Drawing.Size(23, 22);
			this.toolStripAlignRight.Text = "toolStripButton3";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripAlignTop
			// 
			this.toolStripAlignTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripAlignTop.Enabled = false;
			this.toolStripAlignTop.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.AlignTop;
			this.toolStripAlignTop.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripAlignTop.Name = "toolStripAlignTop";
			this.toolStripAlignTop.Size = new System.Drawing.Size(23, 22);
			this.toolStripAlignTop.Text = "toolStripButton4";
			// 
			// toolStripAlignMiddle
			// 
			this.toolStripAlignMiddle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripAlignMiddle.Enabled = false;
			this.toolStripAlignMiddle.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.AlignMiddle;
			this.toolStripAlignMiddle.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripAlignMiddle.Name = "toolStripAlignMiddle";
			this.toolStripAlignMiddle.Size = new System.Drawing.Size(23, 22);
			this.toolStripAlignMiddle.Text = "toolStripButton5";
			// 
			// toolStripAlignBottom
			// 
			this.toolStripAlignBottom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripAlignBottom.Enabled = false;
			this.toolStripAlignBottom.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.AlignBottom;
			this.toolStripAlignBottom.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripAlignBottom.Name = "toolStripAlignBottom";
			this.toolStripAlignBottom.Size = new System.Drawing.Size(23, 22);
			this.toolStripAlignBottom.Text = "toolStripButton6";
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripSameWidth
			// 
			this.toolStripSameWidth.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripSameWidth.Enabled = false;
			this.toolStripSameWidth.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.SameWidth;
			this.toolStripSameWidth.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripSameWidth.Name = "toolStripSameWidth";
			this.toolStripSameWidth.Size = new System.Drawing.Size(23, 22);
			this.toolStripSameWidth.Text = "toolStripButton7";
			// 
			// toolStripSameHeight
			// 
			this.toolStripSameHeight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripSameHeight.Enabled = false;
			this.toolStripSameHeight.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.SameHeight;
			this.toolStripSameHeight.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripSameHeight.Name = "toolStripSameHeight";
			this.toolStripSameHeight.Size = new System.Drawing.Size(23, 22);
			this.toolStripSameHeight.Text = "toolStripButton8";
			// 
			// toolStripSameSize
			// 
			this.toolStripSameSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripSameSize.Enabled = false;
			this.toolStripSameSize.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.SameSize;
			this.toolStripSameSize.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripSameSize.Name = "toolStripSameSize";
			this.toolStripSameSize.Size = new System.Drawing.Size(23, 22);
			this.toolStripSameSize.Text = "toolStripButton9";
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripHSpace
			// 
			this.toolStripHSpace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripHSpace.Enabled = false;
			this.toolStripHSpace.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.HSpace;
			this.toolStripHSpace.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripHSpace.Name = "toolStripHSpace";
			this.toolStripHSpace.Size = new System.Drawing.Size(23, 22);
			this.toolStripHSpace.Text = "toolStripButton10";
			// 
			// toolStripIncrHSpace
			// 
			this.toolStripIncrHSpace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripIncrHSpace.Enabled = false;
			this.toolStripIncrHSpace.Image = ((System.Drawing.Image)(resources.GetObject("toolStripIncrHSpace.Image")));
			this.toolStripIncrHSpace.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripIncrHSpace.Name = "toolStripIncrHSpace";
			this.toolStripIncrHSpace.Size = new System.Drawing.Size(23, 22);
			this.toolStripIncrHSpace.Text = "toolStripButton11";
			// 
			// toolStripDecrHSpace
			// 
			this.toolStripDecrHSpace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripDecrHSpace.Enabled = false;
			this.toolStripDecrHSpace.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDecrHSpace.Image")));
			this.toolStripDecrHSpace.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripDecrHSpace.Name = "toolStripDecrHSpace";
			this.toolStripDecrHSpace.Size = new System.Drawing.Size(23, 22);
			this.toolStripDecrHSpace.Text = "toolStripButton12";
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripVSpace
			// 
			this.toolStripVSpace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripVSpace.Enabled = false;
			this.toolStripVSpace.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.VSpace;
			this.toolStripVSpace.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripVSpace.Name = "toolStripVSpace";
			this.toolStripVSpace.Size = new System.Drawing.Size(23, 22);
			this.toolStripVSpace.Text = "toolStripButton1";
			// 
			// toolStripIncrVSpace
			// 
			this.toolStripIncrVSpace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripIncrVSpace.Enabled = false;
			this.toolStripIncrVSpace.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.IncrVSpace;
			this.toolStripIncrVSpace.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripIncrVSpace.Name = "toolStripIncrVSpace";
			this.toolStripIncrVSpace.Size = new System.Drawing.Size(23, 22);
			this.toolStripIncrVSpace.Text = "toolStripButton2";
			// 
			// toolStripDecrVSpace
			// 
			this.toolStripDecrVSpace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripDecrVSpace.Enabled = false;
			this.toolStripDecrVSpace.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.DecrVSpace;
			this.toolStripDecrVSpace.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripDecrVSpace.Name = "toolStripDecrVSpace";
			this.toolStripDecrVSpace.Size = new System.Drawing.Size(23, 22);
			this.toolStripDecrVSpace.Text = "toolStripButton3";
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripTop
			// 
			this.toolStripTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripTop.Enabled = false;
			this.toolStripTop.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.top;
			this.toolStripTop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripTop.Name = "toolStripTop";
			this.toolStripTop.Size = new System.Drawing.Size(23, 22);
			// 
			// toolStripLast
			// 
			this.toolStripLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripLast.Enabled = false;
			this.toolStripLast.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.last;
			this.toolStripLast.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripLast.Name = "toolStripLast";
			this.toolStripLast.Size = new System.Drawing.Size(23, 22);
			this.toolStripLast.Text = "toolStripButton2";
			// 
			// toolStripFront
			// 
			this.toolStripFront.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripFront.Enabled = false;
			this.toolStripFront.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.front;
			this.toolStripFront.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripFront.Name = "toolStripFront";
			this.toolStripFront.Size = new System.Drawing.Size(23, 22);
			this.toolStripFront.Text = "toolStripButton3";
			// 
			// toolStripBack
			// 
			this.toolStripBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripBack.Enabled = false;
			this.toolStripBack.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.back;
			this.toolStripBack.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripBack.Name = "toolStripBack";
			this.toolStripBack.Size = new System.Drawing.Size(23, 22);
			this.toolStripBack.Text = "toolStripButton4";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripFlipX
			// 
			this.toolStripFlipX.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripFlipX.Enabled = false;
			this.toolStripFlipX.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.FlipX;
			this.toolStripFlipX.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripFlipX.Name = "toolStripFlipX";
			this.toolStripFlipX.Size = new System.Drawing.Size(23, 22);
			// 
			// toolStripFlipY
			// 
			this.toolStripFlipY.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripFlipY.Enabled = false;
			this.toolStripFlipY.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.FlipY;
			this.toolStripFlipY.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripFlipY.Name = "toolStripFlipY";
			this.toolStripFlipY.Size = new System.Drawing.Size(23, 22);
			// 
			// toolStripSeparator8
			// 
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripGroup
			// 
			this.toolStripGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripGroup.Enabled = false;
			this.toolStripGroup.Image = ((System.Drawing.Image)(resources.GetObject("toolStripGroup.Image")));
			this.toolStripGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripGroup.Name = "toolStripGroup";
			this.toolStripGroup.Size = new System.Drawing.Size(23, 22);
			// 
			// toolStripUnGroup
			// 
			this.toolStripUnGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripUnGroup.Enabled = false;
			this.toolStripUnGroup.Image = ((System.Drawing.Image)(resources.GetObject("toolStripUnGroup.Image")));
			this.toolStripUnGroup.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripUnGroup.Name = "toolStripUnGroup";
			this.toolStripUnGroup.Size = new System.Drawing.Size(23, 22);
			// 
			// toolStripSeparator9
			// 
			this.toolStripSeparator9.Name = "toolStripSeparator9";
			this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
			// 
			// statusStripHMI
			// 
			this.statusStripHMI.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.statusStripHMI.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSize,
            this.toolStripSizeImage,
            this.toolStripLocation,
            this.toolStripLocationImage,
            this.toolStripMouse,
            this.toolStripMouseImage,
            this.toolStripDebug});
			this.statusStripHMI.Location = new System.Drawing.Point(0, 457);
			this.statusStripHMI.Name = "statusStripHMI";
			this.statusStripHMI.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.statusStripHMI.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.statusStripHMI.Size = new System.Drawing.Size(895, 22);
			this.statusStripHMI.TabIndex = 4;
			// 
			// toolStripSize
			// 
			this.toolStripSize.AutoSize = false;
			this.toolStripSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripSize.Name = "toolStripSize";
			this.toolStripSize.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.toolStripSize.Size = new System.Drawing.Size(80, 17);
			this.toolStripSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripSizeImage
			// 
			this.toolStripSizeImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripSizeImage.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.size;
			this.toolStripSizeImage.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripSizeImage.Name = "toolStripSizeImage";
			this.toolStripSizeImage.Size = new System.Drawing.Size(16, 17);
			this.toolStripSizeImage.Text = "toolStripStatusLabel1";
			// 
			// toolStripLocation
			// 
			this.toolStripLocation.AutoSize = false;
			this.toolStripLocation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripLocation.Name = "toolStripLocation";
			this.toolStripLocation.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.toolStripLocation.Size = new System.Drawing.Size(80, 17);
			this.toolStripLocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripLocationImage
			// 
			this.toolStripLocationImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripLocationImage.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.location1;
			this.toolStripLocationImage.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripLocationImage.Name = "toolStripLocationImage";
			this.toolStripLocationImage.Size = new System.Drawing.Size(16, 17);
			this.toolStripLocationImage.Text = "toolStripStatusLabel1";
			// 
			// toolStripMouse
			// 
			this.toolStripMouse.AutoSize = false;
			this.toolStripMouse.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolStripMouse.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolStripMouse.Name = "toolStripMouse";
			this.toolStripMouse.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.toolStripMouse.Size = new System.Drawing.Size(100, 17);
			this.toolStripMouse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripMouseImage
			// 
			this.toolStripMouseImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripMouseImage.Image = global::NetSCADA6.HMI.NSHMIForm.Properties.Resources.mouse;
			this.toolStripMouseImage.ImageTransparentColor = System.Drawing.Color.White;
			this.toolStripMouseImage.Name = "toolStripMouseImage";
			this.toolStripMouseImage.Size = new System.Drawing.Size(16, 17);
			this.toolStripMouseImage.Text = "toolStripStatusLabel1";
			// 
			// toolStripDebug
			// 
			this.toolStripDebug.Name = "toolStripDebug";
			this.toolStripDebug.Size = new System.Drawing.Size(0, 17);
			this.toolStripDebug.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// menuStripHMI
			// 
			this.menuStripHMI.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打印ToolStripMenuItem});
			this.menuStripHMI.Location = new System.Drawing.Point(0, 0);
			this.menuStripHMI.Name = "menuStripHMI";
			this.menuStripHMI.Size = new System.Drawing.Size(895, 24);
			this.menuStripHMI.TabIndex = 6;
			this.menuStripHMI.Text = "menuStrip1";
			this.menuStripHMI.Visible = false;
			// 
			// 打印ToolStripMenuItem
			// 
			this.打印ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripPrint,
            this.ToolStripPrintPreview});
			this.打印ToolStripMenuItem.Name = "打印ToolStripMenuItem";
			this.打印ToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
			this.打印ToolStripMenuItem.Text = "打印";
			// 
			// ToolStripPrint
			// 
			this.ToolStripPrint.Name = "ToolStripPrint";
			this.ToolStripPrint.Size = new System.Drawing.Size(118, 22);
			this.ToolStripPrint.Text = "打印";
			this.ToolStripPrint.Click += new System.EventHandler(this.ToolStripPrint_Click);
			// 
			// ToolStripPrintPreview
			// 
			this.ToolStripPrintPreview.Name = "ToolStripPrintPreview";
			this.ToolStripPrintPreview.Size = new System.Drawing.Size(118, 22);
			this.ToolStripPrintPreview.Text = "打印预览";
			this.ToolStripPrintPreview.Click += new System.EventHandler(this.ToolStripPrintPreview_Click);
			// 
			// HMIForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(895, 479);
			this.Controls.Add(this.statusStripHMI);
			this.Controls.Add(this.toolStripLayout);
			this.Controls.Add(this.menuStripHMI);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "HMIForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "HMIForm";
			this.ToolTipText = "";
			this.Activated += new System.EventHandler(this.HMIForm_Activated);
			this.Deactivate += new System.EventHandler(this.HMIForm_Deactivate);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HMIForm_FormClosed);
			this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HMIForm_Scroll);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.HMIForm_DragDrop);
			this.DragOver += new System.Windows.Forms.DragEventHandler(this.HMIForm_DragOver);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.HMIForm_Paint);
			this.DoubleClick += new System.EventHandler(this.HMIForm_DoubleClick);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HMIForm_KeyDown);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HMIForm_MouseDown);
			this.MouseLeave += new System.EventHandler(this.HMIForm_MouseLeave);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HMIForm_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HMIForm_MouseUp);
			this.MenuStripControl.ResumeLayout(false);
			this.toolStripLayout.ResumeLayout(false);
			this.toolStripLayout.PerformLayout();
			this.statusStripHMI.ResumeLayout(false);
			this.statusStripHMI.PerformLayout();
			this.menuStripHMI.ResumeLayout(false);
			this.menuStripHMI.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip MenuStripControl;
        private System.Windows.Forms.ToolStripMenuItem MenuItemCopy;
		private System.Windows.Forms.ToolStripMenuItem MenuItemPaste;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDelete;
		private System.Windows.Forms.ToolStrip toolStripLayout;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem MenuItemGroup;
        private System.Windows.Forms.ToolStripMenuItem MenuItemUngroup;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.StatusStrip statusStripHMI;
		private System.Windows.Forms.MenuStrip menuStripHMI;
		private System.Windows.Forms.ToolStripMenuItem 打印ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ToolStripPrint;
		private System.Windows.Forms.ToolStripMenuItem ToolStripPrintPreview;
		private System.Windows.Forms.ToolStripMenuItem MenuItemCombine;
		private System.Windows.Forms.ToolStripStatusLabel toolStripLocationImage;
		internal System.Windows.Forms.ToolStripStatusLabel toolStripSize;
		private System.Windows.Forms.ToolStripStatusLabel toolStripMouseImage;
		private System.Windows.Forms.ToolStripStatusLabel toolStripMouse;
		internal System.Windows.Forms.ToolStripStatusLabel toolStripLocation;
		private System.Windows.Forms.ToolStripStatusLabel toolStripSizeImage;
		private System.Windows.Forms.ToolStripButton toolStripTop;
		private System.Windows.Forms.ToolStripButton toolStripLast;
		private System.Windows.Forms.ToolStripButton toolStripFront;
		private System.Windows.Forms.ToolStripButton toolStripBack;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton toolStripAlignLeft;
		private System.Windows.Forms.ToolStripButton toolStripAlignCenter;
		private System.Windows.Forms.ToolStripButton toolStripAlignRight;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton toolStripAlignTop;
		private System.Windows.Forms.ToolStripButton toolStripAlignMiddle;
		private System.Windows.Forms.ToolStripButton toolStripAlignBottom;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripButton toolStripSameWidth;
		private System.Windows.Forms.ToolStripButton toolStripSameHeight;
		private System.Windows.Forms.ToolStripButton toolStripSameSize;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripButton toolStripHSpace;
		private System.Windows.Forms.ToolStripButton toolStripIncrHSpace;
		private System.Windows.Forms.ToolStripButton toolStripDecrHSpace;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripButton toolStripVSpace;
		private System.Windows.Forms.ToolStripButton toolStripIncrVSpace;
		private System.Windows.Forms.ToolStripButton toolStripDecrVSpace;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolStripFlipX;
		private System.Windows.Forms.ToolStripButton toolStripFlipY;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
		private System.Windows.Forms.ToolStripMenuItem MenuItemProperty;
		private System.Windows.Forms.ToolStripButton toolStripGroup;
		private System.Windows.Forms.ToolStripButton toolStripUnGroup;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
		private System.Windows.Forms.ToolStripStatusLabel toolStripDebug;
		private System.Windows.Forms.ToolStripButton toolStripOrtho;
		private System.Windows.Forms.ToolStripButton toolStripGrid;
		private System.Windows.Forms.ToolStripComboBox toolStripScale;
		private System.Windows.Forms.ToolStripButton toolStripLayer;
    }
}