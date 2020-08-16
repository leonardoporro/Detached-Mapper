using AgileObjects.ReadableExpressions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static Detached.Expressions.ExtendedExpression;

namespace Detached.Model
{
    public class ClassTypeOptions : ITypeOptions
    {
        public virtual Type Type { get; set; }

        public virtual Type ItemType { get; set; }

        public virtual ClassMemberOptionsCollection Members { get; set; } = new ClassMemberOptionsCollection();

        public virtual IEnumerable<string> MemberNames => Members.Keys;

        public virtual IMemberOptions GetMember(string memberName)
        {
            Members.TryGetValue(memberName, out ClassMemberOptions memberOptions);
            return memberOptions;
        }

        public virtual Dictionary<string, object> Annotations { get; } = new Dictionary<string, object>();

        public virtual bool IsValue { get; set; }

        public virtual bool IsCollection { get; set; }

        public virtual bool IsComplexType { get; set; }

        public virtual bool IsEntity { get; set; }

        public virtual bool IsFragment { get; set; }

        public virtual bool UsePatchProxy { get; internal set; }

        public virtual LambdaExpression Constructor { get; set; }

        public virtual Expression Construct(Expression context)
        {
            if (Constructor == null)
                throw new InvalidOperationException($"Can't construct {Type.GetFriendlyName()}. It does not have a parameterless constructor or a concrete type specified.");

            return Import(Constructor, context);
        }

        public override string ToString() => $"{Type.GetFriendlyName()} (EntityOptions)";
    }
}