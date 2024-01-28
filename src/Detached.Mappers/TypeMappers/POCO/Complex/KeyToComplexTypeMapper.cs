using Detached.Mappers.TypeMappers;
using System;

namespace Detached.Mappers.TypeMappers.POCO.Complex
{
    public class KeyToComplexTypeMapper<TSource, TTarget> : TypeMapper<TSource, TTarget>
        where TTarget : class
    {
        readonly Func<IMapContext, TTarget> _construct;
        readonly Action<TSource, TTarget, IMapContext> _setKey;

        public KeyToComplexTypeMapper(
            Func<IMapContext, TTarget> construct,
            Action<TSource, TTarget, IMapContext> set)
        {
            _construct = construct;
            _setKey = set;
        }

        public override TTarget Map(TSource source, TTarget target, IMapContext mapContext)
        {
            if (target == null)
            {
                target = _construct(mapContext);
            }

            _setKey(source, target, mapContext);

            return target;
        }
    }
}