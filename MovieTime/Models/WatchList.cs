using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTime.Models
{
    public class WatchList
    {
        public string UserName { get; set; }
        public int? Rating { get; set; }
        public IEnumerable<(Movie Movie, int? Rating)> Movies { get; set; }
    }
}
