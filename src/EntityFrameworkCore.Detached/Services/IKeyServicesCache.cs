using System;

namespace EntityFrameworkCore.Detached.Services
{
    /// <summary>
    /// Provides cache for IKeyServices implementations.
    /// </summary>
    public interface IKeyServicesCache
    {
        /// <summary>
        /// Gets or creates an instance of IKeyServices for the given DbContext and entity Clr Type.
        /// </summary>
        /// <param name="dbContextType">The Crl type of the current DbContext.</param>
        /// <param name="entityType">The Clr type of the entity whose key services are requested.</param>
        /// <param name="factory">A delegate that creates an IKeyServices instance if needed.</param>
        /// <returns>An IKeyServices instance for the given entity type.</returns>
        IKeyServices GetOrCreate(Type dbContextType, Type entityType, Func<IKeyServices> factory);
    }
}