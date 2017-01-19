using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Detached.Angular2Demo.Server.Home
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("./Server/Home/Views/Index.cshtml");
        }

        public IActionResult Error()
        {
            return View("./Server/Home/Views/Error.cshtml");
        }
    }
}
