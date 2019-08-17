using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTime.Entities.Overwatch
{
    public class PlayerRoleRecord
    {
        public int Wins { get; set; }
        public int Losses { get; set; }
        public string Role { get; set; }
        public string PlayerName { get; set; }
        public int PlayerId { get; set; }
    }
}
