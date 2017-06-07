using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; 
using NetSCADA6.NSInterface.HMI.DrawObj;
using System;
using NetSCADA6.Common.NSColorManger;
using NetSCADA6.HMI.NSDrawObj;

namespace NetSCADA6.HMI.NSDrawNodes
{
    /// <summary>
    /// 绘制节点图形
    /// </summary>
    public abstract partial class DrawNodes : DrawVector, ICloneable, INodeEdit
    {

        public virtual bool CreateFinish
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 节点操作类
        /// </summary>  
        #region property2
        private bool _isEdit;
        /// <summary>
        /// 是否进入节点编辑模式
        /// </summary>
        public bool IsEdit
        {
            set
            {
                if (value)
                    NodeState = NodesState.Move;
                _isEdit = value;
            }
            get { return _isEdit; }
        }
        /// <summary>
        /// 编辑状态
        /// </summary>
        public NodesState NodeState { set; get; }
        private bool _isNodeOperate;
        /// <summary>
        /// 是否是使用鼠标编辑节点
        /// </summary>
        public bool IsNodeOperate
        {
            get { return _isNodeOperate; }
        }
        /// <summary>
        /// 是否是在用鼠标创建
        /// </summary>
        public bool IsNodeCreating { get; set; }
        /// <summary>
        /// 节点控件是否创建成功
        /// </summary>
        public  bool CreateSuccess
        {
            get
            {
                if (NodeDatas != null)
                    return (NodeDatas.Count >= MinCount);
                return false;
            }
        } 

        public virtual int MinCount
        {
            get { return 2; }
        }
        #endregion
        #region public function
        public void MoveNode(PointF point, int pos)
        {
            PointF p = Calculation.GetInvertPos(DataBk.Matrix, point);
            PointF mouse = Calculation.GetInvertPos(DataBk.Matrix, DataBk.MousePos);
            PointF off = new PointF(p.X - mouse.X, p.Y - mouse.Y);

            _isNodeOperate = true;
            OnMove(p, off, pos);
            LoadGeneratePathEvent();
            _isNodeOperate = false;
        }
        public void AddNode(PointF point)
        {
            BackupData();

            PointF p = Calculation.GetInvertPos(DataBk.Matrix, point);

            _isNodeOperate = true;
            OnAdd(p);
            LoadGeneratePathEvent();
            _isNodeOperate = false;
        }
        public void DeleteNode(int pos)
        {

            BackupData();

            _isNodeOperate = true;
            OnDelete(pos);
            LoadGeneratePathEvent();
            _isNodeOperate = false;
        }
        public PointF CalculateOffset(List<PointF> current, List<PointF> backup)
        {
            if (current.Count < 2)
                return PointF.Empty;


            //Rect可能发生变化，需要计算偏移
            GraphicsPath path = new GraphicsPath();
            path.AddLines(current.ToArray());
            RectangleF rf = path.GetBounds();
            path.Dispose();

            NewRect = rf;

            PointF p1 = Calculation.CalcMatrixPoint(DataBk, PointF.Empty);
            PointF p2 = Calculation.CalcMatrixPoint(this, PointF.Empty);

            return new PointF(p1.X - p2.X, p1.Y - p2.Y);
        }
        /// <summary>
        /// 鼠标创建控件时绘制图形
        /// </summary>
        /// <param name="g"></param>
        public virtual void CreatingPaint(Graphics g)
        {
            List<PointF> ds = NodeDatas;

            if (ds != null && ds.Count > 1)
            {
                //完成线绘制黑线，最后一段线红线
                int count = ds.Count;
                for (int i = 0; i < count - 2; i++)
                {
                    g.DrawLine(Pens.Black, ds[i], ds[i + 1]);
                }
                g.DrawLine(Pens.Red, ds[count - 2], ds[count - 1]);
            }
        }
        /// <summary>
        /// 平移数据列表
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="datasBk"></param>
        /// <param name="offset"></param>
        public void OffsetDatas(List<PointF> datas, List<PointF> datasBk, PointF offset)
        {
            if (IsNodeCreating)
                return;

            SizeF sf = new SizeF(offset);
            int count = datas.Count;

            if (datasBk == null)
            {
                for (int i = 0; i < count; i++)
                    datas[i] = PointF.Add(datas[i], sf);
            }
            else
            {
                for (int i = 0; i < count; i++)
                    datas[i] = PointF.Add(datasBk[i], sf);
            }
        }
        /// <summary>
        /// 平移缩放数据列表
        /// </summary>
        /// <param name="current"></param>
        /// <param name="backup"></param>
        /// <param name="datas"></param>
        /// <param name="datasBk"></param>
        public void ScaleDatas(RectangleF current, RectangleF backup, List<PointF> datas, List<PointF> datasBk)
        {
            if (backup == RectangleF.Empty || datasBk.Count == 0 || IsNodeCreating)
                return;

            float xOff = current.X - backup.X;
            float yOff = current.Y - backup.Y;
            float xScale = current.Width / backup.Width;
            float yScale = current.Height / backup.Height;

            PointF pf = PointF.Empty;
            for (int i = 0; i < datas.Count; i++)
            {
                pf.X = backup.X + (datasBk[i].X - backup.X) * xScale + xOff;
                pf.Y = backup.Y + (datasBk[i].Y - backup.Y) * yScale + yOff;
                datas[i] = pf;
            }

            OnNodeChanged();
        }
        /// <summary>
        /// 点point是否和datas数据构成的线段相交
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="point"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual bool FindIndex(List<PointF> datas, PointF point, ref int index)
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
        #endregion
        #region event
        protected virtual void OnMove(PointF point, PointF offset, int pos)
        {
            List<PointF> datas = NodeDatas;
            List<PointF> datasBk = _nodeDatasBk;

            if (IsNodeCreating)
            {
                datas[pos] = point;
                OnNodeChanged();
                return;
            }

            SizeF sf = new SizeF(offset);

            List<PointF> list = new List<PointF>();
            list.AddRange(datasBk);
            list[pos] = PointF.Add(datasBk[pos], sf);

            OffsetDatas(datas, datasBk, CalculateOffset(list, datasBk));

            datas[pos] = PointF.Add(datas[pos], sf);

            OnNodeChanged();
        }
        protected virtual void OnAdd(PointF point)
        {
            List<PointF> datas = NodeDatas;
            List<PointF> datasBk = _nodeDatasBk;

            //鼠标创建时不需要做其他操作
            if (IsNodeCreating)
            {
                datas.Add(point);
                OnNodeChanged();
                 
                return;
            }
            int index = 0;
            if (FindIndex(datas, point, ref index))
                datas.Insert(index, point);
            else
                datas.Add(point); 
            OffsetDatas(datas, null, CalculateOffset(datas, datasBk));

            OnNodeChanged();
        }
        protected virtual void OnDelete(int pos)
        {
            List<PointF> datas = NodeDatas;
            List<PointF> datasBk = _nodeDatasBk;

            if (datas.Count < (MinCount + 1))
                return;

            datas.RemoveAt(pos);

            OffsetDatas(datas, null, CalculateOffset(datas, datasBk));

            OnNodeChanged();
        }
        #endregion
    }
}
