using Detached.Mappers.Exceptions;
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
        readonly Mapper _mapper;

        public ExpressionBuilder(Mapper mapper)
        {
            _mapper = mapper;
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
                if (pairMember.IsKey())
                {
                    keyParamTypes.Add(pairMember.TargetMember.ClrType);
                    Expression targetParamExpr = pairMember.TargetMember.BuildGetExpression(targetExpr, contextExpr);
                    targetParamExprList.Add(targetParamExpr);
 
                    if (pairMember.SourceMember == null || !pairMember.SourceMember.CanRead || pairMember.SourceMember.IsIgnored())
                    {
                        keyType = typeof(NoKey);
                        getSourceKeyExpr = Lambda(New(typeof(NoKey)), new[] { sourceExpr, contextExpr });
                        getTargetKeyExpr = Lambda(New(typeof(NoKey)), new[] { targetExpr, contextExpr });
                        return;
                    }

                    Expression sourceParamExpr = pairMember.SourceMember.BuildGetExpression(sourceExpr, contextExpr);

                    IType sourceMemberType = _mapper.Options.GetType(pairMember.SourceMember.ClrType);
                    IType targetMemberType = _mapper.Options.GetType(pairMember.TargetMember.ClrType);

                    if (_mapper.Options.ShouldMap(sourceMemberType, targetMemberType))
                    {
                        TypePair memberTypePair = _mapper.Options.GetTypePair(sourceMemberType, targetMemberType, null);
                        ITypeMapper typeMapper = _mapper.GetTypeMapper(memberTypePair);

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
                if (!memberPair.IsIgnored())
                {
                    if (memberPair.TargetMember.IsParent())
                    {
                        memberMapsExprs.Add(BuildFindParentExpression(targetExpr, contextExpr, memberPair.TargetMember));
                    }
                    else if (memberPair.SourceMember != null && isIncluded(memberPair.SourceMember, memberPair.TargetMember))
                    {
                        Expression memberMapExpr = BuildMapSingleMemberExpression(memberPair, sourceExpr, targetExpr, contextExpr);
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
            Expression sourceExpr,
            Expression targetExpr,
            Expression contextExpr)
        {
            IType sourceMemberType = _mapper.Options.GetType(memberPair.SourceMember.ClrType);
            IType targetMemberType = _mapper.Options.GetType(memberPair.TargetMember.ClrType);
            TypePair memberTypePair = _mapper.Options.GetTypePair(sourceMemberType, targetMemberType, memberPair);

            Expression memberSetExpr;

            if (memberPair.SourceMember.CanTryGet)
            {
                ParameterExpression outVar = Parameter(memberPair.SourceMember.ClrType, "outVar");
                Expression sourceValueExpr = outVar;

                if (_mapper.Options.ShouldMap(sourceMemberType, targetMemberType))
                {
                    Expression targetValueExpr = memberPair.TargetMember.BuildGetExpression(targetExpr, contextExpr);
                    Expression typeMapperExpr = BuildGetMapperExpression(memberTypePair);

                    sourceValueExpr = Call("Map", typeMapperExpr, sourceValueExpr, targetValueExpr, contextExpr);
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

                bool forceCopy = memberPair.IsSetAsPrimitive();

                if (forceCopy && sourceMemberType.ClrType != targetMemberType.ClrType)
                {
                    throw new MapperException($"[Primitive] can only be used when source and target types are the same. Source = {sourceMemberType.ClrType}, Target = {targetMemberType.ClrType}");
                }

                if (!forceCopy && _mapper.Options.ShouldMap(sourceMemberType, targetMemberType))
                {
                    Expression targetValueExpr = memberPair.TargetMember.BuildGetExpression(targetExpr, contextExpr);
                    Expression typeMapperExpr = BuildGetMapperExpression(memberTypePair);
                    sourceValueExpr = Call("Map", typeMapperExpr, sourceValueExpr, targetValueExpr, contextExpr);
                }

                memberSetExpr = memberPair.TargetMember.BuildSetExpression(targetExpr, sourceValueExpr, contextExpr);
            }

            return memberSetExpr;
        }

        public Expression BuildGetMapperExpression(TypePair typePair)
        {
            Type lazyType = typeof(LazyTypeMapper<,>).MakeGenericType(typePair.SourceType.ClrType, typePair.TargetType.ClrType);

            var memberMapper = (ITypeMapper)Activator.CreateInstance(lazyType, _mapper, typePair);

            return Constant(memberMapper, lazyType);
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