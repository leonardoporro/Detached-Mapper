using Detached.Mappers.TypeOptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Json.Nodes;
using static Detached.RuntimeTypes.Expressions.ExtendedExpression;
using static System.Linq.Expressions.Expression;

namespace Detached.Mappers.Json.TypeOptions
{
    public class JsonArrayTypeOptions : ITypeOptions
    {
        public Type ClrType => typeof(JsonArray);

        public Type ItemClrType => typeof(JsonNode);

        public Dictionary<string, object> Annotations { get; } = new Dictionary<string, object>();

        public MappingStrategy MappingStrategy => MappingStrategy.Collection;

        public IEnumerable<string> MemberNames => null;

        public bool IsAbstract => false;

        public Expression BuildNewExpression(Expression context, Expression discriminator)
        {
            return New(ClrType);
        }

        public IMemberOptions GetMember(string memberName)
        {
            throw new NotImplementedException();
        }
    }
}
