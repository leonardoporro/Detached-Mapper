using Detached.Mappers.Context;

namespace Detached.Mappers.TypeMappers.POCO.Nullable
{
    public class NullableSourceTypeMapper<TSource, TTarget> : TypeMapper<TSource?, TTarget>
        where TSource : struct
    {
        readonly LazyTypeMapper<TSource, TTarget> _typeMapper;

        public NullableSourceTypeMapper(LazyTypeMapper<TSource, TTarget> typeMapper)
        {
            _typeMapper = typeMapper;
        }

        public override TTarget Map(TSource? source, TTarget target, IMapContext context)
        {
            if (source == null)
                return default;
            else
                return _typeMapper.Value.Map(source.Value, target, context);
        }
    }

    public class NullableSourceTypeMapper<TTarget> : TypeMapper<TTarget?, TTarget>
        where TTarget : struct
    {

        public override TTarget Map(TTarget? source, TTarget target, IMapContext context)
        {
            if (source.HasValue)
                return source.Value;
            else
                return default;
        }
    }
}