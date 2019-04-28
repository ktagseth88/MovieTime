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
            var watchParty = _movieTimeDb.User.FirstOrDefault().UserWatchPartyXref.Select(x => new WatchPartyModel
            {
                PartyName = x.WatchParty.Name,
                Users = x.WatchParty.UserWatchPartyXref.Select(y => y.User.Username)
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
            foreach(var u in watchParty.Users.Intersect(_movieTimeDb.User.Select(y => y.Username)))
            {
                watchPartyEntity.UserWatchPartyXref.Add(new UserWatchPartyXref
                {
                    WatchPartyId = watchPartyEntity.WatchPartyId,
                    UserId = _movieTimeDb.User.FirstOrDefault(x => x.Username == u).UserId
                });
            }
            _movieTimeDb.SaveChanges();
        }
    }
}
