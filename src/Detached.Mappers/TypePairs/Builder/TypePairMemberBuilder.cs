using Detached.Mappers.Annotations;
using Detached.Mappers.Types;
using System;
using System.Linq.Expressions;

namespace Detached.Mappers.TypePairs.Builder
{
    public class TypePairMemberBuilder<TSource, TTarget> : TypePairBuilder<TSource, TTarget>
    {
        public TypePairMemberBuilder(MapperOptions mapperOptions, TypePair typePairOptions, TypePairMember typePairMemberOptions)
            : base(mapperOptions, typePairOptions)
        {
            TypePairMember = typePairMemberOptions;
        }

        public TypePairMember TypePairMember { get; }

        public TypePairMemberBuilder<TSource, TTarget> FromMember(string memberName)
        {
            ITypeMember typeMember = TypePairMember.SourceType.GetMember(memberName);
            if (typeMember == null)
                throw new ArgumentException($"Member {memberName} doesn't exist on type {TypePairMember.SourceType}");

            TypePairMember.SourceMember = typeMember;
            TypePairMember.NotMapped(false);
            return this;
        }

        public TypePairMemberBuilder<TSource, TTarget> FromMember<TMember>(Expression<Func<TSource, TMember>> selector)
        {
            string memberName = ((MemberExpression)selector.Body).Member.Name;
            return FromMember(memberName);
        }
    }
}