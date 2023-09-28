using Detached.Mappers.TypeMappers.Entity;
using System;

namespace Detached.Mappers.EntityFramework.TypeMappers
{
    public class AggregationEntityTypeMapper<TSource, TTarget, TKey> : EntityTypeMapper<TSource, TTarget, TKey>
        where TSource : class
        where TTarget : class
        where TKey : IEntityKey
    {
        public AggregationEntityTypeMapper(
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