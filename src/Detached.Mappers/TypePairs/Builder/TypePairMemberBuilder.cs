using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Context;
using Detached.Mappers.Options;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using System;
using System.Linq.Expressions;

namespace Detached.Mappers.TypePairs.Builder
{
    public class TypePairMemberBuilder<TSource, TTarget> : TypePairBuilder<TSource, TTarget>
    {
        public TypePairMemberBuilder(MapperOptions mapperOptions, TypePair typePairOptions, TypePairMember typePairMemberOptions)
            : base(mapperOptions, typePairOptions)
        {
            Member = typePairMemberOptions;
        }

        public TypePairMember Member { get; }

        public TypePairMemberBuilder<TSource, TTarget> FromMember(string memberName)
        {
            ITypeMember typeMember = Member.SourceType.GetMember(memberName);
            if (typeMember == null)
                throw new ArgumentException($"Member {memberName} doesn't exist on type {Member.SourceType}");

            Member.SourceMember = typeMember;
            Member.Include();
            return this;
        }

        public TypePairMemberBuilder<TSource, TTarget> FromMember<TMember>(Expression<Func<TSource, TMember>> selector)
        {
            string memberName = ((MemberExpression)selector.Body).Member.Name;
            return FromMember(memberName);
        }

        public TypePairMemberBuilder<TSource, TTarget> FromValue<TMember>(Expression<Func<TSource, IMapContext, TMember>> expression)
        {
            Member.SourceMember = new ClassTypeMember
            {
                Getter = expression,
                Setter = null,
                TryGetter = null,
                ClrType = typeof(TMember),
                Name = Member.TargetMember?.Name
            };

            Member.Include();
            return this;
        }
    }
}