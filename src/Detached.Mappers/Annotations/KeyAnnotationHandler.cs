using Detached.Mappers.TypeOptions.Types.Class;
using System.ComponentModel.DataAnnotations;

namespace Detached.Mappers.Annotations
{
    public class KeyAnnotationHandler : AnnotationHandler<KeyAttribute>
    {
        public override void Apply(KeyAttribute annotation, MapperOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            memberOptions.IsKey = true;
        }
    }
}
