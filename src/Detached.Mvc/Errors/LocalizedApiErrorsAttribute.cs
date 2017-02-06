using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Errors
{
    public class LocalizedApiErrorsAttribute : TypeFilterAttribute
    {
        public LocalizedApiErrorsAttribute()
            : base(typeof(LocalizedApiErrorsFilter))
        {

        }
    }
}
