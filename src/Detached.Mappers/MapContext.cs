using Detached.Mappers.TypeMappers.Entity;
using System;
using System.Collections.Generic;

namespace Detached.Mappers
{
    public class MapContext : IMapContext
    {
        readonly LinkedList<(object, object)> _objectStack =
            new LinkedList<(object, object)>();

        public MapContext(MapParameters parameters = null)
        {
            Parameters = parameters ?? new MapParameters();
        }

        public MapParameters Parameters { get; }

        public virtual TEntity TrackChange<TEntity, TSource, TKey>(TEntity entity, TSource source, TKey key, MapperActionType actionType)
            where TEntity : class
            where TSource : class
            where TKey : IEntityKey
        {
            return entity;
        }

        public virtual void PushResult<TKeyOrSource, TTarget>(TKeyOrSource keyOrSource, TTarget target)
        {
            _objectStack.AddLast((keyOrSource, target));
        }

        public virtual void PopResult()
        {
            _objectStack.RemoveLast();
        }

        public virtual bool TryGetResult<TKeyOrSource, TTarget>(TKeyOrSource keyOrDto, out TTarget target)
            where TTarget : class
        {
            var entry = _objectStack.Last;
            while (entry != null && !Equals(entry.Value.Item1, keyOrDto))
            {
                entry = entry.Previous;
            }

            if (entry != null)
            {
                target = (TTarget)entry.Value.Item2;
                return true;
            }
            else
            {
                target = default;
                return false;
            }
        }

        public virtual bool TryGetParent<TTarget>(out TTarget target)
            where TTarget : class
        {
            Type targetType = typeof(TTarget);
            var entry = _objectStack.Last;
            while (entry != null 
                   && !(entry.Value.Item1 is EntityRef entityRef 
                        && !entityRef.Key.IsEmpty
                        && targetType.IsAssignableFrom(entityRef.ClrType)))
            {
                entry = entry.Previous;
            }

            if (entry != null)
            {
                target = (TTarget)entry.Value.Item2;
                return true;
            }
            else
            {
                target = null;
                return false;
            }
        }
    }
}