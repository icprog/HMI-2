
namespace NetSCADA6.NSInterface.HMI.DrawObj
{
    /// <summary>
    /// 控件类型
    /// </summary>
    public enum DrawType
    {
        Obj,			//基类
		Vector,			//矢量基类
		Group,			//组
		Combine,		//合并
		Rect,           //矩形
        Ellipse,        //椭圆
        Text,           //文本
		StraightLine,	//直线
		FoldLine,		//折线
		Bezier,			//曲线
		Polygon,		//多边形
		ClosedBezier,	//曲线多边形
    }

    /// <summary>
    /// 控件活动状态
    /// </summary>
    public enum ControlState
    {
        None,			//无
        Move,			//平移
        XScale,			//X轴缩放
        YScale,			//Y轴缩放
        Shear,			//倾斜
        Center,			//旋转中心点
        Rotate,			//旋转
        FrameMove,		//边框移动
        BoundMove,		//矩形边框移动
        GroupRotate,	//成组旋转
        Custom,			//自定义点
		MoveNode,		//移动节点
		AddNode,		//增加节点
		DeleteNode,		//删除节点
		Segment,		//路径切割
		FormWidth,		//窗体宽度
		FormHeight		//窗体高度
    }
    /// <summary>
    /// 鼠标事件类型
    /// </summary>
    public enum MouseType { Down, Up, Move }
	/// <summary>
	/// 节点编辑状态
	/// </summary>
	public enum NodesState{ Move, Add, Delete }

	/// <summary>
	/// 编辑模式
	/// </summary>
	public enum EditMode
	{
		Normal,				//常用
		Node,				//节点
		Segment				//切割
	}
	/// <summary>
	/// 正交编辑模式
	/// </summary>
	public enum OrthoMode
	{
		Square,				//正方形
		HoriOrVert,			//水平或垂直
		Invalid				//无效
	}



}
