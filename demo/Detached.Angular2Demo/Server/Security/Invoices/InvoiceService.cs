using Detached.Angular2Demo.Model;
using Detached.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Detached.EntityFramework;

namespace Detached.Angular2Demo.Server.Security.Invoices
{
    public class InvoiceService : DetachedService<DefaultContext, Invoice, InvoiceQuery>, IInvoiceService
    {
        public InvoiceService(IDetachedContext<DefaultContext> detachedContext) : base(detachedContext)
        {
        }
    }

    public interface IInvoiceService : IDetachedService<Invoice, InvoiceQuery>
    {

    }
}
