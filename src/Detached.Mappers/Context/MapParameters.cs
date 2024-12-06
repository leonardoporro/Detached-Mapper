namespace Detached.Mappers.Context
{
    /// <summary>
    /// Expected behavior if the root entity being updated doesn't exist in the database.
    /// </summary>
    public enum MissingRootBehavior
    {
        /// <summary>
        /// The missing root is created, so Maps behave as an Upsert.
        /// </summary>
        Create,
        /// <summary>
        /// A MapperException is thrown.
        /// </summary>
        ThrowException
    }

    /// <summary>
    /// Expected behavior if an aggregated entity is assigned and that entity doesn't exist.
    /// </summary>
    public enum MissingAggregationBehavior
    {
        /// <summary>
        /// Create the aggregation as a part of the current mapping operator.
        /// </summary>
        Create,
        /// <summary>
        /// A SQL exception of a misisng FK will be thrown.
        /// </summary>
        ThrowException
    }

    /// <summary>
    /// Expected behavior if a composed entity is assigned and that entity already exists as
    /// an individual entity or as a part of another composition.
    /// </summary>
    public enum ExistingCompositionBehavior
    {
        /// <summary>
        /// Associate the existing entity to the mapping entity.
        /// </summary>
        Associate,
        /// <summary>
        /// Throw a database exception about a duplicate entity.
        /// </summary>
        ThrowException
    }

    /// <summary>
    /// Expected behavior when a collection marked as composition is mapped. 
    /// </summary>
    public enum CompositeCollectionBehavior
    {
        /// <summary>
        /// Existing entities in source are appended, missing are deleted.
        /// </summary>
        Merge,
        /// <summary>
        /// Existing entities in source are appendend but missing are not deleted.
        /// </summary>
        Append
    }

    public class MapParameters
    {
        /// <summary>
        /// Expected behavior if the root entity being updated doesn't exist in the database.
        /// Default: Create. It works as an Upsert.
        /// </summary>
        public MissingRootBehavior MissingRootBehavior { get; set; } = MissingRootBehavior.Create;

        /// <summary>
        /// Expected behavior if an aggregated entity is assigned and that entity doesn't exist.
        /// Aggregations are the boundaries of the graph and are expected to exist before the current map
        /// operation and be managed in a different map operations.
        /// This flag allows to create missing aggregations for internal process like Import Json to 
        /// avoid the need to sort the dependencies and ensure the aggregation is mapped first.
        /// Default: Throw. Create is not recommended as it consumes resources and breaks the expected behavior of the mapper.
        /// </summary>
        public MissingAggregationBehavior MissingAggregationBehavior { get; set; } = MissingAggregationBehavior.ThrowException;

        /// <summary>
        /// Expected behavior if a composed entity is assigned and that entity already exists as
        /// an individual entity or as a part of another composition.
        /// When a root and a composed entity are mapped, the composed entity is part of the graph and are not expected
        /// to exist as a part of another graph, like aggregations.
        /// If it exists, a duplicated entity exception is thrown, unless this flag is set to Associate.
        /// Default: Throw. Set it as Create if your use case really needs to share compositions, which is rare.
        /// </summary>
        public ExistingCompositionBehavior ExistingCompositionBehavior { get; set; } = ExistingCompositionBehavior.ThrowException;

        /// <summary>
        /// When a collection marked as Composition is mapped, entities that exist only in source are created, entities
        /// only in target are deleted and in both are merged.
        /// Setting this behavior to Append would prevent non specified entities for being deleted, allowing to add 
        /// entities to the collection without having to specify the existing ones.
        /// Default: Merge. Use Append only in a controlled context, otherwise it could end in tons of duplicated entities.
        /// </summary>
        public CompositeCollectionBehavior CompositeCollectionBehavior { get; set; } = CompositeCollectionBehavior.Merge;
    }
}
