using System;
using System.Collections.Generic;

namespace MovieTime.DAL.EFCore.Entities
{
    public partial class Director
    {
        public Director()
        {
            Movie = new HashSet<Movie>();
        }

        public int DirectorId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Movie> Movie { get; set; }
    }
}
