using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached
{
    public interface IQueryManager
    {
        /// <summary>
        /// Gets a query to find a root entity by its key.
        /// </summary>
        /// <typeparam name="TEntity">The clr type of the entity.</typeparam>
        /// <param name="keyValues">One or more values for the entity key to find.</param>
        /// <returns>IQueryable filtered by key.</returns>
        Task<TEntity> FindEntityByKey<TEntity>(EntityType entityType, object[] keyValues) where TEntity : class;

        /// <summary>
        /// Gets a query to find root entities filtered by the given expression.
        /// </summary>
        /// <typeparam name="TEntity">The clr type of the entity.</typeparam>
        /// <param name="filter">The expression to filter the entities.</param>
        /// <returns>IQueryable filtered by the given expression.</returns>
        Task<List<TEntity>> FindEntities<TEntity>(EntityType entityType, Expression<Func<TEntity, bool>> filter) where TEntity : class;

        /// <summary>
        /// Gets a query to find entities by key values.
        /// </summary>
        /// <typeparam name="TEntity">The clr type of the entity.</typeparam>
        /// <param name="keyValues">One or more values for the entity key to find.</param>
        /// <returns>IQueryable filtered by key.</returns>
        Task<TEntity> FindPersistedEntity<TEntity>(EntityType entityType, object detachedEntity) where TEntity : class;
    }
}
