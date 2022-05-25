using Detached.Mappers.Context;
using System;

namespace Detached.Mappers.TypeMappers.POCO.Nullable
{
    public class NullableTypeMapper<TSource, TTarget> : ITypeMapper<TSource?, TTarget?>
        where TSource : struct
        where TTarget : struct
    {
        readonly LazyTypeMapper<TSource, TTarget> _baseMapper;

        public NullableTypeMapper(LazyTypeMapper<TSource, TTarget> baseMapper)
        {
            _baseMapper = baseMapper;
        }

        public TTarget? Map(TSource? source, TTarget? target, IMapperContext mapperContext)
        {
            if (source.HasValue)
                return (TTarget?)_baseMapper.Value.Map(source.Value, target ?? default(TTarget), mapperContext);
            else
                return null;
        }

        public object Map(object source, object target, IMapperContext mapperContext)
        {
            return Map((TSource?)source, (TTarget?)target, mapperContext);  
        }
    }
}