using Detached.Mappers.Annotations;
using Detached.Mappers.Types;

namespace Detached.Mappers.TypePairs
{
    public class TypePairMember
    {
        public IType SourceType { get; set; }

        public IType TargetType { get; set; }

        public ITypeMember SourceMember { get; set; }

        public ITypeMember TargetMember { get; set; }

        public AnnotationCollection Annotations { get; set; } = new();

        public override string ToString()
        {
            string sourceName = SourceMember == null
                ? "null"
                : SourceMember.Name + ": " + SourceMember.ClrType.GetFriendlyName();

            string targetName = TargetMember.ClrType.GetFriendlyName();

            return $"TypePairMember ({sourceName} -> {targetName})";
        }
    }
}