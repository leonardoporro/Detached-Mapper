using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Exceptions
{
    public class ApiError
    { 
        public string Code { get; set; }

        public Dictionary<string, string> Arguments { get; set; } = new Dictionary<string, string>();

        public string Exception { get; set; }

        public string Details { get; set; }
    }
}