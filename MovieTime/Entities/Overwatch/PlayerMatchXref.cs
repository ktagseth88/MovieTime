using System;
using System.Collections.Generic;

namespace MovieTime.Entities.Overwatch
{
    public partial class PlayerMatchXref
    {
        public int PlayerMatchXrefId { get; set; }
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public string Role { get; set; }

        public virtual Match Match { get; set; }
        public virtual Player Player { get; set; }
    }
}
