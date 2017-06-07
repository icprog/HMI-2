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
	[Ortho(OrthoMode.HoriOrVert)]
	public class DrawPolygon : DrawNodes
    {
        public DrawPolygon()
        {
        }

        public override DrawType Type { get { return DrawType.Polygon; } }

        /// <summary>
        /// 点point是否和datas数据构成的线段相交
        /// </summary> 
        public override bool FindIndex(List<PointF> datas, PointF point, ref int index)
        {

            int count = datas.Count;
            const int width = 6;
            bool isVisible = false;
            GraphicsPath path = new GraphicsPath();
            Pen p = new Pen(Color.Black, width);

            for (index = 1; index < count; index++)
            {
                path.Reset();
                path.AddLine(datas[index - 1], datas[index]);
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

        /// <summary>
        /// 鼠标创建控件时绘制图形
        /// </summary>
        /// <param name="g"></param>
        public override void CreatingPaint(Graphics g)
        {
            List<PointF> ds = NodeDatas;
            if (ds != null && ds.Count > 1)
            {
                Brush br = BrushData.CreateBrush(Rect, Path);
                g.FillPolygon(br, ds.ToArray());
                g.DrawPolygon(Pens.Black, ds.ToArray());
            }
        }

        /*   /// <summary>
           /// 更新绘图路径
           /// </summary>
           protected override void UpdateDisPath()
           {
               if (NodeDatas.Count > 1)
               {
                   disGraphicsPath.Reset();
                   disGraphicsPath.AddLines(NodeDatas.ToArray());
                   disGraphicsPath.CloseAllFigures();
               } 
           }*/


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

        protected override void GenerateNodePath(ref GraphicsPath path)
        {
            if (NodeDatas.Count > 2)
            {
                path.Reset();
                path.AddPolygon(NodeDatas.ToArray());
            }
            else if (NodeDatas.Count > 1)
            {
                path.Reset();
                path.AddLines(NodeDatas.ToArray());
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
                bool b1 = Path.IsOutlineVisible(point, p);
                bool b2 = Path.IsVisible(point);
                p.Width = width;
                return b1 || b2;
            }
            return false;
        }
    }
}
