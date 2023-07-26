using Detached.Annotations;
using Detached.Mappers.TypePairs;
using Detached.Mappers.TypePairs.Builder;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers.Annotations
{
    public class MapCopyAnnotationHandler : AnnotationHandler<MapCopyAttribute>
    {
        public override void Apply(MapCopyAttribute annotation, MapperOptions mapperOptions, ClassType typeOptions, ClassTypeMember memberOptions)
        {
            memberOptions.MapCopy(true);
        }
    }
}

namespace Detached.Mappers
{
    public static class MapCopyAnnotationHandlerExtensions
    {
        public const string KEY = "DETACHED_MAPCOPY";

        public static bool IsMapCopy(this ITypeMember member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static void MapCopy(this ITypeMember member, bool value = true)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static ClassTypeMemberBuilder<TType, TMember> MapCopy<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.MemberOptions.MapCopy(value);

            return member;
        }

        public static bool IsMapCopy(this TypePairMember memberPair)
        {
            return memberPair.TargetMember.Annotations.ContainsKey(KEY) ||
                memberPair.SourceType.Annotations.ContainsKey(KEY) ||
                memberPair.Annotations.ContainsKey(KEY);
        }

        public static void MapCopy(this TypePairMember member, bool value = true)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static TypePairMemberBuilder<TType, TMember> MapCopy<TType, TMember>(this TypePairMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.TypePairMember.MapCopy();

            return member;
        }
    }
}