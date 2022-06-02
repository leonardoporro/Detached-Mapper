using AgileObjects.ReadableExpressions;
using Detached.Mappers.Annotations;
using Detached.Mappers.Exceptions;
using Detached.Mappers.TypeOptions;
using Detached.Mappers.TypeOptions.Class;
using Detached.RuntimeTypes.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.EntityFramework.Queries
{
    public class DetachedQueryProvider
    {
        readonly MapperOptions _options;
        IMemoryCache _memoryCache;

        public DetachedQueryProvider(MapperOptions options, IMemoryCache memoryCache)
        {
            _options = options;
            _memoryCache = memoryCache;
        }

        public IQueryable<TProjection> Project<TEntity, TProjection>(IQueryable<TEntity> query)
            where TProjection : class
            where TEntity : class
        {
            var key = new DetachedQueryCacheKey(typeof(TEntity), typeof(TProjection), QueryType.Projection);

            var filter = _memoryCache.GetOrCreate(key, entry =>
            {
                ITypeOptions entityType = _options.GetTypeOptions(typeof(TEntity));
                ITypeOptions projectionType = _options.GetTypeOptions(typeof(TProjection));

                var param = Parameter(entityType.ClrType, "e");
                Expression projection = ToLambda(entityType.ClrType, param, CreateSelectProjection(entityType, projectionType, param));

                entry.SetSize(1);

                return (Expression<Func<TEntity, TProjection>>)projection;
            });

            return query.Select(filter);
        }

        public Task<TTarget> LoadAsync<TSource, TTarget>(IQueryable<TTarget> queryable, TSource source)
            where TSource : class
            where TTarget : class
        {
            DetachedQueryTemplate<TSource, TTarget> queryTemplate = GetTemplate<TSource, TTarget>();
            IQueryable<TTarget> query = queryTemplate.Render(queryable, source);

            return query.FirstOrDefaultAsync();
        }

        public TTarget Load<TSource, TTarget>(IQueryable<TTarget> queryable, TSource source)
            where TSource : class
            where TTarget : class
        {
            DetachedQueryTemplate<TSource, TTarget> queryTemplate = GetTemplate<TSource, TTarget>();
            IQueryable<TTarget> query = queryTemplate.Render(queryable, source);

            return query.FirstOrDefault();
        }

        DetachedQueryTemplate<TSource, TTarget> GetTemplate<TSource, TTarget>()
            where TSource : class
            where TTarget : class
        {
            var key = new DetachedQueryCacheKey(typeof(TSource), typeof(TTarget), QueryType.Load);

            return _memoryCache.GetOrCreate(key, entry =>
            {
                ITypeOptions sourceType = _options.GetTypeOptions(typeof(TSource));
                ITypeOptions targetType = _options.GetTypeOptions(typeof(TTarget));

                DetachedQueryTemplate<TSource, TTarget> queryTemplate = new DetachedQueryTemplate<TSource, TTarget>();

                queryTemplate.SourceConstant = Constant(null, sourceType.ClrType);
                queryTemplate.FilterExpression = CreateFilter<TSource, TTarget>(sourceType, targetType, queryTemplate.SourceConstant);
                GetIncludes(sourceType, targetType, queryTemplate.Includes, null);

                entry.SetSize(1);

                return queryTemplate;
            });
        }

        Expression<Func<TTarget, bool>> CreateFilter<TSource, TTarget>(ITypeOptions sourceType, ITypeOptions targetType, ConstantExpression sourceConstant)
        {
            var targetParam = Parameter(targetType.ClrType, "e");

            Expression expression = null;

            foreach (string memberName in targetType.MemberNames)
            {
                IMemberOptions targetMember = targetType.GetMember(memberName);
                if (targetMember.IsKey())
                {
                    IMemberOptions sourceMember = sourceType.GetMember(memberName);
                    if (sourceMember == null)
                    {
                        throw new MapperException($"Can't build query filter, key member {memberName} not found.");
                    }

                    var targetExpr = targetMember.BuildGetterExpression(targetParam, null);
                    var sourceExpr = sourceMember.BuildGetterExpression(sourceConstant, null);

                    if (sourceExpr.Type.IsNullable(out _))
                    {
                        sourceExpr = Property(sourceExpr, "Value");
                    }

                    if (expression == null)
                    {
                        expression = Equal(targetExpr, sourceExpr);
                    }
                    else
                    {
                        expression = And(expression, Equal(targetExpr, sourceExpr));
                    }
                }
            }

            return Lambda<Func<TTarget, bool>>(expression, targetParam);
        }

        void GetIncludes(ITypeOptions sourceType, ITypeOptions targetType, List<string> includes, string prefix)
        {
            foreach (string memberName in targetType.MemberNames)
            {
                IMemberOptions targetMember = targetType.GetMember(memberName);
                if (!targetMember.IsParent())
                {
                    IMemberOptions sourceMember = sourceType.GetMember(memberName);
                    if (sourceMember != null)
                    {
                        ITypeOptions sourceMemberType = _options.GetTypeOptions(sourceMember.ClrType);
                        ITypeOptions targetMemberType = _options.GetTypeOptions(targetMember.ClrType);

                        if (targetMemberType.IsCollection())
                        {
                            string name = prefix + targetMember.Name;
                            includes.Add(name);

                            if (targetMember.IsComposition())
                            {
                                ITypeOptions sourceItemType = _options.GetTypeOptions(sourceMemberType.ItemClrType);
                                ITypeOptions targetItemType = _options.GetTypeOptions(targetMemberType.ItemClrType);

                                GetIncludes(sourceItemType, targetItemType, includes, name + ".");
                            }

                        }
                        else if (targetMemberType.IsComplexOrEntity())
                        {
                            string name = prefix + targetMember.Name;
                            includes.Add(name);

                            if (targetMember.IsComposition())
                            {
                                GetIncludes(sourceMemberType, targetMemberType, includes, name + ".");
                            }
                        }
                    }
                }
            }
        }

        Expression CreateSelectProjection(ITypeOptions entityType, ITypeOptions projectionType, Expression entityExpr)
        {
            if (entityType.IsCollection())
            {
                ITypeOptions entityItemType = _options.GetTypeOptions(entityType.ItemClrType);
                ITypeOptions projectionItemType = _options.GetTypeOptions(projectionType.ItemClrType);

                var param = Parameter(entityItemType.ClrType, "e");
                var itemMap = CreateSelectProjection(entityItemType, projectionItemType, param);

                LambdaExpression lambda = ToLambda(entityItemType.ClrType, param, itemMap);

                return Call("ToList", typeof(Enumerable), Call("Select", typeof(Enumerable), entityExpr, lambda));
            }
            else if (entityType.IsComplexOrEntity())
            {
                List<MemberBinding> bindings = new List<MemberBinding>();

                foreach (string memberName in entityType.MemberNames)
                {
                    IMemberOptions entityMember = entityType.GetMember(memberName);

                    if (entityMember.CanRead && !entityMember.IsNotMapped())
                    {
                        IMemberOptions projectionMember = projectionType.GetMember(memberName);

                        if (projectionMember != null && projectionMember.CanWrite && !projectionMember.IsNotMapped())
                        {
                            PropertyInfo propInfo = projectionMember.GetPropertyInfo();
                            if (propInfo != null)
                            {
                                ITypeOptions projectionMemberType = _options.GetTypeOptions(projectionMember.ClrType);
                                ITypeOptions entityMemberType = _options.GetTypeOptions(entityMember.ClrType);

                                Expression map = entityMember.BuildGetterExpression(entityExpr, null);
                                Expression body = CreateSelectProjection(entityMemberType, projectionMemberType, map);

                                if (entityMemberType.IsComplexOrEntity())
                                {
                                    map = Condition(NotEqual(map, Constant(null, map.Type)), body, Constant(null, body.Type));
                                }
                                else
                                {
                                    map = body;
                                }

                                bindings.Add(Bind(propInfo, map));
                            }
                        }
                    }
                }

                return MemberInit(New(projectionType.ClrType), bindings.ToArray());
            }
            else
            {
                return entityExpr;
            }
        }

        LambdaExpression ToLambda(Type type, ParameterExpression paramExpr, Expression body)
        {
            Type funcType = typeof(Func<,>).MakeGenericType(type, body.Type);
            var lambda = Lambda(funcType, body, new[] { paramExpr });
            return lambda;
        }
    }
}