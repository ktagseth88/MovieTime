using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            return View(ooo);
        }

        [HttpGet]
        public IActionResult AddMatch()
        {
            var playerSelectList = _overwatchDb.Player
                .Select(x => new SelectListItem
                {
                    Value = x.PlayerId.ToString(),
                    Text = x.Name
                }).ToList();
            playerSelectList.Add(new SelectListItem { Text = "n/a", Value = "-1" });
            var mapSelectList = _overwatchDb.Map
                .Select(x => new SelectListItem
                {
                    Value = x.MapId.ToString(),
                    Text = x.Name
                });

            var mmm = new CreateMatchViewModel
            {
                FirstDps = playerSelectList,
                SecondDps = playerSelectList,
                FirstHealer = playerSelectList,
                SecondHealer = playerSelectList,
                FirstTank = playerSelectList,
                SecondTank = playerSelectList,
                Map = mapSelectList
            };

            return View(mmm);
        }

        [HttpPost]
        public IActionResult AddMatch(CreateMatchViewModel match)
        {
            var newMatch = new Match
            {
                MapId = int.Parse(match.MapId),
                Victory = match.IsVictory
            };
            _overwatchDb.Match.Add(newMatch);
            _overwatchDb.SaveChanges();


            if(match.FirstDpsId != "-1")
            {
                _overwatchDb.Add(new PlayerMatchXref
                {
                    PlayerId = int.Parse(match.FirstDpsId),
                    Role = "dps",
                    MatchId = newMatch.MatchId
                });
            }
            if (match.SecondDpsId != "-1")
            {
                _overwatchDb.Add(new PlayerMatchXref
                {
                    PlayerId = int.Parse(match.SecondDpsId),
                    Role = "dps",
                    MatchId = newMatch.MatchId
                });
            }
            if (match.FirstTankId != "-1")
            {
                _overwatchDb.Add(new PlayerMatchXref
                {
                    PlayerId = int.Parse(match.FirstTankId),
                    Role = "tank",
                    MatchId = newMatch.MatchId
                });
            }
            if (match.SecondTankId != "-1")
            {
                _overwatchDb.Add(new PlayerMatchXref
                {
                    PlayerId = int.Parse(match.SecondTankId),
                    Role = "tank",
                    MatchId = newMatch.MatchId
                });
            }
            if (match.FirstHealerId != "-1")
            {
                _overwatchDb.Add(new PlayerMatchXref
                {
                    PlayerId = int.Parse(match.FirstHealerId),
                    Role = "heal",
                    MatchId = newMatch.MatchId
                });
            }
            if (match.SecondHealerId != "-1")
            {
                _overwatchDb.Add(new PlayerMatchXref
                {
                    PlayerId = int.Parse(match.SecondHealerId),
                    Role = "heal",
                    MatchId = newMatch.MatchId
                });
            }

            _overwatchDb.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}