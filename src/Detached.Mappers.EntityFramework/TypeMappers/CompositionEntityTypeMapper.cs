using Detached.Mappers.TypeMappers.Entity;
using System;

namespace Detached.Mappers.EntityFramework.TypeMappers
{
    public class CompositionEntityTypeMapper<TSource, TTarget, TKey> : EntityTypeMapper<TSource, TTarget, TKey>
        where TSource : class
        where TTarget : class
        where TKey : IEntityKey
    {
        public CompositionEntityTypeMapper(
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
                if (target != null)
                {
                    TKey targetKey = GetTargetKey(target, mapContext);
                    target = Delete(source, target, targetKey, mapContext);
                }
            }
            else
            {
                TKey sourceKey = GetSourceKey(source, mapContext);
                EntityRef entityRef = new EntityRef(sourceKey, typeof(TTarget));

                if (mapContext.TryGetResult(entityRef, out TTarget parent))
                {
                    target = parent;
                }
                else if (target == null)
                {
                    target = Create(source, sourceKey, mapContext, entityRef);
                }
                else
                {
                    TKey targetKey = GetTargetKey(target, mapContext);

                    if (Equals(sourceKey, targetKey))
                    {
                        target = Merge(source, target, targetKey, mapContext, entityRef);
                    }
                    else
                    {
                        target = Replace(source, mapContext, sourceKey, entityRef);
                    }
                }
            }

            return target;
        }
    }
}