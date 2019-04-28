using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTime.Models
{
    public class WatchPartyModel
    {
        public int? WatchPartyId { get; set; }
        public string PartyName { get; set; }
        public IEnumerable<string> Users { get; set; }
    }
}
