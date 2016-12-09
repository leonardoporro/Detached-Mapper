using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;
using System.Reflection;

namespace EntityFrameworkCore.Detached.Services
{
    public class EntryFinder : IEntryFinder
    {
        #region Fields

        IKeyServicesFactory _keyServicesFactory;
        DbContext _dbContext;
        Func<IKey, IIdentityMap> _findIdentityMap;
       
        #endregion

        #region Ctor.

        public EntryFinder(DbContext dbContext,
                           IKeyServicesFactory keyServicesFactory)
        {
            _keyServicesFactory = keyServicesFactory;
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
            IKeyServices keyServices = _keyServicesFactory.GetKeyServices(entity.GetType());
            object[] keyValues = keyServices.GetValues(entity);

            IIdentityMap map = _findIdentityMap(keyServices.GetKey());
            if (map != null)
            {
                var existing = map.TryGetEntry(keyValues);
                if (existing != null)
                {
                    return new EntityEntry(existing);
                }
            }
            return null;
        }
    }
}
