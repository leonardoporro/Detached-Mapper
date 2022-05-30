using Detached.Annotations;
using Detached.Mappers.TypeOptions;
using Detached.Mappers.TypeOptions.Class;
using Detached.Mappers.TypeOptions.Class.Builder;

namespace Detached.Mappers.Annotations
{
    public class ParentAnnotationHandler : AnnotationHandler<ParentAttribute>
    {
        public override void Apply(ParentAttribute annotation, MapperOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            memberOptions.IsParent(true);
        }
    }

    public static class ParentAnnotationHandlerExtensions
    {
        const string KEY = "DETACHED_PARENT_REFERENCE";

        public static bool IsParent(this IMemberOptions member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static void IsParent(this IMemberOptions member, bool value)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        } 

        public static void IsParent<TType, TMember>(this ClassMemberOptionsBuilder<TType, TMember> member, bool value)
        {
            member.MemberOptions.IsParent(value);
        }
    }
}