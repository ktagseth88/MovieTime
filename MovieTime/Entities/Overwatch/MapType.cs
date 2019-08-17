using System;
using System.Collections.Generic;

namespace MovieTime.Entities.Overwatch
{
    public partial class MapType
    {
        public MapType()
        {
            Map = new HashSet<Map>();
        }

        public int MapTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Map> Map { get; set; }
    }
}
