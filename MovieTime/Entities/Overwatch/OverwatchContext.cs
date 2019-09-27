using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace MovieTime.Entities.Overwatch
{
    public class OverwatchContext : OverwatchContextBase
    {
        public OverwatchContext()
        {
        }

        public OverwatchContext(DbContextOptions<OverwatchContextBase> options)
            : base(options)
        {
        }

        public virtual DbSet<PlayerRoleRecord> PlayerRoleRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
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
