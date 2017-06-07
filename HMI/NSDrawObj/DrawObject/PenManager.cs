using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using NetSCADA6.Common.NSColorManger;

namespace NetSCADA6.HMI.NSDrawObj
{
	/// <summary>
	/// 画笔管理类
	/// </summary>
	public class PenManager : ICloneable
	{
		#region property
		private PenData _data = new PenData();
		/// <summary>
		/// 数据
		/// </summary>
		public PenData Data
		{
			set { _data = value; }
			get { return _data; }
		}
		private Pen _content;
		/// <summary>
		/// 画笔
		/// </summary>
		public Pen Content
		{
			get { return _content; }
		}
		#endregion

		#region public function
		/// <summary>
		/// 数据初始化
		/// </summary>
		/// <param name="color"></param>
		/// <param name="width"></param>
		public void InitData(Color color, float width)
		{
			_data.Init(color, width);
		}
		public void InitData()
		{
			_data.Init();
		}
		/// <summary>
		/// 初始化Content
		/// </summary>
		/// <param name="rf"></param>
		/// <param name="path"></param>
		public void InitContent(RectangleF rf, GraphicsPath path)
		{
			CreateContent(ref _content, rf, path);
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
				if (!(_content.Brush is TextureBrush || _content.Brush is SolidBrush))
				{
					CreateContent(ref _content, rf, path);
				}
			}
		}
		public void Draw(Graphics g, RectangleF rf, GraphicsPath path)
		{
			if (_content == null)
				return;

			//流动管道
			if (_data.IsPiple)
				_data.DrawPath(g, path);
			else
			{
				//如果是平铺图片模式，Brush需要偏移
				TextureBrush texture = _content.Brush as TextureBrush;
				if (texture != null)
				{
					//Pen.Brush的获取和设置应该都是Clone实现的
					texture.ResetTransform();
					texture.TranslateTransform(rf.X, rf.Y);
					_content.Brush = texture;
					texture.Dispose();
				}

				g.DrawPath(_content, path);
			}
		}
		#endregion

		#region private function
		private void CreateContent(ref Pen content, RectangleF rf, GraphicsPath path)
		{
			const float redundancy = 0.1f;
			rf.Inflate(_data.Width / 2 + redundancy, _data.Width / 2 + redundancy);
			
			if (content != null)
				content.Dispose();
			content = _data.CreatePen(rf, path);
		}
		#endregion

		#region clone
		public object Clone()
		{
			var obj = new PenManager {_data = _data.Clone() as PenData};
			if (_content != null)
				obj._content = _content.Clone() as Pen;

			return obj;
		}
		#endregion
	}
}
