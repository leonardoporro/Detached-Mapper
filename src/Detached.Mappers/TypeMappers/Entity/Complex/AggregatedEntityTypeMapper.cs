using System;
using Detached.Mappers.Context;

namespace Detached.Mappers.TypeMappers.Entity.Complex
{
    public class AggregatedEntityTypeMapper<TSource, TTarget, TKey> : EntityTypeMapper<TSource, TTarget, TKey>
        where TSource : class
        where TTarget : class
        where TKey : IEntityKey
    {
        public AggregatedEntityTypeMapper(
            Func<IMapContext, TTarget> construct,
            Func<TSource, IMapContext, TKey> getSourceKey,
            Func<TTarget, IMapContext, TKey> getTargetKey,
            Action<TSource, TTarget, IMapContext> mapKeyMembers,
            Action<TSource, TTarget, IMapContext> mapNoKeyMembers)
            : base(construct, getSourceKey, getTargetKey, mapKeyMembers, mapNoKeyMembers)
        {
        }

        public override TTarget Map(TSource source, TTarget target, IMapContext context)
        {
            if (source == null)
            {
                target = null;
            }
            else
            {
                TKey sourceKey = GetSourceKey(source, context);

                if (target != null)
                {
                    TKey targetKey = GetTargetKey(target, context);
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
    }
}