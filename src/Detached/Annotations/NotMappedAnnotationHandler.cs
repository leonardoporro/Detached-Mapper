using Detached.Model;

namespace Detached.Annotations
{
    public class NotMappedAnnotationHandler : AnnotationHandler<NotMappedAttribute>
    {
        public override void Apply(NotMappedAttribute annotation, MapperModelOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            memberOptions.Ignored = true;
        }
    }
}
