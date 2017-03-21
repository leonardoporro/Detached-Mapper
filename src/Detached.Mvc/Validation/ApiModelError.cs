using Detached.Mvc.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Validation
{
    public class ApiModelError : ApiError
    {
        public Dictionary<string, string> Members { get; set; } = new Dictionary<string, string>();
    }
}
