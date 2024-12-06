using Detached.Mappers.Context;
using Detached.Mappers.TypeMappers;
using HotChocolate;

namespace Detached.Mappers.HotChocolate.TypeMappers
{
    public class OptionalSourceTypeMapper<TSource, TTarget> : TypeMapper<Optional<TSource>, TTarget>
    {
        readonly ITypeMapper<TSource, TTarget> _typeMapper;

        public OptionalSourceTypeMapper(ITypeMapper<TSource, TTarget> typeMapper)
        {
            _typeMapper = typeMapper;
        }

        public override TTarget Map(Optional<TSource> source, TTarget target, IMapContext context)
        {
            if (source.IsEmpty)
                return target;
            else
                return _typeMapper.Map(source.Value, target, context);
        }
    }

    public class OptionalSourceTypeMapper<TTarget> : TypeMapper<Optional<TTarget>, TTarget>
    {

        public override TTarget Map(Optional<TTarget> source, TTarget target, IMapContext context)
        {
            if (source.HasValue)
                return source.Value;
            else
                return target;
        }
    }
}