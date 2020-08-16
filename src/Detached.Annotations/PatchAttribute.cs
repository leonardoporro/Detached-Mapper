using System;

namespace Detached.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class PatchAttribute : Attribute
    {
    }
}