using Detached.Mappers.Context;
using Detached.Mappers.TypeOptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.TypeMappers.ComplexType
{
    public class ComplexTypeMapperFactory : ITypeMapperFactory
    {
        public bool CanCreate(Mapper mapper, TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            return sourceType.IsComplex
                && targetType.IsComplex
                && !targetType.IsEntity;
        }

        public ITypeMapper Create(Mapper mapper, TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            object construct =
                Lambda(
                    typeof(Func<,>).MakeGenericType(typeof(IMapperContext), typePair.TargetType),
                    Parameter("source", typeof(IMapperContext), out Expression contextExpr),
                    targetType.Construct(contextExpr, Constant(null, typeof(IMapperContext)))
                ).Compile();

            object mapMembers = 
                Lambda(
                    typeof(Action<,,>).MakeGenericType(typePair.SourceType, typePair.TargetType, typeof(IMapperContext)),
                    Parameter("source", typePair.SourceType, out Expression sourceParamExpr),
                    Parameter("source", typePair.TargetType, out Expression targetParamExpr),
                    Parameter("source", typeof(IMapperContext), out Expression context2Expr),
                    Block(
                        CreateAllMembersMapExpressions(mapper, sourceType, targetType, sourceParamExpr, targetParamExpr, context2Expr)
                    )
                ).Compile();

            Type mapperType = typeof(ComplexTypeMapper<,>).MakeGenericType(typePair.SourceType, typePair.TargetType);
            return (ITypeMapper)Activator.CreateInstance(mapperType, new[] { construct, mapMembers });
        }

        private Expression CreateAllMembersMapExpressions(
            Mapper mapper,
            ITypeOptions sourceType,
            ITypeOptions targetType,
            Expression sourceExpr,
            Expression targetExpr,
            Expression contextExpr)
        {
            List<Expression> memberMapsExprs = new List<Expression>();

            if (targetType.MemberNames != null)
            {
                foreach (string memberName in targetType.MemberNames)
                {
                    Expression memberMapExpr = CreateMemberMapExpression(mapper, sourceType, targetType, sourceExpr, targetExpr, contextExpr, memberName);
                    if (memberMapExpr != null)
                    {
                        memberMapsExprs.Add(memberMapExpr);
                    }
                }
            }

            return Include(memberMapsExprs);
        }

        private Expression CreateMemberMapExpression(
            Mapper mapper,
            ITypeOptions sourceType,
            ITypeOptions targetType,
            Expression sourceExpr,
            Expression targetExpr,
            Expression contextExpr,
            string memberName)
        {
            Expression memberSetExpr = null;

            IMemberOptions targetMember = targetType.GetMember(memberName);

            if (targetMember != null && targetMember.CanWrite && !targetMember.Ignored)
            {
                IMemberOptions sourceMember = sourceType.GetMember(memberName);

                if (sourceMember != null && sourceMember.CanRead && !sourceMember.Ignored)
                {
                    TypePair memberTypePair = new TypePair(sourceMember.Type, targetMember.Type, targetMember.IsComposition ? TypePairFlags.Owned : TypePairFlags.None);

                    ITypeOptions sourceMemberType = mapper.GetTypeOptions(sourceMember.Type);
                    ITypeOptions targetMemberType = mapper.GetTypeOptions(targetMember.Type);

                    Expression sourceValueExpr = sourceMember.GetValue(sourceExpr, contextExpr);

                    if (mapper.ShouldMap(sourceMemberType, targetMemberType))
                    {
                        Expression targetValueExpr = targetMember.GetValue(targetExpr, contextExpr);
                        Expression typeMapperExpr = CreateLazyTypeMappperExpression(mapper, memberTypePair);
                        sourceValueExpr = Call("Map", Property(typeMapperExpr, "Value"), sourceValueExpr, targetValueExpr, contextExpr);
                    }

                    memberSetExpr = targetMember.SetValue(targetExpr, sourceValueExpr, contextExpr);
                }
            }

            return memberSetExpr;
        }

        public Expression CreateLazyTypeMappperExpression(Mapper mapper, TypePair typePair)
        {
            Type lazyType = typeof(LazyTypeMapper<,>).MakeGenericType(typePair.SourceType, typePair.TargetType);
            return Constant(mapper.GetLazyTypeMapper(typePair), lazyType);
        }
         
    }
}