using Detached.Mvc.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Detached.Mvc.Validation
{
    public class ValidateModelFilter : IAsyncActionFilter
    {
        IStringLocalizerFactory _localizerFactory;
      
        public ValidateModelFilter(IStringLocalizerFactory localizerFactory)
        {
            _localizerFactory = localizerFactory;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid)
            {
                await next();
            }
            else
            {
                IStringLocalizer localizer = _localizerFactory.Create(context.Controller.GetType());
                //ModelError modelError = new ModelError(context.ModelState);
                context.Result = new ObjectResult("")
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }
    }
}
