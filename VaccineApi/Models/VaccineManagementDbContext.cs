using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VaccineApi.Models
{
    public partial class VaccineManagementDbContext : DbContext
    {
        public VaccineManagementDbContext()
        {
        }

        public VaccineManagementDbContext(DbContextOptions<VaccineManagementDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<DateTimeSlot> DateTimeSlots { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<UserDetail> UserDetails { get; set; } = null!;
        public virtual DbSet<VaccineDetail> VaccineDetails { get; set; } = null!;
        public virtual DbSet<VaccineDose> VaccineDoses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=VICKY_BHASKAR;Initial Catalog=VaccineManagementDb;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admin");

                entity.Property(e => e.AdminId).HasColumnName("AdminID");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<DateTimeSlot>(entity =>
            {
                entity.HasKey(e => e.DateTimings)
                    .HasName("PK__DateTime__BC397B5E211BB72A");

                entity.Property(e => e.DateTimings).HasColumnType("datetime");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.CityName)
                    .HasName("PK__Location__886159E49D0CFDCA");

                entity.ToTable("Location");

                entity.Property(e => e.CityName).HasMaxLength(50);
            });

            modelBuilder.Entity<UserDetail>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserDeta__1788CCAC3C10A88E");

                entity.HasIndex(e => e.Username, "UQ__UserDeta__536C85E41C86E952")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<VaccineDetail>(entity =>
            {
                entity.HasKey(e => e.VaccineId)
                    .HasName("PK__VaccineD__45DC68E9FD0733FF");

                entity.Property(e => e.VaccineId).HasColumnName("VaccineID");

                entity.Property(e => e.Manufacturer).HasMaxLength(100);

                entity.Property(e => e.VaccineName).HasMaxLength(100);
            });

            modelBuilder.Entity<VaccineDose>(entity =>
            {
                entity.HasKey(e => e.NumberOfDose)
                    .HasName("PK__VaccineD__B0E6411564846B7C");

                entity.ToTable("VaccineDose");

                entity.Property(e => e.NumberOfDose).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
