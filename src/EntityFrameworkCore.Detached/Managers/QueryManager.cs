using EntityFrameworkCore.Detached.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Managers
{
    public class QueryManager : IDetachedQueryManager
    {
        #region Fields

        DbContext _dbContext;

        #endregion

        #region Ctor.

        public QueryManager(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion
   
        public virtual Task<TEntity> FindEntityByKey<TEntity>(EntityType entityType, object[] keyValues)
            where TEntity : class
        {
            Expression<Func<TEntity, bool>> filter = GetFindByKeyExpression<TEntity>(entityType, entityType.FindPrimaryKey(), keyValues);
            return GetBaseQuery<TEntity>(entityType).Where(filter).AsNoTracking().SingleOrDefaultAsync();
        }

        public virtual Task<List<TEntity>> FindEntities<TEntity>(EntityType entityType, Expression<Func<TEntity, bool>> filter)
           where TEntity : class
        {
            return GetBaseQuery<TEntity>(entityType).Where(filter).AsNoTracking().ToListAsync();
        }

        public virtual Task<TEntity> FindPersistedEntity<TEntity>(EntityType entityType, object detachedEntity)
            where TEntity : class
        {
            Key key = entityType.FindPrimaryKey();
            object[] keyValues = key.Properties.Select(p => p.Getter.GetClrValue(detachedEntity)).ToArray();
            Expression<Func<TEntity, bool>> filter = GetFindByKeyExpression<TEntity>(entityType, entityType.FindPrimaryKey(), keyValues);

            return GetBaseQuery<TEntity>(entityType).Where(filter).AsNoTracking().SingleOrDefaultAsync();
        }

        protected virtual IQueryable<TEntity> GetBaseQuery<TEntity>(EntityType entityType)
            where TEntity : class
        {
            //include paths.
            List<string> paths = new List<string>();
            GetIncludePaths(null, entityType, null, paths);

            IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            foreach (string path in paths)
            {
                query = query.Include(path);
            }

            return query;
        }

        protected virtual void GetIncludePaths(EntityType parentType, EntityType entityType, NavigationNode path, List<string> results)
        {
            var navs = entityType.GetNavigations()
                                .Select(n => new
                                {
                                    Navigation = n,
                                    IsOwned = n.IsOwned(),
                                    IsAssociated = n.IsAssociated(),
                                    TargetType = n.GetTargetType()
                                })
                                .Where(n => n.TargetType != parentType && (n.IsAssociated || n.IsOwned))
                                .ToList();

            if (navs.Any())
            {
                foreach (var nav in navs)
                {
                    NavigationNode newPath = new NavigationNode();
                    newPath.Navigation = nav.Navigation;
                    newPath.Previous = path;

                    if (nav.IsOwned)
                        GetIncludePaths(entityType, nav.TargetType, newPath, results);
                    else
                        results.Add(newPath.ToString());
                }
            }
            else if (path != null)
            {
                results.Add(path.ToString());
            }
        }

        protected virtual Expression<Func<TEntity, bool>> GetFindByKeyExpression<TEntity>(EntityType entityType, Key key, object[] keyValues)
        {
            ParameterExpression param = Expression.Parameter(entityType.ClrType, entityType.ClrType.Name.ToLower());
            Func<int, Expression> buildCompare = i =>
                Expression.Equal(Expression.Property(param, key.Properties[i].PropertyInfo),
                                 Expression.Constant(keyValues[i]));

            Expression findExpr = buildCompare(0);
            for (int i = 1; i < key.Properties.Count; i++)
            {
                findExpr = Expression.AndAlso(findExpr, buildCompare(i));
            }
            return Expression.Lambda<Func<TEntity, bool>>(findExpr, param);
        }
    }

    /// <summary>
    /// Linked list that hold the partial/final paths to include when creating the root query.
    /// </summary>
    public class NavigationNode
    {
        /// <summary>
        /// The previous (parent) property to include.
        /// </summary>
        public NavigationNode Previous { get; set; }

        /// <summary>
        /// The current property to include.
        /// </summary>
        public Navigation Navigation { get; set; }

        /// <summary>
        /// Returns the string representation of the path.
        /// </summary>
        /// <returns>The string representation of the path.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            NavigationNode path = this;
            while (path != null)
            {
                builder.Insert(0, path.Navigation.PropertyInfo.Name + ".");
                path = path.Previous;
            }

            return builder.ToString().Trim('.');
        }
    }
}
