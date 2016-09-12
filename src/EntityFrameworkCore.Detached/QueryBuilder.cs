using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached
{
    public class QueryBuilder
    {
        #region Fields

        static readonly MethodInfo ThenIncludeAfterCollectionMethodInfo
            = typeof(EntityFrameworkQueryableExtensions)
                .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
                .Single(mi => !mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter);

        static readonly MethodInfo ThenIncludeAfterReferenceMethodInfo
            = typeof(EntityFrameworkQueryableExtensions)
                .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
                .Single(mi => mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter);

        static readonly MethodInfo IncludeMethodInfo
            = typeof(EntityFrameworkQueryableExtensions)
                .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.Include))
                .Single(mi => mi.GetParameters().Any(pi => pi.Name == "navigationPropertyPath"));

        DbContext _context;

        #endregion

        #region Ctor.

        public QueryBuilder(DbContext context)
        {
            _context = context;
        }

        #endregion

        public virtual IQueryable<TEntity> GetRootQuery<TEntity>()
            where TEntity : class
        {
            object query = _context.Set<TEntity>();
            EntityType entityType = _context.Model.FindEntityType(typeof(TEntity)) as EntityType;

            //include paths.
            List<NavigationNode> paths = new List<NavigationNode>();
            GetIncludePaths(null, entityType, null, paths);

            foreach (var path in paths)
            {
                query = IncludePath(query, path, entityType.ClrType);
            }

            return (IQueryable<TEntity>)query;
        }

        void GetIncludePaths(EntityType parentType, EntityType entityType, NavigationNode path, List<NavigationNode> results)
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
                        results.Add(newPath);
                }
            }
            else
            {
                results.Add(path);
            }
        }

        protected virtual object IncludePath(object query, NavigationNode path, Type rootType)
        {
            MethodInfo methodInfo;

            if (path.Previous != null)
            {
                // recurse to root.
                query = IncludePath(query, path.Previous, rootType);

                methodInfo = path.Previous.Navigation.IsCollection()
                     ? ThenIncludeAfterCollectionMethodInfo
                     : ThenIncludeAfterReferenceMethodInfo;

                methodInfo = methodInfo.MakeGenericMethod(rootType,
                                                          path.Navigation.DeclaringEntityType.ClrType,
                                                          path.Navigation.PropertyInfo.PropertyType);
            }
            else
            {
                methodInfo = IncludeMethodInfo.MakeGenericMethod(
                    path.Navigation.DeclaringEntityType.ClrType,
                    path.Navigation.PropertyInfo.PropertyType);
            }

            return methodInfo.Invoke(null, new object[] { query, path.Navigation.GetMemberExpression() });
        }
    }


    public class NavigationNode
    {
        public NavigationNode Previous { get; set; }

        public Navigation Navigation { get; set; }

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
