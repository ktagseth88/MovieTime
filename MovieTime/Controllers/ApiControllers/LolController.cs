using Microsoft.AspNetCore.Mvc;
using MovieTime.Models.Lol;
using MovieTime.Services.Lol;

namespace MovieTime.Controllers.ApiControllers
{
    public class LolController : Controller
    {
        private readonly PlayerStatService _playerStatService;
        public LolController(PlayerStatService playerStatService)
        {
            _playerStatService = playerStatService;
        }

        [HttpGet]
        [Route("{controller}/Winrates")]
        public async Task<ActionResult<IEnumerable<PlayerChampionWinRate>>> GetPlayerChampWinrates([FromQuery]string summonerName)
        {
            return Ok(await _playerStatService.GetPlayerStats(summonerName));
        }
    }
}
