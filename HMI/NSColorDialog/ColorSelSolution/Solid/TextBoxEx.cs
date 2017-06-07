using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace NetSCADA6.Common.NSColorManger
{
    internal enum ENUM_TYPE
    {
        T_STRING, T_NUM, T_DECIMAL
    }
    internal class TextBoxEx : TextBox
    {
        public TextBoxEx()
        {
            KeyPress += KeyPressFun;
        }
        private bool _isnegativenumber = false; //是否允许输入负号 

        /// <summary>
        /// 是否允许输入负号 
        /// </summary>
        [Category("A_CUSTOM属性"), Description("是否允许输入负号。")]
        public bool IsNegativeNumber
        {
            get { return _isnegativenumber; }
            set { _isnegativenumber = value; }
        }

        private ENUM_TYPE _type = ENUM_TYPE.T_STRING;
        /// <summary>
        /// 类型
        /// </summary>
        [Category("A_CUSTOM属性"), Description("类型")]
        public ENUM_TYPE ValType
        {
            get { return this._type; }
            set { _type = value; }
        }

        private void ValidNumeric(System.Windows.Forms.KeyPressEventArgs e)
        {
            int KeyAsc = Convert.ToInt32(e.KeyChar);
            int CurPos = this.SelectionStart;

            if (_type == ENUM_TYPE.T_STRING)
            {
                return;
            }

            if (_type == ENUM_TYPE.T_NUM)
            {
                if (KeyAsc == '-')//负号 
                {
                    if (!_isnegativenumber || CurPos != 0 || Text.IndexOf("-") != -1)
                    {
                        e.Handled = true;
                    }
                    return;
                }

                //如果输入的不是数字键，也不是回车键、Backspace键，则取消该输入
                if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
                {
                    e.Handled = true;
                }
                return;
            }

            if (_type == ENUM_TYPE.T_DECIMAL)
            {
                if (KeyAsc == '-')//负号 
                {
                    if (!_isnegativenumber || CurPos != 0 || Text.IndexOf("-") != -1)
                    {
                        e.Handled = true;
                    }
                    return;
                }

                if (KeyAsc == '.')//小数点 
                {
                    if (Text.IndexOf(".") != -1 || CurPos == 0)
                    {
                        e.Handled = true;
                    }
                    return;
                }

                //如果输入的不是数字键，也不是回车键、Backspace键，则取消该输入
                if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
                {
                    e.Handled = true;
                }
            }
        }


        private void KeyPressFun(object sender, KeyPressEventArgs e)
        {
            ValidNumeric(e);

        }
         
    }
}
