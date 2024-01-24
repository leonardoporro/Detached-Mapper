using Detached.Annotations;
using Detached.Mappers.TypePairs;
using Detached.Mappers.TypePairs.Builder;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers.Annotations
{
    public class CompositionAnnotationHandler : AnnotationHandler<CompositionAttribute>
    {
        public override void Apply(CompositionAttribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member)
        {
            member.Composition(true);
        }
    }
}

namespace Detached.Mappers
{
    public static class CompositionAnnotationHandlerExtensions
    {
        public const string VALUE_KEY = "DETACHED_COMPOSITION";

        public static bool IsComposition(this ITypeMember member)
        {
            return member.Annotations.TryGetValue(VALUE_KEY, out var value) && Equals(value, true);
        }

        public static bool IsComposition(this TypePairMember member)
        {
            return member.Annotations.TryGetValue(VALUE_KEY, out var value) && Equals(value, true);
        }

        public static void Composition(this ITypeMember member, bool value)
        {
            member.Annotations[VALUE_KEY] = value;
        }

        public static ClassTypeMemberBuilder<TType, TMember> Composition<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.MemberOptions.Composition(value);
            return member;
        }

        public static void Composition(this TypePairMember member, bool value = true)
        {
            member.Annotations[VALUE_KEY] = value;
        }

        public static TypePairMemberBuilder<TType, TMember> Composition<TType, TMember>(this TypePairMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.TypePairMember.Composition(value);
            return member;
        }
    }
}