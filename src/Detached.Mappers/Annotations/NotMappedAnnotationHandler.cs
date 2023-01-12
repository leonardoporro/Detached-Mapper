using Detached.Mappers.TypePairs;
using Detached.Mappers.TypePairs.Builder;
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
            memberOptions.NotMapped(true);
        }
    }
}

namespace Detached.Mappers
{
    public static class NotMappedParentAnnotationHandlerExtensions
    {
        const string KEY = "DETACHED_NOT_MAPPED";

        public static bool IsNotMapped(this ITypeMember member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static void NotMapped(this ITypeMember member, bool value = true)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static ClassTypeMemberBuilder<TType, TMember> NotMapped<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.MemberOptions.NotMapped(value);
            return member;
        }

        public static bool IsNotMapped(this TypePairMember member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static bool IsMapped(this TypePairMember member)
        {
            return !member.Annotations.ContainsKey(KEY);
        }

        public static void NotMapped(this TypePairMember member, bool value = true)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static TypePairMemberBuilder<TType, TMember> NotMapped<TType, TMember>(this TypePairMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.TypePairMember.NotMapped(value);
            return member;
        }
    }
}