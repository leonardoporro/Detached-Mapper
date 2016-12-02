using EntityFrameworkCore.Detached.Events;
using EntityFrameworkCore.Detached.Plugins;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached
{
    /// <summary>
    /// Handles Detached Entity Graphs.
    /// A Detached Entity Graph is an object graph that consists of a Root entity and its
    /// Associated and Owned entities, working as a single unit.
    /// </summary>
    public interface IDetachedContext : IDisposable
    {
        /// <summary>
        /// Gets the current IEventManager implementation.
        /// </summary>
        IEventManager Events { get; }

        /// <summary>
        /// Gets the current IPluginManager implementation.
        /// </summary>
        IPluginManager Plugins { get; }

        /// <summary>
        /// Gets the underlying DbContext instance.
        /// </summary>
        DbContext DbContext { get; }

        /// <summary>
        /// Returns a Detached Set for the given entity Clr type.
        /// </summary>
        /// <typeparam name="TEntity">Clr type of the entity.</typeparam>
        /// <returns>An implementation of IDetachedSet.</returns>
        IDetachedSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Returns a Detached Set for the given entity Clr type.
        /// </summary>
        /// <param name="type">The Clr type of the entity.</param>
        /// <returns>An implementation of IDetachedSet.</returns>
        IDetachedSet Set(Type type);

        /// <summary>
        /// Returns a Detached Set by the original DbContext property name.
        /// </summary>
        /// <param name="propertyName">The property name of the DbSet<> in the underlying DbContext.</param>
        /// <param name="throwIfNotFound">Whether to throw a detailed exception if the property was not found or is invalid.</param>
        /// <returns>An implementation of IDetachedSet.</returns>
        IDetachedSet Set(string propertyName, bool throwIfNotFound = true);

        /// <summary>
        /// Asynchronously save the context changes to the database.
        /// (This version of SaveChangesAsync disables automatic change tracking. Using SaveChangesAsync from 
        /// the underlying DbContext may lead to unexpected behaviour).
        /// </summary>
        Task<int> SaveChangesAsync(); 
    }

    /// <summary>
    /// Handles Detached Entity Graphs.
    /// A Detached Entity Graph is an object graph that consists of a Root entity and its
    /// Associated and Owned entities, working as a single unit.
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
