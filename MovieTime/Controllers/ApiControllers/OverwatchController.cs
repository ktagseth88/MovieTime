using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTime.Models.ApiModels;
using MovieTime.Services.Overwatch;

namespace MovieTime.Controllers.ApiControllers
{
    [Produces("application/json")]
    [Route("api/Overwatch")]
    public class OverwatchController : Controller
    {
        private readonly MatchService _matchService;
        public OverwatchController(MatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet]
        [Route("Matches")]
        public ActionResult<IEnumerable<Match>> GetMatchHistory()
        {
            return Ok(_matchService.GetAllMatches());
        }

        [HttpGet]
        [Route("Players/RoleSummaries")]
        public ActionResult<IEnumerable<Models.ApiModels.PlayerRoleRecord>> GetPlayerRoleSummaries()
        {
            return Ok(_matchService.GetPlayerRoleRecords());
        }

        [HttpGet]
        [Route("Players/{playerIdentifier}/RoleSummaries")]
        public ActionResult<IEnumerable<Models.ApiModels.PlayerRoleRecord>> GetPlayerRoleSummaries(string playerIdentifier)
        {
            try
            {
                return Ok(_matchService.GetPlayerRoleRecords(playerIdentifier));
            }
            catch
            {
                return NotFound(playerIdentifier);
            }
        }
    }
}