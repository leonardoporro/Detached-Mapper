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
    /// <summary>
    /// Handles exceptions and return errors in an uniform way wrapping them in an
    /// ApiError object instance.
    /// </summary>
    public class ApiErrorsFilter : ExceptionFilterAttribute
    {
        IErrorLocalizer _errorLocalizer;

        /// <summary>
        /// Creates a new instance of ApiErrorsFilter.
        /// </summary>
        /// <param name="errorLocalizer">A localizer service to translate the errors.</param>
        public ApiErrorsFilter(IErrorLocalizer errorLocalizer)
        {
            _errorLocalizer = errorLocalizer;
        }

        public override void OnException(ExceptionContext context)
        {
            // this is the error object that will be returned.
            ApiError apiError = new ApiError();
            int httpStatusCode;

            // check for ApiException or other known exceptions.
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

            // localize the error message.
            apiError.ErrorMessage = 
                _errorLocalizer.GetLocalizedErrorMessage(context.ActionDescriptor as ControllerActionDescriptor,
                                                         apiError.ErrorCode);
           
            // return the result.
            context.Result = new ObjectResult(apiError)
            {
                StatusCode = httpStatusCode
            };

            base.OnException(context);
        }
    }
}