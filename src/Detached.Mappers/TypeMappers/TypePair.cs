using System;

namespace Detached.Mappers.TypeMappers
{
    public enum TypePairFlags
    {
        None = 0,
        Root = 2,
        Owned = 4
    }

    public struct TypePair
    {
        public TypePair(Type sourceType, Type targetType, TypePairFlags flags)
        {
            SourceType = sourceType;
            TargetType = targetType;
            Flags = flags;
        }

        public Type SourceType { get; }

        public Type TargetType { get; }

        public TypePairFlags Flags { get; }

        public override bool Equals(object obj)
        {
            return obj is TypePair other
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