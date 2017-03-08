using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;
using System.Reflection;

namespace Detached.EntityFramework.Services
{
    public class EntryFinder : IEntryFinder
    {
        #region Fields

        IEntityServicesFactory _entityServicesFactory;
        DbContext _dbContext;
        Func<IKey, IIdentityMap> _findIdentityMap;
       
        #endregion

        #region Ctor.

        public EntryFinder(DbContext dbContext,
                           IEntityServicesFactory entityServicesFactory)
        {
            _entityServicesFactory = entityServicesFactory;
            _dbContext = dbContext;

            IStateManager stateManager = dbContext.ChangeTracker.GetInfrastructure();

            // HACK:
            MethodInfo findMapMethod = stateManager.GetType()
                                .GetTypeInfo().DeclaredMethods.Where(m => m.Name == "FindIdentityMap").FirstOrDefault();

            _findIdentityMap = (Func<IKey, IIdentityMap>)findMapMethod.CreateDelegate(typeof(Func<IKey, IIdentityMap>), stateManager);
        }

        #endregion

        public virtual EntityEntry FindEntry(object entity)
        {
            IEntityServices entityServices = _entityServicesFactory.GetEntityServices(entity.GetType());
            KeyValue keyValue = entityServices.GetKeyValue(entity);

            IIdentityMap map = _findIdentityMap(entityServices.GetKey());
            if (map != null)
            {
                var existing = map.TryGetEntry(keyValue.Values);
                if (existing != null)
                {
                    return new EntityEntry(existing);
                }
            }
            return null;
        }
    }
}
