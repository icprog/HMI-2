using System.ComponentModel;
using System;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;

//这里定义的类用于 propertyGrid里的转换用
namespace NetSCADA6.Common.NSColorManger
{
    /// <summary>    
    /// 枚举控件属性转换   
    /// 用此类之前，必须保证在枚举项中定义了Description   
    /// </summary>    
    public class EnumConverterEx : EnumConverter
    {
        /// <summary>    
        /// 枚举项集合      
        /// </summary>        
        Dictionary<object, string> dic;
        /// <summary>      
        /// 构造函数      
        /// </summary>    
        public EnumConverterEx(Type type)
            : base(type)
        {
            dic = new Dictionary<object, string>();
        }
        /// <summary>     
        /// 加载枚举项集合 
        /// </summary>   
        /// <param name="context"></param>    
        private void LoadDic(ITypeDescriptorContext context)
        {
            dic = GetEnumValueDesDic(context.PropertyDescriptor.PropertyType);
        }
        /// <summary>        
        /// 是否可从来源转换     
        /// </summary>        
        /// <param name="context"></param>        
        /// <param name="sourceType"></param>        
        /// <returns></returns>        
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }
        /// <summary>        
        /// 从来源转换        
        /// </summary>        
        /// <param name="context"></param>        
        /// <param name="culture"></param>        
        /// <param name="value"></param>       
        /// /// <returns></returns>        
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                //如果是枚举    
                if (context.PropertyDescriptor.PropertyType.IsEnum)
                {
                    if (dic.Count <= 0)
                        LoadDic(context);
                    if (dic.ContainsValue(value.ToString()))
                    {
                        foreach (object obj in dic.Keys)
                        {
                            if (dic[obj] == value.ToString())
                            {
                                return obj;
                            }
                        }
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
        /// <summary>        
        /// 是否可转换        
        /// </summary>        
        /// <param name="context"></param>        
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return true;
        }
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            //ListAttribute listAttribute = (ListAttribute)context.PropertyDescriptor.Attributes[typeof(ListAttribute)];      
            //StandardValuesCollection vals = new TypeConverter.StandardValuesCollection(listAttribute._lst);          
            //Dictionary<object, string> dic = GetEnumValueDesDic(typeof(PKGenerator));       
            //StandardValuesCollection vals = new TypeConverter.StandardValuesCollection(dic.Keys);   
            if (dic == null || dic.Count <= 0)
                LoadDic(context);
            StandardValuesCollection vals = new TypeConverter.StandardValuesCollection(dic.Keys);
            return vals;
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (dic.Count <= 0)
                LoadDic(context);
            foreach (object key in dic.Keys)
            {
                if (key.ToString() == value.ToString() || dic[key] == value.ToString())
                {
                    return dic[key].ToString();
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        /// <summary>        
        /// 记载枚举的值+描述        
        /// </summary>       
        public Dictionary<object, string> GetEnumValueDesDic(Type enumType)
        {
            Dictionary<object, string> dic = new Dictionary<object, string>();
            FieldInfo[] fieldinfos = enumType.GetFields();
            foreach (FieldInfo field in fieldinfos)
            {
                if (field.FieldType.IsEnum)
                {
                    Object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (objs.Length > 0)
                    {
                        dic.Add(Enum.Parse(enumType, field.Name), ((DescriptionAttribute)objs[0]).Description);
                    }
                }
            }
            return dic;
        }
    }

    /// <summary>
    /// 扩展控件属性转换
    /// </summary>
    public class ExpandableObjectConverterEx : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            return "(集合)"; 
        }
        /*
                public override bool CanConvertTo(ITypeDescriptorContext context,
                                           System.Type destinationType)
                {
                    if (destinationType == typeof(LineControl))
                        return true;
                    return base.CanConvertTo(context, destinationType);
                }

                public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
                {
                    if (sourceType == typeof(string))
                        return true;
                    return base.CanConvertFrom(context, sourceType);
                }

                public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
                {
                    if (value is string)
                    {
                        try
                        {
                            string s = (string)value;
                            int colon = s.IndexOf(':');
                            int comma = s.IndexOf(',');
                            if (colon != -1 && comma != -1)
                            {
                                string checkWhileTyping = s.Substring(colon + 1, (comma - colon - 1));
                                colon = s.IndexOf(':', comma + 1);
                                comma = s.IndexOf(',', comma + 1);
                                string checkCaps = s.Substring(colon + 1, (comma - colon - 1));
                                colon = s.IndexOf(':', comma + 1);
                                string suggCorr = s.Substring(colon + 1);
                                LineControl so = new LineControl();
                                so.Width = float.Parse(checkWhileTyping);
                                return so;
                            }
                        }
                        catch
                        {
                            throw new ArgumentException(
                                "无法将“" + (string)value +
                                                   "”转换为 SpellingOptions 类型");
                        }
                    }
                    return base.ConvertFrom(context, culture, value);
                }*/
    }
     



}
