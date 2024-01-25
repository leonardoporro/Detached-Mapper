using Detached.Mappers.Annotations;
using Detached.Mappers.TypePairs;
using Detached.Mappers.TypePairs.Builder;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers
{
    public static class MapIgnoreAnnotationHandlerExtensions
    {
        public static Annotation<bool> Ignored(this AnnotationCollection annotations)
        {
            return annotations.Annotation<bool>("DETACHED_MAP_IGNORE");
        }

        public static ITypeMember Ignore(this ITypeMember member, bool value = true)
        {
            member.Annotations.Ignored().Set(value);

            return member;
        }

        public static ITypeMember Include(this ITypeMember member)
        {
            member.Annotations.Ignored().Set(false);

            return member;
        }

        public static TypePairMember Include(this TypePairMember memberPair)
        {
            memberPair.Annotations.Ignored().Set(false);

            return memberPair;
        }

        public static ClassTypeMemberBuilder<TType, TMember> Include<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.Member.Include();
            return member;
        }

        public static TypePairMemberBuilder<TType, TMember> Include<TType, TMember>(this TypePairMemberBuilder<TType, TMember> member)
        {
            member.Member.Include();
            return member;
        }

        public static ITypeMember Exclude(this ITypeMember member)
        {
            member.Annotations.Ignored().Set(true);

            return member;
        }

        public static ClassTypeMemberBuilder<TType, TMember> Exclude<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member)
        {
            member.Member.Include();
            return member;
        }

        public static TypePairMember Exclude(this TypePairMember memberPair)
        {
            memberPair.Annotations.Ignored().Set(true);

            return memberPair;
        }

        public static TypePairMemberBuilder<TType, TMember> Exclude<TType, TMember>(this TypePairMemberBuilder<TType, TMember> member)
        {
            member.Member.Exclude();

            return member;
        }

        public static bool IsIncluded(this TypePairMember member)
        {
            return !member.IsIgnored();
        }

        public static bool IsIgnored(this ITypeMember member)
        {
            return member.Annotations.Ignored().Value();
        }

        public static bool IsIgnored(this TypePairMember member)
        {
            return member.Annotations.Ignored().Value();
        }
    }
}