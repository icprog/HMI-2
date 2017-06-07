using System;
using System.Collections.Generic;
using System.Text;

namespace NetSCADA6.Common.NSColorManger
{ 
    /// <summary>
    /// 控件的状态。
    /// </summary>
    internal enum ControlState
    {
        /// <summary>
        ///  正常。
        /// </summary>
        Normal,
        /// <summary>
        /// 鼠标进入。
        /// </summary>
        Hover,
        /// <summary>
        /// 鼠标按下。
        /// </summary>
        Pressed,
        /// <summary>
        /// 获得焦点。
        /// </summary>
        Focused,
    }
}