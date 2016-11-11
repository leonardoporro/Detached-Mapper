using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityFrameworkCore.Detached.Services
{
    public interface IEntryFinder
    {
        EntityEntry FindEntry(object entity);
    }
}