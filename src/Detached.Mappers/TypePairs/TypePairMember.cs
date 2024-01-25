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
    }
}