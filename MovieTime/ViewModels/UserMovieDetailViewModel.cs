using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTime.ViewModels
{
    public class UserMovieDetailViewModel
    {
        public string MovieTitle { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public byte? Rating { get; set; }
    }
}
