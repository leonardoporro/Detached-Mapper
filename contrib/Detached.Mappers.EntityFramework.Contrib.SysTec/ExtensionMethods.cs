using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec
{
    public static class ExtensionMethods
    {
        public static string GetFriendlyName(this Enum genericEnum)
        {
            Type genericEnumType = genericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(genericEnum.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                var _attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (_attribs != null && _attribs.Length > 0)
                {
                    return ((System.ComponentModel.DescriptionAttribute)_attribs.ElementAt(0)).Description;
                }
            }

            return genericEnum.ToString();
        }
    }
}
