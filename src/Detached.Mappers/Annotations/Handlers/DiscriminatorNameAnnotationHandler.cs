using Detached.Annotations;
using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Options;
using Detached.Mappers.Types;

namespace Detached.Mappers.Annotations.Handlers
{
    public class DiscriminatorNameAnnotationHandler : AnnotationHandler<DiscriminatorNameAttribute>
    {
        public override void Apply(DiscriminatorNameAttribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member)
        {
            type.SetDiscriminatorName(annotation.PropertyName);
        }
    } 
}
