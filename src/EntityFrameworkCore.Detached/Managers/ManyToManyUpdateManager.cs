using EntityFrameworkCore.Detached.Contracts;
using EntityFrameworkCore.Detached.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EntityFrameworkCore.Detached.Managers
{
    public class ManyToManyUpdateManager : UpdateManager
    {
        #region Fields

        DbContext _dbContext;
        IDetachedSessionInfoProvider sessionInfoProvider;

        #endregion

        #region Ctor.

        public ManyToManyUpdateManager(DbContext dbContext, IDetachedSessionInfoProvider sessionInfoProvider)
            : base(dbContext, sessionInfoProvider)
        {
            _dbContext = dbContext;
        }

        #endregion

        public override void Add(EntityType entityType, object entity)
        {
            base.Add(entityType, entity);
            UpdateNavigations(entityType, entity, EntityState.Added);
        }

        public override void Delete(EntityType entityType, object entity)
        {
            base.Delete(entityType, entity);
            UpdateNavigations(entityType, entity, EntityState.Deleted);
        }

        public override void Merge(EntityType entityType, object newEntity, object dbEntity)
        {
            base.Merge(entityType, newEntity, dbEntity);

            foreach (ManyToManyNavigation navigation in entityType.GetManyToManyNavigations())
            {
                IEnumerable newCollection = navigation.End1.Getter.GetClrValue(newEntity) as IEnumerable;
                IEnumerable dbCollection = navigation.End1.Getter.GetClrValue(dbEntity) as IEnumerable;
                Dictionary<string, object> dbTable = navigation.End2.EntityType.CreateHashTable(dbCollection);

                foreach (object newItem in newCollection)
                {
                    string key = navigation.End2.EntityType.GetKeyForHashTable(newItem);
                    if (dbTable.ContainsKey(key))
                    {
                        dbTable.Remove(key);
                    }
                    else
                    {
                        IManyToManyEntity entity = CreateIntermediateEntity(navigation, dbEntity, newItem);
                        _dbContext.Add(entity);
                    }
                }

                foreach (var dbItem in dbTable)
                {
                    IManyToManyEntity entity = CreateIntermediateEntity(navigation, dbEntity, dbItem.Value);
                    _dbContext.Entry(dbItem.Value).State = EntityState.Deleted;
                }
            }
        }

        protected virtual IManyToManyEntity CreateIntermediateEntity(ManyToManyNavigation navigation, object end1, object end2)
        {
            IManyToManyEntity entity = Activator.CreateInstance(navigation.IntermediateEntityType.ClrType) as IManyToManyEntity;
            entity.End1 = end1;
            entity.End2 = end2;
            return entity;
        }

        protected virtual void UpdateNavigations(EntityType entityType, object entity, EntityState state)
        {
            foreach (ManyToManyNavigation navigation in entityType.GetManyToManyNavigations())
            {
                IEnumerable list = navigation.End1.Getter.GetClrValue(entity) as IEnumerable;
                if (list != null)
                {
                    foreach (object item in list)
                    {
                        IManyToManyEntity m2mEntity = CreateIntermediateEntity(navigation, entity, item);
                        _dbContext.Entry(m2mEntity).State = state;
                    }
                }
            }
        }
    }
}
