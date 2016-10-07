using EntityFrameworkCore.Detached.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkCore.Detached.DataAnnotations;
using EntityFrameworkCore.Detached.Paged;
using EntityFrameworkCore.Detached.DataAnnotations.Paged;
using System.Reflection;

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

        public virtual async Task<TEntity> FindEntityByKey<TEntity>(EntityType entityType, object[] keyValues)
            where TEntity : class
        {
            Expression<Func<TEntity, bool>> filter = GetFindByKeyExpression<TEntity>(entityType, entityType.FindPrimaryKey(), keyValues);
            return await GetBaseQuery<TEntity>(entityType).Where(filter).AsNoTracking().SingleOrDefaultAsync();
        }

        public virtual async Task<List<TEntity>> FindEntities<TEntity>(EntityType entityType, Expression<Func<TEntity, bool>> filter)
           where TEntity : class
        {
            return await GetBaseQuery<TEntity>(entityType).Where(filter).AsNoTracking().ToListAsync();
        }

        public virtual async Task<List<TItem>> FindEntities<TEntity, TItem>(EntityType entityType, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TItem>> project)
           where TEntity : class
           where TItem : class
        {
            return await GetBaseQuery<TEntity>(entityType).Where(filter).AsNoTracking().Select(project).ToListAsync();
        }

        public virtual async Task<TEntity> FindPersistedEntity<TEntity>(EntityType entityType, object detachedEntity)
            where TEntity : class
        {
            Key key = entityType.FindPrimaryKey();
            object[] keyValues = key.Properties.Select(p => p.Getter.GetClrValue(detachedEntity)).ToArray();
            Expression<Func<TEntity, bool>> filter = GetFindByKeyExpression<TEntity>(entityType, entityType.FindPrimaryKey(), keyValues);

            return await GetBaseQuery<TEntity>(entityType).Where(filter).AsTracking().SingleOrDefaultAsync();
        }

        public async Task<IPagedResult<TEntity>> GetPage<TEntity>(EntityType entityType, IPagedRequest<TEntity> request) where TEntity : class
        {
            IPagedResult<TEntity> result = new PagedResult<TEntity>();
            result.PageSize = request.PageSize;
            result.PageIndex = request.PageIndex;

            IQueryable<TEntity> baseQuery = GetBaseQuery<TEntity>(entityType);
            // apply filter.
            if (request.FilterBy != null)
            {
                baseQuery = baseQuery.Where(request.FilterBy);
            }

            // apply order by.
            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                string propertyName;
                bool asc;
                GetOrderByParameters(request.OrderBy, out propertyName, out asc);
                var orderByExpression = GetOrderByMemberExpression<TEntity>(entityType.ClrType, propertyName);
                if (asc)
                    baseQuery = baseQuery.OrderBy(orderByExpression);
                else
                    baseQuery = baseQuery.OrderByDescending(orderByExpression);
            }

            // apply pagination.
            if (request.PageSize > 0)
            {
                result.RowCount = await baseQuery.CountAsync();
                baseQuery = baseQuery.Skip((request.PageIndex - 1) * request.PageSize)
                                     .Take(request.PageSize);

                result.PageCount = (int)Math.Ceiling((float)result.RowCount / result.PageSize);
            }

            // fill results.
            result.Items = await baseQuery.ToListAsync();

            return result;
        }

        public async Task<IPagedResult<TItem>> GetPage<TEntity, TItem>(EntityType entityType, IPagedRequest<TEntity> request, Expression<Func<TEntity, TItem>> project) 
            where TEntity : class
            where TItem : class
        {
            IPagedResult<TItem> result = new PagedResult<TItem>();
            result.PageSize = request.PageSize;
            result.PageIndex = request.PageIndex;

            IQueryable<TEntity> baseQuery = GetBaseQuery<TEntity>(entityType);
            // apply filter.
            if (request.FilterBy != null)
            {
                baseQuery = baseQuery.Where(request.FilterBy);
            }

            // apply order by.
            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                string propertyName;
                bool asc;
                GetOrderByParameters(request.OrderBy, out propertyName, out asc);
                var orderByExpression = GetOrderByMemberExpression<TEntity>(entityType.ClrType, propertyName);
                if (asc)
                    baseQuery = baseQuery.OrderBy(orderByExpression);
                else
                    baseQuery = baseQuery.OrderByDescending(orderByExpression);
            }

            // apply pagination.
            if (request.PageSize > 0)
            {
                result.RowCount = await baseQuery.CountAsync();
                baseQuery = baseQuery.Skip((request.PageIndex - 1) * request.PageSize)
                                     .Take(request.PageSize);

                result.PageCount = (int)Math.Ceiling((float)result.RowCount / result.PageSize);
            }

            // fill results.
            result.Items = await baseQuery.Select(project).ToListAsync();

            return result;
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
            if (keyValues == null || keyValues.Any(kv => kv == null))
                throw new ArgumentException("Key values cannot be null.", nameof(keyValues));

            if (key.Properties.Count != keyValues.Length)
                throw new ArgumentException($"Key values count mismatch, expected {string.Join(",", key.Properties.Select(p => p.Name))}");

            ParameterExpression param = Expression.Parameter(entityType.ClrType, entityType.ClrType.Name.ToLower());
            Func<int, Expression> buildCompare = i =>
            {
                object keyValue = keyValues[i];
                Property keyProperty = key.Properties[i];
                if (keyValue.GetType() != keyProperty.ClrType)
                {
                    keyValue = Convert.ChangeType(keyValue, keyProperty.ClrType);
                }

                return Expression.Equal(Expression.Property(param, keyProperty.PropertyInfo),
                                        Expression.Constant(keyValue));
            };

            Expression findExpr = buildCompare(0);
            for (int i = 1; i < key.Properties.Count; i++)
            {
                findExpr = Expression.AndAlso(findExpr, buildCompare(i));
            }
            return Expression.Lambda<Func<TEntity, bool>>(findExpr, param);
        }

        Expression<Func<TEntity, TValue>> GetMemberExpression<TEntity, TValue>(Type clrType, string propertyName)
        {
            ParameterExpression param = Expression.Parameter(clrType);
            Expression body = Expression.Property(param, propertyName);
            return Expression.Lambda<Func<TEntity, TValue>>(body, param);
        }

        void GetOrderByParameters(string columnExpression, out string propertyName, out bool asc)
        {
            string[] orderByParts = columnExpression.Split(' ');
            if (orderByParts.Length == 2)
            {
                propertyName = orderByParts[0].Trim();
                switch (orderByParts[1].Trim().ToLower())
                {
                    case "asc":
                        asc = true;
                        break;
                    case "desc":
                        asc = false;
                        break;
                    default:
                        throw new Exception("Invalid order direction. Please specify ASC or DESC.");
                }
            }
            else
            {
                propertyName = columnExpression.Trim();
                asc = true;
            }
        }

        PropertyInfo GetPropertyByName(Type clrType, string propertyName)
        {
            foreach(PropertyInfo propInfo in clrType.GetRuntimeProperties())
            {
                if (string.Compare(propInfo.Name, propertyName, true) == 0)
                    return propInfo;
            }
            return null;
        }

        Expression<Func<TEntity, object>> GetOrderByMemberExpression<TEntity>(Type clrType, string propertyName)
        {
            PropertyInfo propInfo = GetPropertyByName(clrType, propertyName);
            if (propInfo == null)
                throw new ArgumentException($"Property {propertyName} does not exist in object {clrType}.");

            ParameterExpression param = Expression.Parameter(clrType);
            Expression body = Expression.Property(param, propInfo);
            return Expression.Lambda<Func<TEntity, object>>(body, param);
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
