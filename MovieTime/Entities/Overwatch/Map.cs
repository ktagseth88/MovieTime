using System;
using System.Collections.Generic;

namespace MovieTime.Entities.Overwatch
{
    public partial class Map
    {
        public Map()
        {
            Match = new HashSet<Match>();
        }

        public int MapId { get; set; }
        public string Name { get; set; }
        public int MapTypeId { get; set; }

        public virtual MapType MapType { get; set; }
        public virtual ICollection<Match> Match { get; set; }
    }
}
