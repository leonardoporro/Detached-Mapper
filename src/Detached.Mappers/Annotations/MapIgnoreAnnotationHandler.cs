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
        public const string KEY = "DETACHED_MAP_IGNORE";

        public static bool IsIgnored(this ITypeMember member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static void Ignore(this ITypeMember member, bool value = true)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static ClassTypeMemberBuilder<TType, TMember> Exclude<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member)
        {
            member.MemberOptions.Ignore(true);
            return member;
        }

        public static ClassTypeMemberBuilder<TType, TMember> Include<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.MemberOptions.Ignore(false);
            return member;
        }

        public static bool IsIgnored(this TypePairMember member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static bool IsMapped(this TypePairMember member)
        {
            return !member.Annotations.ContainsKey(KEY);
        }

        public static void Exclude(this TypePairMember member)
        {
            member.Annotations[KEY] = true;
        }

        public static TypePairMemberBuilder<TType, TMember> Exclude<TType, TMember>(this TypePairMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.TypePairMember.Exclude();
            return member;
        }

        public static void Include(this TypePairMember member)
        {
            member.Annotations.Remove(KEY);
        }

        public static TypePairMemberBuilder<TType, TMember> Include<TType, TMember>(this TypePairMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.TypePairMember.Include();
            return member;
        }
    }
}