using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTime.Models
{
    public class UserReviewModel
    {
        public byte? Rating { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
        public string MovieTitle { get; set; }
        public int movie_id { get; set; }
    }
}
