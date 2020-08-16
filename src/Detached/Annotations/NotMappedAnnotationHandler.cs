using Detached.Model;

namespace Detached.Annotations
{
    public class NotMappedAnnotationHandler : AnnotationHandler<NotMappedAttribute>
    {
        public override void Apply(NotMappedAttribute annotation, ModelOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            memberOptions.Ignored = true;
        }
    }
}
