using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NetSCADA6.Common.NSColorManger;
using NetSCADA6.HMI.NSDrawObj;
using NetSCADA6.HMI.NSDrawObj.PropertyEdit;
using NetSCADA6.NSInterface.HMI.DrawObj;
using NetSCADA6.NSInterface.HMI.Var;

namespace NetSCADA6.HMI.NSDrawVector
{
    /// <summary>
    /// 文本控件
    /// </summary>
	[ToolboxBitmap(typeof(DrawRect), "Resources.Rect.bmp")]
	[DisplayName("文本")]
	[Ortho(OrthoMode.Square)]
	public class DrawText : DrawVector
    {
        public DrawText()
        {
			_textBrush.InitData(Color.Black);
        }

        #region drawVector
		#region virtual property
		public override DrawType Type { get { return DrawType.Text; } }
		#endregion

		#region protected function
		protected override void OnPaint(Graphics g)
		{
			base.OnPaint(g);

			if (_textBrush.Content != null)
				g.DrawString(_text, _font, _textBrush.Content, Rect, _format);
		}
		protected override void GeneratePath()
		{
			base.GeneratePath();

			_textBrush.Generate(Rect, BasePath);
		}
		protected override void InitContent()
		{
			base.InitContent();

			if (_textBrush.Content == null)
				_textBrush.InitContent(Rect, BasePath);
		}
		#endregion

		#region common
		#region serialize
		public override void Serialize(BinaryFormatter bf, Stream s)
        {
            base.Serialize(bf, s);

            const int version = 1;

            bf.Serialize(s, version);
            bf.Serialize(s, _font);
            bf.Serialize(s, _text);

			_textBrush.Data.Serialize(bf, s);
        }
        public override void Deserialize(BinaryFormatter bf, Stream s)
        {
            base.Deserialize(bf, s);

            int version = (int)bf.Deserialize(s);
            _font = (Font)bf.Deserialize(s);
            _text = (string)bf.Deserialize(s);

			_textBrush.Data.Deserialize(bf, s);
        }
        #endregion

		#region clone
		public override Object Clone()
        {
            DrawText obj = (DrawText)base.Clone();
            obj._font = (Font)_font.Clone();
            obj._format = (StringFormat)_format.Clone();
			obj._textBrush = obj._textBrush.Clone() as BrushManager;

            return obj;
        }
		#endregion

		#region dispose
		protected override void DisposeResource()
		{
			if (Disposed)
			{
				_font.Dispose();
				_format.Dispose();
				if (_textBrush.Content != null)
					_textBrush.Content.Dispose();
			}

			base.DisposeResource();
		}
		#endregion
		#endregion
		#endregion

		#region property
		private Font _font = new Font("微软雅黑", 20, GraphicsUnit.Pixel);
		/// <summary>
		/// 字体
		/// </summary>
		[Category("文本")]
		[DisplayName("字体")]
		[PropertyOrder(1002)]
        public Font Font
        {
            set
            {
                _font = value;
                Invalidate();
            } 
            get { return _font; }
        }
        private string _text = "Text";
		/// <summary>
		/// 文本
		/// </summary>
		[Category("文本")]
		[DisplayName("文本")]
		[PropertyOrder(1001)]
		public string Text
        {
            set
            {
                _text = value;
                Invalidate();
            }
            get { return _text; }
        }
        private StringFormat _format = new StringFormat();
		/// <summary>
		/// 格式
		/// </summary>
		[Category("布局")]
		[DisplayName("对齐")]
		[PropertyOrder(1001)]
		public StringFormat Format
        {
            set
            {
                _format = value;
                Invalidate();
            }
            get { return _format; }
        }

    	private BrushManager _textBrush = new BrushManager();
		/// <summary>
		/// 文本画刷
		/// </summary>
		[Category("外观")]
		[DisplayName("文本填充")]
		[Description("文本的填充方式")]
    	public BrushData TextBrushData
    	{
			set
			{
				_textBrush.Data = value;

				_textBrush.InitContent(Rect, BasePath);
				Invalidate();
			}
			get { return _textBrush.Data; }
    	}
        #endregion

		#region var
		private static readonly string[] _textPropertyNames = new[] 
        {"DrawText.Text"};
        public new static string[] GetPropertyNames()
        {
			return _textPropertyNames;
        }
        public override string[] PropertyNames
        {
			get { return _textPropertyNames; }
        }
        protected override void OnDataChanged(IPropertyExpression expression)
        {
			if (expression.Index.ClassType == (int)Type)
            {
				switch (expression.Index.Index)
                {
                    case 0:     //text
						Text = expression.DecimalValue.ToString();
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
    }
}
