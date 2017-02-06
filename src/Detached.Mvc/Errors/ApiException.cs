using Microsoft.AspNetCore.Http;
using System;

namespace Detached.Mvc.Errors
{
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

        public string ErrorCode { get; set; }
        
        public int HttpStatusCode { get; set; }
    }
}
