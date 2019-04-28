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
        public IActionResult Index()
        {
            var watchPartyModel = _watchPartyService.GetWatchPartiesByUser(HttpContext.User.Identity.Name);
            var watchPartyViewModels = watchPartyModel.Select(x => new WatchPartyViewModel
            {
                WatchPartyId = x.WatchPartyId,
                Users = x.Users,
                Name = x.PartyName
            });

            return View(watchPartyViewModels);
        }

        [HttpGet]
        public IActionResult Edit(int? WatchPartyId)
        {
            if(WatchPartyId == null)
            {
                return View(new WatchPartyViewModel());
            }
            else
            {
                var model = _watchPartyService.GetWatchPartyById(WatchPartyId.Value);
                var viewModel = new WatchPartyViewModel
                {
                    Name = model.PartyName,
                    Users = model.Users,
                    WatchPartyId = model.WatchPartyId
                };
                return View(viewModel);
            }
        }

        [HttpPost]
        public IActionResult Edit(WatchPartyViewModel watchParty)
        {
            var model = new WatchPartyModel
            {
                Users = watchParty.Users,
                PartyName = watchParty.Name,
                WatchPartyId = watchParty.WatchPartyId
            };

            if(watchParty.WatchPartyId == null)
            {
                _watchPartyService.InsertWatchParty(model);
            }
            else
            {
                _watchPartyService.UpdateWatchParty(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetMovieSuggestion(int WatchPartyId)
        {
            string movieSuggestion = _watchPartyService.GetMovieRecomendation(WatchPartyId);
            return Ok(movieSuggestion);
        }
    }
}
