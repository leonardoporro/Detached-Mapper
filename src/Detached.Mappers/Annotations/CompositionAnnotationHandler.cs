using Detached.Annotations;
using Detached.Mappers.TypeOptions;
using Detached.Mappers.TypeOptions.Class;
using Detached.Mappers.TypeOptions.Class.Builder;

namespace Detached.Mappers.Annotations
{
    public class CompositionAnnotationHandler : AnnotationHandler<CompositionAttribute>
    {
        public override void Apply(CompositionAttribute annotation, MapperOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            memberOptions.IsComposition(true);
        }
    }

    public static class CompositionAnnotationHandlerExtensions
    {
        const string KEY = "DETACHED_COMPOSITION";

        public static bool IsComposition(this IMemberOptions member)
        {
            return member.Annotations.ContainsKey(KEY);
        }

        public static void IsComposition(this IMemberOptions member, bool value)
        {
            if (value)
                member.Annotations[KEY] = true;
            else
                member.Annotations.Remove(KEY);
        }

        public static void IsComposition<TType, TMember>(this ClassMemberOptionsBuilder<TType, TMember> member, bool value)
        {
            member.MemberOptions.IsComposition(value);
        }
    }
}