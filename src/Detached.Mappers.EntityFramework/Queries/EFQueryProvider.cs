using Detached.Mappers.Annotations;
using Detached.Mappers.Exceptions;
using Detached.Mappers.TypeMappers;
using Detached.Mappers.TypeOptions;
using Detached.Mappers.TypeOptions.Class;
using Detached.RuntimeTypes.Reflection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.EntityFramework.Queries
{
    public class EFQueryProvider
    {
        readonly EFMapperServices _mapperServices;
        readonly ConcurrentDictionary<EFQueryCacheKey, object> _cache;

        public EFQueryProvider(EFMapperServices mapperServices)
        {
            _mapperServices = mapperServices;
            _cache = new ConcurrentDictionary<EFQueryCacheKey, object>();
        }

        public IQueryable<TProjection> Project<TEntity, TProjection>(IQueryable<TEntity> query)
            where TProjection : class
            where TEntity : class
        {
            var key = new EFQueryCacheKey(typeof(TEntity), typeof(TProjection), QueryType.Projection);

            var filter = (Expression<Func<TEntity, TProjection>>)_cache.GetOrAdd(key, fn =>
            {
                ITypeOptions entityType = _mapperServices.MapperOptions.GetTypeOptions(typeof(TEntity));
                ITypeOptions projectionType = _mapperServices.MapperOptions.GetTypeOptions(typeof(TProjection));

                var param = Parameter(entityType.ClrType, "e");
                Expression projection = ToLambda(entityType.ClrType, param, CreateSelectProjection(entityType, projectionType, param));


                return projection;
            });

            return query.Select(filter);
        }

        public Task<TTarget> LoadAsync<TSource, TTarget>(IQueryable<TTarget> queryable, TSource source)
            where TSource : class
            where TTarget : class
        {
            EFQueryTemplate<TSource, TTarget> queryTemplate = GetTemplate<TSource, TTarget>();
            IQueryable<TTarget> query = queryTemplate.Render(queryable, source);

            return query.FirstOrDefaultAsync();
        }

        public TTarget Load<TSource, TTarget>(IQueryable<TTarget> queryable, TSource source)
            where TSource : class
            where TTarget : class
        {
            EFQueryTemplate<TSource, TTarget> queryTemplate = GetTemplate<TSource, TTarget>();
            IQueryable<TTarget> query = queryTemplate.Render(queryable, source);

            return query.FirstOrDefault();
        }

        EFQueryTemplate<TSource, TTarget> GetTemplate<TSource, TTarget>()
            where TSource : class
            where TTarget : class
        {
            var key = new EFQueryCacheKey(typeof(TSource), typeof(TTarget), QueryType.Load);

            return (EFQueryTemplate<TSource, TTarget>)_cache.GetOrAdd(key, fn =>
            {
                ITypeOptions sourceType = _mapperServices.MapperOptions.GetTypeOptions(typeof(TSource));
                ITypeOptions targetType = _mapperServices.MapperOptions.GetTypeOptions(typeof(TTarget));

                EFQueryTemplate<TSource, TTarget> queryTemplate = new EFQueryTemplate<TSource, TTarget>();

                queryTemplate.SourceConstant = Constant(null, sourceType.ClrType);
                queryTemplate.FilterExpression = CreateFilterExpression<TSource, TTarget>(sourceType, targetType, queryTemplate.SourceConstant);
                queryTemplate.Includes = GetIncludes(sourceType, targetType);

                return queryTemplate;
            });
        }

        Expression<Func<TTarget, bool>> CreateFilterExpression<TSource, TTarget>(ITypeOptions sourceType, ITypeOptions targetType, ConstantExpression sourceConstant)
        {
            var targetParam = Parameter(targetType.ClrType, "e");

            Expression expression = null;

            foreach (string targetMemberName in targetType.MemberNames)
            {
                IMemberOptions targetMember = targetType.GetMember(targetMemberName);
                if (targetMember.IsKey())
                {
                    string sourceMemberName = _mapperServices.MapperOptions.GetSourcePropertyName(sourceType, targetType, targetMemberName);

                    IMemberOptions sourceMember = sourceType.GetMember(sourceMemberName);
                    if (sourceMember == null)
                    {
                        throw new MapperException($"Can't build query filter, key member {sourceMemberName} not found.");
                    }

                    var targetExpr = targetMember.BuildGetExpression(targetParam, null);
                    var sourceExpr = sourceMember.BuildGetExpression(sourceConstant, null);

                    if (sourceExpr.Type.IsNullable(out _))
                    {
                        sourceExpr = Property(sourceExpr, "Value");
                    }

                    if (sourceExpr.Type != targetExpr.Type)
                    {
                        sourceExpr = Convert(sourceExpr, targetExpr.Type);
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

        List<string> GetIncludes(ITypeOptions sourceType, ITypeOptions targetType)
        {
            List<string> result = new List<string>();
            Stack<ITypeOptions> stack = new Stack<ITypeOptions>();

            GetIncludes(sourceType, targetType, stack, null, result);

            for (int i = result.Count - 1; i >= 0; i--)
            {
                string descendantPrefix = result[i] + ".";
                if (result.Any(i => i.StartsWith(descendantPrefix)))
                {
                    result.RemoveAt(i);
                }
            }

            return result;
        }

        void GetIncludes(ITypeOptions sourceType, ITypeOptions targetType, Stack<ITypeOptions> stack, string prefix, List<string> result)
        {
            stack.Push(targetType);

            foreach (string targetMemberName in targetType.MemberNames)
            {
                IMemberOptions targetMember = targetType.GetMember(targetMemberName);
                if (!targetMember.IsParent())
                {
                    string sourceMemberName = _mapperServices.MapperOptions.GetSourcePropertyName(sourceType, targetType, targetMemberName);

                    IMemberOptions sourceMember = sourceType.GetMember(sourceMemberName);
                    if (sourceMember != null)
                    {
                        ITypeOptions sourceMemberType = _mapperServices.MapperOptions.GetTypeOptions(sourceMember.ClrType);
                        ITypeOptions targetMemberType = _mapperServices.MapperOptions.GetTypeOptions(targetMember.ClrType);

                        if (!stack.Contains(targetMemberType))
                        {
                            if (targetMemberType.IsCollection())
                            {
                                string name = prefix + targetMember.Name;
                                result.Add(name);

                                if (targetMember.IsComposition())
                                {
                                    ITypeOptions sourceItemType = _mapperServices.MapperOptions.GetTypeOptions(sourceMemberType.ItemClrType);
                                    ITypeOptions targetItemType = _mapperServices.MapperOptions.GetTypeOptions(targetMemberType.ItemClrType);

                                    GetIncludes(sourceItemType, targetItemType, stack, name + ".", result);
                                }

                            }
                            else if (targetMemberType.IsComplexOrEntity())
                            {
                                string name = prefix + targetMember.Name;
                                result.Add(name);

                                if (targetMember.IsComposition())
                                {
                                    GetIncludes(sourceMemberType, targetMemberType, stack, name + ".", result);
                                }
                            }
                        }
                    }
                }
            }

            stack.Pop();
        }

        Expression CreateSelectProjection(ITypeOptions entityType, ITypeOptions projectionType, Expression entityExpr)
        {
            if (entityType.IsCollection())
            {
                ITypeOptions entityItemType = _mapperServices.MapperOptions.GetTypeOptions(entityType.ItemClrType);
                ITypeOptions projectionItemType = _mapperServices.MapperOptions.GetTypeOptions(projectionType.ItemClrType);

                var param = Parameter(entityItemType.ClrType, "e");
                var itemMap = CreateSelectProjection(entityItemType, projectionItemType, param);

                LambdaExpression lambda = ToLambda(entityItemType.ClrType, param, itemMap);

                return Call("ToList", typeof(Enumerable), Call("Select", typeof(Enumerable), entityExpr, lambda));
            }
            else if (entityType.IsComplexOrEntity())
            {
                List<MemberBinding> bindings = new List<MemberBinding>();

                foreach (string targetMemberName in entityType.MemberNames)
                {
                    IMemberOptions entityMember = entityType.GetMember(targetMemberName);

                    if (entityMember.CanRead && !entityMember.IsNotMapped())
                    {
                        IMemberOptions projectionMember = projectionType.GetMember(targetMemberName);

                        if (projectionMember != null && projectionMember.CanWrite && !projectionMember.IsNotMapped())
                        {
                            PropertyInfo propInfo = projectionMember.GetPropertyInfo();
                            if (propInfo != null)
                            {
                                ITypeOptions projectionMemberType = _mapperServices.MapperOptions.GetTypeOptions(projectionMember.ClrType);
                                ITypeOptions entityMemberType = _mapperServices.MapperOptions.GetTypeOptions(entityMember.ClrType);

                                Expression map = entityMember.BuildGetExpression(entityExpr, null);
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