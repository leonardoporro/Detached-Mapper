using Detached.Annotations;
using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Options;
using Detached.Mappers.Types;

namespace Detached.Mappers.Annotations.Handlers
{
    public class AggregationAnnotationHandler : AnnotationHandler<AggregationAttribute>
    {
        public override void Apply(AggregationAttribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member)
        {
            member.Aggregation();
        }
    }
} 