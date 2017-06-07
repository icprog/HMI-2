using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Drawing2D;



namespace NetSCADA6.Common.NSColorManger
{
    internal class FlowCell
    {
        public FlowCell(Brush brush, float angle = 0)
        {
            Angle = angle;
            if (brush is SolidBrush)
                Brush = brush;
            if (brush is LinearGradientBrush)
            {
                LinearGradientBrush lb = (LinearGradientBrush)brush;
                Rectangle Rect = new Rectangle(0, 0, CellLength, CellLength);
                Brush = new LinearGradientBrush(Rect, Color.Red, Color.FromArgb(255, 0, 255, 0), Angle);

                ((LinearGradientBrush)Brush).InterpolationColors = lb.InterpolationColors;

            }
            if (brush is HatchBrush)
            {
                Brush = brush;
            }
            if (brush is PathGradientBrush)
            {
                Brush = brush;
            }
        }
        Brush _brush;
        public Brush Brush
        {
            get
            {
                Rectangle Rect = new Rectangle(0, 0, CellLength, CellLength);
                if (_brush is LinearGradientBrush)
                {
                    LinearGradientBrush lb = (LinearGradientBrush)_brush;
                    if (lb.Rectangle != Rect)
                    {
                        Brush = new LinearGradientBrush(Rect, Color.Red, Color.FromArgb(255, 0, 255, 0), LinearGradientMode.Horizontal);
                        ((LinearGradientBrush)Brush).InterpolationColors = lb.InterpolationColors;
                    }
                }
                return _brush;
            }
            set
            {
                _brush = value;
            }
        }
        public string Text = "";
        public float Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }
        private float _angle = 0;
        int version = 1;
        public void Serialize(FileStream fs, BinaryFormatter bf)
        {
            bf.Serialize(fs, version);
            bf.Serialize(fs, Text);
            if (Brush is SolidBrush)
            {
                bf.Serialize(fs, NSBrushType.Solid);
                SolidBrush br = (SolidBrush)Brush;
                bf.Serialize(fs, br.Color);
            }
            else if (Brush is LinearGradientBrush)
            {
                bf.Serialize(fs, NSBrushType.LinearGradient);
                LinearGradientBrush lg = (LinearGradientBrush)Brush;
                bf.Serialize(fs, lg.InterpolationColors.Colors.Length);
                for (int i = 0; i < lg.InterpolationColors.Colors.Length; i++)
                {
                    bf.Serialize(fs, lg.InterpolationColors.Colors[i]);
                    bf.Serialize(fs, lg.InterpolationColors.Positions[i]);
                }
                bf.Serialize(fs, Angle); 
            }
            else if (Brush is PathGradientBrush)
            {
                bf.Serialize(fs, NSBrushType.PathGradient);
                PathGradientBrush pg = (PathGradientBrush)Brush;
                bf.Serialize(fs, pg.InterpolationColors.Colors.Length);
                for (int i = 0; i < pg.InterpolationColors.Colors.Length; i++)
                {
                    bf.Serialize(fs, pg.InterpolationColors.Colors[i]);
                    bf.Serialize(fs, pg.InterpolationColors.Positions[i]);
                }
            } 
        }
        public void Deserialize(FileStream fs, BinaryFormatter bf)
        {
            version = (int)bf.Deserialize(fs);
            Text = (string)bf.Deserialize(fs);
            NSBrushType BrushType = (NSBrushType)bf.Deserialize(fs);
            if (BrushType == NSBrushType.Solid)
            {
                Color clr = (Color)bf.Deserialize(fs);
                Brush = new SolidBrush(clr);
            }
            else if (BrushType == NSBrushType.LinearGradient)
            {
                int length = (int)bf.Deserialize(fs);
                Color[] clrs = new Color[length];
                float[] pos = new float[length];
                for (int i = 0; i < length; i++)
                {
                    clrs[i] = (Color)bf.Deserialize(fs);
                    pos[i] = (float)bf.Deserialize(fs);
                }
                Angle = (float)bf.Deserialize(fs);
                Rectangle Rect = new Rectangle(0, 0, CellLength, CellLength);
                Brush = new LinearGradientBrush(Rect, Color.Red, Color.FromArgb(255, 0, 255, 0), Angle);
                ColorBlend cb = new ColorBlend();
                cb.Colors = clrs;
                cb.Positions = pos;
                ((LinearGradientBrush)Brush).InterpolationColors = cb;
            }
            else if (BrushType == NSBrushType.PathGradient)
            {
                int length = (int)bf.Deserialize(fs);
                Color[] clrs = new Color[length];
                float[] pos = new float[length];
                for (int i = 0; i < length; i++)
                {
                    clrs[i] = (Color)bf.Deserialize(fs);
                    pos[i] = (float)bf.Deserialize(fs);
                }
                Point[] pts = new Point[4];
                pts[0] = new Point(0, 0);
                pts[1] = new Point(0, CellLength);
                pts[2] = new Point(CellLength - 4, CellLength);
                pts[3] = new Point(CellLength - 4, 0);
                GraphicsPath path = new GraphicsPath();
                path.AddLines(pts);
                Brush = new PathGradientBrush(path);
                ColorBlend cb = new ColorBlend();
                cb.Colors = clrs;
                cb.Positions = pos;
                ((PathGradientBrush)Brush).InterpolationColors = cb;
            }
        }
        const int CellLength = 50;
    }
    /////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 画刷预置链表
    /// </summary>
    internal class PresetList
    {
        private List<FlowCell> Data;

        public PresetList()
        {
            Data = new List<FlowCell>();
        }
        public void ClearAll()
        {
            Data.Clear();
        }
        public PresetList Clone()
        {
            PresetList other = new PresetList();
            for (int i = 0; i < Data.Count; i++)
            {
                other.Add(Data[i]);
            }
            return other;
        }

        #region 链表操作
        public void DisposeAll()
        {
            foreach (FlowCell cell in Data)
                cell.Brush.Dispose();
        }

        public int Count
        {
            get { return Data.Count; }
        }

        public FlowCell this[int index]
        {
            set
            {
                Data[index] = value;
            }
            get
            {
                return Data[index];
            }
        }
        #endregion

        public void Add(FlowCell cell)
        {
            Data.Add(cell);
        }

        public void Remove(FlowCell cell)
        {
            //     cell.Brush.Dispose();
            Data.Remove(cell);
        }

        public void Serialize(FileStream fs, BinaryFormatter bf)
        {
            bf.Serialize(fs, Data.Count);
            foreach (FlowCell cell in Data)
                cell.Serialize(fs, bf);
        }

        public void Deserialize(FileStream fs, BinaryFormatter bf)
        {
            int count = (int)bf.Deserialize(fs);
            FlowCell cell;
            for (int i = 0; i < count; i++)
            {
                cell = new FlowCell(null);
                cell.Deserialize(fs, bf);
                Data.Add(cell);
            }
        }

    }
}
