using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.Mappers.Types.Class.Builder;
using System.ComponentModel.DataAnnotations.Schema;

namespace Detached.Mappers.Annotations
{
    public class NotMappedAnnotationHandler : AnnotationHandler<NotMappedAttribute>
    {
        public override void Apply(NotMappedAttribute annotation, MapperOptions modelOptions, ClassType typeOptions, ClassTypeMember memberOptions)
        {
            memberOptions.IsNotMapped(true);
        }
    }

    public static class NotMappedParentAnnotationHandlerExtensions
    {
        const string KEY = "DETACHED_NOT_MAPPED";

        public static bool IsNotMapped(this ITypeMember member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static void IsNotMapped(this ITypeMember member, bool value)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static void IsNotMapped<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value)
        {
            member.MemberOptions.IsNotMapped(value);
        }
    }
}
