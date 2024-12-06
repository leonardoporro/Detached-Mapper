using System;

namespace Detached.Annotations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class MapToAttribute : Attribute
    {
        public MapToAttribute(Type targetType, string targetMemberName)
        {
            TargetMemberName = targetMemberName;
            SourceType = targetType;
        }

        public Type SourceType { get; }

        public string TargetMemberName { get; }
    }
}