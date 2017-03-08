using System;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Detached.EntityFramework.Services
{
    /// <summary>
    /// Builds IEntityServices instances for the given entities.
    /// </summary>
    public interface IEntityServicesFactory
    {
        /// <summary>
        /// Gets an IEntityServices implementation for the given Clr type.
        /// </summary>
        /// <param name="clrType">The Crl type of the entity whose key services are needed.</param>
        /// <returns>An instance of IentityServices for the given entity.</returns>
        IEntityServices GetEntityServices(Type clrType);

        /// <summary>
        /// Gets an IEntityServices implementation for the given entity type.
        /// </summary>
        /// <param name="entityType">The entity type whose key services are requested.</param>
        /// <returns>An instance of IentityServices for the given entity.</returns>
        IEntityServices GetEntityServices(IEntityType entityType);

        /// <summary>
        /// Gets an IEntityServices implementation for the given entity type. 
        /// </summary>
        /// <typeparam name="TEntity">The Clr type of the entity whose key services are requested.</typeparam>
        /// <returns>An instance of IentityServices for the given entity.</returns>
        IEntityServices<TEntity> GetEntityServices<TEntity>();
    }
}