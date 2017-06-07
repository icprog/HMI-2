using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetSCADA6.Common.NSColorManger
{
    class FlowLayoutPanelEx : FlowLayoutPanel
    {
        /// <summary>
        /// 画刷链表
        /// </summary>
        PresetList list = new PresetList();
        /// <summary>
        /// 单格画刷
        /// </summary>
        public int CellLength
        {
            set
            {
                _cellLength = value;
            }
            get
            {
                return _cellLength;
            }
        }
        int _cellLength = 50; 
        /// <summary>
        /// 当前选择色变化事件
        /// </summary>
        public delegate void SelectedChange(FlowCellUserControl uc);
        public SelectedChange EventSelectedChange;


        #region 添加删除
        public void Add(FlowCell cell)
        { 
            list.Add(cell);//添加到链表 
            FlowCellUserControl CellUserControl = new FlowCellUserControl(cell);
            CellUserControl.Length = CellLength;
            CellUserControl.EventSelectTrue = CellPresetUserControl_SelectedTrue;
            CellUserControl.EventMenuDelete = CellPresetUserControl_MenuDelete;
            Controls.Add(CellUserControl);
        }
        public void ClearAllCell()
        {
            Controls.Clear();
            list.ClearAll();
        }
        #endregion

        #region  鼠标动作 选择 删除
        void CellPresetUserControl_SelectedTrue(FlowCellUserControl cellUC)
        { 
            foreach (UserControl userControl in Controls)
            {
                if (userControl is FlowCellUserControl && userControl != cellUC)
                {
                    ((FlowCellUserControl)userControl)._Selected = false;
                    ((FlowCellUserControl)userControl).Invalidate();
                }
            } 
            if (EventSelectedChange != null)
                EventSelectedChange(cellUC);
        }
        void CellPresetUserControl_MenuDelete(FlowCellUserControl cellUC)
        {
            list.Remove(cellUC.CellPreset);
            Controls.Remove(cellUC);
        }
        #endregion

        #region 序列化
        void UpdateData(bool ToControl)
        {
            if (ToControl)
            {
                Controls.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    FlowCell cell = list[i];
                    FlowCellUserControl CellUserControl = new FlowCellUserControl(cell);
                    CellUserControl.Length = CellLength;
                    CellUserControl.EventSelectTrue = CellPresetUserControl_SelectedTrue;
                    CellUserControl.EventMenuDelete = CellPresetUserControl_MenuDelete;
                    Controls.Add(CellUserControl);
                }
            }
            else
            {
                //...
            }
        }
        public void Serialize(FileStream fs, BinaryFormatter bf)
        {
            list.Serialize(fs, bf);
        }
        public void Deserialize(FileStream fs, BinaryFormatter bf)
        {
            list.Deserialize(fs, bf);
            UpdateData(true);
        }
        #endregion
    }
}
