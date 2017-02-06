using Detached.Mvc.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Validation
{
    public class ModelError : ApiError
    {
        public Dictionary<string, string> MemberErrors { get; set; } = new Dictionary<string, string>();
    }
}
