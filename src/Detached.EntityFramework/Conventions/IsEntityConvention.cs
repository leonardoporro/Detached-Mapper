using Detached.Model;
using Detached.Model.Conventions;
using Microsoft.EntityFrameworkCore;

namespace Detached.EntityFramework.Conventions
{
    public class IsEntityConvention : ITypeOptionsConvention
    {
        readonly DbContext _dbContext;

        public IsEntityConvention(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Apply(ModelOptions modelOptions, ClassTypeOptions typeOptions)
        {
            typeOptions.IsEntity = _dbContext.Model.FindEntityType(typeOptions.Type) != null;
        }
    }
}