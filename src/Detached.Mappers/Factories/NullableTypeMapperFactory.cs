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
            return typeMap.TargetOptions.Type.IsNullable(out _) ^ typeMap.SourceOptions.Type.IsNullable(out _);
        }

        public override LambdaExpression Create(TypeMap typeMap)
        {
            if (typeMap.TargetOptions.Type.IsNullable(out Type baseType))
            {
                // nullable target.
                return Lambda(
                         GetDelegateType(typeMap),
                         Parameter(typeMap.SourceExpr),
                         Parameter(typeMap.TargetExpr),
                         Parameter(typeMap.BuildContextExpr),
                         Assign(typeMap.TargetExpr, Convert(typeMap.SourceExpr, typeMap.TargetExpr.Type))
                      );
            }
            else
            {
                // nullable source.
                return Lambda(
                          GetDelegateType(typeMap),
                          Parameter(typeMap.SourceExpr),
                          Parameter(typeMap.TargetExpr),
                          Parameter(typeMap.BuildContextExpr),
                          Block(
                              IfThenElse(IsNull(typeMap.SourceExpr),
                                Assign(typeMap.TargetExpr, Default(typeMap.TargetExpr.Type)),
                                Assign(typeMap.TargetExpr, Property(typeMap.SourceExpr, "Value"))
                              ),
                              Result(typeMap.TargetExpr)
                          )
                       );
            }
        }
    }
}