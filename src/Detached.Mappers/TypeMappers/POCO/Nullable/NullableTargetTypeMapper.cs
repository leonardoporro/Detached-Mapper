using Detached.Mappers.Context;

namespace Detached.Mappers.TypeMappers.POCO.Nullable
{
    public class NullableTargetTypeMapper<TSource, TTarget> : ITypeMapper<TSource, TTarget?>
        where TTarget : struct
    {
        readonly LazyTypeMapper<TSource, TTarget> _typeMapper;

        public NullableTargetTypeMapper(LazyTypeMapper<TSource, TTarget> typeMapper)
        {
            _typeMapper = typeMapper;
        }

        public TTarget? Map(TSource source, TTarget? target, IMapperContext mapperContext)
        {
            if (Equals(source, null))
                return null;
            else
                return _typeMapper.Value.Map(source, target ?? default, mapperContext);
        }

        public object Map(object source, object target, IMapperContext mapperContext)
        {
            return Map((TSource)source, (TTarget?)target, mapperContext);
        }
    }

    public class NullableTargetTypeMapper<TTarget> : ITypeMapper<TTarget, TTarget?>
        where TTarget : struct
    {
        public TTarget? Map(TTarget source, TTarget? target, IMapperContext mapperContext)
        {
            return source;
        }

        public object Map(object source, object target, IMapperContext mapperContext)
        {
            return Map((TTarget)source, (TTarget?)target, mapperContext);
        }
    }
}