using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
namespace MovieTime.Entities
{
    public partial class MovieTimeContext : DbContext
    {
        public MovieTimeContext()
        {
        }

        public MovieTimeContext(DbContextOptions<MovieTimeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<Movie> Movie { get; set; }
        public virtual DbSet<Review> Review { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genre");

                entity.Property(e => e.GenreId).HasColumnName("genre_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("movie");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.Property(e => e.DirectorId).HasColumnName("director_id");

                entity.Property(e => e.GenreId).HasColumnName("genre_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.ReleaseYear).HasColumnName("release_year");

                entity.Property(e => e.SubGenreId).HasColumnName("sub_genre_id");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Movie)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movie_to_sub_genre");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("review");

                entity.Property(e => e.ReviewId).HasColumnName("review_id");

                entity.Property(e => e.CreateTimestamp)
                    .HasColumnName("create_timestamp")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifyTimestamp)
                    .HasColumnName("modify_timestamp")
                    .HasColumnType("datetime");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.ReviewText)
                    .IsRequired()
                    .HasColumnName("review_text");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Review)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_review_to_movie");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Review)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_review_to_user");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CreateTimestamp)
                    .HasColumnName("create_timestamp")
                    .HasColumnType("datetime");

                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(20);
            });
        }
    }
}
