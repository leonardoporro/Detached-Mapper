using Detached.Model;

namespace Detached.Annotations
{
    public class PatchAnnotationHandler : AnnotationHandler<PatchAttribute>
    {
        public override void Apply(PatchAttribute annotation, ModelOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            typeOptions.UsePatchProxy = true;
        }
    }
}