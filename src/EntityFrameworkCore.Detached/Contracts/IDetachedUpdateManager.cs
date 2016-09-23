using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Contracts
{
    public interface IDetachedUpdateManager
    {
        /// <summary>
        /// Merges a detached entity with a persisted entity.
        /// </summary>
        /// <param name="entityType">EntityType of the entities being merged.</param>
        /// <param name="newEntity">The detached entity.</param>
        /// <param name="dbEntity">The entity actually persisted in the database.</param>
        void Merge(EntityType entityType, object newEntity, object dbEntity);

        /// <summary>
        /// Adds a detached entity.
        /// </summary>
        /// <param name="entityType">The EntityType of the entity being added.</param>
        /// <param name="entity">The entity to add.</param>
        void Add(EntityType entityType, object entity);

        /// <summary>
        /// Deletes a database persisted entity.
        /// </summary>
        /// <param name="entityType">The EntityType of the entity being deleted.</param>
        /// <param name="entity">The entity to delete.</param>
        void Delete(EntityType entityType, object entity);

        /// <summary>
        /// Attaches a detached entity and set its state to Unchaged (for Associations).
        /// </summary>
        /// <param name="entityType">The EntityType being attached.</param>
        /// <param name="entity">The entity to attach.</param>
        void Attach(EntityType entityType, object entity);
    }
}
