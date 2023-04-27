using Detached.Annotations;
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
        public override void Apply(NotMappedAttribute annotation, MapperOptions mapperOptions, ClassType typeOptions, ClassTypeMember memberOptions)
        {
            memberOptions.NotMapped(true);
        }
    }

    public class NotAttachedAnnotationHandler : AnnotationHandler<NotAttachedAttribute>
    {
        public override void Apply(NotAttachedAttribute annotation, MapperOptions mapperOptions, ClassType typeOptions, ClassTypeMember memberOptions)
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

        public static ClassTypeMemberBuilder<TType, TMember> Exclude<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member)
        {
            member.MemberOptions.NotMapped(true);
            return member;
        }

        public static ClassTypeMemberBuilder<TType, TMember> Include<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.MemberOptions.NotMapped(false);
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

        public static void Exclude(this TypePairMember member)
        {
            member.Annotations[KEY] = true;
        }

        public static TypePairMemberBuilder<TType, TMember> Exclude<TType, TMember>(this TypePairMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.TypePairMember.Exclude();
            return member;
        }

        public static void Include(this TypePairMember member)
        {
            member.Annotations.Remove(KEY);
        }

        public static TypePairMemberBuilder<TType, TMember> Include<TType, TMember>(this TypePairMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.TypePairMember.Include();
            return member;
        }
    }
}