using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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
        public virtual DbSet<Guide> Guides { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
               optionsBuilder.UseSqlServer("Server=(localdb)\\ProjectsV13;Database=DB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<BookedTrip>(entity =>
            {
                entity.HasKey(e => e.TripId)
                    .HasName("PK__BOOKED_T__685272BE50EA8A1B");

                entity.Property(e => e.TripId).ValueGeneratedNever();

                entity.Property(e => e.FishingStyle).IsUnicode(false);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.BookedTrips)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK__BOOKED_TR__Clien__408F9238");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.BookedTrips)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("FK__BOOKED_TR__Sched__4183B671");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.ClientId).ValueGeneratedNever();

                entity.Property(e => e.ClientName).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.PaymentInfo).IsUnicode(false);

                entity.Property(e => e.Username).IsUnicode(false);
            });

            modelBuilder.Entity<Guide>(entity =>
            {
                entity.Property(e => e.GuideId).ValueGeneratedNever();

                entity.Property(e => e.FishingStyle).IsUnicode(false);

                entity.Property(e => e.GuideName).IsUnicode(false);

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Guides)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__GUIDE__Region_ID__3BCADD1B");

                entity.HasOne(d => d.Season)
                    .WithMany(p => p.Guides)
                    .HasForeignKey(d => d.SeasonId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__GUIDE__Season_ID__3CBF0154");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.Property(e => e.RegionId).ValueGeneratedNever();

                entity.Property(e => e.RegionName).IsUnicode(false);

                entity.HasOne(d => d.Season)
                    .WithMany(p => p.Regions)
                    .HasForeignKey(d => d.SeasonId)
                    .HasConstraintName("FK__REGION__Season_I__2F650636");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.Property(e => e.ScheduleId).ValueGeneratedNever();
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
