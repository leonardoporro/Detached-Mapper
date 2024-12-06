using System;

namespace Detached.Annotations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class MapFromAttribute : Attribute
    {
        public MapFromAttribute(Type targetType, string sourceMemberType)
        {
            SourceMemberType = sourceMemberType;
            TargetType = targetType;
        }

        public Type TargetType { get; }

        public string SourceMemberType { get; }
    }
}