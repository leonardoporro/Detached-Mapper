using Detached.Mappers.Context;

namespace Detached.Mappers.TypeMappers.Class.Nullable
{
    public class NullableTargetTypeMapper<TSource, TTarget> : TypeMapper<TSource, TTarget?>
        where TTarget : struct
    {
        readonly ITypeMapper<TSource, TTarget> _typeMapper;

        public NullableTargetTypeMapper(ITypeMapper<TSource, TTarget> typeMapper)
        {
            _typeMapper = typeMapper;
        }

        public override TTarget? Map(TSource source, TTarget? target, IMapContext context)
        {
            if (Equals(source, null))
                return null;
            else
                return _typeMapper.Map(source, target ?? default, context);
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