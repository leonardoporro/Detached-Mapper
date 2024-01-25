using Detached.Mappers.TypePairs;
using Detached.Mappers.TypePairs.Builder;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers
{
    public static class MapCopyAnnotationHandlerExtensions
    {
        public const string VALUE_KEY = "DETACHED_PRIMITIVE";

        public static void Primitive(this TypePairMember member, bool value = true)
        {
            member.Annotations[VALUE_KEY] = value;
        }

        public static void Primitive(this ITypeMember member, bool value = true)
        {
            member.Annotations[VALUE_KEY] = value;
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

        public static bool IsSetAsPrimitive(this TypePairMember memberPair)
        {
            return
                memberPair.TargetMember.Annotations.TryGetValue(VALUE_KEY, out var value1) && Equals(value1, true) ||
                memberPair.SourceType.Annotations.TryGetValue(VALUE_KEY, out var value2) && Equals(value2, true) ||
                memberPair.Annotations.TryGetValue(VALUE_KEY, out var value3) && Equals(value3, true);
        }
    }
}