using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.DataAnnotations.Plugins.ManyToMany
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class ManyToManyAttribute : Attribute
    {
        string _navigationPropertyName;

        public ManyToManyAttribute(string navigationPropertyName)
        {
            _navigationPropertyName = navigationPropertyName;
        }

        public string NavigationPropertyName
        {
            get
            {
                return _navigationPropertyName;
            }
        }
    }
}
