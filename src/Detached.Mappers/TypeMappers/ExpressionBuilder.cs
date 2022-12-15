using Detached.Mappers.Annotations;
using Detached.Mappers.TypeMappers.Entity;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
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
        readonly MapperOptions _mapperOptions;

        public ExpressionBuilder(MapperOptions mapperOptions)
        {
            _mapperOptions = mapperOptions;
        }

        public void BuildGetKeyExpressions(
            TypePair typePair,
            out LambdaExpression getSourceKeyExpr,
            out LambdaExpression getTargetKeyExpr,
            out Type keyType)
        {
            List<Type> keyParamTypes = new List<Type>();
            ParameterExpression sourceExpr = Parameter(typePair.SourceType.ClrType, "source");
            ParameterExpression targetExpr = Parameter(typePair.TargetType.ClrType, "target");
            ParameterExpression contextExpr = Parameter(typeof(IMapContext), "context");
            List<Expression> sourceParamExprList = new List<Expression>();
            List<Expression> targetParamExprList = new List<Expression>();

            foreach (TypePairMember pairMember in typePair.Members.Values)
            {
                
                if (pairMember.TargetMember.IsKey())
                {
                    keyParamTypes.Add(pairMember.TargetMember.ClrType);
                    Expression targetParamExpr = pairMember.TargetMember.BuildGetExpression(targetExpr, contextExpr);
                    targetParamExprList.Add(targetParamExpr);
 
                    if (pairMember.SourceMember == null || !pairMember.SourceMember.CanRead || pairMember.SourceMember.IsNotMapped())
                    {
                        keyType = typeof(NoKey);
                        getSourceKeyExpr = Lambda(New(typeof(NoKey)), new[] { sourceExpr, contextExpr });
                        getTargetKeyExpr = Lambda(New(typeof(NoKey)), new[] { targetExpr, contextExpr });
                        return;
                    }

                    Expression sourceParamExpr = pairMember.SourceMember.BuildGetExpression(sourceExpr, contextExpr);

                    IType sourceMemberType = _mapperOptions.GetType(pairMember.SourceMember.ClrType);
                    IType targetMemberType = _mapperOptions.GetType(pairMember.TargetMember.ClrType);

                    if (_mapperOptions.ShouldMap(sourceMemberType, targetMemberType))
                    {
                        TypePair memberTypePair = _mapperOptions.GetTypePair(sourceMemberType, targetMemberType, null);
                        ITypeMapper typeMapper = _mapperOptions.GetTypeMapper(memberTypePair);

                        sourceParamExpr = Call("Map", Constant(typeMapper, typeMapper.GetType()), sourceParamExpr, Default(targetMemberType.ClrType), contextExpr);
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

        public LambdaExpression BuildMapMembersExpression(TypePair typePair, Func<ITypeMember, ITypeMember, bool> isIncluded)
        {
            return Lambda(
                    typeof(Action<,,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType, typeof(IMapContext)),
                    Parameter("source", typePair.SourceType.ClrType, out Expression sourceParamExpr),
                    Parameter("target", typePair.TargetType.ClrType, out Expression targetParamExpr),
                    Parameter("context", typeof(IMapContext), out Expression context2Expr),
                    Block(
                        BuildMapAllMembersExpression(typePair, sourceParamExpr, targetParamExpr, context2Expr, isIncluded)
                    )
                );
        }

        private Expression BuildMapAllMembersExpression(
            TypePair typePair,
            Expression sourceExpr,
            Expression targetExpr,
            Expression contextExpr,
            Func<ITypeMember, ITypeMember, bool> isIncluded)
        {
            List<Expression> memberMapsExprs = new List<Expression>();

            foreach (TypePairMember memberPair in typePair.Members.Values)
            {
                if (!memberPair.IsNotMapped())
                {
                    if (memberPair.TargetMember.IsParent())
                    {
                        memberMapsExprs.Add(BuildFindParentExpression(targetExpr, contextExpr, memberPair.TargetMember));
                    }
                    else if (isIncluded(memberPair.SourceMember, memberPair.TargetMember))
                    {
                        Expression memberMapExpr = BuildMapSingleMemberExpression(memberPair, _mapperOptions, sourceExpr, targetExpr, contextExpr);
                        memberMapsExprs.Add(memberMapExpr);
                    }
                }
            }

            return Include(memberMapsExprs);
        }

        private Expression BuildFindParentExpression(Expression targetExpr, Expression contextExpr, ITypeMember targetMember)
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
            TypePairMember memberPair,
            MapperOptions mapperOptions,
            Expression sourceExpr,
            Expression targetExpr,
            Expression contextExpr)
        {
            IType sourceMemberType = _mapperOptions.GetType(memberPair.SourceMember.ClrType);
            IType targetMemberType = _mapperOptions.GetType(memberPair.TargetMember.ClrType);
            TypePair memberTypePair = mapperOptions.GetTypePair(sourceMemberType, targetMemberType, memberPair);

            Expression memberSetExpr;

            if (memberPair.SourceMember.CanTryGet)
            {
                ParameterExpression outVar = Parameter(memberPair.SourceMember.ClrType, "outVar");
                Expression sourceValueExpr = outVar;

                if (_mapperOptions.ShouldMap(sourceMemberType, targetMemberType))
                {
                    Expression targetValueExpr = memberPair.TargetMember.BuildGetExpression(targetExpr, contextExpr);
                    Expression typeMapperExpr = BuildGetLazyMapperExpression(memberTypePair);

                    sourceValueExpr = Call("Map", Property(typeMapperExpr, "Value"), sourceValueExpr, targetValueExpr, contextExpr);
                }

                memberSetExpr = Block(
                    Variable(outVar),
                    If(memberPair.SourceMember.BuildTryGetExpression(sourceExpr, contextExpr, outVar),
                       memberPair.TargetMember.BuildSetExpression(targetExpr, sourceValueExpr, contextExpr)
                    )
                );
            }
            else
            {
                Expression sourceValueExpr = memberPair.SourceMember.BuildGetExpression(sourceExpr, contextExpr);

                if (_mapperOptions.ShouldMap(sourceMemberType, targetMemberType))
                {
                    Expression targetValueExpr = memberPair.TargetMember.BuildGetExpression(targetExpr, contextExpr);
                    Expression typeMapperExpr = BuildGetLazyMapperExpression(memberTypePair);
                    sourceValueExpr = Call("Map", Property(typeMapperExpr, "Value"), sourceValueExpr, targetValueExpr, contextExpr);
                }

                memberSetExpr = memberPair.TargetMember.BuildSetExpression(targetExpr, sourceValueExpr, contextExpr);
            }

            return memberSetExpr;
        }

        public Expression BuildGetLazyMapperExpression(TypePair typePair)
        {
            Type lazyType = typeof(LazyTypeMapper<,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType);
            return Constant(_mapperOptions.GetLazyTypeMapper(typePair), lazyType);
        }

        public LambdaExpression BuildNewExpression(IType typeOptions)
        {
            return Lambda(
                    typeof(Func<,>).MakeGenericType(typeof(IMapContext), typeOptions.ClrType),
                    Parameter("context", typeof(IMapContext), out Expression contextExpr),
                    typeOptions.BuildNewExpression(contextExpr, Constant(null, typeof(IMapContext)))
                );
        }
    }
}