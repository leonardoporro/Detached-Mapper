using System;

namespace Detached.Mappers.TypeMappers.Entity.Complex;

public class OwnedEntityTypeMapper<TSource, TTarget, TKey> : TypeMapper<TSource, TTarget>
    where TSource : class
    where TTarget : class
    where TKey : IEntityKey
{
    private readonly Func<IMapContext, TTarget> _construct;
    private readonly Func<TSource, IMapContext, TKey> _getSourceKey;
    private readonly Func<TTarget, IMapContext, TKey> _getTargetKey;
    private readonly Action<TSource, TTarget, IMapContext> _mapKeyMembers;
    private readonly Action<TSource, TTarget, IMapContext> _mapNoKeyMembers;

    public OwnedEntityTypeMapper(
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
            if (target == null) return target;
            var sourceKey = _getTargetKey(target, context);
            target = context.TrackChange(target, source, sourceKey, MapperActionType.Load);
        }
        else
        {
            var sourceKey = _getSourceKey(source, context);
            var entityRef = new EntityRef(sourceKey, typeof(TTarget));

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
                var targetKey = _getTargetKey(target, context);

                context.PushResult(entityRef, target);
                _mapNoKeyMembers(source, target, context);
                context.PopResult();

                target = context.TrackChange(target, source, targetKey, MapperActionType.Update);
            }
        }
        return target;
    }

    private TTarget Create(TSource source, IMapContext context, TKey sourceKey, EntityRef entityRef)
    {
        var target = _construct(context);
        _mapKeyMembers(source, target, context);
        target = context.TrackChange(target, source, sourceKey, MapperActionType.Create);

        context.PushResult(entityRef, target);
        _mapNoKeyMembers(source, target, context);
        context.PopResult();
        return target;
    }
}