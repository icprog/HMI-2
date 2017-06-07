using System;
using System.Collections.Generic;
using NetSCADA6.HMI.NSDrawObj;
using NetSCADA6.NSInterface.HMI.DrawObj;
using NetSCADA6.NSInterface.HMI.Form;

namespace NetSCADA6.HMI.NSHMIForm
{
    /// <summary>
    /// 用于生成控件名称
    /// </summary>
    internal class ObjName : IObjName
    {
        public ObjName()
        {
            _names = Enum.GetNames(typeof(DrawType));
            int len = Enum.GetValues(typeof(DrawType)).Length;
            _indexs = new int[len];
        }

		#region field
		private readonly Dictionary<string, object> _nameDict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
		private readonly int[] _indexs;
		private readonly string[] _names;
		#endregion

		#region private function
		private void AddName(IDrawObj obj)
		{
			if (!string.IsNullOrWhiteSpace(obj.Name))
				_nameDict.Add(obj.Name, null);
			if (obj.Type == DrawType.Group)
			{
				DrawGroup group = (DrawGroup)obj;
				foreach (IDrawObj o in group.ObjList)
					AddName(o);
			}
		}
		#endregion

        #region public function
		public void ResetDict(List<IDrawObj> objList)
		{
			_nameDict.Clear();

			foreach (IDrawObj obj in objList)
				AddName(obj);
			for (int i = 0; i < _indexs.Length; i++)
				_indexs[i] = 0;
		}
		public void AddName(string name)
		{
			if (!_nameDict.ContainsKey(name))
				_nameDict.Add(name, null);
		}
		public bool ContainsName(string name)
		{
			return _nameDict.ContainsKey(name);
		}
		public void CreateName(IDrawObj obj)
		{
			if (string.IsNullOrWhiteSpace(obj.Name) || _nameDict.ContainsKey(obj.Name))
			{
				int type = (int)obj.Type;
				int index = _indexs[type] + 1;
				while (_nameDict.ContainsKey(_names[type] + index))
				{
					index++;
				}
				_indexs[type] = index;
				obj.Name = _names[type] + index;
			}
		}
		public void RemoveName(IDrawObj obj)
		{
			_nameDict.Remove(obj.Name);
			if (obj.Type == DrawType.Group)
			{
				DrawGroup group = (DrawGroup)obj;
				foreach (IDrawObj o in group.ObjList)
					RemoveName(o);
			}
		}
		/// <summary>
		/// 改变名称，用于控件粘贴
		/// </summary>
		/// <param name="obj"></param>
		public void ChangeName(IDrawObj obj)
		{
			CreateName(obj);
			//drawgoup
			if (obj.Type == DrawType.Group)
			{
				DrawGroup group = (DrawGroup)obj;
				foreach (IDrawObj o in group.ObjList)
					ChangeName(o);
			}
		}
        #endregion

    }
}
