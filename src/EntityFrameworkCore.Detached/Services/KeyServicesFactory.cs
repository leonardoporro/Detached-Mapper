using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Services
{
    public class KeyServicesFactory : IKeyServicesFactory
    {
        DbContext _dbContext;

        public KeyServicesFactory(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IKeyServices<TEntity> GetKeyServices<TEntity>()
        {
            IEntityType entityType = _dbContext.Model.FindEntityType(typeof(TEntity));
            return new KeyServices<TEntity>(entityType);
        }

        public IKeyServices GetKeyServices(IEntityType entityType)
        {
            return  CreateKeyServices(entityType);
        }

        public IKeyServices GetKeyServices(Type clrType)
        {
            return CreateKeyServices(clrType);
        }

        public IKeyServices CreateKeyServices(IEntityType entityType)
        {
            return (IKeyServices)Activator.CreateInstance(typeof(KeyServices<>).MakeGenericType(entityType.ClrType), entityType);
        }

        public IKeyServices CreateKeyServices(Type clrType)
        {
            IEntityType entityType = _dbContext.Model.FindEntityType(clrType);
            return (IKeyServices)Activator.CreateInstance(typeof(KeyServices<>).MakeGenericType(entityType.ClrType), entityType);
        }
    }
}
