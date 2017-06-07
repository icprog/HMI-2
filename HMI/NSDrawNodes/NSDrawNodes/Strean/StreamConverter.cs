using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization; 

namespace NetSCADA6.HMI.NSDrawNodes
{

    public class StreamConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context,
            Type sourceType)
        {
            return false;
        }
        public override object ConvertTo(ITypeDescriptorContext context,
           CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is StreamControl)
            {
                string str = (value as StreamControl).Enable.ToString();
                return str;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

}
