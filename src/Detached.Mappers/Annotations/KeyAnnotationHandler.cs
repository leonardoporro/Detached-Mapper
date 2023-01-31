using Detached.Mappers.TypePairs;
using Detached.Mappers.TypePairs.Builder;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.Mappers.Types.Class.Builder;
using System.ComponentModel.DataAnnotations;

namespace Detached.Mappers.Annotations
{
    public class KeyAnnotationHandler : AnnotationHandler<KeyAttribute>
    {
        public override void Apply(KeyAttribute annotation, MapperOptions mapperOptions, ClassType typeOptions, ClassTypeMember memberOptions)
        {
            memberOptions.Key(true);
        }
    }
}

namespace Detached.Mappers
{
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

        public static void Key<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value)
        {
            member.MemberOptions.Key(value);
        }

        public static bool IsKey(this TypePairMember member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static void Key(this TypePairMember member, bool value = true)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static TypePairMemberBuilder<TType, TMember> Key<TType, TMember>(this TypePairMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.TypePairMember.Exclude();
            return member;
        }
    }
}