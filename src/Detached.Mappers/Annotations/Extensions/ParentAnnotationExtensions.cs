using Detached.Mappers.Annotations;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers
{
    public static class ParentAnnotationExtensions
    {
        public static Annotation<bool> Parent(this AnnotationCollection annotations)
        {
            return annotations.Annotation<bool>("DETACHED_PARENT_REFERENCE");
        }

        public static ITypeMember Parent(this ITypeMember member, bool value = true)
        {
            member.Annotations.Parent().Set(value);

            return member;
        }

        public static ClassTypeMemberBuilder<TType, TMember> Parent<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> memberPair, bool value = true)
        {
            memberPair.Member.Parent(value);

            return memberPair;
        }

        public static TypePairMember Parent(this TypePairMember memberPair, bool value = true)
        {
            memberPair.Annotations.Parent().Set(value);

            return memberPair;
        }

        public static bool IsParent(this ITypeMember member)
        {
            return member.Annotations.Parent().Value();
        }
    }
}