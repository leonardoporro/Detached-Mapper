using Detached.Mappers.Context;
using Detached.Mappers.TypeMappers;
using HotChocolate;

namespace Detached.Mappers.HotChocolate.TypeMappers
{
    public class OptionalTargetTypeMapper<TSource, TTarget> : TypeMapper<TSource, Optional<TTarget>>
    {
        readonly ITypeMapper<TSource, TTarget> _typeMapper;

        public OptionalTargetTypeMapper(ITypeMapper<TSource, TTarget> typeMapper)
        {
            _typeMapper = typeMapper;
        }

        public override Optional<TTarget> Map(TSource source, Optional<TTarget> target, IMapContext context)
        {
            if (Equals(source, null))
                return default;
            else
                return _typeMapper.Map(source, target, context);
        }
    }

    public class OptionalTargetTypeMapper<TTarget> : TypeMapper<TTarget, Optional<TTarget>>
    {
        public override Optional<TTarget> Map(TTarget source, Optional<TTarget> target, IMapContext context)
        {
            return source;
        }
    }
}