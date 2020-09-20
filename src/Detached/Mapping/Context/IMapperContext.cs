namespace Detached.Mapping.Context
{
    public enum MapperActionType { Load, Create, Update, Delete, Attach }

    public interface IMapperContext
    {
        MappingOptions MappingOptions { get; }

        TTarget OnMapperAction<TTarget, TSource, TKey>(TTarget entity, TSource source, TKey key, MapperActionType actionType)
            where TTarget : class
            where TSource : class
            where TKey : IEntityKey;
    }
}