using Detached.Mappers.Context;
using Detached.Mappers.Exceptions;
using System;
using System.Collections.Generic;

namespace Detached.Mappers.TypeMappers.POCO.Inherited
{
    public class InheritedTypeMapper<TSource, TTarget, TDiscriminator> : TypeMapper<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        Func<TSource, IMapperContext, TDiscriminator> _getDiscriminator;
        Dictionary<TDiscriminator, ILazyTypeMapper> _mappers;

        public InheritedTypeMapper(
            Func<TSource, IMapperContext, TDiscriminator> getDiscriminator, 
            Dictionary<TDiscriminator, ILazyTypeMapper> mappers)
        {
            _getDiscriminator = getDiscriminator;
            _mappers = mappers;
        }   

        public override TTarget Map(TSource source, TTarget target, IMapperContext mapperContext)
        {
            TTarget result = null;

            if (source != null)
            {
                TDiscriminator discriminator = _getDiscriminator(source, mapperContext);

                if (_mappers.TryGetValue(discriminator, out ILazyTypeMapper mapper))
                {
                    result = (TTarget)mapper.Value.Map(source, target, mapperContext);
                }
                else
                {
                    throw new MapperException($"{discriminator} is not a valid value for {typeof(TTarget)} discriminator.");
                }
            }

            return result;
        }
    }
}