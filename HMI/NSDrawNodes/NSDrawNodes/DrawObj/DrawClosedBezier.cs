using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using NetSCADA6.Common.NSColorManger;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using NetSCADA6.NSInterface.HMI.DrawObj;
using NetSCADA6.HMI.NSDrawObj;

namespace NetSCADA6.HMI.NSDrawNodes
{

    public class DrawClosedBezier : DrawNodes
    {
        public DrawClosedBezier()
        {
        }
        #region property

        public override DrawType Type { get { return DrawType.ClosedBezier; } }

        /// <summary>
        /// 点point是否和datas数据构成的线段相交
        /// </summary> 
        public override bool FindIndex(List<PointF> datas, PointF point, ref int index)
        {
            int count = datas.Count;
            const int width = 6;
            bool isVisible = false;
            Pen p = new Pen(Color.Black, width);
            GraphicsPath path = new GraphicsPath();
            PointF[] ptfs = new PointF[4];
            List<PointF> list = MyTools.ConvertBeziers(datas, true);
            for (index = 1; index < count; index++)
            {
                ptfs = MyTools.GetBzr(list, index);
                path.Reset();
                path.AddBezier(ptfs[0], ptfs[1], ptfs[2], ptfs[3]);
                if (path.IsOutlineVisible(point, p))
                {
                    isVisible = true;
                    break;
                }
            }
            path.Dispose();
            p.Dispose();
            return isVisible;
        }

        public override void CreatingPaint(Graphics g)
        {
            List<PointF> ds = NodeDatas;
            if (ds != null && ds.Count > 1)
            {
                Pen p = PenData.CreatePen(Rect, Path);
                Brush br = BrushData.CreateBrush(Rect, Path);
                List<PointF> list = MyTools.ConvertBeziers(ds, true);

                if (list.Count > 0)
                {
                    GraphicsPath path = new GraphicsPath();
                    path.AddBeziers(list.ToArray());
                    g.FillPath(br, path);
                    g.DrawPath(p, path);
                }
                p.Dispose();
                br.Dispose();
            }
        }
        protected override void GenerateNodePath(ref GraphicsPath path)
        {
            if (NodeDatas.Count > 1)
            {
                path.AddBeziers(MyTools.ConvertBeziers(NodeDatas, true).ToArray());
            }
        } 
        public override bool IsVisible(PointF point)
        {
            if (Bound.Contains(point))
            {
                Pen p = Pen.Content;
                //线宽过小时不易选中，设置最小宽度为GraphicsTool.WidenWidth
                float minWdith = GraphicsTool.WidenWidth;
                float width = p.Width;
                if (p.Width < minWdith)
                    p.Width = minWdith;
                bool b2 = Path.IsVisible(point);
                p.Width = width;
                return b2;
            }
            return false;
        }
        #endregion

        #region  Serialize
        public override void Serialize(BinaryFormatter bf, Stream s)
        {
            base.Serialize(bf, s);
            const int version = 1;
            bf.Serialize(s, version);
        }
        public override void Deserialize(BinaryFormatter bf, Stream s)
        {
            base.Deserialize(bf, s);
            int version = (int)bf.Deserialize(s);
        }
        #endregion

    }
}
