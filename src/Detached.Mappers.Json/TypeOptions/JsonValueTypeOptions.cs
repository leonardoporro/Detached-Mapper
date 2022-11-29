using Detached.Mappers.TypeOptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Json.Nodes;

namespace Detached.Mappers.Json.TypeOptions
{
    public class JsonValueTypeOptions : ITypeOptions
    {
        public Type ClrType => typeof(JsonValue);

        public Type ItemClrType => throw new NotImplementedException();

        public Dictionary<string, object> Annotations { get; } = new Dictionary<string, object>();

        public MappingStrategy MappingStrategy => MappingStrategy.Primitive;

        public IEnumerable<string> MemberNames => null;

        public bool IsAbstract => false;

        public Expression BuildNewExpression(Expression context, Expression discriminator)
        {
            throw new NotImplementedException();
        }

        public IMemberOptions GetMember(string memberName)
        {
            throw new NotImplementedException();
        }
    }
}
