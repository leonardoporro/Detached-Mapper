using Detached.Annotations;
using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Options;
using Detached.Mappers.Types;

namespace Detached.Mappers.Annotations.Handlers
{
    public class ParentAnnotationHandler : AnnotationHandler<ParentAttribute>
    {
        public override void Apply(ParentAttribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member)
        {
            member.Parent(true);
        }
    }
}