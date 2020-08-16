using Detached.Mapping.Context;
using System;
using System.Linq.Expressions;

namespace Detached.Model.Builder
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
            string memberName = ((MemberExpression)selector.Body).Member.Name;
            if (!TypeOptions.Members.TryGetValue(memberName, out ClassMemberOptions memberOptions))
                throw new ArgumentException($"Member {memberName} does not exist.");

            return new ClassMemberOptionsBuilder<TType, TMember>(TypeOptions, memberOptions);
        }

        public ClassTypeOptionsBuilder<TType> IsEntity(bool entity = true)
        {
            TypeOptions.IsFragment = !entity;
            TypeOptions.IsEntity = entity;
            return this;
        }

        public ClassTypeOptionsBuilder<TType> IsFragment(bool entity = true)
        {
            TypeOptions.IsEntity = !entity; 
            TypeOptions.IsFragment = entity;
            return this;
        }

        public ClassTypeOptionsBuilder<TType> Constructor(Expression<Func<IMapperContext, TType>> constructor)
        {
            TypeOptions.Constructor = constructor;
            return this;
        }
    }
}