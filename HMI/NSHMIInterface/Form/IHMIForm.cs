using System.Drawing.Design;

using NetSCADA6.NSInterface.HMI.DrawObj;
using NetSCADA6.NSInterface.HMI.Framework;

namespace NetSCADA6.NSInterface.HMI.Form
{
    /// <summary>
    /// 图形窗体接口
    /// </summary>
	public interface IHMIForm
    {
        #region property
		/// <summary>
		/// 公共环境
		/// </summary>
    	IEnvironment Common { get; }
		/// <summary>
		/// 编辑环境
		/// </summary>
    	IStudio Studio { get; }
		/// <summary>
		/// 运行环境
		/// </summary>
    	IRun Run { get; }
		/// <summary>
		/// 窗体全路径名称
		/// </summary>
		string FullName { set; get; }
		/// <summary>
		/// 图形框架
		/// </summary>
    	IFramework Framework { get; }
        #endregion

        #region function
		void Save();
		/// <summary>
		/// 显示调试信息
		/// </summary>
		/// <param name="message"></param>
    	void ShowDebugMessage(string message);

    	#endregion
    }
}
