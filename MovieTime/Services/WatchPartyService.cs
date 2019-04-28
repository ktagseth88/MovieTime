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

        public IEnumerable<WatchPartyModel> GetWatchPartiesByUser(string username)
        {
            //TODO: Don't do this awful join thing, this goes around in circles with joins for no reason
            var watchParty = _movieTimeDb.User
                .Include(x => x.UserWatchPartyXref)
                    .ThenInclude(x => x.WatchParty)
                    .ThenInclude(x => x.UserWatchPartyXref)
                    .ThenInclude(x => x.User)
                .FirstOrDefault(x => x.Username == username)
                .UserWatchPartyXref
                .Select(x => new WatchPartyModel
                {
                    PartyName = x.WatchParty.Name,
                    Users = x.WatchParty.UserWatchPartyXref.Select(y => y.User.Username),
                    WatchPartyId = x.WatchPartyId
                });

            return watchParty;
        }

        public void UpdateWatchParty(WatchPartyModel watchParty)
        {
            var existingWatchPartyEntity = _movieTimeDb.WatchParty.FirstOrDefault(x => x.WatchPartyId == watchParty.WatchPartyId);

            var updatedWatchPartyEntity = new WatchParty
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
            _movieTimeDb.SaveChanges();
        }

        public void InsertWatchParty(WatchPartyModel watchParty)
        {
            var watchPartyEntity = new WatchParty
            {
                Name = watchParty.PartyName
            };

            _movieTimeDb.Add(watchPartyEntity);
            _movieTimeDb.SaveChanges();//Have to make intermittent save to apply watch_party_id to new watch party

            //TODO: make these horrible fucking lambda variable names not awful
            foreach (var u in watchParty.Users.Intersect(_movieTimeDb.User.Select(y => y.Username)))
            {
                watchPartyEntity.UserWatchPartyXref.Add(new UserWatchPartyXref
                {
                    WatchPartyId = watchPartyEntity.WatchPartyId,
                    UserId = _movieTimeDb.User.FirstOrDefault(x => x.Username == u).UserId
                });
            }

            _movieTimeDb.SaveChanges();
        }

        public string GetMovieRecomendation(int WatchPartyId)
        {
            var weightedMovieTitles = new List<string>();

            var watchListMovies = from uwpx in _movieTimeDb.UserWatchPartyXref
                       join u in _movieTimeDb.User on uwpx.UserId equals u.UserId
                       join rl in _movieTimeDb.Review on u.UserId equals rl.UserId
                       where uwpx.WatchPartyId == WatchPartyId
                       select new { user = u.Username, title = rl.Movie.Name, rating = rl.Rating };
            
            foreach (var m in watchListMovies)
            {
                weightedMovieTitles.Add(m.title);
                if(!m.rating.HasValue)
                {
                    weightedMovieTitles.AddRange(new[] {m.title, m.title});
                }
            }

            var groupedWatchListMovies = watchListMovies.GroupBy(x => x.title).Where(x => x.Count() > 1);
            foreach(var x in groupedWatchListMovies)
            {
                weightedMovieTitles.AddRange(Enumerable.Repeat(x.First().title, x.Count()));
            }
            Random randomGenerator = new Random();

            var selectedMovie = weightedMovieTitles.ElementAt(randomGenerator.Next(weightedMovieTitles.Count() - 1));
            return selectedMovie;
        }

        public WatchPartyModel GetWatchPartyById(int watchPartyId)
        {
            var watchPartyModel = _movieTimeDb.WatchParty.Where(x => x.WatchPartyId == watchPartyId).Select(x => new WatchPartyModel
            {
                PartyName = x.Name,
                Users = x.UserWatchPartyXref.Select(y => y.User.Username),
                WatchPartyId = x.WatchPartyId
            }).FirstOrDefault();

            return watchPartyModel;
        }
    }
}
