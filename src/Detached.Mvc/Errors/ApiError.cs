using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Errors
{
    public class ApiError
    { 
        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string DebugInfo { get; set; }
    }
}