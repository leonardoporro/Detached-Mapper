
using AgileObjects.ReadableExpressions.Extensions;
using Detached.PatchTypes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.TypeOptions.Class
{
    public class ClassMemberOptions : IMemberOptions
    {
        public virtual PropertyInfo PropertyInfo { get; set; }

        public virtual string Name { get; set; }

        public virtual Type ClrType { get; set; }

        public virtual Dictionary<string, object> Annotations { get; } = new Dictionary<string, object>();

        public virtual LambdaExpression Getter { get; set; }

        public virtual LambdaExpression Setter { get; set; }

        public bool CanRead => Getter != null;

        public bool CanWrite => Setter != null;

        public bool CanTryGet { get; set; }

        public virtual Expression BuildGetExpression(Expression instance, Expression context)
        {
            return Import(Getter, instance, context);
        }

        public virtual Expression BuildSetExpression(Expression instance, Expression value, Expression context)
        {
            return Import(Setter, instance, value, context);
        }

        public virtual Expression BuildTryGetExpression(Expression instance, Expression context, Expression outVar)
        {
            return Block(
                Variable(typeof(bool), out Expression resultExpr),
                If(Call("IsSet", Convert(instance, typeof(IPatch)), Constant(Name)),
                    Block(
                        Assign(outVar, BuildGetExpression(instance, context)),
                        Assign(resultExpr, Constant(true))
                    ),
                    Block(
                        Assign(outVar, Default(outVar.Type)),
                        Assign(resultExpr, Constant(false))
                    )
                ),
                resultExpr
            );
        }

        public override string ToString() => $"{Name} [{ClrType.GetFriendlyName()}] (MemberOptions)";
    }
}