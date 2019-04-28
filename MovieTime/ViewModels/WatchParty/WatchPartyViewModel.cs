using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTime.ViewModels.WatchParty
{
    public class WatchPartyViewModel
    {
        public IEnumerable<string> Users { get; set; } = new List<string>();
        public IEnumerable<string> Movies { get; set; }
        public int? WatchPartyId { get; set; }
        public string Name { get; set; }
        public string UsersAsSingleString
        {
            get
            {
                return string.Join(", ", Users);
            }
            set
            {
                Users = value.Split(',').Select(x => x.Trim());
            }
        }
    }
}
