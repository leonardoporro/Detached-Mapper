using Detached.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Angular2Demo.Server.Security.Invoices
{
    public class Invoice
    {
        public int Id { get; set; }
        
        public string Customer { get; set; }

        [Owned]
        public IList<InvoiceRow> Rows { get; set; }

        [Required(ErrorMessage = "XXX")]
        public string N { get; set; }
    }
}
