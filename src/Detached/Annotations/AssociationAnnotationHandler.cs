using Detached.Model;

namespace Detached.Annotations
{
    public class AssociationAnnotationHandler : AnnotationHandler<AggregationAttribute>
    {
        public override void Apply(AggregationAttribute annotation, ModelOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            memberOptions.Owned = false;
        }
    }
}
