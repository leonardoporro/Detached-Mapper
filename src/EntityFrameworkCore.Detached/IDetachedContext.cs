using EntityFrameworkCore.Detached.Events;
using EntityFrameworkCore.Detached.Plugins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached
{
    /// <summary>
    /// Handles detached root entities. A root is an entity with their owned and associated
    /// children that works as a single unit.
    /// </summary>
    public interface IDetachedContext : IDisposable
    {
        /// <summary>
        /// Gets the events for this detached context.
        /// </summary>
        IEventManager Events { get; }

        IPluginManager Plugins { get; }

        DbContext DbContext { get; }

        /// <summary>
        /// Gets the IQueryable for the given entity with the includes needed for associated and 
        /// owned navigations.
        /// </summary>
        /// <typeparam name="TEntity">Entity Clr Type.</typeparam>
        /// <returns>IQueryable<TEntity> with the needed Includes.</returns>
        IQueryable<TEntity> GetBaseQuery<TEntity>() where TEntity : class;

        /// <summary>
        /// Loads a detached root entity by its key. A root is an entity with their owned and associated
        /// children that works as a single unit.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the root entity to load.</typeparam>
        /// <param name="keyValues">Entity key values.</param>
        /// <returns>A detached root entity with its associated and owned children.</returns>
        Task<TEntity> LoadAsync<TEntity>(params object[] key) where TEntity : class;

        /// <summary>
        /// Loads a detached root and allows query customization in the middle of the query creation.
        /// A root is an entity with their owned and associated children that works as a single unit.
        /// Filter, order and any other operation can be applied while maintaining Detached and plugins 
        /// compatibility.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the root entity to load.</typeparam>
        /// <param name="queryConfig">An action that customizes the query.</param>
        /// <returns>A list of detached root entities from the database with its associated and owned children.</returns>
        Task<List<TEntity>> LoadAsync<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryConfig)
            where TEntity : class;

        /// <summary>
        /// Loads a detached root and allows query customization in the middle of the query creation.
        /// A root is an entity with their owned and associated children that works as a single unit.
        /// Filter, order and any other operation can be applied while maintaining Detached and plugins 
        /// compatibility.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the root entity to load.</typeparam>
        /// <param name="queryConfig">An action that customizes the query.</param>
        /// <returns>A list of detached root entities from the database with its associated and owned children.</returns>
        Task<List<TResult>> LoadAsync<TEntity, TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> queryConfig)
            where TEntity : class
            where TResult : class;

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
        /// <param name="key">The key of the detached root entity to delete.</param>
        Task DeleteAsync<TEntity>(params object[] key) where TEntity : class;

        /// <summary>
        /// Asynchronously save the context changes to the db.
        /// It also disables automatic change tracking to prevent unwanted modifications.
        /// </summary>
        Task<int> SaveChangesAsync();
    }

    /// <summary>
    /// Handles detached root entities. A root is an entity with their owned and associated
    /// children that works as a single unit.
    /// </summary>
    public interface IDetachedContext<TDbContext> : IDetachedContext
        where TDbContext : DbContext
    {
        /// <summary>
        /// Gets the underlying DbContext instance.
        /// </summary>
        new TDbContext DbContext { get; }
    }
}
