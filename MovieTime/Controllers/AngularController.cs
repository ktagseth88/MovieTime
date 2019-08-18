using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MovieTime.Controllers
{
    public class AngularController : Controller
    {
        public IActionResult Start()
        {
            return View();
        }
    }
}