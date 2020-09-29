using Detached.Mappers.Reflection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using static System.Linq.Expressions.Expression;
using static Detached.Mappers.Expressions.ExtendedExpression;

namespace Detached.Mappers.Patching
{
    public static class Patch
    {
        readonly static ConcurrentDictionary<Type, Type> _proxyTypes
            = new ConcurrentDictionary<Type, Type>();

        readonly static ConcurrentDictionary<Type, object> _typedFactories
           = new ConcurrentDictionary<Type, object>();

        readonly static ConcurrentDictionary<Type, Func<object>> _factories
           = new ConcurrentDictionary<Type, Func<object>>();

        public static TModel Create<TModel>()
        {
            return ((Func<TModel>)_typedFactories.GetOrAdd(typeof(TModel), type =>
            {
                Type patchType = GetType(typeof(TModel));
                return Lambda<Func<TModel>>(Convert(Expression.New(patchType), type)).Compile();
            }))();
        }

        public static object Create(Type type)
        {
            return _factories.GetOrAdd(type, t =>
            {
                Type patchType = GetType(t);
                return Lambda<Func<object>>(Convert(Expression.New(patchType), typeof(object))).Compile();
            })();
        }

        public static Type GetType(Type type)
        {
            return _proxyTypes.GetOrAdd(type, CreateType);
        }
 
        static Type CreateType(Type type)
        {
            if (type.GetConstructor(new Type[0]) == null)
                throw new PatchProxyTypeException($"Type {type} doesn't have an empty constructor.");

            RuntimeTypeBuilder proxyBuilder = new RuntimeTypeBuilder($"PatchProxyTypeFactory.{type.FullName}Patch", type);

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
