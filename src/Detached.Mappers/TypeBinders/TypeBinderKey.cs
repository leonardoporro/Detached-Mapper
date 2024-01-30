using System;

namespace Detached.Mappers.TypeBinders
{
    public struct TypeBinderKey
    {
        public TypeBinderKey(Type sourceClrType, Type targetClrType)
        {
            SourceClrType = sourceClrType;
            TargetClrType = targetClrType;
        }

        public Type SourceClrType { get; }

        public Type TargetClrType { get; }

        public override bool Equals(object obj)
        {
            return obj is TypeBinderKey other 
                && SourceClrType == other.SourceClrType 
                && TargetClrType == other.TargetClrType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(typeof(TypeBinderKey), SourceClrType, TargetClrType);
        }
    }
}
