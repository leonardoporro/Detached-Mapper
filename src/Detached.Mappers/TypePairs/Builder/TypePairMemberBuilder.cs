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
            TypePairMember = typePairMemberOptions;
        }

        public TypePairMember TypePairMember { get; }

        public TypePairMemberBuilder<TSource, TTarget> FromMember(string memberName)
        {
            ITypeMember typeMember = TypePairMember.SourceType.GetMember(memberName);
            if (typeMember == null)
                throw new ArgumentException($"Member {memberName} doesn't exist on type {TypePairMember.SourceType}");

            TypePairMember.SourceMember = typeMember;
            TypePairMember.Include();
            return this;
        }

        public TypePairMemberBuilder<TSource, TTarget> FromMember<TMember>(Expression<Func<TSource, TMember>> selector)
        {
            string memberName = ((MemberExpression)selector.Body).Member.Name;
            return FromMember(memberName);
        }

        public TypePairMemberBuilder<TSource, TTarget> FromValue<TMember>(Expression<Func<TSource, IMapContext, TMember>> expression)
        {
            TypePairMember.SourceMember = new ClassTypeMember
            {
                Getter = expression,
                Setter = null,
                CanTryGet = false,
                ClrType = typeof(TMember),
                Name = TypePairMember.TargetMember?.Name
            };

            TypePairMember.Include();
            return this;
        }
    }
}