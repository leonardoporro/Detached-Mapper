using System;

namespace Detached.Mappers.TypeMappers.Entity.Complex
{
    public class ComposedEntityTypeMapper<TSource, TTarget, TKey> : TypeMapper<TSource, TTarget>
        where TSource : class
        where TTarget : class
        where TKey : IEntityKey
    {
        readonly Action<TSource, TTarget, IMapContext> _mapKeyMembers;
        readonly Action<TSource, TTarget, IMapContext> _mapNoKeyMembers;
        readonly Func<IMapContext, TTarget> _construct;
        readonly Func<TTarget, IMapContext, TKey> _getTargetKey;
        readonly Func<TSource, IMapContext, TKey> _getSourceKey;

        public ComposedEntityTypeMapper(
            Func<IMapContext, TTarget> construct,
            Func<TSource, IMapContext, TKey> getSourceKey,
            Func<TTarget, IMapContext, TKey> getTargetKey,
            Action<TSource, TTarget, IMapContext> mapKeyMembers,
            Action<TSource, TTarget, IMapContext> mapNoKeyMembers)
        {
            _construct = construct;
            _getSourceKey = getSourceKey;
            _getTargetKey = getTargetKey;
            _mapKeyMembers = mapKeyMembers;
            _mapNoKeyMembers = mapNoKeyMembers;
        }

        public override TTarget Map(TSource source, TTarget target, IMapContext context)
        {
            if (source == null)
            {
                if (target != null)
                {
                    TKey sourceKey = _getTargetKey(target, context);
                    context.TrackChange(target, source, sourceKey, MapperActionType.Delete);
                    target = null;
                }
            }
            else
            {
                TKey sourceKey = _getSourceKey(source, context);
                EntityRef entityRef = new EntityRef(sourceKey, typeof(TTarget));

                if (context.TryGetResult(entityRef, out TTarget parent))
                {
                    target = parent;
                }
                else if (target == null)
                {
                    target = Create(source, context, sourceKey, entityRef);
                }
                else
                {
                    TKey targetKey = _getTargetKey(target, context);
                    if (Equals(sourceKey, targetKey))
                    {
                        target = context.TrackChange(target, source, targetKey, MapperActionType.Update);

                        context.PushResult(entityRef, target);
                        _mapNoKeyMembers(source, target, context);
                        context.PopResult();
                    }
                    else
                    {
                        context.TrackChange(target, source, targetKey, MapperActionType.Delete);

                        target = Create(source, context, sourceKey, entityRef);
                    }
                }
            }

            return target;
        }

        private TTarget Create(TSource source, IMapContext context, TKey sourceKey, EntityRef entityRef)
        {
            TTarget target = _construct(context);
            _mapKeyMembers(source, target, context);
            target = context.TrackChange(target, source, sourceKey, MapperActionType.Create);

            context.PushResult(entityRef, target);
            _mapNoKeyMembers(source, target, context);
            context.PopResult();
            return target;
        }
    }
}