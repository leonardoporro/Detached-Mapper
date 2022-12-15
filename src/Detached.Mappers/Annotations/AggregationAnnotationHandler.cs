using Detached.Annotations;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers.Annotations
{
    public class AggregationAnnotationHandler : AnnotationHandler<AggregationAttribute>
    {
        public override void Apply(AggregationAttribute annotation, MapperOptions modelOptions, ClassType typeOptions, ClassTypeMember memberOptions)
        {
            memberOptions.Aggregation(true);
        }
    }

    public static class AggregationAnnotationHandlerExtensions
    {
        const string KEY = "DETACHED_AGGREGATION";

        public static bool IsAssociation(this ITypeMember member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static void Aggregation(this ITypeMember member, bool value = true)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static void Aggregation<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.MemberOptions.Aggregation(value);
        }
    }
}