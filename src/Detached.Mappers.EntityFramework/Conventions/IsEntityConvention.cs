using Detached.Mappers.Model;
using Detached.Mappers.Model.Types.Class;
using Detached.Mappers.Model.Types.Class.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Detached.Mappers.EntityFramework.Conventions
{
    public class IsEntityConvention : ITypeOptionsConvention
    {
        readonly IModel _model;

        public IsEntityConvention(IModel model)
        {
            _model = model;
        }

        public void Apply(MapperOptions modelOptions, ClassTypeOptions typeOptions)
        {
            IEntityType entityType = _model.FindEntityType(typeOptions.Type);

            typeOptions.IsEntity = entityType != null && !entityType.IsOwned();
        }
    }
}