using Detached.Mappers.Context;
using System;

namespace Detached.Mappers.TypeMappers.ComplexType
{
    public class ComplexTypeMapper<TSource, TTarget> : ITypeMapper<TSource, TTarget>
    {
        readonly Func<IMapperContext, TTarget> _construct;
        readonly Action<TSource, TTarget, IMapperContext> _mapMembers;

        public ComplexTypeMapper(
            Func<IMapperContext, TTarget> construct,
            Action<TSource, TTarget, IMapperContext> mapMembers)
        {
            _construct = construct;
            _mapMembers = mapMembers;
        }

        public TTarget Map(TSource source, TTarget target, IMapperContext mapperContext)
        {
            if (Equals(source, null))
            {
                return default;
            }
            else
            {
                if (Equals(target, null))
                {
                    target = _construct(mapperContext);
                }
            }

            _mapMembers(source, target, mapperContext);
            
            return target;
        }

        public object Map(object source, object target, IMapperContext mapperContext)
        {
            return Map((TSource)source, (TTarget)target, mapperContext);
        }
    }
}
