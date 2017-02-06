using Detached.Mvc.Localization.Errors;
using Detached.Mvc.Localization.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using System;

namespace Detached.Mvc.Errors
{
    public class LocalizedApiErrorsFilter : ExceptionFilterAttribute
    {
        IErrorLocalizer _errorLocalizer;

        public LocalizedApiErrorsFilter(IErrorLocalizer errorLocalizer)
        {
            _errorLocalizer = errorLocalizer;
        }

        public override void OnException(ExceptionContext context)
        {
            ApiError apiError = new ApiError();
            int httpStatusCode;

            if (context.Exception is ApiException)
            {
                ApiException apiException = (ApiException)context.Exception;
                httpStatusCode = apiException.HttpStatusCode;
                apiError.ErrorCode = apiException.ErrorCode;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                apiError.ErrorCode = ApiErrorCodes.UnauthorizedAccess;
                httpStatusCode = StatusCodes.Status403Forbidden;
            }
            else
            {
                apiError.ErrorCode = ApiErrorCodes.InternalError;
                httpStatusCode = StatusCodes.Status500InternalServerError;
            }

            apiError.ErrorMessage = 
                _errorLocalizer.GetLocalizedErrorMessage(context.ActionDescriptor as ControllerActionDescriptor,
                                                         apiError.ErrorCode);
           
            context.Result = new ObjectResult(apiError)
            {
                StatusCode = httpStatusCode
            };

            base.OnException(context);
        }
    }
}