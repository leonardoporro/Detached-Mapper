using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Detached.Mvc.Errors
{
    public class ClientErrorData
    {
        public int Code { get; set; } = (int)HttpStatusCode.InternalServerError;

        public string Message { get; set; }

        public string Exception { get; set; }
    }
}
