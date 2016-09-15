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
    /// <summary>
    /// Provides queries to fetch root entities. A root is an entity with their owned and associated
    /// children that works as a single unit.
    /// </summary>
    public class QueryBuilder
    {
        #region Fields
    
        // Include LINQ methods to invoke dynamically.
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

        /// <summary>
        /// Initializes a new instance of QueryBuilder.
        /// </summary>
        /// <param name="context">An instance of a regular DbContext.</param>
        public QueryBuilder(DbContext context)
        {
            _context = context;
        }

        #endregion

        /// <summary>
        /// Creates a query and includes 1st level associated properties and nth level owned
        /// properties recursively.
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity to query.</typeparam>
        /// <returns>Base IQueryable with the included navigation properties.</TEntity></returns>
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

        /// <summary>
        /// Gets the path list that should be included for the given entity type.
        /// </summary>
        /// <param name="parentType">EntityType of the parent entity.</param>
        /// <param name="entityType">Target EntityType of the property.</param>
        /// <param name="path">Path that is currently being built.</param>
        /// <param name="results">Final list of paths to include.</param>
        protected virtual void GetIncludePaths(EntityType parentType, EntityType entityType, NavigationNode path, List<NavigationNode> results)
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

        /// <summary>
        /// Includes the specified path into the given query.
        /// </summary>
        /// <param name="query">IQueryable that will include the path.</param>
        /// <param name="path">Path to include.</param>
        /// <param name="rootType">Clr type of the initial IQueryable</param>
        /// <returns>IQueryable with the included path.</returns>
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
