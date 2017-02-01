using Microsoft.AspNetCore.Mvc;

namespace Detached.Mvc.Validation
{
    public class ValidateModelAttribute : TypeFilterAttribute
    {
        public ValidateModelAttribute() : base(typeof(ValidateModelFilter))
        {
        }
    }
}
