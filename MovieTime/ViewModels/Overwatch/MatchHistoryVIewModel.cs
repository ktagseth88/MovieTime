using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTime.ViewModels.Overwatch
{
    public class MatchHistoryViewModel
    {
        public bool IsVictory { get; set; }
        public string MapName { get; set; }
        public DateTime? EndTime { get; set; }

        public Dictionary<string, string> PlayerRole { get; set; }

        public string PlayerSummary => string.Concat(PlayerRole.Select(x => $"({x.Key}-{x.Value}) "));
    }
}
