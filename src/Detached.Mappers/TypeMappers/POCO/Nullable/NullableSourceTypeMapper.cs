using Detached.Mappers.Context;

namespace Detached.Mappers.TypeMappers.POCO.Nullable
{
    public class NullableSourceTypeMapper<TSource, TTarget> : ITypeMapper<TSource?, TTarget>
        where TSource : struct
    {
        readonly LazyTypeMapper<TSource, TTarget> _typeMapper;

        public NullableSourceTypeMapper(LazyTypeMapper<TSource, TTarget> typeMapper)
        {
            _typeMapper = typeMapper;
        }

        public TTarget Map(TSource? source, TTarget target, IMapperContext mapperContext)
        {
            if (source == null)
                return default;
            else
                return _typeMapper.Value.Map(source.Value, target, mapperContext);
        }

        public object Map(object source, object target, IMapperContext mapperContext)
        {
            return Map((TSource?)source, (TTarget)target, mapperContext);
        }
    }

    public class NullableSourceTypeMapper<TTarget> : ITypeMapper<TTarget?, TTarget>
        where TTarget : struct
    {

        public TTarget Map(TTarget? source, TTarget target, IMapperContext mapperContext)
        {
            if (source.HasValue)
                return source.Value;
            else
                return default;
        }

        public object Map(object source, object target, IMapperContext mapperContext)
        {
            return Map((TTarget?)source, (TTarget)target, mapperContext);
        }
    }
}