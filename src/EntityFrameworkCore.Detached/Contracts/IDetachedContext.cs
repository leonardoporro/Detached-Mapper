using EntityFrameworkCore.Detached.DataAnnotations;
using EntityFrameworkCore.Detached.DataAnnotations.Paged;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Contracts
{
    /// <summary>
    /// Handles detached root entities. A root is an entity with their owned and associated
    /// children that works as a single unit.
    /// </summary>
    public interface IDetachedContext<TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        /// <summary>
        /// Gets the underlying DbContext instance.
        /// </summary>
        TDbContext DbContext { get; }

        /// <summary>
        /// Loads a detached root entity by its key. A root is an entity with their owned and associated
        /// children that works as a single unit.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the root entity to load.</typeparam>
        /// <param name="keyValues">Entity key values.</param>
        /// <returns>A detached root entity with its associated and owned children.</returns>
        Task<TEntity> LoadAsync<TEntity>(params object[] key) where TEntity : class;

        /// <summary>
        /// Loads a detached root entity by a filter. A root is an entity with their owned and associated
        /// children that works as a single unit.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the root entity to load.</typeparam>
        /// <param name="filter">Filter expression</param>
        /// <returns>A detached list of root entity with its associated and owned children.</returns>
        Task<List<TEntity>> LoadAsync<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class;

        /// <summary>
        /// Loads a detached root entity by a filter and projects it into the given TItem type. A root is an entity with their owned and associated
        /// children that works as a single unit.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the root entity to load.</typeparam>
        /// <param name="filter">Filter expression</param>
        /// <returns>A detached list of root entity with its associated and owned children.</returns>
        Task<List<TItem>> LoadAsync<TEntity, TItem>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TItem>> project) 
            where TEntity : class
            where TItem : class;

        /// <summary>
        /// Loads a given page along with query statistics (page number, rowcount, etc).
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the entity.</typeparam>
        /// <param name="request">Paged query request.</param>
        /// <returns>Paged result instance.</returns>
        Task<IPagedResult<TEntity>> LoadPageAsync<TEntity>(IPagedRequest<TEntity> request) where TEntity : class;

        /// <summary>
        /// Loads a given page along with query statistics (page number, rowcount, etc) and 
        /// projects the result to the given TItem type.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the entity.</typeparam>
        /// <param name="request">Paged query request.</param>
        /// <returns>Paged result instance.</returns>
        Task<IPagedResult<TItem>> LoadPageAsync<TEntity, TItem>(IPagedRequest<TEntity> request, Expression<Func<TEntity, TItem>> project)
            where TEntity : class
            where TItem : class;

        /// <summary>
        /// Saves a detached root entity. A root is an entity with their owned and associated
        /// children that works as a single unit.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the root entity to save.</typeparam>
        /// <param name="root">The detached root entity to save.</param>
        /// <returns>The saved root entity.</returns>
        Task<TEntity> UpdateAsync<TEntity>(TEntity root) where TEntity : class;

        /// <summary>
        /// Deletes a detached root entity with its owned children.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the root entity to delete.</typeparam>
        /// <param name="root">The detached root entity to delete.</param>
        Task DeleteAsync<TEntity>(TEntity root) where TEntity : class;

        /// <summary>
        /// Deletes a detached root entity with its owned children.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the root entity to delete.</typeparam>
        /// <param name="key">The key of the detached root entity to delete.</param>
        Task DeleteAsync<TEntity>(params object[] key) where TEntity : class;

        /// <summary>
        /// Asynchronously save the context changes to the db.
        /// It also disables automatic change tracking to prevent unwanted modifications.
        /// </summary>
        Task<int> SaveChangesAsync();
    }
}
