using Detached.Annotations;
using Detached.Mappers.Model;
using Detached.Mappers.Model.Types.Class;

namespace Detached.Mappers.Annotations
{
    public class EntityAnnotationHandler : AnnotationHandler<EntityAttribute>
    {
        public override void Apply(EntityAttribute annotation, MapperOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            typeOptions.IsEntity = true;
        }
    }
}