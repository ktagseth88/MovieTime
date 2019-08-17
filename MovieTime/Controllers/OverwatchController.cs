using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTime.Entities.Overwatch;
using MovieTime.ViewModels.Overwatch;

namespace MovieTime.Controllers
{
    [AllowAnonymous]
    public class OverwatchController : Controller
    {
        private readonly OverwatchContext _overwatchDb;
        public OverwatchController(OverwatchContext overWatchDb)
        {
            _overwatchDb = overWatchDb;
        }

        public IActionResult Index()
        {

            var oujouh = _overwatchDb.Match
                .Include(x => x.PlayerMatchXref)
                    .ThenInclude(x => x.Player)
                .Include(x => x.Map)
                .ToList();

            var ooo = new List<MatchHistoryViewModel>();
            foreach(var k in oujouh)
            {
                ooo.Add(new MatchHistoryViewModel
                {
                    MapName = k.Map.Name,
                    IsVictory = k.Victory,
                    EndTime = k.Timestamp,
                    PlayerRole = k.PlayerMatchXref.ToDictionary(x => x.Player.Name, x => x.Role)
                });
            }

            //var vm = _overwatchDb.Match.Select(x => new MatchHistoryViewModel
            //{
            //    MapName = x.Map.Name,
            //    IsVictory = x.Victory,
            //    PlayerRole = x.PlayerMatchXref.ToDictionary(y => y.Player.Name, y => y.Role)
            //}).ToList();

            return View(ooo);
        }
    }
}