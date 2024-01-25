using Detached.Annotations;
using Detached.Mappers.Types;

namespace Detached.Mappers.Annotations.Handlers
{
    public class AbstractAnnotationHandler : AnnotationHandler<AbstractAttribute>
    {
        public override void Apply(AbstractAttribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member)
        {
            type.Abstract(true);
        }
    }
}