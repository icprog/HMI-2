using System.Collections.Generic;
using NetSCADA6.NSInterface.HMI.Var;

namespace NetSCADA6.HMI.NSDrawObj.Var
{
    public class Parameter : IParameter
    {
        public Parameter(string name)
        {
            Name = name;
            List = new List<IPropertyExpression>();
        }
        
        public string Name { set; get; }
        public List<IPropertyExpression> List { set; get; }
        public double DecimalValue { set; get; }
        public string StringValue { set; get; }
    }
}
