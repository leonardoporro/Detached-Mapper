namespace Detached.Mappers
{
    public class MapParameters
    {
        /// <summary>
        /// If true, when root entity is not found, then it is created; otherwise, an exception is thrown.
        /// </summary>
        public bool Upsert { get; set; } = true;

        /// <summary>
        /// If true, when an aggregation does not exist, it is created; otherwise, Entity Framework throws an FK exception
        /// when saving.
        /// This option is used by Json import, to ensure that aggregations always exist, disregarding the order in what
        /// entities are saved.
        /// </summary>
        public bool AddAggregations { get; set; } = false;

        /// <summary>
        /// When an entity does not exist, before creating a new one, a query is made to the DB to look for an existing one.
        /// If it exists, an asociation to the existing one is made; otherwise, a new entity is created.
        /// This option fixes the "duplicated key" error when an existing entity that exists separately is added to a composition.
        /// Default: false.  
        /// </summary>
        public bool AssociateExistingCompositions { get; set; } = false;
    }
}
