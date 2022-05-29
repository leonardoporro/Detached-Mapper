using System;

namespace Detached.Mappers.TypeMappers.POCO.Abstract
{
    public class AbstractTypeMapper<TSource, TTarget> : TypeMapper<TSource, TTarget>
    {
        readonly MapperOptions _options;
        readonly TypePairFlags _flags;
        readonly Type _concreteTargetType;

        public AbstractTypeMapper(MapperOptions options, TypePairFlags flags, Type concreteTargetType)
        {
            _options = options;
            _flags = flags;
            _concreteTargetType = concreteTargetType;
        }

        public override TTarget Map(TSource source, TTarget target, IMapContext context)
        {
            if (Equals(source, null))
            {
                return default;
            }
            else
            {
                Type sourceType = source.GetType();
                Type targetType = target?.GetType() ?? _concreteTargetType;

                if (targetType.IsAssignableFrom(sourceType))
                {
                    return (TTarget)(object)source;
                }
                else
                {
                    ITypeMapper typeMapper = _options.GetTypeMapper(new TypePair(sourceType, targetType, _flags));
                    return (TTarget)typeMapper.Map(source, target, context);
                }
            }
        }
    }
}
