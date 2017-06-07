using System;
using System.Collections.Generic;
using System.Drawing;
using NetSCADA6.HMI.NSDrawObj;
using NetSCADA6.HMI.NSDrawObj.Var;
using NetSCADA6.HMI.NSDrawVector;
using NetSCADA6.NSInterface.HMI.DrawObj;
using NetSCADA6.NSInterface.HMI.Form;
using NetSCADA6.NSInterface.HMI.Var;

namespace NetSCADA6.HMI.NSHMIForm
{
    /// <summary>
    /// 运行版环境
    /// </summary>
	internal class Run : Environment, IRun
    {
        public Run(HMIForm control):base(control)
        {
        }
        static Run()
        {
            InitPropertyDict();
        }

        #region private function
        private static void InitPropertyDict()
        {
            //drawobj
            int count = DrawObj.GetPropertyNames().Length;
            for (int i = 0; i < count; i++)
				_propertyIndexDict.Add(DrawObj.GetPropertyNames()[i], new PropertyIndex((int)DrawType.Obj, i));

            //drawvector
			count = DrawVector.GetPropertyNames().Length;
            for (int i = 0; i < count; i++)
				_propertyIndexDict.Add(DrawVector.GetPropertyNames()[i], new PropertyIndex((int)DrawType.Vector, i));

            //drawtext
            count = DrawText.GetPropertyNames().Length;
            for (int i = 0; i < count; i++)
                _propertyIndexDict.Add(DrawText.GetPropertyNames()[i], new PropertyIndex((int)DrawType.Text, i));

            //todo add control:variable
        }
        #endregion

        #region var
        private readonly Dictionary<string, IParameter> _varDict = new Dictionary<string, IParameter>(StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<string, IPropertyIndex> _propertyIndexDict = 
            new Dictionary<string, IPropertyIndex>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 初始化变量列表
        /// </summary>
        public void InitParameters()
        {
            foreach (IDrawObj obj in Objs)
            {
                obj.GenerateIndex(_propertyIndexDict);
				obj.AnalyseExpression(_varDict);
            }
        }
        public void OnDataChanged(string name, string value)
        {
            if (_varDict.ContainsKey(name))
            {
                Parameter p = (Parameter)_varDict[name];
                p.StringValue = value;
                foreach (IPropertyExpression para in p.List)
                {
					para.OnDataChanged();
                }
            }
        }
        public void OnDataChanged(string name, double value)
        {
            if (_varDict.ContainsKey(name))
            {
                Parameter p = (Parameter)_varDict[name];
                p.DecimalValue = value;
                foreach (IPropertyExpression para in p.List)
                {
					para.OnDataChanged();
                }
            }
        }
        #endregion

		#region virtual function
		public override void Initialization()
		{
			base.Initialization();
			InitParameters();
		}
		public override void InvalidateObject(IDrawObj sender)
		{
			const int width = 3;
			RectangleF rf = sender.Bound;
			rf.Inflate(width, width);

			RectInvalidate = RectangleF.Union(RectInvalidate, rf);
		}
		public override string GetCaption() { return Caption; }
		#endregion

		#region public function
		public override void ShowStyle()
		{
			switch (Style)
			{
				case FormStyle.Normal:
					Container.Show();
					break;
				case FormStyle.TopMost:
					Container.MdiParent = null;
					Container.TopMost = true;
					Container.Show();
					break;
				case FormStyle.Modal:
					Container.MdiParent = null;
					Container.ShowDialog();
					break;
				default:
					break;
			}
		}
		#endregion


	}
}
