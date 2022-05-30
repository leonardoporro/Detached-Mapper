using System.Linq.Expressions;

namespace Detached.Mappers.TypeOptions.Class.Builder
{
    public class ClassMemberOptionsBuilder<TType, TMember> : ClassTypeOptionsBuilder<TType>
    {
        public ClassMemberOptionsBuilder(ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
            : base(typeOptions)
        {
            MemberOptions = memberOptions;
        }

        public ClassMemberOptions MemberOptions { get; }
 
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