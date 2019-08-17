using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MovieTime.Entities.Overwatch
{
    public partial class OverwatchContext : DbContext
    {
        public OverwatchContext()
        {
        }

        public OverwatchContext(DbContextOptions<OverwatchContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Map> Map { get; set; }
        public virtual DbSet<MapType> MapType { get; set; }
        public virtual DbSet<Match> Match { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<PlayerMatchXref> PlayerMatchXref { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<Map>(entity =>
            {
                entity.ToTable("map", "overwatch");

                entity.Property(e => e.MapId).HasColumnName("map_id");

                entity.Property(e => e.MapTypeId).HasColumnName("map_type_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.MapType)
                    .WithMany(p => p.Map)
                    .HasForeignKey(d => d.MapTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_map_id_to_map_type_id");
            });

            modelBuilder.Entity<MapType>(entity =>
            {
                entity.ToTable("map_type", "overwatch");

                entity.Property(e => e.MapTypeId).HasColumnName("map_type_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Match>(entity =>
            {
                entity.ToTable("match", "overwatch");

                entity.Property(e => e.MatchId).HasColumnName("match_id");

                entity.Property(e => e.MapId).HasColumnName("map_id");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Victory).HasColumnName("victory");

                entity.HasOne(d => d.Map)
                    .WithMany(p => p.Match)
                    .HasForeignKey(d => d.MapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_match_id_to_map_id");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("player", "overwatch");

                entity.Property(e => e.PlayerId).HasColumnName("player_id");

                entity.Property(e => e.DpsSr).HasColumnName("dps_sr");

                entity.Property(e => e.HealSr).HasColumnName("heal_sr");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TankSr).HasColumnName("tank_sr");
            });

            modelBuilder.Entity<PlayerMatchXref>(entity =>
            {
                entity.ToTable("player_match_xref", "overwatch");

                entity.Property(e => e.PlayerMatchXrefId).HasColumnName("player_match_xref_id");

                entity.Property(e => e.MatchId).HasColumnName("match_id");

                entity.Property(e => e.PlayerId).HasColumnName("player_id");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasColumnName("role")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Match)
                    .WithMany(p => p.PlayerMatchXref)
                    .HasForeignKey(d => d.MatchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_player_match_xref_to_match");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerMatchXref)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_player_match_xref_to_player");
            });
        }
    }
}
