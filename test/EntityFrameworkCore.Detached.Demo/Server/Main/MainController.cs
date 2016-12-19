using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Angular2Application2.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View("Server/Main/Index.cshtml");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
