using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using NetSCADA6.HMI.NSDrawObj.Var;
using NetSCADA6.NSInterface.HMI.DrawObj;
using NetSCADA6.NSInterface.HMI.Form;
using NetSCADA6.NSInterface.HMI.Var;

namespace NetSCADA6.HMI.NSDrawObj
{
    /// <summary>
    /// 成组控件
    /// </summary>
	public class DrawGroup : DrawVector, IDrawGroup
    {
        public DrawGroup(IEnumerable<IDrawObj> list = null)
        {
            if (list == null)
                return;
            
            _objList.AddRange(list);
            _isVector = _objList.All(obj => obj.IsVector);
            foreach (IDrawObj obj in ObjList)
                obj.GroupParant = this;
            base.Parant = _objList[0].Parant;
        }
        
        #region property
        private List<IDrawObj> _objList = new List<IDrawObj>();
        public List<IDrawObj> ObjList { get { return _objList; } }
        public override DrawType Type { get { return DrawType.Group; } }
    	private bool _isVector = true;
        public override bool IsVector { get { return _isVector; } }
        public override GraphicsPath MatrixPath
        {
            get
            {
            	return IsVector ? base.MatrixPath : BasePath;
            }
        }
        public override GraphicsPath Path
        {
            get
            {
            	return IsVector ? base.Path : BasePath;
            }
        }
        public override Matrix Matrix
        {
            get
            {
            	return IsVector ? base.Matrix : null;
            }
        }
    	public override bool CanCombine
    	{
			get { return IsVector && _objList.All(obj => ((IDrawVector)obj).CanCombine); }
    	}
        #endregion

        /// <summary>
        /// 解组操作，解组时调用
        /// </summary>
        public void Ungroup()
        {
            foreach (IDrawObj obj in ObjList)
                ((DrawObj)obj).UngroupOper();
        }

        #region public virtual function
        public override void Draw(Graphics g)
        {
			foreach (IDrawObj obj in ObjList) 
                obj.Draw(g);
        }
        public override bool IsVisible(PointF point)
        {
            return _objList.Any(obj => obj.IsVisible(point));
        }
        public override void MouseMove(PointF point)
        {
            _groupInvalidate = true;
            foreach (IDrawObj obj in ObjList)
                obj.MouseMove(point);

            LoadGeneratePathEvent();
        }
        public override void MouseFrameMove(PointF point, int pos)
        {
			//正交模式
			float orthoTan = 0;
			if (Parant.Studio.IsOrtho)
				orthoTan = Calculation.CalcOrthoTan(DataBk, pos);
			
			float xOff, yOff, wOff, hOff;
            Calculation.GetFrameOffset(DataBk, point, pos, out xOff, out yOff, out wOff, out hOff, orthoTan);

            ChildObjBoundMove(xOff, yOff, wOff, hOff);

            if (IsVector)
                FrameMove(xOff, yOff, wOff, hOff);
            else
                LoadGeneratePathEvent();
        }
        public override void BoundMove(float xOff, float yOff, float wOff, float hOff)
        {
        	RectangleF rect = IsVector ? 
				BaseBoundMove(xOff, yOff, wOff, hOff) : Calculation.OffsetRect(DataBk.Rect, xOff, yOff, wOff, hOff);

        	SetRect(rect);
        }
    	public override void GroupRotate(float angle, PointF center)
        {
            RectangleF rect = BaseGroupRotate(angle, center);
            
            float xOff = rect.X - DataBk.Rect.X;
            float yOff = rect.Y - DataBk.Rect.Y;
            _groupInvalidate = true;
            foreach (IDrawObj obj in ObjList)
                ((DrawObj)obj).SetRect(Calculation.OffsetRect(((DrawObj)obj).DataBk.Rect, xOff, yOff, 0, 0));

            LoadGeneratePathEvent();
        }
		public override void LoadInitializationEvent()
		{
			foreach (IDrawObj obj in _objList)
				obj.LoadInitializationEvent();

			base.LoadInitializationEvent();
		}
    	#endregion

        #region protected function
		protected internal override void SetRect(RectangleF rect)
		{
			float xOff = rect.X - DataBk.Rect.X;
			float yOff = rect.Y - DataBk.Rect.Y;
			float wOff = rect.Width - DataBk.Rect.Width;
			float hOff = rect.Height - DataBk.Rect.Height;
			ChildObjBoundMove(xOff, yOff, wOff, hOff);

			LoadGeneratePathEvent();
		}
        protected sealed override void OnGeneratePath(ref GraphicsPath path)
        {
            foreach (IDrawObj obj in ObjList)
            {
                BasePath.AddPath(obj.MatrixPath, false);
            }
            SetDirectRect(BasePath.GetBounds());
        }
        protected internal override void GenerateMatrix()
        {
            if (!IsVector)
                return;
            
            base.GenerateMatrix();
            foreach (IDrawObj obj in ObjList)
                ((DrawObj)obj).GenerateMatrix();

        }
        protected internal override void GenerateBound()
        {
        	Bound = Rectangle.Empty;
			
            foreach (IDrawObj obj in ObjList)
            {
            	((DrawObj)obj).GenerateBound();
            	Bound = Bound == Rectangle.Empty ? obj.Bound : RectangleF.Union(Bound, obj.Bound);
            }
        }
        protected internal override void LoadGeneratePathEvent(bool needCalculatePath = true)
        {
            base.LoadGeneratePathEvent(needCalculatePath);
            _groupInvalidate = false;
        }
		#endregion

        #region private function
        private void ChildObjBoundMove(float xOff, float yOff, float wOff, float hOff)
        {
            float xOffRate = wOff / DataBk.Rect.Width;
            float yOffRate = hOff / DataBk.Rect.Height;

            _groupInvalidate = true;
            foreach (IDrawObj obj in ObjList)
            {
                RectangleF ojbRect = ((DrawObj) obj).DataBk.MatrixBound;
                float objX = xOff + (ojbRect.X - DataBk.Rect.X) * xOffRate;
                float objW = ojbRect.Width * xOffRate;
                float objY = yOff + (ojbRect.Y - DataBk.Rect.Y) * yOffRate;
                float objH = ojbRect.Height * yOffRate;

                obj.BoundMove(objX, objY, objW, objH);
            }
        }
        #endregion

		#region common
		#region serialize
		public override void Serialize(BinaryFormatter bf, Stream s)
        {
            base.Serialize(bf, s);

            const int version = 1;

            bf.Serialize(s, version);

            int objCount = _objList.Count;
            bf.Serialize(s, objCount);
            for (int i = 0; i < objCount; i++)
            {
                IDrawObj obj = _objList[i];
                
                bf.Serialize(s, obj.Type);
                obj.Serialize(bf, s);
            }
        }
        public override void Deserialize(BinaryFormatter bf, Stream s)
        {
            base.Deserialize(bf, s);

            int version = (int)bf.Deserialize(s);

            int objCount = (int) bf.Deserialize(s);
            _objList.Clear();
            _objList.Capacity = objCount;
            for (int i = 0; i < objCount; i++)
            {
                DrawType type = (DrawType)bf.Deserialize(s);
                IDrawObj obj = Parant.Common.CreateDrawObj(type);
				//DrawGroup控件需要提前获取Form，用来创建子控件
				if (obj.Type == DrawType.Group)
					obj.Parant = Parant;
				obj.Deserialize(bf, s);
                _objList.Add(obj);
            }
        }

        protected sealed override void OnInitialization()
        {
            _isVector = _objList.All(obj => obj.IsVector);
            foreach (IDrawObj obj in ObjList)
            {
                obj.GroupParant = this;
                obj.LoadInitializationEvent();
            }
        }
        #endregion

        #region clone
        public override Object Clone()
        {
            DrawGroup obj = (DrawGroup)base.Clone();
            obj._objList = new List<IDrawObj>();
            foreach (IDrawObj drawObj in _objList)
                obj._objList.Add((IDrawObj)drawObj.Clone());

            return obj;
        }
        #endregion

		#region dispose
		protected override void DisposeResource()
		{
			if (Disposed)
			{
				int count = _objList.Count;
				for (int i = 0; i < count; i++)
					_objList[i].Dispose();
			}

			base.DisposeResource();
		}
		#endregion
		#endregion

		#region virtual property
		public override IHMIForm Parant
        {
            set 
            { 
                base.Parant = value;

                foreach (IDrawObj obj in ObjList)
                    obj.Parant = value;
            }
        }
        #endregion

        #region event
		public override void LoadMouseEvent(MouseButtons button, PointF location, MouseType type)
        {
            base.LoadMouseEvent(button, location, type);

            IDrawObj draw = ObjList.LastOrDefault(obj => obj.CanSelect(location));
            if (draw != null)
                ((DrawObj) draw).LoadMouseEvent(button, location, type);
        }
        #endregion

        #region refresh
        private bool _groupInvalidate;
        /// <summary>
        /// 父控件刷新标志，为true时，子控件不刷新
        /// </summary>
		[Browsable(false)]
        public bool GroupInvalideate
        {
            get
            {
                if (_groupInvalidate)
                    return true;

                IDrawGroup g = GroupParant;
                while (g != null)
                {
                    if (g.GroupInvalideate)
                        return true;

                    g = g.GroupParant;
                }

                return false;
            }
        }

        #endregion

		#region data backup
        protected internal override void BackupData()
        {
			foreach (IDrawObj obj in _objList)
				((DrawObj)obj).BackupData();

			base.BackupData();
        }
		protected internal override void BackupMousePos(PointF point)
		{
			foreach (IDrawObj obj in _objList)
				((DrawObj)obj).BackupMousePos(point);

			base.BackupMousePos(point);
		}
        #endregion

		#region var
		public override void GenerateIndex(Dictionary<string, IPropertyIndex> dict)
		{
			base.GenerateIndex(dict);
			foreach (IDrawObj obj in _objList)
				obj.GenerateIndex(dict);
		}
		public override void AnalyseExpression(Dictionary<string, IParameter> dict)
		{
			base.AnalyseExpression(dict);
			foreach (IDrawObj obj in _objList)
				obj.AnalyseExpression(dict);
		}
		#endregion

	}


}
