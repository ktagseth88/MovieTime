using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTime.ViewModels.WatchParty
{
    public class WatchPartyViewModel
    {
        public IEnumerable<string> Users { get; set; }
        public IEnumerable<string> Movies { get; set; }
        public int? WatchPartyId { get; set; }
    }
}
