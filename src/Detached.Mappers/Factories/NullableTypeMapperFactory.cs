using Detached.Mappers.TypeMaps;
using Detached.RuntimeTypes.Reflection;
using System;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Factories
{
    public class NullableTypeMapperFactory : MapperFactory
    {
        public override bool CanMap(TypeMap typeMap)
        {
            return typeMap.TargetTypeOptions.Type.IsNullable(out _) ^ typeMap.SourceTypeOptions.Type.IsNullable(out _);
        }

        public override LambdaExpression Create(TypeMap typeMap)
        {
            if (typeMap.TargetTypeOptions.Type.IsNullable(out Type baseType))
            {
                // nullable target.
                return Lambda(
                         GetDelegateType(typeMap),
                         Parameter(typeMap.SourceExpression),
                         Parameter(typeMap.TargetExpression),
                         Parameter(typeMap.BuildContextExpression),
                         Assign(typeMap.TargetExpression, Convert(typeMap.SourceExpression, typeMap.TargetExpression.Type))
                      );
            }
            else
            {
                // nullable source.
                return Lambda(
                          GetDelegateType(typeMap),
                          Parameter(typeMap.SourceExpression),
                          Parameter(typeMap.TargetExpression),
                          Parameter(typeMap.BuildContextExpression),
                          Block(
                              IfThenElse(IsNull(typeMap.SourceExpression),
                                Assign(typeMap.TargetExpression, Default(typeMap.TargetExpression.Type)),
                                Assign(typeMap.TargetExpression, Property(typeMap.SourceExpression, "Value"))
                              ),
                              Result(typeMap.TargetExpression)
                          )
                       );
            }
        }
    }
}