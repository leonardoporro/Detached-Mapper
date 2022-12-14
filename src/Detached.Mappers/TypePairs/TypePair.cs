using Detached.Mappers.Types;
using System.Collections.Generic;

namespace Detached.Mappers.TypePairs
{
    public class TypePair
    {
        public TypePair(IType sourceType, IType targetType, TypePairMember parentMember)
        {
            SourceType = sourceType;
            TargetType = targetType;
            ParentMember = parentMember;
        }

        public IType SourceType { get; }

        public IType TargetType { get; }

        public TypePairMember ParentMember { get; } 

        public Dictionary<string, TypePairMember> Members { get; } = new Dictionary<string, TypePairMember>();

        public override string ToString()
        {
            return $"TypePair ({SourceType} -> {TargetType})";
        }
    }
}