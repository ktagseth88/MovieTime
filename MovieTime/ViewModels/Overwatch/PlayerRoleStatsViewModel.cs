using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTime.ViewModels.Overwatch
{
    public class PlayerRoleStatsViewModel
    {
        public string PlayerName { get; set; }
        public int PlayerId { get; set; }
        public string Role { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
    }
}
