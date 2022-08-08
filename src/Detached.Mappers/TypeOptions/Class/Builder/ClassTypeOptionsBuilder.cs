using System;
using System.Linq.Expressions;

namespace Detached.Mappers.TypeOptions.Class.Builder
{
    public class ClassTypeOptionsBuilder<TType>
    {
        public ClassTypeOptionsBuilder(ClassTypeOptions typeOptions)
        {
            TypeOptions = typeOptions;
        }

        public ClassTypeOptions TypeOptions { get; }

        public ClassMemberOptionsBuilder<TType, TMember> Member<TMember>(Expression<Func<TType, TMember>> selector)
        {
            ClassMemberOptions memberOptions = GetMember(selector);

            return new ClassMemberOptionsBuilder<TType, TMember>(TypeOptions, memberOptions);
        }
 
        public ClassTypeOptionsBuilder<TType> Constructor(Expression<Func<IMapContext, TType>> constructor)
        {
            TypeOptions.Constructor = constructor;
            return this;
        }

        public ClassDiscriminatorBuilder<TType, TMember> Discriminator<TMember>(Expression<Func<TType, TMember>> selector)
        {
            ClassMemberOptions memberOptions = GetMember(selector);
            TypeOptions.SetDiscriminatorName(memberOptions.Name);

            return new ClassDiscriminatorBuilder<TType, TMember>(TypeOptions);
        }

        ClassMemberOptions GetMember<TMember>(Expression<Func<TType, TMember>> selector)
        {
            string memberName = ((MemberExpression)selector.Body).Member.Name;
            if (!TypeOptions.Members.TryGetValue(memberName, out ClassMemberOptions memberOptions))
                throw new ArgumentException($"Member {memberName} does not exist.");

            return memberOptions;
        }
    }
}