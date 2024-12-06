using Detached.Annotations;
using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Options;
using Detached.Mappers.Types;

namespace Detached.Mappers.Annotations.Handlers
{
    public class EntityAnnotationHandler : AnnotationHandler<EntityAttribute>
    {
        public override void Apply(EntityAttribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member)
        {
            type.Entity(true);
        }
    }
}