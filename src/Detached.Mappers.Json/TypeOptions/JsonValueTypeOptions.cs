using Detached.Mappers.Annotations;
using Detached.Mappers.Types;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Json.Nodes;

namespace Detached.Mappers.Json.TypeOptions
{
    public class JsonValueTypeOptions : IType
    {
        public Type ClrType => typeof(JsonValue);

        public Type ItemClrType => throw new NotImplementedException();

        public AnnotationCollection Annotations { get; } = new();

        public MappingSchema MappingSchema => MappingSchema.Primitive;

        public IEnumerable<string> MemberNames => null;

        public Expression BuildNewExpression(Expression context, Expression discriminator)
        {
            throw new NotImplementedException();
        }

        public ITypeMember GetMember(string memberName)
        {
            throw new NotImplementedException();
        }
    }
}
