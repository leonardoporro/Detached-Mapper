using Detached.Mappers.Types;
using System;

namespace Detached.Mappers.TypePairs
{
    public struct TypePairKey
    {
        public TypePairKey(
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
            return obj is TypePairKey other
                && other.SourceType == SourceType
                && other.TargetType == TargetType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SourceType, TargetType, ParentMember);
        }
    }
}
