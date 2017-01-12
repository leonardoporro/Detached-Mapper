using System;

namespace Detached.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class AssociatedAttribute : Attribute
    {

    }
}
