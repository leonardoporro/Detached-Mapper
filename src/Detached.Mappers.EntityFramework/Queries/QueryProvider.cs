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
using System.Reflection;
using System.Threading.Tasks;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.EntityFramework.Queries
{
    public class QueryProvider
    {
        readonly ConcurrentDictionary<QueryCacheKey, object> _cache;
        readonly EFMapper _mapper;

        public QueryProvider(EFMapper mapper)
        {
            _mapper = mapper;
            _cache = new ConcurrentDictionary<QueryCacheKey, object>();
        }

        public IQueryable<TProjection> Project<TEntity, TProjection>(IQueryable<TEntity> query)
            where TProjection : class
            where TEntity : class
        {
            var key = new QueryCacheKey(typeof(TEntity), typeof(TProjection), QueryType.Projection);

            var filter = (Expression<Func<TEntity, TProjection>>)_cache.GetOrAdd(key, fn =>
            {
                IType entityType = _mapper.Options.GetType(typeof(TEntity));
                IType projectionType = _mapper.Options.GetType(typeof(TProjection));
                TypePair typePair = _mapper.Options.GetTypePair(entityType, projectionType, null);

                var param = Parameter(entityType.ClrType, "e");
                Expression projection = ToLambda(entityType.ClrType, param, CreateSelectProjection(typePair, param, 0));

                return projection;
            });

            return query.Select(filter);
        }

        public static int MaxProjectionDetph { get; set; } = 5;

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

                IType sourceType = _mapper.Options.GetType(typeof(TSource));
                IType targetType = _mapper.Options.GetType(typeof(TTarget));
                TypePair typePair = _mapper.Options.GetTypePair(sourceType, targetType, null);

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
                    IType sourceMemberType = _mapper.Options.GetType(memberPair.SourceMember.ClrType);
                    IType targetMemberType = _mapper.Options.GetType(memberPair.TargetMember.ClrType);

                    if (!stack.Contains(targetMemberType))
                    {
                        if (targetMemberType.IsCollection() && _mapper.Options.GetType(targetMemberType.ItemClrType).IsEntity())
                        {
                            string name = prefix + memberPair.TargetMember.Name;
                            result.Add(name);

                            if (memberPair.TargetMember.IsComposition())
                            {
                                IType sourceItemType = _mapper.Options.GetType(sourceMemberType.ItemClrType);
                                IType targetItemType = _mapper.Options.GetType(targetMemberType.ItemClrType);
                                TypePair itemTypePair = _mapper.Options.GetTypePair(sourceItemType, targetItemType, memberPair);

                                GetIncludes(itemTypePair, stack, name + ".", result);
                            }

                        }
                        else if (targetMemberType.IsEntity())
                        {
                            string name = prefix + memberPair.TargetMember.Name;
                            result.Add(name);

                            if (memberPair.TargetMember.IsComposition())
                            {
                                TypePair memberTypePair = _mapper.Options.GetTypePair(sourceMemberType, targetMemberType, memberPair);
                                GetIncludes(memberTypePair, stack, name + ".", result);
                            }
                        }
                    }
                }
            }

            stack.Pop();
        }

        Expression CreateSelectProjection(TypePair typePair, Expression entityExpr, int depth)
        {
            if (depth > MaxProjectionDetph)
            {
                entityExpr = null;
            }
            else if (typePair.SourceType.IsCollection())
            {
                entityExpr = ResolveCollection(typePair, entityExpr, depth);
            }
            else if (typePair.SourceType.IsComplexOrEntity())
            {
                string discriminatorName = typePair.TargetType.GetDiscriminatorName();
                if (discriminatorName != null)
                {
                    entityExpr = ResolveInheritance(typePair, entityExpr, depth);
                }
                else
                {
                    entityExpr = BindMembers(typePair, entityExpr, depth);
                }
            }

            return entityExpr;
        }

        private Expression ResolveCollection(TypePair typePair, Expression entityExpr, int depth)
        {
            IType entityItemType = _mapper.Options.GetType(typePair.SourceType.ItemClrType);
            IType projectionItemType = _mapper.Options.GetType(typePair.TargetType.ItemClrType);
            TypePair itemTypePair = _mapper.Options.GetTypePair(entityItemType, projectionItemType, typePair.ParentMember);

            var param = Parameter(entityItemType.ClrType, "e");
            var itemMap = CreateSelectProjection(itemTypePair, param, depth + 1);
            if (itemMap != null)
            {

                LambdaExpression bodyExpr = ToLambda(entityItemType.ClrType, param, itemMap);
                return Call("ToList", typeof(Enumerable), Call("Select", typeof(Enumerable), entityExpr, bodyExpr));
            }
            else
            {
                return null;
            }
        }

        Expression ResolveInheritance(TypePair typePair, Expression entityExpr, int depth)
        {
            string discriminatorName = typePair.TargetType.GetDiscriminatorName();
            
            var targetDiscriminatorValues = typePair.TargetType.GetDiscriminatorValues().ToList();
            var sourceDiscriminatorValues = typePair.SourceType.GetDiscriminatorValues();

            foreach (var entry in targetDiscriminatorValues)
            {
                if (!sourceDiscriminatorValues.ContainsKey(entry.Key))
                {
                    throw new MapperException($"Cannot resolve inheritance. Source type '{typePair.SourceType}' does not contain value '{entry.Key}' for target type '{typePair.TargetType}.'");
                }
            }

            ITypeMember discriminatorMember = typePair.Members[discriminatorName].SourceMember;

            Expression discriminatorPropertyExpr = Property(entityExpr, discriminatorMember.GetPropertyInfo());

            return ResolveInheritance(typePair, 
                sourceDiscriminatorValues, 
                targetDiscriminatorValues, 
                discriminatorPropertyExpr,  
                0, 
                entityExpr, 
                depth);
        }

        Expression ResolveInheritance(
            TypePair typePair,
            Dictionary<object, Type> sourceDiscriminatorValues,
            List<KeyValuePair<object, Type>> targetDiscriminatorValues,
            Expression discriminatorPropertyExpr,
            int index, 
            Expression entityExpr, 
            int depth)
        {
            object targetDiscriminatorValue = targetDiscriminatorValues[index].Key;
            Type targetDiscriminatorClrType = targetDiscriminatorValues[index].Value;
            IType targetDiscriminatorType = _mapper.Options.GetType(targetDiscriminatorClrType);

            Type sourceDiscriminatorClrType = sourceDiscriminatorValues[targetDiscriminatorValue];
            IType sourceDiscriminatorType = _mapper.Options.GetType(sourceDiscriminatorClrType);

            TypePair concreteTypePair = _mapper.Options.GetTypePair(sourceDiscriminatorType, targetDiscriminatorType, typePair.ParentMember);

            var bindingMemberExpression = BindMembers(concreteTypePair, Convert(entityExpr, sourceDiscriminatorClrType), depth + 1);

            bindingMemberExpression = Convert(bindingMemberExpression, typePair.TargetType.ClrType);

            if (index < sourceDiscriminatorValues.Count - 2)
            {
                var discriminatorCheckCondition = Equal(discriminatorPropertyExpr, Constant(targetDiscriminatorValue));

                var innerExpression = ResolveInheritance(
                    typePair, 
                    sourceDiscriminatorValues,
                    targetDiscriminatorValues,
                    discriminatorPropertyExpr,
                    index + 1, 
                    entityExpr, 
                    depth + 1);

                return Condition(discriminatorCheckCondition, bindingMemberExpression, innerExpression);
            }
            else
            {
                return bindingMemberExpression;
            }
        }

        private Expression BindMembers(TypePair typePair, Expression entityExpr, int depth)
        {
            List<MemberBinding> bindings = new List<MemberBinding>();

            foreach (TypePairMember memberPair in typePair.Members.Values)
            {
                if (memberPair.IsMapped())
                {
                    PropertyInfo propInfo = memberPair.TargetMember.GetPropertyInfo();
                    if (propInfo != null)
                    {
                        IType projectionMemberType = _mapper.Options.GetType(memberPair.TargetMember.ClrType);
                        IType entityMemberType = _mapper.Options.GetType(memberPair.SourceMember.ClrType);
                        TypePair memberTypePair = _mapper.Options.GetTypePair(entityMemberType, projectionMemberType, memberPair);

                        Expression map = memberPair.SourceMember.BuildGetExpression(entityExpr, null);
                        Expression body = CreateSelectProjection(memberTypePair, map, depth + 1);
                        if (body != null)
                        {
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

            return MemberInit(New(typePair.TargetType.ClrType), bindings.ToArray());
        }

        LambdaExpression ToLambda(Type type, ParameterExpression paramExpr, Expression body)
        {
            Type funcType = typeof(Func<,>).MakeGenericType(type, body.Type);
            var lambda = Lambda(funcType, body, new[] { paramExpr });
            return lambda;
        }
    }
}