using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieTime.Entities.Overwatch;
using Microsoft.EntityFrameworkCore;
using MovieTime.Models.ApiModels;

namespace MovieTime.Services.Overwatch
{
    public class MatchService
    {
        private readonly OverwatchContext _overwatchDb;
        public MatchService(OverwatchContext overwatchDb)
        {
            _overwatchDb = overwatchDb;
        }

        public IEnumerable<Models.ApiModels.Match> GetAllMatches()
        {
            var matchEntities = _overwatchDb.Match
                .Include(x => x.PlayerMatchXref)
                    .ThenInclude(x => x.Player)
                .Include(x => x.Map)
                .ToList();

            return matchEntities.Select(x => new Models.ApiModels.Match
            {
                IsVictory = x.Victory,
                Map = x.Map.Name,
                Id = x.MatchId,
                Players = x.PlayerMatchXref.Select(y => new MatchPlayer
                {
                    PlayerName = y.Player.Name,
                    Role = y.Role
                })
            });
        }
    }
}
