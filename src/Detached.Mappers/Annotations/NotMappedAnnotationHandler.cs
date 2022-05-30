using Detached.Mappers.TypeOptions;
using Detached.Mappers.TypeOptions.Class;
using Detached.Mappers.TypeOptions.Class.Builder;
using System.ComponentModel.DataAnnotations.Schema;

namespace Detached.Mappers.Annotations
{
    public class NotMappedAnnotationHandler : AnnotationHandler<NotMappedAttribute>
    {
        public override void Apply(NotMappedAttribute annotation, MapperOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            memberOptions.IsNotMapped(true);
        }
    }

    public static class NotMappedParentAnnotationHandlerExtensions
    {
        const string KEY = "DETACHED_NOT_MAPPED";

        public static bool IsNotMapped(this IMemberOptions member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static void IsNotMapped(this IMemberOptions member, bool value)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static void IsNotMapped<TType, TMember>(this ClassMemberOptionsBuilder<TType, TMember> member, bool value)
        {
            member.MemberOptions.IsNotMapped(value);
        }
    }
}
