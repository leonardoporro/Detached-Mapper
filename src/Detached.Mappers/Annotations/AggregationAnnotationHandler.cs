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
            memberOptions.IsAggregation(true);
        }
    }

    public static class AssociationAnnotationHandlerExtensions
    {
        const string KEY = "DETACHED_AGGREGATION";

        public static bool IsAssociation(this ITypeMember member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static void IsAggregation(this ITypeMember member, bool value)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static void IsAssociation<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value)
        {
            member.MemberOptions.IsAggregation(value);
        }
    }
}