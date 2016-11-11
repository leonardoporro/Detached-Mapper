using System;

namespace EntityFrameworkCore.Detached.Services
{
    public interface IKeyServicesCache
    {
        IKeyServices GetOrCreate(Type dbContextType, Type entityType, Func<IKeyServices> factory);
    }
}