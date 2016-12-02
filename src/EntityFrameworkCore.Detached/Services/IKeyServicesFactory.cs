using System;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityFrameworkCore.Detached.Services
{
    /// <summary>
    /// Builds IKeyServices instances for the given entities.
    /// </summary>
    public interface IKeyServicesFactory
    {
        /// <summary>
        /// Gets an IKeyServices implementation for the given Clr type.
        /// </summary>
        /// <param name="clrType">The Crl type of the entity whose key services are needed.</param>
        /// <returns>An instance of IKeyServices for the given entity.</returns>
        IKeyServices GetKeyServices(Type clrType);

        /// <summary>
        /// Gets an IKeyServices implementation for the given entity type.
        /// </summary>
        /// <param name="entityType">The entity type whose key services are requested.</param>
        /// <returns>An instance of IKeyServices for the given entity.</returns>
        IKeyServices GetKeyServices(IEntityType entityType);

        /// <summary>
        /// Gets an IKeyServices implementation for the given entity type. 
        /// </summary>
        /// <typeparam name="TEntity">The Clr type of the entity whose key services are requested.</typeparam>
        /// <returns>An instance of IKeyServices for the given entity.</returns>
        IKeyServices<TEntity> GetKeyServices<TEntity>();
    }
}