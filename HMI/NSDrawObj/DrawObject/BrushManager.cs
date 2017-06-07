using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using NetSCADA6.Common.NSColorManger;

namespace NetSCADA6.HMI.NSDrawObj
{
	/// <summary>
	/// 画刷管理类
	/// </summary>
	public class BrushManager : ICloneable
	{
		#region property
		private BrushData _data = new BrushData();
		/// <summary>
		/// 数据
		/// </summary>
		public BrushData Data
		{
			set { _data = value; }
			get { return _data; }
		}
		private Brush _content;
		/// <summary>
		/// 画刷
		/// </summary>
		public Brush Content
		{
			get { return _content; }
		}
		#endregion
		
		#region public function
		/// <summary>
		/// 数据初始化
		/// </summary>
		/// <param name="color1"></param>
		/// <param name="color2"></param>
		/// <param name="angle"></param>
		public void InitData(Color color1, Color color2, float angle)
		{
			_data.InitLinearGradientBrush(color1, color2, angle);
		}
		public void InitData(Color color)
		{
			_data.InitSolidBrush(color);
		}
		public void InitData()
		{
			_data.InitNull();
		}
		/// <summary>
		/// 初始化Content
		/// </summary>
		/// <param name="rf"></param>
		/// <param name="path"></param>
		public void InitContent(RectangleF rf, GraphicsPath path)
		{
			if (_content != null)
				_content.Dispose();
			_content = _data.CreateBrush(rf, path);
		}
		/// <summary>
		/// 生成Content,仅限于rf和path发生变化的情况，否则使用InitContent
		/// </summary>
		/// <param name="rf"></param>
		/// <param name="path"></param>
		public void Generate(RectangleF rf, GraphicsPath path)
		{
			if (_content != null)
			{
				if (!(_content is TextureBrush || _content is SolidBrush))
				{
					_content.Dispose();
					_content = _data.CreateBrush(rf, path);
				}
			}
		}
		public void Draw(Graphics g, RectangleF rf, GraphicsPath path)
		{
			if (_content == null)
				return;
			
			if (_data.IsImage)			//DrawImage
			{
				Image im = _data.GetBrushImage(_content);
				if (im == null)
					return;

				if (_data.ImageMode == ImageDrawMode.Stretch)
				{
					g.DrawImage(im, rf);
				}
				else if (_data.ImageMode == ImageDrawMode.Center)
				{
					Region rgn = g.Clip;

					Size s = im.Size;
					float x = rf.X + (rf.Width - s.Width) / 2;
					float y = rf.Y + (rf.Height - s.Height) / 2;

					g.SetClip(rf);
					g.DrawImage(im, x, y);

					//需要恢复，否则接下来的绘图不完整
					g.SetClip(rgn, CombineMode.Replace);
				}
			}
			else							//FillBrush
			{
				//如果是平铺图片模式，_brush需要偏移
				TextureBrush texture = _content as TextureBrush;
				if (texture != null)
				{
					texture.ResetTransform();
					texture.TranslateTransform(rf.X, rf.Y);
				}

				g.FillPath(_content, path);
			}
		}
		#endregion

		#region clone
		public object Clone()
		{
			BrushManager obj = new BrushManager {_data = _data.Clone() as BrushData};
			if (_content != null)
				obj._content = _content.Clone() as Brush;

			return obj;
		}
		#endregion
	}
}
