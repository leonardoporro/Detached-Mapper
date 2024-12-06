using Detached.Annotations;
using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Options;
using Detached.Mappers.Types;

namespace Detached.Mappers.Annotations.Handlers
{
    public class MapToAnnotationHandler : AnnotationHandler<MapToAttribute>
    {
        public override void Apply(MapToAttribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member)
        {
            member.MapTo(annotation.SourceType, annotation.TargetMemberName);
        }
    }
}