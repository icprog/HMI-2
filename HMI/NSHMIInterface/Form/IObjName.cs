
namespace NetSCADA6.NSInterface.HMI.Form
{
	/// <summary>
	/// 控件名称管理类,名称比较时忽略大小写
	/// </summary>
	public interface IObjName
	{
		#region function
		/// <summary>
		/// 控件名称是否包含在字典中
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		bool ContainsName(string name);
		/// <summary>
		/// 将控件名称添加到字典中
		/// </summary>
		/// <param name="name"></param>
		void AddName(string name);
		#endregion
	}
}
