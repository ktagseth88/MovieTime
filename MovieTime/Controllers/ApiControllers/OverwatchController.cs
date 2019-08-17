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
            return  Ok(_matchService.GetAllMatches());
        }
    }
}