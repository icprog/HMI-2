using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using NetSCADA6.HMI.NSDrawObj;
using NetSCADA6.HMI.NSDrawObj.PropertyEdit;
using NetSCADA6.NSInterface.HMI.DrawObj;

namespace NetSCADA6.HMI.NSDrawVector
{
	/// <summary>
	/// 矩形控件
	/// </summary>
	[ToolboxBitmap(typeof(DrawRect), "Resources.Rect.bmp")]
	[DisplayName("矩形")]
	[Ortho(OrthoMode.Square)]
	public partial class DrawRect : DrawVector
	{
		#region property

		internal float XRoundBk;
		internal float YRoundBk;
		private float _roundLimit;
		private float _xRound;
		[Category("布局")]
		[DisplayName("水平圆角半径")]
		[Description("矩形控件的水平圆角半径")]
		[PropertyOrder(1000)]
		public float XRound
		{
			set
			{
				SetRound(value, true);
			}
			get { return _xRound; }
		}
		private float _yRound;
		[Category("布局")]
		[DisplayName("垂直圆角半径")]
		[Description("矩形控件的垂直圆角半径")]
		[PropertyOrder(1001)]
		public float YRound
		{
			set
			{
				SetRound(value, false);
			}
			get { return _yRound; }
		}

		#endregion

		#region protected function
		protected override void OnGeneratePath(ref GraphicsPath path)
		{
			LimitRound();
			GenerateRoundedPath(ref path);

			GenerateCustom();
		}

		protected override void BackupData()
		{
			base.BackupData();

			XRoundBk = _xRound;
			YRoundBk = _yRound;
		}
		#endregion

		#region common
		protected override void OnInitialization()
		{
			_roundLimit = (_xRound > _yRound) ? _xRound : _yRound;
		}
		public override void Serialize(BinaryFormatter bf, Stream s)
		{
			base.Serialize(bf, s);

			const int version = 1;

			bf.Serialize(s, version);
			bf.Serialize(s, _xRound);
			bf.Serialize(s, _yRound);
		}
		public override void Deserialize(BinaryFormatter bf, Stream s)
		{
			base.Deserialize(bf, s);

			int version = (int)bf.Deserialize(s);
			_xRound = (float)bf.Deserialize(s);
			_yRound = (float)bf.Deserialize(s);
		}
		public override object Clone()
		{
			var obj = (DrawRect)base.Clone();

			obj._customDatas = (PointF[]) _customDatas.Clone();
			return obj;
		}

		#endregion

		#region virtual property
		public override DrawType Type { get { return DrawType.Rect; } }
		#endregion

		#region private function
		private void GenerateRoundedPath(ref GraphicsPath path)
		{
			float w = _xRound * 2;
			float h = _yRound * 2;
			float xLen = Rect.Width - w;
			float yLen = Rect.Height - h;
			bool hasArc = (_xRound > 0 && _yRound > 0);
			bool hasXLine = (xLen > 0);
			bool hasYLine = (yLen > 0);

			path.Reset();
			if (hasArc)
				GraphicsTool.AddArc(path, new RectangleF(Rect.X, Rect.Y, w, h), 180, 90);
			if (hasXLine)
				path.AddLine(Rect.X + _xRound, Rect.Y, Rect.X + Rect.Width - _xRound, Rect.Y);
			if (hasArc)
				GraphicsTool.AddArc(path, new RectangleF(Rect.X + xLen, Rect.Y, w, h), 270, 90);
			if (hasYLine)
				path.AddLine(Rect.X + Rect.Width, Rect.Y + _yRound, Rect.X + Rect.Width, Rect.Y + Rect.Height - _yRound);
			if (hasArc)
				GraphicsTool.AddArc(path, new RectangleF(Rect.X + xLen, Rect.Y + yLen, w, h), 0, 90);
			if (hasXLine)
				path.AddLine(Rect.X + Rect.Width - _xRound, Rect.Y + Rect.Height, Rect.X + _xRound, Rect.Y + Rect.Height);
			if (hasArc)
				GraphicsTool.AddArc(path, new RectangleF(Rect.X, Rect.Y + yLen, w, h), 90, 90);
			if (hasYLine)
				path.AddLine(Rect.X, Rect.Y + Rect.Height - _yRound, Rect.X, Rect.Y + _yRound);

			path.CloseFigure();
		}
		internal void SetRound(float value, bool isX)
		{
			const float min = 0;
			float wMax = Rect.Width / 2f;
			float hMax = Rect.Height / 2f;
			float max = isX ? wMax : hMax;

			value = (value > min) ? value : min;
			value = (value < max) ? value : max;

			_xRound = (value < wMax) ? value : wMax;
			_yRound = (value < hMax) ? value : hMax;

			_roundLimit = (_xRound > _yRound) ? _xRound : _yRound;

			LoadGeneratePathEvent();
		}
		private void LimitRound()
		{
			_xRound = (_xRound > _roundLimit) ? _xRound : _roundLimit;
			_yRound = (_yRound > _roundLimit) ? _yRound : _roundLimit;

			float wMax = Rect.Width / 2f;
			float hMax = Rect.Height / 2f;
			_xRound = (_xRound < wMax) ? _xRound : wMax;
			_yRound = (_yRound < hMax) ? _yRound : hMax;
		}
		#endregion
	}
}
