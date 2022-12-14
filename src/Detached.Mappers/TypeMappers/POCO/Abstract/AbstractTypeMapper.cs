using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using System;

namespace Detached.Mappers.TypeMappers.POCO.Abstract
{
    public class AbstractTypeMapper<TSource, TTarget> : TypeMapper<TSource, TTarget>
    {
        readonly MapperOptions _options;
        readonly TypePair _typePair;
        readonly Type _concreteTargetType;

        public AbstractTypeMapper(MapperOptions options, TypePair typePair, Type concreteTargetType)
        {
            _options = options;
            _typePair = typePair;
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
                    IType concreteSourceType = _options.GetType(sourceType);
                    IType concreteTargetType = _options.GetType(targetType);

                    TypePair concreteTypePair = _options.GetTypePair(concreteSourceType, concreteTargetType, _typePair.ParentMember);
                    ITypeMapper typeMapper = _options.GetTypeMapper(concreteTypePair);

                    return (TTarget)typeMapper.Map(source, target, context);
                }
            }
        }
    }
}