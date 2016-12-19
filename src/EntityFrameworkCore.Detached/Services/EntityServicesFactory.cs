using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;

namespace EntityFrameworkCore.Detached.Services
{
    public class EntityServicesFactory : IEntityServicesFactory
    {
        DbContext _dbContext;
        Type _dbContextType;

        Dictionary<Type, IEntityServices> cache = new Dictionary<Type, IEntityServices>();


        public EntityServicesFactory(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContextType = dbContext.GetType();
        }

        public IEntityServices<TEntity> GetEntityServices<TEntity>()
        {
            IEntityServices result;
            Type clrType = typeof(TEntity);

            if (!cache.TryGetValue(clrType, out result))
            { 
                IEntityType entityType = _dbContext.Model.FindEntityType(clrType);
                result = new KeyServices<TEntity>(entityType);
                cache[clrType] = result;
            }
            return (IEntityServices<TEntity>)result;
        } 

        public IEntityServices GetEntityServices(IEntityType entityType)
        {
            IEntityServices result;
            Type clrType = entityType.ClrType;

            if (!cache.TryGetValue(clrType, out result))
            {
                result = (IEntityServices)Activator.CreateInstance(typeof(KeyServices<>).MakeGenericType(entityType.ClrType), entityType);
                cache[clrType] = result;
            }
            return result;
        }

        public IEntityServices GetEntityServices(Type clrType)
        {
            IEntityServices result;
            if (!cache.TryGetValue(clrType, out result))
            {
                IEntityType entityType = _dbContext.Model.FindEntityType(clrType);
                result = (IEntityServices)Activator.CreateInstance(typeof(KeyServices<>).MakeGenericType(entityType.ClrType), entityType);
                cache[clrType] = result;
            }
            return result;
        }   
    }
}
