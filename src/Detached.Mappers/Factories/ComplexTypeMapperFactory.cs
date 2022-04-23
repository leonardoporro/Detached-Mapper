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
                        Parameter(typeMap.Source),
                        Parameter(typeMap.Target),
                        Parameter(typeMap.Context),
                        Block(
                            CreateMemberMappers(typeMap),
                            If(IsNull(typeMap.Source),
                                Then(
                                    Assign(typeMap.Target, Default(typeMap.Target.Type))
                                ),
                                Else(
                                    If(IsNull(typeMap.Target),
                                        Assign(typeMap.Target, Construct(typeMap))
                                    ),
                                    CreateMembers(typeMap)
                                )
                            ),
                            Result(typeMap.Target)
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
                    Expression sourceMember = memberMap.SourceOptions.GetValue(typeMap.Source, typeMap.Context);
                    Expression targetMember = memberMap.TargetOptions.GetValue(typeMap.Target, typeMap.Context);

                    sourceMember = CallMapper(memberMap.TypeMap, sourceMember, targetMember);

                    if (typeof(IPatch).IsAssignableFrom(typeMap.Source.Type))
                    {
                        memberExprs.Add(
                            If(Call("IsSet", Convert(typeMap.Source, typeof(IPatch)), Constant(memberMap.SourceOptions.Name)),
                                memberMap.TargetOptions.SetValue(typeMap.Target, sourceMember, typeMap.Context)
                            )
                        );

                    }
                    else
                    {
                        memberExprs.Add(
                            memberMap.TargetOptions.SetValue(typeMap.Target, sourceMember, typeMap.Context)
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
                Expression discriminator = typeMap.Discriminator.GetValue(typeMap.Source, typeMap.Context);
                return typeMap.TargetOptions.Construct(typeMap.Context, discriminator);
            }
            else
            {
                return typeMap.TargetOptions.Construct(typeMap.Context, null);
            }
        }
    }
}