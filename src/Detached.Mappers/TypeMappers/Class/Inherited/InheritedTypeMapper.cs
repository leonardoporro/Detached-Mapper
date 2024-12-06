using Detached.Mappers.Context;
using Detached.Mappers.Exceptions;
using System;
using System.Collections.Generic;

namespace Detached.Mappers.TypeMappers.Class.Inherited
{
    public class InheritedTypeMapper<TSource, TTarget, TDiscriminator> : TypeMapper<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        readonly Func<TSource, IMapContext, TDiscriminator> _getDiscriminator;
        readonly Dictionary<TDiscriminator, ITypeMapper> _mappers;

        public InheritedTypeMapper(
            Func<TSource, IMapContext, TDiscriminator> getDiscriminator,
            Dictionary<TDiscriminator, ITypeMapper> mappers)
        {
            _getDiscriminator = getDiscriminator;
            _mappers = mappers;
        }

        public override TTarget Map(TSource source, TTarget target, IMapContext context)
        {
            TTarget result = null;

            if (source != null)
            {
                TDiscriminator discriminator = _getDiscriminator(source, context);

                if (_mappers.TryGetValue(discriminator, out ITypeMapper mapper))
                {
                    result = (TTarget)mapper.Map(source, target, context);
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