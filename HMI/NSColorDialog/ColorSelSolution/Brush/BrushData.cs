using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using System.Drawing.Design; 
using System.Resources;
using NetSCADA6.Common.NSColorManger.Properties;

namespace NetSCADA6.Common.NSColorManger
{
    /// <summary>
    /// 画刷数据集合与操作。
    /// </summary>
    [EditorAttribute(typeof(BrushDataEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(BrushConverter))]
    public class BrushData : ICloneable
    {
        #region 构造函数、初始化
        public BrushData()
        {
            Init();
        }
        private void Init()
        {
            BrushType = NSBrushType.Solid;
            SolidBrushInfo = new NSSolidBrushInfo(Color.Red);
            ColorBlend cb = new ColorBlend(2);
            Color[] clrs = new Color[2];
            float[] floats = new float[2];
            clrs[0] = Color.White;
            clrs[1] = Color.Black;
            floats[0] = 0;
            floats[1] = 1;
            cb.Colors = clrs; cb.Positions = floats;
            LinearGradientBrushInfo = new NSLinearGradientBrushInfo(cb, 0);
            HatchBrushInfo = new NSHatchBrushInfo();
            TextrueBrushInfo = new NSTextrueBrushInfo();
            LinearGradientBrush h;
                HatchBrush global;
                TextureBrush a;
            PathGradientBrushInfo = new NSPathGradientBrushInfo(cb);
        }
        #endregion

        #region properetyInfo
        internal NSBrushType BrushType = NSBrushType.Null;
        internal NSSolidBrushInfo SolidBrushInfo;
        internal NSLinearGradientBrushInfo LinearGradientBrushInfo;
        internal NSHatchBrushInfo HatchBrushInfo;
        internal NSTextrueBrushInfo TextrueBrushInfo;
        internal NSPathGradientBrushInfo PathGradientBrushInfo;
        #endregion


        /// <summary>
        /// 是否是图片.仅当Brush为TextureBrush，并且ImageMode为Center或Stretch时返回true
        /// </summary>
        public bool IsImage
        {
            get
            {
                if (BrushType == NSBrushType.Textrue)
                {
                    if (TextrueBrushInfo.ImageDrawMode == ImageDrawMode.Center || TextrueBrushInfo.ImageDrawMode == ImageDrawMode.Stretch)
                    {
                        return true;
                        
                    }
                }
                return false;
            }
        }

        public ImageDrawMode ImageMode
        {
            get { return TextrueBrushInfo.ImageDrawMode; }
        }


        public void InitNull()
        {
            BrushType = NSBrushType.Null;
        }

        public void InitSolidBrush(Color color)
        {
            BrushType = NSBrushType.Solid;
            SolidBrushInfo.Color = color;
        }


        public void InitLinearGradientBrush(Color color1, Color color2, float angle)
        {
            BrushType = NSBrushType.LinearGradient;
            LinearGradientBrushInfo.Angle = angle;
            ColorBlend cb = new ColorBlend();
            Color[] cls = new Color[2]; cls[0] = color1; cls[1] = color2;
            cb.Colors = cls;
            float[] fs = new float[2]; fs[0] = 0; fs[1] = 1;
            cb.Positions = fs;
            LinearGradientBrushInfo.ColorBlend = cb;
        }

        internal Brush CreateExampleBrush(RectangleF rf, GraphicsPath path = null)
        {
            if (rf.Width <= 0)
                rf.Width = 1;
            if (rf.Height <= 0)
                rf.Height = 1;
            if (BrushType == NSBrushType.Null)
            {
                return null;
            }

            if (BrushType == NSBrushType.Solid)
            {
                return new SolidBrush(SolidBrushInfo.Color);
            }

            if (BrushType == NSBrushType.LinearGradient)
            {
                rf.Offset(-1, -1);
                rf.Width += 1;
                rf.Height += 1;
                LinearGradientBrush LBrush = new LinearGradientBrush(rf, Color.Red, Color.FromArgb(255, 0, 255, 0), LinearGradientBrushInfo.Angle);
                LBrush.InterpolationColors = LinearGradientBrushInfo.ColorBlend;
                return LBrush;
            }

            if (BrushType == NSBrushType.Hatch)
            {
                return new HatchBrush(HatchBrushInfo.Style, HatchBrushInfo.ForeColor, HatchBrushInfo.BackColor);
            }

            if (BrushType == NSBrushType.Textrue)
            {
                if (TextrueBrushInfo.IsResource)
                {
                    if (TextrueBrushInfo.ResourceImage != "")
                    {
                        Object res = Resources.ResourceManager.GetObject(TextrueBrushInfo.ResourceImage);
                        TextureBrush _ImageBrush = new TextureBrush((Image)res, TextrueBrushInfo.WrapMode);
                        return _ImageBrush;
                    }
                }
                else
                {
                    if (TextrueBrushInfo.FileName != "")
                    {
                        Bitmap _ImageBmp = new Bitmap(TextrueBrushInfo.FileName);
                        TextureBrush _ImageBrush = new TextureBrush(_ImageBmp, TextrueBrushInfo.WrapMode);
                        return _ImageBrush;
                    }
                }
            }

            if (BrushType == NSBrushType.PathGradient)
            {
                if (path != null)
                {
                    PathGradientBrush pgb = new PathGradientBrush(path);
                    pgb.InterpolationColors = PathGradientBrushInfo.LinearGradient.ColorBlend;
                    return pgb;
                }
                if (rf != null)
                {
                    PointF[] pts = new PointF[4];
                    pts[0] = new PointF(rf.Left, rf.Top);
                    pts[1] = new PointF(rf.Right, rf.Top);
                    pts[2] = new PointF(rf.Right, rf.Bottom);
                    pts[3] = new PointF(rf.Left, rf.Bottom);
                    PathGradientBrush pgb1 = new PathGradientBrush(pts);
                    pgb1.InterpolationColors = PathGradientBrushInfo.LinearGradient.ColorBlend;
                    return pgb1;
                }
            }
            return new SolidBrush(Color.Black);
        }

        public Brush CreateBrush(RectangleF rf, GraphicsPath path = null)
        {
            if (rf.Width <= 0)
                rf.Width = 1;
            if (rf.Height <= 0)
                rf.Height = 1;
            if (BrushType == NSBrushType.Null)
            {
                return null;
            }

            if (BrushType == NSBrushType.Solid)
            {
                return new SolidBrush(SolidBrushInfo.Color);
            }

            if (BrushType == NSBrushType.LinearGradient)
            {
                LinearGradientBrush LBrush = new LinearGradientBrush(rf, Color.Red, Color.FromArgb(255, 0, 255, 0), LinearGradientBrushInfo.Angle);
                LBrush.InterpolationColors = LinearGradientBrushInfo.ColorBlend;
                return LBrush;
            }

            if (BrushType == NSBrushType.Hatch)
            {
                return new HatchBrush(HatchBrushInfo.Style, HatchBrushInfo.ForeColor, HatchBrushInfo.BackColor);
            }

            if (BrushType == NSBrushType.Textrue)
            {
                if (TextrueBrushInfo.IsResource)
                {
                    if (TextrueBrushInfo.ResourceImage != "")
                    {
                        Image res = (Image)Resources.ResourceManager.GetObject(TextrueBrushInfo.ResourceImage);
                        TextureBrush _ImageBrush = new TextureBrush(res, TextrueBrushInfo.WrapMode);
                        return _ImageBrush;
                    }
                }
                else
                {
                    if (TextrueBrushInfo.FileName != "")
                    {
                        Bitmap _ImageBmp = new Bitmap(TextrueBrushInfo.FileName);
                        TextureBrush _ImageBrush = new TextureBrush(_ImageBmp, TextrueBrushInfo.WrapMode);
                        return _ImageBrush;
                    }
                }
            }

            if (BrushType == NSBrushType.PathGradient)
            {
                if (path != null)
                {
                    PathGradientBrush pgb = new PathGradientBrush(path);
                    pgb.InterpolationColors = PathGradientBrushInfo.LinearGradient.ColorBlend;
                    return pgb;
                }
                if (rf != null)
                {
                    PointF[] pts = new PointF[4];
                    pts[0] = new PointF(rf.Left, rf.Top);
                    pts[1] = new PointF(rf.Right, rf.Top);
                    pts[2] = new PointF(rf.Right, rf.Bottom);
                    pts[3] = new PointF(rf.Left, rf.Bottom);
                    PathGradientBrush pgb1 = new PathGradientBrush(pts);
                    pgb1.InterpolationColors = PathGradientBrushInfo.LinearGradient.ColorBlend;
                    return pgb1;
                }
            }
            return new SolidBrush(Color.Black);
        }

        /// <summary>
        /// 获取brsuh中的Color.仅当Brush为SolidBrush时生效，否则返回黑色
        /// </summary>
        /// <returns></returns>
        public Color GetBrushColor()
        {
            if (BrushType == NSBrushType.Solid)
                return SolidBrushInfo.Color;
            return Color.Black;
        }

        /// <summary>
        /// 获取Brush中的Iamge.仅当Brush为TextureBrush时生效，否则返回空值
        /// </summary>
        /// <param name="brush"></param>
        /// <returns></returns> 
        public Image GetBrushImage(Brush brush)
        {
            if (brush is TextureBrush)
                return (brush as TextureBrush).Image;
            return null;
        }


        #region 序列化、克隆
        int version = 1;
        public void Serialize(BinaryFormatter bf, Stream s)
        {
            bf.Serialize(s, version);
            bf.Serialize(s, (int)this.BrushType);
            this.SolidBrushInfo.Serialize(bf, s);
            this.LinearGradientBrushInfo.Serialize(bf, s);
            this.TextrueBrushInfo.Serialize(bf, s);
            this.HatchBrushInfo.Serialize(bf, s);
            this.PathGradientBrushInfo.Serialize(bf, s);

        }
        public void Deserialize(BinaryFormatter bf, Stream s)
        {
            version = (int)bf.Deserialize(s);
            BrushType = (NSBrushType)bf.Deserialize(s);
            this.SolidBrushInfo.Deserialize(bf, s);
            this.LinearGradientBrushInfo.Deserialize(bf, s);
            this.TextrueBrushInfo.Deserialize(bf, s);
            this.HatchBrushInfo.Deserialize(bf, s);
            this.PathGradientBrushInfo.Deserialize(bf, s);
        }
        public object Clone()
        {
            BrushData other = new BrushData();
            other.BrushType = this.BrushType;
            other.SolidBrushInfo = this.SolidBrushInfo.Clone();
            other.LinearGradientBrushInfo = this.LinearGradientBrushInfo.Clone();
            other.TextrueBrushInfo = this.TextrueBrushInfo.Clone();
            other.HatchBrushInfo = this.HatchBrushInfo.Clone();
            other.PathGradientBrushInfo = this.PathGradientBrushInfo.Clone();
            return other;
        }
        #endregion

    }


}
