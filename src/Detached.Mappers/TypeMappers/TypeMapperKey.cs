using System;

namespace Detached.Mappers.TypeMappers
{
    public enum TypeMapperKeyFlags
    {
        None = 0,
        Root = 2,
        Owned = 4
    }

    public struct TypeMapperKey
    {
        public TypeMapperKey(Type sourceType, Type targetType, TypeMapperKeyFlags flags)
        {
            SourceType = sourceType;
            TargetType = targetType;
            Flags = flags;
        }

        public Type SourceType { get; }

        public Type TargetType { get; }

        public TypeMapperKeyFlags Flags { get; }

        public override bool Equals(object obj)
        {
            return obj is TypeMapperKey other
                && other.SourceType == SourceType
                && other.TargetType == TargetType
                && other.Flags == Flags;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SourceType, TargetType, Flags);
        }
    }
}