using Detached.Mappers.Annotations;
using Detached.Mappers.TypePairs;
using Detached.Mappers.TypePairs.Builder;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers.Annotations.Extensions
{
    public static class PrimitiveAnnotationExtensions
    {
        public static Annotation<bool> Primitive(this AnnotationCollection annotations)
        {
            return annotations.Annotation<bool>("DETACHED_PRIMITIVE");
        }

        public static ITypeMember Primitive(this ITypeMember member, bool value = true)
        {
            member.Annotations.Primitive().Set(value);

            return member;
        }

        public static TypePairMember Primitive(this TypePairMember memberPair, bool value = true)
        {
            memberPair.Annotations.Primitive().Set(value);

            return memberPair;
        }
        public static ClassTypeMemberBuilder<TType, TMember> Primitive<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.Member.Primitive(value);

            return member;
        }

        public static TypePairMemberBuilder<TType, TMember> Primitive<TType, TMember>(this TypePairMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.Member.Primitive(value);

            return member;
        }

        public static bool IsSetAsPrimitive(this ITypeMember member, bool value = true)
        {
            return member.Annotations.Primitive().Value();
        }

        public static bool IsSetAsPrimitive(this TypePairMember memberPair)
        {
            return memberPair.TargetMember.IsSetAsPrimitive()
                || memberPair.SourceType.IsPrimitive()
                || memberPair.Annotations.Primitive().Value();
        }
    }
}