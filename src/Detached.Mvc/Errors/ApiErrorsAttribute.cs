using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Errors
{
    /// <summary>
    /// Handles exceptions and return errors in an uniform way wrapping them in an
    /// ApiError object instance.
    /// </summary>
    public class ApiErrorsAttribute : TypeFilterAttribute
    {
        public ApiErrorsAttribute()
            : base(typeof(ApiErrorsFilter))
        {

        }
    }
}
