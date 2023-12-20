using System;

namespace Detached.Mappers.TypeMappers.Entity.Complex
{
    public class RootEntityTypeMapper<TSource, TTarget, TKey> : EntityTypeMapper<TSource, TTarget, TKey>
        where TSource : class
        where TTarget : class
        where TKey : IEntityKey
    {
        public RootEntityTypeMapper(
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
            TKey key = GetSourceKey(source, context);
            var entityRef = new EntityRef(key, typeof(TTarget));

            if (target == null)
            {
                target = context.TrackChange(target, source, key, MapperActionType.Load);
                if (target == null)
                {
                    target = Create(source, key, context, entityRef);
                }
                else
                {
                    target = Merge(source, target, key, context, entityRef);
                }
            }
            else
            {
                target = Merge(source, target, key, context, entityRef);
            }

            return target;
        }
    }
}