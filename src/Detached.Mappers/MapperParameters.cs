namespace Detached.Mappers
{
    public class MapperParameters
    {
        /// <summary>
        /// If true, non-existing root entities are created; otherwise, an exception is thrown.
        /// </summary>
        public bool RootUpsert { get; set; } = true;

        /// <summary>
        /// If true, non-existing aggregtions are created.
        /// </summary>
        public bool EnsureAggregations { get; set; } = false;
    }
}
