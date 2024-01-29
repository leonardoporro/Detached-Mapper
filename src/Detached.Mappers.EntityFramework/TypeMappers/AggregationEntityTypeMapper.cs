using Detached.Mappers.TypeMappers.Entity;
using System;

namespace Detached.Mappers.EntityFramework.TypeMappers
{
    public class AggregationEntityTypeMapper<TSource, TTarget, TKey> : EntityTypeMapper<TSource, TTarget, TKey>
        where TTarget : class
        where TKey : IEntityKey
    {
        public AggregationEntityTypeMapper(
            Func<IMapContext, TTarget> construct,
            Func<TSource, IMapContext, TKey> getSourceKey,
            Func<TTarget, IMapContext, TKey> getTargetKey,
            Action<TSource, TTarget, IMapContext> mapKeyMembers,
            Action<TSource, TTarget, IMapContext> mapNoKeyMembers,
            string concurrencyTokenName)
            : base(construct, getSourceKey, getTargetKey, mapKeyMembers, mapNoKeyMembers, concurrencyTokenName)
        {
        }

        public override TTarget Map(TSource source, TTarget target, IMapContext mapContext)
        {
            if (source == null)
            {
                target = null;
            }
            else
            {
                TKey sourceKey = GetSourceKey(source, mapContext);

                if (target != null)
                {
                    TKey targetKey = GetTargetKey(target, mapContext);
                    if (!Equals(sourceKey, targetKey))
                    {
                        target = Attach(source, sourceKey, mapContext);
                    }
                }
                else
                {
                    target = Attach(source, sourceKey, mapContext);
                }
            }

            return target;
        }
    }
}