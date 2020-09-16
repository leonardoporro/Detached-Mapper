using Detached.Reflection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using static Detached.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Patch
{
    public class PatchProxyTypeFactory
    {
        readonly ConcurrentDictionary<Type, Type> _proxyTypes
            = new ConcurrentDictionary<Type, Type>();

        public Type Create(Type type)
        {
            return _proxyTypes.GetOrAdd(type, CreateProxyType);
        }

        public Type Create<TModel>() => Create(typeof(TModel));

        protected Type CreateProxyType(Type type)
        {
            if (type.GetConstructor(new Type[0]) == null)
                throw new PatchProxyTypeException($"Type {type} doesn't have an empty constructor.");

            RuntimeTypeBuilder proxyBuilder = new RuntimeTypeBuilder($"{Guid.NewGuid()}.{type.FullName}Patch", type);

            FieldBuilder modified = proxyBuilder.DefineField("_modified", typeof(HashSet<string>), FieldAttributes.Private);
            var modifiedField = Field(proxyBuilder.This, modified);

            foreach (PropertyInfo propInfo in type.GetRuntimeProperties())
            {
                if (propInfo.CanRead && propInfo.CanWrite && propInfo.GetSetMethod().IsVirtual)
                {
                    ParameterExpression valueExpr = Parameter(propInfo.PropertyType, "value");

                    var baseGet = Call(proxyBuilder.Base, propInfo.GetGetMethod());

                    proxyBuilder.OverrideMethod(propInfo.GetSetMethod(),
                        new[] { valueExpr },
                        If(NotEqual(baseGet, valueExpr),
                            Then(
                                If(IsNull(modifiedField),
                                    Assign(modifiedField, New(typeof(HashSet<string>)))
                                ),
                                Call("Add", modifiedField, Constant(propInfo.Name)),
                                Call(proxyBuilder.Base, propInfo.SetMethod, valueExpr)
                            )
                        )
                    );
                }
            }

            proxyBuilder.DefineMethod("Reset", null,
                Block(
                    If(IsNotNull(modifiedField),
                        Call("Clear", modifiedField)
                    )
                )
            );

            var propNameParam = Parameter(typeof(string), "propName");
            proxyBuilder.DefineMethod("IsSet", 
               new[] { propNameParam },
               Block(
                   Variable("result", Constant(false), out Expression result),
                   If(IsNotNull(modifiedField),
                       Assign(result, Call("Contains", modifiedField, propNameParam))
                   ),
                   Result(result)
               )
           );

            proxyBuilder.AutoImplementInterface(typeof(IPatch));

            return proxyBuilder.Create();
        }
    }
}
