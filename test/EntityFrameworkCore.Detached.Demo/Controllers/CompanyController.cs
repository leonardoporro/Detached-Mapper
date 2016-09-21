using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EntityFrameworkCore.Detached.Demo.Model;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Detached.Demo.Controllers
{
    [Route("api/company")]
    public class CompanyController : Controller
    {
        MainDbContext dbContext;
        DetachedContext detached;

        public CompanyController(MainDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.detached = new DetachedContext(dbContext);
        }

        [HttpGet("{id}")]
        public async Task<Company> Get(int id)
        {
            return await detached.LoadAsync<Company>(id);
        }

        [HttpPost]
        public async Task<JsonResult> Post([FromBody]Company company)
        {
            try
            {
                await detached.SaveAsync(company);
                return Json(new { Result = true });
            }
            catch(Exception x)
            {
                JsonResult result = Json(new { Message = x.InnerException != null ? x.InnerException.Message : x.Message });
                result.StatusCode = 500;
                return result;
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
