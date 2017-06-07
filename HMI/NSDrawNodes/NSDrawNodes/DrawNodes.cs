using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using NetSCADA6.HMI.NSDrawObj;
using NetSCADA6.NSInterface.HMI.DrawObj;
using NetSCADA6.Common.NSColorManger;
using NetSCADA6.NSInterface.HMI.DrawObj;

namespace NetSCADA6.HMI.NSDrawNodes
{
    /// <summary>
    /// 抽象的该类无法被创建用于派生实际的类 节点控件类 派生于矢量基类
    /// </summary>
    public abstract partial class DrawNodes : DrawVector, ICloneable, INodeEdit
    {
        public DrawNodes()
        {
            Stream = new StreamControl(this);
        }

        #region public property

        /// <summary>
        /// 类型
        /// </summary>
        public override DrawType Type { get { return DrawType.StraightLine; } }
        /// <summary>
        /// 画刷是否有效
        /// </summary>
        public bool BrushHasValid = true;

        /// <summary>
        /// 数据点
        /// </summary>
        public List<PointF> NodeDatas
        {
            get { return _nodeDatas; }
        }
        private List<PointF> _nodeDatas = new List<PointF>();

        /// <summary>
        /// 线条流动模块
        /// </summary>
        public StreamControl Stream { get; set; }

        #endregion

        #region protected function

        /// <summary>
        /// 输出路径提供
        /// </summary>
        /// <param name="path"></param>
        protected override void OnGeneratePath(ref GraphicsPath path)
        {
            if (false == IsNodeOperate)//不是鼠标操作
            {
                ScaleDatas(Rect, DataBk.Rect, NodeDatas, _nodeDatasBk);
            }
            GenerateNodePath(ref path);
            SetDirectRect(BasePath.GetBounds());
        }

        /// <summary>
        /// 输出路径提供（子类使用这个）
        /// </summary>
        /// <param name="path"></param>
        protected virtual void GenerateNodePath(ref GraphicsPath path)
        {
            if (NodeDatas.Count > 0)
                path.AddLines(NodeDatas.ToArray());
        }

        /// <summary>
        /// 显示使用的画笔
        /// </summary>
        private Pen disPen;
        protected virtual void DrawPath(Graphics g)
        {
            if (BasePath.PointCount > 1 && PenData != null)
            {
                if (PenData.IsPiple)
                {
                    PipleData pipleData = PenData.PipleData;
                    float tempWidthInterval = pipleData.Width / MyTools.accuracy;
                    float RInterval = (pipleData.HighlightColor.R - pipleData.BaseColor.R) / MyTools.accuracy;
                    float GInterval = (pipleData.HighlightColor.G - pipleData.BaseColor.G) / MyTools.accuracy;
                    float BInterval = (pipleData.HighlightColor.B - pipleData.BaseColor.B) / MyTools.accuracy;
                    Pen p = new Pen(pipleData.BaseColor, pipleData.Width);
                    p.StartCap = pipleData.StartCap;
                    p.EndCap = pipleData.EndCap;
                    p.LineJoin = pipleData.LineJoin;
                    p.Color = Color.FromArgb(pipleData.Alpha, pipleData.BaseColor);
                    for (int i = 0; i < MyTools.accuracy; i++)
                    {
                        g.DrawPath(p, BasePath);
                        p.Width -= tempWidthInterval;
                        p.Color = Color.FromArgb(pipleData.Alpha, p.Color.R + (int)RInterval, p.Color.G + (int)GInterval, p.Color.B + (int)BInterval);
                    }
                    p.Dispose();
                }
                else
                {
                    UpdateDisPen(Stream.Enable);
                    if (disPen != null)
                    {
                        //g.DrawPath(disPen, disGraphicsPath);
                        g.DrawPath(disPen, BasePath);
                    }
                }
            }
        }
        protected virtual void FillPath(Graphics g)
        {
            if (BasePath.PointCount > 1 && BrushHasValid)
            {
                Brush br = BrushData.CreateBrush(Rect, Path);
                if (br != null)
                {
                    g.FillPath(br, BasePath);
                    br.Dispose();
                }
            }
        }
        protected override void OnPaint(Graphics g)
        {
            GraphicsState state = g.Save();
            g.SmoothingMode = SmoothingMode.HighQuality;
            FillPath(g);
            DrawPath(g);
            g.Restore(state);
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

                bool isVisible = Path.IsOutlineVisible(point, p);
                p.Width = width;
                return isVisible;
            }
            return false;
        }

        protected virtual void OnNodeChanged()
        {
            //  UpdateDisPath();
        }

        /// <summary>
        /// 最终初始化函数
        /// </summary>
        protected override void OnInitialization()
        {
            base.OnInitialization();
            UpdateDisPen(Stream.Enable);
            //UpdateDisPath();
        }

        #endregion

        #region Stream
        /// <summary>
        /// 启动前调用
        /// </summary>
        public void FirstTimerTick()
        {
            UpdateDisPen(true);
            Invalidate();
        }
        /// <summary>
        /// 结束前调用
        /// </summary>
        public void EndTimerTick()
        {
            UpdateDisPen(false);
            Invalidate();
        }
        /// <summary>
        /// 更新流动画笔
        /// </summary>
        private void UpdateDisPen(bool IsStream)
        {
            if (disPen == null)
            {
                disPen = PenData.CreatePen(Rect, Path);
            }
            else if (disPen.Width != PenData.Width)
            {
                disPen.Dispose();
                disPen = PenData.CreatePen(Rect, Path);
            }

            if (IsStream)
            {
                if (disPen != null && disPen.DashStyle != DashStyle.Custom)
                {
                    float[] dashValues = { 1, 1 };
                    disPen.DashPattern = dashValues;
                }
            }
        }
        /// <summary>
        /// 流动虚线偏移。
        /// </summary>
        /// <param name="offset"></param>
        public void SetPenDashOffset(float offset)
        {
            if (Stream.Enable)
            {
                if (disPen != null)
                {
                    if (disPen.DashStyle == DashStyle.Custom)
                    {
                        disPen.DashOffset = offset;
                    }
                }
            }
        }
        #endregion

        #region serialize
        public override void Serialize(BinaryFormatter bf, Stream s)
        {
            base.Serialize(bf, s);
            const int version = 2;
            bf.Serialize(s, version);
            bf.Serialize(s, NodeDatas.Count);
            for (int i = 0; i < NodeDatas.Count; i++)
            {
                bf.Serialize(s, NodeDatas[i]);
            }
            if (Stream != null)
                Stream.Serialize(bf, s);
        }
        public override void Deserialize(BinaryFormatter bf, Stream s)
        {
            base.Deserialize(bf, s);
            int version = (int)bf.Deserialize(s);
            int Count = (int)bf.Deserialize(s);
            for (int i = 0; i < Count; i++)
            {
                PointF pf = (PointF)bf.Deserialize(s);
                NodeDatas.Add(pf);
            }
            if (version >= 2)
            {
                if (Stream != null)
                    Stream.Deserialize(bf, s);
            }
        }
        #endregion

		private readonly List<PointF> _nodeDatasBk = new List<PointF>();
		protected override void BackupData()
		{
			base.BackupData();
			_nodeDatasBk.Clear();
			_nodeDatasBk.AddRange(_nodeDatas);
		}
    }
}
