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

        public IEnumerable<Models.ApiModels.PlayerRoleRecord> GetPlayerRoleRecords()
        {            
            return _overwatchDb.PlayerRoleRecords.Select(x => new Models.ApiModels.PlayerRoleRecord
            {
                PlayerName = x.PlayerName,
                Losses = x.Losses,
                Wins = x.Wins,
                Role = x.Role,
                PlayerId = x.PlayerId
            });
        }

        public IEnumerable<Models.ApiModels.PlayerRoleRecord> GetPlayerRoleRecords(string playerIdentifier)
        {
            try
            {
                var playerId = ResolvePlayerIdentifier(playerIdentifier);

                return _overwatchDb.PlayerRoleRecords
                    .Where(x => x.PlayerId == playerId)
                    .Select(x => new Models.ApiModels.PlayerRoleRecord
                    {
                        PlayerName = x.PlayerName,
                        Losses = x.Losses,
                        Wins = x.Wins,
                        Role = x.Role,
                        PlayerId = x.PlayerId
                    });
            }
            catch
            {
                throw new Exception("Unable to find player");
            }
        }

        private int ResolvePlayerIdentifier(string playerIdentifier)
        {
            playerIdentifier = playerIdentifier.ToLower();
            if(int.TryParse(playerIdentifier, out int playerId))
            {
                return _overwatchDb.Player.FirstOrDefault(x => x.PlayerId == playerId).PlayerId;
            }
            else
            {
                return _overwatchDb.Player.FirstOrDefault(x => x.Name == playerIdentifier).PlayerId;
            }
        }
    }
}
