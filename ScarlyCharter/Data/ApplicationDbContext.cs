using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ScarlyCharter.Models;

#nullable disable

namespace ScarlyCharter.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BookedTrip> BookedTrips { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Fish> Fish { get; set; }
        public virtual DbSet<Guide> Guides { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\ProjectsV13;Database=DB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<BookedTrip>(entity =>
            {
                entity.HasKey(e => e.TripId)
                    .HasName("PK__BOOKED_T__6852735E1671D51B");

                entity.Property(e => e.TripId).ValueGeneratedNever();

                entity.Property(e => e.ClientName).IsUnicode(false);

                entity.Property(e => e.FishingStyle).IsUnicode(false);

                entity.Property(e => e.GuideName).IsUnicode(false);

                entity.Property(e => e.TargetFish).IsUnicode(false);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.ClientId).ValueGeneratedNever();

                entity.Property(e => e.ClientName).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.PaymentInfo).IsUnicode(false);

                entity.Property(e => e.Username).IsUnicode(false);
            });

            modelBuilder.Entity<Fish>(entity =>
            {
                entity.HasKey(e => e.FishName)
                    .HasName("PK__FISH__B841E62C9B0483A9");

                entity.Property(e => e.FishName).IsUnicode(false);

                entity.HasOne(d => d.LocationNavigation)
                    .WithMany(p => p.Fish)
                    .HasForeignKey(d => d.Location)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__FISH__Location__6BE40491");
            });

            modelBuilder.Entity<Guide>(entity =>
            {
                entity.HasKey(e => e.GuideName)
                    .HasName("PK__GUIDE__E10C8E3FF92A1AF6");

                entity.Property(e => e.GuideName).IsUnicode(false);

                entity.Property(e => e.FishingStyle).IsUnicode(false);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.LocationId).ValueGeneratedNever();

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Locations)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK__LOCATION__Region__69FBBC1F");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.Property(e => e.RegionId).ValueGeneratedNever();

                entity.Property(e => e.Fish).IsUnicode(false);

                entity.Property(e => e.RegionName).IsUnicode(false);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(e => e.Date)
                    .HasName("PK__SCHEDULE__77387D06FAF7B089");

                entity.Property(e => e.GuideName).IsUnicode(false);

                entity.HasOne(d => d.GuideNameNavigation)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.GuideName)
                    .HasConstraintName("FK__SCHEDULE__Guide___6AEFE058");
            });

            modelBuilder.Entity<Season>(entity =>
            {
                entity.Property(e => e.SeasonId).ValueGeneratedNever();

                entity.Property(e => e.SeasonName).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
