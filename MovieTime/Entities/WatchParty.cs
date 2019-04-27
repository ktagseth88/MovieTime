using System;
using System.Collections.Generic;

namespace MovieTime.Entities
{
    public partial class WatchParty
    {
        public WatchParty()
        {
            UserWatchPartyXref = new HashSet<UserWatchPartyXref>();
        }

        public int WatchPartyId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserWatchPartyXref> UserWatchPartyXref { get; set; }
    }
}
