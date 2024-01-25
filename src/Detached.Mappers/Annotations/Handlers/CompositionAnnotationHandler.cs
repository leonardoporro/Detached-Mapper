using Detached.Annotations;
using Detached.Mappers.Types;

namespace Detached.Mappers.Annotations.Handlers
{
    public class CompositionAnnotationHandler : AnnotationHandler<CompositionAttribute>
    {
        public override void Apply(CompositionAttribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member)
        {
            member.Composition();
        }
    }
}