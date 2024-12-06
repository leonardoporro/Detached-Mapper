using Detached.Mappers.Context;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using System;

namespace Detached.Mappers.TypeMappers.Class.Abstract
{
    public class AbstractTypeMapper<TSource, TTarget> : TypeMapper<TSource, TTarget>
    {
        readonly Mapper _mapper;
        readonly TypePair _typePair;
        readonly Type _concreteTargetType;

        public AbstractTypeMapper(Mapper mapper, TypePair typePair, Type concreteTargetType)
        {
            _mapper = mapper;
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
                    IType concreteSourceType = _mapper.Options.GetType(sourceType);
                    IType concreteTargetType = _mapper.Options.GetType(targetType);
                    TypePair concreteTypePair = _mapper.Options.GetTypePair(concreteSourceType, concreteTargetType, _typePair.ParentMember);

                    ITypeMapper typeMapper = _mapper.GetTypeMapper(concreteTypePair);

                    return (TTarget)typeMapper.Map(source, target, context);
                }
            }
        }
    }
}