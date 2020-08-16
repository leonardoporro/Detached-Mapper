using Detached.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Detached.Expressions
{
    public abstract partial class ExtendedExpression : Expression
    {
        public override Type Type => typeof(void);

        public override ExpressionType NodeType => ExpressionType.Extension;

        public override bool CanReduce => true;

        public static IncludeExpression Include(IEnumerable<Expression> expressions)
            => new IncludeExpression(expressions);

        public static LambdaExpression Lambda(Type delegateType, IEnumerable<Expression> expressions)
        {
            List<ParameterExpression> paramExprs = new List<ParameterExpression>();
            List<Expression> bodyExprs = new List<Expression>();
            ReturnTypeExpression returnExpr = null;

            foreach (Expression expression in expressions)
            {
                switch (expression)
                {
                    case VariableDefinitionExpression varExpr:
                        bodyExprs.Add(expression);
                        break;
                    case ParameterDefinitionExpression defExpr:
                        paramExprs.Add(defExpr.ParameterExpression);
                        break;
                    case ReturnTypeExpression retExpr:
                        returnExpr = retExpr;
                        break;
                    default:
                        bodyExprs.Add(expression);
                        break;
                }
            }

            if (returnExpr != null)
            {
                bodyExprs.Add(Expression.Label(returnExpr.LabelTarget, Expression.Default(returnExpr.LabelTarget.Type)));
            }

            Expression bodyExpr;
            if (bodyExprs.Count == 1)
                bodyExpr = bodyExprs[0];
            else
                bodyExpr = Block(bodyExprs);

            if (delegateType != null)
                return Expression.Lambda(delegateType, bodyExpr, paramExprs);
            else
                return Expression.Lambda(bodyExpr, paramExprs);
        }

        public static LambdaExpression Lambda(Type delegateType, params Expression[] expressions) 
            => Lambda(delegateType, (IEnumerable<Expression>)expressions);

        public static LambdaExpression Lambda(params Expression[] expressions) 
            => Lambda(null, (IEnumerable<Expression>)expressions);

        public static ReturnTypeExpression ReturnType(Type type, out LabelTarget labelTarget) 
            => new ReturnTypeExpression(type, out labelTarget);

        public static ParameterDefinitionExpression Parameter(Type type, out Expression paramExpr)
        {
            return new ParameterDefinitionExpression(null, type, out paramExpr);
        }

        public static ParameterDefinitionExpression Parameter(string name, Type type, out Expression paramExpr)
        {
            return new ParameterDefinitionExpression(name, type, out paramExpr);
        }

        public static ParameterDefinitionExpression Parameter(ParameterExpression paramExpr)
        {
            return new ParameterDefinitionExpression(paramExpr);
        }

        public static VariableDefinitionExpression Variable(string name, Type type, out Expression varExpr)
        {
            return new VariableDefinitionExpression(name, type, out varExpr);
        }

        public static VariableDefinitionExpression Variable(ParameterExpression variable, Expression initialize)
        {
            return new VariableDefinitionExpression(variable, initialize);
        }

        public static VariableDefinitionExpression Variable(ParameterExpression parameter)
        {
            return new VariableDefinitionExpression(parameter);
        }

        public static VariableDefinitionExpression Variable(Type type, out Expression varExpr)
        {
            return new VariableDefinitionExpression(null, type, out varExpr);
        }

        public static VariableDefinitionExpression Variable(string name, Expression initialize, out Expression varExpr)
        {
            return new VariableDefinitionExpression(name, initialize, out varExpr);
        }

        public static VariableDefinitionExpression Variable(Expression initialize, out Expression varExpr)
        {
            return new VariableDefinitionExpression(null, initialize, out varExpr);
        }

        public static VariableDefinitionExpression Variable(ParameterExpression param, out Expression varExpr)
        {
            varExpr = param;
            return new VariableDefinitionExpression(param);
        }

        public static Expression While(Expression conditionExpr, Expression bodyExpr) => While(conditionExpr, bodyExpr, out _, out _);

        public static Expression While(Expression conditionExpr, Expression bodyExpr, out LabelTarget breakTarget, out LabelTarget continueTarget)
        {
            breakTarget = Expression.Label("break");
            continueTarget = Expression.Label("continue");

            return Loop(
               IfThenElse(conditionExpr, bodyExpr, Break(breakTarget)),
               breakTarget,
               continueTarget
           );
        }

        public static Expression For(Expression conditionExpr, Expression operationExpr, Expression bodyExpr)
            => For(conditionExpr, operationExpr, bodyExpr, out _, out _);

        public static Expression For(Expression conditionExpr, Expression operationExpr, Expression bodyExpr, out LabelTarget breakTarget, out LabelTarget continueTarget)
        {
            breakTarget = Expression.Label("break");
            continueTarget = Expression.Label("continue");

            return Block(
                Loop(
                    IfThenElse(conditionExpr, Block(bodyExpr, operationExpr), Break(breakTarget)),
                    breakTarget,
                    continueTarget)
            );
        }

        public static Expression ForEach(Expression definitionExpr, Expression enumerableExpr, Expression bodyExpr)
            => ForEach(definitionExpr, enumerableExpr, bodyExpr, out _, out _);

        public static Expression ForEach(Expression definitionExpr, Expression enumerableExpr, Expression bodyExpr, out LabelTarget breakTarget, out LabelTarget continueTarget)
        {
            if (!enumerableExpr.Type.IsEnumerable(out Type itemType))
                throw new ArgumentException($"{enumerableExpr.Type} ({enumerableExpr}) is not enumerable.");

            Type enumeratorType = typeof(IEnumerator<>).MakeGenericType(itemType);
            Type enumerableType = typeof(IEnumerable<>).MakeGenericType(itemType);

            breakTarget = Label("break");
            continueTarget = Label("continue");

            return Block(
                Variable("enumerator", enumeratorType, out Expression enumerator),
                Assign(enumerator, Call("GetEnumerator", Convert(enumerableExpr, enumerableType), new[] { itemType })),
                Loop(
                    IfThenElse(Call("MoveNext", enumerator),
                        Block(
                            Assign(definitionExpr, Property(enumerator, "Current")),
                            bodyExpr
                        ),
                        Break(breakTarget)
                    ),
                    breakTarget,
                    continueTarget
                )
            );
        }

        public static Expression In(Expression expression)
        {
            if (!expression.Type.IsEnumerable(out _))
                throw new ArgumentException($"{expression} is not enumeable.");

            return expression;
        }

        public static Expression Result(Expression expression)
        {
            if (expression.Type == typeof(void))
                throw new ArgumentException($"{expression} is void.");

            return expression;
        }

        public static new MemberExpression Property(Expression instanceExpr, string propName)
        {
            PropertyInfo propertyInfo = instanceExpr.Type.ResolveProperty(propName);
            if (propertyInfo == null)
                throw new MemberAccessException($"Property {propName} doesn't exist in {instanceExpr.Type}");

            return Expression.Property(instanceExpr, propertyInfo);
        }

        public static IndexExpression Index(Expression instanceExpr, params Expression[] keyExprs)
        {
            PropertyInfo propertyInfo = instanceExpr.Type.ResolveIndexer(GetTypes(keyExprs));

            if (propertyInfo == null)
                throw new MemberAccessException($"Indexer [{string.Join(", ", keyExprs.Select(x => x.Type))}] doesn't exist in {instanceExpr.Type}");

            return Property(instanceExpr, propertyInfo, keyExprs);
        }

        public static MethodCallExpression Call(string methodName, Expression instanceExpr, params Expression[] paramsExpr)
        {
            MethodInfo methodInfo = instanceExpr.Type.ResolveMethod(methodName, null, GetTypes(paramsExpr));

            if (methodInfo == null)
                throw new MemberAccessException(methodName);

            return Call(instanceExpr, methodInfo, paramsExpr);
        }

        public static Expression Then(params Expression[] expressions) => Block(expressions);

        public static Expression Else(params Expression[] expressions) => Block(expressions);

        public static ConditionalExpression If(Expression testExpr, Expression thenExpr, Expression elseExpr = null)
        {
            if (elseExpr != null)
                return IfThenElse(testExpr, thenExpr, elseExpr);
            else
                return IfThen(testExpr, thenExpr);
        }

        public new static Expression Block(IEnumerable<Expression> expressions)
        {
            if (expressions == null)
            {
                throw new ArgumentNullException(nameof(expressions));
            }

            List<ParameterExpression> localExprs = new List<ParameterExpression>();
            List<Expression> bodyExprs = new List<Expression>();

            foreach (Expression expr in expressions)
            {
                AddExpression(localExprs, bodyExprs, expr);
            }

            if (bodyExprs.Count == 0)
            {
                bodyExprs.Add(Empty());
            }

            if (localExprs.Count == 0 && bodyExprs.Count == 1)
                return bodyExprs[0];
            else
                return Expression.Block(localExprs, bodyExprs);
        }

        static void AddExpression(List<ParameterExpression> localExprs, List<Expression> bodyExprs, Expression expr)
        {
            switch (expr)
            {
                case IncludeExpression includeExpr:
                    foreach (Expression subExpr in includeExpr.Expressions)
                    {
                        AddExpression(localExprs, bodyExprs, subExpr);
                    }
                    break;
                case VariableDefinitionExpression defExpr:
                    localExprs.Add(defExpr.ParameterExpression);
                    if (defExpr.InitializeExpression != null)
                    {
                        bodyExprs.Add(Assign(defExpr.ParameterExpression, defExpr.InitializeExpression));
                    }
                    break;
                default:
                    bodyExprs.Add(expr);
                    break;
            }
        }

        public new static Expression Block(params Expression[] expressions) => Block((IEnumerable<Expression>)expressions);

        public static MethodCallExpression Call(string methodName, Expression instanceExpr, Type[] genericArgTypes, params Expression[] paramsExpr)
        {
            MethodInfo methodInfo = instanceExpr.Type.ResolveMethod(methodName, genericArgTypes, GetTypes(paramsExpr));

            if (methodInfo == null)
                throw new MemberAccessException(methodName);

            return Call(instanceExpr, methodInfo, paramsExpr);
        }

        public static MethodCallExpression Call(string methodName, Type staticMethodType, params Expression[] paramsExpr)
        {
            MethodInfo methodInfo = staticMethodType.ResolveMethod(methodName, null, GetTypes(paramsExpr));

            if (methodInfo == null)
                throw new MemberAccessException($"Method {methodName} not found.");

            return Call(null, methodInfo, paramsExpr);
        }

        public static MethodCallExpression Call(string methodName, Type staticMethodType, Type[] genericArgTypes, params Expression[] paramsExpr)
        {
            MethodInfo methodInfo = staticMethodType.ResolveMethod(methodName, genericArgTypes, GetTypes(paramsExpr));

            if (methodInfo == null)
                throw new MemberAccessException($"Method {methodName} not found.");

            return Call(null, methodInfo, paramsExpr);
        }

        public static Expression SetToDefault(Expression expression)
        {
            return Assign(expression, Default(expression.Type));
        }

        public static BinaryExpression IsNull(Expression expression)
        {
            return Equal(expression, Default(expression.Type));
        }

        public static BinaryExpression IsNotNull(Expression expression)
        {
            return NotEqual(expression, Default(expression.Type));
        }

        public static Expression Import(LambdaExpression expression, params Expression[] parameters)
        {
            Dictionary<Expression, Expression> replacements = new Dictionary<Expression, Expression>();

            for (int i = 0; i < expression.Parameters.Count; i++)
            {
                replacements.Add(expression.Parameters[i], parameters[i]);
            }

            return new ReplaceExpressionVisitor(replacements).Visit(expression.Body);
        }

        public static NewExpression New(Type type, IReadOnlyList<Expression> arguments = null)
        {
            NewExpression newExpr = null;

            if (arguments != null && arguments.Count > 0)
            {
                var constructor = type.ResolveConstructor(GetTypes(arguments));

                newExpr = Expression.New(constructor, arguments);
            }
            else
            {
                newExpr = Expression.New(type);
            }

            NewExpression initExpr = newExpr;

            //if (ObjectInitializers != null)
            //{
            //    initExpr = Expression.MemberInit(
            //        newExpr,
            //        ObjectInitializers.Select(i =>
            //        {
            //            MemberInfo propInfo = Type.GetProperty(i.Key);
            //            return Expression.Bind(propInfo, i.Value);
            //        })
            //   );
            //}

            //if (ListInitializers != null)
            //{
            //    if (Type.IsArray)
            //    {
            //        Type elementType = Type.GetElementType();
            //        initExpr = Expression.NewArrayInit(
            //            elementType,
            //            ListInitializers.Select(a => Convert(elementType, a).ToExpression(scope)));
            //    }
            //    else if (Type.IsList(out Type itemType))
            //    {
            //        var addMethod = scope.Resolver.ResolveMethod(Type, "Add", new[] { itemType }, null);
            //        initExpr = Expression.ListInit(newExpr,
            //            ListInitializers.Select(i => Expression.ElementInit(addMethod, i.ToExpression(scope))));
            //    }
            //}

            //if (DictionaryInitializers != null)
            //{
            //    if (Type.IsDictionary(out Type keyType, out Type valueType))
            //    {
            //        var addMethod = scope.Resolver.ResolveMethod(Type, "Add", new[] { keyType, valueType }, null);
            //        initExpr = Expression.ListInit(newExpr,
            //            DictionaryInitializers.Select(i =>
            //                Expression.ElementInit(addMethod,
            //                    i.Key.ToExpression(scope),
            //                    i.Value.ToExpression(scope)
            //                )
            //           ));
            //    }
            //}

            return initExpr;
        }

        public static Expression DebugWrite(Expression expr)
        {
            return Call("WriteLine", typeof(System.Diagnostics.Debug), Convert(expr, typeof(object)));
        }

        public static Expression DebugWrite(object value)
        {
            return Call("WriteLine", typeof(System.Diagnostics.Debug), Constant(value));
        }

        public static Type DictionaryOf(Type keyType, Type valueType)
        {
            return typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
        }

        public static Type ListOf(Type itemType)
        {
            return typeof(List<>).MakeGenericType(itemType);
        }

        public static Type KeyValueOf(Type keyType, Type valueType)
        {
            return typeof(KeyValuePair<,>).MakeGenericType(keyType, valueType);
        }

        public static Type[] GetTypes(IReadOnlyList<Expression> expressions)
        {
            Type[] types = new Type[expressions.Count];

            for (int i = 0; i < types.Length; i++)
            {
                types[i] = expressions[i].Type;
            }

            return types;
        }
    }
}