using System;
using System.Collections.Generic;

namespace MovieTime.Entities
{
    public partial class Movie
    {
        public Movie()
        {
            Review = new HashSet<Review>();
        }

        public int MovieId { get; set; }
        public string Name { get; set; }
        public short ReleaseYear { get; set; }
        public int DirectorId { get; set; }
        public int GenreId { get; set; }
        public int SubGenreId { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual ICollection<Review> Review { get; set; }
    }
}
