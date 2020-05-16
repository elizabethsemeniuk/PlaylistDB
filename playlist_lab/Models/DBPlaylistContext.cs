using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace playlist_lab
{
    public partial class DBPlaylistContext : DbContext
    {
        public DBPlaylistContext()
        {
        }

        public DBPlaylistContext(DbContextOptions<DBPlaylistContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Album> Album { get; set; }
        public virtual DbSet<Artists> Artists { get; set; }
        public virtual DbSet<ArtistsTracks> ArtistsTracks { get; set; }
        public virtual DbSet<Jenres> Jenres { get; set; }
        public virtual DbSet<Tracks> Tracks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-83R4LQR\\SQLEXPRESS; Database=DBPlaylist; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Artists>(entity =>
            {
                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasMaxLength(50);

                entity.Property(e => e.Fullname)
                    .IsRequired()
                    .HasColumnName("fullname")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ArtistsTracks>(entity =>
            {
                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.ArtistsTracks)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ARTISTS_TRACKS_ARTISTS");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.ArtistsTracks)
                    .HasForeignKey(d => d.TrackId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArtistsTracks_Tracks");
            });

            modelBuilder.Entity<Jenres>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Tracks>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.Tracks)
                    .HasForeignKey(d => d.AlbumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tracks_Album");

                entity.HasOne(d => d.Jenre)
                    .WithMany(p => p.Tracks)
                    .HasForeignKey(d => d.JenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TRACKS_DIC_JENRES");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
