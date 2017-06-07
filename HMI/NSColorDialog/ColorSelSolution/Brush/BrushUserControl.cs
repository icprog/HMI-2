using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; 
using System.Diagnostics;
using NetSCADA6.Common.NSColorManger.Properties;

namespace NetSCADA6.Common.NSColorManger
{
    public partial class BrushUserControl : UserControl
    {
        #region 初始化
        public BrushUserControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// true:允许 false不允许 画刷变化通知开关
        /// </summary>
        bool _EanbleNotify = false;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitExamplesBrushes();
            InitEvent();
            LoadHatchBrushes();
            UpdateData(false);
            _EanbleNotify = true;
            UpdateFlowLayoutPanel();
            Notify();
        }
        #region 示例画刷
        /// <summary>
        /// 纯色
        /// </summary>
        private SolidBrush _solidBrushExample;
        /// <summary>
        /// 渐变色
        /// </summary>
        private LinearGradientBrush _linearGradientBrushExample;
        /// <summary>
        /// 底纹
        /// </summary>
        HatchBrush _hatchBrushExample;
        /// <summary>
        /// 图片
        /// </summary> 
        TextureBrush _textureBrushExample;
        /// <summary>
        /// 路径渐变 
        /// </summary>
        private PathGradientBrush _pathGradientBrushExample;
        /// <summary>
        /// 路径渐变得坐标集合
        /// </summary>
        private Point[] _pointsPathBrushExample = new Point[4];
        #endregion
        /// <summary>
        /// 初始化示例画刷
        /// </summary>
        private void InitExamplesBrushes()
        {
            _solidBrushExample = new SolidBrush(Color.Black);
            _linearGradientBrushExample = new LinearGradientBrush(new Point(0, 0), new Point(100, 100), Color.Black, Color.White);
            _hatchBrushExample = new HatchBrush(HatchStyle.BackwardDiagonal, Color.Black, Color.White);
            _textureBrushExample = new TextureBrush(Resources._1);
            _pointsPathBrushExample[0] = new Point(0, 0);//CellLength
            _pointsPathBrushExample[1] = new Point(0, 50);
            _pointsPathBrushExample[2] = new Point(46, 50);
            _pointsPathBrushExample[3] = new Point(46, 0);
            _pathGradientBrushExample = new PathGradientBrush(_pointsPathBrushExample);
        }
        private void InitEvent()
        {
            /// controls changed
            solidUserControl1.ColorChanged += Event_SolidBrushControl;
            linearGradientUserControl1.ValueChanged += Event_LinearBrushControl;
            linearGradientUserControl2.ValueChanged += Event_PathBrushControl;
            HScrollBarUserControl_ForeColor.valueChange += Event_HatchBrushControl;
            hScrollBarUserControl_BackColor.valueChange += Event_HatchBrushControl;

            //flow
            flowLayoutPanelEx_solid.EventSelectedChange += Event_FlowControl;
            flowLayoutPanelEx_linear.EventSelectedChange += Event_FlowControl;
            flowLayoutPanelEx_hatch.EventSelectedChange += Event_FlowControl;
            flowLayoutPanelEx_radiate.EventSelectedChange += Event_FlowControl;

            //flow button 
            this.button_solid_save.Click += button_flow_save_Click;
            this.button_linear_save.Click += button_flow_save_Click;
            this.button_radiate_save.Click += this.button_flow_save_Click;
            this.button_solid_load.Click += button_flow_load_Click;
            this.button_linear_load.Click += button_flow_load_Click;
            this.button_radiate_load.Click += button_flow_load_Click;

            //radio 
            this.radio_pos_tile.CheckedChanged += new System.EventHandler(this.radio_pos_tile_CheckedChanged);
            this.radio_pos_center.CheckedChanged += new System.EventHandler(this.radio_pos_tile_CheckedChanged);
            this.radio_pos_stretch.CheckedChanged += new System.EventHandler(this.radio_pos_tile_CheckedChanged);
            this.radio_title_none.CheckedChanged += new System.EventHandler(this.radio_pos_tile_CheckedChanged);
            this.radio_title_horizontal.CheckedChanged += new System.EventHandler(this.radio_pos_tile_CheckedChanged);
            this.radio_title_vertical.CheckedChanged += new System.EventHandler(this.radio_pos_tile_CheckedChanged);
            this.radio_title_hv.CheckedChanged += new System.EventHandler(this.radio_pos_tile_CheckedChanged);

            this.btn1.Click += new System.EventHandler(this.btn_image_1_12_Click);
            this.btn2.Click += new System.EventHandler(this.btn_image_1_12_Click);
            this.btn3.Click += new System.EventHandler(this.btn_image_1_12_Click);
            this.btn4.Click += new System.EventHandler(this.btn_image_1_12_Click);
            this.btn5.Click += new System.EventHandler(this.btn_image_1_12_Click);
            this.btn6.Click += new System.EventHandler(this.btn_image_1_12_Click);
            this.btn7.Click += new System.EventHandler(this.btn_image_1_12_Click);
            this.btn8.Click += new System.EventHandler(this.btn_image_1_12_Click);
            this.btn9.Click += new System.EventHandler(this.btn_image_1_12_Click);
            this.btn10.Click += new System.EventHandler(this.btn_image_1_12_Click);
            this.btn11.Click += new System.EventHandler(this.btn_image_1_12_Click);
            this.btn12.Click += new System.EventHandler(this.btn_image_1_12_Click);
        }
        /// <summary>
        /// 初始化 加载遍历底纹图
        /// </summary>
        private void LoadHatchBrushes()
        {
            foreach (HatchStyle style in Enum.GetValues(typeof(HatchStyle)))
            {
                HatchBrush brush = new HatchBrush(style, Color.Black, Color.White);
                FlowCell cell = new FlowCell(brush);
                flowLayoutPanelEx_hatch.Add(cell);
            }
        }

        #endregion

        #region 控件对应数据 以及刷新
        public BrushData brushData;
        NSHatchBrushInfo hatchBrushInfo = new NSHatchBrushInfo();
        NSTextrueBrushInfo TextrueBrushInfo = new NSTextrueBrushInfo();
        NSPathGradientBrushInfo PathGradientBrushInfo = new NSPathGradientBrushInfo();

        public void UpdateData(bool bSave)
        {
            if (brushData == null) return;
            if (bSave)
            {
                brushData.BrushType = (NSBrushType)tabControl1.SelectedIndex;
                //solid
                brushData.SolidBrushInfo.Color = solidUserControl1.color;
                brushData.LinearGradientBrushInfo = linearGradientUserControl1.LinearGradientBrushInfo;
                brushData.HatchBrushInfo = hatchBrushInfo;

                TextrueBrushInfo.ImageDrawMode = GetImageDrawMode();
                TextrueBrushInfo.WrapMode = GetWrapMode();
                brushData.TextrueBrushInfo = TextrueBrushInfo;

                PathGradientBrushInfo.LinearGradient = linearGradientUserControl2.LinearGradientBrushInfo;
                brushData.PathGradientBrushInfo = PathGradientBrushInfo;
            }
            else//映射到窗口
            {
                tabControl1.SelectedIndex = (int)brushData.BrushType;
                //solid
                solidUserControl1.color = brushData.SolidBrushInfo.Color;
                Event_SolidBrushControl(solidUserControl1.color);


                //linear
                linearGradientUserControl1.LinearGradientBrushInfo = brushData.LinearGradientBrushInfo;

                //path
                linearGradientUserControl2.LinearGradientBrushInfo = brushData.PathGradientBrushInfo.LinearGradient;

                //hatch
                hatchBrushInfo = brushData.HatchBrushInfo;
                buttonEx_ForeColor.Color = brushData.HatchBrushInfo.ForeColor;
                buttonEx_BackColor.Color = brushData.HatchBrushInfo.BackColor;
                HScrollBarUserControl_ForeColor.Value = brushData.HatchBrushInfo.ForeAValue;
                hScrollBarUserControl_BackColor.Value = brushData.HatchBrushInfo.BackAValue;
                if (_hatchBrushExample != null)
                    _hatchBrushExample.Dispose();
                _hatchBrushExample = new HatchBrush(hatchBrushInfo.Style, hatchBrushInfo.ForeColor, hatchBrushInfo.BackColor);

                //image 
                TextrueBrushInfo = brushData.TextrueBrushInfo;
                textBox_image.Text = TextrueBrushInfo.FileName;

                RadioButtonEanble();
                SetWrapMode(TextrueBrushInfo.WrapMode);
                SetImageDrawMode(TextrueBrushInfo.ImageDrawMode);
                if (brushData.BrushType == NSBrushType.Textrue)
                {
                    if (TextrueBrushInfo.IsResource)
                    {
                        if (TextrueBrushInfo.ResourceImage != "")
                        {
                            Object res = Resources.ResourceManager.GetObject(TextrueBrushInfo.ResourceImage);
                            Image im = res as Image;
                            _textureBrushExample = new TextureBrush(im, TextrueBrushInfo.WrapMode);
                        }
                    }
                    else
                    {
                        if (TextrueBrushInfo.FileName != "")
                        {
                            Bitmap _ImageBmp = new Bitmap(TextrueBrushInfo.FileName);
                            _textureBrushExample = new TextureBrush(_ImageBmp, TextrueBrushInfo.WrapMode);
                        }
                    }
                }
            }
        }
        #endregion

        #region 控件本身发生变化通知
        /// <summary>
        /// 纯色控件的颜色发生变化通知 [一般作用于示例图使用]
        /// </summary>
        /// <param name="color">变化后的颜色</param>
        private void Event_SolidBrushControl(Color color)  //_solidBrush
        {
            if (_solidBrushExample.Color != solidUserControl1.color)
            {
                _solidBrushExample.Color = solidUserControl1.color;
                Notify();
            }
        }

        /// <summary>
        /// 渐变色控件的颜色发生变化通知 [一般作用于示例图使用]
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="angle"></param>
        private void Event_LinearBrushControl(ColorBlend cb, float angle)
        {
            if (_linearGradientBrushExample != null)
            {
                _linearGradientBrushExample.Dispose();
                _linearGradientBrushExample = null;
            }
            _linearGradientBrushExample = new LinearGradientBrush(ClientRectangle, Color.Red, Color.FromArgb(255, 0, 255, 0), angle);
            _linearGradientBrushExample.InterpolationColors = linearGradientUserControl1.LinearGradientBrushInfo.ColorBlend;
            Notify();
            Invalidate();
        }

        /// <summary>
        /// 路径控件的颜色发生变化通知 [一般作用于示例图使用]
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="angle"></param>
        private void Event_PathBrushControl(ColorBlend cb, float angle)
        {
            _pathGradientBrushExample.InterpolationColors = linearGradientUserControl2.LinearGradientBrushInfo.ColorBlend;
            _pathGradientBrushExample.ResetTransform();
            Point centpt = new Point(Location.X - 1 + Width / 2, Location.Y + Height / 2);
            Notify();
            //             Matrix matrix = new Matrix();
            //             matrix.Reset();
            //             matrix.RotateAt(0, centpt);
            //          _pathGradientBrushExample.Transform = matrix;
            Invalidate();
        }

        /// <summary> 
        /// 图片发生改变 发生变化通知 【前景、背景、透明度】
        /// </summary>
        private void Event_TextrueBrushControl(bool IsResource, string strFileName, WrapMode WrapMode)
        {
            if (_textureBrushExample != null)
                _textureBrushExample.Dispose();
            if (IsResource)
            {
                Object res = Resources.ResourceManager.GetObject(strFileName);
                Image im = res as Image;
                _textureBrushExample = new TextureBrush(im, WrapMode);
            }
            else
            {
                TextrueBrushInfo.ResourceImage = "";

                Bitmap _ImageBmp = new Bitmap(strFileName);
                _textureBrushExample = new TextureBrush(_ImageBmp, WrapMode);

            }
            Notify();
        }
        #endregion

        #region 其他控件发生变化通知
        /// <summary> 
        ///  flowLayoutPanelEx 选择发生变化通知 【示例】
        /// </summary>
        void Event_FlowControl(FlowCellUserControl uc)
        {
            if (uc.CellPreset.Brush is SolidBrush)
            {
                solidUserControl1.color = ((SolidBrush)uc.CellPreset.Brush).Color;
            }
            else if (uc.CellPreset.Brush is LinearGradientBrush)
            {
                // _EanbleNotify = false;
                linearGradientUserControl1.LinearGradientBrushInfo = new NSLinearGradientBrushInfo(((LinearGradientBrush)uc.CellPreset.Brush).InterpolationColors, uc.CellPreset.Angle);
                Event_LinearBrushControl(((LinearGradientBrush)uc.CellPreset.Brush).InterpolationColors, uc.CellPreset.Angle);
                _EanbleNotify = true;
            }
            else if (uc.CellPreset.Brush is HatchBrush)
            {
                HatchBrush hb = ((HatchBrush)uc.CellPreset.Brush);
                if (_hatchBrushExample != null)
                    _hatchBrushExample.Dispose();
                hatchBrushInfo.Style = hb.HatchStyle;
                _hatchBrushExample = new HatchBrush(hatchBrushInfo.Style, hatchBrushInfo.ForeColor, hatchBrushInfo.BackColor);
                Notify();
            }
            else if (uc.CellPreset.Brush is PathGradientBrush)
            {
                //   _EanbleNotify = false;
                linearGradientUserControl2.LinearGradientBrushInfo = new NSLinearGradientBrushInfo(((PathGradientBrush)uc.CellPreset.Brush).InterpolationColors, 0);
                Event_PathBrushControl(((PathGradientBrush)uc.CellPreset.Brush).InterpolationColors, 0);
                //   _EanbleNotify = true;
            }
        }

        /// <summary>
        /// 图片画刷 单选按钮选 发生变化通知
        /// </summary> 
        private void radio_pos_tile_CheckedChanged(object sender, EventArgs e)
        {
            if (_bSetMode == false)//没有设置
            {
                TextrueBrushInfo.ImageDrawMode = GetImageDrawMode();
                TextrueBrushInfo.WrapMode = GetWrapMode();
            }
            if (_textureBrushExample != null)
            {
                _textureBrushExample.WrapMode = TextrueBrushInfo.WrapMode;
            }
            _bSetMode = false;
            Notify();
            Invalidate();
            RadioButtonEanble();
        }

        /// <summary>
        /// 选择现有图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_image_1_12_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            TextrueBrushInfo.ResourceImage = btn.Tag.ToString();
            Event_TextrueBrushControl(true, TextrueBrushInfo.ResourceImage, TextrueBrushInfo.WrapMode);
        }

        /// <summary>
        /// 选择其他文件图片
        /// </summary> 
        private void button_imageSel_Click(object sender, EventArgs e)
        {
            string openFilePath = Tool.FileTool.OpenFileDialog(imageExt, false);
            if (string.IsNullOrEmpty(openFilePath))
                return;
            TextrueBrushInfo.FileName = openFilePath;
            textBox_image.Text = openFilePath;
            Event_TextrueBrushControl(false, openFilePath, GetWrapMode());
            Invalidate();
        }

        /// <summary>
        /// Tab页切 刷新flow 发生变化通知 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFlowLayoutPanel();

            Notify();
        }
        #endregion

        #region 刷新Flow
        /// <summary>
        /// 刷新默认图例Flow
        /// </summary>
        void UpdateFlowLayoutPanel()
        {
            if (tabControl1.SelectedIndex == (int)NSBrushType.Solid)
            {
                if (flowLayoutPanelEx_solid.Controls.Count == 0)
                    button_solid_flowdefault_Click(null, null);
                //   Notify();
            }
            else if (tabControl1.SelectedIndex == (int)NSBrushType.LinearGradient)
            {
                if (flowLayoutPanelEx_linear.Controls.Count == 0)
                    button_linear_flowdefalut_Click(null, null);
                //   Notify();
            }
            else if (tabControl1.SelectedIndex == (int)NSBrushType.PathGradient)
            {
                if (flowLayoutPanelEx_radiate.Controls.Count == 0)
                    button_radiate_flowdefault_Click(null, null);
                //   Notify();
            }
            else if (tabControl1.SelectedIndex == (int)NSBrushType.Null)
            {
                //      Notify();
            }
        }
        /// <summary>
        /// ToFlowCell("#000000")
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        FlowCell ToFlowCell(string text)
        {
            ColorConverter wcc = new ColorConverter();
            Color clr = (Color)wcc.ConvertFromString(text);
            FlowCell cell = new FlowCell(new SolidBrush(clr));
            cell.Text = text;
            return cell;
        }
        /// <summary>
        /// 转换到 FlowCell  clrs和float ColorBlend
        /// </summary>
        /// <param name="IsLinear"> 真：渐变画刷，假：路径画刷</param>
        /// <param name="clrs"></param>
        /// <param name="floats"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        FlowCell ToFlowCell(bool IsLinear, Color[] clrs, float[] floats, float angle = 0)
        {
            ColorBlend cb = new ColorBlend(clrs.Length);
            cb.Colors = clrs; cb.Positions = floats;

            FlowCell cell;
            if (IsLinear)
            {
                LinearGradientBrush br = new LinearGradientBrush(ClientRectangle, Color.Red, Color.FromArgb(255, 0, 255, 0), LinearGradientMode.Horizontal);
                br.InterpolationColors = cb;
                cell = new FlowCell(br, angle);
            }
            else
            {
                PathGradientBrush br = new PathGradientBrush(_pointsPathBrushExample);
                br.InterpolationColors = cb;
                cell = new FlowCell(br);
            }
            return cell;
        }
        private void button_solid_flowdefault_Click(object sender, EventArgs e)
        {
            flowLayoutPanelEx_solid.ClearAllCell();
            flowLayoutPanelEx_solid.Add(ToFlowCell("#000000"));
            flowLayoutPanelEx_solid.Add(ToFlowCell("#FF0000"));
            flowLayoutPanelEx_solid.Add(ToFlowCell("#FFFF00"));
            flowLayoutPanelEx_solid.Add(ToFlowCell("#00FF00"));
            flowLayoutPanelEx_solid.Add(ToFlowCell("#80FF00"));
            flowLayoutPanelEx_solid.Add(ToFlowCell("#00FFFF"));
            flowLayoutPanelEx_solid.Add(ToFlowCell("#0000FF"));
            flowLayoutPanelEx_solid.Add(ToFlowCell("#FF00FF"));
            flowLayoutPanelEx_solid.Add(ToFlowCell("#FF007F"));
        }
        private void button_linear_flowdefalut_Click(object sender, EventArgs e)
        {
            flowLayoutPanelEx_linear.ClearAllCell();
            ColorBlend cb = new ColorBlend(2);
            Color[] clrs = new Color[2];
            clrs[0] = Color.Black;
            clrs[1] = Color.White;
            float[] floats = new float[2];
            floats[0] = 0;
            floats[1] = 1;
            flowLayoutPanelEx_linear.Add(ToFlowCell(true, clrs, floats, 45));
            flowLayoutPanelEx_linear.Add(ToFlowCell(true, clrs, floats, 135));
            flowLayoutPanelEx_linear.Add(ToFlowCell(true, clrs, floats, 0));
            flowLayoutPanelEx_linear.Add(ToFlowCell(true, clrs, floats, 315));
            flowLayoutPanelEx_linear.Add(ToFlowCell(true, clrs, floats, 225));
            flowLayoutPanelEx_linear.Add(ToFlowCell(true, clrs, floats, 90));

            cb = new ColorBlend(3);
            clrs = new Color[3];
            clrs[0] = Color.Black;
            clrs[1] = Color.White;
            clrs[2] = Color.Black;
            floats = new float[3];
            floats[0] = 0;
            floats[1] = 0.5f;
            floats[2] = 1;
            flowLayoutPanelEx_linear.Add(ToFlowCell(true, clrs, floats, 0));
            flowLayoutPanelEx_linear.Add(ToFlowCell(true, clrs, floats, 90));
        }
        private void button_radiate_flowdefault_Click(object sender, EventArgs e)
        {
            flowLayoutPanelEx_radiate.ClearAllCell();
            ColorBlend cb = new ColorBlend(3);
            Color[] clrs = new Color[3];
            clrs[0] = Color.Black;
            clrs[1] = Color.White;
            clrs[2] = Color.Black;
            float[] floats = new float[3];
            floats[0] = 0;
            floats[1] = 0.5f;
            floats[2] = 1;
            cb.Colors = clrs; cb.Positions = floats;

            PathGradientBrush br = new PathGradientBrush(_pointsPathBrushExample);
            br.InterpolationColors = cb;
            FlowCell cell1 = new FlowCell(br);
            flowLayoutPanelEx_radiate.Add(ToFlowCell(false, clrs, floats));
            clrs[0] = Color.Red;
            clrs[1] = Color.Yellow;
            clrs[2] = Color.Black;
            floats[0] = 0;
            floats[1] = 0.5f;
            floats[2] = 1;
            cb.Colors = clrs; cb.Positions = floats;
            flowLayoutPanelEx_radiate.Add(ToFlowCell(false, clrs, floats));
            clrs[0] = Color.Red;
            clrs[1] = Color.White;
            clrs[2] = Color.Red;
            floats[0] = 0;
            floats[1] = 0.5f;
            floats[2] = 1;
            cb.Colors = clrs; cb.Positions = floats;
            flowLayoutPanelEx_radiate.Add(ToFlowCell(false, clrs, floats));
            clrs[0] = Color.Blue;
            clrs[1] = Color.Yellow;
            clrs[2] = Color.Blue;
            floats[0] = 0;
            floats[1] = 0.5f;
            floats[2] = 1;
            cb.Colors = clrs; cb.Positions = floats;
            flowLayoutPanelEx_radiate.Add(ToFlowCell(false, clrs, floats));
            clrs[0] = Color.YellowGreen;
            clrs[1] = Color.GreenYellow;
            clrs[2] = Color.Green;
            floats[0] = 0;
            floats[1] = 0.5f;
            floats[2] = 1;
            cb.Colors = clrs; cb.Positions = floats;
            flowLayoutPanelEx_radiate.Add(ToFlowCell(false, clrs, floats));
            clrs[0] = Color.White;
            clrs[1] = Color.Black;
            clrs[2] = Color.White;
            floats[0] = 0;
            floats[1] = 0.5f;
            floats[2] = 1;
            cb.Colors = clrs; cb.Positions = floats;
            flowLayoutPanelEx_radiate.Add(ToFlowCell(false, clrs, floats));
            clrs[0] = Color.Pink;
            clrs[1] = Color.SlateGray;
            clrs[2] = Color.GreenYellow;
            floats[0] = 0;
            floats[1] = 0.5f;
            floats[2] = 1;
            cb.Colors = clrs; cb.Positions = floats;
            flowLayoutPanelEx_radiate.Add(ToFlowCell(false, clrs, floats));
            clrs[0] = Color.SlateGray;
            clrs[1] = Color.BlueViolet;
            clrs[2] = Color.DimGray;
            floats[0] = 0;
            floats[1] = 0.5f;
            floats[2] = 1;
            cb.Colors = clrs; cb.Positions = floats;
            flowLayoutPanelEx_radiate.Add(ToFlowCell(false, clrs, floats));
            clrs[0] = Color.Indigo;
            clrs[1] = Color.Maroon;
            clrs[2] = Color.NavajoWhite;
            floats[0] = 0;
            floats[1] = 0.5f;
            floats[2] = 1;
            cb.Colors = clrs; cb.Positions = floats;
            flowLayoutPanelEx_radiate.Add(ToFlowCell(false, clrs, floats));
        }
        #endregion

        #region 按钮操作Flow  (文件操作)
        private string _solidExt = "单色文件(*.nssolid)|*.nssolid";
        private string _linearExt = "渐变文件(*.nslinear)|*.nslinear";
        private string _radiateExt = "放射文件(*.nsradiate)|*.nsradiate";
        //保存
        private void button_flow_save_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Tag == (object)"solid")
            {
                string path = Tool.FileTool.OpenSaveFileDialog("default", _solidExt);
                if (path == "") return;
                FileStream fs = new FileStream(path, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                flowLayoutPanelEx_solid.Serialize(fs, bf);
                fs.Close();
            }
            else if (btn.Tag == (object)"linear")
            {
                string path = Tool.FileTool.OpenSaveFileDialog("default", _linearExt);
                if (path == "") return;
                FileStream fs = new FileStream(path, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                flowLayoutPanelEx_linear.Serialize(fs, bf);
                fs.Close();
            }
            else if (btn.Tag == (object)"radiate")
            {
                string path = Tool.FileTool.OpenSaveFileDialog("default", _radiateExt);
                if (path == "") return;
                FileStream fs = new FileStream(path, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                flowLayoutPanelEx_radiate.Serialize(fs, bf);
                fs.Close();
            }
        }
        //加载
        private void button_flow_load_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Tag == (object)"solid")
            {
                string openFilePath = Tool.FileTool.OpenFileDialog(_solidExt);
                if (openFilePath == "") return;
                FileStream fs = new FileStream(openFilePath, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                flowLayoutPanelEx_solid.ClearAllCell();
                flowLayoutPanelEx_solid.Deserialize(fs, bf);
                fs.Close();
            }
            else if (btn.Tag == (object)"linear")
            {
                string openFilePath = Tool.FileTool.OpenFileDialog(_linearExt);
                if (openFilePath == "") return;
                FileStream fs = new FileStream(openFilePath, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                flowLayoutPanelEx_linear.ClearAllCell();
                flowLayoutPanelEx_linear.Deserialize(fs, bf);
                fs.Close();
            }
            else if (btn.Tag == (object)"radiate")
            {
                string openFilePath = Tool.FileTool.OpenFileDialog(_radiateExt);
                if (openFilePath == "") return;
                FileStream fs = new FileStream(openFilePath, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                flowLayoutPanelEx_radiate.ClearAllCell();
                flowLayoutPanelEx_radiate.Deserialize(fs, bf);
                fs.Close();
            }
        }
        //添加flow按钮
        private void button_solid_add(object sender, EventArgs e)
        {
            Color clr = solidUserControl1.color;
            FlowCell cell = new FlowCell(new SolidBrush(clr));
            Form_Message fm_msg = new Form_Message();
            string str = clr.ToString();
            if (fm_msg.ShowDialog() == DialogResult.OK)
            {
                cell.Text = str;
                flowLayoutPanelEx_solid.Add(cell);
            }
        }
        private void button_linear_add_Click(object sender, EventArgs e)
        {
            NSLinearGradientBrushInfo LinearGradientBrushInfo = linearGradientUserControl1.LinearGradientBrushInfo;
            LinearGradientBrush br = new LinearGradientBrush(ClientRectangle, Color.Red, Color.FromArgb(255, 0, 255, 0), LinearGradientMode.Horizontal);
            br.InterpolationColors = LinearGradientBrushInfo.ColorBlend;
            FlowCell cell = new FlowCell(br);
            Form_Message fm_msg = new Form_Message();
            string str = "渐变色" + flowLayoutPanelEx_linear.Controls.Count.ToString();
            if (fm_msg.ShowDialog() == DialogResult.OK)
            {
                cell.Text = str;
                flowLayoutPanelEx_linear.Add(cell);
            }
        }
        private void button_radiate_add_Click(object sender, EventArgs e)
        {
            NSLinearGradientBrushInfo LinearGradientBrushInfo = linearGradientUserControl2.LinearGradientBrushInfo;


            PathGradientBrush br = new PathGradientBrush(_pointsPathBrushExample);

            br.InterpolationColors = LinearGradientBrushInfo.ColorBlend;
            FlowCell cell = new FlowCell(br);
            Form_Message fm_msg = new Form_Message();
            string str = "渐变色" + flowLayoutPanelEx_radiate.Controls.Count.ToString();
            if (fm_msg.ShowDialog() == DialogResult.OK)
            {
                cell.Text = str;
                flowLayoutPanelEx_radiate.Add(cell);
            }
        }

        /// <summary>
        ///  居中 拉升 单选 
        /// </summary>
        public bool EanbleImageDrawMode
        {
            get
            {
                return _eanbleImageDrawMode;
            }
            set
            {
                radio_pos_center.Enabled = value;
                radio_pos_stretch.Enabled = value;
                _eanbleImageDrawMode = value;
            }
        }
        private bool _eanbleImageDrawMode = true;
        #endregion

        #region 底纹通知
        // 前景色按钮通知
        private void buttonEx_ForeColor_Click(object sender, EventArgs e)
        {
            ColorDialog Dlg = new ColorDialog();
            Dlg.FullOpen = true;
            Dlg.Color = brushData.HatchBrushInfo.ForeColor;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                hatchBrushInfo.ForeColor = Color.FromArgb(hatchBrushInfo.ForeColor.A, Dlg.Color);
                buttonEx_ForeColor.Color = hatchBrushInfo.ForeColor;
                if (_hatchBrushExample != null)
                {
                    _hatchBrushExample.Dispose();
                }
                _hatchBrushExample = new HatchBrush(hatchBrushInfo.Style, hatchBrushInfo.ForeColor, hatchBrushInfo.BackColor);
                Notify();
            }
            Invalidate();
        }
        //背景色按钮通知
        private void buttonEx_BackColor_Click(object sender, EventArgs e)
        {
            ColorDialog Dlg = new ColorDialog();
            Dlg.FullOpen = true;
            Dlg.Color = hatchBrushInfo.BackColor;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                hatchBrushInfo.BackColor = Color.FromArgb(hatchBrushInfo.BackColor.A, Dlg.Color);
                buttonEx_BackColor.Color = hatchBrushInfo.BackColor;
                HatchStyle hs = _hatchBrushExample.HatchStyle;
                if (_hatchBrushExample != null)
                {
                    _hatchBrushExample.Dispose();
                }
                _hatchBrushExample = new HatchBrush(hatchBrushInfo.Style, hatchBrushInfo.ForeColor, hatchBrushInfo.BackColor);
                Notify();
            }
            Invalidate();
        }
        /// <summary> 
        /// 透明条发生变化通知
        /// </summary>
        void Event_HatchBrushControl()
        {
            float foreA = (HScrollBarUserControl_ForeColor.Value * 2.55f);
            float bkA = (hScrollBarUserControl_BackColor.Value * 2.55f);
            hatchBrushInfo.ForeAValue = (int)HScrollBarUserControl_ForeColor.Value;
            hatchBrushInfo.BackAValue = (int)hScrollBarUserControl_BackColor.Value;
            hatchBrushInfo.ForeColor = Color.FromArgb((int)foreA, hatchBrushInfo.ForeColor);
            hatchBrushInfo.BackColor = Color.FromArgb((int)bkA, hatchBrushInfo.BackColor);
            buttonEx_BackColor.Color = hatchBrushInfo.BackColor;
            buttonEx_ForeColor.Color = hatchBrushInfo.ForeColor;
            if (_hatchBrushExample != null)
                _hatchBrushExample.Dispose();
            _hatchBrushExample = new HatchBrush(hatchBrushInfo.Style, hatchBrushInfo.ForeColor, hatchBrushInfo.BackColor);
            Notify();
            Invalidate();
        }
        #endregion

        #region 图片辅助函数功能块
        private string imageExt = "图片文件(*.jpg*)|*.jpg*";
        /// <summary>
        /// 设置单选按钮的Eanble
        /// </summary>
        void RadioButtonEanble()
        {
            //image 平铺Eanble
            radio_title_none.Enabled = radio_pos_tile.Checked;
            radio_title_horizontal.Enabled = radio_pos_tile.Checked;
            radio_title_vertical.Enabled = radio_pos_tile.Checked;
            radio_title_hv.Enabled = radio_pos_tile.Checked;
        }
        /// <summary>
        /// 获取radio图片绘制模式
        /// </summary>
        /// <returns></returns>
        ImageDrawMode GetImageDrawMode()
        {
            if (!EanbleImageDrawMode)
                return ImageDrawMode.Wrap;
            if (radio_pos_tile.Checked)
                return ImageDrawMode.Wrap;
            if (radio_pos_center.Checked)
                return ImageDrawMode.Center;
            return ImageDrawMode.Stretch;
        }
        /// <summary>
        ///  设置radio平铺镜像
        /// </summary>
        /// <returns></returns>
        void SetImageDrawMode(ImageDrawMode idm)
        {
            if (!EanbleImageDrawMode)
                idm = ImageDrawMode.Wrap;
            radio_pos_tile.Checked = idm == ImageDrawMode.Wrap;
            radio_pos_center.Checked = idm == ImageDrawMode.Center;
            radio_pos_stretch.Checked = idm == ImageDrawMode.Stretch;
        }
        /// <summary>
        /// 获取radio平铺镜像
        /// </summary>
        /// <returns></returns>
        WrapMode GetWrapMode()
        {
            if (radio_title_none.Checked)
                return WrapMode.Tile;
            if (radio_title_horizontal.Checked)
                return WrapMode.TileFlipX;
            if (radio_title_vertical.Checked)
                return WrapMode.TileFlipY;
            return WrapMode.TileFlipXY;
        }
        bool _bSetMode = false;
        /// <summary>
        ///  设置radio平铺镜像
        /// </summary>
        /// <returns></returns>
        void SetWrapMode(WrapMode wm)
        {
            _bSetMode = true;
            radio_title_none.Checked = wm == WrapMode.Tile;
            radio_title_horizontal.Checked = wm == WrapMode.TileFlipX;
            radio_title_vertical.Checked = wm == WrapMode.TileFlipY;
            radio_title_hv.Checked = wm == WrapMode.TileFlipXY;
        }
        #endregion

        #region 事件  (画刷发生变化实时通知托管)
        public delegate void DelegateBrushChanged();
        public DelegateBrushChanged BrushChanged;
        private void Notify()
        {
            if (!_EanbleNotify) return;
            UpdateData(true);
            if (BrushChanged != null)
                BrushChanged();
        }
        #endregion
    }
}
