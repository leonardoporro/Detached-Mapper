using Detached.Annotations;
using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Options;
using Detached.Mappers.Types;

namespace Detached.Mappers.Annotations.Handlers
{
    public class DiscriminatorValueAnnotationHandler : AnnotationHandler<DiscriminatorValueAttribute>
    {
        public override void Apply(DiscriminatorValueAttribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member)
        {
            type.AddDiscriminatorValue(annotation.Value, annotation.ConcreteType);
        }
    } 
}
