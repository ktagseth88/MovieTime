using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTime.Entities.Overwatch;
using MovieTime.Models;
using MovieTime.Services.Overwatch;
using MovieTime.ViewModels.Overwatch;

namespace MovieTime.Controllers
{
    [Authorize]
    public class OverwatchController : Controller
    {
        private readonly OverwatchContext _overwatchDb;
        private readonly MatchService _matchService;
        public OverwatchController(OverwatchContext overWatchDb, MatchService matchService)
        {
            _overwatchDb = overWatchDb;
            _matchService = matchService;
        }

        public IActionResult Index()
        {
            var matchEntities = _overwatchDb.Match
                .Include(x => x.PlayerMatchXref)
                    .ThenInclude(x => x.Player)
                .Include(x => x.Map)
                .ToList();

            var matchHistory = new List<MatchHistoryViewModel>();
            foreach (var match in matchEntities)
            {
                matchHistory.Add(new MatchHistoryViewModel
                {
                    MapName = match.Map.Name,
                    IsVictory = match.Victory,
                    EndTime = match.Timestamp,
                    PlayerRole = match.PlayerMatchXref.ToDictionary(x => x.Player.Name, x => x.Role)
                });
            }

            return View(matchHistory);
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
            playerSelectList.Add(new SelectListItem { Text = "n/a", Value = "-1", Selected = true });
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
            if (MatchContainsDuplicatePlayers(match))
            {
                return View("Error", new ErrorViewModel { Message = "Cannot contain duplicate players" });
            }

            var newMatch = new Match
            {
                MapId = int.Parse(match.MapId),
                Victory = match.IsVictory
            };
            _overwatchDb.Match.Add(newMatch);
            _overwatchDb.SaveChanges();

            if (match.FirstDpsId != "-1")
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

        private bool MatchContainsDuplicatePlayers(CreateMatchViewModel match)
        {
            var playerIds = new List<string>
            {
                match.FirstDpsId,
                match.SecondDpsId,
                match.FirstHealerId,
                match.SecondHealerId,
                match.FirstTankId,
                match.SecondTankId
            };

            var playerCounter = playerIds.Where(x => x != "-1").Count();

            return playerCounter != (playerIds.Distinct().Count() - 1);
        }

        [HttpGet]
        public IActionResult PlayerRoleStats(string playerIdentifier = null)
        {
            if (playerIdentifier != null)
            {
                return GetRoleStatsForPlayer(playerIdentifier);
            }
            var roleRecords = _matchService.GetPlayerRoleRecords();

            var viewModels = roleRecords.Select(x => new PlayerRoleStatsViewModel
            {
                PlayerId = x.PlayerId,
                PlayerName = x.PlayerName,
                Losses = x.Losses,
                Wins = x.Wins,
                Role = x.Role
            });

            ViewBag.PlayerSelectList = _overwatchDb.Player.Select(x => new SelectListItem
            {
                Value = x.PlayerId.ToString(),
                Text = x.Name
            }).ToList();

            ViewBag.PlayerSelectList.Add(new SelectListItem
            {
                Value = "All",
                Selected = true,
                Text = "All"
            });

            return View(viewModels);
        }

        private IActionResult GetRoleStatsForPlayer(string playerIdentifier)
        {
            var roleRecords = new List<Models.ApiModels.PlayerRoleRecord>();
            if (playerIdentifier == "All")
            {
                roleRecords = _matchService.GetPlayerRoleRecords().ToList();
            }
            else
            {
                roleRecords = _matchService.GetPlayerRoleRecords(playerIdentifier).ToList();
            }

            var viewModels = roleRecords.Select(x => new PlayerRoleStatsViewModel
            {
                PlayerId = x.PlayerId,
                PlayerName = x.PlayerName,
                Losses = x.Losses,
                Wins = x.Wins,
                Role = x.Role
            });

            return PartialView("_playerRoleStatsTable", viewModels);
        }
    }
}