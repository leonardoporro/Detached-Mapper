using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Detached.EntityFramework.Services
{
    /// <summary>
    /// Provides services for working with entities.
    /// </summary>
    /// <typeparam name="TEntity">Clr type of the entity.</typeparam>
    public interface IEntityServices<TEntity> : IEntityServices
    {
        /// <summary>
        /// Creates an expression that compares the entity key with the given values.
        /// Useful to search for entities by key.
        /// </summary>
        /// <param name="keyValues">The value(s) of the entity key being searched.</param>
        /// <returns></returns>
        Expression<Func<TEntity, bool>> CreateEqualityExpression(object[] keyValues);
    }

    /// <summary>
    /// Provides services for working with entities.
    /// </summary>
    public interface IEntityServices
    {
        /// <summary>
        /// Compares two entities by their key.
        /// </summary>
        /// <param name="entityA">An entity to compare.</param>
        /// <param name="entityB">An entity to compare.</param>
        /// <returns>True, if the keys of both entities are equal; otherwise, false.</returns>
        bool Equal(object entityA, object entityB);

        /// <summary>
        /// Returns the key values for the given entity.
        /// </summary>
        /// <param name="entity">The entity whose key is wanted.</param>
        /// <returns>object[] containing key value(s).</returns>
        object[] GetKeyValues(object entity);

        /// <summary>
        /// Gets the underlying key object.
        /// </summary>
        /// <returns>An EF Core IKey implementation for the current EntityType.</returns>
        IKey GetKey();

        /// <summary>
        /// Creates a Key -> Value table for the given enumeration of entities.
        /// </summary>
        /// <param name="instances">Enumeration of entities to convert to a table.</param>
        /// <returns>A dictionary where the key is the entity key, and the value is the entity itself.</returns>
        Dictionary<object[], object> CreateTable(IEnumerable instances);
    }
}