using Detached.Model;

namespace Detached.Annotations
{
    public class CompositionAnnotationHandler : AnnotationHandler<CompositionAttribute>
    {
        public override void Apply(CompositionAttribute annotation, MapperModelOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            memberOptions.IsComposition = true;
        }
    }
}
