using Microsoft.AspNetCore.Mvc;

namespace Detached.Mvc.Validation
{
    public class LocalizedModelValidation : TypeFilterAttribute
    {
        public LocalizedModelValidation() : base(typeof(LocalizedModelValidationFilter))
        {
        }
    }
}
