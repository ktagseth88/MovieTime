using Microsoft.EntityFrameworkCore;

namespace MovieTime.Entities.Lol
{
    public class LolContext : DbContext
    {
        public LolContext()
        {
        }

        public LolContext(DbContextOptions<LolContext> options)
            : base(options)
        {
        }
    }
}
