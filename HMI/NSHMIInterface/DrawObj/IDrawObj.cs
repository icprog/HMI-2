using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using NetSCADA6.NSInterface.HMI.Form;
using NetSCADA6.NSInterface.HMI.Var;

namespace NetSCADA6.NSInterface.HMI.DrawObj
{
    
    /// <summary>
    /// 图形控件，基类接口
    /// </summary>
    public interface IDrawObj:IDisposable
    {
        #region property
        /// <summary>
        /// 控件尺寸
        /// </summary>
        RectangleF Rect { set; get; }
        /// <summary>
        /// 原始控件路径
        /// </summary>
        GraphicsPath BasePath { get; }
        /// <summary>
        /// BasePath自身矩阵转换后的路径。此属性只应用于IDrawVector，放在这里是为了简化代码，不用在BasePath和MatrixPath间写代码切换。
        /// </summary>
        GraphicsPath MatrixPath { get; }
        /// <summary>
        /// BasePath自身矩阵和成组矩阵转换后的路径。此属性只应用于IDrawVector，放在这里是为了简化代码，不用写代码切换
        /// </summary>
        GraphicsPath Path { get; }
        /// <summary>
        /// 控件名称
        /// </summary>
        string Name { set; get; }
        /// <summary>
        /// 控件类型
        /// </summary>
        DrawType Type { get; }
        /// <summary>
        /// 是否是矢量控件
        /// </summary>
        bool IsVector { get; }
        /// <summary>
        /// 成组控件的父控件，未成组情况下为null
        /// </summary>
        IDrawGroup GroupParant { set; get; }
        /// <summary>
        /// 父窗体
        /// </summary>
        IHMIForm Parant { set; get; }
        /// <summary>
        /// 控件路径扩展宽度，一般为Pen.Width/2+3,刷新和判断鼠标点击使用
        /// </summary>
        float WidenWidth { get; }
		/// <summary>
		/// 控件实际边框,和Path属性对应
		/// </summary>
    	RectangleF Bound { get; }
		/// <summary>
		/// 可见
		/// </summary>
		bool Visible { set; get; }
		/// <summary>
		/// 锁定
		/// </summary>
		bool Locked { get; }
		/// <summary>
		/// 最终可见。由Visible和LayerVisible综合判断
		/// </summary>
    	bool FinalVisible { get; }
		/// <summary>
		/// 所在层数
		/// </summary>
		int Layer { set; get; }
        #endregion

        #region function
        //IDrawObj Create(DrawType type);
        /// <summary>
        /// 绘图
        /// </summary>
        /// <param name="g"></param>
        void Draw(Graphics g);

        /// <summary>
        /// 指示指定点是否包含在控件内
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        bool IsVisible(PointF point);

        /// <summary>
        /// 鼠标边框移动
        /// </summary>
        /// <param name="currMouse"></param>
        /// <param name="pos">控制位置</param>
        void MouseFrameMove(PointF currMouse, int pos);
        /// <summary>
        /// 矩形边框移动
        /// </summary>
        /// <param name="xOff"></param>
        /// <param name="yOff"></param>
        /// <param name="wOff"></param>
        /// <param name="hOff"></param>
        void BoundMove(float xOff, float yOff, float wOff, float hOff);
        /// <summary>
        /// 鼠标点下
        /// </summary>
        /// <param name="point"></param>
        void MouseDown(PointF point);
        /// <summary>
        /// 整体移动
        /// </summary>
        /// <param name="point"></param>
        void MouseMove(PointF point);
        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <returns></returns>
        Object Clone();
        /// <summary>
        /// 画面刷新
        /// </summary>
        void Invalidate();
		
		/// <summary>
		/// 能否选中，根据Visible、Locked和Layer综合判断
		/// </summary>
		/// <returns></returns>
    	bool CanSelect();
    	/// <summary>
    	/// 能否选中，根据Visible、Locked和Layer综合判断
    	/// </summary>
    	/// <param name="point"></param>
    	/// <returns></returns>
    	bool CanSelect(PointF point);
		/// <summary>
		/// 能否选中，根据Visible、Locked和Layer综合判断
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		bool CanSelect(RectangleF rect);
        #endregion

        #region event
        /// <summary>
        /// 鼠标事件
        /// </summary>
        /// <param name="e"></param>
        /// <param name="type"></param>
		void LoadMouseEvent(MouseButtons button, PointF location, MouseType type);
        /// <summary>
        /// 初始化
        /// </summary>
        void LoadInitializationEvent();
		/// <summary>
		/// 鼠标移入
		/// </summary>
    	void LoadMouseEnterEvent();
		/// <summary>
		/// 鼠标移出
		/// </summary>
    	void LoadMouseLeaveEvent();
        #endregion

        #region serialize
        void Serialize(BinaryFormatter bf, Stream s);
        void Deserialize(BinaryFormatter bf, Stream s);
        #endregion

        #region var
        /// <summary>
        /// 变量属性名称数组，只包含子类，不包括DrawObj和DrawVector中的变量属性
        /// </summary>
        string[] PropertyNames { get; }
        /// <summary>
        /// 变量改变
        /// </summary>
		/// <param name="expression">变量属性数据</param>
		void LoadDataChangedEvent(IPropertyExpression expression);
		/// <summary>
		/// 生成索引
		/// </summary>
		/// <param name="dict"></param>
    	void GenerateIndex(Dictionary<string, IPropertyIndex> dict);
		/// <summary>
		/// 分析表达式，生成变量列表
		/// </summary>
    	void AnalyseExpression(Dictionary<string, IParameter> dict);
    	#endregion

    }
}
