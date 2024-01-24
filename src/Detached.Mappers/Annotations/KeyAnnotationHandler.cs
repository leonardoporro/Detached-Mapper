using Detached.Mappers.TypePairs;
using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.Mappers.Types.Class.Builder;
using System.ComponentModel.DataAnnotations;

namespace Detached.Mappers.Annotations
{
    public class KeyAnnotationHandler : AnnotationHandler<KeyAttribute>
    {
        public override void Apply(KeyAttribute annotation, MapperOptions mapperOptions, ClassType typeOptions, ClassTypeMember memberOptions)
        {
            memberOptions.Key(true);
        }
    }
}

namespace Detached.Mappers
{
    public static class KeyAnnotationHandlerHandlerExtensions
    {
        public const string VALUE_KEY = "DETACHED_KEY";

        public static bool IsKey(this ITypeMember member)
        {
            return member.Annotations.TryGetValue(VALUE_KEY, out var value) && Equals(value, true);
        }

        public static bool IsKey(this TypePairMember member)
        {
            return member.Annotations.TryGetValue(VALUE_KEY, out var value) && Equals(value, true);
        }

        public static void Key(this ITypeMember member, bool value = true)
        {
            member.Annotations[VALUE_KEY] = value;
        }

        public static void Key<TType, TMember>(this ClassTypeMemberBuilder<TType, TMember> member, bool value = true)
        {
            member.MemberOptions.Key(value);
        }

        public static void Key(this TypePairMember member, bool value = true)
        {
            if (value)
                member.Annotations[VALUE_KEY] = true;
            else
                member.Annotations[VALUE_KEY] = false;
        }

        public static bool IsKeyConfigured(this ITypeMember member)
        {
            return member.Annotations.ContainsKey(VALUE_KEY);
        }
    }
}