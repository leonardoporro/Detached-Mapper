using Detached.Mappers.HotChocolate.Extensions;
using Detached.Mappers.Options;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.HotChocolate.Types
{
    public class OptionalClassTypeFactory : ClassTypeFactory
    {
        public const string OPTIONAL_ANNOTATION = "HOTCHOCOLATE_OPTIONAL";

        public override IType Create(MapperOptions options, Type type)
        {
            ClassType result = null;

            if (type.IsOptional())
            {
                result = (ClassType)base.Create(options, type);

                result.ItemClrType = type.GetGenericArguments()[0];
                result.Annotations.Add(OPTIONAL_ANNOTATION, true);
            }
            else if (type.GetRuntimeProperties().Any(p => p.PropertyType.IsOptional()))
            {
                result = (ClassType)base.Create(options, type);

                foreach (var member in result.Members)
                {
                    if (member.ClrType.IsOptional())
                    {
                        member.TryGetter = Lambda(
                            Parameter(result.ClrType, out Expression instanceExpr),
                            Parameter(member.PropertyInfo.PropertyType, out Expression outVar),
                            Block(
                                Variable(typeof(bool), out Expression resultExpr),
                                If(Property(Property(instanceExpr, member.PropertyInfo), "HasValue"),
                                    Block(
                                        Assign(outVar, Property(instanceExpr, member.PropertyInfo)),
                                        Assign(resultExpr, Constant(true))
                                    ),
                                    Block(
                                        Assign(outVar, Default(outVar.Type)),
                                        Assign(resultExpr, Constant(false))
                                    )
                                ),
                                resultExpr
                            ));
                    }
                }
            }

            return result;
        }
    }
}