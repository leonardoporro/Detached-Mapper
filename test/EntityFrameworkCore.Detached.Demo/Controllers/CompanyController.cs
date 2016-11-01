using EntityFrameworkCore.Detached.Demo.Model;
using EntityFrameworkCore.Detached.Demo.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Demo.Controllers
{
    [Route("api/company")]
    public class CompanyController : Controller
    {
        IDetachedContext<MainDbContext> _detached;
        static bool initialized = false;

        public CompanyController(IDetachedContext<MainDbContext> detached)
        {
            _detached = detached;
            if (!initialized)
            {
                Initialize().Wait();
                initialized = true;
            }
        }

        async Task Initialize()
        {
            await _detached.DbContext.Database.EnsureDeletedAsync();
            await _detached.DbContext.Database.EnsureCreatedAsync();

            _detached.DbContext.AddRange(new[]
            {
                new SellPointType { Name = "Type 1" },
                new SellPointType { Name = "Type 2" },
                new SellPointType { Name = "Type 3" },
                new SellPointType { Name = "Type 4" }
            });
            await _detached.DbContext.SaveChangesAsync();

            await _detached.UpdateAsync(new Company()
            {
                Name = "Hello Company!",
                SellPoints = new List<SellPoint>(new[]
                {
                    new SellPoint { Name = "USA", Address = "9999 12th Avenue Seattle, WA 98122, EE. UU.", Type = new SellPointType { Id = 1 }},
                    new SellPoint { Name = "Argentina", Address = "Mitre 564, Rosario, Santa Fe, Argentina", Type = new SellPointType { Id = 1 } },
                    new SellPoint { Name = "Germany", Address = "Gartenstraße 1000, 30161 Hannover, Germany", Type = new SellPointType { Id = 1 } },
                    new SellPoint { Name = "France", Address = "500 Rue Saint-Paul 34000 Montpellier, France", Type = new SellPointType { Id = 1 } }
                })
            });
            await _detached.SaveChangesAsync();
        }

        [HttpGet("{id}")]
        public async Task<Company> Get(int id)
        {
            return await _detached.LoadAsync<Company>(id);
        }

        [HttpPost]
        public async Task<JsonResult> Post([FromBody]Company company)
        {
            try
            {
                SessionInfoProvider.Default.CurrentUser = "CurrentUser";

                await _detached.UpdateAsync(company);
                await _detached.SaveChangesAsync();

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
