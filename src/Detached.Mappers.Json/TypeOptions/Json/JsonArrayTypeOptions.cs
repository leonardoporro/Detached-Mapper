using Detached.Mappers.TypeOptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Json.Nodes;

namespace Detached.Mappers.Json.TypeOptions.Json
{
    public class JsonArrayTypeOptions : ITypeOptions
    {
        public Type ClrType => typeof(JsonArray);

        public Type ItemClrType => typeof(JsonNode);

        public Dictionary<string, object> Annotations { get; } = new Dictionary<string, object>();

        public TypeKind Kind => TypeKind.Collection;

        public IEnumerable<string> MemberNames => new string[0];

        public string DiscriminatorName => null;

        public Dictionary<object, Type> DiscriminatorValues => null;

        public Expression BuildIsSetExpression(Expression instance, Expression context, string memberName)
        {
            throw new NotImplementedException();
        }

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
