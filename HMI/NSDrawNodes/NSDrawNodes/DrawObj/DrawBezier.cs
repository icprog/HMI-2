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


namespace NetSCADA6.HMI.NSDrawNodes
{
    public class DrawBezier : DrawNodes
    {
        public DrawBezier()
        {
        }
        #region nodemanger
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
            List<PointF> bzr = MyTools.ConvertBeziers(datas, false);

            for (index = 1; index < count; index++)
            {
                path.Reset();
                PointF[] ptfs = MyTools.GetBzr(bzr, index);
                if (ptfs != null)
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
                List<PointF> dis = MyTools.ConvertBeziers(NodeDatas, false);
                PointF[] ary = dis.ToArray();
                g.DrawBeziers(p, ary);
                p.Dispose();
            }
        }
        #endregion

        public override DrawType Type { get { return DrawType.Bezier; } }

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
            if (NodeDatas.Count > 1)
            {
                path.AddBeziers(MyTools.ConvertBeziers(NodeDatas, false).ToArray());
            }
        }

        protected override void OnInitialization()
        {
            base.OnInitialization();
            BrushHasValid = false;
        } 
    }

}
