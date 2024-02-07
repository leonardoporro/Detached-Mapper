using Detached.Annotations;
using Detached.Mappers.Types;

namespace Detached.Mappers.Annotations.Handlers
{
    public class MapFromAnnotationHandler : AnnotationHandler<MapFromAttribute>
    {
        public override void Apply(MapFromAttribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member)
        {
            member.MapFrom(annotation.TargetType, annotation.SourceMemberType);
        }
    }
}