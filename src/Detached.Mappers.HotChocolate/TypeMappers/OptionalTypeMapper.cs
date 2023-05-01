using Detached.Mappers.TypeMappers;
using HotChocolate;

namespace Detached.Mappers.HotChocolate.TypeMappers
{
    public class OptionalTypeMapper<TSource, TTarget> : TypeMapper<Optional<TSource>, Optional<TTarget>>
    {
        readonly LazyTypeMapper<TSource, TTarget> _baseMapper;

        public OptionalTypeMapper(LazyTypeMapper<TSource, TTarget> baseMapper)
        {
            _baseMapper = baseMapper;
        }

        public override Optional<TTarget> Map(Optional<TSource> source, Optional<TTarget> target, IMapContext context)
        {
            if (source.HasValue)
                return (Optional<TTarget>)_baseMapper.Value.Map(source.Value, target.Value, context);
            else
                return target;
        }
    }
}