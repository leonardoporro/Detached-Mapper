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
        Type _dbContextType;
        IKeyServicesCache _keyServicesCache;

        public KeyServicesFactory(DbContext dbContext, IKeyServicesCache keyServicesCache)
        {
            _dbContext = dbContext;
            _dbContextType = dbContext.GetType();
            _keyServicesCache = keyServicesCache;
        }

        public IKeyServices<TEntity> GetKeyServices<TEntity>()
        {
            return (IKeyServices<TEntity>)_keyServicesCache.GetOrCreate(_dbContext.GetType(), typeof(TEntity), () =>
            {
                IEntityType entityType = _dbContext.Model.FindEntityType(typeof(TEntity));
                return new KeyServices<TEntity>(entityType);
            });
        }

        public IKeyServices GetKeyServices(IEntityType entityType)
        {
            return _keyServicesCache.GetOrCreate(_dbContext.GetType(), entityType.ClrType, () =>
                {
                    return (IKeyServices)Activator.CreateInstance(typeof(KeyServices<>).MakeGenericType(entityType.ClrType), entityType);
                });
        }

        public IKeyServices GetKeyServices(Type clrType)
        {
            return _keyServicesCache.GetOrCreate(_dbContext.GetType(), clrType, () =>
                {
                    IEntityType entityType = _dbContext.Model.FindEntityType(clrType);
                    return (IKeyServices)Activator.CreateInstance(typeof(KeyServices<>).MakeGenericType(entityType.ClrType), entityType);
                });
        }
    }
}
