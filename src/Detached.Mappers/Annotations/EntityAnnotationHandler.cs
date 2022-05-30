using Detached.Annotations;
using Detached.Mappers.TypeOptions;
using Detached.Mappers.TypeOptions.Class;

namespace Detached.Mappers.Annotations
{
    public class EntityAnnotationHandler : AnnotationHandler<EntityAttribute>
    {
        public override void Apply(EntityAttribute annotation, MapperOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            typeOptions.Kind = TypeKind.Entity;
        }
    }
}