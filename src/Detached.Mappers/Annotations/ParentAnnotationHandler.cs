using Detached.Annotations;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers.Annotations
{
    public class ParentAnnotationHandler : AnnotationHandler<ParentAttribute>
    {
        public override void Apply(ParentAttribute annotation, MapperOptions modelOptions, ClassType typeOptions, ClassTypeMember memberOptions)
        {
            memberOptions.IsParent(true);
        }
    }

    public static class ParentAnnotationHandlerExtensions
    {
        const string KEY = "DETACHED_PARENT_REFERENCE";

        public static bool IsParent(this ITypeMember member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static void IsParent(this ITypeMember member, bool value)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        } 

        public static void IsParent<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value)
        {
            member.MemberOptions.IsParent(value);
        }
    }
}