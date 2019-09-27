using Microsoft.EntityFrameworkCore;
using MovieTime.Entities;
using MovieTime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTime.Services
{
    public class WatchPartyService
    {
        private MovieTimeContext _movieTimeDb;

        public WatchPartyService(MovieTimeContext movieTimeDb)
        {
            _movieTimeDb = movieTimeDb;
        }

        public async Task<IEnumerable<Models.WatchParty>> GetWatchPartiesByUser(string username)
        {
            var watchParty = await _movieTimeDb.WatchParty.AsNoTracking()
                .Where(x => x.UserWatchPartyXref
                    .Select(y => y.User.Username)
                    .Contains(username))
                .Select(x => new Models.WatchParty
                {
                    PartyName = x.Name,
                    Users = x.UserWatchPartyXref.Select(y => y.User.Username),
                    WatchPartyId = x.WatchPartyId
                }).ToListAsync();

            return watchParty;
        }

        public async Task UpdateWatchParty(Models.WatchParty watchParty)
        {
            var existingWatchPartyEntity = await _movieTimeDb.WatchParty.FirstOrDefaultAsync(x => x.WatchPartyId == watchParty.WatchPartyId);

            var updatedWatchPartyEntity = new Entities.WatchParty
            {
                Name = watchParty.PartyName,
                UserWatchPartyXref = watchParty.Users.Select(x => new UserWatchPartyXref
                {
                    UserId = _movieTimeDb.User.FirstOrDefault(y => y.Username == x).UserId,
                    WatchPartyId = existingWatchPartyEntity.WatchPartyId
                }).ToList(),
                WatchPartyId = existingWatchPartyEntity.WatchPartyId
            };

            existingWatchPartyEntity = updatedWatchPartyEntity;
            await _movieTimeDb.SaveChangesAsync();
        }

        public async Task InsertWatchParty(Models.WatchParty watchParty)
        {
            var watchPartyEntity = new Entities.WatchParty
            {
                Name = watchParty.PartyName
            };

            await _movieTimeDb.AddAsync(watchPartyEntity);
            await _movieTimeDb.SaveChangesAsync();//Have to make intermittent save to apply watch_party_id to new watch party

            foreach (var u in watchParty.Users.Intersect(_movieTimeDb.User.Select(y => y.Username)))
            {
                watchPartyEntity.UserWatchPartyXref.Add(new UserWatchPartyXref
                {
                    WatchPartyId = watchPartyEntity.WatchPartyId,
                    UserId = _movieTimeDb.User.FirstOrDefault(x => x.Username == u).UserId
                });
            }

            await _movieTimeDb.SaveChangesAsync();
        }

        public async Task<string> GetMovieRecomendation(int WatchPartyId)
        {
            var weightedMovieTitles = new List<string>();

            var watchListMovies = await (from uwpx in _movieTimeDb.UserWatchPartyXref
                                         join u in _movieTimeDb.User on uwpx.UserId equals u.UserId
                                         join rl in _movieTimeDb.Review on u.UserId equals rl.UserId
                                         where uwpx.WatchPartyId == WatchPartyId
                                         select new { user = u.Username, title = rl.Movie.Name, rating = rl.Rating }).ToListAsync();

            foreach (var m in watchListMovies)
            {
                //weightedMovieTitles.Add(m.title);
                if (!m.rating.HasValue)
                {
                    weightedMovieTitles.AddRange(new[] { m.title, m.title, m.title });
                }
            }

            var groupedWatchListMovies = watchListMovies.GroupBy(x => x.title).Where(x => x.Count() > 1);
            foreach (var x in groupedWatchListMovies)
            {
                weightedMovieTitles.AddRange(Enumerable.Repeat(x.First().title, x.Count()));
            }

            return weightedMovieTitles.ElementAt(new Random().Next(weightedMovieTitles.Count() - 1));
        }

        public async Task<Models.WatchParty> GetWatchPartyById(int watchPartyId)
        {
            var watchPartyModel = await _movieTimeDb.WatchParty.Where(x => x.WatchPartyId == watchPartyId).Select(x => new Models.WatchParty
            {
                PartyName = x.Name,
                Users = x.UserWatchPartyXref.Select(y => y.User.Username),
                WatchPartyId = x.WatchPartyId
            }).FirstOrDefaultAsync();

            return watchPartyModel;
        }
    }
}
