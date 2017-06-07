using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using NetSCADA6.NSInterface.HMI.DrawObj;
using NetSCADA6.NSInterface.HMI.Var;

namespace NetSCADA6.HMI.NSDrawObj.Var
{
    /// <summary>
    /// 属性变量管理
    /// </summary>
    public class ExpressionMananger:ICloneable
    {
        public ExpressionMananger(IDrawObj container)
        {
            _container = container;

            InitList();
        }

        #region field
        private IDrawObj _container;
        #endregion

        #region property
        private List<IPropertyExpression> _list;
        public List<IPropertyExpression> List
        {
            get { return _list; }
        }
        #endregion

		#region private function
		private void InitList()
		{
			_list = new List<IPropertyExpression>();

			//drawobj
			string[] names = DrawObj.GetPropertyNames();
			int count = names.Length;
			for (int i = 0; i < count; i++)
				AddParameter(names[i]);

			//drawvector
			if (_container.IsVector)
			{
				names = DrawVector.GetPropertyNames();
				count = names.Length;
				for (int i = 0; i < count; i++)
					AddParameter(names[i]);
			}

			//draw child
			if (_container.PropertyNames != null)
			{
				count = _container.PropertyNames.Length;
				for (int i = 0; i < count; i++)
					AddParameter(_container.PropertyNames[i]);
			}
		}
		private void AddParameter(string name)
		{
			_list.Add(new PropertyExpression(name, _container));
		}
		#endregion

		#region public function
        public void GenerateIndex(Dictionary<string, IPropertyIndex> dict)
        {
        	int count = _list.Count;
            for (int i = 0; i < count; i++)
            {
                string name = ((PropertyExpression)_list[i]).PropertyName;
                
                if (!string.IsNullOrWhiteSpace(name))
                {
                	IPropertyIndex value = null;
                	dict.TryGetValue(name, out value);
                    _list[i].Index = value;

					#if DEBUG
                    if (value == null)
                        throw new Exception("ParameterManager.GenerateIndex");
					#endif
                }
            }
        }
        #endregion

		#region clone
		public object Clone()
		{
			ExpressionMananger obj = (ExpressionMananger)MemberwiseClone();
			int count = _list.Count;
			obj._list = new List<IPropertyExpression>(count);
			for (int i = 0 ; i < count; i++)
			{
				ICloneable c = (ICloneable)_list[i];
				obj._list.Add((IPropertyExpression)(c).Clone());
			}

			return obj;
		}
		#endregion

		#region serialize
		private void SetExpression(string name, string expression)
		{
			int count = _list.Count;
			for (int i = 0; i < count; i++)
			{
				if (string.Compare(name, _list[i].PropertyName, true) == 0)
					_list[i].Expression = expression;
			}
		}
		public void Serialize(BinaryFormatter bf, Stream s)
		{
			const int version = 1;

			bf.Serialize(s, version);

			int count = _list.Count(t => !string.IsNullOrWhiteSpace(t.Expression));
			bf.Serialize(s, count);
			foreach (IPropertyExpression t in _list)
			{
				if (string.IsNullOrWhiteSpace(t.Expression))
					continue;
				bf.Serialize(s, t.PropertyName);
				bf.Serialize(s, t.Expression);
			}

		}
		public void Deserialize(BinaryFormatter bf, Stream s)
		{
			int version = (int)bf.Deserialize(s);
			int count = (int) bf.Deserialize(s);

			for (int i = 0; i < count; i++)
			{
				string name = (string) bf.Deserialize(s);
				string expression = (string) bf.Deserialize(s);
				SetExpression(name, expression);
			}
		}
		#endregion



	}
}
