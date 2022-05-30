using Detached.Annotations;
using Detached.Mappers.TypeOptions;
using Detached.Mappers.TypeOptions.Class;
using Detached.Mappers.TypeOptions.Class.Builder;

namespace Detached.Mappers.Annotations
{
    public class AggregationAnnotationHandler : AnnotationHandler<AggregationAttribute>
    {
        public override void Apply(AggregationAttribute annotation, MapperOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            memberOptions.IsAggregation(true);
        }
    }

    public static class AssociationAnnotationHandlerExtensions
    {
        const string KEY = "DETACHED_AGGREGATION";

        public static bool IsAssociation(this IMemberOptions member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static void IsAggregation(this IMemberOptions member, bool value)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static void IsAssociation<TType, TMember>(this ClassMemberOptionsBuilder<TType, TMember> member, bool value)
        {
            member.MemberOptions.IsAggregation(value);
        }
    }
}