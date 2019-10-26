using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTime.Common.Models
{
    public class UserReview
    {
        public byte? Rating { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
        public string MovieTitle { get; set; }
        public int MovieId { get; set; }
        public bool? WouldRewatch { get; set; }
    }
}
