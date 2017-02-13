using Microsoft.AspNetCore.Mvc;

namespace Detached.Mvc.Validation
{
    public class ModelErrorsAttribute : TypeFilterAttribute
    {
        public ModelErrorsAttribute() : base(typeof(ModelErrorsFilter))
        {
        }
    }
}
