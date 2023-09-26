using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using System;

namespace Detached.Mappers.TypeMappers
{
    public struct TypeMapperKey
    {
        public TypeMapperKey(
            IType sourceType,
            IType targetType,
            TypePairMember parentMember)
        {
            SourceType = sourceType;
            TargetType = targetType;
            ParentMember = parentMember;
        }

        public IType SourceType { get; }

        public IType TargetType { get; }

        public TypePairMember ParentMember { get; }

        public override bool Equals(object obj)
        {
            return obj is TypeMapperKey other
                && other.SourceType == SourceType
                && other.TargetType == TargetType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(typeof(TypeMapperKey), SourceType, TargetType, ParentMember);
        }
    }
}
