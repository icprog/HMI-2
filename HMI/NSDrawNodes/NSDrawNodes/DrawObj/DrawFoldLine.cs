// -----------------------------------------------------------------------
// <copyright file="DrawFoldLine.cs" company="">
// </copyright>
// -----------------------------------------------------------------------

using NetSCADA6.HMI.NSDrawObj;

namespace NetSCADA6.HMI.NSDrawNodes.DrawObj
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NetSCADA6.NSInterface.HMI.DrawObj;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

	[Ortho(OrthoMode.HoriOrVert)]
	public class DrawFoldLine : DrawNodes
    {
        public DrawFoldLine()
        {
        }
        public override DrawType Type { get { return DrawType.FoldLine; } }


        /// <summary>
        /// 鼠标创建控件时绘制图形
        /// </summary>
        /// <param name="g"></param>
        public override void CreatingPaint(Graphics g)
        {
            List<PointF> ds = /*CALCTool.CopyListPoint*/(NodeDatas);
            if (ds != null && ds.Count > 1)
            {
                Pen p = PenData.CreatePen(Rect, Path);
                g.DrawLines(p, ds.ToArray());
                p.Dispose();
            }
        }

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

        protected override void OnInitialization()
        {
            base.OnInitialization();
            BrushHasValid = false;
        } 

        protected override void GenerateNodePath(ref GraphicsPath path)
        {
            if (NodeDatas.Count > 0)
            {
                path.AddLines(NodeDatas.ToArray());
            }
        }

        public override void Serialize(BinaryFormatter bf, Stream s)
        {

            base.Serialize(bf, s);
            const int version = 2;
            bf.Serialize(s, version);
        }
        public override void Deserialize(BinaryFormatter bf, Stream s)
        {
            base.Deserialize(bf, s);
            int version = (int)bf.Deserialize(s);
        }
    }
}
