using Detached.Model;
using Detached.Model.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Detached.EntityFramework.Conventions
{
    public class IsEntityConvention : ITypeOptionsConvention
    {
        readonly IModel _model;

        public IsEntityConvention(IModel model)
        {
            _model = model;
        }

        public void Apply(MapperModelOptions modelOptions, ClassTypeOptions typeOptions)
        {
            IEntityType entityType = _model.FindEntityType(typeOptions.Type);

            typeOptions.IsEntity = entityType != null && !entityType.IsOwned();
        }
    }
}