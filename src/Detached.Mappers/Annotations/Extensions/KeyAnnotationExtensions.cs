using Detached.Mappers.Annotations;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers
{
    public static class KeyAnnotationExtensions
    {
        public static Annotation<bool> Key(this AnnotationCollection annotations)
        {
            return annotations.Annotation<bool>("DETACHED_KEY");
        }

        public static ITypeMember Key(this ITypeMember member, bool value = true)
        {
            member.Annotations.Key().Set(value);

            return member;
        }

        public static ClassTypeMemberBuilder<TType, TMember> Key<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> memberBuilder, bool value = true)
        {
            memberBuilder.Member.Key(value);

            return memberBuilder;
        }

        public static TypePairMember Key(this TypePairMember memberPair, bool value = true)
        {
            memberPair.Annotations.Key().Set(value);

            return memberPair;
        }

        public static bool IsKey(this ITypeMember member)
        {
            return member.Annotations.Key().Value();
        }

        public static bool IsKey(this TypePairMember memberPair)
        {
            return memberPair.Annotations.Key().Value();
        }

        public static bool IsKeyDefined(this IType type)
        {
            foreach (string memberName in type.MemberNames)
            {
                var member = type.GetMember(memberName);

                if (member.Annotations.Key().IsDefined())
                {
                    return true;
                }
            }

            return false;
        }
    }
}