using Detached.Mappers.Annotations;
using System.Linq.Expressions;

namespace Detached.Mappers.TypeOptions.Class.Builder
{
    public class ClassMemberOptionsBuilder<TType, TMember> : ClassTypeOptionsBuilder<TType>
    {
        public ClassMemberOptionsBuilder(ClassTypeOptions typeOptions, ClassMemberOptions memberOptions, MapperOptions mapperOptions)
            : base(typeOptions, mapperOptions)
        {
            MemberOptions = memberOptions;
        }

        public ClassMemberOptions MemberOptions { get; }
 
        public ClassMemberOptionsBuilder<TType, TMember> Getter(LambdaExpression lambda)
        {
            MemberOptions.Getter = lambda;
            return this;
        }

        public ClassMemberOptionsBuilder<TType, TMember> Exclude()
        {
            MemberOptions.IsNotMapped(true);
            return this;
        }

        public ClassMemberOptionsBuilder<TType, TMember> Include()
        {
            MemberOptions.IsNotMapped(false);
            return this;
        }

        public ClassMemberOptionsBuilder<TType, TMember> Setter(LambdaExpression lambda)
        {
            MemberOptions.Setter = lambda;
            return this;
        }
    }
}