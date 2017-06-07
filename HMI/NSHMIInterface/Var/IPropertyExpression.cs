using System.Collections.Generic;

namespace NetSCADA6.NSInterface.HMI.Var
{
    /// <summary>
    /// 变量接口
    /// </summary>
    public interface IParameter
    {
        string Name { get; }
        List<IPropertyExpression> List { get; }
        double DecimalValue { set; get; }
        string StringValue { set; get; }
    }
    /// <summary>
    /// 变量属性索引
    /// </summary>
    public interface IPropertyIndex
    {
        int ClassType { get; }
        int Index { get; }
    }
    public interface IPropertyExpression
    {
    	/// <summary>
    	/// 属性名称
    	/// </summary>
    	string PropertyName { get; }
		/// <summary>
		/// 表达式
		/// </summary>
		string Expression { set; get; }
		/// <summary>
		/// 属性索引，仅在运行版中应用
		/// </summary>
        IPropertyIndex Index { set; get; }
		/// <summary>
		/// 表达式中的变量列表
		/// </summary>
		List<IParameter> ParameterList { get; }
		double DecimalValue { set; get; }
		string StringValue { set; get; }

		void OnDataChanged();
    }
}
