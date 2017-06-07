using NetSCADA6.NSInterface.HMI.Var;

namespace NetSCADA6.HMI.NSDrawObj.Var
{
    /// <summary>
    /// 属性索引
    /// </summary>
    public class PropertyIndex : IPropertyIndex
    {
        public PropertyIndex(int classType, int index)
        {
            _classType = classType;
            _index = index;
        }

        private readonly int _classType;
        public int ClassType
        {
            get { return _classType; }
        }
        private readonly int _index;
        public int Index
        {
            get { return _index; }
        }

    }
}
