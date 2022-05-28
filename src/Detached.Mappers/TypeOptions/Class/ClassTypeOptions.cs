using AgileObjects.ReadableExpressions.Extensions;
using Detached.PatchTypes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.TypeOptions.Class
{
    public class ClassTypeOptions : ITypeOptions
    {
        public virtual Type ClrType { get; set; }

        public virtual Type ItemClrType { get; set; }

        public virtual ClassMemberOptionsCollection Members { get; set; } = new ClassMemberOptionsCollection();

        public virtual IEnumerable<string> MemberNames => Members.Keys;

        public virtual string DiscriminatorName { get; set; }

        public virtual Dictionary<object, Type> DiscriminatorValues { get; } = new Dictionary<object, Type>();

        public virtual IMemberOptions GetMember(string memberName)
        {
            Members.TryGetValue(memberName, out ClassMemberOptions memberOptions);
            return memberOptions;
        }

        public virtual Dictionary<string, object> Annotations { get; } = new Dictionary<string, object>();

        public virtual bool IsPrimitive { get; set; }

        public virtual bool IsCollection { get; set; }

        public virtual bool IsComplex { get; set; }

        public virtual bool IsEntity { get; set; } 

        public virtual bool IsAbstract { get; set; }

        public virtual bool IsInherited => DiscriminatorName != null;

        public virtual LambdaExpression Constructor { get; set; }

        public bool IsNullable { get; set; }

        public virtual Expression BuildNewExpression(Expression context, Expression discriminator)
        {
            if (Constructor == null)
            {
                throw new InvalidOperationException($"Can't construct {ClrType.GetFriendlyName()}. It does not have a parameterless constructor or a concrete type specified.");
            }

            return Import(Constructor, context);
        }

        public Expression BuildIsSetExpression(Expression instance, Expression context, string memberName)
        {
            Expression result = null;

            if (typeof(IPatch).IsAssignableFrom(ClrType))
            {
                result = Call("IsSet", Convert(instance, typeof(IPatch)), Constant(memberName));
            }

            return result;
        }

        public override string ToString() => $"{ClrType.GetFriendlyName()} (TypeOptions)";
    }
}