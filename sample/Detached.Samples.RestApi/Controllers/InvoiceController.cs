using Detached.Samples.RestApi.Models.Inputs;
using Detached.Samples.RestApi.Models.Outputs;
using Detached.Samples.RestApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detached.Samples.RestApi.Controllers
{
    [ApiController]
    [Route("api/invoices")]
    public class InvoiceController : Controller
    {
        readonly InvoiceService _invoiceService;

        public InvoiceController(InvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceListItem>>> GetAsync()
        {
            return Ok(await _invoiceService.GetAsync());
        }

        [HttpPost("identity_fks")]
        public async Task<ActionResult> SaveAsync_Identity([FromBody] SaveInvoiceInput_Identity input)
        {
            await _invoiceService.SaveAsync(input);
            return Ok();
        }

        [HttpPost("primitive_fks")]
        public async Task<ActionResult> SaveAsync_Primitive([FromBody] SaveInvoiceInput_Primitive input)
        {
            await _invoiceService.SaveAsync(input);
            return Ok();
        }
    }
}