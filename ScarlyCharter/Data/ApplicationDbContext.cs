using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ScarlyCharter.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext ()
        {
        }

        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base (options)
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

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer ("Server=(localdb)\\ProjectsV13;Database=DB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            base.OnModelCreating (modelBuilder);

            modelBuilder.HasAnnotation ("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<BookedTrip> (entity =>
            {
                entity.HasKey (e => e.TripId)
                    .HasName ("PK__BOOKED_T__685272BE2EA383A9");

                entity.Property (e => e.TripId).ValueGeneratedNever ();

                entity.Property (e => e.FishingStyle).IsUnicode (false);

                entity.HasOne (d => d.Client)
                    .WithMany (p => p.BookedTrips)
                    .HasForeignKey (d => d.ClientId)
                    .HasConstraintName ("FK__BOOKED_TR__Clien__4AD81681");

                entity.HasOne (d => d.Guide)
                    .WithMany (p => p.BookedTrips)
                    .HasForeignKey (d => d.GuideId)
                    .HasConstraintName ("FK__BOOKED_TR__Guide__4BCC3ABA");

                entity.HasOne (d => d.Schedule)
                    .WithMany (p => p.BookedTrips)
                    .HasForeignKey (d => d.ScheduleId)
                    .HasConstraintName ("FK__BOOKED_TR__Sched__4CC05EF3");
            });

            modelBuilder.Entity<Client> (entity =>
            {
                entity.Property (e => e.ClientId).ValueGeneratedNever ();

                entity.Property (e => e.ClientName).IsUnicode (false);

                entity.Property (e => e.Password).IsUnicode (false);

                entity.Property (e => e.PaymentInfo).IsUnicode (false);

                entity.Property (e => e.Username).IsUnicode (false);
            });

            modelBuilder.Entity<Fish> (entity =>
            {
                entity.Property (e => e.FishId).ValueGeneratedNever ();

                entity.Property (e => e.FishName).IsUnicode (false);

                entity.HasOne (d => d.Region)
                    .WithMany (p => p.Fish)
                    .HasForeignKey (d => d.RegionId)
                    .HasConstraintName ("FK__FISH__Region_ID__405A880E");

                entity.HasOne (d => d.Season)
                    .WithMany (p => p.Fish)
                    .HasForeignKey (d => d.SeasonId)
                    .OnDelete (DeleteBehavior.SetNull)
                    .HasConstraintName ("FK__FISH__Season_ID__414EAC47");
            });

            modelBuilder.Entity<Guide> (entity =>
            {
                entity.Property (e => e.GuideId).ValueGeneratedNever ();

                entity.Property (e => e.FishingStyle).IsUnicode (false);

                entity.Property (e => e.GuideName).IsUnicode (false);

                entity.HasOne (d => d.Region)
                    .WithMany (p => p.Guides)
                    .HasForeignKey (d => d.RegionId)
                    .HasConstraintName ("FK__GUIDE__Region_ID__46136164");

                entity.HasOne (d => d.Season)
                    .WithMany (p => p.Guides)
                    .HasForeignKey (d => d.SeasonId)
                    .OnDelete (DeleteBehavior.SetNull)
                    .HasConstraintName ("FK__GUIDE__Season_ID__4707859D");
            });

            modelBuilder.Entity<Location> (entity =>
            {
                entity.Property (e => e.LocationId).ValueGeneratedNever ();

                entity.Property (e => e.Type).IsUnicode (false);

                entity.HasOne (d => d.Region)
                    .WithMany (p => p.Locations)
                    .HasForeignKey (d => d.RegionId)
                    .HasConstraintName ("FK__LOCATION___Regio__3D7E1B63");
            });

            modelBuilder.Entity<Region> (entity =>
            {
                entity.Property (e => e.RegionId).ValueGeneratedNever ();

                entity.Property (e => e.RegionName).IsUnicode (false);

                entity.HasOne (d => d.Season)
                    .WithMany (p => p.Regions)
                    .HasForeignKey (d => d.SeasonId)
                    .HasConstraintName ("FK__REGION__Season_I__39AD8A7F");
            });

            modelBuilder.Entity<Schedule> (entity =>
            {
                entity.Property (e => e.ScheduleId).ValueGeneratedNever ();
            });

            modelBuilder.Entity<Season> (entity =>
            {
                entity.Property (e => e.SeasonId).ValueGeneratedNever ();

                entity.Property (e => e.SeasonName).IsUnicode (false);
            });

            OnModelCreatingPartial (modelBuilder);
        }

        partial void OnModelCreatingPartial (ModelBuilder modelBuilder);
    }
}
