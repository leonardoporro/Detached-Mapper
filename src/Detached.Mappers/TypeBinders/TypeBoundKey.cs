using System;

namespace Detached.Mappers.TypeBinders
{
    public struct TypeBoundKey
    {
        public TypeBoundKey(Type sourceClrType, Type targetClrType)
        {
            SourceClrType = sourceClrType;
            TargetClrType = targetClrType;
        }

        public Type SourceClrType { get; }

        public Type TargetClrType { get; }

        public override bool Equals(object obj)
        {
            return obj is TypeBoundKey other 
                && SourceClrType == other.SourceClrType 
                && TargetClrType == other.TargetClrType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(typeof(TypeBoundKey), SourceClrType, TargetClrType);
        }
    }
}
