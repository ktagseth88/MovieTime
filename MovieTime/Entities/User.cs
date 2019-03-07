using System;
using System.Collections.Generic;

namespace MovieTime.Entities
{
    public partial class User
    {
        public User()
        {
            Review = new HashSet<Review>();
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public int PasswordHash { get; set; }
        public DateTime CreateTimestamp { get; set; }

        public virtual ICollection<Review> Review { get; set; }
    }
}
