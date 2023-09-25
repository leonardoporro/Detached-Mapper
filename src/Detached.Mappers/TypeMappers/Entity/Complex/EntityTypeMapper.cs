using System;

namespace Detached.Mappers.TypeMappers.Entity.Complex
{
    public abstract class EntityTypeMapper<TSource, TTarget, TKey> : TypeMapper<TSource, TTarget>
        where TSource : class
        where TTarget : class
        where TKey : IEntityKey
    {
        protected EntityTypeMapper(
            Func<IMapContext, TTarget> construct,
            Func<TSource, IMapContext, TKey> getSourceKey,
            Func<TTarget, IMapContext, TKey> getTargetKey,
            Action<TSource, TTarget, IMapContext> mapKeyMembers,
            Action<TSource, TTarget, IMapContext> mapNoKeyMembers)
        {
            Construct = construct;
            GetSourceKey = getSourceKey;
            GetTargetKey = getTargetKey;
            MapKeyMembers = mapKeyMembers;
            MapMembers = mapNoKeyMembers;
        }

        protected Action<TSource, TTarget, IMapContext> MapKeyMembers { get; }

        protected Action<TSource, TTarget, IMapContext> MapMembers { get; }

        protected Func<IMapContext, TTarget> Construct { get; }

        protected Func<TTarget, IMapContext, TKey> GetTargetKey { get; }

        protected Func<TSource, IMapContext, TKey> GetSourceKey { get; }
         
        protected virtual TTarget Create(TSource source, TKey key, IMapContext context, EntityRef entityRef)
        {
            TTarget target = Construct(context);
            MapKeyMembers(source, target, context);
            target = context.TrackChange(target, source, key, MapperActionType.Create);

            context.PushResult(entityRef, target);
            MapMembers(source, target, context);
            context.PopResult();
            return target;
        }

        protected virtual TTarget Merge(TSource source, TTarget target, TKey key, IMapContext context, EntityRef entityRef)
        {
            context.PushResult(entityRef, target);
            MapMembers(source, target, context);
            context.PopResult();

            return context.TrackChange(target, source, key, MapperActionType.Update);
        }

        protected virtual TTarget Replace(TSource source, IMapContext context, TKey sourceKey, EntityRef entityRef, TKey targetKey)
        {
            TTarget target = Create(source, sourceKey, context, entityRef);
            context.TrackChange(target, source, targetKey, MapperActionType.Delete);
            return target;
        }

        protected virtual TTarget Delete(TTarget target, TKey key, IMapContext context)
        {
            context.TrackChange<TTarget, TSource, TKey>(target, null, key, MapperActionType.Delete);
            return null;
        }

        protected virtual TTarget Attach(TSource source, TKey sourceKey, IMapContext context)
        {
            TTarget target = Construct(context);
            MapKeyMembers(source, target, context);
            MapMembers(source, target, context); // Only primitives, like Name, Description, etc.

            return context.TrackChange(target, source, sourceKey, MapperActionType.Attach);
        }
    }
}