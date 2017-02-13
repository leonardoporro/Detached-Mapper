using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Errors
{
    /// <summary>
    /// Provides a standard object to report API errors in an uniform way.
    /// </summary>
    public class ApiError
    { 
        /// <summary>
        /// The error code/key.
        /// ApiErrorCodes class contains some standard error codes, but any other
        /// error code can be defined by the user.
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Formatted/localized error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Debug information that will be sent in debug mode only.
        /// It could contain sensitive information, like class names or object values.
        /// </summary>
        public string DebugInfo { get; set; }
    }
}