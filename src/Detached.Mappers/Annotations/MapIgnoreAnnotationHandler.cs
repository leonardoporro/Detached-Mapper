using Detached.Annotations;
using Detached.Mappers.TypePairs;
using Detached.Mappers.TypePairs.Builder;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers.Annotations
{
    public class MapIgnoreAnnotationHandler : AnnotationHandler<MapIgnoreAttribute>
    {
        public override void Apply(MapIgnoreAttribute annotation, MapperOptions mapperOptions, ClassType typeOptions, ClassTypeMember memberOptions)
        {
            memberOptions.Ignore(true);
        }
    }
}

namespace Detached.Mappers
{
    public static class MapIgnoreAnnotationHandlerExtensions
    {
        public const string VALUE_KEY = "DETACHED_MAP_IGNORE";

        public static bool IsIgnored(this ITypeMember member)
        {
            return member.Annotations.TryGetValue(VALUE_KEY, out var value) && Equals(value, true);
        }

        public static bool IsIgnored(this TypePairMember member)
        {
            return member.Annotations.TryGetValue(VALUE_KEY, out var value) && Equals(value, true);
        }

        public static void Ignore(this ITypeMember member, bool value = true)
        {
            member.Annotations[VALUE_KEY] = value;
        }

        public static ClassTypeMemberBuilder<TType, TMember> Include<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.MemberOptions.Ignore(false);
            return member;
        }

        public static void Include(this TypePairMember member)
        {
            member.Annotations[VALUE_KEY] = false;
        }

        public static TypePairMemberBuilder<TType, TMember> Include<TType, TMember>(this TypePairMemberBuilder<TType, TMember> member)
        {
            member.TypePairMember.Include();
            return member;
        }

        public static ClassTypeMemberBuilder<TType, TMember> Exclude<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member)
        {
            member.MemberOptions.Ignore(true);
            return member;
        }

        public static void Exclude(this TypePairMember member)
        {
            member.Annotations[VALUE_KEY] = true;
        }

        public static TypePairMemberBuilder<TType, TMember> Exclude<TType, TMember>(this TypePairMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.TypePairMember.Exclude();
            return member;
        }

        public static TypePairMemberBuilder<TType, TMember> Exclude<TType, TMember>(this TypePairMemberBuilder<TType, TMember> member)
        {
            member.TypePairMember.Exclude();
            return member;
        }

        public static bool IsMapped(this TypePairMember member)
        {
            return !member.IsIgnored();
        }

        public static bool IsIgnoredConfigured(this ITypeMember member)
        {
            return member.Annotations.ContainsKey(VALUE_KEY);
        }
    }
}