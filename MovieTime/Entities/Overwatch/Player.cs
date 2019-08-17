using System;
using System.Collections.Generic;

namespace MovieTime.Entities.Overwatch
{
    public partial class Player
    {
        public Player()
        {
            PlayerMatchXref = new HashSet<PlayerMatchXref>();
        }

        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int? TankSr { get; set; }
        public int? HealSr { get; set; }
        public int? DpsSr { get; set; }

        public virtual ICollection<PlayerMatchXref> PlayerMatchXref { get; set; }
    }
}
