using Detached.Mappers.Annotations;
using Detached.Mappers.TypePairs;
using Detached.Mappers.TypePairs.Builder;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers
{
    public static class AggregationAnnotationHandlerExtensions
    {
        public static Annotation<bool> Aggregation(this AnnotationCollection annotations)
        {
            return annotations.Annotation<bool>("DETACHED_AGGREGATION");
        }

        public static void Aggregation(this ITypeMember member, bool value = true)
        {
            member.Annotations.Aggregation().Set(value);
        }

        public static ClassTypeMemberBuilder<TType, TMember> Aggregation<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> memberBuilder, bool value = true)
        {
            memberBuilder.Member.Annotations.Aggregation().Set(value);

            return memberBuilder;
        }

        public static TypePairMember Aggregation(this TypePairMember memberPair, bool value = true)
        {
            memberPair.Annotations.Aggregation().Set(value);

            return memberPair;
        }

        public static TypePairMemberBuilder<TType, TMember> Aggregation<TType, TMember>(this TypePairMemberBuilder<TType, TMember> memberBuilder, bool value = true)
        {
            memberBuilder.Member.Annotations.Aggregation().Set(value);

            return memberBuilder;
        }
    }
}