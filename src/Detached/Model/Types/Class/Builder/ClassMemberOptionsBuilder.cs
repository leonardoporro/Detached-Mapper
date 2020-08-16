using Detached.Mapping.Context;
using System;
using System.Linq.Expressions;

namespace Detached.Model.Builder
{
    public class ClassMemberOptionsBuilder<TType, TMember> : ClassTypeOptionsBuilder<TType>
    {
        public ClassMemberOptionsBuilder(ClassTypeOptions typeOptions, ClassMemberOptions memberOptions) 
            : base(typeOptions)
        {
            MemberOptions = memberOptions;
        }

        public ClassMemberOptions MemberOptions { get; }

        public ClassMemberOptionsBuilder<TType, TMember> Composition()
        {
            MemberOptions.Owned = true;
            return this;
        }

        public ClassMemberOptionsBuilder<TType, TMember> Aggregation()
        {
            MemberOptions.Owned = false;
            return this;
        }


        public ClassMemberOptionsBuilder<TType, TMember> IsKey(bool value = true)
        {
            MemberOptions.IsKey = value;
            return this;
        }

        public ClassMemberOptionsBuilder<TType, TMember> Getter(LambdaExpression lambda)
        {
            MemberOptions.Getter = lambda;
            return this;
        }

        public ClassMemberOptionsBuilder<TType, TMember> Setter(LambdaExpression lambda)
        {
            MemberOptions.Setter = lambda;
            return this;
        }
    }
}