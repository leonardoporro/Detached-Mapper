using Detached.Mappers.Context;
using System;

namespace Detached.Mappers.TypeMappers.POCO.Primitive
{
    public class PrimitiveTypeMapper<TSource, TTarget> : TypeMapper<TSource, TTarget>
    {
        public override TTarget Map(TSource source, TTarget target, IMapperContext mapperContext)
        {
            return (TTarget)Convert.ChangeType(source, typeof(TTarget));
        }
    }
}
