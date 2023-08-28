namespace Detached.Mappers.TypeMappers.Entity.Collection
{
    /// <summary>
    /// EntityCollectionTypeMapper mapping behaviour on null collections
    /// </summary>
    public enum EntityCollectionNullBehavior
    {
        /// <summary>
        /// Set target to empty collection if source is null
        /// </summary>
        SetEmpty,
        /// <summary>
        /// Leave target value as-is if source is null
        /// </summary>
        Ignore
    }
}
