using AgileObjects.ReadableExpressions;
using Detached.Mappers.Annotations;
using Detached.Mappers.TypeMappers.Entity;
using Detached.Mappers.TypeOptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
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

            foreach (string targetMemberName in targetType.MemberNames)
            {
                IMemberOptions targetMember = targetType.GetMember(targetMemberName);
                if (targetMember.IsKey())
                {
                    keyParamTypes.Add(targetMember.ClrType);
                    Expression targetParamExpr = targetMember.BuildGetExpression(targetExpr, contextExpr);
                    targetParamExprList.Add(targetParamExpr);

                    string sourceMemberName = _options.GetSourcePropertyName(sourceType, targetType, targetMemberName);

                    IMemberOptions sourceMember = sourceType.GetMember(sourceMemberName);
                    if (sourceMember == null || !sourceMember.CanRead || sourceMember.IsNotMapped())
                    {
                        keyType = typeof(NoKey);
                        getSourceKeyExpr = Lambda(New(typeof(NoKey)), new[] { sourceExpr, contextExpr });
                        getTargetKeyExpr = Lambda(New(typeof(NoKey)), new[] { targetExpr, contextExpr }); ;
                        return;
                    }

                    Expression sourceParamExpr = sourceMember.BuildGetExpression(sourceExpr, contextExpr);

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
                case 0:
                    return typeof(NoKey);
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
            Func<IMemberOptions, IMemberOptions, bool> isIncluded)
        {
            List<Expression> memberMapsExprs = new List<Expression>();

            if (targetType.MemberNames != null)
            {
                foreach (string targetMemberName in targetType.MemberNames)
                {
                    IMemberOptions targetMember = targetType.GetMember(targetMemberName);

                    if (targetMember.IsParent())
                    {
                        memberMapsExprs.Add(BuildFindParentExpression(targetExpr, contextExpr, targetMember));
                    }
                    else if (targetMember != null && targetMember.CanWrite && !targetMember.IsNotMapped())
                    {
                        string sourceMemberName = _options.GetSourcePropertyName(sourceType, targetType, targetMemberName);
 
                        IMemberOptions sourceMember = sourceType.GetMember(sourceMemberName);

                        if (sourceMember != null && sourceMember.CanRead && !sourceMember.IsNotMapped() && isIncluded(sourceMember, targetMember))
                        {
                            Expression memberMapExpr = BuildMapSingleMemberExpression(sourceExpr, targetExpr, contextExpr, sourceMember, targetMember);
                            memberMapsExprs.Add(memberMapExpr);
                        }
                    }
                }
            }

            return Include(memberMapsExprs);
        }

        private Expression BuildFindParentExpression(Expression targetExpr, Expression contextExpr, IMemberOptions targetMember)
        {
            MethodInfo methodInfo = typeof(IMapContext).GetMethod("TryGetParent").MakeGenericMethod(targetMember.ClrType);

            return Block(
                Variable("parent", targetMember.ClrType, out Expression parentExpr),
                If(And(Call(contextExpr, methodInfo, parentExpr), ReferenceNotEqual(Convert(targetExpr, typeof(object)), parentExpr)),
                    targetMember.BuildSetExpression(targetExpr, parentExpr, contextExpr)
                )
            );
        }

        private Expression BuildMapSingleMemberExpression(
            Expression sourceExpr,
            Expression targetExpr,
            Expression contextExpr,
            IMemberOptions sourceMember,
            IMemberOptions targetMember)
        {
            TypePair memberTypePair = new TypePair(sourceMember.ClrType, targetMember.ClrType, targetMember.IsComposition() ? TypePairFlags.Owned : TypePairFlags.None);

            ITypeOptions sourceMemberType = _options.GetTypeOptions(sourceMember.ClrType);
            ITypeOptions targetMemberType = _options.GetTypeOptions(targetMember.ClrType);

            Expression memberSetExpr;

            if (sourceMember.CanTryGet)
            {
                ParameterExpression outVar = Parameter(sourceMember.ClrType, "outVar");
                Expression sourceValueExpr = outVar;

                if (_options.ShouldMap(sourceMemberType, targetMemberType))
                {
                    Expression targetValueExpr = targetMember.BuildGetExpression(targetExpr, contextExpr);
                    Expression typeMapperExpr = BuildGetLazyMapperExpression(memberTypePair);
                    sourceValueExpr = Call("Map", Property(typeMapperExpr, "Value"), sourceValueExpr, targetValueExpr, contextExpr);
                }

                memberSetExpr = Block(
                    Variable(outVar),
                    If(sourceMember.BuildTryGetExpression(sourceExpr, contextExpr, outVar),
                       targetMember.BuildSetExpression(targetExpr, sourceValueExpr, contextExpr)
                    )
                );
            }
            else
            {
                Expression sourceValueExpr = sourceMember.BuildGetExpression(sourceExpr, contextExpr);

                if (_options.ShouldMap(sourceMemberType, targetMemberType))
                {
                    Expression targetValueExpr = targetMember.BuildGetExpression(targetExpr, contextExpr);
                    Expression typeMapperExpr = BuildGetLazyMapperExpression(memberTypePair);
                    sourceValueExpr = Call("Map", Property(typeMapperExpr, "Value"), sourceValueExpr, targetValueExpr, contextExpr);
                }

                memberSetExpr = targetMember.BuildSetExpression(targetExpr, sourceValueExpr, contextExpr);
            }

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
