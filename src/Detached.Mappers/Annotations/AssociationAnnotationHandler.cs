using Detached.Mappers.Annotations;
using Detached.Annotations;
using Detached.Mappers.TypeOptions.Class;

namespace Detached.Mappers.Annotations
{
    public class AssociationAnnotationHandler : AnnotationHandler<AggregationAttribute>
    {
        public override void Apply(AggregationAttribute annotation, MapperOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            memberOptions.IsComposition = false;
        }
    }
}