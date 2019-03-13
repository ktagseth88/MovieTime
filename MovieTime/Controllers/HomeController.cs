﻿using Microsoft.AspNetCore.Mvc;
using MovieTime.Entities;
using MovieTime.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace MovieTime.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private MovieTimeContext movieTimeContext;
        public HomeController(MovieTimeContext movieTimeContext)
        {
            this.movieTimeContext = movieTimeContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
