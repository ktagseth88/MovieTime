using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace MovieTime.Entities.Overwatch
{
    public partial class OverwatchContext : DbContext
    {
        public virtual DbSet<PlayerRoleRecord> PlayerRoleRecords { get; set; }

        protected void ViewBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerRoleRecord>(prr =>
            {
                prr.ToView("v_player_role_record", "overwatch");
                prr.HasNoKey();
                prr.Property(x => x.PlayerName).HasColumnName("player_name");
                prr.Property(x => x.Losses).HasColumnName("losses");
                prr.Property(x => x.Wins).HasColumnName("wins");
                prr.Property(x => x.Role).HasColumnName("role");
                prr.Property(x => x.PlayerId).HasColumnName("player_id");
            });
        }
    }
}
