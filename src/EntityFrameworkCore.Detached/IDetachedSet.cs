using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached
{
    public interface IDetachedSet<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Returns an queryable that can be used to fetch Detached Entity Graphs from the database. 
        /// A Detached Entity Graph is an object graph that consists of a Root entity and its
        /// Associated and Owned entities, working as a single unit.
        /// </summary>
        /// <returns>IQueryable<TEntity> instance that can be used to fetch Detached Entity Graphs.</returns>
        IQueryable<TEntity> GetBaseQuery();

        /// <summary>
        /// Loads a Detached Entity Graph whose Root has the given key value.
        /// A Detached Entity Graph is an object graph that consists of a Root entity and its
        /// Associated and Owned entities, working as a single unit.
        /// </summary>
        /// <param name="keyValues">Entity key values.</param>
        /// <returns>A detached root entity with its associated and owned children.</returns>
        Task<TEntity> LoadAsync(params object[] key);

        /// <summary>
        /// Loads a Detached Entity Graph allowing the base query to be customized by the given
        /// delegate.
        /// A Detached Entity Graph is an object graph that consists of a Root entity and its
        /// Associated and Owned entities, working as a single unit.
        /// </summary>
        /// <param name="queryConfig">An Action that customizes the query.</param>
        /// <returns>A list of Detached Entity Graphs.</returns>
        Task<List<TEntity>> LoadAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryConfig);

        /// <summary>
        /// Loads a Projection of a Detached Entity Graph. The base query must be customized by the
        /// specified action.
        /// A Detached Entity Graph is an object graph that consists of a Root entity and its
        /// Associated and Owned entities, working as a single unit.
        /// The Projection is one or more fields from the Detached Graph.
        /// </summary>
        /// <param name="queryConfig">An action that customizes the query.</param>
        /// <returns>A list of items of TResult type.</returns>
        Task<List<TResult>> LoadAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> queryConfig)
            where TResult : class;

        /// <summary>
        /// Updates the given Detached Entity Graph. 
        /// A Detached Entity Graph is an object graph that consists of a Root entity and its
        /// Associated and Owned entities, working as a single unit.
        /// </summary>
        /// <param name="root">The Root of the Detached Entity Graph.</param>
        /// <returns>The Root of the Detached Entity Graph after being saved 
        /// (with Plugins processing and DB generated values, if any).</returns>
        Task<TEntity> UpdateAsync(TEntity root);

        /// <summary>
        /// Deletes a Detached Entity Graph by its Root key.
        /// A Detached Entity Graph is an object graph that consist of a Root entity and its
        /// Associated and Owned entities, working as a single unit.
        /// </summary>
        /// <param name="key">The key of the detached root entity to delete.</param>
        Task DeleteAsync(params object[] key);
    }

    public interface IDetachedSet
    {
        /// <summary>
        /// Returns an queryable that can be used to fetch Detached Entity Graphs from the database. 
        /// A Detached Entity Graph is an object graph that consists of a Root entity and its
        /// Associated and Owned entities, working as a single unit.
        /// </summary>
        /// <returns>IQueryable<TEntity> instance that can be used to fetch Detached Entity Graphs.</returns>
        IQueryable GetBaseQuery();

        /// <summary>
        /// Loads a Detached Entity Graph whose Root has the given key value.
        /// A Detached Entity Graph is an object graph that consists of a Root entity and its
        /// Associated and Owned entities, working as a single unit.
        /// </summary>
        /// <param name="keyValues">Entity key values.</param>
        /// <returns>A detached root entity with its associated and owned children.</returns>
        Task<object> LoadAsync(params object[] key);

        /// <summary>
        /// Updates the given Detached Entity Graph. 
        /// A Detached Entity Graph is an object graph that consists of a Root entity and its
        /// Associated and Owned entities, working as a single unit.
        /// </summary>
        /// <param name="root">The Root of the Detached Entity Graph.</param>
        /// <returns>The Root of the Detached Entity Graph after being saved 
        /// (with Plugins processing and DB generated values, if any).</returns>
        Task<object> UpdateAsync(object root);

        /// <summary>
        /// Deletes a Detached Entity Graph by its Root key.
        /// A Detached Entity Graph is an object graph that consist of a Root entity and its
        /// Associated and Owned entities, working as a single unit.
        /// </summary>
        /// <param name="key">The key of the detached root entity to delete.</param>
        Task DeleteAsync(params object[] key);

        /// <summary>
        /// Gets the Clr Type of the entities in this set.
        /// </summary>
        Type EntityType { get; }
    }
}
