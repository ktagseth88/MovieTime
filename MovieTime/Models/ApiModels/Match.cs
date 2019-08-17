using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTime.Models.ApiModels
{
    public class Match
    {
        public int Id { get; set; }
        public bool IsVictory { get; set; }
        public string Map { get; set; }
        public IEnumerable<MatchPlayer> Players { get; set; }
    }

    public class MatchPlayer
    {
        public string PlayerName { get; set; }
        public string Role { get; set; }
    }
}
