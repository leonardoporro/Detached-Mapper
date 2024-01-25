using Detached.Annotations;
using Detached.Mappers.Types;

namespace Detached.Mappers.Annotations.Handlers
{
    public class MapIgnoreAnnotationHandler : AnnotationHandler<MapIgnoreAttribute>
    {
        public override void Apply(MapIgnoreAttribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member)
        {
            member.Ignore(true);
        }
    }
} 