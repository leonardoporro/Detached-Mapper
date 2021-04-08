namespace Detached.Mappers
{
    public enum AggregationAction
    {
        /// <summary>
        /// Set entity status as Unchanged.
        /// </summary>
        Attach,
        /// <summary>
        /// Reload from the database, if exists, sets as Unchanged, otherwise, set as Added.
        /// </summary>
        Map
    }

    public class MapperParameters
    {
        /// <summary>
        /// If true, non-existing root entities are created; otherwise, an exception is thrown.
        /// </summary>
        public bool RootUpsert { get; set; } = true;

        /// <summary>
        /// Action to apply when an aggregated entity is processed.
        /// </summary>
        public AggregationAction AggregationAction { get; set; } = AggregationAction.Attach;
    }
}
