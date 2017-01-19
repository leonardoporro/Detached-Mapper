using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Errors
{
    public class HandledException<TError> : Exception
    {
        public HandledException(TError errorData)
        {

        }
    }

    public class ApiException : Exception
    {
        public ApiException(string message)
            : base(message)
        {

        }

        public object ClientData { get; set; }
    }
}
