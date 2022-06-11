using Detached.Mappers.TypeOptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Json.Nodes;

namespace Detached.Mappers.Json.TypeOptions
{
    public class JsonNodeTypeOptions : ITypeOptions
    {
        public Type ClrType => typeof(JsonNode);

        public Type ItemClrType => default;

        public Dictionary<string, object> Annotations { get; } = new Dictionary<string, object>();

        public TypeKind Kind => TypeKind.None;

        public IEnumerable<string> MemberNames => null;

        public bool IsAbstract => true;

        public Expression BuildNewExpression(Expression context, Expression discriminator)
        {
            throw new NotImplementedException();
        }

        public IMemberOptions GetMember(string memberName)
        {
            return new JsonObjectMemberOptions(memberName);
        }
    }
}
