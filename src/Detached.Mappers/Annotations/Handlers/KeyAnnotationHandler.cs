using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Options;
using Detached.Mappers.Types;
using System.ComponentModel.DataAnnotations;

namespace Detached.Mappers.Annotations.Handlers
{
    public class KeyAnnotationHandler : AnnotationHandler<KeyAttribute>
    {
        public override void Apply(KeyAttribute annotation, MapperOptions mapperOptions, IType type, ITypeMember member)
        {
            member.Key(true);
        }
    }
}