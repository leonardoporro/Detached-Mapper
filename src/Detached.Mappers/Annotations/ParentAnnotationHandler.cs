using Detached.Annotations;
using Detached.Mappers.TypeOptions;
using Detached.Mappers.TypeOptions.Class;

namespace Detached.Mappers.Annotations
{
    public class ParentAnnotationHandler : AnnotationHandler<ParentAttribute>
    {
        public override void Apply(ParentAttribute annotation, MapperOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            memberOptions.IsParentReference = true;
        }
    }

    public static class ParentAnnotationHandlerExtensions
    {
        public static bool GetIsParentReference(this IMemberOptions member)
        {
            return member is ClassMemberOptions classMember && classMember.IsParentReference;
        }
    }
}