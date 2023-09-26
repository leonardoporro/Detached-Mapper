using Detached.Mappers.Exceptions;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.RuntimeTypes.Reflection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.EntityFramework.Queries
{
    public class QueryProvider
    {
        readonly ConcurrentDictionary<QueryCacheKey, object> _cache = new();
        readonly MapperOptions _options;

        public QueryProvider(MapperOptions options)
        {
            _options = options;
        }
 
        public Task<TTarget> LoadAsync<TSource, TTarget>(IQueryable<TTarget> queryable, TSource source)
            where TSource : class
            where TTarget : class
        {
            QueryTemplate<TSource, TTarget> queryTemplate = GetTemplate<TSource, TTarget>();
            IQueryable<TTarget> query = queryTemplate.Render(queryable, source);

            return query.FirstOrDefaultAsync();
        }

        public TTarget Load<TSource, TTarget>(IQueryable<TTarget> queryable, TSource source)
            where TSource : class
            where TTarget : class
        {
            QueryTemplate<TSource, TTarget> queryTemplate = GetTemplate<TSource, TTarget>();
            IQueryable<TTarget> query = queryTemplate.Render(queryable, source);

            return query.FirstOrDefault();
        }

        QueryTemplate<TSource, TTarget> GetTemplate<TSource, TTarget>()
            where TSource : class
            where TTarget : class
        {
            var key = new QueryCacheKey(typeof(TSource), typeof(TTarget), QueryType.Load);

            return (QueryTemplate<TSource, TTarget>)_cache.GetOrAdd(key, fn =>
            {
                QueryTemplate<TSource, TTarget> queryTemplate = new QueryTemplate<TSource, TTarget>();

                IType sourceType = _options.GetType(typeof(TSource));
                IType targetType = _options.GetType(typeof(TTarget));
                TypePair typePair = _options.GetTypePair(sourceType, targetType, null);

                queryTemplate.SourceConstant = Constant(null, sourceType.ClrType);
                queryTemplate.FilterExpression = CreateFilterExpression<TSource, TTarget>(typePair, queryTemplate.SourceConstant);
                queryTemplate.Includes = GetIncludes(typePair);

                return queryTemplate;
            });
        }

        Expression<Func<TTarget, bool>> CreateFilterExpression<TSource, TTarget>(TypePair typePair, ConstantExpression sourceConstant)
        {
            var targetParam = Parameter(typePair.TargetType.ClrType, "e");

            Expression expression = null;

            foreach (TypePairMember memberPair in typePair.Members.Values)
            {
                if (memberPair.IsKey())
                {
                    if (memberPair.IsIgnored())
                    {
                        throw new MapperException($"Can't build query filter, key member {memberPair.TargetMember.Name} is not mapped.");
                    }

                    var targetExpr = memberPair.TargetMember.BuildGetExpression(targetParam, null);
                    var sourceExpr = memberPair.SourceMember.BuildGetExpression(sourceConstant, null);

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

        List<string> GetIncludes(TypePair typePair)
        {
            List<string> result = new List<string>();
            Stack<IType> stack = new Stack<IType>();

            GetIncludes(typePair, stack, null, result);

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

        void GetIncludes(TypePair typePair, Stack<IType> stack, string prefix, List<string> result)
        {
            stack.Push(typePair.TargetType);

            foreach (TypePairMember memberPair in typePair.Members.Values)
            {
                if (memberPair.IsMapped() && !memberPair.IsParent() && !memberPair.IsSetAsPrimitive())
                {
                    IType sourceMemberType = _options.GetType(memberPair.SourceMember.ClrType);
                    IType targetMemberType = _options.GetType(memberPair.TargetMember.ClrType);

                    if (!stack.Contains(targetMemberType))
                    {
                        if (targetMemberType.IsCollection() && _options.GetType(targetMemberType.ItemClrType).IsEntity())
                        {
                            string name = prefix + memberPair.TargetMember.Name;
                            result.Add(name);

                            if (memberPair.TargetMember.IsComposition())
                            {
                                IType sourceItemType = _options.GetType(sourceMemberType.ItemClrType);
                                IType targetItemType = _options.GetType(targetMemberType.ItemClrType);
                                TypePair itemTypePair = _options.GetTypePair(sourceItemType, targetItemType, memberPair);

                                GetIncludes(itemTypePair, stack, name + ".", result);
                            }

                        }
                        else if (targetMemberType.IsEntity())
                        {
                            string name = prefix + memberPair.TargetMember.Name;
                            result.Add(name);

                            if (memberPair.TargetMember.IsComposition())
                            {
                                TypePair memberTypePair = _options.GetTypePair(sourceMemberType, targetMemberType, memberPair);
                                GetIncludes(memberTypePair, stack, name + ".", result);
                            }
                        }
                    }
                }
            }

            stack.Pop();
        }
    }
}