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
        readonly MapperOptions _options;

        public ExpressionBuilder(MapperOptions options)
        {
            _options = options;
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

            foreach (string targetMemberName in typePair.TargetType.MemberNames)
            {
                ITypeMember targetMember = typePair.TargetType.GetMember(targetMemberName);
                if (targetMember.IsKey())
                {
                    keyParamTypes.Add(targetMember.ClrType);
                    Expression targetParamExpr = targetMember.BuildGetExpression(targetExpr, contextExpr);
                    targetParamExprList.Add(targetParamExpr);

                    string sourceMemberName = _options.GetSourcePropertyName(typePair.SourceType, typePair.TargetType, targetMemberName);

                    ITypeMember sourceMember = typePair.SourceType.GetMember(sourceMemberName);
                    if (sourceMember == null || !sourceMember.CanRead || sourceMember.IsNotMapped())
                    {
                        keyType = typeof(NoKey);
                        getSourceKeyExpr = Lambda(New(typeof(NoKey)), new[] { sourceExpr, contextExpr });
                        getTargetKeyExpr = Lambda(New(typeof(NoKey)), new[] { targetExpr, contextExpr }); ;
                        return;
                    }

                    Expression sourceParamExpr = sourceMember.BuildGetExpression(sourceExpr, contextExpr);

                    IType sourceMemberType = _options.GetType(sourceMember.ClrType);
                    IType targetMemberType = _options.GetType(targetMember.ClrType);

                    if (_options.ShouldMap(sourceMemberType, targetMemberType))
                    {
                        IType sourceDiscriminatorMemberType = _options.GetType(sourceMember.ClrType);
                        IType targetDiscriminatorMemberType = _options.GetType(targetMember.ClrType);
                        TypePair discriminatorTypePair = _options.GetTypePair(sourceDiscriminatorMemberType, targetDiscriminatorMemberType, null);


                        ITypeMapper typeMapper = _options.GetTypeMapper(discriminatorTypePair);

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
                if (memberPair.SourceMember != null)
                {
                    if (memberPair.TargetMember.IsParent())
                    {
                        memberMapsExprs.Add(BuildFindParentExpression(targetExpr, contextExpr, memberPair.TargetMember));
                    }
                    else if (isIncluded(memberPair.SourceMember, memberPair.TargetMember))
                    {
                        Expression memberMapExpr = BuildMapSingleMemberExpression(memberPair, _options, sourceExpr, targetExpr, contextExpr);
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
            IType sourceMemberType = _options.GetType(memberPair.SourceMember.ClrType);
            IType targetMemberType = _options.GetType(memberPair.TargetMember.ClrType);
            TypePair memberTypePair = mapperOptions.GetTypePair(sourceMemberType, targetMemberType, memberPair);

            Expression memberSetExpr;

            if (memberPair.SourceMember.CanTryGet)
            {
                ParameterExpression outVar = Parameter(memberPair.SourceMember.ClrType, "outVar");
                Expression sourceValueExpr = outVar;

                if (_options.ShouldMap(sourceMemberType, targetMemberType))
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

                if (_options.ShouldMap(sourceMemberType, targetMemberType))
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
            return Constant(_options.GetLazyTypeMapper(typePair), lazyType);
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
