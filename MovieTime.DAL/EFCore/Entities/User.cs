using System;
using System.Collections.Generic;

namespace MovieTime.DAL.EFCore.Entities
{
    public partial class User
    {
        public User()
        {
            Review = new HashSet<Review>();
            UserWatchPartyXref = new HashSet<UserWatchPartyXref>();
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreateTimestamp { get; set; }

        public virtual ICollection<Review> Review { get; set; }
        public virtual ICollection<UserWatchPartyXref> UserWatchPartyXref { get; set; }
    }
}
