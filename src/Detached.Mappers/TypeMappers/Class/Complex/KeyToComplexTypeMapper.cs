using System;
using Detached.Mappers.Context;

namespace Detached.Mappers.TypeMappers.Class.Complex
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