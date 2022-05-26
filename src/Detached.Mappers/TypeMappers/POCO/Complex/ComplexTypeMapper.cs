using Detached.Mappers.Context;
using System;

namespace Detached.Mappers.TypeMappers.POCO.Complex
{
    public class ComplexTypeMapper<TSource, TTarget> : TypeMapper<TSource, TTarget>
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

        public override TTarget Map(TSource source, TTarget target, IMapperContext mapperContext)
        {
            if (Equals(source, null))
            {
                return default;
            }
            else if (mapperContext.TryGetTrackedObject(source, out TTarget trackedTarget))
            {
                return trackedTarget;
            }
            else
            {
                if (Equals(target, null))
                {
                    target = _construct(mapperContext);
                }

                mapperContext.TrackObject(source, target);


                _mapMembers(source, target, mapperContext);

                return target;
            }
        }
    }
}
