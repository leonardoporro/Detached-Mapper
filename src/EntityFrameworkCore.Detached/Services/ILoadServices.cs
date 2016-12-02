using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityFrameworkCore.Detached.Services
{
    /// <summary>
    /// Loads Detached Entity Graphs for a detached context.
    /// A Detached Entity Graph is an object graph that consists of a Root entity and its
    /// Associated and Owned entities, working as a single unit.
    /// </summary>
    public interface ILoadServices
    {
        /// <summary>
        /// Returns an queryable that can be used to fetch Detached Entity Graphs from the database. 
        /// A Detached Entity Graph is an object graph that consists of a Root entity and its
        /// Associated and Owned entities, working as a single unit.
        /// </summary>
        /// <typeparam name="TEntity">Clr Type of the Root entity.</typeparam>
        /// <returns>IQueryable<TEntity> instance that can be used to fetch Detached Entity Graphs.</returns>
        IQueryable<TEntity> GetBaseQuery<TEntity>() where TEntity : class;

        /// <summary>
        /// Gets the property paths that must be included (joined) in order to get a Detached Entity Graph. 
        /// </summary>
        /// <param name="entityType">The Crl type of the entity whose navigations are going to be inspected.</param>
        /// <param name="visited">A hash table of the visited types, to avoid cycles.</param>
        /// <param name="currentPath">The current path being built (this is a recursive method).</param>
        /// <param name="paths">The resulting list of paths.</param>
        void GetIncludePaths(IEntityType entityType, HashSet<IEntityType> visited, string currentPath, ref List<string> paths);

        /// <summary>
        /// Loads a Detached Entity Graph whose Root has the given key value.
        /// A Detached Entity Graph is an object graph that consists of a Root entity and its
        /// Associated and Owned entities, working as a single unit.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the Root entity.</typeparam>
        /// <param name="keyValues">Entity key values.</param>
        /// <returns>A detached root entity with its associated and owned children.</returns>
        Task<TEntity> LoadAsync<TEntity>(params object[] keyValues) where TEntity : class;

        /// <summary>
        /// Loads a Detached Entity Graph allowing the base query to be customized by the given
        /// delegate.
        /// A Detached Entity Graph is an object graph that consists of a Root entity and its
        /// Associated and Owned entities, working as a single unit.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the Root entity.</typeparam>
        /// <param name="queryConfig">An Action that customizes the query.</param>
        /// <returns>A list of Detached Entity Graphs.</returns>
        Task<List<TEntity>> LoadAsync<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryConfig) where TEntity : class;

        /// <summary>
        /// Loads a Projection of a Detached Entity Graph. The base query must be customized by the
        /// specified action.
        /// A Detached Entity Graph is an object graph that consists of a Root entity and its
        /// Associated and Owned entities, working as a single unit.
        /// The Projection is one or more fields from the Detached Graph.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the Root entity.</typeparam>
        /// <param name="queryConfig">An action that customizes the query.</param>
        /// <returns>A list of items of TResult type.</returns>
        Task<List<TResult>> LoadAsync<TEntity, TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> queryConfig)
            where TEntity : class
            where TResult : class;

        /// <summary>
        /// Loads the persisted (in database) version for the given entity.
        /// </summary>
        /// <typeparam name="TEntity">Clr type for the entity.</typeparam>
        /// <param name="entity">Entity whose persisted version is going to be loaded.</param>
        /// <returns>The attached and persisted version of the given entity, or null if not found.</returns>
        Task<TEntity> LoadPersisted<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Loads the persisted (in database) version for the given entity.
        /// </summary>
        /// <typeparam name="TEntity">Clr type for the entity.</typeparam>
        /// <param name="keyValues">Entity key value(s).</param>
        /// <returns>The attached and persisted version of the given entity, or null if not found.</returns>
        Task<TEntity> LoadPersisted<TEntity>(object[] keyValues) where TEntity : class;
    }
}