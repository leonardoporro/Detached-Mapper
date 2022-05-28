using Detached.Mappers.Annotations;
using Detached.Mappers.Context;
using Detached.Mappers.Exceptions;
using Detached.Mappers.TypeMappers.Entity;
using Detached.Mappers.TypeOptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.TypeMappers
{
    public class ExpressionBuilder
    {
        readonly MapperOptions _options;

        public ExpressionBuilder(MapperOptions options)
        {
            _options = options;
        }

        public void BuildGetKeyExpressions(
            ITypeOptions sourceType,
            ITypeOptions targetType,
            out LambdaExpression getSourceKeyExpr,
            out LambdaExpression getTargetKeyExpr,
            out Type keyType)
        {
            List<Type> keyParamTypes = new List<Type>();
            ParameterExpression sourceExpr = Parameter(sourceType.ClrType, "source");
            ParameterExpression targetExpr = Parameter(targetType.ClrType, "target");
            ParameterExpression contextExpr = Parameter(typeof(IMapContext), "context");
            List<Expression> sourceParamExprList = new List<Expression>();
            List<Expression> targetParamExprList = new List<Expression>();

            foreach (string memberName in targetType.MemberNames)
            {
                IMemberOptions targetMember = targetType.GetMember(memberName);
                if (targetMember.IsKey)
                {
                    keyParamTypes.Add(targetMember.ClrType);
                    Expression targetParamExpr = targetMember.BuildGetterExpression(targetExpr, contextExpr);
                    targetParamExprList.Add(targetParamExpr);

                    IMemberOptions sourceMember = sourceType.GetMember(memberName);
                    if (sourceMember == null || !sourceMember.CanRead || sourceMember.IsIgnored)
                    {
                        keyType = typeof(NoKey);
                        getSourceKeyExpr = Lambda(New(typeof(NoKey)), new[] { sourceExpr, contextExpr });
                        getTargetKeyExpr = Lambda(New(typeof(NoKey)), new[] { targetExpr, contextExpr }); ;
                        return;
                    }

                    Expression sourceParamExpr = sourceMember.BuildGetterExpression(sourceExpr, contextExpr);

                    ITypeOptions sourceMemberType = _options.GetTypeOptions(sourceMember.ClrType);
                    ITypeOptions targetMemberType = _options.GetTypeOptions(targetMember.ClrType);
                    if (_options.ShouldMap(sourceMemberType, targetMemberType))
                    {
                        ITypeMapper typeMapper = _options.GetTypeMapper(new TypePair(sourceMember.ClrType, targetMember.ClrType, TypePairFlags.None));
                        sourceParamExpr = Call("Map", Constant(typeMapper, typeMapper.GetType()), sourceParamExpr, Default(targetMember.ClrType), contextExpr);
                    }

                    sourceParamExprList.Add(sourceParamExpr);
                }
            }

            keyType = GetKeyType(keyParamTypes.ToArray());
            getSourceKeyExpr = Lambda(New(keyType, sourceParamExprList), new[] { sourceExpr, contextExpr });
            getTargetKeyExpr = Lambda(New(keyType, targetParamExprList), new[] { targetExpr, contextExpr });
        }

        protected virtual Type GetKeyType(Type[] types)
        {
            switch (types.Length)
            {
                case 1:
                    return typeof(EntityKey<>).MakeGenericType(types);
                case 2:
                    return typeof(EntityKey<,>).MakeGenericType(types);
                case 3:
                    return typeof(EntityKey<,,>).MakeGenericType(types);
                case 4:
                    return typeof(EntityKey<,,,>).MakeGenericType(types);
                case 5:
                    return typeof(EntityKey<,,,,>).MakeGenericType(types);
                default:
                    throw new InvalidOperationException("Maximum of 5 key members allowed.");
            }
        }

        public LambdaExpression BuildMapMembersExpression(
            TypePair typePair,
            ITypeOptions sourceType,
            ITypeOptions targetType,
            Func<IMemberOptions, IMemberOptions, bool> shouldMap)
        {
            return Lambda(
                    typeof(Action<,,>).MakeGenericType(typePair.SourceType, typePair.TargetType, typeof(IMapContext)),
                    Parameter("source", typePair.SourceType, out Expression sourceParamExpr),
                    Parameter("target", typePair.TargetType, out Expression targetParamExpr),
                    Parameter("context", typeof(IMapContext), out Expression context2Expr),
                    Block(
                        BuildMapAllMembersExpression(sourceType, targetType, sourceParamExpr, targetParamExpr, context2Expr, shouldMap)
                    )
                );
        }

        private Expression BuildMapAllMembersExpression(
            ITypeOptions sourceType,
            ITypeOptions targetType,
            Expression sourceExpr,
            Expression targetExpr,
            Expression contextExpr,
            Func<IMemberOptions, IMemberOptions, bool> shouldMap)
        {
            List<Expression> memberMapsExprs = new List<Expression>();

            if (targetType.MemberNames != null)
            {
                foreach (string memberName in targetType.MemberNames)
                {
                    IMemberOptions targetMember = targetType.GetMember(memberName);

                    if (targetMember.GetIsParentReference())
                    {
                        memberMapsExprs.Add(BuildFindParentExpression(targetExpr, contextExpr, targetMember));
                    }
                    else if (targetMember != null && targetMember.CanWrite && !targetMember.IsIgnored)
                    {
                        // TODO: map transform.
                        IMemberOptions sourceMember = sourceType.GetMember(memberName);

                        if (sourceMember != null && sourceMember.CanRead && !sourceMember.IsIgnored && shouldMap(sourceMember, targetMember))
                        {
                            Expression memberMapExpr = BuildMapSingleMemberExpression(sourceExpr, targetExpr, contextExpr, sourceMember, targetMember);

                            Expression isSetExpr = sourceType.BuildIsSetExpression(sourceExpr, contextExpr, targetMember.Name);
                            if (isSetExpr != null)
                            {
                                memberMapExpr = If(isSetExpr, memberMapExpr);
                            }

                            memberMapsExprs.Add(memberMapExpr);
                        }
                    }
                }
            }

            return Include(memberMapsExprs);
        }

        private Expression BuildFindParentExpression(Expression targetExpr, Expression contextExpr, IMemberOptions targetMember)
        {
            return targetMember.BuildSetterExpression(targetExpr, Call(contextExpr, "FindParent", new[] { targetMember.ClrType }), contextExpr);
        }

        private Expression BuildMapSingleMemberExpression(
            Expression sourceExpr,
            Expression targetExpr,
            Expression contextExpr,
            IMemberOptions sourceMember,
            IMemberOptions targetMember)
        {
            TypePair memberTypePair = new TypePair(sourceMember.ClrType, targetMember.ClrType, targetMember.IsComposition ? TypePairFlags.Owned : TypePairFlags.None);

            ITypeOptions sourceMemberType = _options.GetTypeOptions(sourceMember.ClrType);
            ITypeOptions targetMemberType = _options.GetTypeOptions(targetMember.ClrType);

            Expression sourceValueExpr = sourceMember.BuildGetterExpression(sourceExpr, contextExpr);

            if (_options.ShouldMap(sourceMemberType, targetMemberType))
            {
                Expression targetValueExpr = targetMember.BuildGetterExpression(targetExpr, contextExpr);
                Expression typeMapperExpr = BuildGetLazyMapperExpression(memberTypePair);
                sourceValueExpr = Call("Map", Property(typeMapperExpr, "Value"), sourceValueExpr, targetValueExpr, contextExpr);
            }

            Expression memberSetExpr = targetMember.BuildSetterExpression(targetExpr, sourceValueExpr, contextExpr);

            return memberSetExpr;
        }

        public Expression BuildGetLazyMapperExpression(TypePair typePair)
        {
            Type lazyType = typeof(LazyTypeMapper<,>).MakeGenericType(typePair.SourceType, typePair.TargetType);
            return Constant(_options.GetLazyTypeMapper(typePair), lazyType);
        }

        public LambdaExpression BuildNewExpression(ITypeOptions typeOptions)
        {
            return Lambda(
                    typeof(Func<,>).MakeGenericType(typeof(IMapContext), typeOptions.ClrType),
                    Parameter("context", typeof(IMapContext), out Expression contextExpr),
                    typeOptions.BuildNewExpression(contextExpr, Constant(null, typeof(IMapContext)))
                );
        }
    }
}
