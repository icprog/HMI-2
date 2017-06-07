using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace NetSCADA6.Common.NSColorManger.Tool
{
    internal static class FileTool
    { 
        /// <summary>
        ///  打开保存文件对话框
        /// </summary>
        /// <param name="FileName"></param> 
        /// <param name="Filter">(过滤)Filter = "单色文件(*.nssolid)|*.nssolid |All files (*.*)|*.*"</param>
        /// <returns></returns>
        public static string OpenSaveFileDialog(string FileName, string Filter)
        {
            string defaultFilter = "All files (*.*)|*.*";
            string sFilePath = string.Empty;
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = Filter;
            saveFile.FileName = FileName; saveFile.Filter = defaultFilter;
            if (Filter != string.Empty)
                saveFile.Filter = Filter + "|" + defaultFilter;
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                sFilePath = saveFile.FileName;
            }
            return sFilePath;
        } 
        /// <summary>
        /// 打开文件对话框
        /// </summary>
        /// <param name="Filter"></param>
        /// <param name="bMultiselect"></param>
        /// <returns></returns>
        public static string OpenFileDialog(string Filter, bool bMultiselect = true)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = bMultiselect;
            fileDialog.Title = "请选择文件";

            fileDialog.Filter = Filter;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                return fileDialog.FileName;
            }
            return "";
        }
    }
}
