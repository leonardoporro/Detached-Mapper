using Detached.Mappers.Annotations;
using Detached.Mappers.Extensions;
using System;
using System.Linq.Expressions;
using System.Reflection;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;

namespace Detached.Mappers.Types.Class
{
    public class ClassTypeMember : ITypeMember
    {
        public virtual PropertyInfo PropertyInfo { get; set; }

        public virtual string Name { get; set; }

        public virtual Type ClrType { get; set; }

        public virtual AnnotationCollection Annotations { get; } = new();

        public virtual LambdaExpression Getter { get; set; }

        public virtual LambdaExpression Setter { get; set; }

        public virtual LambdaExpression TryGetter { get; set; }

        public bool CanRead => Getter != null;

        public bool CanWrite => Setter != null;

        public bool CanTryGet => TryGetter != null;


        public virtual Expression BuildGetExpression(Expression instance, Expression context)
        {
            return Import(Getter, instance, context);
        }

        public virtual Expression BuildSetExpression(Expression instance, Expression value, Expression context)
        {
            if (Setter != null)
                return Import(Setter, instance, value, context);
            else
                return value;
        }

        public virtual Expression BuildTryGetExpression(Expression instance, Expression context, Expression outVar)
        {
            return Import(TryGetter, instance, outVar, context);
        }

        public override string ToString() => $"{Name} [{ClrType.GetFriendlyName()}] (MemberOptions)";
    }
}