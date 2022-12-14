using Detached.Mappers.Types;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Json.Nodes;

namespace Detached.Mappers.Json.TypeOptions
{
    public class JsonNodeTypeOptions : IType
    {
        public Type ClrType => typeof(JsonNode);

        public Type ItemClrType => default;

        public Dictionary<string, object> Annotations { get; } = new Dictionary<string, object>();

        public MappingStrategy MappingStrategy => MappingStrategy.None;

        public IEnumerable<string> MemberNames => null;

        public bool IsAbstract => true;

        public Expression BuildNewExpression(Expression context, Expression discriminator)
        {
            throw new NotImplementedException();
        }

        public ITypeMember GetMember(string memberName)
        {
            return new JsonObjectMemberOptions(memberName);
        }
    }
}
