using System;

namespace Detached.Mappers.TypeMappers.Entity.Complex
{
    public class RootEntityTypeMapper<TSource, TTarget, TKey> : TypeMapper<TSource, TTarget>
        where TSource : class
        where TTarget : class
        where TKey : IEntityKey
    {
        readonly Action<TSource, TTarget, IMapContext> _mapKeyMembers;
        readonly Action<TSource, TTarget, IMapContext> _mapNoKeyMembers;
        readonly Func<IMapContext, TTarget> _construct;
        readonly Func<TTarget, IMapContext, TKey> _getTargetKey;
        readonly Func<TSource, IMapContext, TKey> _getSourceKey;

        public RootEntityTypeMapper(
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
            TKey key = _getSourceKey(source, context);

            if (target == null)
            {
                target = context.TrackChange(target, source, key, MapperActionType.Load);
                if (target == null)
                {
                    target = _construct(context);
                    _mapKeyMembers(source, target, context);
                    target = context.TrackChange(target, source, key, MapperActionType.Create);
                }

                context.PushResult(new EntityRef(key, typeof(TTarget)), target);
                _mapNoKeyMembers(source, target, context);
                context.PopResult();
            }
            else
            {
                context.PushResult(new EntityRef(key, typeof(TTarget)), target);
                _mapNoKeyMembers(source, target, context); 
                context.PopResult();

                target = context.TrackChange(target, source, key, MapperActionType.Update);
            }

            return target;
        }
    }
}