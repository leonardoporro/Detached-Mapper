using Detached.Mappers.TypeOptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Detached.Mappers.Json.TypeOptions.Json
{
    public class JsonObjectTypeOptions : ITypeOptions
    {
        public Type ClrType => throw new NotImplementedException();

        public Type ItemClrType => throw new NotImplementedException();

        public Dictionary<string, object> Annotations => throw new NotImplementedException();

        public TypeKind Kind => throw new NotImplementedException();

        public IEnumerable<string> MemberNames => throw new NotImplementedException();

        public string DiscriminatorName => throw new NotImplementedException();

        public Dictionary<object, Type> DiscriminatorValues => throw new NotImplementedException();

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
