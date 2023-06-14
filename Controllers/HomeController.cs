using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Json("Hello World");
        }
    }
}
