using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using NetSCADA6.NSInterface.HMI.DrawObj;

namespace NetSCADA6.HMI.NSDrawObj
{
    /// <summary>
    /// drawvector的绘图计算参数
    /// </summary>
    public interface IDrawData
    {
        RectangleF Rect { set; get; }
        float RotateAngle { set; get; }
        float Shear { set; get; }
        PointF RotatePointPos { get; }
        PointF RotatePoint { set; get; }
        float XScale { set; get; }
        float YScale { set; get; }
        PointF ScalePoint { set; get; }
    	bool IsFlipX { get; }
    	bool IsFlipY { get; }
    }
    /// <summary>
    /// 矢量控件计算时所需要的备份数据
    /// </summary>
    public class CalcData: IDrawData, ICloneable
    {
        #region IDrawData
        public RectangleF Rect { set; get; }
        public float RotateAngle { set; get; }
        public float Shear { set; get; }
        public PointF RotatePointPos { set; get; }
        public PointF RotatePoint { set; get; }
        public float XScale { set; get; }
        public float YScale { set; get; }
        public PointF ScalePoint { set; get; }
    	public bool IsFlipX { set; get; }
    	public bool IsFlipY { set; get; }
        #endregion

        #region 中间数据
        private ControlState _state;
        public ControlState State
        {
            set 
            { 
                _state = value;
                switch (_state)
                {
                    case ControlState.Shear:
                    case ControlState.Center:
                    case ControlState.GroupRotate:
                        FixRate = new PointF(0.5f, 0.5f);
                        break;
                    case ControlState.XScale:
                    case ControlState.YScale:
                        FixRate = ScalePoint;
                        break;
                    case ControlState.BoundMove:
                        FixRate = PointF.Empty;
                        break;
                    default:
                        break;
                }
                Offset = PointF.Empty;
            }
            get { return _state; }
        }
        public PointF FixRate;							//特定点，比例系数
        public PointF Offset;							//偏移值
        public PointF MousePos;							//鼠标值
        public Matrix Matrix { set; get; }
        public RectangleF MatrixBound { set; get; }		//应用控件矩阵后的尺寸
        public RectangleF Bound { set; get; }			//合并成组矩阵后的最终尺寸
        #endregion

        public CalcData()
        {
            Matrix = new Matrix();
        }
        public object Clone()
        {
            CalcData obj = MemberwiseClone() as CalcData;
			obj.Matrix = Matrix.Clone();
            return obj;
		}

		#region dispose
        private bool _disposed;
        public void Dispose()
        {
            DisposeResource();
            
            GC.SuppressFinalize(this);
        }
        private void DisposeResource()
        {
            if (!_disposed)
            {
                Matrix.Dispose();

                _disposed = true;
            }
            
        }
		~CalcData()
		{
			DisposeResource();
		}
		#endregion

	}


    
    
}
