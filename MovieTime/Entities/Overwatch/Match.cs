using System;
using System.Collections.Generic;

namespace MovieTime.Entities.Overwatch
{
    public partial class Match
    {
        public Match()
        {
            PlayerMatchXref = new HashSet<PlayerMatchXref>();
        }

        public int MatchId { get; set; }
        public int MapId { get; set; }
        public DateTime? Timestamp { get; set; }
        public bool Victory { get; set; }

        public virtual Map Map { get; set; }
        public virtual ICollection<PlayerMatchXref> PlayerMatchXref { get; set; }
    }
}
