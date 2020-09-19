using Detached.Model;

namespace Detached.Annotations
{
    public class EntityAnnotationHandler : AnnotationHandler<EntityAttribute>
    {
        public override void Apply(EntityAttribute annotation, MapperModelOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            typeOptions.IsEntity = true;
        }
    }
}
