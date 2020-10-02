using Detached.Mappers.Model;
using Detached.Mappers.Model.Types.Class;
using Detached.Mappers.Annotations;
using Detached.Annotations;

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