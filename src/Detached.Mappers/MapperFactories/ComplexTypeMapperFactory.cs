using Detached.Mappers.Exceptions;
using Detached.Mappers.TypeMaps;
using Detached.PatchTypes;
using Detached.RuntimeTypes.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.MapperFactories
{
    public class ComplexTypeMapperFactory : MapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.SourceTypeOptions.IsComplex
                && typeMap.TargetTypeOptions.IsComplex;
        }

        public override LambdaExpression Create(TypeMap typeMap)
        {
            return Lambda(
                        GetDelegateType(typeMap),
                        Parameter(typeMap.SourceExpression),
                        Parameter(typeMap.TargetExpression),
                        Parameter(typeMap.BuildContextExpression),
                        Block(
                            CreateMemberMappers(typeMap),
                            If(IsNull(typeMap.SourceExpression),
                                Then(
                                    Assign(typeMap.TargetExpression, Default(typeMap.TargetExpression.Type))
                                ),
                                Else(
                                    If(IsNull(typeMap.TargetExpression),
                                        Assign(typeMap.TargetExpression, Construct(typeMap))
                                    ),
                                    CreateMembers(typeMap)
                                )
                            ),
                            Result(typeMap.TargetExpression)
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
                    Expression sourceMember = memberMap.SourceOptions.GetValue(typeMap.SourceExpression, typeMap.BuildContextExpression);
                    Expression targetMember = memberMap.TargetOptions.GetValue(typeMap.TargetExpression, typeMap.BuildContextExpression);

                    sourceMember = CallMapper(memberMap.TypeMap, sourceMember, targetMember);

                    if (typeof(IPatch).IsAssignableFrom(typeMap.SourceExpression.Type))
                    {
                        memberExprs.Add(
                            If(Call("IsSet", Convert(typeMap.SourceExpression, typeof(IPatch)), Constant(memberMap.SourceOptions.Name)),
                                memberMap.TargetOptions.SetValue(typeMap.TargetExpression, sourceMember, typeMap.BuildContextExpression)
                            )
                        );

                    }
                    else
                    {
                        memberExprs.Add(
                            memberMap.TargetOptions.SetValue(typeMap.TargetExpression, sourceMember, typeMap.BuildContextExpression)
                        );
                    }
                }
            }

            return Include(memberExprs);
        }

        public Expression Construct(TypeMap typeMap)
        {
            if (typeMap.TargetTypeOptions.DiscriminatorName != null && typeMap.DiscriminatorMember == null)
            {
                throw new MapperException($"Entity {typeMap.TargetTypeOptions.ClrType} uses inheritance but discriminator was not found in source type.");
            }

            if (typeMap.DiscriminatorMember != null)
            {
                Expression discriminator = typeMap.DiscriminatorMember.GetValue(typeMap.SourceExpression, typeMap.BuildContextExpression);
                return typeMap.TargetTypeOptions.Construct(typeMap.BuildContextExpression, discriminator);
            }
            else
            {
                return typeMap.TargetTypeOptions.Construct(typeMap.BuildContextExpression, null);
            }
        }
    }
}