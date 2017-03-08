using Detached.EntityFramework.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Detached.EntityFramework.Services
{
    public class EntityServicesFactory : IEntityServicesFactory
    {
        DbContext _dbContext;
        Type _dbContextType;

        ConcurrentDictionary<Type, IEntityServices> cache = new ConcurrentDictionary<Type, IEntityServices>();


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
                result = new EntityServices<TEntity>(entityType);
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
                result = (IEntityServices)Activator.CreateInstance(typeof(EntityServices<>).MakeGenericType(entityType.ClrType), entityType);
                cache[clrType] = result;
            }
            return result;
        }

        public IEntityServices GetEntityServices(Type clrType)
        {
            return cache.GetOrAdd(clrType, t =>
            {
                IEntityType entityType = _dbContext.Model.FindEntityType(t);
                if (entityType == null)
                    throw new EntityNotFoundException();

                return (IEntityServices)Activator.CreateInstance(typeof(EntityServices<>).MakeGenericType(entityType.ClrType), entityType);
            });
        }   
    }
}
