using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NetSCADA6.NSInterface.HMI.DrawObj
{
    /// <summary>
    /// 矢量控件接口
    /// </summary>
	public interface IDrawVector:IDrawObj
    {
        #region property
        /// <summary>
        /// 旋转角度
        /// </summary>
        float RotateAngle { set; get; }
        /// <summary>
        /// 水平倾斜
        /// </summary>
        float Shear { set; get; }
        /// <summary>
        /// 旋转中心点,比例系数
        /// </summary>
        PointF RotatePoint { set; get; }
        /// <summary>
        /// 旋转中心点，实际坐标
        /// </summary>
        PointF RotatePointPos { get; }
        /// <summary>
        /// 缩放中心点，比例系数
        /// </summary>
        PointF ScalePoint { set; get; }
        /// <summary>
        /// X轴缩放比例
        /// </summary>
        float XScale { set; get; }
        /// <summary>
        /// Y轴缩放比例
        /// </summary>
        float YScale { set; get; }
        /// <summary>
        /// 转换矩阵
        /// </summary>
        Matrix Matrix { get; }
		/// <summary>
		/// 编辑模式
		/// </summary>
		EditMode EditMode { set; get; }
		/// <summary>
		/// 可以合并
		/// </summary>
    	bool CanCombine { get; }
		/// <summary>
		/// 水平翻转
		/// </summary>
    	bool IsFlipX { set; get; }
		/// <summary>
		/// 垂直翻转
		/// </summary>
		bool IsFlipY { set; get; }
		#endregion

        #region function
        /// <summary>
        /// 鼠标拖拉旋转中心点
        /// </summary>
        /// <param name="point"></param>
        void MouseRotatePoint(PointF point);
        /// <summary>
        /// 鼠标旋转
        /// </summary>
        /// <param name="point"></param>
        void MouseRotate(PointF point, bool isOrtho);
        /// <summary>
        /// 鼠标倾斜
        /// </summary>
        /// <param name="point"></param>
        /// <param name="pos"></param>
		void MouseShear(PointF point, int pos, bool isOrtho);
        /// <summary>
        /// 成组控件旋转
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="center"></param>
        void GroupRotate(float angle, PointF center);
        #endregion

    }
}
