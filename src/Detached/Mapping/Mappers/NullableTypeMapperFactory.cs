using Detached.Reflection;
using System;
using System.Linq.Expressions;
using static Detached.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mapping.Mappers
{
    public class NullableTypeMapperFactory : MapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.TargetOptions.Type.IsNullable(out _) ^ typeMap.SourceOptions.Type.IsNullable(out _);
        }

        public override LambdaExpression Create(TypeMap typeMap)
        {
            if (typeMap.TargetOptions.Type.IsNullable(out Type baseType))
            {
                // nullable target.
                return Lambda(
                         GetDelegateType(typeMap),
                         Parameter(typeMap.Source),
                         Parameter(typeMap.Target),
                         Parameter(typeMap.Context),
                         Assign(typeMap.Target, Convert(typeMap.Source, typeMap.Target.Type))
                      );
            }
            else
            {
                // nullable source.
                return Lambda(
                          GetDelegateType(typeMap),
                          Parameter(typeMap.Source),
                          Parameter(typeMap.Target),
                          Parameter(typeMap.Context),
                          Block(
                              IfThenElse(IsNull(typeMap.Source),
                                Assign(typeMap.Target, Default(typeMap.Target.Type)),
                                Assign(typeMap.Target, Property(typeMap.Source, "Value"))
                              ),
                              Result(typeMap.Target)
                          )
                       );
            }
        }
    }
}