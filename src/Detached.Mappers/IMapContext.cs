using Detached.Mappers.TypeMappers.Entity;

namespace Detached.Mappers
{
    public enum MapperActionType { Load, Create, Update, Delete, Attach }

    public interface IMapContext
    {
        MapParameters Parameters { get; }

        TTarget TrackChange<TTarget, TSource, TKey>(TTarget entity, TSource source, TKey key, MapperActionType actionType)
            where TTarget : class
            where TSource : class
            where TKey : IEntityKey;

        void PushResult<TKeyOrSource, TTarget>(TKeyOrSource keyOrSource, TTarget target);

        void PopResult();

        bool TryGetResult<TKeyOrSource, TTarget>(TKeyOrSource keyOrSource, out TTarget target)
            where TTarget : class;

        bool TryGetParent<TTarget>(out TTarget target)
            where TTarget : class;
    }
}