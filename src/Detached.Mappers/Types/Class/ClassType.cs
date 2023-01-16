using Detached.Mappers.Extensions;
using Detached.PatchTypes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Types.Class
{
    public class ClassType : IType
    {
        public virtual Type ClrType { get; set; }

        public virtual Type ItemClrType { get; set; }

        public virtual MappingSchema MappingSchema { get; set; }

        public virtual bool IsAbstract { get; set; }

        public virtual ClassTypeMemberCollection Members { get; set; } = new ClassTypeMemberCollection();

        public virtual IEnumerable<string> MemberNames => Members.Keys;

        public virtual Dictionary<string, object> Annotations { get; } = new Dictionary<string, object>();

        public virtual LambdaExpression Constructor { get; set; }

        public virtual ITypeMember GetMember(string memberName)
        {
            Members.TryGetValue(memberName, out ClassTypeMember memberOptions);
            return memberOptions;
        }

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

        public override string ToString() => $"{ClrType.GetFriendlyName()} (ClassType)";
    }
}