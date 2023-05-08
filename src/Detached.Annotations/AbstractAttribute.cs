using System;

namespace Detached.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class AbstractAttribute : Attribute
    {
    }
}