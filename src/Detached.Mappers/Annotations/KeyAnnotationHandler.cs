using Detached.Mappers.Model;
using Detached.Mappers.Model.Types.Class;
using System.ComponentModel.DataAnnotations;

namespace Detached.Mappers.Annotations
{
    public class KeyAnnotationHandler : AnnotationHandler<KeyAttribute>
    {
        public override void Apply(KeyAttribute annotation, MapperModelOptions modelOptions, ClassTypeOptions typeOptions, ClassMemberOptions memberOptions)
        {
            memberOptions.IsKey = true;
        }
    }
}
