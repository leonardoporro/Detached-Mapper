using System;

namespace EntityFrameworkCore.Detached.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class OwnedAttribute : Attribute
    {

    }
}
