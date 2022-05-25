using Detached.Mappers.Context;
using System;

namespace Detached.Mappers.TypeMappers.POCO.Object
{
    public class BoxingTypeMapper<TSource, TTarget> : ITypeMapper<TSource, TTarget>
    {
        readonly Mapper _mapper;
        readonly TypePairFlags _flags;

        public BoxingTypeMapper(Mapper mapper, TypePairFlags flags)
        {
            _mapper = mapper;
            _flags = flags;
        }

        public TTarget Map(TSource source, TTarget target, IMapperContext mapperContext)
        {
            if (Equals(source, null))
            {
                return default(TTarget);
            }
            else
            {
                Type sourceType = source.GetType();
                Type targetType = target?.GetType() ?? typeof(TTarget);

                if (targetType.IsAssignableFrom(sourceType))
                {
                    return (TTarget)(object)source;
                }
                else
                {
                    ITypeMapper typeMapper = _mapper.GetTypeMapper(new TypePair(sourceType, targetType, _flags));
                    return (TTarget)typeMapper.Map(source, target, mapperContext);
                }
            }
        }

        public object Map(object source, object target, IMapperContext mapperContext)
        {
            return Map((TSource)source, target, mapperContext);
        }
    }
}
