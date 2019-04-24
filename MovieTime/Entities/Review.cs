using System;
using System.Collections.Generic;

namespace MovieTime.Entities
{
    public partial class Review
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string ReviewText { get; set; }
        public byte? Rating { get; set; }
        public DateTime CreateTimestamp { get; set; }
        public DateTime? ModifyTimestamp { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual User User { get; set; }
    }
}
