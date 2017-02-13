using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Errors
{
    /// <summary>
    /// Provides some standard error codes to be used in ApiError.ErrorCode.
    /// </summary>
    public static class ApiErrorCodes
    {
        public static string UnauthorizedAccess = "UnauthorizedAccess";
        public static string InternalError = "InternalError";
        public static string InvalidModel = "InvalidModel";
        public static string NotFound = "NotFound";
    }
}
