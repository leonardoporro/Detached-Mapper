using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityFrameworkCore.Detached.Services
{
    /// <summary>
    /// Provides services for finding local (attached) entries for a specified entity object.
    /// </summary>
    public interface IEntryFinder
    {
        /// <summary>
        /// Returns the local EntityEntry for the given entity object.
        /// (It doesn't query the database, if not found).
        /// </summary>
        /// <param name="entity">The entity object.-</param>
        /// <returns>The EntityEntry associated to the given object, or null if not found.</returns>
        EntityEntry FindEntry(object entity);
    }
}