using System;
using System.Collections.Generic;
using NetSCADA6.NSInterface.HMI.DrawObj;
using NetSCADA6.NSInterface.HMI.Var;

namespace NetSCADA6.HMI.NSDrawObj.Var
{
    /// <summary>
    /// 属性的变量绑定
    /// </summary>
    public class PropertyExpression : IPropertyExpression, ICloneable
    {
        public PropertyExpression(string name, IDrawObj container)
        {
            _propertyName = name;
            _container = container;
        }

        #region property
        private readonly string _propertyName;
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }
        private readonly IDrawObj _container;
        /// <summary>
        /// 所属控件
        /// </summary>
        public IDrawObj Container
        {
            get { return _container; }
        }
		private List<IParameter> _parameterList = new List<IParameter>();
		public List<IParameter> ParameterList { get { return _parameterList; } }
        public IPropertyIndex Index { set; get; }
        public string Expression { set; get; }
        public double DecimalValue { set; get; }
        public string StringValue { set; get; }
        #endregion

		#region private function
		private void CalculateExpression()
		{
			#warning 表达式计算
			DecimalValue = _parameterList[0].DecimalValue;
			StringValue = _parameterList[0].StringValue;
		}
		#endregion

		#region public function
		public void OnDataChanged()
		{
			CalculateExpression();
			_container.LoadDataChangedEvent(this);
        }
        #endregion

		#region clone
		public object Clone()
		{
			return MemberwiseClone();
		}
		#endregion
	}
}
