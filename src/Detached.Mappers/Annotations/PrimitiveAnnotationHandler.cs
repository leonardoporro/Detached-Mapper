using Detached.Annotations;
using Detached.Mappers.TypePairs;
using Detached.Mappers.TypePairs.Builder;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers.Annotations
{
    public class PrimitiveAnnotationHandler : AnnotationHandler<PrimitiveAttribute>
    {
        public override void Apply(PrimitiveAttribute annotation, MapperOptions mapperOptions, ClassType typeOptions, ClassTypeMember memberOptions)
        {
            memberOptions.Primitive(true);
        }
    }
}

namespace Detached.Mappers
{
    public static class MapCopyAnnotationHandlerExtensions
    {
        public const string KEY = "DETACHED_PRIMITIVE";
 
        public static void Primitive(this ITypeMember member, bool value = true)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static ClassTypeMemberBuilder<TType, TMember> Primitive<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.MemberOptions.Primitive(value);

            return member;
        }

        public static bool IsSetAsPrimitive(this TypePairMember memberPair)
        {
            return memberPair.TargetMember.Annotations.ContainsKey(KEY) ||
                memberPair.SourceType.Annotations.ContainsKey(KEY) ||
                memberPair.Annotations.ContainsKey(KEY);
        }

        public static void Primitive(this TypePairMember member, bool value = true)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static TypePairMemberBuilder<TType, TMember> Primitive<TType, TMember>(this TypePairMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.TypePairMember.Primitive();

            return member;
        }
    }
}