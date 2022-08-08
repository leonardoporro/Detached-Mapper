using Detached.Mappers.TypeOptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Json.Nodes;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Json.TypeOptions
{
    public class JsonObjectTypeOptions : ITypeOptions
    {
        public Type ClrType => typeof(JsonObject);

        public Type ItemClrType => null;

        public Dictionary<string, object> Annotations { get; } = new Dictionary<string, object>();

        public TypeKind Kind => TypeKind.Complex;

        public IEnumerable<string> MemberNames => new string[0];

        public bool IsAbstract => false;

        public Expression BuildNewExpression(Expression context, Expression discriminator)
        {
            return New(ClrType);
        }

        public IMemberOptions GetMember(string memberName)
        {
            return new JsonObjectMemberOptions(memberName);
        }
    }
}
