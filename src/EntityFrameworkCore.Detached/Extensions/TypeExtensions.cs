using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached
{
    public static class TypeExtensions
    {
        public static PropertyInfo GetPropertyByNameCaseInsensitive(this Type clrType, string propertyName, bool throwIfNotFound = true)
        {
            foreach (PropertyInfo propInfo in clrType.GetRuntimeProperties())
            {
                if (string.Compare(propInfo.Name, propertyName, true) == 0)
                    return propInfo;
            }

            if (throwIfNotFound)
                throw new ArgumentException($"Property {propertyName} does not exist in object {clrType}.");
            else
                return null;
        }
    }
}
