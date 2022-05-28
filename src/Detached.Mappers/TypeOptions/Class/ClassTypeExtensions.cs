using System.Reflection;

namespace Detached.Mappers.TypeOptions.Class
{
    public static class ClassTypeExtensions
    {
        public static PropertyInfo GetPropertyInfo(this IMemberOptions memberOptions)
        {
            if(memberOptions is ClassMemberOptions clrMemberOptions)
            {
                return clrMemberOptions.PropertyInfo;
            }
            else
            {
                return null;
            }
        }
    }
}
