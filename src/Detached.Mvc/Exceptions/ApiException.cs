using System;

namespace Detached.Mvc.Exceptions
{
    public class ApiException<TError> : Exception
        where TError : ApiError
    {
        public ApiException(TError error)
            : base(error.Details)
        {
            Error = error;
        }

        public TError Error { get; set; }
    }
}
