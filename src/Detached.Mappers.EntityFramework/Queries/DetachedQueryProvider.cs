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

        public IQueryable<TSource> Project<TSource, TTarget>(IQueryable<TTarget> query)
            where TTarget : class
            where TSource : class
        {
            var key = new DetachedQueryCacheKey(typeof(TSource), typeof(TTarget), QueryType.Projection);

            var filter = _memoryCache.GetOrCreate(key, entry =>
            {
                ITypeOptions sourceType = _options.GetTypeOptions(typeof(TSource));
                ITypeOptions targetType = _options.GetTypeOptions(typeof(TTarget));

                var param = Parameter(sourceType.ClrType, "e");
                Expression projection = ToLambda(targetType.ClrType, param, CreateSelectProjection(sourceType, targetType, param));

                entry.SetSize(1);

                return (Expression<Func<TTarget, TSource>>)projection;
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
                if (targetMember.IsKey)
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
                if (targetMember.IsComposition)
                {
                    IMemberOptions sourceMember = sourceType.GetMember(memberName);
                    if (sourceMember != null)
                    {
                        ITypeOptions sourceMemberType = _options.GetTypeOptions(sourceMember.ClrType);
                        ITypeOptions targetMemberType = _options.GetTypeOptions(targetMember.ClrType);

                        if (targetMemberType.IsCollection)
                        {
                            string name = prefix + targetMember.Name;
                            includes.Add(name);

                            ITypeOptions sourceItemType = _options.GetTypeOptions(sourceMemberType.ItemClrType);
                            ITypeOptions targetItemType = _options.GetTypeOptions(targetMemberType.ItemClrType);

                            GetIncludes(sourceItemType, targetItemType, includes, name + ".");

                        }
                        else if (targetMemberType.IsComplex)
                        {
                            string name = prefix + targetMember.Name;
                            includes.Add(name);



                            GetIncludes(sourceMemberType, targetMemberType, includes, name + ".");
                        }
                    }
                }
            }
        }

        Expression CreateSelectProjection(ITypeOptions sourceType, ITypeOptions targetType, Expression targetExpr)
        {
            if (sourceType.IsCollection)
            {
                ITypeOptions sourceItemType = _options.GetTypeOptions(sourceType.ItemClrType);
                ITypeOptions targetItemType = _options.GetTypeOptions(targetType.ItemClrType);

                var param = Parameter(sourceItemType.ClrType, "e");
                var itemMap = CreateSelectProjection(sourceItemType, targetItemType, param);

                LambdaExpression lambda = ToLambda(targetItemType.ClrType, param, itemMap);

                return Call("ToList", typeof(Enumerable), Call("Select", typeof(Enumerable), targetExpr, lambda));
            }
            else if (sourceType.IsComplex)
            {
                List<MemberBinding> bindings = new List<MemberBinding>();

                foreach (string memberName in targetType.MemberNames)
                {
                    IMemberOptions targetMember = targetType.GetMember(memberName);

                    if (targetMember.CanWrite && !targetMember.IsIgnored)
                    {
                        IMemberOptions sourceMember = sourceType.GetMember(memberName);

                        if (sourceMember.CanRead && !sourceMember.IsIgnored)
                        {
                            PropertyInfo propInfo = sourceMember.GetPropertyInfo();
                            if (propInfo != null)
                            {
                                ITypeOptions sourceMemberType = _options.GetTypeOptions(sourceMember.ClrType);
                                ITypeOptions targetMemberType = _options.GetTypeOptions(targetMember.ClrType);

                                Expression map = targetMember.BuildGetterExpression(targetExpr, null);
                                map = CreateSelectProjection(sourceMemberType, targetMemberType, map);
                                bindings.Add(Bind(propInfo, map));
                            }
                        }
                    }
                }

                return MemberInit(New(sourceType.ClrType), bindings.ToArray());
            }
            else
            {
                return targetExpr;
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