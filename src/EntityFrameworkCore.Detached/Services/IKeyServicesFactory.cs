using System;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityFrameworkCore.Detached.Services
{
    public interface IKeyServicesFactory
    {
        IKeyServices GetKeyServices(Type clrType);

        IKeyServices GetKeyServices(IEntityType entityType);

        IKeyServices<TEntity> GetKeyServices<TEntity>();
    }
}