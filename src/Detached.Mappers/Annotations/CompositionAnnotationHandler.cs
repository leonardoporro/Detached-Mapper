using Detached.Annotations;
using Detached.Mappers.Model;
using Detached.Mappers.Model.Types.Class;

namespace Detached.Mappers.Annotations
{
    public class CompositionAnnotationHandler : AnnotationHandler<CompositionAttribute>
    {
        public override void Apply(CompositionAttribute annotation, MapperOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            memberOptions.IsComposition = true;
        }
    }
}