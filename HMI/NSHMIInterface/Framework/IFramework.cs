using System;

namespace NetSCADA6.NSInterface.HMI.Framework
{
    /// <summary>
    /// 图形模块框架接口
    /// </summary>
	public interface IFramework : IDisposable
	{
		#region property
		/// <summary>
		/// 是否是编辑模式
		/// </summary>
    	bool IsStudioMode { get; }
		/// <summary>
		/// 框架管理类，只用于编辑版
		/// </summary>
		IFrameworkManager Manager { get; }
		#endregion

    }
}
