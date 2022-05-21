using Detached.Mappers.Exceptions;
using Detached.Mappers.TypeMaps;
using Detached.PatchTypes;
using Detached.RuntimeTypes.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Factories
{
    public class ComplexTypeMapperFactory : MapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.SourceOptions.IsComplexType
                && typeMap.TargetOptions.IsComplexType;
        }

        public override LambdaExpression Create(TypeMap typeMap)
        {
            return Lambda(
                        GetDelegateType(typeMap),
                        Parameter(typeMap.SourceExpr),
                        Parameter(typeMap.TargetExpr),
                        Parameter(typeMap.BuildContextExpr),
                        Block(
                            CreateMemberMappers(typeMap),
                            If(IsNull(typeMap.SourceExpr),
                                Then(
                                    Assign(typeMap.TargetExpr, Default(typeMap.TargetExpr.Type))
                                ),
                                Else(
                                    If(IsNull(typeMap.TargetExpr),
                                        Assign(typeMap.TargetExpr, Construct(typeMap))
                                    ),
                                    CreateMembers(typeMap)
                                )
                            ),
                            Result(typeMap.TargetExpr)
                        )
                    );
        }

        protected virtual IncludeExpression CreateMembers(TypeMap typeMap, Func<MemberMap, bool> filter = null)
        {
            List<Expression> memberExprs = new List<Expression>();

            foreach (MemberMap memberMap in typeMap.Members)
            {
                if (filter == null || filter(memberMap))
                {
                    Expression sourceMember = memberMap.SourceOptions.GetValue(typeMap.SourceExpr, typeMap.BuildContextExpr);
                    Expression targetMember = memberMap.TargetOptions.GetValue(typeMap.TargetExpr, typeMap.BuildContextExpr);

                    sourceMember = CallMapper(memberMap.TypeMap, sourceMember, targetMember);

                    if (typeof(IPatch).IsAssignableFrom(typeMap.SourceExpr.Type))
                    {
                        memberExprs.Add(
                            If(Call("IsSet", Convert(typeMap.SourceExpr, typeof(IPatch)), Constant(memberMap.SourceOptions.Name)),
                                memberMap.TargetOptions.SetValue(typeMap.TargetExpr, sourceMember, typeMap.BuildContextExpr)
                            )
                        );

                    }
                    else
                    {
                        memberExprs.Add(
                            memberMap.TargetOptions.SetValue(typeMap.TargetExpr, sourceMember, typeMap.BuildContextExpr)
                        );
                    }
                }
            }

            return Include(memberExprs);
        }

        public Expression Construct(TypeMap typeMap)
        {
            if (typeMap.TargetOptions.DiscriminatorName != null && typeMap.Discriminator == null)
            {
                throw new MapperException($"Entity {typeMap.TargetOptions.Type} uses inheritance but discriminator was not found in source type.");
            }

            if (typeMap.Discriminator != null)
            {
                Expression discriminator = typeMap.Discriminator.GetValue(typeMap.SourceExpr, typeMap.BuildContextExpr);
                return typeMap.TargetOptions.Construct(typeMap.BuildContextExpr, discriminator);
            }
            else
            {
                return typeMap.TargetOptions.Construct(typeMap.BuildContextExpr, null);
            }
        }
    }
}