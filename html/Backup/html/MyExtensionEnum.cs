using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace UtilETWeb
{

    public class DescriptionEnum : Attribute
    {
        public string Text;
        public DescriptionEnum(string text)
        {
            Text = text;
        }
    }

    public static class MyExtensionEnum
    {
        public static string GetDescription(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionEnum), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionEnum)attrs[0]).Text;
            }
            return en.ToString();
        }
    }   
}
