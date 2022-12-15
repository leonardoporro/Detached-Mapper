using Detached.Annotations;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.Mappers.Types.Class.Builder;
using System.ComponentModel.DataAnnotations;

namespace Detached.Mappers.Annotations
{
    public class KeyAnnotationHandler : AnnotationHandler<KeyAttribute>
    {
        public override void Apply(KeyAttribute annotation, MapperOptions modelOptions, ClassType typeOptions, ClassTypeMember memberOptions)
        {
            memberOptions.Key(true);
        }
    }

    public static class KeyAnnotationHandlerHandlerExtensions
    {
        const string KEY = "DETACHED_KEY";

        public static bool IsKey(this ITypeMember member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static void Key(this ITypeMember member, bool value)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static void IsKey<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value)
        {
            member.MemberOptions.Key(value);
        }
    }
}