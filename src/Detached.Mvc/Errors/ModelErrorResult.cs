using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Errors
{
    public class ModelErrorResult : ClientErrorData
    {
        public Dictionary<string, string> ModelErrors { get; set; }
    }
}
