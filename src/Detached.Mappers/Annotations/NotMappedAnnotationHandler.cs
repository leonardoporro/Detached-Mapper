using Detached.Mappers.Model;
using Detached.Mappers.Model.Types.Class;
using System.ComponentModel.DataAnnotations.Schema;

namespace Detached.Mappers.Annotations
{
    public class NotMappedAnnotationHandler : AnnotationHandler<NotMappedAttribute>
    {
        public override void Apply(NotMappedAttribute annotation, MapperModelOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            memberOptions.Ignored = true;
        }
    }
}
