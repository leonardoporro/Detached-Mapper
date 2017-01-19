using EntityFrameworkCore.Detached.Demo.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Detached.Services;

namespace Detached.Angular2Demo.Server.Security.Invoices
{
    public class InvoiceController : DetachedController<Invoice, InvoiceQuery>
    {
        public InvoiceController(IInvoiceService detachedService) : base(detachedService)
        {
        }
    }
}
