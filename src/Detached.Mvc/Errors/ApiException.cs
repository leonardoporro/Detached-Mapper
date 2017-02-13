using Microsoft.AspNetCore.Http;
using System;

namespace Detached.Mvc.Errors
{
    /// <summary>
    /// Common known exception that allows to define the ErrorCode and the 
    /// StatusCode to be used by the ApiErrorsFilter.
    /// </summary>
    public class ApiException : Exception
    {
        #region Ctor.

        public ApiException(string errorCode, int httpStatus, string errorMessage, Exception innerException)
            : base(errorMessage, innerException)
        {
            ErrorCode = errorCode;
            HttpStatusCode = httpStatus;
        }

        public ApiException(string errorCode, int httpStatus, string errorMessage)
            : base(errorMessage)
        {
            ErrorCode = errorCode;
            HttpStatusCode = httpStatus;
        }

        public ApiException(string errorCode, int httpStatus)
        {
            ErrorCode = errorCode;
            HttpStatusCode = httpStatus;
        }

        public ApiException(string errorCode)
        {
            ErrorCode = errorCode;
            HttpStatusCode = StatusCodes.Status500InternalServerError;
        }

        #endregion

        /// <summary>
        /// A standarized code for this error. 
        /// ApiErrorCodes class provides some constants for known errors.
        /// </summary>
        public string ErrorCode { get; set; }
        
        /// <summary>
        /// The HTTP error to be returned for the request.
        /// </summary>
        public int HttpStatusCode { get; set; }
    }
}
