using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTime.Common.Models
{
    public class UserMovieDetails
    {
        public string MovieTitle { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public byte? Rating { get; set; }
        public int ReviewId { get; set; }
    }
}
