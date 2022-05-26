
using AgileObjects.ReadableExpressions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;

namespace Detached.Mappers.TypeOptions.Class
{
    public class ClassMemberOptions : IMemberOptions
    {
        public virtual string Name { get; set; }

        public virtual Type ClrType { get; set; }

        public virtual bool IsComposition { get; set; }

        public virtual bool Ignored { get; set; }

        public virtual bool IsKey { get; set; }

        public virtual Dictionary<string, object> Annotations { get; } = new Dictionary<string, object>();

        public virtual LambdaExpression Getter { get; set; }

        public virtual LambdaExpression Setter { get; set; }

        public bool CanRead => Getter != null;

        public bool CanWrite => Setter != null;

        public virtual Expression BuildGetterExpression(Expression instance, Expression context)
        {
            return Import(Getter, instance, context);
        }

        public virtual Expression BuildSetterExpression(Expression instance, Expression value, Expression context)
        {
            return Import(Setter, instance, value, context);
        }

        public override string ToString() => $"{Name} [{ClrType.GetFriendlyName()}] (MemberOptions)";
    }
}