using Detached.Mappers.Context;

namespace Detached.Mappers.TypeMappers.POCO.Nullable
{
    public class NullableTargetTypeMapper<TSource, TTarget> : TypeMapper<TSource, TTarget?>
        where TTarget : struct
    {
        readonly LazyTypeMapper<TSource, TTarget> _typeMapper;

        public NullableTargetTypeMapper(LazyTypeMapper<TSource, TTarget> typeMapper)
        {
            _typeMapper = typeMapper;
        }

        public override TTarget? Map(TSource source, TTarget? target, IMapContext context)
        {
            if (Equals(source, null))
                return null;
            else
                return _typeMapper.Value.Map(source, target ?? default, context);
        }
    }

    public class NullableTargetTypeMapper<TTarget> : TypeMapper<TTarget, TTarget?>
        where TTarget : struct
    {
        public override TTarget? Map(TTarget source, TTarget? target, IMapContext context)
        {
            return source;
        }
    }
}