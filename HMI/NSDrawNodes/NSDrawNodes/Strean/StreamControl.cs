using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using NetSCADA6.HMI.NSDrawObj;
using NetSCADA6.NSInterface.HMI.DrawObj;

namespace NetSCADA6.HMI.NSDrawNodes
{
    /// <summary>
    /// 线条流动
    /// </summary> 
    [EditorAttribute(typeof(StreamEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(StreamConverter))]
    [DescriptionAttribute("展开以查看获取或设置流动样式")]
    [DisplayName("流动")]
    public class StreamControl : ICloneable
    {
        public StreamControl(DrawNodes content)
        {
            _content = content;
            _timer.Tick += Timer_Tick;
        }
        private DrawNodes _content;
        private Timer _timer = new Timer();

        #region property
        /// <summary>
        /// 控制流动启停
        /// </summary>
        [DisplayName("启停")]
        public bool Enable
        {
            set
            {
                if (_timer != null)
                {
                    if (_enable == false && value == true)
                    {
                        FirstTimerTick();
                    }
                    if (_enable == true && value == false)
                    {
                        EndTimerTick();
                    }
                    _timer.Enabled = value;
                }
                _enable = value;
            }
            get
            {
                return _enable;
            }
        }
        bool _enable = false;

        /// <summary>
        /// 流动速度
        /// </summary>
        [DisplayName("流动速度")]
        public int Interval
        {
            set;
            get;
        }

        /// <summary>
        /// 是否正方向开始流动
        /// </summary>
        [DisplayName("流动方向")]
        public bool IsForward
        {
            set { _isForward = value; }
            get { return _isForward; }
        }
        bool _isForward = true;

        /// <summary>
        /// 流动时每步移动的百分比。
        /// </summary>
        [DisplayName("步长")]
        public int StepLength
        {
            set { if (value > 0 && value < 100)_stepLength = value / 100f; }
            get { return (int)(_stepLength * 100); }
        }
        float _stepLength = 0.3f;

        #endregion

        /// <summary>
        /// 启动前调用
        /// </summary>
        private void FirstTimerTick()
        {
            _timer.Interval = Interval; //流速
            _content.FirstTimerTick();
        }
        /// <summary>
        /// 结束前调用
        /// </summary>
        private void EndTimerTick()
        {
            _dashOffset = 0;
            _content.EndTimerTick();
        }

        #region private function
        private void Timer_Tick(object sender, EventArgs e)
        {
            CalculateDashOffset(); //计算线形偏移量。

           // _content.Invalidate();
        }
        /// <summary>
        /// 计算线形偏移量。
        /// </summary>
        private void CalculateDashOffset()
        {
            _dashOffset += _stepLength;
            //流向
            if (_content != null)
            {
                if (IsForward)
                    _content.SetPenDashOffset(_dashOffset);
                else
                    _content.SetPenDashOffset(-_dashOffset);
                _dashOffset %= 2;
            }
        }
        private float _dashOffset = 0f;
        #endregion

        #region serialize clone
        public object Clone()
        {
            StreamControl other = new StreamControl(_content);
            other.IsForward = this.IsForward;
            other._stepLength = this._stepLength;
            other.Interval = this.Interval;
            other.Enable = this.Enable;
            this.Enable = false;
            return other;
        }
        public void Serialize(BinaryFormatter bf, Stream s)
        {
            const int version = 1;
            bf.Serialize(s, version);
            bf.Serialize(s, IsForward);
            bf.Serialize(s, _stepLength);
            bf.Serialize(s, Interval);
            bf.Serialize(s, Enable);
        }
        public void Deserialize(BinaryFormatter bf, Stream s)
        {
            int version = (int)bf.Deserialize(s);
            IsForward = (bool)bf.Deserialize(s);
            _stepLength = (float)bf.Deserialize(s);
            Interval = (int)bf.Deserialize(s);
            Enable = (bool)bf.Deserialize(s);
        }
        #endregion
    }
}
