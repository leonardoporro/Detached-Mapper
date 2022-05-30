using Detached.Annotations;
using Detached.Mappers.TypeOptions;
using Detached.Mappers.TypeOptions.Class;
using Detached.Mappers.TypeOptions.Class.Builder;
using System.ComponentModel.DataAnnotations;

namespace Detached.Mappers.Annotations
{
    public class KeyAnnotationHandler : AnnotationHandler<KeyAttribute>
    {
        public override void Apply(KeyAttribute annotation, MapperOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            memberOptions.IsKey(true);
        }
    }

    public static class KeyAnnotationHandlerHandlerExtensions
    {
        const string KEY = "DETACHED_KEY";

        public static bool IsKey(this IMemberOptions member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static void IsKey(this IMemberOptions member, bool value)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static void IsKey<TType, TMember>(this ClassMemberOptionsBuilder<TType, TMember> member, bool value)
        {
            member.MemberOptions.IsKey(value);
        }
    }
}