using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTime.Common.Models
{
    public class WatchList
    {
        public string UserName { get; set; }
        public IEnumerable<(Movie Movie, byte? Rating)> Movies { get; set; }
    }
}
