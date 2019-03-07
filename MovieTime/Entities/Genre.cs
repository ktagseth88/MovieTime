using System;
using System.Collections.Generic;

namespace MovieTime.Entities
{
    public partial class Genre
    {
        public Genre()
        {
            Movie = new HashSet<Movie>();
        }

        public int GenreId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Movie> Movie { get; set; }
    }
}
