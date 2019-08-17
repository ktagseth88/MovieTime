using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTime.Models.ApiModels
{
    public class PlayerRoleRecord
    {
        public string PlayerName { get; set; }
        public int PlayerId { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public string Role { get; set; }
    }
}
