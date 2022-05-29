using System;

namespace Detached.Mappers.TypeMappers.Entity.Complex
{
    public class AggregatedEntityTypeMapper<TSource, TTarget, TKey> : TypeMapper<TSource, TTarget>
        where TSource : class
        where TTarget : class
        where TKey : IEntityKey
    {
        Func<TSource, IMapContext, TKey> _getSourceKey;
        Func<TTarget, IMapContext, TKey> _getTargetKey;
        Func<IMapContext, TTarget> _construct;
        Action<TSource, TTarget, IMapContext> _mapKeyMembers;

        public AggregatedEntityTypeMapper(
            Func<IMapContext, TTarget> construct,
            Func<TSource, IMapContext, TKey> getSourceKey,
            Func<TTarget, IMapContext, TKey> getTargetKey,
            Action<TSource, TTarget, IMapContext> mapKeyMembers)
        {
            _construct = construct;
            _getSourceKey = getSourceKey;
            _getTargetKey = getTargetKey;
            _mapKeyMembers = mapKeyMembers;
        }

        public override TTarget Map(TSource source, TTarget target, IMapContext context)
        {
            if (source == null)
            {
                target = null;
            }
            else 
            {
                TKey sourceKey = _getSourceKey(source, context);

                if (target != null)
                {
                    TKey targetKey = _getTargetKey(target, context);
                    if (!Equals(sourceKey, targetKey))
                    {
                        target = Attach(source, sourceKey, context);
                    }
                }
                else
                {
                    target = Attach(source, sourceKey, context);
                }
            }

            return target;
        }

        TTarget Attach(TSource source, TKey sourceKey, IMapContext context)
        {
            TTarget target = _construct(context);
            _mapKeyMembers(source, target, context);

            return context.TrackChange(target, source, sourceKey, MapperActionType.Attach);
        }
    }
}