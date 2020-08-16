using Detached.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using static Detached.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.EntityFramework.Queries
{
    public class QueryProvider
    {
        readonly ConcurrentDictionary<(Type, Type), object> _templates = new ConcurrentDictionary<(Type, Type), object>();
        readonly Mapper _mapper;

        public QueryProvider(Mapper mapper)
        {
            _mapper = mapper;
        }

        public IQueryable<TSource> Project<TSource, TTarget>(IQueryable<TTarget> query)
            where TTarget : class
            where TSource : class
        {
            return query.Select(GetTemplate<TSource, TTarget>().ProjectExpression);
        }

        public Task<TTarget> LoadAsync<TSource, TTarget>(IQueryable<TTarget> queryable, TSource source)
            where TSource : class
            where TTarget : class
        {
            return GetTemplate<TSource, TTarget>().Render(queryable, source).FirstOrDefaultAsync();
        }

        public TTarget Load<TSource, TTarget>(IQueryable<TTarget> queryable, TSource source)
            where TSource : class
            where TTarget : class
        {
            return GetTemplate<TSource, TTarget>().Render(queryable, source).FirstOrDefault();
        }

        QueryTemplate<TSource, TTarget> GetTemplate<TSource, TTarget>()
            where TSource : class
            where TTarget : class
        {
            return (QueryTemplate<TSource, TTarget>)_templates.GetOrAdd((typeof(TSource), typeof(TTarget)), key =>
            {
                TypeMap typeMap = _mapper.GetTypeMap(typeof(TSource), typeof(TTarget));

                QueryTemplate<TSource, TTarget> queryTemplate = new QueryTemplate<TSource, TTarget>();

                queryTemplate.SourceConstant = Constant(null, typeMap.SourceOptions.Type);
                queryTemplate.FilterExpression = CreateFilter<TSource, TTarget>(typeMap, queryTemplate.SourceConstant);
                GetIncludes(typeMap, queryTemplate.Includes, null);
                queryTemplate.ProjectExpression = CreateSelectProjection<TSource, TTarget>(typeMap);

                return queryTemplate;
            });
        }

        Expression<Func<TTarget, bool>> CreateFilter<TSource, TTarget>(TypeMap typeMap, ConstantExpression sourceConstant)
        {
            var targetParam = Parameter(typeMap.TargetOptions.Type, "e");

            Expression expression = null;

            foreach (MemberMap memberMap in typeMap.Members)
            {
                if (memberMap.IsKey)
                {
                    var targetExpr = memberMap.TargetOptions.GetValue(targetParam, null);
                    var sourceExpr = memberMap.SourceOptions.GetValue(sourceConstant, null);

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

        void GetIncludes(TypeMap typeMap, List<string> includes, string prefix)
        {
            foreach (MemberMap memberMap in typeMap.Members)
            {
                if (memberMap.TypeMap.TargetOptions.IsCollection || memberMap.TypeMap.TargetOptions.IsComplexType)
                {
                    if (!memberMap.IsBackReference)
                    {
                        string name = prefix + memberMap.TargetOptions.Name;
                        includes.Add(name);
                        if (memberMap.Owned)
                        {
                            GetIncludes(memberMap.TypeMap, includes, name + ".");
                        }
                    }
                }
            }
        }

        Expression<Func<TTarget, TSource>> CreateSelectProjection<TSource, TTarget>(TypeMap typeMap)
        {
            var param = Parameter(typeMap.TargetOptions.Type, "e");
            Expression projection = ToLambda(typeMap.TargetOptions.Type, param, CreateSelectProjectionInternal(typeMap, param));

            return (Expression<Func<TTarget, TSource>>)projection;
        }

        Expression CreateSelectProjectionInternal(TypeMap typeMap, Expression targetExpr)
        {
            if (typeMap.SourceOptions.IsCollection)
            {
                var itemType = typeMap.ItemMap.TargetOptions.Type;
                var param = Parameter(itemType, "e");
                var itemMap = CreateSelectProjectionInternal(typeMap.ItemMap, param);

                LambdaExpression lambda = ToLambda(itemType, param, itemMap);

                return Call("ToList", typeof(Enumerable), Call("Select", typeof(Enumerable), targetExpr, lambda));
            }
            else if (typeMap.SourceOptions.IsComplexType)
            {
                List<MemberBinding> bindings = new List<MemberBinding>();
                foreach (MemberMap memberMap in typeMap.Members)
                {
                    PropertyInfo propInfo = typeMap.SourceOptions.Type.GetProperty(memberMap.SourceOptions.Name);
                    if (propInfo != null)
                    {
                        Expression map = memberMap.TargetOptions.GetValue(targetExpr, null);
                        map = CreateSelectProjectionInternal(memberMap.TypeMap, map);
                        bindings.Add(Bind(propInfo, map));
                    }
                }

                return MemberInit(New(typeMap.SourceOptions.Type), bindings.ToArray());
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