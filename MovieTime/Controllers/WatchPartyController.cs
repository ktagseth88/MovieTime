using Microsoft.AspNetCore.Mvc;
using MovieTime.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using CsvHelper;
using MovieTime.ViewModels.WatchParty;
using System.Collections.Generic;
using System.Linq;
using CsvHelper.Configuration;
using System;
using MovieTime.Services;

namespace MovieTime.Controllers
{
    [Authorize]
    public class WatchPartyController : Controller
    {
        private readonly WatchPartyService _watchPartyService;
        public WatchPartyController(WatchPartyService watchPartyService)
        {
            _watchPartyService = watchPartyService;
        }

        [HttpGet]
        public IActionResult GetUserWatchParties()
        {
            var watchPartyModel = _watchPartyService.GetWatchPartiesByUser(HttpContext.User.Identity.Name);
            var watchPartyViewModels = watchPartyModel.Select(x => new WatchPartyViewModel
            {
                WatchPartyId = x.WatchPartyId,
                Users = x.Users
            });

            return View("UserWatchParties", watchPartyViewModels);
        }
    }
}
