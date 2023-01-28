using Detached.Annotations;
using Detached.Mappers.TypePairs.Builder;
using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers.Annotations
{
    public class CompositionAnnotationHandler : AnnotationHandler<CompositionAttribute>
    {
        public override void Apply(CompositionAttribute annotation, MapperOptions modelOptions, ClassType typeOptions, ClassTypeMember memberOptions)
        {
            memberOptions.Composition(true);
        }
    }
}

namespace Detached.Mappers
{
    public static class CompositionAnnotationHandlerExtensions
    {
        const string KEY = "DETACHED_COMPOSITION";

        public static bool IsComposition(this ITypeMember member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static void Composition(this ITypeMember member, bool value)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static ClassTypeMemberBuilder<TType, TMember> Composition<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.MemberOptions.Composition(value);
            return member;
        }

        public static bool IsComposition(this TypePairMember member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static void Composition(this TypePairMember member, bool value = true)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static TypePairMemberBuilder<TType, TMember> Composition<TType, TMember>(this TypePairMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.TypePairMember.Exclude();
            return member;
        }
    }
}