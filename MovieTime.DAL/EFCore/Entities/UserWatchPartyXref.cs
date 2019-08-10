using System;
using System.Collections.Generic;

namespace MovieTime.DAL.EFCore.Entities
{
    public partial class UserWatchPartyXref
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int WatchPartyId { get; set; }

        public virtual User User { get; set; }
        public virtual WatchParty WatchParty { get; set; }
    }
}
