using Detached.Mappers.Annotations;
using Detached.Mappers.TypePairs;
using Detached.Mappers.TypePairs.Builder;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers
{
    public static class AssociationAnnotationExtensions
    {
        public static Annotation<bool> Composition(this AnnotationCollection annotations)
        {
            return annotations.Annotation<bool>("DETACHED_COMPOSITION");
        }

        public static ITypeMember Composition(this ITypeMember member)
        {
            member.Annotations.Composition().Set(true);

            return member;
        }

        public static ClassTypeMemberBuilder<TType, TMember> Composition<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> memberBuilder)
        {
            memberBuilder.Member.Composition();

            return memberBuilder;
        }

        public static TypePairMember Composition(this TypePairMember member)
        {
            member.Annotations.Composition().Set(true);

            return member;
        }

        public static TypePairMemberBuilder<TType, TMember> Composition<TType, TMember>(this TypePairMemberBuilder<TType, TMember> memberBuilder)
        {
            memberBuilder.Member.Composition();

            return memberBuilder;
        }        
        
        public static bool IsComposition(this ITypeMember member)
        {
            return member.Annotations.Composition().Value(); 
        }

        public static bool IsComposition(this TypePairMember member)
        {
            var annotation = member.Annotations.Composition();

            if (annotation.IsDefined())
                return annotation.Value();
            else
                return member.TargetMember.IsComposition();
        }

        public static ITypeMember Aggregation(this ITypeMember member)
        {
            member.Annotations.Composition().Set(false);

            return member;
        }

        public static TypePairMember Aggregation(this TypePairMember memberPair)
        {
            memberPair.Annotations.Composition().Set(false);

            return memberPair;
        }

        public static ClassTypeMemberBuilder<TType, TMember> Aggregation<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> memberBuilder)
        {
            memberBuilder.Member.Aggregation();

            return memberBuilder;
        }


        public static TypePairMemberBuilder<TType, TMember> Aggregation<TType, TMember>(this TypePairMemberBuilder<TType, TMember> memberBuilder)
        {
            memberBuilder.Member.Aggregation();

            return memberBuilder;
        }
    }
}