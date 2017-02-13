using Detached.Mvc.Errors;
using Detached.Mvc.Localization.Errors;
using Detached.Mvc.Localization.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Detached.Mvc.Validation
{
    public class ModelErrorsFilter : IAsyncActionFilter
    {
        IErrorLocalizer _errorLocalizer;
      
        public ModelErrorsFilter(IErrorLocalizer errorLocalizer)
        {
            _errorLocalizer = errorLocalizer;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid)
            {
                await next(); 
            }
            else
            {
                ModelError apiError = new ModelError();
                apiError.ErrorCode = ApiErrorCodes.InvalidModel;

                foreach (var entry in context.ModelState)
                {
                    if (entry.Value.Errors.Count > 0)
                        apiError.MemberErrors[entry.Key] = entry.Value.Errors[0].ErrorMessage;
                }

                apiError.ErrorMessage =
                    _errorLocalizer.GetLocalizedErrorMessage(context.ActionDescriptor as ControllerActionDescriptor, 
                                                             ApiErrorCodes.InvalidModel);

                context.Result = new ObjectResult(apiError)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }
    }
}
