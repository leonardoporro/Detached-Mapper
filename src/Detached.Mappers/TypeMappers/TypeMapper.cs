using Detached.Mappers.Context;
using System;

namespace Detached.Mappers.TypeMappers
{
    public abstract class TypeMapper<TSource, TTarget> : ITypeMapper<TSource, TTarget>
    {
        public abstract TTarget Map(TSource source, TTarget target, IMapperContext mapperContext);

        public object Map(object source, object target, IMapperContext mapperContext)
        {
            return Map((TSource)source, (TTarget)target, mapperContext);
        }
    }
}
