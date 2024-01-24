using Detached.Annotations;
using Detached.Mappers.TypePairs;
using Detached.Mappers.TypePairs.Builder;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers.Annotations
{
    public class AggregationAnnotationHandler : AnnotationHandler<AggregationAttribute>
    {
        public override void Apply(AggregationAttribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member)
        {
            member.Aggregation(true);
        }
    }
}

namespace Detached.Mappers
{
    public static class AggregationAnnotationHandlerExtensions
    {
        public const string VALUE_KEY = "DETACHED_AGGREGATION";

        public static bool IsAggregation(this ITypeMember member)
        {
            return member.Annotations.TryGetValue(VALUE_KEY, out var value) && Equals(value, true);
        }

        public static bool IsAggregation(this TypePairMember member)
        {
            return member.Annotations.TryGetValue(VALUE_KEY, out var value) && Equals(value, true);
        }

        public static void Aggregation(this ITypeMember member, bool value = true)
        {
            member.Annotations[VALUE_KEY] = value;
        }

        public static ClassTypeMemberBuilder<TType, TMember> Aggregation<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.MemberOptions.Aggregation(value);

            return member;
        }

        public static void Aggregation(this TypePairMember member, bool value = true)
        {
            member.Annotations[VALUE_KEY] = value;
        }

        public static TypePairMemberBuilder<TType, TMember> Aggregation<TType, TMember>(this TypePairMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.TypePairMember.Aggregation();

            return member;
        }
    }
}