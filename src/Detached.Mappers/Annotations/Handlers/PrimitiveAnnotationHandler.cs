using Detached.Annotations;
using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Options;
using Detached.Mappers.Types;

namespace Detached.Mappers.Annotations.Handlers
{
    public class PrimitiveAnnotationHandler : AnnotationHandler<PrimitiveAttribute>
    {
        public override void Apply(PrimitiveAttribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member)
        {
            member.Primitive(true);
        }
    }
}