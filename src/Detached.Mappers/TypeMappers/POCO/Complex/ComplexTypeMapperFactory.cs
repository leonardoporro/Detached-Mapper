using Detached.Mappers.Context;
using Detached.Mappers.TypeOptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.TypeMappers.POCO.Complex
{
    public class ComplexTypeMapperFactory : ITypeMapperFactory
    {
        readonly MapperOptions _options;

        public ComplexTypeMapperFactory(MapperOptions options)
        {
            _options = options;
        }

        public bool CanCreate(TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
        {
            return sourceType.IsComplex
                && targetType.IsComplex
                && !targetType.IsEntity;
        }

        public ITypeMapper Create(TypePair typePair, ITypeOptions sourceType, ITypeOptions targetType)
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
                        CreateAllMembersMapExpressions(_options, sourceType, targetType, sourceParamExpr, targetParamExpr, context2Expr)
                    )
                ).Compile();

            Type mapperType = typeof(ComplexTypeMapper<,>).MakeGenericType(typePair.SourceType, typePair.TargetType);
            return (ITypeMapper)Activator.CreateInstance(mapperType, new[] { construct, mapMembers });
        }

        private Expression CreateAllMembersMapExpressions(
            MapperOptions options,
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
                    Expression memberMapExpr = CreateMemberMapExpression(options, sourceType, targetType, sourceExpr, targetExpr, contextExpr, memberName);
                    if (memberMapExpr != null)
                    {
                        memberMapsExprs.Add(memberMapExpr);
                    }
                }
            }

            return Include(memberMapsExprs);
        }

        private Expression CreateMemberMapExpression(
            MapperOptions options,
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
                    TypePair memberTypePair = new TypePair(sourceMember.ClrType, targetMember.ClrType, targetMember.IsComposition ? TypePairFlags.Owned : TypePairFlags.None);

                    ITypeOptions sourceMemberType = options.GetTypeOptions(sourceMember.ClrType);
                    ITypeOptions targetMemberType = options.GetTypeOptions(targetMember.ClrType);

                    Expression sourceValueExpr = sourceMember.GetValue(sourceExpr, contextExpr);

                    if (options.ShouldMap(sourceMemberType, targetMemberType))
                    {
                        Expression targetValueExpr = targetMember.GetValue(targetExpr, contextExpr);
                        Expression typeMapperExpr = CreateLazyTypeMappperExpression(options, memberTypePair);
                        sourceValueExpr = Call("Map", Property(typeMapperExpr, "Value"), sourceValueExpr, targetValueExpr, contextExpr);
                    }

                    memberSetExpr = targetMember.SetValue(targetExpr, sourceValueExpr, contextExpr);
                }
            }

            return memberSetExpr;
        }

        public Expression CreateLazyTypeMappperExpression(MapperOptions options, TypePair typePair)
        {
            Type lazyType = typeof(LazyTypeMapper<,>).MakeGenericType(typePair.SourceType, typePair.TargetType);
            return Constant(options.GetLazyTypeMapper(typePair), lazyType);
        }

    }
}