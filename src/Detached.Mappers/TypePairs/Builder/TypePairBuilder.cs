using System;
using System.Linq.Expressions;

namespace Detached.Mappers.TypePairs.Builder
{
    public class TypePairBuilder<TSource, TTarget>
    {
        public TypePairBuilder(MapperOptions mapperOptions, TypePair typePairOptions)
        {
            MapperOptions = mapperOptions;
            TypePairOptions = typePairOptions;
        }

        public MapperOptions MapperOptions { get; }

        public TypePair TypePairOptions { get; }

        public TypePairMemberBuilder<TSource, TTarget> Member<TMember>(Expression<Func<TTarget, TMember>> selector)
        {
            string memberName = ((MemberExpression)selector.Body).Member.Name;

            if (!TypePairOptions.Members.TryGetValue(memberName, out TypePairMember memberOptions))
            {
                throw new ArgumentException($"Member {memberName} does not exist.");
            }

            return new TypePairMemberBuilder<TSource, TTarget>(MapperOptions, TypePairOptions, memberOptions);
        }
    }
}