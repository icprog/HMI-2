using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Drawing.Drawing2D;
using NetSCADA6.Common.NSColorManger;

namespace NetSCADA6.Common.NSColorManger
{
    /// <summary>
    /// 图片
    /// </summary>
    internal class NSTextrueBrushInfo
    {
        public NSTextrueBrushInfo() { }

        public NSTextrueBrushInfo Clone()
        {
            NSTextrueBrushInfo other = new NSTextrueBrushInfo();
            other.FileName = this.FileName;
            other.WrapMode = this.WrapMode;
            other.ResourceImage = this.ResourceImage;
            other.ImageDrawMode = this.ImageDrawMode;
            return other;
        }
        public ImageDrawMode ImageDrawMode = ImageDrawMode.Wrap;
        public WrapMode WrapMode = WrapMode.Tile;
        public string FileName = "";
        public string ResourceImage = "";
        public bool IsResource
        {
            get
            {
                if (ResourceImage != "")
                    return true;
                return false;
            }
        }
        int version = 1;
        public void Serialize(BinaryFormatter bf, Stream s)
        {
            bf.Serialize(s, version);
            bf.Serialize(s, FileName);
            bf.Serialize(s, WrapMode);
            bf.Serialize(s, ResourceImage);
            bf.Serialize(s, (int)ImageDrawMode);
        }
        public void Deserialize(BinaryFormatter bf, Stream s)
        {
            version = (int)bf.Deserialize(s);
            FileName = (string)bf.Deserialize(s);
            WrapMode = (WrapMode)bf.Deserialize(s);
            ResourceImage = (string)bf.Deserialize(s);
            ImageDrawMode = (ImageDrawMode)bf.Deserialize(s);
        }
    }

}
